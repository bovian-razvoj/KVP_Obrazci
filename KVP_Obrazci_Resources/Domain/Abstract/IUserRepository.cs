using KVP_Obrazci.Domain.GrafolitOTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IUserRepository
    {
        Osebe_OTP UserLogIn(string userName, string password);

        Vloga_OTP GetRoleByID(int id);

        string GetRoleNameByID(int id);
    }
}
