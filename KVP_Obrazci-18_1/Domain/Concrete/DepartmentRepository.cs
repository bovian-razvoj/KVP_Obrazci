using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class DepartmentRepository : IDepartmentRepository
    {
        Session session;
        IEmployeeRepository employeeRepo;

        public DepartmentRepository(Session session)
        {
            this.session = session;
            employeeRepo = new EmployeeRepository(session);
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_43, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveDepartment(Departments model)
        {
             try
            {
                model.Save();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_44, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public Departments GetDepartmentByName(string sName, Session currentSession = null)
        {
            try
            {
                XPQuery<Departments> department = null;


                if (currentSession != null)
                    department = currentSession.Query<Departments>();
                else
                    department = session.Query<Departments>();

                return department.Where(d => d.Name == sName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_43, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<DepartmentModel> GetDepartmentsDataSource()
        {
            try
            {
                XPQuery<Departments> department = session.Query<Departments>();
                List<DepartmentModel> list = new List<DepartmentModel>();
                List<Departments> listOfDepartments = department.Where(d=>d.Id == d.Id).ToList();
                foreach (var item in listOfDepartments)
                {
                    DepartmentModel model = new DepartmentModel();
                    Users head = employeeRepo.GetEmployeeByID(item.DepartmentHeadId);
                    Users deputyHead = employeeRepo.GetEmployeeByID(item.DepartmentHeadDeputyId);
                    Departments headDepartment = department.Where(d=>d.ParentId == item.ParentId).FirstOrDefault();

                    model.Id = item.Id;
                    model.Name = item.Name;
                    model.DepartmentHeadId = item.DepartmentHeadId;
                    model.DepartmentHeadName = head != null ? head.Firstname + " " + head.Lastname : "";
                    model.DepartmentHeadDeputyId = item.DepartmentHeadDeputyId;
                    model.DepartmentHeadDeputyName = deputyHead != null ? deputyHead.Firstname + " " + deputyHead.Lastname : "";
                    model.ParentId = item.ParentId;
                    model.DepartmentSupName = headDepartment != null ? headDepartment.Name : "";
                    
                    list.Add(model);
                }

                return list;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_43, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}