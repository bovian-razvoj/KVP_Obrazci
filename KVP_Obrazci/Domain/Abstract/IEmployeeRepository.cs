using DevExpress.Web.Data;
using DevExpress.Xpo;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IEmployeeRepository
    {
        Users GetEmployeeByID(int id, Session currentSession = null);
        int SaveEmployee(Users model);
        bool DeleteEmployee(Users model);
        bool DeleteEmployee(int id);
        bool SetDeleteFlagEmployee(int id);
        void SaveEmployeeFromBatchUpdate(List<ASPxDataUpdateValues> updateValues);
        Vloga GetRoleByID(int id, Session currentSession = null);
        Departments GetDepartmentByID(int id, Session currentSession = null);
        bool IsEmployeeCEO(int id);
        bool IsEmployeeCEO(Users employee);
        int GetDepartmentHeadID(Departments department = null);
        Departments GetParentDepartment(Users currentUser);
        int GetNotEmployeedAnymoreEmployeeesCount();
        void UpdateEmployees(List<int> employees, List<UpdateColumnWithValueModel> updateColumns); 
        int GetNewEmployeesCount();
        int GetEmployeesNameChangedCount();
        int GetEmployeesDuplicatedCount();
        
    }
}
