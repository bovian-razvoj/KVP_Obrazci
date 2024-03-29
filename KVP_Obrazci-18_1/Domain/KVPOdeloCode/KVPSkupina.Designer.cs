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

    public partial class KVPSkupina : XPLiteObject
    {
        int fidKVPSkupina;
        [Key(true)]
        public int idKVPSkupina
        {
            get { return fidKVPSkupina; }
            set { SetPropertyValue<int>("idKVPSkupina", ref fidKVPSkupina, value); }
        }
        Users fPotrjevalec1;
        [Association(@"KVPSkupinaReferencesUsers")]
        public Users Potrjevalec1
        {
            get { return fPotrjevalec1; }
            set { SetPropertyValue<Users>("Potrjevalec1", ref fPotrjevalec1, value); }
        }
        Users fPotrjevalec2;
        [Association(@"KVPSkupinaReferencesUsers1")]
        public Users Potrjevalec2
        {
            get { return fPotrjevalec2; }
            set { SetPropertyValue<Users>("Potrjevalec2", ref fPotrjevalec2, value); }
        }
        Users fPotrjevalec3;
        [Association(@"KVPSkupinaReferencesUsers2")]
        public Users Potrjevalec3
        {
            get { return fPotrjevalec3; }
            set { SetPropertyValue<Users>("Potrjevalec3", ref fPotrjevalec3, value); }
        }
        string fKoda;
        [Size(50)]
        public string Koda
        {
            get { return fKoda; }
            set { SetPropertyValue<string>("Koda", ref fKoda, value); }
        }
        string fNaziv;
        [Size(200)]
        public string Naziv
        {
            get { return fNaziv; }
            set { SetPropertyValue<string>("Naziv", ref fNaziv, value); }
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
        [Association(@"KVPSkupina_UsersReferencesKVPSkupina")]
        public XPCollection<KVPSkupina_Users> KVPSkupina_Userss { get { return GetCollection<KVPSkupina_Users>("KVPSkupina_Userss"); } }
        [Association(@"PlanRealizacijaReferencesKVPSkupina")]
        public XPCollection<PlanRealizacija> PlanRealizacijas { get { return GetCollection<PlanRealizacija>("PlanRealizacijas"); } }
        [Association(@"KVPDocumentReferencesKVPSkupina")]
        public XPCollection<KVPDocument> KVPDocuments { get { return GetCollection<KVPDocument>("KVPDocuments"); } }
    }

}
