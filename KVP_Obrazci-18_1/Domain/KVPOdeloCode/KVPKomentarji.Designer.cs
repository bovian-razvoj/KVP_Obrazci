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

    public partial class KVPKomentarji : XPLiteObject
    {
        int fKVPKomentarjiID;
        [Key(true)]
        public int KVPKomentarjiID
        {
            get { return fKVPKomentarjiID; }
            set { SetPropertyValue<int>("KVPKomentarjiID", ref fKVPKomentarjiID, value); }
        }
        Users fUserId;
        [Association(@"KVPKomentarjiReferencesUsers")]
        public Users UserId
        {
            get { return fUserId; }
            set { SetPropertyValue<Users>("UserId", ref fUserId, value); }
        }
        KVPDocument fKVPDocId;
        [Association(@"KVPKomentarjiReferencesKVPDocument")]
        public KVPDocument KVPDocId
        {
            get { return fKVPDocId; }
            set { SetPropertyValue<KVPDocument>("KVPDocId", ref fKVPDocId, value); }
        }
        string fKoda;
        [Size(25)]
        public string Koda
        {
            get { return fKoda; }
            set { SetPropertyValue<string>("Koda", ref fKoda, value); }
        }
        string fOpombe;
        [Size(2000)]
        public string Opombe
        {
            get { return fOpombe; }
            set { SetPropertyValue<string>("Opombe", ref fOpombe, value); }
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
    }

}