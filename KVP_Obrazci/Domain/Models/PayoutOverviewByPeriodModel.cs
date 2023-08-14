using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Models
{
    public class PayoutOverviewByPeriodModel
    {
        public int UserID { get; set; }
        public string KVPSkupinaNaziv { get; set; }
        public string Zaposlen { get; set; }

        public List<PayoutModel> payoutOverviewList { get; set; }
    }

    public class PayoutModel
    {
        public int idIzplacila { get; set; }
        public string Mesec { get; set; }
        public int Leto { get; set; }
        public decimal PrenosIzprejsnjegaMeseca { get; set; }
        public decimal PredlagateljT { get; set; }
        public decimal RealizatorT { get; set; }
        public decimal VsotaT { get; set; }
        public decimal PrenosVNaslednjiMesec { get; set; }
    }
}