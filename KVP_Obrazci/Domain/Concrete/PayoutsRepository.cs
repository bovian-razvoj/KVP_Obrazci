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

        public Izplacila GetPayoutsForMonthAndYearAndUserId(string month, int year, Users usrUser)
        {
            try
            {
                XPQuery<Izplacila> payouts = session.Query<Izplacila>();

                return payouts.Where(p => p.Mesec == month && p.Leto == year && p.IdUser == usrUser).FirstOrDefault();
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

        public void SavePayoutsForNewMonth(List<Izplacila> payouts, DateTime date, bool bAddBlankRecord, bool bSetIzplacilo = false)
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

                            payout.NePreneseneTocke = item.NePreneseneTocke;

                            payout.PrenosIzPrejsnjegaMeseca = item.PrenosTvNaslednjiMesec;
                            payout.VsotaT = payout.PrenosIzPrejsnjegaMeseca;

                            if (payout.VsotaT >= minimalPaymentPoints)
                            {
                                decimal paymentMultiplicator = payout.VsotaT / minimalPaymentPoints;
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
                            payout.IzplaciloVMesecu = bSetIzplacilo ? euroPayout : 0;

                            // 26.11.2019 - Boris
                            if (bAddBlankRecord)
                            {
                                payout.VsotaT = item.VsotaT;
                                payout.PrenosTvNaslednjiMesec = 0;
                            }
                            
                            payout.Notes = item.Notes != null ? item.Notes : "";

                            string sImePriimek = payout.IdUser.Firstname == null ? "" : payout.IdUser.Firstname;
                            sImePriimek += " " + (payout.IdUser.Lastname == null ? "" : payout.IdUser.Lastname);

                            payout.ImePriimek = sImePriimek;

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
                        // preveri, če ima iz prešnjega meseca prenesene točke = 0
                        if (payout.PrenosIzPrejsnjegaMeseca == 0)
                        {
                            Izplacila previousPayout = GetLastPayoutByUserIDAndPayoutDate(payout.IdUser.Id);
                            payout.PrenosIzPrejsnjegaMeseca = previousPayout != null ? previousPayout.PrenosTvNaslednjiMesec : 0;
                            payout.VsotaT += payout.PrenosIzPrejsnjegaMeseca;
                        }

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

        public Izplacila GetLastPayoutByUserIDAndPayoutDate(int userID)
        {
            try
            {
                XPQuery<Izplacila> payouts = session.Query<Izplacila>();

                return payouts.Where(p => p.IdUser.Id == userID && p.PrenosIzPrejsnjegaMeseca>0).OrderByDescending(iz => iz.DatumIzracuna).FirstOrDefault();
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
                    newPayout.PredlagateljT = 0;

                    //Če se je uporabnik prijavil pred generiranjem izplačil za prejšnji mesec je potrebno preveriti in sešteti vse potrebne točke in jih prenesti v trenutni mesec
                    if (previousPayout != null && previousPayout.DatumIzracuna == DateTime.MinValue)
                    {
                        decimal paymentMultiplicator = previousPayout.VsotaT / minimalPaymentPoints;
                        int payment = CommonMethods.ParseInt(Math.Floor(paymentMultiplicator));
                        decimal remainigPoints = (paymentMultiplicator - payment);
                        
                        newPayout.PrenosIzPrejsnjegaMeseca = remainigPoints * minimalPaymentPoints;
                    }
                    else
                        newPayout.PrenosIzPrejsnjegaMeseca = previousPayout != null ? (previousPayout.PrenosTvNaslednjiMesec) : 0;

                    newPayout.PrenosTvNaslednjiMesec = 0;
                    newPayout.RealizatorT = 0;
                    newPayout.VsotaT = (newPayout.PrenosIzPrejsnjegaMeseca + newPayout.PredlagateljT + newPayout.RealizatorT);
                    newPayout.ts = dateNow;
                    newPayout.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;

                    string sImePriimek = newPayout.IdUser.Firstname == null ? "" : newPayout.IdUser.Firstname;
                    sImePriimek += " " + (newPayout.IdUser.Lastname == null ? "" : newPayout.IdUser.Lastname);

                    newPayout.ImePriimek = sImePriimek;

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

        public List<PayoutOverviewByPeriodModel> GetPayoutsByPeriod(DateTime firstDate, DateTime lastDate, string employee)
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

                List<Izplacila> list = new List<Izplacila>();

                if(String.IsNullOrEmpty(employee))
                    list = payouts.Where(p => months.Contains(p.Mesec) && years.Contains(p.Leto)).ToList();
                else
                    list = payouts.Where(p => months.Contains(p.Mesec) && years.Contains(p.Leto) && p.ImePriimek.Equals(employee) ).ToList();

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

        public void CreateNewPayout(KVPDocument model, bool isRealizator = false)
        {
            Izplacila payout = null;

            if (model.DatumZakljuceneIdeje < new DateTime(2000, 1, 1)) model.DatumZakljuceneIdeje = DateTime.Now;

            DateTime previousMonthDate = new DateTime(2000, 1, 1);
            string previousMonth = "";
            // nazaj za 1 mesec
            previousMonthDate = model.DatumZakljuceneIdeje.Date.AddMonths(-1);
            previousMonth = CommonMethods.GetDateTimeMonthByNumber(previousMonthDate.Date.Month);
            Izplacila payoutPreviousMonth = null;
            payoutPreviousMonth = UserPayoutRecordForMonthAndYear((!isRealizator ? model.Predlagatelj.Id : model.Realizator.Id), previousMonth, previousMonthDate.Year);

            if (payoutPreviousMonth == null)
            {
                // nazaj za 2 meseca
                previousMonthDate = model.DatumZakljuceneIdeje.Date.AddMonths(-2);
                previousMonth = CommonMethods.GetDateTimeMonthByNumber(previousMonthDate.Date.Month);
                payoutPreviousMonth = UserPayoutRecordForMonthAndYear((!isRealizator ? model.Predlagatelj.Id : model.Realizator.Id), previousMonth, previousMonthDate.Year);
                if (payoutPreviousMonth == null)
                {
                    // nazaj za 3 mesece
                    previousMonthDate = model.DatumZakljuceneIdeje.Date.AddMonths(-3);
                    previousMonth = CommonMethods.GetDateTimeMonthByNumber(previousMonthDate.Date.Month);
                    payoutPreviousMonth = UserPayoutRecordForMonthAndYear((!isRealizator ? model.Predlagatelj.Id : model.Realizator.Id), previousMonth, previousMonthDate.Year);
                    if (payoutPreviousMonth != null)
                    {
                        payout = InsertNewPayOut(model, previousMonthDate.AddMonths(1), payoutPreviousMonth, true, isRealizator);
                        SavePayout(payout);
                        payout = InsertNewPayOut(model, previousMonthDate.AddMonths(2), payoutPreviousMonth, true, isRealizator);
                        SavePayout(payout);
                    }
                }
                else
                {
                    payout = InsertNewPayOut(model, previousMonthDate.AddMonths(1), payoutPreviousMonth, true, isRealizator);
                }
            }

            payout = InsertNewPayOut(model, model.DatumZakljuceneIdeje, payoutPreviousMonth, false, isRealizator);
            SavePayout(payout);

        }

        private Izplacila InsertNewPayOut(KVPDocument model, DateTime dtDatumZakljuceneIdeje, Izplacila payoutPreviousMonth, bool bNewRowIzplacila, bool isRealizator = false)
        {
            Izplacila payout = new Izplacila(session);
            payout.DatumOd = CommonMethods.GetFirstDayOfMonth(dtDatumZakljuceneIdeje);
            payout.DatumDo = CommonMethods.GetLastDayOfMonth(dtDatumZakljuceneIdeje);
            //payout.DatumIzracuna
            payout.IdUser = employeeRepo.GetEmployeeByID((!isRealizator ? model.Predlagatelj.Id : model.Realizator.Id));
            payout.IzplaciloVMesecu = 0;
            payout.Leto = dtDatumZakljuceneIdeje.Date.Year;
            payout.Mesec = CommonMethods.GetDateTimeMonthByNumber(dtDatumZakljuceneIdeje.Date.Month);
            payout.PredlagateljT = !isRealizator ? model.idTip.TockePredlagatelj : 0;
            payout.PrenosIzPrejsnjegaMeseca = payoutPreviousMonth != null ? payoutPreviousMonth.PrenosTvNaslednjiMesec : 0;
            payout.PrenosTvNaslednjiMesec = 0;
            payout.RealizatorT = !isRealizator ? 0 : model.idTip.TockeRealizator;
            payout.VsotaT = (payout.PrenosIzPrejsnjegaMeseca + payout.PredlagateljT + payout.RealizatorT);

            if (bNewRowIzplacila)
            {
                payout.PredlagateljT = 0;
                payout.RealizatorT = 0;
                payout.VsotaT = payout.PrenosIzPrejsnjegaMeseca;
                payout.PrenosTvNaslednjiMesec = payout.PrenosIzPrejsnjegaMeseca;
            }

            string sImePriimek = payout.IdUser.Firstname == null ? "" : payout.IdUser.Firstname;
            sImePriimek += " " + (payout.IdUser.Lastname == null ? "" : payout.IdUser.Lastname);

            payout.ImePriimek = sImePriimek;
            // pripravi izplačilo za priimenk in ime

            return payout;
        }
    }
}

