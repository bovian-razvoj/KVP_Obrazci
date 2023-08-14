using DevExpress.Xpo;
using KVP_Obrazci.Domain.KVPOdelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IKVPStatusRepository
    {
        KVP_Status GetLatestKVPStatus(int kvpDocID);
        KVP_Status GetLatestKVPStatus(KVPDocument doc);
        List<KVP_Status> GetKVPStatusesBYDocID(int kvpDocID);
        int SaveKVPStatus(KVP_Status model);
        bool DeleteKVPStatus(int statusID);
        KVP_Status GetKVPRedCardStatus(int kvpRedCardDocID);
        KVP_Status GetKVPRedCardStatus(KVPDocument doc);
        bool HasKVPDocumentKVPStatus(int kvpDocID, string kodaStatus);
    }
}
