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
    public interface ILineRepository
    {
        Linija GetLineByID(int id, Session currentSession = null);
        void SaveLine(Linija model);
        void DeleteLine(int id, Session currentSession = null);
        void DeleteLine(Linija model);
        void SaveLineFromBatchUpdate(List<ASPxDataUpdateValues> updateValues);
        int GetCountForSort();
        string GenerateCode(string title);
    }
}
