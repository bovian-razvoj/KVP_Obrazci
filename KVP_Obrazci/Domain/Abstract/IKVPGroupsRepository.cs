using DevExpress.Web.Data;
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
    public interface IKVPGroupsRepository
    {
        KVPSkupina GetKVPGroupByID(int kvpGroupID, Session currentSession = null);
        KVPSkupina GetKVPGroupByCode(string kvpCode, Session currentSession = null);
        int SaveKVPGroup(KVPSkupina model);
        bool DeleteKVPGroup(KVPSkupina model);
        bool DeleteKVPGroup(int kvpGroupID);

        void SaveEmployeesToKVPGroup(List<object> selectedRows, int kvpGroupID, bool isChampions = false, bool isNewEmployee = false);
        void DeleteEmployeesFromKVPGroupUsers(List<int> selectedItems);
        List<KVPSkupina_Users> GetKVPGroupUsersChampionsByGroupID(int kvpGroupID);

        KVPSkupina_Users GetKVPGroupUserByUserID(int userID, Session currentSession = null);
        List<Users> GetUsersFromKVPGroupByID(int kvpGroupID);
        List<KVPSkupina> GetGroupsFromUsersByUsersID(int UserID);
        int GetUserCountWithNoKVPGroup();
        int GetDeletedUserCountWithKVPGroup();
        List<Users> GetKVPGroupChampionsByKVPGroupID(int id);
        void SaveKVPGroupFromBatchUpdate(List<ASPxDataUpdateValues> updateValues);


        List<KVPGroupReportModel> GetKVPDocForDatePeriodLastStatusAndGroupID(DateTime dtFrom, DateTime dtTo, Int32 idKVPGroup,
            Session currentSession = null,
            bool onlyCompletedKVP = false);
    }
}
