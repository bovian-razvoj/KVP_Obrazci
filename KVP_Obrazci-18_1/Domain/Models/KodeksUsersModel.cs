using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Models
{
    public class KodeksUsersModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int DepartmentId { get; set; }
        public string ExternalId { get; set; }
        public bool IsTimeAttendance { get; set; }
        public string Card { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}