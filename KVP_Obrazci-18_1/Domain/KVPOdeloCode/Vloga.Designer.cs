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

    public partial class Vloga : XPLiteObject
    {
        int fVlogaID;
        [Key(true)]
        public int VlogaID
        {
            get { return fVlogaID; }
            set { SetPropertyValue<int>("VlogaID", ref fVlogaID, value); }
        }
        string fKoda;
        [Size(50)]
        public string Koda
        {
            get { return fKoda; }
            set { SetPropertyValue<string>("Koda", ref fKoda, value); }
        }
        string fNaziv;
        [Size(150)]
        public string Naziv
        {
            get { return fNaziv; }
            set { SetPropertyValue<string>("Naziv", ref fNaziv, value); }
        }
        string fOpis;
        [Size(500)]
        public string Opis
        {
            get { return fOpis; }
            set { SetPropertyValue<string>("Opis", ref fOpis, value); }
        }
        DateTime fts;
        public DateTime ts
        {
            get { return fts; }
            set { SetPropertyValue<DateTime>("ts", ref fts, value); }
        }
        [Association(@"UsersReferencesVloga")]
        public XPCollection<Users> UsersCollection { get { return GetCollection<Users>("UsersCollection"); } }
    }

}
