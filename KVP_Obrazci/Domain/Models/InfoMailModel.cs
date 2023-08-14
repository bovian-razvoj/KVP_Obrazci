using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Models
{
    public class InfoMailModel
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string ServerTag { get; set; }
        public string ServerTagDoc { get; set; }

        public string SenderFirstName { get; set; }
        public string SenderLastname { get; set; }
        public string Senders { get; set; }
        public string Notes { get; set; }

        public string StevilkaKVP { get; set; }
        public Int32 DocumentID { get; set; }

        public string OldUserName { get; set; }
        public string NewUserName { get; set; }
    }
}