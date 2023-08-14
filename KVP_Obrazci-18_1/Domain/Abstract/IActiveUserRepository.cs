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
        void SaveActiveUser(int userID);
        void SaveUserLoggedInActivity(bool active, int userID);
    }
}
