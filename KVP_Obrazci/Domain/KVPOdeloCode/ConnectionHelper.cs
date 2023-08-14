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
    public static class ConnectionHelper
    {
        static Type[] persistentTypes = new Type[] {
            typeof(Hierarhija),
            typeof(KVP_Status),
            typeof(KVPSkupina_Users),
            typeof(Oddelek),
            typeof(Status),
            typeof(Tip),
            typeof(TipRdeciKarton),
            typeof(Vloga),
            typeof(Zaposleni),
            typeof(Departments),
            typeof(KVPExcell),
            typeof(KVPPresoje),
            typeof(Users),
            typeof(Izplacila),
            typeof(Realizacija),
            typeof(StPlan),
            typeof(KVPSkupina),
            typeof(PlanRealizacija),
            typeof(zaposleni_tocke),
            typeof(SystemMessageProcessor),
            typeof(SystemEmailMessage),
            typeof(KVPDocumentArh),
            typeof(KVPExcellDokumenti),
            typeof(KVPExellDoc),
            typeof(KVPUvoz15_12),
            typeof(TockeDec2018),
            typeof(Nastavitve),
            typeof(KVPKomentarji),
            typeof(KVPFullReport),
            typeof(KVPGroupReport),
            typeof(PrispevekKategorija),
            typeof(Prispevek),
            typeof(Lokacija),
            typeof(Linija),
            typeof(Stroj),
            typeof(ActiveUser),
            typeof(KVPDocument),
            typeof(ErrorLog)
        };
        public static Type[] GetPersistentTypes()
        {
            Type[] copy = new Type[persistentTypes.Length];
            Array.Copy(persistentTypes, copy, persistentTypes.Length);
            return copy;
        }
#warning We recommend moving the connection string out of your source code (for instance, to a configuration file) to improve your application's maintainability and security.
        public const string ConnectionString = "XpoProvider=MSSqlServer;data source=10.10.10.10;user id=martinp;password=m123.;initial catalog=KVPOdeloTest;Persist Security Info=true";
        public static void Connect(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption, bool threadSafe = false)
        {
            if (threadSafe)
            {
                var provider = XpoDefault.GetConnectionProvider(ConnectionString, autoCreateOption);
                var dictionary = new DevExpress.Xpo.Metadata.ReflectionDictionary();
                dictionary.GetDataStoreSchema(persistentTypes);
                XpoDefault.DataLayer = new ThreadSafeDataLayer(dictionary, provider);
            }
            else
            {
                XpoDefault.DataLayer = XpoDefault.GetDataLayer(ConnectionString, autoCreateOption);
            }
            XpoDefault.Session = null;
        }
        public static DevExpress.Xpo.DB.IDataStore GetConnectionProvider(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
        {
            return XpoDefault.GetConnectionProvider(ConnectionString, autoCreateOption);
        }
        public static DevExpress.Xpo.DB.IDataStore GetConnectionProvider(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
        {
            return XpoDefault.GetConnectionProvider(ConnectionString, autoCreateOption, out objectsToDisposeOnDisconnect);
        }
        public static IDataLayer GetDataLayer(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
        {
            return XpoDefault.GetDataLayer(ConnectionString, autoCreateOption);
        }
    }

}
