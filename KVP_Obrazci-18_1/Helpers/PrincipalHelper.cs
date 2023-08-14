using KVP_Obrazci.Common;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Helpers
{
    public class PrincipalHelper
    {
        public static UserPrincipal GetUserPrincipal()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return (UserPrincipal)HttpContext.Current.User;
            }

            return null;
        }

        public static bool IsUserSuperAdmin()
        {
            var princip = GetUserPrincipal();

            return princip != null ? princip.IsInRole(Enums.UserRole.SuperAdmin.ToString()) : false;
        }

        public static bool IsUserAdmin()
        {
            var princip = GetUserPrincipal();
            return princip != null ? princip.IsInRole(Enums.UserRole.Admin.ToString()) : false;
        }

        public static bool IsUserLeader()
        {
            var princip = GetUserPrincipal();
            return princip != null ? princip.IsInRole(Enums.UserRole.Leader.ToString()) : false;
        }

        public static bool IsUserChampion()
        {
            var princip = GetUserPrincipal();
            return princip != null ? princip.IsInRole(Enums.UserRole.Champion.ToString()) : false;
        }

        public static bool IsUserEmployee()
        {
            var princip = GetUserPrincipal();
            return princip != null ? princip.IsInRole(Enums.UserRole.Employee.ToString()) : false;
        }

        public static bool IsUserTpmAdmin()
        {
            var princip = GetUserPrincipal();
            return princip != null ? princip.IsInRole(Enums.UserRole.TpmAdmin.ToString()) : false;
        }
    }
}