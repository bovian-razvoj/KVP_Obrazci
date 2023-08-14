using DevExpress.Web.Data;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class KVPTypeRepository : IKVPTypeRepository
    {
        Session session;
        public KVPTypeRepository(Session session)
        {
            this.session = session;
        }

        public Tip GetKVPTypeByID(int id, Session currentSession = null)
        {
            try
            {
                XPQuery<Tip> type = null;

                if (currentSession != null)
                    type = currentSession.Query<Tip>();
                else
                    type = session.Query<Tip>();

                return type.Where(t => t.idTip == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_37, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveKVPType(Tip model)
        {
            try
            {
                if (model.idTip == 0)
                {
                    model.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                    model.ts = DateTime.Now;
                }

                model.Save();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_38, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteKVPType(int id, Session currentSession = null)
        {
            try
            {
                Tip type = GetKVPTypeByID(id);

                if (type != null)
                    type.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_39, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteKVPType(Tip model)
        {
            try
            {
                model.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_39, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveKVPTypeFromBatchUpdate(List<ASPxDataUpdateValues> updateValues)
        {
            using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
            {
                Tip kvpType = null;
                Type myType = typeof(Tip);
                List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();

                foreach (ASPxDataUpdateValues item in updateValues)
                {
                    kvpType = new Tip(session);

                    foreach (DictionaryEntry obj in item.Keys)//we set table ID
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                        {
                            kvpType = GetKVPTypeByID((int)obj.Value, uow);
                            break;
                        }
                    }

                    foreach (DictionaryEntry obj in item.NewValues)
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                            info.SetValue(kvpType, obj.Value);

                    }
                }
                uow.CommitChanges();
            }
        }
    }
}