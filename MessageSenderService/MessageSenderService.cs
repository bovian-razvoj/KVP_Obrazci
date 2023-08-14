using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageSenderService
{
    public partial class MessageSenderService : ServiceBase
    {
        private Timer timerSchedular;
        private bool isSending = false;
        private IMessageSenderRepository messageSenderRepo;

        public MessageSenderService()
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

                string scheduleMode = ConfigurationManager.AppSettings["ScheduleMode"].ToString();

                if (scheduleMode == "Dnevno")
                {
                    scheduledTime = DateTime.Parse(ConfigurationManager.AppSettings["ScheduledTime"]);
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
                CommonMethods.LogThis(ex.Message + ex.StackTrace);
            }
        }

        private void TimerScheduleCallback(object e)
        {
            if (isSending)
                return;

            isSending = true;

            try
            {
                messageSenderRepo = new MessageSenderRepository();
                SmtpClient client = new SmtpClient();
                client.Host = ConfigurationManager.AppSettings["SmtpHost"];
                CommonMethods.LogThis("1");
                CommonMethods.LogThis("SmtpHost :" + client.Host.ToString());
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);//Port 465 (SSL required)                
                CommonMethods.LogThis("2");
                CommonMethods.LogThis("SmtpPort :" + client.Port.ToString());
                client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
                CommonMethods.LogThis("3");
                CommonMethods.LogThis("SmtpEnableSsl :" + client.EnableSsl.ToString());
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
                client.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpTimeout"]);
                CommonMethods.LogThis("4");
                CommonMethods.LogThis("Credentials : " + client.Credentials.ToString());
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["HasCredentials"]))
                    client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
                else
                    client.UseDefaultCredentials = true;
                CommonMethods.LogThis("5");
                CommonMethods.LogThis("HasCredentials : " + ConfigurationManager.AppSettings["HasCredentials"].ToString());
                messageSenderRepo.UpdateFailedMessges();
                CommonMethods.LogThis("6");
                List<SystemEmailMessage> emailList = messageSenderRepo.GetUnprocessedEmails().Take(50).ToList();
                CommonMethods.LogThis("List of Unproccessed mail's. Count: " + emailList.Count.ToString());
                foreach (var item in emailList)
                {
                    CommonMethods.LogThis(client.ToString());
                    SendMessages(client, item);
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                CommonMethods.LogThis("*****ReflectionTypeLoadException!  - ***** " + errorMessage);
                //Display or log the error based on your application.
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message);
            }
            finally
            {
                isSending = false;
            }

            this.ScheduleService();
        }

        private void SendMessages(SmtpClient client, SystemEmailMessage item = null)
        {
            bool isOTPitem = false;

            try
            {
                if (item != null)
                    isOTPitem = true;
                CommonMethods.LogThis("7");
                if (String.IsNullOrEmpty(isOTPitem ? item.EmailTo : item.EmailTo))
                {
                    if (isOTPitem)
                        item.Status = (int)Enums.SystemEmailMessageStatus.Processed;
                    else
                        item.Status = (int)Enums.SystemEmailMessageStatus.Processed;

                    CommonMethods.LogThis("Couldn't send email! Email to is empty");
                }
                else
                {
                    CommonMethods.LogThis("8");
                    string sender = "";
                    string emailTitle = "";

                    sender = ConfigurationManager.AppSettings["SenderKVP"].ToString();
                    emailTitle = ConfigurationManager.AppSettings["EmailTitle"].ToString();
                    CommonMethods.LogThis("9");
                    MailMessage message = new MailMessage();
                    message.To.Add(new MailAddress(item.EmailTo));                   
                    message.Sender = new MailAddress(sender);
                    message.From = new MailAddress(sender, emailTitle);
                    message.Subject = item.EmailSubject;
                    message.IsBodyHtml = true;
                    message.Body = item.EmailBody;
                    message.BodyEncoding = Encoding.UTF8;
                    CommonMethods.LogThis("item.EmailTo :" + item.EmailTo.ToString());
                    CommonMethods.LogThis("sender : " + message.Sender.ToString());
                    CommonMethods.LogThis("message.From : " + message.From.ToString());
                    CommonMethods.LogThis("message.Body : " + message.Body.ToString());                    
                    item.Status = (int)Enums.SystemEmailMessageStatus.Processed;
                    CommonMethods.LogThis("11");
                    CommonMethods.LogThis(client.ToString());
                    CommonMethods.LogThis(message.ToString());
                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception ex2)
                    {
                        CommonMethods.LogThis("EX2 : " + ex2.ToString() + "Inner :" + ex2.InnerException.ToString());
                    }
                    
                    CommonMethods.LogThis("12");
                }


                messageSenderRepo.SaveEmail(item);
                CommonMethods.LogThis("13");
            }
            catch (SmtpFailedRecipientsException ex)
            {

                item.Status = (int)Enums.SystemEmailMessageStatus.RecipientError;

                CommonMethods.LogThis("Couldn't send the email to receipient: " + item.EmailTo + "\n" + ex.Message + "\n" + ex.InnerException != null ? ex.InnerException.ToString() : "");


                messageSenderRepo.SaveEmail(item);
            }
            catch (SmtpException ex)
            {
                if (ex.Message.Contains("Mailbox unavailable"))
                {

                    item.Status = (int)Enums.SystemEmailMessageStatus.RecipientError;

                    CommonMethods.LogThis("Could not send the email to receipient: " + item.EmailTo + "\n" + ex.InnerException != null ? ex.InnerException.ToString() : "");


                    messageSenderRepo.SaveEmail(item);
                }
                else
                {

                    item.Status = (int)Enums.SystemEmailMessageStatus.RecipientError;

                    CommonMethods.LogThis("SmtpException: " + ex.Message + "\n" + ex.InnerException != null ? ex.InnerException.ToString() : "");


                    messageSenderRepo.SaveEmail(item);
                }
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis("Exception:");
                CommonMethods.LogThis("LOG1: " + ex.Message);                

                item.Status = (int)Enums.SystemEmailMessageStatus.Error;               
                messageSenderRepo.SaveEmail(item);
                CommonMethods.LogThis("LOG1: " + ex.Message + "\n" + ex.InnerException.ToString());
            }
        }

        //private void TimerScheduleCallback(object e)
        //{
        //    if (isSending)
        //        return;

        //    isSending = true;

        //    try
        //    {
        //        messageSenderRepo = new MessageSenderRepository();
        //        SmtpClient client = new SmtpClient();
        //        client.Host = ConfigurationManager.AppSettings["SmtpHost"];
        //        client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);//Port 465 (SSL required)
        //        client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
        //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
        //        client.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpTimeout"]);

        //        messageSenderRepo.UpdateFailedMessges();

        //        MailMessage message;

        //        List<SystemEmailMessage> emailList = messageSenderRepo.GetUnprocessedEmails().Take(50).ToList();

        //        CommonMethods.LogThis("List of Unproccessed mail's. Count: " + emailList.Count.ToString());
        //        foreach (var item in emailList)
        //        {
        //            try
        //            {
        //                if (String.IsNullOrEmpty(item.EmailTo))
        //                {
        //                    item.Status = (int)Enums.SystemServiceSatus.Processed;
        //                    CommonMethods.LogThis("Couldn't send email! Email to is empty");
        //                }
        //                else
        //                {
        //                    message = new MailMessage();
        //                    message.To.Add(new MailAddress(item.EmailTo));

        //                    message.Sender = new MailAddress(ConfigurationManager.AppSettings["Username"]);
        //                    message.From = new MailAddress(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["EmailTitle"]);
        //                    message.Subject = item.EmailSubject;
        //                    message.IsBodyHtml = true;
        //                    message.Body = item.EmailBody;
        //                    message.BodyEncoding = Encoding.UTF8;

        //                    item.Status = (int)Enums.SystemServiceSatus.Processed;
        //                    client.Send(message);
        //                }

        //                messageSenderRepo.SaveEmail(item);
        //            }
        //            catch (SmtpFailedRecipientsException ex)
        //            {
        //                item.Status = (int)Enums.SystemServiceSatus.RecipientError;
        //                CommonMethods.LogThis("Couldn't send the email to receipient: " + item.EmailTo + "\n" + ex.Message + "\n" + ex.InnerException);
        //                messageSenderRepo.SaveEmail(item);
        //            }
        //            catch (SmtpException ex)
        //            {
        //                if (ex.Message.Contains("Mailbox unavailable"))
        //                {
        //                    item.Status = (int)Enums.SystemServiceSatus.RecipientError;
        //                    CommonMethods.LogThis("Could not send the email to receipient: " + item.EmailTo + "\n" + ex.InnerException);
        //                    messageSenderRepo.SaveEmail(item);
        //                }
        //                else
        //                {
        //                    item.Status = (int)Enums.SystemServiceSatus.RecipientError;
        //                    CommonMethods.LogThis("SmtpException: " + ex.Message + "\n" + ex.InnerException);
        //                    messageSenderRepo.SaveEmail(item);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                item.Status = (int)Enums.SystemServiceSatus.Error;
        //                messageSenderRepo.SaveEmail(item);
        //                CommonMethods.LogThis("LOG1: " + ex.Message + "\n" + ex.InnerException);
        //            }
        //        }                
        //    }
        //    catch (ReflectionTypeLoadException ex)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        foreach (Exception exSub in ex.LoaderExceptions)
        //        {
        //            sb.AppendLine(exSub.Message);
        //            FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
        //            if (exFileNotFound != null)
        //            {
        //                if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
        //                {
        //                    sb.AppendLine("Fusion Log:");
        //                    sb.AppendLine(exFileNotFound.FusionLog);
        //                }
        //            }
        //            sb.AppendLine();
        //        }
        //        string errorMessage = sb.ToString();
        //        CommonMethods.LogThis("*****ReflectionTypeLoadException!  - ***** " + errorMessage);
        //        //Display or log the error based on your application.
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonMethods.LogThis(ex.Message);
        //    }
        //    finally
        //    {
        //        isSending = false;
        //    }

        //    this.ScheduleService();
        //}
    }
}
