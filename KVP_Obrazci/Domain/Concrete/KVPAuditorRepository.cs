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
    public class KVPAuditorRepository : IKVPAuditorRepository
    {
        Session session;

        public KVPAuditorRepository(Session session)
        {
            this.session = session;
        }

        public KVPPresoje GetKVPAuditorByID(int kvpAuditorID, Session currentSession = null)
        {
            try
            {
                XPQuery<KVPPresoje> auditor = null;

                if (currentSession != null)
                    auditor = currentSession.Query<KVPPresoje>();
                else
                    auditor = session.Query<KVPPresoje>();

                return auditor.Where(kvpA => kvpA.idKVPPresoje == kvpAuditorID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_19, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveKVPAuditor(KVPPresoje model)
        {
            try
            {
                if (model.idKVPPresoje == 0)
                {
                    foreach (var item in GetKVPAuditorsByKVPId(model.idKVPDocument.idKVPDocument))
                    {
                        item.JeZadnjiPresojevalec = false;
                        item.Save();
                    }

                    model.JeZadnjiPresojevalec = true;
                    model.ts = DateTime.Now;
                    model.tsIDOseba = PrincipalHelper.GetUserPrincipal().ID;
                }
            
                model.Save();

                return model.idKVPPresoje;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_20, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteKVPAuditor(KVPPresoje model)
        {
            try
            {
                model.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_21, error, CommonMethods.GetCurrentMethodName()));
            }
        }
        public void DeleteKVPAuditor(int id)
        {
            try
            {
                GetKVPAuditorByID(id).Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_21, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public int GetKVPAuditorsCountByKVPId(int kvpDocID)
        {
            try
            {
                XPQuery<KVPPresoje> auditor = session.Query<KVPPresoje>();

                return auditor.Where(kvpA => kvpA.idKVPDocument.idKVPDocument == kvpDocID).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_19, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<KVPPresoje> GetKVPAuditorsByKVPId(int kvpDocID)
        {
            try
            {
                XPQuery<KVPPresoje> auditor = session.Query<KVPPresoje>();

                return auditor.Where(kvpA => kvpA.idKVPDocument.idKVPDocument == kvpDocID).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_19, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Users GetLatestAuditorOnKVP(int kvpDocID)
        {
            try
            {
                XPQuery<KVPPresoje> auditor = session.Query<KVPPresoje>();
                return auditor.Where(p => p.idKVPDocument.idKVPDocument == kvpDocID).OrderByDescending(kvpP => kvpP.ts).Select(presoje => presoje.Presojevalec).FirstOrDefault();
            }
            catch(Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_19, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}