﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace KVP_Obrazci.Domain.KVPOdelo
{

    public partial class KVPGroupReport : XPLiteObject
    {
        int fidKVPGroupReport;
        [Key(true)]
        public int idKVPGroupReport
        {
            get { return fidKVPGroupReport; }
            set { SetPropertyValue<int>("idKVPGroupReport", ref fidKVPGroupReport, value); }
        }
        string fSkupinaKoda;
        [Size(50)]
        public string SkupinaKoda
        {
            get { return fSkupinaKoda; }
            set { SetPropertyValue<string>("SkupinaKoda", ref fSkupinaKoda, value); }
        }
        string fSkupinaNaziv;
        [Size(150)]
        public string SkupinaNaziv
        {
            get { return fSkupinaNaziv; }
            set { SetPropertyValue<string>("SkupinaNaziv", ref fSkupinaNaziv, value); }
        }
        int fPodani;
        public int Podani
        {
            get { return fPodani; }
            set { SetPropertyValue<int>("Podani", ref fPodani, value); }
        }
        int fRealizirani;
        public int Realizirani
        {
            get { return fRealizirani; }
            set { SetPropertyValue<int>("Realizirani", ref fRealizirani, value); }
        }
        int fZavrnjeni;
        public int Zavrnjeni
        {
            get { return fZavrnjeni; }
            set { SetPropertyValue<int>("Zavrnjeni", ref fZavrnjeni, value); }
        }
        int fOdprti;
        public int Odprti
        {
            get { return fOdprti; }
            set { SetPropertyValue<int>("Odprti", ref fOdprti, value); }
        }
    }

}
