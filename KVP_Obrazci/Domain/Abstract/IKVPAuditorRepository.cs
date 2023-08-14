using DevExpress.Xpo;
using KVP_Obrazci.Domain.KVPOdelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IKVPAuditorRepository
    {
        KVPPresoje GetKVPAuditorByID(int kvpAuditorID, Session currentSession = null);
        int SaveKVPAuditor(KVPPresoje model);

        void DeleteKVPAuditor(int id);
        void DeleteKVPAuditor(KVPPresoje model);
        int GetKVPAuditorsCountByKVPId(int kvpDocID);
        List<KVPPresoje> GetKVPAuditorsByKVPId(int kvpDocID);
        Users GetLatestAuditorOnKVP(int kvpDocID);
    }
}
