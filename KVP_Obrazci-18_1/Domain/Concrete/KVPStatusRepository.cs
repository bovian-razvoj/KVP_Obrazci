using DevExpress.Data.Filtering;
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
    public class KVPStatusRepository : IKVPStatusRepository
    {
        Session session;
        IEmployeeRepository employeeRepo;

        public KVPStatusRepository(Session session)
        {
            this.session = session;
            employeeRepo = new EmployeeRepository(session);
        }

        public KVP_Status GetLatestKVPStatus(int kvpDocID)
        {
            try
            {
                XPQuery<KVP_Status> kvpStatus = session.Query<KVP_Status>();

                return kvpStatus.Where(s => s.idKVPDocument.idKVPDocument == kvpDocID).OrderByDescending(s => s.ts).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public KVP_Status GetLatestKVPStatus(KVPDocument doc)
        {
            try
            {
                return doc.KVP_Statuss.OrderByDescending(s => s.idKVP_Status).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<KVP_Status> GetKVPStatusesBYDocID(int kvpDocID)
        {
            try
            {
                XPQuery<KVP_Status> kvpStatuses = session.Query<KVP_Status>();
                
                return kvpStatuses.Where(s => s.idKVPDocument.idKVPDocument == kvpDocID).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveKVPStatus(KVP_Status model)
        {
            try
            {
                if (model.idKVP_Status == 0)
                {
                    model.IDPrijava = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, model.Session);
                    model.ts = DateTime.Now; 
                }

                model.Save();

                return model.idKVP_Status;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_05, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteKVPStatus(int statusID)
        {
            try
            {
                session.GetObjectByKey<KVP_Status>(statusID).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_06, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public KVP_Status GetKVPRedCardStatus(int kvpRedCardDocID)
        {
            try
            {
                XPQuery<KVP_Status> kvpStatus = session.Query<KVP_Status>();
                return kvpStatus.Where(s => s.idKVPDocument.idKVPDocument == kvpRedCardDocID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public KVP_Status GetKVPRedCardStatus(KVPDocument doc)
        {
            try
            {
                return doc.KVP_Statuss.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_07, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool HasKVPDocumentKVPStatus(int kvpDocID, string kodaStatus)
        {
            try
            {
                XPQuery<KVP_Status> kvpStatus = session.Query<KVP_Status>();
                return kvpStatus.Where(s => s.idKVPDocument.idKVPDocument == kvpDocID && s.idStatus.Koda == kodaStatus).FirstOrDefault() != null;
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