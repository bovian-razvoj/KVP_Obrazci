using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using KVP_Obrazci.Helpers;
using DevExpress.Xpo;

namespace KVPMessageProcessorService
{
    public partial class KVPMessageProcessorService : ServiceBase
    {
        private IEmployeeRepository employee;
        private Timer timerSchedular;
        private IMessageProcessorRepository messageRepo;
        private IKodeksToEKVPRepository kodeksRepo;

        Session session;

        public KVPMessageProcessorService()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                this.ScheduleService();
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message);
            }
        }

        protected override void OnStop()
        {
        }

        private void ScheduleService()
        {
            try
            {
                timerSchedular = new Timer(new TimerCallback(TimerScheduleCallback));
                //Set the Default Time.
                DateTime scheduledTime = DateTime.MinValue;
                CommonMethods.LogThis("ScheduleService");
                string scheduleMode = ConfigurationManager.AppSettings["ScheduleMode"].ToString();
                CommonMethods.LogThis(scheduleMode);
                if (scheduleMode == "Dnevno")
                {
                    scheduledTime = DateTime.Parse(ConfigurationManager.AppSettings["ScheduledTime"]);
                    CommonMethods.LogThis("scheduledTime");

                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next day.
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }
                else if (scheduleMode == "Interval")
                {
                    int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMin"]);
                    scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);

                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next Interval.
                        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                    }
                }

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                timerSchedular.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                //   DataTypesHelper.LogThis(ex.Message + ex.StackTrace);
            }
        }

        private void TimerScheduleCallback(object e)
        {
            try
            {

                CommonMethods.LogThis("Start service");

                if (session == null)
                    session = XpoHelper.GetNewSession();

                employee = new EmployeeRepository(session);
                messageRepo = new MessageProcessorRepository(session);
                //messageRepo.DisposeSession();
                //messageRepo = new MessageProcessorRepository(session);
                kodeksRepo = new KodeksToEKVPRepository(session);
                CommonMethods.LogThis("Start ProceesKVPsToSendEmployeeStatistic");
                messageRepo.ProceesKVPsToSendEmployeeStatistic(session);
                CommonMethods.LogThis("End ProceesKVPsToSendEmployeeStatistic");
                //messageRepo.DisposeSession();

                string sMergKodeks = ConfigurationManager.AppSettings["MergeKodeksKVP"].ToString();

                if (sMergKodeks == "1")
                {
                    CommonMethods.LogThis("Start MergeKodeks_eKVP");
                    kodeksRepo.MergeKodeks_eKVP();
                    CommonMethods.LogThis("End MergeKodeks_eKVP");
                }
            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                CommonMethods.getError(ex, ref error);
                errorToThrow = CommonMethods.ConcatenateErrorIN_DB("", error, CommonMethods.GetCurrentMethodName());
                CommonMethods.LogThis(errorToThrow);
            }

            this.ScheduleService();
        }
    }
}
