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

    public partial class Lokacija : XPLiteObject
    {
        int fidLokacija;
        [Key(true)]
        public int idLokacija
        {
            get { return fidLokacija; }
            set { SetPropertyValue<int>("idLokacija", ref fidLokacija, value); }
        }
        int fSort;
        public int Sort
        {
            get { return fSort; }
            set { SetPropertyValue<int>("Sort", ref fSort, value); }
        }
        string fKoda;
        [Size(50)]
        public string Koda
        {
            get { return fKoda; }
            set { SetPropertyValue<string>("Koda", ref fKoda, value); }
        }
        string fOpis;
        [Size(250)]
        public string Opis
        {
            get { return fOpis; }
            set { SetPropertyValue<string>("Opis", ref fOpis, value); }
        }
        int fidIDOseba;
        public int idIDOseba
        {
            get { return fidIDOseba; }
            set { SetPropertyValue<int>("idIDOseba", ref fidIDOseba, value); }
        }
        DateTime fts;
        public DateTime ts
        {
            get { return fts; }
            set { SetPropertyValue<DateTime>("ts", ref fts, value); }
        }
        [Association(@"KVPDocumentReferencesLokacija")]
        public XPCollection<KVPDocument> KVPDocuments { get { return GetCollection<KVPDocument>("KVPDocuments"); } }
    }

}
