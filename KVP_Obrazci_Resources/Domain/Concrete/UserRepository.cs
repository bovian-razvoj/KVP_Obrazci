using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.GrafolitOTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class UserRepository : IUserRepository
    {
        public Osebe_OTP UserLogIn(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Vloga_OTP GetRoleByID(int id)
        {
            throw new NotImplementedException();
        }

        public string GetRoleNameByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}