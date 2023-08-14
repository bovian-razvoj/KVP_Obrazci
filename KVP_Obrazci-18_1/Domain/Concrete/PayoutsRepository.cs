using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class PayoutsRepository : IPayoutsRepository
    {
        private decimal minimalPaymentPoints = 500M;
        private int minimalPayout = 25;

        Session session;
        IEmployeeRepository employeeRepo;
        ICompanySettingsRepository companySetingsRepo;
        public PayoutsRepository(Session session)
        {
            this.session = session;
            employeeRepo = new EmployeeRepository(session);
            companySetingsRepo = new CompanySettingsRepository(session);

            minimalPaymentPoints = companySetingsRepo.GetQuotientAmount();
            minimalPayout = CommonMethods.ParseInt(companySetingsRepo.GetPayoutAmount());
        }

        //REsource item 22,23,24

        public List<Izplacila> GetPayoutsForMonthAndYear(string month, int year)
        {
            try
            {
                XPQuery<Izplacila> payouts = session.Query<Izplacila>();

                return payouts.Where(p => p.Mesec == month && p.Leto == year).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public int SavePayout(Izplacila model)
        {

            try
            {
                if (model.idIzplacilla == 0)
                {
                    model.ts = DateTime.Now;
                    model.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                }

                model.Save();

                return model.idIzplacilla;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_23, error, CommonMethods.GetCurrentMethodName()));
            }

        }

        public void SavePayoutsForNewMonth(List<Izplacila> payouts, DateTime date)
        {
            try
            {
                using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
                {
                    Izplacila payout = null;
                    DateTime firstDay = CommonMethods.GetFirstDayOfMonth(date);
                    DateTime lastDay = CommonMethods.GetLastDayOfMonth(date);
                    int euroPayout = 0;
                    decimal pointsToTransfer = 0;
                    string mesec = CommonMethods.GetDateTimeMonthByNumber(date.Month);

                    foreach (var item in payouts)
                    {
                        Users employee = employeeRepo.GetEmployeeByID(item.IdUser.Id, uow);
                        if (employee.UpravicenDoKVP)
                        {
                            payout = new Izplacila(uow);
                            payout.DatumDo = lastDay;
                            payout.DatumIzracuna = DateTime.Now;
                            payout.DatumOd = firstDay;
                            payout.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                            payout.IdUser = employee;
                            payout.Leto = date.Year;
                            payout.Mesec = mesec;

                            payout.PrenosIzPrejsnjegaMeseca = item.PrenosTvNaslednjiMesec;
                            payout.VsotaT = payout.PrenosIzPrejsnjegaMeseca;

                            if (payout.VsotaT >= minimalPaymentPoints)
                            {
                                decimal paymentMultiplicator = item.VsotaT / minimalPaymentPoints;
                                int payment = CommonMethods.ParseInt(Math.Floor(paymentMultiplicator));
                                decimal remainigPoints = (paymentMultiplicator - payment);

                                pointsToTransfer = remainigPoints * minimalPaymentPoints;
                                euroPayout = payment * minimalPayout;
                            }
                            else
                            {
                                pointsToTransfer = payout.VsotaT;
                            }

                            payout.PrenosTvNaslednjiMesec = pointsToTransfer;
                            payout.IzplaciloVMesecu = euroPayout;

                            payout.PredlagateljT = 0;
                            payout.RealizatorT = 0;
                            payout.ts = DateTime.Now;
                        }
                    }
                    uow.CommitChanges();
                }

            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_23, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool ExistPayoutsForCurrentMonthAndYear(string month, int year)
        {
            try
            {
                XPQuery<Izplacila> payouts = session.Query<Izplacila>();
                return payouts.Where(p => p.Mesec == month && p.Leto == year).FirstOrDefault() != null;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Izplacila UserPayoutRecordForMonthAndYear(int userID, string month, int year, Session currentSession = null)
        {
            try
            {
                XPQuery<Izplacila> payouts = null;

                if (currentSession != null)
                    payouts = currentSession.Query<Izplacila>();
                else
                    payouts = session.Query<Izplacila>();

                return payouts.Where(p => p.IdUser.Id == userID && p.Mesec == month && p.Leto == year).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void UpdatePayoutsForNewMonth(List<Izplacila> payouts, DateTime date)
        {
            try
            {
                using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
                {
                    Izplacila payout = null;
                    int euroPayout = 0;
                    decimal pointsToTransfer = 0;

                    foreach (var item in payouts)
                    {
                        payout = GetPayoutByID(item.idIzplacilla, uow);
                        payout.DatumIzracuna = DateTime.Now;

                        if (payout.VsotaT >= minimalPaymentPoints)
                        {
                            decimal paymentMultiplicator = item.VsotaT / minimalPaymentPoints;
                            int payment = CommonMethods.ParseInt(Math.Floor(paymentMultiplicator));
                            decimal remainigPoints = (paymentMultiplicator - payment);

                            pointsToTransfer = remainigPoints * minimalPaymentPoints;
                            euroPayout = payment * minimalPayout;
                        }
                        else
                        {
                            euroPayout = 0;
                            pointsToTransfer = payout.VsotaT;
                        }

                        payout.PrenosTvNaslednjiMesec = pointsToTransfer;
                        payout.IzplaciloVMesecu = euroPayout;
                    }
                    uow.CommitChanges();
                }

            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_23, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Izplacila GetPayoutByID(int id, Session currentSession)
        {
            try
            {
                XPQuery<Izplacila> payout = null;

                if (currentSession != null)
                    payout = currentSession.Query<Izplacila>();
                else
                    payout = session.Query<Izplacila>();

                return payout.Where(p => p.idIzplacilla == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Izplacila GetLastPayoutByUserID(int userID)
        {
            try
            {
                XPQuery<Izplacila> payouts = session.Query<Izplacila>();

                return payouts.Where(p => p.IdUser.Id == userID).OrderByDescending(iz => iz.ts).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public decimal GetActualPointsForUser(int userID)
        {
            try
            {
                DateTime dateNow = DateTime.Now;
                string month = CommonMethods.GetDateTimeMonthByNumber(dateNow.Month);
                int year = dateNow.Year;
                Izplacila currentMonthPayout = UserPayoutRecordForMonthAndYear(userID, month, year);

                decimal points = 0;
                if (currentMonthPayout != null)
                {
                    points = currentMonthPayout.VsotaT;
                }
                else
                {
                    Izplacila previousPayout = GetLastPayoutByUserID(userID);
                    Izplacila newPayout = new Izplacila(session);

                    newPayout.DatumOd = CommonMethods.GetFirstDayOfMonth(dateNow);
                    newPayout.DatumDo = CommonMethods.GetLastDayOfMonth(dateNow);
                    //payout.DatumIzracuna
                    newPayout.IdUser = employeeRepo.GetEmployeeByID(userID);
                    newPayout.IzplaciloVMesecu = 0;
                    newPayout.Leto = year;
                    newPayout.Mesec = month;
                    newPayout.PredlagateljT = previousPayout != null ? previousPayout.PredlagateljT : 0;
                    newPayout.PrenosIzPrejsnjegaMeseca = previousPayout != null ? previousPayout.PrenosTvNaslednjiMesec : 0;
                    newPayout.PrenosTvNaslednjiMesec = 0;
                    newPayout.RealizatorT = previousPayout != null ? previousPayout.RealizatorT : 0;
                    newPayout.VsotaT = (newPayout.PrenosIzPrejsnjegaMeseca + newPayout.PredlagateljT + newPayout.RealizatorT);
                    newPayout.ts = dateNow;
                    newPayout.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                    points = newPayout.VsotaT;

                    newPayout.Save();
                }
                return points;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<PayoutOverviewByPeriodModel> GetPayoutsByPeriod(DateTime firstDate, DateTime lastDate)
        {
            try
            {
                XPQuery<Izplacila> payouts = session.Query<Izplacila>();
                XPQuery<KVPSkupina_Users> kvpGroupUsers = session.Query<KVPSkupina_Users>();
                DateTime firstDayOfMonth = CommonMethods.GetFirstDayOfMonth(firstDate);
                DateTime lastDayOfMonth = CommonMethods.GetLastDayOfMonth(lastDate);
                List<string> monthsYears = CommonMethods.GetListOfMonths(firstDate, lastDate, true);
                List<string> months = new List<string>();
                List<int> years = new List<int>();

                foreach (var item in monthsYears)
                {
                    string[] split = item.Split('_');
                    months.Add(split[0]);
                    years.Add(CommonMethods.ParseInt(split[1]));
                }

                List<Izplacila> list = payouts.Where(p => months.Contains(p.Mesec) && years.Contains(p.Leto)).ToList();

                //List<Izplacila> list = payouts.Where(p => p.DatumOd.CompareTo(firstDayOfMonth) <= 0 || p.DatumDo.CompareTo(lastDayOfMonth) >= 0).ToList();
                #region previous querys
                /*var query = from p in list
                            group p. by new { p.Mesec, p.Leto } into pM
                            select new PayoutOverviewByPeriodModel
                            {
                                Mesec = pM.Key.Mesec,
                                Leto = pM.Key.Leto,
                                payoutOverviewList = (from s in pM
                                                      select new PayoutModel
                                                      {
                                                          idIzplacila = s.idIzplacilla,
                                                          KVPSkupinaNaziv = kvpGroupUsers.Where(kvpS => kvpS.IdUser.Id == (s.IdUser != null ? s.IdUser.Id : 0)).FirstOrDefault() !=null ? kvpGroupUsers.Where(kvpS => kvpS.IdUser.Id == (s.IdUser != null ? s.IdUser.Id : 0)).FirstOrDefault().idKVPSkupina.Naziv : "Ni dodeljen v KVP skupino",
                                                          Zaposlen = s.IdUser != null ? s.IdUser.Firstname + " "+s.IdUser.Lastname : "",
                                                          PrenosIzprejsnjegaMeseca = s.PrenosIzPrejsnjegaMeseca,
                                                          PredlagateljT = s.PredlagateljT,
                                                          RealizatorT = s.RealizatorT,
                                                          VsotaT = s.VsotaT,
                                                          PrenosVNaslednjiMesec = s.PrenosTvNaslednjiMesec
                                                      }).ToList()
                            };*/
                #endregion

                var query2 = from p in list
                             group p by p.IdUser into pM
                             select new PayoutOverviewByPeriodModel
                             {
                                 UserID = pM.Key.Id,
                                 Zaposlen = pM.Key.Firstname + " " + pM.Key.Lastname,
                                 KVPSkupinaNaziv = kvpGroupUsers.Where(kvpS => kvpS.IdUser.Id == (pM.Key != null ? pM.Key.Id : 0)).FirstOrDefault() != null ? kvpGroupUsers.Where(kvpS => kvpS.IdUser.Id == (pM.Key != null ? pM.Key.Id : 0)).FirstOrDefault().idKVPSkupina.Naziv : "Ni dodeljen v KVP skupino",
                                 payoutOverviewList = (from s in pM
                                                       select new PayoutModel
                                                       {
                                                           idIzplacila = s.idIzplacilla,
                                                           Mesec = s.Mesec,
                                                           Leto = s.Leto,
                                                           PrenosIzprejsnjegaMeseca = s.PrenosIzPrejsnjegaMeseca,
                                                           PredlagateljT = s.PredlagateljT,
                                                           RealizatorT = s.RealizatorT,
                                                           VsotaT = s.VsotaT,
                                                           PrenosVNaslednjiMesec = s.PrenosTvNaslednjiMesec
                                                       }).ToList()
                             };

                return query2.ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}
