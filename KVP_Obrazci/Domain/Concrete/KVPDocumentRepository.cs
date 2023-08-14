using DevExpress.Data.Filtering;
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
using System.Text;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class KVPDocumentRepository : IKVPDocumentRepository
    {
        Session session;
        IStatusRepository statusRepo;
        IKVPStatusRepository kvpStatusRepo;
        IEmployeeRepository employeeRepo;
        IPayoutsRepository payoutRepo;
        IPlaningAndRealizationRepository planRealRepo;
        ICompanySettingsRepository companySettingsRepo;
        IKVPGroupsRepository kvpGroupUser;


        public KVPDocumentRepository(Session session)
        {
            this.session = session;
            statusRepo = new StatusRepository(session);
            kvpStatusRepo = new KVPStatusRepository(session);
            employeeRepo = new EmployeeRepository(session);
            payoutRepo = new PayoutsRepository(session);
            planRealRepo = new PlaningAndRealizationRepository(session);
            companySettingsRepo = new CompanySettingsRepository(session);
            kvpGroupUser = new KVPGroupsRepository(session);
        }

        public KVPDocument GetKVPByID(int kvpID, Session currentSession = null)
        {
            try
            {
                XPQuery<KVPDocument> documents = null;


                if (currentSession != null)
                    documents = currentSession.Query<KVPDocument>();
                else
                    documents = session.Query<KVPDocument>();

                return documents.Where(kvp => kvp.idKVPDocument == kvpID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        /* public int SaveKVP(KVPDocument model, bool submitProposalDirect = false)
         {
             try
             {
                 model.DatumSpremembe = DateTime.Now;
                 if (model.idKVPDocument == 0 && submitProposalDirect)
                 {
                     model.ts = DateTime.Now;
                     ChangeStatusOnKVPDocument(model, Enums.KVPStatuses.ODOBRITEV_VODJA, true);
                     model.StevilkaKVP = DateTime.Now.Year.ToString() + " " + model.idKVPDocument.ToString();

                     planRealRepo.UpdatePlanRealizationByKvpSubmit(model.Predlagatelj.Id, DateTime.Now.Year, DateTime.Now.Month, 1);

                     model.Save();
                 }
                 else if (model.idKVPDocument > 0 && submitProposalDirect)
                 {
                     ChangeStatusOnKVPDocument(model.idKVPDocument, Enums.KVPStatuses.ODOBRITEV_VODJA, true);
                     planRealRepo.UpdatePlanRealizationByKvpSubmit(model.Predlagatelj.Id, DateTime.Now.Year, DateTime.Now.Month, 1);
                 }
                 else if (model.idKVPDocument == 0 && !submitProposalDirect)
                 {
                     model.ts = DateTime.Now;
                     ChangeStatusOnKVPDocument(model, Enums.KVPStatuses.VNOS, true);
                     model.StevilkaKVP = DateTime.Now.Year.ToString() + " " + model.idKVPDocument.ToString();
                     model.Save();
                 }
                 else
                     model.Save();

                 return model.idKVPDocument;
             }
             catch (Exception ex)
             {
                 string error = "";
                 CommonMethods.getError(ex, ref error);
                 throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
             }
         }*/

        public int SaveKVP(KVPDocument model, Enums.SubmitProposalType kvpProposalTypeSubmit = Enums.SubmitProposalType.OnlySaveProposal, bool transferToRedCard = false)
        {
            try
            {
                bool isNewKVP = (model.idKVPDocument == 0);

                model.DatumSpremembe = DateTime.Now;
                if (kvpProposalTypeSubmit == Enums.SubmitProposalType.OnlySaveProposal)
                {
                    if (isNewKVP)
                    {
                        model.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                        model.ts = DateTime.Now;

                        if (!transferToRedCard)
                        {
                            ChangeStatusOnKVPDocument(model, Enums.KVPStatuses.VNOS, true, model.Session);
                            model.ZaporednaStevilka = GetNewKVPNumber();
                            model.StevilkaKVP = DateTime.Now.Year.ToString() + " " + model.ZaporednaStevilka;
                        }
                        else
                        {
                            ChangeStatusOnRedCardDocument(model, Enums.RedCardStatus.ODPRTO, true, model.Session);
                            model.ZaporednaStevilka = GetNewRedCardNumber();
                            model.StevilkaKVP = "E- " + DateTime.Now.Year.ToString() + " " + model.ZaporednaStevilka;
                            model.PrenosRKizKVP = true;
                            model.RKVnesel = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, model.Session);
                            model.DatumVnosa = DateTime.Now;
                            model.idTipRdeciKarton = GetRedCardTypeByID(1, model.Session);//Manjse popravilo 
                        }
                    }

                    model.Save();
                }
                else if (kvpProposalTypeSubmit == Enums.SubmitProposalType.SubmitProposalToChampion)
                {
                    ChangeStatusOnKVPDocument(model, Enums.KVPStatuses.V_PREVERJANJE, true, model.Session);
                    planRealRepo.UpdatePlanRealizationByKvpSubmit(model.Predlagatelj.Id, DateTime.Now.Year, DateTime.Now.Month, 1);

                    if (isNewKVP)
                    {
                        model.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                        model.ts = DateTime.Now;
                        model.ZaporednaStevilka = GetNewKVPNumber();
                        model.StevilkaKVP = DateTime.Now.Year.ToString() + " " + model.ZaporednaStevilka;
                        model.Save();
                    }
                }
                else if (kvpProposalTypeSubmit == Enums.SubmitProposalType.SubmitProposalToLeader)
                {
                    ChangeStatusOnKVPDocument(model, Enums.KVPStatuses.ODOBRITEV_VODJA, true, model.Session);

                    if (isNewKVP)//this happens only when Champion is inserting new KVP's than he directly sends proposal to leader
                    {
                        model.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                        model.ts = DateTime.Now;
                        model.ZaporednaStevilka = GetNewKVPNumber();
                        model.StevilkaKVP = DateTime.Now.Year.ToString() + " " + model.ZaporednaStevilka;
                        planRealRepo.UpdatePlanRealizationByKvpSubmit(model.Predlagatelj.Id, DateTime.Now.Year, DateTime.Now.Month, 1);
                        model.Save();
                    }
                }
                else if (kvpProposalTypeSubmit == Enums.SubmitProposalType.SubmitProposalAndReject)
                {
                    ChangeStatusOnKVPDocument(model, Enums.KVPStatuses.ZAVRNJEN, true, model.Session);

                    if (isNewKVP)//this happens only when Champion is inserting new KVP's than he directly sends proposal to leader
                    {
                        model.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                        model.ts = DateTime.Now;
                        model.ZaporednaStevilka = GetNewKVPNumber();
                        model.StevilkaKVP = DateTime.Now.Year.ToString() + " " + model.ZaporednaStevilka;
                        planRealRepo.UpdatePlanRealizationByKvpSubmit(model.Predlagatelj.Id, DateTime.Now.Year, DateTime.Now.Month, 1);
                        model.Save();
                    }
                }

                return model.idKVPDocument;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetNewKVPNumber()
        {

            XPQuery<KVPDocument> documents = session.Query<KVPDocument>();

            int companyStartKVPNum = companySettingsRepo.GetStartKVPNumber();
            //int maxNumInDB = documents.Where(rc => rc.idTipRdeciKarton == null).Max(kvp => kvp.ZaporednaStevilka);            


            KVPDocument kvpDoc = documents.Where(kvp=> kvp.ZaporednaStevilka > 0 && kvp.idTipRdeciKarton == null).OrderByDescending(kvp => kvp.idKVPDocument).FirstOrDefault();

            

            if (kvpDoc != null)
            {

                int maxNumInDB = kvpDoc.ZaporednaStevilka;
                if (kvpDoc.ts.Year != DateTime.Now.Year)
                {
                    maxNumInDB = companyStartKVPNum;
                }

                if (maxNumInDB == companyStartKVPNum)
                {
                    companySettingsRepo.SetNewKVPNumber(maxNumInDB + 1);
                    return maxNumInDB + 1;
                }
                else if (maxNumInDB > companyStartKVPNum)
                {
                    companySettingsRepo.SetNewKVPNumber(maxNumInDB+1);
                    return maxNumInDB + 1;
                }
                else
                {
                    companySettingsRepo.SetNewKVPNumber(companyStartKVPNum + 1);
                    return companyStartKVPNum + 1;
                }
            }
            else return 1;
        }

        public void DeleteKVP(KVPDocument model)
        {
            try
            {
                model.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }
        public void DeleteKVP(int id)
        {
            try
            {
                GetKVPByID(id).Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Users GetEmployeeByID(int employeeID, Session currentSession = null)
        {
            try
            {
                XPQuery<Users> employee = null;

                if (currentSession != null)
                    employee = currentSession.Query<Users>();
                else
                    employee = session.Query<Users>();

                return employee.Where(u => u.Id == employeeID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Tip GetTypeByID(int typeID, Session currentSession = null)
        {
            try
            {
                XPQuery<Tip> type = null;

                if (currentSession != null)
                    type = currentSession.Query<Tip>();
                else
                    type = session.Query<Tip>();

                return type.Where(t => t.idTip == typeID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public TipRdeciKarton GetRedCardTypeByID(int recCardTypeID, Session currentSession = null)
        {
            try
            {
                XPQuery<TipRdeciKarton> typeRedCard = null;

                if (currentSession != null)
                    typeRedCard = currentSession.Query<TipRdeciKarton>();
                else
                    typeRedCard = session.Query<TipRdeciKarton>();

                return typeRedCard.Where(rkt => rkt.idTipRdeciKarton == recCardTypeID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public TipRdeciKarton GetRedCardTypeByCode(string recCardTypeCode, Session currentSession = null)
        {
            try
            {
                XPQuery<TipRdeciKarton> typeRedCard = null;

                if (currentSession != null)
                    typeRedCard = currentSession.Query<TipRdeciKarton>();
                else
                    typeRedCard = session.Query<TipRdeciKarton>();

                return typeRedCard.Where(rkt => rkt.Koda == recCardTypeCode).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public int GetKVPCountByUserID(int userID, Session currentSession = null)
        {
            try
            {
                XPQuery<KVPDocument> kvpDoc = null;

                if (currentSession != null)
                    kvpDoc = currentSession.Query<KVPDocument>();
                else
                    kvpDoc = session.Query<KVPDocument>();

                return kvpDoc.Where(kvp => kvp.Predlagatelj.Id == userID && kvp.idTipRdeciKarton == null).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetKVPRealizedCountByUserID(int userID, Session currentSession = null)
        {
            throw new NotImplementedException();
        }

        public int GetKVPRedCardsCountByUserID(int userID, Session currentSession = null)
        {
            throw new NotImplementedException();
        }


        public void ChangeStatusOnKVPDocument(int kvpID, Enums.KVPStatuses status, bool changeStatusOnKVP = false)
        {
            try
            {
                KVP_Status kvpStatus = new KVP_Status(session);
                KVPDocument doc = GetKVPByID(kvpID);

                if (changeStatusOnKVP)
                {
                    if (doc != null)
                    {
                        doc.LastStatusId = doc.LastStatusId != null ? statusRepo.GetStatusByCode(status.ToString(), doc.LastStatusId.Session) : statusRepo.GetStatusByCode(status.ToString());
                        doc.Save();
                    }
                }

                kvpStatus.idKVP_Status = 0;
                kvpStatus.idKVPDocument = doc;
                kvpStatus.idStatus = statusRepo.GetStatusByCode(status.ToString());
                kvpStatus.Opomba = "";
                kvpStatus.ts = DateTime.Now;
                kvpStatus.IDPrijava = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, kvpStatus.Session);

                kvpStatusRepo.SaveKVPStatus(kvpStatus);
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void ChangeStatusOnKVPDocument(KVPDocument model, Enums.KVPStatuses status, bool changeStatusOnKVP = false, Session currentSession = null)
        {
            try
            {
                if (model != null)
                {
                    if (changeStatusOnKVP)
                    {
                        model.LastStatusId = model.LastStatusId != null ? statusRepo.GetStatusByCode(status.ToString(), model.LastStatusId.Session) : statusRepo.GetStatusByCode(status.ToString());
                        model.Save();//save only once
                    }

                    KVP_Status kvpStatus = new KVP_Status(currentSession != null ? currentSession : session);
                    kvpStatus.idKVP_Status = 0;
                    kvpStatus.idKVPDocument = model;
                    kvpStatus.idStatus = statusRepo.GetStatusByCode(status.ToString(), currentSession != null ? currentSession : null);
                    kvpStatus.Opomba = "";
                    kvpStatus.ts = DateTime.Now;
                    kvpStatus.IDPrijava = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, kvpStatus.Session);

                    if (currentSession == null)
                        kvpStatusRepo.SaveKVPStatus(kvpStatus);
                    else
                        kvpStatus.Save();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Tip GetTypeByCode(string code, Session currentSession = null)
        {
            try
            {
                XPQuery<Tip> type = null;

                if (currentSession != null)
                    type = currentSession.Query<Tip>();
                else
                    type = session.Query<Tip>();

                return type.Where(t => t.Koda == code).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void AutomaticRealizationKVPDocument(List<object> selectedKVPs)
        {
            try
            {

                List<int> sKVPs = selectedKVPs.OfType<int>().ToList();

                using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
                {
                    KVPDocument doc = null;

                    XPQuery<KVPDocument> kvps = uow.Query<KVPDocument>();
                    List<IGrouping<Users, KVPDocument>> groupKVPByProposer = kvps.Where(k => sKVPs.Contains(k.idKVPDocument)).GroupBy(k => k.Predlagatelj).ToList();
                    Status realiziredStatus = statusRepo.GetStatusByCode(Enums.KVPStatuses.REALIZIRANO.ToString(), uow);

                    foreach (var item in groupKVPByProposer)
                    {
                        decimal proposerPoint = item.Select(kvp => kvp.idTip.TockePredlagatelj).Sum();
                        decimal realizatorPoint = 0;

                        DateTime datumRealizacije = DateTime.Now;
                        List<KVPDocument> kvpList = item.Select(k => k).ToList();

                        foreach (var obj in kvpList)
                        {
                            doc = GetKVPByID(obj.idKVPDocument, uow);
                            doc.LastStatusId = realiziredStatus;
                            doc.DatumRealizacije = datumRealizacije;
                        }
                        //Predlagatelj
                        RealizeKVPDocumentOnUser(item.Select(u => u.Predlagatelj).FirstOrDefault(), datumRealizacije, proposerPoint, uow);


                        //Realizator -če imamo različne realizatorje za iste predlagatelje
                        List<IGrouping<Users, KVPDocument>> groupKVPByRealizator = kvpList.GroupBy(kl => kl.Realizator).ToList();
                        foreach (var obj in groupKVPByRealizator)
                        {
                            realizatorPoint = obj.Select(k => k.idTip.TockeRealizator).Sum();
                            RealizeKVPDocumentOnUser(obj.Select(u => u.Realizator).FirstOrDefault(), datumRealizacije, realizatorPoint, uow, true);
                        }
                    }

                    uow.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        private void RealizeKVPDocumentOnUser(Users user, DateTime datumRealizacije, decimal tockeVsota, Session uowSession, bool isRealizator = false)
        {
            string month = CommonMethods.GetDateTimeMonthByNumber(datumRealizacije.Date.Month);

            //Pridobimo izplačila za mesec in leto realizacije
            Izplacila userPayoutRecord = payoutRepo.UserPayoutRecordForMonthAndYear(user.Id, month, datumRealizacije.Date.Year, uowSession);

            if (user.UpravicenDoKVP)
            {
                if (userPayoutRecord != null)
                {
                    decimal vsotaTock = 0;
                    userPayoutRecord.PredlagateljT += !isRealizator ? tockeVsota : 0;
                    userPayoutRecord.RealizatorT += isRealizator ? tockeVsota : 0;
                    vsotaTock = userPayoutRecord.PredlagateljT + userPayoutRecord.RealizatorT + userPayoutRecord.PrenosIzPrejsnjegaMeseca;
                    userPayoutRecord.VsotaT = vsotaTock;
                    userPayoutRecord.ts = DateTime.Now;
                }
                else
                {
                    CreateNewPayout(user, tockeVsota, datumRealizacije, uowSession, isRealizator);
                }
            }
        }

        private void CreateNewPayout(Users user, decimal tocke, DateTime datumRealizacije, Session uowSession, bool isRealizator = false)
        {
            //DateTime previousMonthDate = model.DatumRealizacije.Date.AddMonths(-1);
            //string previousMonth = CommonMethods.GetDateTimeMonthByNumber(previousMonthDate.Date.Month);
            Izplacila previousPayout = payoutRepo.GetLastPayoutByUserID(user.Id);

            //payoutRepo.UserPayoutRecordForMonthAndYear((!isRealizator ? model.Predlagatelj.Id : model.Realizator.Id), previousMonth, previousMonthDate.Year, uowSession);



            Izplacila payout = new Izplacila(uowSession);
            payout.DatumOd = CommonMethods.GetFirstDayOfMonth(datumRealizacije);
            payout.DatumDo = CommonMethods.GetLastDayOfMonth(datumRealizacije);
            //payout.DatumIzracuna
            payout.IdUser = employeeRepo.GetEmployeeByID(user.Id, uowSession);
            payout.IzplaciloVMesecu = 0;
            payout.Leto = datumRealizacije.Date.Year;
            payout.Mesec = CommonMethods.GetDateTimeMonthByNumber(datumRealizacije.Date.Month);
            payout.PredlagateljT = !isRealizator ? tocke : 0;
            payout.PrenosIzPrejsnjegaMeseca = previousPayout != null ? previousPayout.PrenosTvNaslednjiMesec : 0;
            payout.PrenosTvNaslednjiMesec = 0;
            payout.RealizatorT = !isRealizator ? 0 : tocke;
            payout.VsotaT = (payout.PrenosIzPrejsnjegaMeseca + payout.PredlagateljT + payout.RealizatorT);
            payout.ts = DateTime.Now;
            payout.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;

            string sImePriimek = payout.IdUser.Firstname == null ? "" : payout.IdUser.Firstname;
            sImePriimek += " " + (payout.IdUser.Lastname == null ? "" : payout.IdUser.Lastname);

            payout.ImePriimek = sImePriimek;




            //payoutRepo.SavePayout(payout);
        }

        public decimal GetToLeaderConfirmKVPsForUser(int userID)
        {
            try
            {
                XPQuery<KVPDocument> kvpDoc = session.Query<KVPDocument>();

                return kvpDoc.Where(kvp => kvp.vodja_teama == userID && kvp.LastStatusId.Koda == Enums.KVPStatuses.ODOBRITEV_VODJA.ToString()).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public decimal GetRealizedKVPsForUser(int userID)
        {
            try
            {
                XPQuery<KVPDocument> kvpDoc = session.Query<KVPDocument>();

                return kvpDoc.Where(kvp => kvp.Predlagatelj.Id == userID && kvp.LastStatusId.Koda == Enums.KVPStatuses.REALIZIRANO.ToString()).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public decimal GetCompletedKVPsForUser(int userID)
        {
            try
            {
                XPQuery<KVPDocument> kvpDoc = session.Query<KVPDocument>();

                return kvpDoc.Where(kvp => kvp.Predlagatelj.Id == userID && kvp.LastStatusId.Koda == Enums.KVPStatuses.ZAKLJUCENO.ToString()).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public decimal GetNumberOfElapsedDayFromSubmitingKVP(KVPDocument model)
        {
            try
            {
                if (model.KVP_Statuss.Count == 0) return 0;

                DateTime submitDate = model.KVP_Statuss.OrderBy(kvpS => kvpS.ts).FirstOrDefault().ts;
                DateTime endDate = DateTime.Now;
                KVP_Status lastStatus = model.KVP_Statuss.OrderByDescending(kvpS => kvpS.ts).FirstOrDefault();

                if (lastStatus.idStatus.Koda == Enums.KVPStatuses.ZAKLJUCENO.ToString() || lastStatus.idStatus.Koda == Enums.KVPStatuses.ZAVRNJEN.ToString())
                    endDate = lastStatus.ts;
                decimal dDays = CommonMethods.ParseDecimal(Math.Floor((endDate - submitDate).TotalDays));
                if (dDays == 0) { dDays = 1; }
                return dDays;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void AutomaticUpdateStatusKVPDocument(List<object> selectedKVPs, Enums.KVPStatuses kStatus = Enums.KVPStatuses.REALIZIRANO)
        {
            try
            {

                List<int> sKVPs = selectedKVPs.OfType<int>().ToList();

                using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
                {
                    KVPDocument doc = null;

                    XPQuery<KVPDocument> kvps = uow.Query<KVPDocument>();

                    foreach (var item in sKVPs)
                    {
                        doc = kvps.Where(k => k.idKVPDocument == item).FirstOrDefault();
                        if (doc != null)
                        {
                            Status newStatus = statusRepo.GetStatusByCode(kStatus.ToString(), uow);
                            doc.LastStatusId = newStatus;
                            doc.DatumRealizacije = DateTime.Now;

                            KVP_Status kvpStatus = new KVP_Status(uow);
                            kvpStatus.idKVPDocument = doc;
                            kvpStatus.idStatus = newStatus;
                            kvpStatus.Opomba = "";
                            kvpStatus.ts = DateTime.Now;
                            kvpStatus.IDPrijava = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, kvpStatus.Session);
                        }
                    }

                    uow.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public KVPKomentarji GetKVPCommentByID(int commentID, Session currentSession = null)
        {
            try
            {
                XPQuery<KVPKomentarji> comments = null;

                if (currentSession != null)
                    comments = currentSession.Query<KVPKomentarji>();
                else
                    comments = session.Query<KVPKomentarji>();

                return comments.Where(com => com.KVPKomentarjiID == commentID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SaveKVPComment(KVPKomentarji model)
        {
            try
            {

                if (model.KVPKomentarjiID == 0)
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
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteKVPComment(KVPKomentarji comment)
        {
            try
            {
                comment.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteKVPComment(int commentID)
        {
            try
            {
                KVPKomentarji model = GetKVPCommentByID(commentID);

                if (model != null)
                    model.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }




        public void SaveLastStatusOnKVP(KVPDocument model, Status LastStatus, Session currentSession = null)
        {
            try
            {

                if (model != null)
                {
                    model.DatumSpremembe = DateTime.Now;
                    model.LastStatusId = statusRepo.GetStatusByID(LastStatus.idStatus, currentSession);
                }

                model.Save();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetSubmittedKVPCountByUserId(int userId)
        {
            try
            {
                XPQuery<KVPDocument> documents = session.Query<KVPDocument>();
                string statusVnos = Enums.KVPStatuses.VNOS.ToString();
                return documents.Where(kvp => kvp.Predlagatelj.Id == userId && kvp.LastStatusId.Koda != statusVnos).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetSubmittedKVPCountByUserIdAndYear(int userId, int year)
        {
            try
            {
                XPQuery<KVPDocument> documents = session.Query<KVPDocument>();
                string statusVnos = Enums.KVPStatuses.VNOS.ToString();
                return documents.Where(kvp => kvp.Predlagatelj.Id == userId && kvp.DatumVnosa.Year == year && kvp.LastStatusId.Koda != statusVnos).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetNotFinishedKVPsForUser(int userID, Session currentSession = null)
        {
            try
            {
                XPQuery<KVPDocument> kvpDoc = session.Query<KVPDocument>();

                return kvpDoc.Where(kvp => kvp.vodja_teama == userID && kvp.LastStatusId.Koda != Enums.KVPStatuses.ZAKLJUCENO.ToString()).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void ChangeLeaderOnKVPDocument(int CurrentLeaderID, int ChangedLeaderID)
        {
            //XPQuery<KVPDocument> documents = session.Query<KVPDocument>();
            //documents.Where(kvp => kvp.vodja_teama == CurrentLeaderID && (kvp.LastStatusId.Koda != statusRepo.GetStatusByCode(Enums.KVPStatuses.ODOBRITEV_VODJA.ToString())));

            CriteriaOperator filterCriteria = null;
            filterCriteria = CriteriaOperator.Parse("(LastStatusId <> 11 and LastStatusId <> 9 and LastStatusId <> 5) and vodja_teama = ?", CurrentLeaderID);

            XPCollection<KVPDocument> collKVPDoc = new XPCollection<KVPDocument>(session, filterCriteria);
            foreach (KVPDocument doc in collKVPDoc)
            {
                doc.vodja_teama = ChangedLeaderID;


                // shrani opombo zamenjava vodje

                KVPKomentarji comment = new KVPKomentarji(session);

                comment.Koda = Enums.KVPCommentCode.ChangeLeaderNotes.ToString();
                comment.KVPDocId = doc;
                comment.KVPKomentarjiID = 0;
                string sCurentLeader = CommonMethods.GetNameByUser(employeeRepo.GetEmployeeByID(CurrentLeaderID));
                string sChangedLeader = CommonMethods.GetNameByUser(employeeRepo.GetEmployeeByID(ChangedLeaderID));
                comment.Opombe = "Urejanje oddelkov; Menjava vodja: " + sCurentLeader + " za vodjo: " + sChangedLeader;
                comment.UserId = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, session);
                SaveKVPComment(comment);

                doc.Save();
            }
        }

        #region Sync User



        public int ChangePredlagateljOnKVPDocument(int CurrentPredlagateljID, int ChangedPredlagateljID)
        {
            try
            {
                //XPQuery<KVPDocument> documents = session.Query<KVPDocument>();
                //documents.Where(kvp => kvp.vodja_teama == CurrentLeaderID && (kvp.LastStatusId.Koda != statusRepo.GetStatusByCode(Enums.KVPStatuses.ODOBRITEV_VODJA.ToString())));
                int cnt = 0;
                CriteriaOperator filterCriteria = null;
                filterCriteria = CriteriaOperator.Parse("Predlagatelj = ?", CurrentPredlagateljID);

                XPCollection<KVPDocument> collKVPDoc = new XPCollection<KVPDocument>(session, filterCriteria);
                foreach (KVPDocument doc in collKVPDoc)
                {
                    doc.Predlagatelj = employeeRepo.GetEmployeeByID(ChangedPredlagateljID);


                    // shrani opombo sinhronizaciji vodje
                    KVPKomentarji comment = new KVPKomentarji(session);

                    comment.Koda = Enums.KVPCommentCode.SyncPredlagateljNotes.ToString();
                    comment.KVPDocId = doc;
                    comment.KVPKomentarjiID = 0;
                    string sCurentPredlagatelj = CommonMethods.GetNameByUser(employeeRepo.GetEmployeeByID(CurrentPredlagateljID));
                    string sChangedPredlagatelj = CommonMethods.GetNameByUser(employeeRepo.GetEmployeeByID(ChangedPredlagateljID));
                    comment.Opombe = "Sinhronizacija zaposlenih; Menjava predlagatelja: " + sCurentPredlagatelj + " za predlagatelja: " + sChangedPredlagatelj;
                    comment.UserId = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, session);
                    SaveKVPComment(comment);
                    cnt++;
                    doc.Save();
                }
                return cnt;
            }

            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int ChangeRealizatorOnKVPDocument(int CurrentRealizatorID, int ChangedRealizatorID)
        {
            try
            {
                //XPQuery<KVPDocument> documents = session.Query<KVPDocument>();
                //documents.Where(kvp => kvp.vodja_teama == CurrentLeaderID && (kvp.LastStatusId.Koda != statusRepo.GetStatusByCode(Enums.KVPStatuses.ODOBRITEV_VODJA.ToString())));

                CriteriaOperator filterCriteria = null;
                filterCriteria = CriteriaOperator.Parse("Realizator = ?", CurrentRealizatorID);
                int cnt = 0;
                XPCollection<KVPDocument> collKVPDoc = new XPCollection<KVPDocument>(session, filterCriteria);
                foreach (KVPDocument doc in collKVPDoc)
                {
                    doc.Realizator = employeeRepo.GetEmployeeByID(ChangedRealizatorID);


                    // shrani opombo sinhronizaciji vodje
                    KVPKomentarji comment = new KVPKomentarji(session);

                    comment.Koda = Enums.KVPCommentCode.SyncRealizatorNotes.ToString();
                    comment.KVPDocId = doc;
                    comment.KVPKomentarjiID = 0;
                    string sCurentRealizator = CommonMethods.GetNameByUser(employeeRepo.GetEmployeeByID(CurrentRealizatorID));
                    string sChangedRealizator = CommonMethods.GetNameByUser(employeeRepo.GetEmployeeByID(ChangedRealizatorID));
                    comment.Opombe = "Sinhronizacija zaposlenih; Menjava Realizatorja: " + sCurentRealizator + " za realizatorja: " + sChangedRealizator;
                    comment.UserId = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, session);
                    SaveKVPComment(comment);
                    cnt++;
                    doc.Save();
                }
                return cnt;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int ChangePresojaOnKVPDocument(int CurrentPresojaID, int ChangedPresojaID)
        {
            try
            {
                //XPQuery<KVPDocument> documents = session.Query<KVPDocument>();
                //documents.Where(kvp => kvp.vodja_teama == CurrentLeaderID && (kvp.LastStatusId.Koda != statusRepo.GetStatusByCode(Enums.KVPStatuses.ODOBRITEV_VODJA.ToString())));
                int cnt = 0;
                CriteriaOperator filterCriteria = null;
                filterCriteria = CriteriaOperator.Parse("Presojevalec = ?", CurrentPresojaID);

                XPCollection<KVPPresoje> collKVPPresoje = new XPCollection<KVPPresoje>(session, filterCriteria);
                foreach (KVPPresoje kvpp in collKVPPresoje)
                {
                    kvpp.Presojevalec = employeeRepo.GetEmployeeByID(ChangedPresojaID);
                    kvpp.ts = DateTime.Now;
                    kvpp.Save();

                    KVPDocument doc = GetKVPByID(kvpp.idKVPDocument.idKVPDocument);

                    // shrani opombo sinhronizaciji vodje
                    KVPKomentarji comment = new KVPKomentarji(session);

                    comment.Koda = Enums.KVPCommentCode.SyncPresojaNotes.ToString();
                    comment.KVPDocId = doc;
                    comment.KVPKomentarjiID = 0;
                    string sCurentPresoja = CommonMethods.GetNameByUser(employeeRepo.GetEmployeeByID(CurrentPresojaID));
                    string sChangedPresoja = CommonMethods.GetNameByUser(employeeRepo.GetEmployeeByID(ChangedPresojaID));
                    comment.Opombe = "Sinhronizacija zaposlenih; Menjava Presoja: " + sCurentPresoja + " za Presoja: " + sChangedPresoja;
                    comment.UserId = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, session);
                    SaveKVPComment(comment);
                    cnt++;
                    doc.Save();
                }




                return cnt;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
            return 0;
        }

        public int SyncPayment(int CurrentUserID, int ChangedUserID)
        {
            try
            {
                CriteriaOperator filterCriteria = null;
                filterCriteria = CriteriaOperator.Parse("IdUser = ?", CurrentUserID);
                XPCollection<Izplacila> collKVPIzplacila = new XPCollection<Izplacila>(session, filterCriteria);
                collKVPIzplacila.Sorting.Add(new SortProperty("ts", DevExpress.Xpo.DB.SortingDirection.Descending));
                int cntCangedMonth = 0;
                foreach (Izplacila izpl in collKVPIzplacila)
                {

                    Izplacila izplChangedUser = payoutRepo.GetPayoutsForMonthAndYearAndUserId(izpl.Mesec, izpl.Leto, employeeRepo.GetEmployeeByID(ChangedUserID));

                    if (izplChangedUser == null)
                    {

                        Izplacila izpNewUser = new Izplacila(session);

                        izpNewUser.IdUser = employeeRepo.GetEmployeeByID(ChangedUserID);
                        izpNewUser.DatumOd = izpl.DatumOd;
                        izpNewUser.DatumDo = izpl.DatumDo;
                        izpNewUser.Mesec = izpl.Mesec;
                        izpNewUser.Leto = izpl.Leto;
                        izpNewUser.PredlagateljT = izpl.PredlagateljT;
                        izpNewUser.RealizatorT = izpl.RealizatorT;
                        izpNewUser.VsotaT = izpl.VsotaT;
                        izpNewUser.IzplaciloVMesecu = izpl.IzplaciloVMesecu;
                        izpNewUser.PrenosTvNaslednjiMesec = izpl.PrenosTvNaslednjiMesec;
                        izpNewUser.DatumIzracuna = izpl.DatumIzracuna;
                        izpNewUser.IDPrijave = izpl.IDPrijave;
                        izpNewUser.ts = izpl.ts;
                        izpNewUser.PrenosIzPrejsnjegaMeseca = izpl.PrenosIzPrejsnjegaMeseca;
                        izpNewUser.SyncEmployee = true;
                        izpNewUser.SyncDate = DateTime.Now;
                        izpNewUser.Save();
                        cntCangedMonth++;
                    }
                    else
                    {

                        izplChangedUser.PredlagateljT += izpl.PredlagateljT;
                        izplChangedUser.RealizatorT += izpl.RealizatorT;
                        izplChangedUser.VsotaT += izpl.VsotaT;
                        izplChangedUser.PrenosTvNaslednjiMesec += izpl.PrenosTvNaslednjiMesec;
                        izplChangedUser.SyncEmployee = true;

                        izplChangedUser.SyncDate = DateTime.Now;
                        izplChangedUser.Save();
                        cntCangedMonth++;

                    }

                    izpl.SyncEmployee = true;
                    izpl.SyncDate = DateTime.Now;
                    izpl.Save();
                }

                return cntCangedMonth;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }

            return 0;
        }


        public int ChangeKVPSkupinaAndDepartmentSync(int SourceUserID, int DestinationUserID)
        {
            try
            {
                Users usrSource = employeeRepo.GetEmployeeByID(SourceUserID);
                Users usrDestination = employeeRepo.GetEmployeeByID(DestinationUserID);

                if ((usrSource != null) && (usrDestination != null))
                {
                    usrDestination.DepartmentId = usrSource.DepartmentId;
                }

                KVPSkupina_Users obj = kvpGroupUser.GetKVPGroupUserByUserID(SourceUserID);

                if (obj != null)
                {
                    obj.IdUser = usrDestination;
                    obj.Save();
                }
                usrDestination.Save();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }

            return 0;
        }



        #endregion

        public KVPInfoUserModel GetKVPDocForMonthByGroupID(int idKVPGroup, Session currentSession = null, bool onlyCompletedKVP = false)
        {
            try
            {
                XPQuery<KVPDocument> documents = null;

                DateTime dateFrom = CommonMethods.GetFirstDayOfMonth(DateTime.Now);
                DateTime dateTo = CommonMethods.GetLastDayOfMonth(DateTime.Now).AddHours(23).AddMinutes(59).AddSeconds(59);

                if (currentSession != null)
                    documents = currentSession.Query<KVPDocument>();
                else
                    documents = session.Query<KVPDocument>();


                var query = from doc in documents //join groupUser in groupsUsers on doc.Predlagatelj.Id equals groupUser.IdUser.Id
                            where (onlyCompletedKVP ? (doc.DatumZakljuceneIdeje >= dateFrom && doc.DatumZakljuceneIdeje <= dateTo) : (doc.DatumVnosa >= dateFrom && doc.DatumVnosa <= dateTo)) &&
                            doc.KVPSkupinaID.idKVPSkupina == idKVPGroup
                            select doc;

                decimal kvpPlan = planRealRepo.GetPlanNumberByKVPGroupMonthAndYear(idKVPGroup, DateTime.Now.Month, DateTime.Now.Year);
                int podaniKVP = query.Count();
                decimal odstotek = Math.Round((podaniKVP / kvpPlan) * 100, 2);

                return new KVPInfoUserModel() { PodaniKVP = podaniKVP, PlanKVP = kvpPlan, OdstotekKVP = odstotek };
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        #region Red cards

        public int SaveRC(KVPDocument model, Enums.SubmitRedCardType redCardProposalTypeSubmit = Enums.SubmitRedCardType.OnlySaveProposal, bool isNumberingTypeSystem = true)
        {
            try
            {
                bool isNewRC = (model.idKVPDocument == 0);

                model.DatumSpremembe = DateTime.Now;
                if (redCardProposalTypeSubmit == Enums.SubmitRedCardType.OnlySaveProposal)
                {
                    if (isNewRC)
                    {
                        model.RKVnesel = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, model.Session);
                        model.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                        model.ts = DateTime.Now;
                        ChangeStatusOnRedCardDocument(model, Enums.RedCardStatus.ODPRTO, true, model.Session);

                        if (isNumberingTypeSystem)
                        {
                            model.ZaporednaStevilka = GetNewRedCardNumber();
                            model.StevilkaKVP = "E- " + DateTime.Now.Year.ToString() + " " + model.ZaporednaStevilka;
                        }
                    }

                    model.Save();
                }
                else if (redCardProposalTypeSubmit == Enums.SubmitRedCardType.SaveNewStatus)
                {
                    Enums.RedCardStatus rcStatus;
                    Enum.TryParse(model.LastStatusId.Koda, out rcStatus);

                    ChangeStatusOnRedCardDocument(model, rcStatus, false, model.Session);

                    if (isNewRC)
                    {
                        model.RKVnesel = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, model.Session);
                        model.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                        model.ts = DateTime.Now;

                        if (isNumberingTypeSystem)
                        {
                            model.ZaporednaStevilka = GetNewRedCardNumber();
                            model.StevilkaKVP = "E- " + DateTime.Now.Year.ToString() + " " + model.ZaporednaStevilka;
                        }
                    }

                    model.Save();
                }

                return model.idKVPDocument;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public void ChangeStatusOnRedCardDocument(int kvpID, Enums.RedCardStatus status, bool changeStatusOnRedCard = false)
        {
            try
            {
                KVP_Status kvpStatus = new KVP_Status(session);
                KVPDocument doc = GetKVPByID(kvpID);

                if (changeStatusOnRedCard)
                {
                    if (doc != null)
                    {
                        doc.LastStatusId = doc.LastStatusId != null ? statusRepo.GetStatusByCode(status.ToString(), doc.LastStatusId.Session) : statusRepo.GetStatusByCode(status.ToString());
                        doc.Save();
                    }
                }

                kvpStatus.idKVP_Status = 0;
                kvpStatus.idKVPDocument = doc;
                kvpStatus.idStatus = statusRepo.GetStatusByCode(status.ToString());
                kvpStatus.Opomba = "";
                kvpStatus.ts = DateTime.Now;
                kvpStatus.IDPrijava = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, kvpStatus.Session);

                kvpStatusRepo.SaveKVPStatus(kvpStatus);
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void ChangeStatusOnRedCardDocument(KVPDocument model, Enums.RedCardStatus status, bool changeStatusOnRedCard = false, Session currentSession = null)
        {
            try
            {
                if (model != null)
                {
                    if (changeStatusOnRedCard)
                    {
                        model.LastStatusId = model.LastStatusId != null ? statusRepo.GetStatusByCode(status.ToString(), model.LastStatusId.Session) : statusRepo.GetStatusByCode(status.ToString());
                        model.Save();//save only once
                    }

                    KVP_Status kvpStatus = new KVP_Status(currentSession != null ? currentSession : session);
                    kvpStatus.idKVP_Status = 0;
                    kvpStatus.idKVPDocument = model;
                    kvpStatus.idStatus = statusRepo.GetStatusByCode(status.ToString(), currentSession != null ? currentSession : null);
                    kvpStatus.Opomba = "";
                    kvpStatus.ts = DateTime.Now;
                    kvpStatus.IDPrijava = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, kvpStatus.Session);

                    if (currentSession == null)
                        kvpStatusRepo.SaveKVPStatus(kvpStatus);
                    else
                        kvpStatus.Save();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetNewRedCardNumber()
        {

            XPQuery<KVPDocument> documents = session.Query<KVPDocument>();

            int companyStartKVPNum = companySettingsRepo.GetStartRedCardNumber();
            //int maxNumInDB = documents.Where(rc => rc.idTipRdeciKarton != null).Max(rc => rc.ZaporednaStevilka);

            KVPDocument kvpDoc = documents.Where(kvp => kvp.ZaporednaStevilka > 0 && kvp.idTipRdeciKarton != null).OrderByDescending(kvp => kvp.idKVPDocument).FirstOrDefault();



            if (kvpDoc != null)
            {

                int maxNumInDB = kvpDoc.ZaporednaStevilka;
                if (kvpDoc.ts.Year != DateTime.Now.Year)
                {
                    maxNumInDB = companyStartKVPNum;
                }

                if (maxNumInDB == companyStartKVPNum)
                {
                    companySettingsRepo.SetNewRedCardNumber(maxNumInDB + 1);
                    return maxNumInDB + 1;
                }
                else if (maxNumInDB > companyStartKVPNum)
                {
                    companySettingsRepo.SetNewRedCardNumber(maxNumInDB+1);
                    return maxNumInDB + 1;
                }
                else
                {
                    companySettingsRepo.SetNewRedCardNumber(companyStartKVPNum + 1);
                    return companyStartKVPNum + 1;
                }
            }
            else return 1;
        }

        public decimal GetNumberOfElapsedDayFromSubmitingRC(KVPDocument model)
        {
            try
            {
                if (model.KVP_Statuss.Count == 0) return 0;

                DateTime submitDate = model.KVP_Statuss.OrderBy(kvpS => kvpS.ts).FirstOrDefault().ts;
                DateTime endDate = DateTime.Now;
                KVP_Status lastStatus = model.KVP_Statuss.OrderByDescending(kvpS => kvpS.ts).FirstOrDefault();

                if (lastStatus.idStatus.Koda == Enums.RedCardStatus.IZVRSEN.ToString())
                    endDate = lastStatus.ts;
                decimal dDays = CommonMethods.ParseDecimal(Math.Floor((endDate - submitDate).TotalDays));
                if (dDays == 0) { dDays = 1; }
                return dDays;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool ExistManualRCNumber(string rcNumber)
        {
            try
            {
                XPQuery<KVPDocument> documents = session.Query<KVPDocument>();
                var query = documents.Where(rc => rc.StevilkaKVP == rcNumber).FirstOrDefault();

                return (query == null ? false : true);
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public decimal GetCompletedRCsForUser(int userID)
        {
            try
            {
                XPQuery<KVPDocument> kvpDoc = session.Query<KVPDocument>();

                return kvpDoc.Where(kvp => kvp.Predlagatelj.Id == userID && kvp.LastStatusId.Koda == Enums.RedCardStatus.IZVRSEN.ToString()).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_22, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetRCCountByUserID(int userID, Session currentSession = null)
        {
            try
            {
                XPQuery<KVPDocument> kvpDoc = null;

                if (currentSession != null)
                    kvpDoc = currentSession.Query<KVPDocument>();
                else
                    kvpDoc = session.Query<KVPDocument>();

                return kvpDoc.Where(kvp => kvp.Predlagatelj.Id == userID && kvp.idTipRdeciKarton != null).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void AutomaticOverDueRKChangeStatus()
        {
            try
            {

                using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
                {

                    XPQuery<KVPDocument> kvps = uow.Query<KVPDocument>();
                    List<KVPDocument> overDueDocs = kvps.Where(k => k.RokOdziva < DateTime.Now && k.idTipRdeciKarton != null).ToList();

                    foreach (var item in overDueDocs)
                    {
                        ChangeStatusOnRedCardDocument(item, Enums.RedCardStatus.CEZ_TERMIN, true, uow);
                    }

                    uow.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        #endregion
    }
}