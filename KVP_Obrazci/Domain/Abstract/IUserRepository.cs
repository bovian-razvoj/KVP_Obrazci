using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IUserRepository
    {
        UserModel UserLogIn(string userName, string password);
        UserModel UserLogInCard(string token);

        //Vloga_OTP GetRoleByID(int id);

        //string GetRoleNameByID(int id);
    }
}
