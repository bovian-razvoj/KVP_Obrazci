using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Models
{
    public class PayoutOverviewSummary
    {
        public string Mesec { get; set; }
        public int Leto { get; set; }

        public decimal VsotaPrenosIzprejsnjegaMeseca { get; set; }
        public decimal VsotaPredlagateljT { get; set; }
        public decimal VsotaRealizatorT { get; set; }
        public decimal VsotaVsotaT { get; set; }
        public decimal VsotaPrenosVNaslednjiMesec { get; set; }

        //public List<PayoutOverviewSummaryItem> Items { get; set; }
    }

    public class PayoutOverviewSummaryItem
    {
        public decimal VsotaPrenosIzprejsnjegaMeseca { get; set; }
        public decimal VsotaPredlagateljT { get; set; }
        public decimal VsotaRealizatorT { get; set; }
        public decimal VsotaVsotaT { get; set; }
        public decimal VsotaPrenosVNaslednjiMesec { get; set; }
    }
}