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

    public partial class TipRdeciKarton : XPLiteObject
    {
        int fidTipRdeciKarton;
        [Key(true)]
        public int idTipRdeciKarton
        {
            get { return fidTipRdeciKarton; }
            set { SetPropertyValue<int>("idTipRdeciKarton", ref fidTipRdeciKarton, value); }
        }
        string fKoda;
        [Size(20)]
        public string Koda
        {
            get { return fKoda; }
            set { SetPropertyValue<string>("Koda", ref fKoda, value); }
        }
        string fNaziv;
        [Size(50)]
        public string Naziv
        {
            get { return fNaziv; }
            set { SetPropertyValue<string>("Naziv", ref fNaziv, value); }
        }
        [Association(@"KVPDocumentReferencesTipRdeciKarton")]
        public XPCollection<KVPDocument> KVPDocuments { get { return GetCollection<KVPDocument>("KVPDocuments"); } }
    }

}
