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

    public partial class Realizacija : XPLiteObject
    {
        int fid;
        [Key]
        public int id
        {
            get { return fid; }
            set { SetPropertyValue<int>("id", ref fid, value); }
        }
        string fkoda;
        [Size(255)]
        public string koda
        {
            get { return fkoda; }
            set { SetPropertyValue<string>("koda", ref fkoda, value); }
        }
        string fSkupina;
        [Size(255)]
        public string Skupina
        {
            get { return fSkupina; }
            set { SetPropertyValue<string>("Skupina", ref fSkupina, value); }
        }
        double fP_1;
        [Persistent(@"1")]
        public double P_1
        {
            get { return fP_1; }
            set { SetPropertyValue<double>("P_1", ref fP_1, value); }
        }
        double fP_2;
        [Persistent(@"2")]
        public double P_2
        {
            get { return fP_2; }
            set { SetPropertyValue<double>("P_2", ref fP_2, value); }
        }
        double fP_3;
        [Persistent(@"3")]
        public double P_3
        {
            get { return fP_3; }
            set { SetPropertyValue<double>("P_3", ref fP_3, value); }
        }
        double fP_4;
        [Persistent(@"4")]
        public double P_4
        {
            get { return fP_4; }
            set { SetPropertyValue<double>("P_4", ref fP_4, value); }
        }
        double fP_5;
        [Persistent(@"5")]
        public double P_5
        {
            get { return fP_5; }
            set { SetPropertyValue<double>("P_5", ref fP_5, value); }
        }
        double fP_6;
        [Persistent(@"6")]
        public double P_6
        {
            get { return fP_6; }
            set { SetPropertyValue<double>("P_6", ref fP_6, value); }
        }
        double fP_7;
        [Persistent(@"7")]
        public double P_7
        {
            get { return fP_7; }
            set { SetPropertyValue<double>("P_7", ref fP_7, value); }
        }
        double fP_8;
        [Persistent(@"8")]
        public double P_8
        {
            get { return fP_8; }
            set { SetPropertyValue<double>("P_8", ref fP_8, value); }
        }
        double fP_9;
        [Persistent(@"9")]
        public double P_9
        {
            get { return fP_9; }
            set { SetPropertyValue<double>("P_9", ref fP_9, value); }
        }
        double fP_10;
        [Persistent(@"10")]
        public double P_10
        {
            get { return fP_10; }
            set { SetPropertyValue<double>("P_10", ref fP_10, value); }
        }
        double fP_11;
        [Persistent(@"11")]
        public double P_11
        {
            get { return fP_11; }
            set { SetPropertyValue<double>("P_11", ref fP_11, value); }
        }
        double fP_12;
        [Persistent(@"12")]
        public double P_12
        {
            get { return fP_12; }
            set { SetPropertyValue<double>("P_12", ref fP_12, value); }
        }
    }

}
