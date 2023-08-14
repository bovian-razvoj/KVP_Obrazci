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
    public interface IPlaningAndRealizationRepository
    {
        PlanRealizacija GetPlanRealizationByID(int id, Session currentSession = null);
        void SavePlaningFromBatchUpdate(List<ASPxDataUpdateValues> updateValues);

        PlanRealizacija GetPlanRealizationByKVPGroupAndYear(int kvpGroupID, int year, Session currentSession = null);
        XPCollection<PlanRealizacija> GetPlanRealizationByKVPGroupAndYearWithSumAndYTD(int year, Session currentSession = null);
        void UpdatePlanRealizationByKvpSubmit(int userID, int year, int month, int quantity, Session currentSession = null);

        void RefreshKVPRealizationPercentage(Session currentSession = null);
    }
}
