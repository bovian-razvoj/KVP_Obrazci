using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace KVP_Obrazci.Infrastructure
{
    public class UserPrincipal : IPrincipal
    {
        public int ID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string Role { get; set; }
        public string ProfileImage { get; set; }
        public string DepartmentName { get; set; }
        public string Card { get; set; }
        public string Champion { get; set; }
        //public int ChampionId { get; set; }
        public string Supervisor { get; set; }
        //ublic int SupervisorId { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public IIdentity Identity
        {
            get;
            set;
        }

        public bool IsInRole(string role)
        {
            return Role == role;
        }
    }
}