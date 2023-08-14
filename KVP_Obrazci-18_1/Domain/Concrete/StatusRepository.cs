using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class StatusRepository : IStatusRepository
    {
        Session session;
        public StatusRepository(Session session)
        {
            this.session = session;
        }

        public Status GetStatusByID(int id, Session currentSession = null)
        {
            try
            {
                XPQuery<Status> status = null;

                if (currentSession != null)
                    status = currentSession.Query<Status>();
                else
                    status = session.Query<Status>();

                return status.Where(s => s.idStatus == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_04, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Status GetStatusByCode(string code, Session currentSession = null)
        {
            try
            {
                XPQuery<Status> status = null;

                if (currentSession != null)
                    status = currentSession.Query<Status>();
                else
                    status = session.Query<Status>();

                return status.Where(s => s.Koda == code).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_04, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveStatus(Status model)
        {
            try
            {
                model.Save();

                return model.idStatus;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_05, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteStatus(int statusId)
        {
            try
            {
                session.GetObjectByKey<Status>(statusId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_06, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetKVPRedCardStatusIDByCode(string code)
        {
            try
            {
                XPQuery<Status> status = session.Query<Status>();
                return status.Where(s => s.Koda == code).FirstOrDefault().idStatus;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}