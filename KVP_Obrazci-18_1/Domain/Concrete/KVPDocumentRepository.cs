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

        public int SaveKVP(KVPDocument model, Enums.SubmitProposalType kvpProposalTypeSubmit = Enums.SubmitProposalType.OnlySaveProposal)
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
                        ChangeStatusOnKVPDocument(model, Enums.KVPStatuses.VNOS, true, model.Session);
                        model.ZaporednaStevilka = GetNewKVPNumber();
                        model.StevilkaKVP = DateTime.Now.Year.ToString() + " " + model.ZaporednaStevilka;
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
            int maxNumInDB = documents.Max(kvp => kvp.ZaporednaStevilka);

            if (maxNumInDB == companyStartKVPNum)
            {
                companySettingsRepo.SetNewKVPNumber(maxNumInDB + 1);
                return maxNumInDB + 1; 
            }
            else if (maxNumInDB > companyStartKVPNum)
            {
                companySettingsRepo.SetNewKVPNumber(maxNumInDB);
                return maxNumInDB + 1;
            }
            else
            {
                companySettingsRepo.SetNewKVPNumber(companyStartKVPNum + 1);
                return companyStartKVPNum + 1;
            }
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

                return kvpDoc.Where(kvp => kvp.Predlagatelj.Id == userID).Count();
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
                        RealizeKVPDocumentOnUser(item.Select(u=> u.Predlagatelj).FirstOrDefault() , datumRealizacije, proposerPoint, uow);


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

            //payoutRepo.SavePayout(payout);
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

        public decimal GetNumberOfElapsedDayFromSubmitingKVP(KVPDocument model)
        {
            try
            {
                if (model.KVP_Statuss.Count == 0) return 0; 

                DateTime submitDate = model.KVP_Statuss.OrderBy(kvpS=>kvpS.ts).FirstOrDefault().ts;
                DateTime endDate = DateTime.Now;
                KVP_Status lastStatus = model.KVP_Statuss.OrderByDescending(kvpS=>kvpS.ts).FirstOrDefault();

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

                if(model.KVPKomentarjiID == 0)
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
                
                if(model != null)
                    model.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<KVPGroupReportModel> GetKVPDocForDatePeriodLastStatusAndGroupID(DateTime dtFrom, DateTime dtTo, int idKVPGroup, 
            Enums.KVPStatuses eStatus = Enums.KVPStatuses.VNOS, 
            Session currentSession = null,
            bool onlyCompletedKVP = false)
        {
            try
            {

                XPQuery<KVPDocument> documents = null; 
                XPQuery<KVPSkupina_Users> groupsUsers = null;
                
                if (currentSession != null)
                    documents = currentSession.Query<KVPDocument>();
                else
                    documents = session.Query<KVPDocument>();

                //groupsUsers = session.Query<KVPSkupina_Users>();

                var query = from doc in documents //join groupUser in groupsUsers on doc.Predlagatelj.Id equals groupUser.IdUser.Id
                            where (onlyCompletedKVP ? (doc.DatumZakljuceneIdeje >= dtFrom && doc.DatumZakljuceneIdeje <= dtTo) : (doc.DatumVnosa >= dtFrom && doc.DatumVnosa <= dtTo))
                            select new KVPGroupReportModelPartial
                            {
                                idKVPSkupina = doc.KVPSkupinaID.idKVPSkupina,
                                Koda = doc.KVPSkupinaID.Koda,
                                Naziv = doc.KVPSkupinaID.Naziv,
                                idKVPDocument = doc.idKVPDocument,
                                idUser = doc.Predlagatelj.Id,
                                StatusID = doc.LastStatusId.idStatus,
                                StatusName = doc.LastStatusId.Naziv,
                                User = doc.Predlagatelj.Firstname + " " + doc.Predlagatelj.Lastname
                            };

                List<KVPGroupReportModelPartial> partialList = query.ToList();
                int statusOdprti = statusRepo.GetStatusByCode(Enums.KVPStatuses.VNOS.ToString()).idStatus;
                int statusRealiziran =  statusRepo.GetStatusByCode(Enums.KVPStatuses.REALIZIRANO.ToString()).idStatus;
                int statusZavrnjen =  statusRepo.GetStatusByCode(Enums.KVPStatuses.ZAVRNJEN.ToString()).idStatus;
                int statusZakljucen = statusRepo.GetStatusByCode(Enums.KVPStatuses.ZAKLJUCENO.ToString()).idStatus;
                int statusOdobritevVodja = statusRepo.GetStatusByCode(Enums.KVPStatuses.ODOBRITEV_VODJA.ToString()).idStatus;

                List<KVPGroupReportModel> list = partialList.GroupBy(l => l.idKVPSkupina)
                    .Select(l => new KVPGroupReportModel
                    {
                        idKVPSkupina = l.Key,
                        Koda = l.Select(s => s.Koda).FirstOrDefault(),
                        Naziv = l.Select(s => s.Naziv).FirstOrDefault(),
                        Podani = l.Count(),
                        Odprti = l.Count(s => (s.StatusID != statusZavrnjen) && (s.StatusID != statusZakljucen)),
                        //Podani = l.Count(s => s.StatusID != statusOdprti),
                        Realizirani = l.Count(s => s.StatusID == statusZakljucen),
                        Zavrnjeni = l.Count(k => k.StatusID == statusZavrnjen)
                    }).ToList();

                return list; //documents.Where(kvp => kvp. >= dtFrom && kvp.DatumVnosa <= dtTo).FirstOrDefault();
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
                    model.LastStatusId = statusRepo.GetStatusByID(LastStatus.idStatus,currentSession);
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
    }
}