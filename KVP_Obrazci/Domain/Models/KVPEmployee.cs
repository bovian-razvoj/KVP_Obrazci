using KVP_Obrazci.Domain.KVPOdelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Models
{
    public class KVPEmployee
    {
        public Users employee { get; set; }
        public decimal employeeID { get; set; }
        public string DisplayChampionItem { get; set; }
        public string DisplayRKItems { get; set; }
        public string DisplayRKItemsToConfirm { get; set; }
        public int KVPToConfirm { get; set; }
        public int KVPToAudit { get; set; }
        public int KVPToRealize { get; set; }
        public int KVPToComplete { get; set; }
        public bool isUserChampion { get; set; }
        public string DisplayListItemStyle { get; set; }
        

        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string ServerTag { get; set; }
        public string ServerTagLogin { get; set; }

        public string StevilkaKVP { get; set; }
        public string KVPRejectFirstName { get; set; }
        public string KVPRejectLastname { get; set; }
        public string Arguments { get; set; }

        public string Password { get; set; }
        public string Username { get; set; }

        public string ChangePassEmployeeUrl { get; set; }

        public int RKToRealize { get; set; }
        public int RKToConfirm { get; set; }
    }
}
