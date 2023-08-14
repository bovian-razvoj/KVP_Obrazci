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
    public class LineRepository : ILineRepository
    {
         Session session;
         public LineRepository(Session session)
        {
            this.session = session;
        }

        public Linija GetLineByID(int id, Session currentSession = null)
        {
            try
            {
                XPQuery<Linija> line = null;

                if (currentSession != null)
                    line = currentSession.Query<Linija>();
                else
                    line = session.Query<Linija>();

                return line.Where(l => l.idLinija == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_52, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveLine(Linija model)
        {
            try
            {
                if (model.idLinija == 0)
                {
                    model.idIDOseba = PrincipalHelper.GetUserPrincipal().ID;
                    model.ts = DateTime.Now;
                    model.Koda = GenerateCode(model.Opis);
                    model.Sort = GetCountForSort() + 1;
                }

                model.Save();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_53, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteLine(int id, Session currentSession = null)
        {
            try
            {
                Linija line = GetLineByID(id);

                if (line != null)
                    line.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_54, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteLine(Linija model)
        {
            try
            {
                model.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_54, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveLineFromBatchUpdate(List<ASPxDataUpdateValues> updateValues)
        {
            using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
            {
                Linija line = null;
                Type myType = typeof(Linija);
                List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();

                foreach (ASPxDataUpdateValues item in updateValues)
                {
                    line = new Linija(session);

                    foreach (DictionaryEntry obj in item.Keys)//we set table ID
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                        {
                            line = GetLineByID((int)obj.Value, uow);
                            break;
                        }
                    }

                    foreach (DictionaryEntry obj in item.NewValues)
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                            info.SetValue(line, obj.Value);

                    }
                }
                uow.CommitChanges();
            }
        }

        public int GetCountForSort()
        {
            try
            {
                XPQuery<Linija> line = session.Query<Linija>();

                return line.Count();

            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_52, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public string GenerateCode(string title)
        {
            string generatedCode = "";
            try
            {

                XPQuery<Linija> line = session.Query<Linija>();

                if (title.Length >= 5)
                {
                    generatedCode = title.Substring(0, 5) + (GetCountForSort() + 1).ToString();
                }
                else
                    generatedCode = title + (GetCountForSort() + 1).ToString();

                generatedCode = CommonMethods.PreveriZaSumnike(generatedCode);

                int indeks = 1;
                for (; ; indeks++)
                {
                    Linija lin = line.Where(l => l.Koda.ToLower() == generatedCode.ToLower()).FirstOrDefault();
                    if (lin != null)
                    {
                        generatedCode += "_" + indeks.ToString();
                    }
                    else
                        break;
                }

                return generatedCode;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_52, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}