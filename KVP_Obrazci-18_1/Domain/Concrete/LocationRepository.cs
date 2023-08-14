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
    public class LocationRepository : ILocationRepository
    {
        Session session;
        public LocationRepository(Session session)
        {
            this.session = session;
        }

        public Lokacija GetLocationByID(int id, Session currentSession = null)
        {
            try
            {
                XPQuery<Lokacija> location = null;

                if (currentSession != null)
                    location = currentSession.Query<Lokacija>();
                else
                    location = session.Query<Lokacija>();

                return location.Where(l => l.idLokacija == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_46, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveLocation(Lokacija model)
        {
            try
            {
                if (model.idLokacija == 0)
                {
                    model.idIDOseba = PrincipalHelper.GetUserPrincipal().ID;
                    model.ts = DateTime.Now;
                    model.Koda = GenerateCode(model.Opis);
                    model.Sort = GetCntForSort() + 1;
                }

                model.Save();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_47, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteLocation(int id, Session currentSession = null)
        {
            try
            {
                Lokacija location = GetLocationByID(id);

                if (location != null)
                    location.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_48, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteLocation(Lokacija model)
        {
            try
            {
                model.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_48, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveLocationFromBatchUpdate(List<ASPxDataUpdateValues> updateValues)
        {
            using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
            {
                Lokacija location = null;
                Type myType = typeof(Lokacija);
                List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();

                foreach (ASPxDataUpdateValues item in updateValues)
                {
                    location = new Lokacija(session);

                    foreach (DictionaryEntry obj in item.Keys)//we set table ID
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                        {
                            location = GetLocationByID((int)obj.Value, uow);
                            break;
                        }
                    }

                    foreach (DictionaryEntry obj in item.NewValues)
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                            info.SetValue(location, obj.Value);

                    }
                }
                uow.CommitChanges();
            }
        }


        public int GetCntForSort()
        {
            try
            {
                XPQuery<Lokacija> location = session.Query<Lokacija>();

                return location.Count();

            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_46, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public string GenerateCode(string title)
        {
            string generatedCode = "";
            try
            {

                XPQuery<Lokacija> location = session.Query<Lokacija>();

                if (title.Length >= 5)
                {
                    generatedCode = title.Substring(0, 5) + (GetCntForSort() + 1).ToString();
                }
                else
                    generatedCode = title + (GetCntForSort() + 1).ToString();

                generatedCode = CommonMethods.PreveriZaSumnike(generatedCode);

                int indeks = 1;
                for (; ; indeks++)
                {
                    Lokacija lok = location.Where(l => l.Koda.ToLower() == generatedCode.ToLower()).FirstOrDefault();
                    if (lok != null)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_46, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}