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

    public partial class KVPFullReport : XPLiteObject
    {
        int fidKVPFullReport;
        [Key(true)]
        public int idKVPFullReport
        {
            get { return fidKVPFullReport; }
            set { SetPropertyValue<int>("idKVPFullReport", ref fidKVPFullReport, value); }
        }
        int fLeto;
        public int Leto
        {
            get { return fLeto; }
            set { SetPropertyValue<int>("Leto", ref fLeto, value); }
        }
        int fMesec;
        public int Mesec
        {
            get { return fMesec; }
            set { SetPropertyValue<int>("Mesec", ref fMesec, value); }
        }
        string fTrenutniStatus;
        [Size(50)]
        public string TrenutniStatus
        {
            get { return fTrenutniStatus; }
            set { SetPropertyValue<string>("TrenutniStatus", ref fTrenutniStatus, value); }
        }
        string fStevilkaKVP;
        [Size(50)]
        public string StevilkaKVP
        {
            get { return fStevilkaKVP; }
            set { SetPropertyValue<string>("StevilkaKVP", ref fStevilkaKVP, value); }
        }
        string fKVPSkupinaKoda;
        [Size(50)]
        public string KVPSkupinaKoda
        {
            get { return fKVPSkupinaKoda; }
            set { SetPropertyValue<string>("KVPSkupinaKoda", ref fKVPSkupinaKoda, value); }
        }
        string fKVPSkupinaNaziv;
        [Size(50)]
        public string KVPSkupinaNaziv
        {
            get { return fKVPSkupinaNaziv; }
            set { SetPropertyValue<string>("KVPSkupinaNaziv", ref fKVPSkupinaNaziv, value); }
        }
        DateTime fDatumVnosa;
        public DateTime DatumVnosa
        {
            get { return fDatumVnosa; }
            set { SetPropertyValue<DateTime>("DatumVnosa", ref fDatumVnosa, value); }
        }
        string fUporabnik;
        [Size(150)]
        public string Uporabnik
        {
            get { return fUporabnik; }
            set { SetPropertyValue<string>("Uporabnik", ref fUporabnik, value); }
        }
        string fKVPProblem;
        [Size(3150)]
        public string KVPProblem
        {
            get { return fKVPProblem; }
            set { SetPropertyValue<string>("KVPProblem", ref fKVPProblem, value); }
        }
        string fKVPPredlogIzboljsave;
        [Size(3150)]
        public string KVPPredlogIzboljsave
        {
            get { return fKVPPredlogIzboljsave; }
            set { SetPropertyValue<string>("KVPPredlogIzboljsave", ref fKVPPredlogIzboljsave, value); }
        }
        string fVodjaZaOdobritev;
        [Size(150)]
        public string VodjaZaOdobritev
        {
            get { return fVodjaZaOdobritev; }
            set { SetPropertyValue<string>("VodjaZaOdobritev", ref fVodjaZaOdobritev, value); }
        }
        string fPresoja;
        [Size(150)]
        public string Presoja
        {
            get { return fPresoja; }
            set { SetPropertyValue<string>("Presoja", ref fPresoja, value); }
        }
        string fRealizator;
        [Size(150)]
        public string Realizator
        {
            get { return fRealizator; }
            set { SetPropertyValue<string>("Realizator", ref fRealizator, value); }
        }
        DateTime fDatumZakljuceneIdeje;
        public DateTime DatumZakljuceneIdeje
        {
            get { return fDatumZakljuceneIdeje; }
            set { SetPropertyValue<DateTime>("DatumZakljuceneIdeje", ref fDatumZakljuceneIdeje, value); }
        }
        int fCasDoZakljuceneIdeje;
        public int CasDoZakljuceneIdeje
        {
            get { return fCasDoZakljuceneIdeje; }
            set { SetPropertyValue<int>("CasDoZakljuceneIdeje", ref fCasDoZakljuceneIdeje, value); }
        }
        string fTipIdeje;
        [Size(150)]
        public string TipIdeje
        {
            get { return fTipIdeje; }
            set { SetPropertyValue<string>("TipIdeje", ref fTipIdeje, value); }
        }
        int fTockePredlagatelj;
        public int TockePredlagatelj
        {
            get { return fTockePredlagatelj; }
            set { SetPropertyValue<int>("TockePredlagatelj", ref fTockePredlagatelj, value); }
        }
        int fTockeRealizator;
        public int TockeRealizator
        {
            get { return fTockeRealizator; }
            set { SetPropertyValue<int>("TockeRealizator", ref fTockeRealizator, value); }
        }
        int fExternalIDaposlenega;
        public int ExternalIDaposlenega
        {
            get { return fExternalIDaposlenega; }
            set { SetPropertyValue<int>("ExternalIDaposlenega", ref fExternalIDaposlenega, value); }
        }
    }

}
