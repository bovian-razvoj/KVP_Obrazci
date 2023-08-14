using DevExpress.Web.Data;
using DevExpress.Xpo;
using KVP_Obrazci.Domain.KVPOdelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IMachineRepository
    {
        Stroj GetMachineByID(int id, Session currentSession = null);
        void SaveMachine(Stroj model);
        void DeleteMachine(int id, Session currentSession = null);
        void DeleteMachine(Stroj model);
        void SaveMachineFromBatchUpdate(List<ASPxDataUpdateValues> updateValues);
        int GetCountForSort();
        string GenerateCode(string title);
        void UpdateMachine(List<int> MachineIDs, bool isActive);
    }
}
