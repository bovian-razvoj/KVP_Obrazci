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

    public partial class KVPPresoje : XPLiteObject
    {
        int fidKVPPresoje;
        [Key(true)]
        public int idKVPPresoje
        {
            get { return fidKVPPresoje; }
            set { SetPropertyValue<int>("idKVPPresoje", ref fidKVPPresoje, value); }
        }
        KVPDocument fidKVPDocument;
        [Association(@"KVPPresojeReferencesKVPDocument")]
        public KVPDocument idKVPDocument
        {
            get { return fidKVPDocument; }
            set { SetPropertyValue<KVPDocument>("idKVPDocument", ref fidKVPDocument, value); }
        }
        Users fPresojevalec;
        [Association(@"KVPPresojeReferencesUsers")]
        public Users Presojevalec
        {
            get { return fPresojevalec; }
            set { SetPropertyValue<Users>("Presojevalec", ref fPresojevalec, value); }
        }
        string fOpomba;
        [Size(5000)]
        public string Opomba
        {
            get { return fOpomba; }
            set { SetPropertyValue<string>("Opomba", ref fOpomba, value); }
        }
        int ftsIDOseba;
        public int tsIDOseba
        {
            get { return ftsIDOseba; }
            set { SetPropertyValue<int>("tsIDOseba", ref ftsIDOseba, value); }
        }
        DateTime fts;
        public DateTime ts
        {
            get { return fts; }
            set { SetPropertyValue<DateTime>("ts", ref fts, value); }
        }
        bool fJeZadnjiPresojevalec;
        public bool JeZadnjiPresojevalec
        {
            get { return fJeZadnjiPresojevalec; }
            set { SetPropertyValue<bool>("JeZadnjiPresojevalec", ref fJeZadnjiPresojevalec, value); }
        }
    }

}