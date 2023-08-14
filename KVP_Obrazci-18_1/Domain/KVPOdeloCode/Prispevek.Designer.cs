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

    public partial class Prispevek : XPLiteObject
    {
        int fPrispevkiID;
        [Key(true)]
        public int PrispevkiID
        {
            get { return fPrispevkiID; }
            set { SetPropertyValue<int>("PrispevkiID", ref fPrispevkiID, value); }
        }
        PrispevekKategorija fKategorijaID;
        [Association(@"PrispevekReferencesPrispevekKategorija")]
        public PrispevekKategorija KategorijaID
        {
            get { return fKategorijaID; }
            set { SetPropertyValue<PrispevekKategorija>("KategorijaID", ref fKategorijaID, value); }
        }
        Users fAvtorID;
        [Association(@"PrispevekReferencesUsers")]
        public Users AvtorID
        {
            get { return fAvtorID; }
            set { SetPropertyValue<Users>("AvtorID", ref fAvtorID, value); }
        }
        string fNaslov;
        [Size(250)]
        public string Naslov
        {
            get { return fNaslov; }
            set { SetPropertyValue<string>("Naslov", ref fNaslov, value); }
        }
        string fBesedilo;
        [Size(SizeAttribute.Unlimited)]
        public string Besedilo
        {
            get { return fBesedilo; }
            set { SetPropertyValue<string>("Besedilo", ref fBesedilo, value); }
        }
        string fIzvlecek;
        [Size(300)]
        public string Izvlecek
        {
            get { return fIzvlecek; }
            set { SetPropertyValue<string>("Izvlecek", ref fIzvlecek, value); }
        }
        string fPriloge;
        [Size(5000)]
        public string Priloge
        {
            get { return fPriloge; }
            set { SetPropertyValue<string>("Priloge", ref fPriloge, value); }
        }
        string fPrikaznaSlika;
        [Size(400)]
        public string PrikaznaSlika
        {
            get { return fPrikaznaSlika; }
            set { SetPropertyValue<string>("PrikaznaSlika", ref fPrikaznaSlika, value); }
        }
        DateTime fDatumSpremembe;
        public DateTime DatumSpremembe
        {
            get { return fDatumSpremembe; }
            set { SetPropertyValue<DateTime>("DatumSpremembe", ref fDatumSpremembe, value); }
        }
        DateTime fDatumVnosa;
        public DateTime DatumVnosa
        {
            get { return fDatumVnosa; }
            set { SetPropertyValue<DateTime>("DatumVnosa", ref fDatumVnosa, value); }
        }
        bool fObjavljen;
        public bool Objavljen
        {
            get { return fObjavljen; }
            set { SetPropertyValue<bool>("Objavljen", ref fObjavljen, value); }
        }
    }

}