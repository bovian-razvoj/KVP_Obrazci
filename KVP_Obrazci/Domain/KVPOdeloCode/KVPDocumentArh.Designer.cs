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

    public partial class KVPDocumentArh : XPLiteObject
    {
        int fidKVPDocumentArh;
        [Key(true)]
        public int idKVPDocumentArh
        {
            get { return fidKVPDocumentArh; }
            set { SetPropertyValue<int>("idKVPDocumentArh", ref fidKVPDocumentArh, value); }
        }
        string fStevilkaKVP;
        [Size(50)]
        public string StevilkaKVP
        {
            get { return fStevilkaKVP; }
            set { SetPropertyValue<string>("StevilkaKVP", ref fStevilkaKVP, value); }
        }
        string fKVPSKupina;
        [Size(50)]
        public string KVPSKupina
        {
            get { return fKVPSKupina; }
            set { SetPropertyValue<string>("KVPSKupina", ref fKVPSKupina, value); }
        }
        string fDatumVnosa;
        [Size(50)]
        public string DatumVnosa
        {
            get { return fDatumVnosa; }
            set { SetPropertyValue<string>("DatumVnosa", ref fDatumVnosa, value); }
        }
        string fPredlagatelj;
        [Size(50)]
        public string Predlagatelj
        {
            get { return fPredlagatelj; }
            set { SetPropertyValue<string>("Predlagatelj", ref fPredlagatelj, value); }
        }
        string fOpisProblem;
        [Size(5000)]
        public string OpisProblem
        {
            get { return fOpisProblem; }
            set { SetPropertyValue<string>("OpisProblem", ref fOpisProblem, value); }
        }
        string fVodjaZaOdobritevIdeje;
        [Size(50)]
        public string VodjaZaOdobritevIdeje
        {
            get { return fVodjaZaOdobritevIdeje; }
            set { SetPropertyValue<string>("VodjaZaOdobritevIdeje", ref fVodjaZaOdobritevIdeje, value); }
        }
        string fPresoja;
        [Size(50)]
        public string Presoja
        {
            get { return fPresoja; }
            set { SetPropertyValue<string>("Presoja", ref fPresoja, value); }
        }
        string fRealizator;
        [Size(50)]
        public string Realizator
        {
            get { return fRealizator; }
            set { SetPropertyValue<string>("Realizator", ref fRealizator, value); }
        }
        string fDatumZakljuceneIdeje;
        [Size(50)]
        public string DatumZakljuceneIdeje
        {
            get { return fDatumZakljuceneIdeje; }
            set { SetPropertyValue<string>("DatumZakljuceneIdeje", ref fDatumZakljuceneIdeje, value); }
        }
        int fSprejel;
        public int Sprejel
        {
            get { return fSprejel; }
            set { SetPropertyValue<int>("Sprejel", ref fSprejel, value); }
        }
        int fRealiziral;
        public int Realiziral
        {
            get { return fRealiziral; }
            set { SetPropertyValue<int>("Realiziral", ref fRealiziral, value); }
        }
        int fZavrnil;
        public int Zavrnil
        {
            get { return fZavrnil; }
            set { SetPropertyValue<int>("Zavrnil", ref fZavrnil, value); }
        }
        string fOpombe;
        [Size(5000)]
        public string Opombe
        {
            get { return fOpombe; }
            set { SetPropertyValue<string>("Opombe", ref fOpombe, value); }
        }
        string fTipIdeje;
        [Size(50)]
        public string TipIdeje
        {
            get { return fTipIdeje; }
            set { SetPropertyValue<string>("TipIdeje", ref fTipIdeje, value); }
        }
    }

}
