using DevExpress.Xpo;
using KVP_Obrazci.Domain.KVPOdelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IStatusRepository
    {
        Status GetStatusByID(int id, Session currentSession = null);
        Status GetStatusByCode(string code, Session currentSession = null);
        int SaveStatus(Status model);
        bool DeleteStatus(int statusId);

        int GetKVPRedCardStatusIDByCode(string code);
    }
}
