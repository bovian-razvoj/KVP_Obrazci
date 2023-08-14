using DevExpress.Web.Data;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class EmployeeRepository : IEmployeeRepository
    {
        Session session;

        IRoleRepository roleRepo = null;
        public EmployeeRepository(Session session = null)
        {
            if (session == null)
                session = XpoHelper.GetNewSession();

            this.session = session;
            roleRepo = new RoleRepository(session);
        }

        public Users GetEmployeeByID(int id, Session currentSession = null)
        {
            try
            {
                XPQuery<Users> employee = null;

                if (currentSession != null)
                    employee = currentSession.Query<Users>();
                else
                    employee = session.Query<Users>();

                return employee.Where(u => u.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_13, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveEmployee(Users model)
        {
            try
            {
                model.Save();

                return model.Id;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_14, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteEmployee(Users model)
        {
            try
            {
                model.Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_15, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteEmployee(int id)
        {
            try
            {
                GetEmployeeByID(id).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_15, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool SetDeleteFlagEmployee(int id)
        {
            try
            {
                Users usr = GetEmployeeByID(id);
                if (usr != null)
                {
                    usr.Deleted = true;
                    usr.ValidTill = DateTime.Now;
                    usr.eKVPPrijava = false;
                    usr.UpravicenDoKVP = false;
                    
                    usr.Save();
                }

                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_15, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveEmployeeFromBatchUpdate(List<ASPxDataUpdateValues> updateValues)
        {
            using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
            {
                Users employee = null;
                Type myType = typeof(Users);
                List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();

                foreach (ASPxDataUpdateValues item in updateValues)
                {
                    employee = new Users(session);

                    foreach (DictionaryEntry obj in item.Keys)//we set table ID
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                        {
                            employee = GetEmployeeByID((int)obj.Value, uow);
                            break;
                        }
                    }

                    foreach (DictionaryEntry obj in item.NewValues)
                    {
                        if (IsNewValue(obj, item.OldValues))
                        {
                            bool isColumnRoleId = obj.Key.ToString().Equals("RoleID.Naziv");
                            PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(isColumnRoleId ? "RoleID" : obj.Key.ToString())).FirstOrDefault();

                            if (info != null)
                                info.SetValue(employee, isColumnRoleId ? roleRepo.GetRoleByID(CommonMethods.ParseInt(obj.Value), uow) : obj.Value);

                            bool isColumnSecondRoleId = obj.Key.ToString().Equals("SecondRoleID.Naziv");
                            PropertyInfo infoSecondRole = myPropInfo.Where(prop => prop.Name.Equals(isColumnSecondRoleId ? "SecondRoleID" : obj.Key.ToString())).FirstOrDefault();

                            if (infoSecondRole != null)
                                infoSecondRole.SetValue(employee, isColumnSecondRoleId ? roleRepo.GetRoleByID(CommonMethods.ParseInt(obj.Value), uow) : obj.Value);
                        }
                    }
                }
                uow.CommitChanges();
            }
        }

        private bool IsNewValue(DictionaryEntry newValue, OrderedDictionary oldValuesDictionary)
        {
            foreach (DictionaryEntry item in oldValuesDictionary)
            {
                if (newValue.Key.Equals(item.Key))
                {
                    if (item.Value != null)
                    {
                        if (newValue.Value.Equals(item.Value)) return false;
                    }
                }
            }

            return true;
        }

        public Vloga GetRoleByID(int id, Session currentSession = null)
        {
            try
            {
                XPQuery<Vloga> role = null;

                if (currentSession != null)
                    role = currentSession.Query<Vloga>();
                else
                    role = session.Query<Vloga>();

                return role.Where(r => r.VlogaID == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_13, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Departments GetDepartmentByID(int id, Session currentSession = null)
        {
            try
            {
                XPQuery<Departments> department = null;

                if (currentSession != null)
                    department = currentSession.Query<Departments>();
                else
                    department = session.Query<Departments>();

                return department.Where(d => d.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_13, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool IsEmployeeCEO(int id)
        {
            int ceoID = CommonMethods.ParseInt(ConfigurationManager.AppSettings["EmployeeCEO_ID"].ToString());

            return ceoID == id;
        }

        public bool IsEmployeeCEO(Users employee)
        {
            if (employee == null) return false;
            int ceoID = CommonMethods.ParseInt(ConfigurationManager.AppSettings["EmployeeCEO_ID"].ToString());

            return ceoID == employee.Id;
        }

        public int GetDepartmentHeadID(Departments department = null)
        {
            int departmentHeadID = 0;

            if (department == null)
                department = GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID).DepartmentId;

            if (department != null)
            {
                departmentHeadID = department.DepartmentHeadId;

                if (PrincipalHelper.GetUserPrincipal().ID == department.DepartmentHeadId && !IsEmployeeCEO(PrincipalHelper.GetUserPrincipal().ID))
                {
                    departmentHeadID = GetDepartmentByID(department.ParentId).DepartmentHeadId;
                }
            }

            if (departmentHeadID <= 0) throw new Exception("&%messageType=1&% There is no department head ID fo user " + PrincipalHelper.GetUserPrincipal().ID + ", " + PrincipalHelper.GetUserPrincipal().firstName + " " + PrincipalHelper.GetUserPrincipal().lastName);

            return departmentHeadID;
        }

        public Departments GetParentDepartment(Users currentUser)
        {
            Departments department = currentUser.DepartmentId;
            if (department == null)
                department = GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID).DepartmentId;

            if (department != null)
            {
                if (currentUser.Id == department.DepartmentHeadId && !IsEmployeeCEO(PrincipalHelper.GetUserPrincipal().ID))
                {
                    department = GetDepartmentByID(department.ParentId);
                }
                else if (currentUser.Id == department.DepartmentHeadDeputyId && !IsEmployeeCEO(PrincipalHelper.GetUserPrincipal().ID))
                {
                    department = GetDepartmentByID(department.ParentId);
                }
            }

            return department;
        }

        public int GetNotEmployeedAnymoreEmployeeesCount()
        {
            try
            {
                XPQuery<Users> employees = session.Query<Users>();

                return employees.Where(e => e.NotEmployedAnymore == 1).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_13, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void UpdateEmployees(List<int> employees, List<UpdateColumnWithValueModel> updateColumns)
        {
            using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
            {
                Users employee = null;
                Type myType = typeof(Users);
                List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();

                foreach (var item in employees)
                {
                    employee = new Users(session);


                    employee = GetEmployeeByID(item, uow);

                    foreach (var column in updateColumns)
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(column.ColumnName)).FirstOrDefault();

                        if (info != null)
                            info.SetValue(employee, column.ColumnValue);
                    }
                }
                uow.CommitChanges();
            }
        }

        public int GetNewEmployeesCount()
        {
            try
            {
                XPQuery<Users> employees = session.Query<Users>();

                return employees.Where(e => e.NewEmployee == 1).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_13, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetEmployeesNameChangedCount()
        {
            try
            {
                XPQuery<Users> employees = session.Query<Users>();

                return employees.Where(e => e.NameChanged == 1 && e.SinhronizationNo == 0).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_13, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetEmployeesDuplicatedCount()
        {
            try
            {
                XPQuery<Users> employees = session.Query<Users>();

                return employees.Where(e => e.IsDuplicated == 1).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_13, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public string SyncEmployee(int SourceEmployeeID, int DestinationEmployeeID)
        {
            try
            {
                // sync open KVP documents 


            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_58, error, CommonMethods.GetCurrentMethodName()));
            }

            return "";
        }
    }
}