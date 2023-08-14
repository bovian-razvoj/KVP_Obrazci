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
    public class MachineRepository : IMachineRepository
    {
        Session session;
        public MachineRepository(Session session)
        {
            this.session = session;
        }

        public Stroj GetMachineByID(int id, Session currentSession = null)
        {
            try
            {
                XPQuery<Stroj> machine = null;

                if (currentSession != null)
                    machine = currentSession.Query<Stroj>();
                else
                    machine = session.Query<Stroj>();

                return machine.Where(m => m.idStroj == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_49, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public void SaveMachine(Stroj model)
        {
            try
            {
                if (model.idStroj == 0)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_50, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteMachine(int id, Session currentSession = null)
        {
            try
            {
                Stroj machine = GetMachineByID(id);

                if (machine != null)
                    machine.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_51, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteMachine(Stroj model)
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

        public void SaveMachineFromBatchUpdate(List<ASPxDataUpdateValues> updateValues)
        {
            using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
            {
                Stroj machine = null;
                Type myType = typeof(Stroj);
                List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();

                foreach (ASPxDataUpdateValues item in updateValues)
                {
                    machine = new Stroj(session);

                    foreach (DictionaryEntry obj in item.Keys)//we set table ID
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                        {
                            machine = GetMachineByID((int)obj.Value, uow);
                            break;
                        }
                    }

                    foreach (DictionaryEntry obj in item.NewValues)
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                            info.SetValue(machine, obj.Value);

                    }
                }
                uow.CommitChanges();
            }
        }

        public int GetCountForSort()
        {
            try
            {
                XPQuery<Stroj> machine = session.Query<Stroj>();

                return machine.Count();

            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_49, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void UpdateMachine(List<int> MachineIDs, bool isActive)
        {
            try
            {
                foreach (var iID in MachineIDs)
                {
                    Stroj model = GetMachineByID(iID);
                    if (model != null)
                    {
                        model.Active = isActive;
                        model.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_44, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public string GenerateCode(string title)
        {
            string generatedCode = "";
            try
            {

                XPQuery<Stroj> machine = session.Query<Stroj>();

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
                    Stroj mach = machine.Where(s => s.Koda.ToLower() == generatedCode.ToLower()).FirstOrDefault();
                    if (mach != null)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_49, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}