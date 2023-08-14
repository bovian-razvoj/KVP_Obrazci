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
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace KVP_Obrazci.Domain.KVPOdelo
{

    public partial class PrispevekKategorija : XPLiteObject
    {
        int fPrispevekKategorijaID;
        [Key(true)]
        public int PrispevekKategorijaID
        {
            get { return fPrispevekKategorijaID; }
            set { SetPropertyValue<int>("PrispevekKategorijaID", ref fPrispevekKategorijaID, value); }
        }
        string fKoda;
        [Size(50)]
        public string Koda
        {
            get { return fKoda; }
            set { SetPropertyValue<string>("Koda", ref fKoda, value); }
        }
        string fNaziv;
        [Size(300)]
        public string Naziv
        {
            get { return fNaziv; }
            set { SetPropertyValue<string>("Naziv", ref fNaziv, value); }
        }
        string fOpomba;
        [Size(600)]
        public string Opomba
        {
            get { return fOpomba; }
            set { SetPropertyValue<string>("Opomba", ref fOpomba, value); }
        }
        int fIDPrijave;
        public int IDPrijave
        {
            get { return fIDPrijave; }
            set { SetPropertyValue<int>("IDPrijave", ref fIDPrijave, value); }
        }
        DateTime fts;
        public DateTime ts
        {
            get { return fts; }
            set { SetPropertyValue<DateTime>("ts", ref fts, value); }
        }
        [Association(@"PrispevekReferencesPrispevekKategorija")]
        public XPCollection<Prispevek> Prispeveks { get { return GetCollection<Prispevek>("Prispeveks"); } }
    }

}
