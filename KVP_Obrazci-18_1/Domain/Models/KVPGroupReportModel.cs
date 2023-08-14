using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Models
{
    public class KVPGroupReportModel
    {
        public int idKVPSkupina { get; set; }

        public string Koda { get; set; }
        public string Naziv { get; set; }
        public int Podani { get; set; }
        public int Realizirani { get; set; }
        public int Zavrnjeni { get; set; }
        public int Odprti { get; set; }
    }

    public class KVPGroupReportModelPartial
    {
        public int idKVPSkupina { get; set; }

        public string Koda { get; set; }
        public string Naziv { get; set; }

        public int idKVPDocument { get; set; }
        public int idUser { get; set; }
        public string User { get; set;}
        public int StatusID { get; set; }
        public string StatusName { get; set; }
    }
}