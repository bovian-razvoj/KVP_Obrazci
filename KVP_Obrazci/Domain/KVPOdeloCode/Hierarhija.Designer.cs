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

    public partial class Hierarhija : XPLiteObject
    {
        int fidHierarhija;
        [Key(true)]
        public int idHierarhija
        {
            get { return fidHierarhija; }
            set { SetPropertyValue<int>("idHierarhija", ref fidHierarhija, value); }
        }
        Zaposleni fidZaposleni;
        [Association(@"HierarhijaReferencesZaposleni1")]
        public Zaposleni idZaposleni
        {
            get { return fidZaposleni; }
            set { SetPropertyValue<Zaposleni>("idZaposleni", ref fidZaposleni, value); }
        }
        Zaposleni fZap_idZaposleni;
        [Association(@"HierarhijaReferencesZaposleni")]
        public Zaposleni Zap_idZaposleni
        {
            get { return fZap_idZaposleni; }
            set { SetPropertyValue<Zaposleni>("Zap_idZaposleni", ref fZap_idZaposleni, value); }
        }
    }

}
