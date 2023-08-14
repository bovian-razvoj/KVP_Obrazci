using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class ActiveUserRepository : IActiveUserRepository
    {
        Session session;
        IEmployeeRepository employeeRepo;

        public ActiveUserRepository(Session session = null)
        {
            if (session == null)
                session = XpoHelper.GetNewSession();

            this.session = session;

            employeeRepo = new EmployeeRepository(session);
        }

        public ActiveUser GetActiveUserByUserID(int userID, Session currentSession = null)
        {
            try
            {
                XPQuery<ActiveUser> activeUser = null;

                if (currentSession != null)
                    activeUser = currentSession.Query<ActiveUser>();
                else
                    activeUser = session.Query<ActiveUser>();

                return activeUser.Where(au => au.UserID.Id == userID).OrderByDescending(au => au.LogInDate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_55, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveActiveUser(int userID)
        {
            try
            {
                ActiveUser activeUser = GetActiveUserByUserID(userID);

                if (activeUser != null)
                {
                    //če v trenutnem dnevu še ni zabeležene prijave se doda nova drugače se posodobijo vrednosti na obstoječi prijavi
                    if (activeUser.LogInDate.Date == DateTime.Today.Date)
                    {
                        activeUser.LogInDate = DateTime.Now;
                        activeUser.IsActive = true;
                        activeUser.LastRequestTS = DateTime.Now;

                        activeUser.Save();
                    }
                    else
                    {
                        CreateModel(userID).Save();
                    }
                }
                else
                {
                    CreateModel(userID).Save();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_56, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveUserLoggedInActivity(bool active, int userID)
        {
            try
            {
                ActiveUser activeUser = GetActiveUserByUserID(userID);

                if (activeUser != null)
                {
                    //če v trenutnem dnevu še ni zabeležene prijave se doda nova drugače se posodobijo vrednosti na obstoječi prijavi
                    if (activeUser.LogInDate.Date == DateTime.Today.Date)
                    {
                        activeUser.LogInDate = DateTime.Now;
                        activeUser.IsActive = active;
                        activeUser.LastRequestTS = DateTime.Now;

                        activeUser.Save();
                    }
                    else
                    {
                        CreateModel(userID, active).Save();
                    }
                }
                else
                {
                    CreateModel(userID, active).Save();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_56, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        private ActiveUser CreateModel(int userID, bool isActive = true)
        {
            ActiveUser newUser = new ActiveUser(session);
            newUser.ActiveUserID = 0;
            newUser.IsActive = true;
            newUser.LastRequestTS = DateTime.Now;
            newUser.LogInDate = DateTime.Now;
            newUser.RequestCount += 1;
            newUser.ts = DateTime.Now;
            newUser.UserID = employeeRepo.GetEmployeeByID(userID, newUser.Session);

            return newUser;
        }
    }
}