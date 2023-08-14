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
    public interface ILocationRepository
    {
        Lokacija GetLocationByID(int id, Session currentSession = null);
        void SaveLocation(Lokacija model);
        void DeleteLocation(int id, Session currentSession = null);
        void DeleteLocation(Lokacija model);
        void SaveLocationFromBatchUpdate(List<ASPxDataUpdateValues> updateValues);

        int GetCntForSort();
        string GenerateCode(string title);
    }
}
