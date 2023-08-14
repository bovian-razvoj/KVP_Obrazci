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
    public interface IDepartmentRepository
    {
        Departments GetDepartmentByID(int id, Session currentSession = null);
        Departments GetDepartmentByName(string  sName, Session currentSession = null);

        void SaveDepartment(Departments model);
        List<DepartmentModel> GetDepartmentsDataSource();
    }
}
