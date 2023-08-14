using DevExpress.Xpo;
using KVP_Obrazci.Domain.KVPOdelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IActiveUserRepository
    {
        ActiveUser GetActiveUserByUserID(int userID, Session currentSession = null);
        void SaveActiveUser(int userID, int sessionExpiresMin = 0);
        void SaveUserLoggedInActivity(bool active, int userID, int sessionExpiresMin = 0);
        void SaveLastRequest(int userID);
        List<ActiveUser> GetHistoryActiveUsers();
        void UpdateUsersLoginActivity();
        List<ActiveUser> GetAllActiveUsersForCurrentDay(Session currentSession = null);
    }
}
