using DevExpress.Xpo;
using KVP_Obrazci.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IKodeksToEKVPRepository
    {
        List<KodeksUsersModel> GetKodeksUsers();
        void UpdateUser(KodeksUsersModel newValues, params string[] columnsToUpdate);
        void UpdateUser(KodeksUsersModel newValues);
        void SaveNewUser(KodeksUsersModel newUser);
        List<KodeksDepartmentsModel> GetKodeksDepartments();
        void UpdateDepartment(KodeksDepartmentsModel newValues, params string[] columnsToUpdate);
        void UpdateDepartment(KodeksDepartmentsModel newValues);
        void SaveNewDepartment(KodeksDepartmentsModel newDepartment);

        void MergeKodeks_eKVP(Session currentSession = null);
    }
}
