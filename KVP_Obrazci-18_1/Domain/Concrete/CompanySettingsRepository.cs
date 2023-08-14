using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class CompanySettingsRepository : ICompanySettingsRepository
    {
        Session session;

        public CompanySettingsRepository(Session session)
        {
            this.session = session;
        }

        public Nastavitve GetCompanySettings()
        {
            try 
            {
                XPQuery<Nastavitve> settings = null;

                settings = session.Query<Nastavitve>();

                return settings.OrderByDescending(n => n.ts).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_34, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public void SaveCompanySettings(Nastavitve model)
        {
            try
            {
                if (model.NastavitveID == 0)
                {
                    model.ts = DateTime.Now;
                    model.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                }

                model.Save();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_35, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool IsEmailSendingEnabled()
        {
            try {
                XPQuery<Nastavitve> settings = null;

                settings = session.Query<Nastavitve>();

                Nastavitve set = settings.OrderByDescending(n => n.ts).FirstOrDefault();

                if (set != null)
                    return set.MailPosiljanje;

                return false;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_34, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public decimal GetPayoutAmount()
        {
            try
            {
                XPQuery<Nastavitve> settings = null;

                settings = session.Query<Nastavitve>();

                Nastavitve set = settings.OrderByDescending(n => n.ts).FirstOrDefault();

                if (set != null)
                    return set.Izplacilo;

                return 0;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_34, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public decimal GetQuotientAmount()
        {
            try
            {
                XPQuery<Nastavitve> settings = null;

                settings = session.Query<Nastavitve>();

                Nastavitve set = settings.OrderByDescending(n => n.ts).FirstOrDefault();

                if (set != null)
                    return set.Kolicnik;

                return 0;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_34, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetStartKVPNumber()
        {
            try
            {
                XPQuery<Nastavitve> settings = null;

                settings = session.Query<Nastavitve>();

                Nastavitve set = settings.OrderByDescending(n => n.ts).FirstOrDefault();

                if (set != null)
                    return set.StevilkaKVP;

                return 0;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_34, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SetNewKVPNumber(int kvpNum)
        {
            try
            {
                Nastavitve model = GetCompanySettings();

                if (model != null)
                {
                    model.StevilkaKVP = kvpNum;
                    model.Save();
                }                
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_35, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetAndSetNewKVPNumber()
        { 
            try
            {
                Nastavitve model = GetCompanySettings();

                if (model != null)
                {
                    int num = model.StevilkaKVP + 1;
                    model.StevilkaKVP = num;
                    model.Save();

                    return num;
                }
                else
                {
                    throw new Exception("Osnovni podatki o podjetju niso nastavljeni!");
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_35, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}