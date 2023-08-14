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

        public string SenderFirstName { get; set; }
        public string SenderLastname { get; set; }
        public string Notes { get; set; }

        public string StevilkaKVP { get; set; }
    }
}