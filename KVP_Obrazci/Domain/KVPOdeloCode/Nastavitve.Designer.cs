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

    public partial class Nastavitve : XPLiteObject
    {
        int fNastavitveID;
        [Key(true)]
        public int NastavitveID
        {
            get { return fNastavitveID; }
            set { SetPropertyValue<int>("NastavitveID", ref fNastavitveID, value); }
        }
        int fStevilkaKVP;
        public int StevilkaKVP
        {
            get { return fStevilkaKVP; }
            set { SetPropertyValue<int>("StevilkaKVP", ref fStevilkaKVP, value); }
        }
        decimal fIzplacilo;
        public decimal Izplacilo
        {
            get { return fIzplacilo; }
            set { SetPropertyValue<decimal>("Izplacilo", ref fIzplacilo, value); }
        }
        decimal fKolicnik;
        public decimal Kolicnik
        {
            get { return fKolicnik; }
            set { SetPropertyValue<decimal>("Kolicnik", ref fKolicnik, value); }
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
        bool fMailPosiljanje;
        public bool MailPosiljanje
        {
            get { return fMailPosiljanje; }
            set { SetPropertyValue<bool>("MailPosiljanje", ref fMailPosiljanje, value); }
        }
        string fOpombe;
        [Size(SizeAttribute.Unlimited)]
        public string Opombe
        {
            get { return fOpombe; }
            set { SetPropertyValue<string>("Opombe", ref fOpombe, value); }
        }
        string fPotNavodilaZaUporabo;
        [Size(3000)]
        public string PotNavodilaZaUporabo
        {
            get { return fPotNavodilaZaUporabo; }
            set { SetPropertyValue<string>("PotNavodilaZaUporabo", ref fPotNavodilaZaUporabo, value); }
        }
        int fStevilkaRK;
        public int StevilkaRK
        {
            get { return fStevilkaRK; }
            set { SetPropertyValue<int>("StevilkaRK", ref fStevilkaRK, value); }
        }
        string fVerzija;
        [Size(SizeAttribute.Unlimited)]
        public string Verzija
        {
            get { return fVerzija; }
            set { SetPropertyValue<string>("Verzija", ref fVerzija, value); }
        }
        string fVerzijaStevilka;
        [Size(50)]
        public string VerzijaStevilka
        {
            get { return fVerzijaStevilka; }
            set { SetPropertyValue<string>("VerzijaStevilka", ref fVerzijaStevilka, value); }
        }
    }

}
