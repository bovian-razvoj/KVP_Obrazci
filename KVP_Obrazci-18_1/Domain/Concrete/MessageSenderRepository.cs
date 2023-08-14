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
    public class MessageSenderRepository : IMessageSenderRepository
    {
        Session session;

        ICompanySettingsRepository companySettingsRepo;
        public MessageSenderRepository(Session session = null)
        {
            if (session == null)
                session = XpoHelper.GetNewSession();

            this.session = session;

            companySettingsRepo = new CompanySettingsRepository(session);
        }

        public void UpdateFailedMessges()
        {
            try
            {                
                using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
                {
                    XPQuery<SystemEmailMessage> emails = uow.Query<SystemEmailMessage>();
                    List<SystemEmailMessage> errorList = emails.Where(e => e.Status == (int)Enums.SystemServiceSatus.Error).ToList();

                    foreach (var item in errorList)
                    {
                        item.Status = (int)Enums.SystemServiceSatus.UnProcessed;
                    }

                    uow.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_31, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<SystemEmailMessage> GetUnprocessedEmails()
        {
            try
            {
                XPQuery<SystemEmailMessage> emails = session.Query<SystemEmailMessage>();

                if (companySettingsRepo.IsEmailSendingEnabled())
                    return emails.Where(e => e.Status == (int)Enums.SystemServiceSatus.UnProcessed).ToList();
                else
                    return new List<SystemEmailMessage>();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_31, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveEmail(SystemEmailMessage model)
        {
            try
            {
                if (model.SystemEmailMessageID != 0)
                {
                    model.Status = (int)Enums.SystemServiceSatus.Processed;
                    model.Save();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_32, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}