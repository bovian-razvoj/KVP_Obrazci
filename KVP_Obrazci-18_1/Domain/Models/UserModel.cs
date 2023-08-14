using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string Role { get; set; }
        public string RoleName { get; set; }
        public int RoleID { get; set; }
        public string Job { get; set; }
        public DateTime dateCreated { get; set; }

        public string DepartmentName { get; set; }
        public string Card { get; set; }

        public string Champion { get; set; }
        //public int ChampionId { get; set; }
        public string Supervisor { get; set; }
        //public int SupervisorId { get; set; }
        public string GroupName { get; set; }
        public int GroupID { get; set; }
        // public int GroupId { get; set; }

        public string username { get; set; }

        public string profileImage { get; set; }
        public bool HasSupervisor { get; set; }
    }
}