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
    public interface IKVPTypeRepository
    {
        Tip GetKVPTypeByID(int id, Session currentSession = null);
        void SaveKVPType(Tip model);
        void DeleteKVPType(int id, Session currentSession = null);
        void DeleteKVPType(Tip model);
        void SaveKVPTypeFromBatchUpdate(List<ASPxDataUpdateValues> updateValues);
    }
}
