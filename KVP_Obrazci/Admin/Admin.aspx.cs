using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Spreadsheet;
using DevExpress.Web.Data;
using DevExpress.Pdf;

namespace KVP_Obrazci.Admin
{
    public partial class Admin : ServerMasterPage
    {
        IKVPDocumentRepository kvpDocRepo;
        IPayoutsRepository payoutRepo;
        IKodeksToEKVPRepository kodeksRepo;
        IEmployeeRepository employeeRepo;
        ILocationRepository locationRepo;
        IErrorLogRepository errorRepo;
        IKVPGroupsRepository kvpGroupRepo = null;
        IMessageProcessorRepository messageRepo;

        Session session = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            //if (!PrincipalHelper.IsUserSuperAdmin() && !PrincipalHelper.IsUserAdmin()) RedirectHome();
            AllowUserWithRole(Enums.UserRole.SuperAdmin, Enums.UserRole.Admin);

            session = XpoHelper.GetNewSession();

            kvpDocRepo = new KVPDocumentRepository(session);
            payoutRepo = new PayoutsRepository(session);
            kodeksRepo = new KodeksToEKVPRepository();
            employeeRepo = new EmployeeRepository();
            locationRepo = new LocationRepository(session);
            kvpGroupRepo = new KVPGroupsRepository(session);
            errorRepo = new ErrorLogRepository(session);
            messageRepo = new MessageProcessorRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { }
            else
            { }
        }


        protected void btnInsertPlan_Click(object sender, EventArgs e)
        {
            XPCollection<StPlan> collection_P = new XPCollection<StPlan>(session);
            foreach (StPlan pl in collection_P)
            {
                PlanRealizacija plaRez = new PlanRealizacija(session);

                if ((pl.koda != null) && (pl.koda.Length > 0))
                {
                    KVPSkupina kvp = GetKVPSkupinaFromKoda(pl.koda);
                    if (kvp != null)
                    {
                        plaRez.idKVPSkupina = kvp;
                        plaRez.Leto = 2018;
                        plaRez.Plan_Jan = Convert.ToDecimal(pl.P_1);
                        plaRez.Plan_Feb = Convert.ToDecimal(pl.P_2);
                        plaRez.Plan_Mar = Convert.ToDecimal(pl.P_3);
                        plaRez.Plan_Apr = Convert.ToDecimal(pl.P_4);
                        plaRez.Plan_Maj = Convert.ToDecimal(pl.P_5);
                        plaRez.Plan_Jun = Convert.ToDecimal(pl.P_6);
                        plaRez.Plan_Jul = Convert.ToDecimal(pl.P_7);
                        plaRez.Plan_Avg = Convert.ToDecimal(pl.P_8);
                        plaRez.Plan_Sep = Convert.ToDecimal(pl.P_9);
                        plaRez.Plan_Okt = Convert.ToDecimal(pl.P_10);
                        plaRez.Plan_Nov = Convert.ToDecimal(pl.P_11);
                        plaRez.Plan_Dec = Convert.ToDecimal(pl.P_12);

                        plaRez.idVnos = 0;
                        plaRez.ts = DateTime.Now;
                    }
                    plaRez.Save();
                }
            }

            XPCollection<Realizacija> collection_R = new XPCollection<Realizacija>(session);
            foreach (Realizacija rea in collection_R)
            {
                if ((rea.koda != null) && (rea.koda.Length > 0))
                {
                    KVPSkupina kvp1 = GetKVPSkupinaFromKoda(rea.koda);
                    if (kvp1 != null)
                    {
                        PlanRealizacija plaRez1 = GetPlanRealizacijaForSkKVPID(kvp1.idKVPSkupina);
                        if (plaRez1 != null)
                        {
                            plaRez1.idKVPSkupina = kvp1;
                            plaRez1.Leto = 2018;

                            plaRez1.Real_Jan = Convert.ToDecimal(rea.P_1);
                            plaRez1.Real_Feb = Convert.ToDecimal(rea.P_2);
                            plaRez1.Real_Mar = Convert.ToDecimal(rea.P_3);
                            plaRez1.Real_Apr = Convert.ToDecimal(rea.P_4);
                            plaRez1.Real_Maj = Convert.ToDecimal(rea.P_5);
                            plaRez1.Real_Jun = Convert.ToDecimal(rea.P_6);
                            plaRez1.Real_Jul = Convert.ToDecimal(rea.P_7);
                            plaRez1.Real_Avg = Convert.ToDecimal(rea.P_8);
                            plaRez1.Real_Sep = Convert.ToDecimal(rea.P_9);
                            plaRez1.Real_Okt = Convert.ToDecimal(rea.P_10);
                            plaRez1.Real_Nov = Convert.ToDecimal(rea.P_11);
                            plaRez1.Real_Dec = Convert.ToDecimal(rea.P_12);

                            plaRez1.idVnos = 0;
                            plaRez1.ts = DateTime.Now;
                        }
                        plaRez1.Save();
                    }

                }
            }

            XPCollection<PlanRealizacija> collection_PR = new XPCollection<PlanRealizacija>(session);

            foreach (PlanRealizacija pr in collection_PR)
            {
                pr.Odst_Jan = Math.Round(pr.Real_Jan / pr.Plan_Jan, 2) * 100;
                pr.Odst_Feb = Math.Round(pr.Real_Feb / pr.Plan_Feb, 2) * 100;
                pr.Odst_Mar = Math.Round(pr.Real_Mar / pr.Plan_Mar, 2) * 100;
                pr.Odst_Apr = Math.Round(pr.Real_Apr / pr.Plan_Apr, 2) * 100;
                pr.Odst_Maj = Math.Round(pr.Real_Maj / pr.Plan_Maj, 2) * 100;
                pr.Odst_Jun = Math.Round(pr.Real_Jun / pr.Plan_Jun, 2) * 100;
                pr.Odst_Jul = Math.Round(pr.Real_Jul / pr.Plan_Jul, 2) * 100;
                pr.Odst_Avg = Math.Round(pr.Real_Avg / pr.Plan_Avg, 2) * 100;
                pr.Odst_Sep = Math.Round(pr.Real_Sep / pr.Plan_Sep, 2) * 100;
                pr.Odst_Okt = Math.Round(pr.Real_Okt / pr.Plan_Okt, 2) * 100;
                pr.Odst_Nov = Math.Round(pr.Real_Nov / pr.Plan_Nov, 2) * 100;
                pr.Odst_Dec = Math.Round(pr.Real_Dec / pr.Plan_Dec, 2) * 100;

                pr.Save();
            }

        }

        private PlanRealizacija GetPlanRealizacijaForSkKVPID(Int32 iIdKVPSkup)
        {

            CriteriaOperator filterCriteria = CriteriaOperator.Parse("idKVPSkupina =" + iIdKVPSkup);

            XPCollection<PlanRealizacija> collection_KVP = new XPCollection<PlanRealizacija>(session, filterCriteria);
            foreach (PlanRealizacija pln in collection_KVP)
            {
                return pln;

            }
            return null;
        }
        private String ReturnMesecByNumber(int iMesec)
        {
            switch (iMesec)
            {
                case 1: return "Januar";
                case 2: return "Februar";
                case 3: return "Marec";
                case 4: return "April";
                case 5: return "Maj";
                case 6: return "Junij";
                case 7: return "Julij";
                case 8: return "Avgust";
                case 9: return "September";
                case 10: return "Oktober";
                case 11: return "November";
                case 12: return "December";
            }

            return "";
        }

        private Int32 ReturnTipIDByNaziv(string sNazivIdeje)
        {
            switch (sNazivIdeje.ToUpper())
            {
                case "VARNOST": return 5;
                case "STROŠKI": return 7;
                case "KVALITETA": return 8;
                case "ZALOGA": return 9;
                case "SPLOŠNO": return 10;
                case "EKOLOGIJA": return 11;
                case "ERGONOMIJA": return 12;
                case "ENERGIJA": return 13;
            }
            return 10;
        }

        protected void btnUpdateTocke_Click(object sender, EventArgs e)
        {
            XPCollection<TockeDec2018> collection_ZT = new XPCollection<TockeDec2018>(session);
            foreach (TockeDec2018 zt in collection_ZT)
            {

                int idZaposlen = Convert.ToInt32(zt.Koda);

                Users cUsrR = GetUsersFromExternalID(idZaposlen);

                if (cUsrR == null)
                {
                    cUsrR = GetUsersFromLastnameAndFirstName(zt.Uporabnik);
                    if (cUsrR == null)
                    {
                        zt.Uporabnik = zt.Uporabnik.Trim();
                        string[] splitName = zt.Uporabnik.Split(' ');
                        int splCnt = splitName.Length;
                        if (splCnt == 2) cUsrR = GetUsersFromLastnameAndFirstName(zt.Uporabnik, true);
                        if (splCnt == 3) cUsrR = GetUsersFromLastnameAndFirstName(zt.Uporabnik, true);
                    }
                }

                if (cUsrR != null)
                {

                    Izplacila izpl = GetIzplacilaByUserId(cUsrR.Id, "December");

                    if (izpl == null)
                    {



                        // izplačilo Avgust 2018
                        Izplacila izpA = new Izplacila(session);

                        izpA.IdUser = cUsrR;
                        izpA.DatumDo = new DateTime(2019, 1, 31);
                        izpA.DatumOd = new DateTime(2019, 1, 1);
                        izpA.Mesec = "Januar";
                        izpA.Leto = 2019;
                        izpA.PrenosIzPrejsnjegaMeseca = decimal.Round(Convert.ToDecimal(zt.Prenos));
                        izpA.PredlagateljT = Convert.ToDecimal(0);
                        izpA.RealizatorT = Convert.ToDecimal(0);
                        izpA.VsotaT = Convert.ToDecimal(0);
                        izpA.IzplaciloVMesecu = Convert.ToDecimal(0);
                        izpA.PrenosTvNaslednjiMesec = Convert.ToDecimal(0);
                        izpA.DatumIzracuna = DateTime.Now;
                        izpA.IDPrijave = 0;
                        izpA.ts = DateTime.Now;

                        string sImePriimek = izpA.IdUser.Firstname == null ? "" : izpA.IdUser.Firstname;
                        sImePriimek += " " + (izpA.IdUser.Lastname == null ? "" : izpA.IdUser.Lastname);

                        izpA.ImePriimek = sImePriimek;

                        izpA.Save();
                    }
                    else
                    {
                        memNotes.Text += "Izplačilo za uporabnika: " + zt.Uporabnik + " obstaja \r\n";
                        continue;
                    }

                }
                else
                {
                    memNotes.Text += "Uporabnik: " + zt.Uporabnik + "; ExternalID = " + zt.Koda + "\r\n";
                    continue;
                }



            }

            //XPCollection<TockeRealizator> collection_TockeRealizator = new XPCollection<TockeRealizator>(session);
            //foreach (TockeRealizator tr in collection_TockeRealizator)
            //{

            //    Users cUsrR = GetUsersFromPersonalID(tr.SifraRealizator);

            //    if (cUsrR != null)
            //    {
            //        cUsrR.TockeKVPRealizator = Convert.ToInt32(tr.TockeRealizator1);
            //        cUsrR.Save();
            //    }

            //}

        }

        private KVPUvoz15_12 GetByZaporednaStevilka(string sZapSt)
        {
            CriteriaOperator filterCriteria = null;

            filterCriteria = CriteriaOperator.Parse("StKVP = '" + sZapSt + "'");

            XPCollection<KVPUvoz15_12> collection_KVPExcell = new XPCollection<KVPUvoz15_12>(session, filterCriteria);
            collection_KVPExcell.Sorting.Add(new SortProperty("DatumVnosa", DevExpress.Xpo.DB.SortingDirection.Ascending));
            foreach (KVPUvoz15_12 KVPexcel in collection_KVPExcell)
            {
                return KVPexcel;
            }

            return null;
        }

        private Izplacila GetIzplacilaByUserId(Int32 iUser, string sMesec)
        {
            CriteriaOperator filterCriteria = null;

            filterCriteria = CriteriaOperator.Parse("IdUser = " + iUser + " and Mesec='" + sMesec + "'");

            XPCollection<Izplacila> collection_KVPExcell = new XPCollection<Izplacila>(session, filterCriteria);

            foreach (Izplacila KVPexcel in collection_KVPExcell)
            {
                return KVPexcel;
            }

            return null;
        }

        private void InsrtKVPDocuments(bool isInserted)
        {
            CriteriaOperator filterCriteria = null;

            //CriteriaOperator filterCriteria = CriteriaOperator.Parse(new SortProperty("ShiftEndTime", DevExpress.Xpo.DB.SortingDirection.Descending));
            //CriteriaOperator filterCriteria = CriteriaOperator.Parse("[datumvnosa]>='2018-01-01' and [InsertedDB] = 0");
            //filterCriteria = CriteriaOperator.Parse("[DatumVnosa]>='2018-01-01'");
            //if (isInserted)
            //{
            //    filterCriteria = CriteriaOperator.Parse("[InsertedDB] > 1");
            //}
            //else
            //{
            //    filterCriteria = CriteriaOperator.Parse("[InsertedDB] = 0 ");
            //}
            //CriteriaOperator filterCriteria = CriteriaOperator.Parse("[Presoja] is not null");
            filterCriteria = CriteriaOperator.Parse("StKVP = '2017 29424'");

            XPCollection<KVPUvoz15_12> collection_KVPExcell = new XPCollection<KVPUvoz15_12>(session, filterCriteria);
            collection_KVPExcell.Sorting.Add(new SortProperty("DatumVnosa", DevExpress.Xpo.DB.SortingDirection.Ascending));
            foreach (KVPUvoz15_12 KVPexcel in collection_KVPExcell)
            {
                if ((KVPexcel.OpisKVPIdeje != null) && (KVPexcel.OpisKVPIdeje.Length > 0))
                {
                    //if ((((KVPexcel.Realiziral == "1") && (KVPexcel.Zavrnil == 0)) || ((KVPexcel.Realiziral == "0") && (KVPexcel.Zavrnil == 1)) && KVPexcel.DatumVnosa >= new DateTime(2016, 1, 1)) || (KVPexcel.DatumVnosa >= new DateTime(2018, 1, 1)))
                    if (KVPexcel.DatumVnosa >= new DateTime(2016, 1, 1))
                    {
                        if (KVPexcel.Realiziral == 0)
                        {
                            if (KVPexcel.Zavrnil == 0)
                            {

                                KVPDocument KVPdoc = new KVPDocument(session);

                               


                                KVPdoc.DatumVnosa = (KVPexcel.DatumVnosa == null) ? new DateTime(2018, 1, 1) : KVPexcel.DatumVnosa;
                                KVPdoc.StevilkaKVP = KVPexcel.StKVP;




                                if ((KVPexcel.Uporabnik != null) && (KVPexcel.Uporabnik.Length > 0))
                                {                                  
                                    Users cUsr = GetUsersFromLastnameAndFirstName(KVPexcel.Uporabnik);
                                    if (cUsr == null)
                                    {
                                        KVPexcel.Uporabnik = KVPexcel.Uporabnik.Trim();
                                        string[] splitName = KVPexcel.Uporabnik.Split(' ');
                                        int splCnt = splitName.Length;
                                        if (splCnt == 2) cUsr = GetUsersFromLastnameAndFirstName(KVPexcel.Uporabnik, true);
                                        if (splCnt == 3) cUsr = GetUsersFromLastnameAndFirstName(KVPexcel.Uporabnik, true);
                                    }
                                    if (cUsr != null)
                                    {
                                        KVPdoc.Predlagatelj = cUsr;
                                    }
                                    else
                                    {

                                        //memNotes.Text += "Excell Predlagatelj: " + KVPexcel.Uporabnik + " KVP Stevilka: " + KVPexcel.StKVP + " \r\n";
                                        cUsr = GetUserByID(3518);
                                        if (cUsr != null) KVPdoc.Predlagatelj = cUsr;

                                        KVPexcel.InsertedDB = 10;
                                        KVPexcel.Save();
                                        continue;
                                    }
                                }


                                KVPdoc.idTip = kvpDocRepo.GetTypeByID(ReturnTipIDByNaziv(KVPexcel.TipIdeje));

                                KVPdoc.OpisProblem = "Prenos";
                                KVPdoc.PredlogIzboljsave = KVPexcel.OpisKVPIdeje;
                                KVPdoc.OpombeVodja = KVPexcel.Opombe;

                                InsertKVPStatus(KVPdoc, 1, KVPexcel, KVPexcel.DatumVnosa);

                                if ((KVPexcel.VodjaZaOdobritevIdeje != null) && (KVPexcel.VodjaZaOdobritevIdeje.Length > 0))
                                {
                                    //if (isInserted)
                                    //{
                                    //    if (KVPEx.Vodjazaodobritevideje == null) KVPEx.Vodjazaodobritevideje = "";
                                    //    if (KVPEx.Vodjazaodobritevideje.Trim().ToUpper() != KVPexcel.VodjaZaOdobritevIdeje.Trim().ToUpper())
                                    //    {
                                    //        memNotes.Text += "Razlika Vodja: " + KVPEx.Vodjazaodobritevideje.Trim().ToUpper() + " / " + KVPexcel.VodjaZaOdobritevIdeje.Trim().ToUpper() + " \r\n";
                                    //        KVPexcel.VodjaZaOdobritevIdeje = KVPEx.Vodjazaodobritevideje.Trim().ToUpper();
                                    //    }
                                    //}

                                    Users cUsrV = GetUsersFromLastnameAndFirstName(KVPexcel.VodjaZaOdobritevIdeje);
                                    if (cUsrV == null)
                                    {
                                        KVPexcel.VodjaZaOdobritevIdeje = KVPexcel.VodjaZaOdobritevIdeje.Trim();
                                        string[] splitName = KVPexcel.VodjaZaOdobritevIdeje.Split(' ');
                                        int splCnt = splitName.Length;
                                        if (splCnt == 2) cUsrV = GetUsersFromLastnameAndFirstName(KVPexcel.VodjaZaOdobritevIdeje, true);
                                        if (splCnt == 3) cUsrV = GetUsersFromLastnameAndFirstName(KVPexcel.VodjaZaOdobritevIdeje, true);
                                    }

                                    if (cUsrV != null)
                                    {
                                        KVPdoc.vodja_teama = cUsrV.Id;
                                    }
                                    else
                                    {
                                        //memNotes.Text += "Excell Vodja: " + KVPexcel.VodjaZaOdIdeje + " KVP Stevilka: " + KVPexcel.StKVP + "\r\n";
                                        cUsrV = GetUserByID(3518);
                                        if (cUsrV != null) KVPdoc.vodja_teama = cUsrV.Id;

                                        KVPexcel.InsertedDB = 20;
                                        KVPexcel.Save();
                                        continue;
                                    }
                                }

                                if ((KVPexcel.Presoja != null) && (KVPexcel.Presoja.Length > 0))
                                {
                                    //if (isInserted)
                                    //{
                                    //    if (KVPEx.Presoja == null) KVPEx.Presoja = "";

                                    //    if (KVPEx.Presoja.Trim().ToUpper() != KVPexcel.Presoja.Trim().ToUpper())
                                    //    {
                                    //        memNotes.Text += "Razlika presoja: " + KVPEx.Presoja.Trim().ToUpper() + " / " + KVPexcel.Presoja.Trim().ToUpper() + " \r\n";
                                    //        KVPexcel.Presoja = KVPEx.Presoja.Trim().ToUpper();
                                    //    }
                                    //}
                                    Users cUsrP = GetUsersFromLastnameAndFirstName(KVPexcel.Presoja);
                                    if (cUsrP == null)
                                    {
                                        KVPexcel.Presoja = KVPexcel.Presoja.Trim();
                                        string[] splitName = KVPexcel.Presoja.Split(' ');
                                        int splCnt = splitName.Length;
                                        if (splCnt == 2) cUsrP = GetUsersFromLastnameAndFirstName(KVPexcel.Presoja, true);
                                        if (splCnt == 3) cUsrP = GetUsersFromLastnameAndFirstName(KVPexcel.Presoja, true);
                                    }

                                    if (cUsrP != null)
                                    {
                                        // insert KVP status
                                        KVPPresoje kvpPres = new KVPPresoje(session);
                                        kvpPres.idKVPDocument = KVPdoc;
                                        kvpPres.Presojevalec = cUsrP;
                                        kvpPres.Opomba = "";
                                        if ((KVPexcel.Opombe != null) && (KVPexcel.Opombe.Length > 0))
                                        {
                                            kvpPres.Opomba = KVPexcel.Opombe;
                                        }
                                        kvpPres.ts = DateTime.Now;
                                        kvpPres.Save();
                                        InsertKVPStatus(KVPdoc, 8, KVPexcel, KVPexcel.DatumVnosa);

                                    }
                                    else
                                    {
                                        //memNotes.Text += "Excell Presoja: " + KVPexcel.Presoja + " KVP Stevilka: " + KVPexcel.StKVP + "\r\n";
                                        //cUsrP = GetUserByID(3412);
                                        //if (cUsrP != null) KVPdoc.Realizator = cUsrP;  
                                        KVPexcel.InsertedDB = 40;
                                        KVPexcel.Save();
                                        continue;
                                    }
                                }

                                if ((KVPexcel.Realizator != null) && (KVPexcel.Realizator.ToString().Trim().Length > 0))
                                {
                                    //if (isInserted)
                                    //{
                                    //    if (KVPEx.Realizator == null) KVPEx.Realizator = "";
                                    //    if (KVPEx.Realizator.Trim().ToUpper() != KVPexcel.Realizator.Trim().ToUpper())
                                    //    {
                                    //        memNotes.Text += "Razlika Realizator: " + KVPEx.Realizator.Trim().ToUpper() + " / " + KVPexcel.Realizator.Trim().ToUpper() + " \r\n";
                                    //        KVPexcel.Realizator = KVPEx.Realizator.Trim().ToUpper();
                                    //    }
                                    //}

                                    Users cUsrR = GetUsersFromLastnameAndFirstName(KVPexcel.Realizator);
                                    if (cUsrR == null)
                                    {
                                        KVPexcel.Realizator = KVPexcel.Realizator.Trim();
                                        string[] splitName = KVPexcel.Realizator.Split(' ');
                                        int splCnt = splitName.Length;
                                        if (splCnt == 2) cUsrR = GetUsersFromLastnameAndFirstName(KVPexcel.Realizator, true);
                                        if (splCnt == 3) cUsrR = GetUsersFromLastnameAndFirstName(KVPexcel.Realizator, true);
                                    }

                                    if (cUsrR == null)
                                    {
                                        cUsrR = GetUserByID(3518);
                                        if (cUsrR != null) KVPdoc.Realizator = cUsrR;
                                    }

                                    if (cUsrR != null)
                                    {
                                        KVPdoc.Realizator = cUsrR;
                                        if (KVPexcel.DatumZakljuceneIdeje < new DateTime(2000, 1, 1)) KVPexcel.DatumZakljuceneIdeje = KVPexcel.DatumVnosa;
                                        InsertKVPStatus(KVPdoc, 4, KVPexcel, KVPexcel.DatumZakljuceneIdeje);

                                        if ((KVPexcel.Realiziral != null) && (KVPexcel.Realiziral == 1))
                                        {
                                            if (KVPexcel.DatumZakljuceneIdeje < new DateTime(2000, 1, 1)) KVPexcel.DatumZakljuceneIdeje = KVPexcel.DatumVnosa;
                                            InsertKVPStatus(KVPdoc, 5, KVPexcel, KVPexcel.DatumZakljuceneIdeje);
                                            // calculate izplačila
                                            if (KVPexcel.DatumZakljuceneIdeje >= new DateTime(2018, 11, 01))
                                            {
                                                // predlagatelj
                                                InsertOrUpdateIzplacilaForMonth("November", KVPdoc.Predlagatelj, KVPdoc, true);
                                                // realizator
                                                InsertOrUpdateIzplacilaForMonth("November", KVPdoc.Realizator, KVPdoc, false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //cUsrR = GetUserByID(3412);
                                        //if (cUsrR != null) KVPdoc.Realizator = cUsrR;                        
                                        //memNotes.Text += "Excell Realizator: " + KVPexcel.Realizator + " KVP Stevilka: " + KVPexcel.StKVP + "\r\n";
                                        KVPexcel.InsertedDB = 30;
                                        KVPexcel.Save();
                                        continue;
                                    }
                                }

                                KVPdoc.DatumSpremembe = KVPexcel.DatumZakljuceneIdeje;

                                if ((KVPexcel.CIPPrihranek != null) && (KVPexcel.CIPPrihranek.ToString().Length > 0))
                                {
                                    KVPdoc.PrihranekStroski = KVPexcel.CIPPrihranek.ToString();
                                    KVPdoc.PrihranekStroskiDA_NE = true;
                                }




                                if ((KVPexcel.Zavrnil != null) && (KVPexcel.Zavrnil == 1))
                                {
                                    InsertKVPStatus(KVPdoc, 9, KVPexcel, KVPexcel.DatumZakljuceneIdeje);
                                    if ((KVPexcel.Opombe != null) && (KVPexcel.Opombe.Length > 0))
                                    {
                                        KVPdoc.ZavrnitevOpis = KVPexcel.Opombe;
                                    }
                                }

                                if (KVPdoc.LastStatusId.Koda == Enums.KVPStatuses.VNOS.ToString())
                                {
                                    InsertKVPStatus(KVPdoc, 14, KVPexcel, KVPexcel.DatumVnosa);
                                    InsertKVPStatus(KVPdoc, 2, KVPexcel, KVPexcel.DatumVnosa);
                                }

                                KVPexcel.InsertedDB = 1;
                                KVPexcel.Save();
                                KVPdoc.Save();
                            }
                            else
                            {
                                AddArhiveDoc(KVPexcel);
                            }
                        }
                        else
                        {
                            AddArhiveDoc(KVPexcel);
                        }
                    }
                    else
                    {
                        AddArhiveDoc(KVPexcel);
                    }
                }
            }
        }


        private void AddArhiveDoc(KVPUvoz15_12 KVPexcel)
        {
            KVPDocumentArh KVPdocArh = new KVPDocumentArh(session);
            KVPexcel.Realiziral = 0;
            if (KVPexcel.Realiziral == 0) KVPexcel.Realiziral = 0;
            KVPdocArh.StevilkaKVP = KVPexcel.StKVP;
            KVPdocArh.KVPSKupina = KVPexcel.KVPskupina;
            KVPdocArh.DatumVnosa = KVPexcel.DatumVnosa.ToShortDateString();
            KVPdocArh.Predlagatelj = KVPexcel.Uporabnik;
            KVPdocArh.OpisProblem = KVPexcel.OpisKVPIdeje;
            KVPdocArh.VodjaZaOdobritevIdeje = KVPexcel.VodjaZaOdobritevIdeje;
            KVPdocArh.Presoja = KVPexcel.Presoja;
            KVPdocArh.Realizator = KVPexcel.Realizator;
            KVPdocArh.DatumZakljuceneIdeje = KVPexcel.DatumZakljuceneIdeje.ToShortDateString();
            KVPdocArh.Sprejel = Convert.ToInt32(KVPexcel.Sprejel);
            KVPdocArh.Realiziral = Convert.ToInt32(KVPexcel.Realiziral);
            KVPexcel.Zavrnil = 0;
            KVPdocArh.Zavrnil = Convert.ToInt32(KVPexcel.Zavrnil);
            KVPdocArh.Presoja = KVPexcel.Presoja;
            KVPdocArh.Opombe = KVPexcel.Opombe;
            KVPdocArh.TipIdeje = KVPexcel.TipIdeje;
            KVPexcel.InsertedDB = 1;
            KVPexcel.Save();
            KVPdocArh.Save();
        }

        private void InsertOrUpdateIzplacilaForMonth(string sMonth, Users usr, KVPDocument kvpDoc, bool Predlagatelj)
        {


            Izplacila izplPrevMonth = payoutRepo.UserPayoutRecordForMonthAndYear(usr.Id, "Oktober", 2018);
            Izplacila userProposerPayoutRecord = payoutRepo.UserPayoutRecordForMonthAndYear(usr.Id, sMonth, 2018);

            if (userProposerPayoutRecord == null)
            {
                userProposerPayoutRecord = new Izplacila(session);
            }

            userProposerPayoutRecord.IdUser = usr;
            userProposerPayoutRecord.DatumDo = new DateTime(2018, 11, 30);
            userProposerPayoutRecord.DatumOd = new DateTime(2018, 11, 1);
            userProposerPayoutRecord.Mesec = "November";
            userProposerPayoutRecord.Leto = 2018;
            userProposerPayoutRecord.PrenosIzPrejsnjegaMeseca = (izplPrevMonth == null) ? 0 : Convert.ToDecimal(izplPrevMonth.PrenosTvNaslednjiMesec);


            if (Predlagatelj)
            {
                userProposerPayoutRecord.PredlagateljT += Convert.ToDecimal(kvpDoc.idTip.TockePredlagatelj);
            }
            else
            {
                userProposerPayoutRecord.RealizatorT += Convert.ToDecimal(kvpDoc.idTip.TockeRealizator);
            }
            decimal dSum = userProposerPayoutRecord.PrenosIzPrejsnjegaMeseca + userProposerPayoutRecord.PredlagateljT + userProposerPayoutRecord.RealizatorT;
            userProposerPayoutRecord.VsotaT = Convert.ToDecimal(dSum);
            userProposerPayoutRecord.IDPrijave = 0;
            userProposerPayoutRecord.ts = DateTime.Now;

            userProposerPayoutRecord.Save();

        }



        protected void btnRun_Click(object sender, EventArgs e)
        {

            //InsrtKVPDocuments(false);

            RichEditDocumentServer server = new RichEditDocumentServer();
            //ExportWordToPDF(server);
            //ExportPicturesToPDF();
            ExportXLSToPDF();

            //Label_Error.Text = "Število spremenjenih zapisov InvoiceUpdateIzdajnoSkladisceDobropisi: " + iSteviloSpremenjenihZapisov;

        }

        private void ExportPicturesToPDF(string triginalFilePath, string targetPDFPath)
        {
            PdfDocumentProcessor pdfDocumentProcessor = new PdfDocumentProcessor();


            using (RichEditDocumentServer server = new RichEditDocumentServer())
            {
                //Insert an image 
                DocumentImage docImage = server.Document.Images.Append(DocumentImageSource.FromFile("c:\\tmp\\FilePNG.png"));

                //Adjust the page width and height to the image's size 
                server.Document.Sections[0].Page.Width = docImage.Size.Width + server.Document.Sections[0].Margins.Right + server.Document.Sections[0].Margins.Left;
                server.Document.Sections[0].Page.Height = docImage.Size.Height + server.Document.Sections[0].Margins.Top + server.Document.Sections[0].Margins.Bottom;

                //Export the result to PDF 
                using (FileStream fs = new FileStream("c:\\tmp\\resultJPG.pdf", FileMode.OpenOrCreate))
                {
                    server.ExportToPdf(fs);
                }
            }
        }

        static void ExportWordToPDF(string triginalFilePath, string targetPDFPath)
        {
            RichEditDocumentServer server = new RichEditDocumentServer();
            #region #ExportToPDF
            server.LoadDocument("C:\\tmp\\test.docx", DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
            //Specify export options:
            PdfExportOptions options = new PdfExportOptions();
            options.DocumentOptions.Author = "Mark Jones";
            options.Compressed = false;
            options.ImageQuality = PdfJpegImageQuality.Highest;
            //Export the document to the stream: 
            using (FileStream pdfFileStream = new FileStream("c:\\tmp\\Document_PDF.pdf", FileMode.Create))
            {
                server.ExportToPdf(pdfFileStream, options);
            }
            #endregion #ExportToPDF
        }


        static void ExportXLSToPDF()
        {
            Workbook workbook = new Workbook();

            // Load a workbook from the file.
            workbook.LoadDocument("c:\\tmp\\GLS_HR.xls", DevExpress.Spreadsheet.DocumentFormat.OpenXml);


            using (FileStream pdfFileStream = new FileStream("c:\\tmp\\Document_XLS.pdf", FileMode.Create))
            {
                workbook.ExportToPdf(pdfFileStream);
            }


        }

        private void SetUsernameByRule(int iZaporendaC, Users usr)
        {
            string sUsrNm = CreateUserName(iZaporendaC, usr);
            if (CheckUserName(iZaporendaC, sUsrNm, usr.Id) > 0)
            {
                if (iZaporendaC > 6)
                {
                    usr.Lastname = usr.Lastname + iZaporendaC;
                    usr.Save();
                }
                SetUsernameByRule(iZaporendaC + 1, usr);

            }
            else
            {
                usr.Username = sUsrNm;
                usr.Password = CreatePassword(usr);
                usr.Save();
            }
        }

        protected void btnUpdateUserName_Click(object sender, EventArgs e)
        {
            XPCollection<Users> usrSes = new XPCollection<Users>(session);
            usrSes.Sorting.Add(new SortProperty("Lastname", DevExpress.Xpo.DB.SortingDirection.Ascending));
            foreach (Users usr in usrSes)
            {
                SetUsernameByRule(0, usr);
            }
        }





        //        string sUsrNm = CreateUserName(0, usr);

        //        if (CheckUserName(0, sUsrNm) > 0)
        //        {
        //            sUsrNm = CreateUserName(1, usr);

        //            if (CheckUserName(1, sUsrNm) > 0)
        //            {
        //                sUsrNm = CreateUserName(2, usr);
        //                if (CheckUserName(2, sUsrNm) > 0)
        //                {
        //                    sUsrNm = CreateUserName(3, usr);
        //                    if (CheckUserName(3, sUsrNm) > 0)
        //                    {
        //                        sUsrNm = CreateUserName(4, usr);
        //                        if (CheckUserName(4, sUsrNm) > 0)
        //                        {
        //                            sUsrNm = CreateUserName(5, usr);
        //                            if (CheckUserName(5, sUsrNm) > 0)
        //                            {
        //                                sUsrNm = CreateUserName(6, usr);
        //                                if (CheckUserName(6, sUsrNm) > 0)
        //                                {
        //                                    usr.Username = sUsrNm;

        //                                }
        //                                else
        //                                {
        //                                    usr.Username = sUsrNm;

        //                                }   
        //                            }
        //                            else
        //                            {
        //                                usr.Username = sUsrNm;

        //                            }
        //                        }
        //                        else
        //                        {
        //                            usr.Username = sUsrNm;

        //                        }
        //                    }
        //                    else
        //                    {
        //                        usr.Username = sUsrNm;

        //                    }
        //                }
        //                else
        //                {
        //                    usr.Username = sUsrNm;

        //                }
        //            }
        //            else
        //            {
        //                usr.Username = sUsrNm;

        //            }

        //        }
        //        else
        //        {
        //            usr.Username = sUsrNm;

        //        }



        //        usr.Password = CreatePassword(usr);
        //        usr.Save();
        //    }
        //}

        /// <summary>
        /// Preveri ali obstaja že uporabniško ime, če obstaja vrže koliko črk imena lahko odrežem
        /// </summary>
        /// <param name="iNumberOfLetters"></param>
        /// <param name="usrName"></param>
        /// <returns></returns>
        private Int32 CheckUserName(int iNumberOfLetters, string usrName, Int32 iCurrentIDUser)
        {
            CriteriaOperator filterCriteria = CriteriaOperator.Parse("Username ='" + usrName + "' and Id <> " + iCurrentIDUser);

            XPCollection<Users> collection_U = new XPCollection<Users>(session, filterCriteria);
            foreach (Users usr in collection_U)
            {
                return iNumberOfLetters + 1;
            }

            return 0;
        }

        private string CreateUserName(Int32 iNumberOfLetters, Users usr)
        {
            string sFirstName = usr.Firstname;
            string sLastName = usr.Lastname;

            sFirstName = Common.CommonMethods.ReplaceSumniki(sFirstName.ToLower());
            sLastName = Common.CommonMethods.ReplaceSumniki(sLastName.ToLower());
            if (sFirstName.Length > 1)
            {
                sFirstName = sFirstName.Substring(0, iNumberOfLetters + 1);
            }
            string sUserName = sFirstName + sLastName;

            return sUserName;
        }

        private string CreatePassword(Users usr)
        {
            string sFirstName = usr.Firstname;
            string sLastName = usr.Lastname;

            sFirstName = Common.CommonMethods.ReplaceSumniki(sFirstName.ToLower());
            sLastName = Common.CommonMethods.ReplaceSumniki(sLastName.ToLower());

            if (sFirstName.Length > 1)
            {
                sFirstName = sFirstName.Substring(0, 1);
            }

            if (sLastName.Length > 1)
            {
                sLastName = sLastName.Substring(0, 1);
            }
            else
            {
                sLastName = "";
            }
            string sPassword = sFirstName + sLastName + "123.";

            return sPassword;
        }



        protected void btnUporabnik_Click(object sender, EventArgs e)
        {
            MenjajGledeNaTipOsebe(10);
        }
        protected void btnVodja_Click(object sender, EventArgs e)
        {
            MenjajGledeNaTipOsebe(20);
        }
        protected void btnRealizator_Click(object sender, EventArgs e)
        {
            MenjajGledeNaTipOsebe(30);
        }
        protected void btnPresoja_Click(object sender, EventArgs e)
        {
            MenjajGledeNaTipOsebe(40);
        }

        private void MenjajGledeNaTipOsebe(int iTipOsebe)
        {
            string sImeVExcellu = txtImeVBazi.Text;
            string sZamenjaZ = txtMenjaj.Text;
            CriteriaOperator filterCriteria = null;
            int cnt = 0;
            switch (iTipOsebe)
            {
                case 10:
                    filterCriteria = CriteriaOperator.Parse("[Uporabnik]='" + sImeVExcellu + "'");
                    break;
                case 20:
                    filterCriteria = CriteriaOperator.Parse("[VodjaZaOdIdeje]='" + sImeVExcellu + "'");
                    break;
                case 30:
                    filterCriteria = CriteriaOperator.Parse("[Realizator]='" + sImeVExcellu + "'");
                    break;
                case 40:
                    filterCriteria = CriteriaOperator.Parse("[Presoja]='" + sImeVExcellu + "'");
                    break;
                default:
                    break;
            }



            XPCollection<KVPExcellDokumenti> collection_KVPExcell = new XPCollection<KVPExcellDokumenti>(session, filterCriteria);
            collection_KVPExcell.Sorting.Add(new SortProperty("DatumVnosa", DevExpress.Xpo.DB.SortingDirection.Ascending));
            foreach (KVPExcellDokumenti KVPexcel in collection_KVPExcell)
            {
                switch (iTipOsebe)
                {
                    case 10:
                        KVPexcel.Uporabnik = sZamenjaZ;
                        break;
                    case 20:
                        KVPexcel.VodjaZaOdIdeje = sZamenjaZ;
                        break;
                    case 30:
                        KVPexcel.Realizator = sZamenjaZ;
                        break;
                    case 40:
                        KVPexcel.Presoja = sZamenjaZ;
                        break;
                    default:
                        break;
                }

                cnt++;
                KVPexcel.Save();
            }

            memRezultat.Text += "TS: " + DateTime.Now + ", Menjano: " + cnt + "\r\n";
        }

        protected void btnZeInsertirane_Click(object sender, EventArgs e)
        {

            InsrtKVPDocuments(true);


            //Label_Error.Text = "Število spremenjenih zapisov InvoiceUpdateIzdajnoSkladisceDobropisi: " + iSteviloSpremenjenihZapisov;

        }

        private void InsertKVPStatus(KVPDocument KVPdoc, int idStatus, KVPUvoz15_12 KVPEx, DateTime dtStatus)
        {
            // insert KVP status
            KVP_Status kvpStatus = new KVP_Status(session);
            kvpStatus.idKVPDocument = KVPdoc;

            Status cSt = GetStatusByID(idStatus);
            if (cSt != null)
            {
                kvpStatus.idStatus = cSt; // vnos
                KVPdoc.LastStatusId = cSt;
                KVPdoc.ts = dtStatus;
            }

            kvpStatus.ts = dtStatus;
            kvpStatus.Save();
        }

        private Users GetUsersFromLastnameAndFirstName(string sName, bool b2Names = false)
        {
            sName = sName.Trim();
            string[] splitName = sName.Split(' ');
            int splCnt = splitName.Length;
            string sFirstName = "";
            string sLastName = "";


            if (splCnt == 1) return null;

            if (splCnt == 2)
            {
                if (!(b2Names))
                {
                    sLastName = sName.Substring(0, sName.IndexOf(" "));
                    sFirstName = sName.Substring(sName.IndexOf(" "), sName.Length - sName.IndexOf(" "));
                }
                else
                {
                    sLastName = sName.Substring(sName.IndexOf(" "), sName.Length - sName.IndexOf(" "));
                    sFirstName = sName.Substring(0, sName.IndexOf(" "));
                }

            }
            else
            {
                if (!(b2Names))
                {
                    sLastName = splitName[0].ToString() + " " + splitName[1].ToString();
                    sFirstName = splitName[2].ToString();
                }
                else
                {
                    sLastName = splitName[0].ToString();
                    sFirstName = splitName[1].ToString() + " " + splitName[2].ToString();
                }
            }

            sLastName = sLastName.Trim();
            sFirstName = sFirstName.Trim();

            CriteriaOperator filterCriteria = CriteriaOperator.Parse("upper(Firstname) ='" + sFirstName.ToUpper() + "' and  upper(Lastname)='" + sLastName.ToUpper() + "'");

            XPCollection<Users> collection_U = new XPCollection<Users>(session, filterCriteria);
            foreach (Users usr in collection_U)
            {
                return usr;
            }


            return null;
        }

        private Users GetUsersFromPersonalID(double iPersonalID)
        {


            CriteriaOperator filterCriteria = CriteriaOperator.Parse("PersonalId =" + iPersonalID);

            XPCollection<Users> collection_U = new XPCollection<Users>(session, filterCriteria);
            foreach (Users usr in collection_U)
            {
                return usr;

            }
            return null;
        }

        private KVPSkupina GetKVPSkupinaFromKoda(string sKoda)
        {


            CriteriaOperator filterCriteria = CriteriaOperator.Parse("Koda ='" + sKoda + "'");

            XPCollection<KVPSkupina> collection_U = new XPCollection<KVPSkupina>(session, filterCriteria);
            foreach (KVPSkupina kvp in collection_U)
            {
                return kvp;

            }
            return null;
        }

        private Users GetUsersFromExternalID(Int32 iPersonalID)
        {


            CriteriaOperator filterCriteria = CriteriaOperator.Parse("ExternalId ='" + iPersonalID.ToString().Trim() + "'");

            XPCollection<Users> collection_U = new XPCollection<Users>(session, filterCriteria);
            foreach (Users usr in collection_U)
            {
                return usr;

            }
            return null;
        }


        private Status GetStatusByID(int IDStat)
        {
            CriteriaOperator criteria_S = CriteriaOperator.Parse("idStatus =" + IDStat);
            XPCollection<Status> collection_S = new XPCollection<Status>(session, criteria_S);
            foreach (Status st in collection_S)
            {

                return st;

            }
            return null;
        }

        private Users GetUserByID(int IDUser)
        {
            CriteriaOperator criteria_S = CriteriaOperator.Parse("Id =" + IDUser);
            XPCollection<Users> collection_S = new XPCollection<Users>(session, criteria_S);
            foreach (Users st in collection_S)
            {
                return st;

            }
            return null;
        }

        protected void btnMergeKVP_Click(object sender, EventArgs e)
        {
            memNotes.Text = DateTime.Now.ToString() + " \r\n";
            kodeksRepo.MergeKodeks_eKVP(session);
            memNotes.Text += DateTime.Now.ToString();
        }

        protected void btnUpdateLok_Click(object sender, EventArgs e)
        {
            UpdateLokacijaSifrant();
            //UpdateStrojSifrant()
            //    UpdateLiniijaSifrant()
        }

        private void UpdateLokacijaSifrant()
        {
            CriteriaOperator filterCriteria = null;
            filterCriteria = CriteriaOperator.Parse("Len(OpisLokacija) > 0");
            Lokacija location = null;
            XPCollection<KVPDocument> collKVPDoc = new XPCollection<KVPDocument>(session, filterCriteria);
            collKVPDoc.Sorting.Add(new SortProperty("DatumVnosa", DevExpress.Xpo.DB.SortingDirection.Ascending));
            foreach (KVPDocument doc in collKVPDoc)
            {
                string sLokKoda = "";
                if (doc.OpisLokacija.Length > 0)
                {
                    location = CheckLokacijaByName(doc.OpisLokacija);

                    if (location == null)
                    {
                        location = new Lokacija(session);

                        location.idLokacija = 0;
                        location.Sort = locationRepo.GetCntForSort() + 1;
                        if (doc.OpisLokacija.Length > 4)
                        {
                            sLokKoda = doc.OpisLokacija.ToUpper().Substring(0, 4);
                        }
                        else
                        {
                            sLokKoda = doc.OpisLokacija.ToUpper();
                        }

                        location.Koda = sLokKoda + location.Sort;
                        if (doc.OpisLokacija.Length > 250)
                        {
                            location.Opis = doc.OpisLokacija.Substring(0, 250);
                        }
                        else
                        {
                            location.Opis = doc.OpisLokacija;
                        }

                        locationRepo.SaveLocation(location);
                    }

                    doc.LokacijaID = location;
                    doc.Save();
                }


            }
        }

        private Lokacija CheckLokacijaByName(string sLokacijaName)
        {
            CriteriaOperator filterCriteria = null;
            sLokacijaName = sLokacijaName.Trim();
            filterCriteria = CriteriaOperator.Parse("Opis = '" + sLokacijaName + "'");
            XPCollection<Lokacija> collLok = new XPCollection<Lokacija>(session, filterCriteria);

            foreach (Lokacija lok in collLok)
            {
                return lok;
            }

            return null;

        }

        protected void btnSetPresoje_Click(object sender, EventArgs e)
        {
            CriteriaOperator filterCriteria = null;
            filterCriteria = CriteriaOperator.Parse("LastStatusId = 8 and LastPresojaID is null ");

            XPCollection<KVPDocument> collKVPDoc = new XPCollection<KVPDocument>(session, filterCriteria);
            collKVPDoc.Sorting.Add(new SortProperty("DatumVnosa", DevExpress.Xpo.DB.SortingDirection.Ascending));
            foreach (KVPDocument doc in collKVPDoc)
            {
                SetPresoje(doc.idKVPDocument, doc);
            }


        }

        protected void btnUpdateKVPSkupina_Click(object sender, EventArgs e)
        {
            CriteriaOperator filterCriteria = null;
            filterCriteria = CriteriaOperator.Parse("KVPSkupinaID is null");

            XPCollection<KVPDocument> collKVPDoc = new XPCollection<KVPDocument>(session, filterCriteria);
            collKVPDoc.Sorting.Add(new SortProperty("DatumVnosa", DevExpress.Xpo.DB.SortingDirection.Ascending));
            foreach (KVPDocument doc in collKVPDoc)
            {
                KVPUvoz15_12 KVPEx = GetByZaporednaStevilka(doc.StevilkaKVP);

                if (KVPEx != null)
                {
                    if (KVPEx.KVPskupina != null)
                    {
                        KVPSkupina kvpSk = kvpGroupRepo.GetKVPGroupByCode(KVPEx.KVPskupina.Trim());
                        if (kvpSk != null)
                        {
                            doc.KVPSkupinaID = kvpSk;
                            doc.Save();
                        }
                        else
                        {
                            KVPEx.InsertedDB = 78;
                            KVPEx.Save();
                        }
                    }
                    else
                    {
                        KVPEx.InsertedDB = 77;
                        KVPEx.Save();
                    }
                }
                else
                {
                    memNotes.Text += "KVP s številko: " + doc.StevilkaKVP + " ni KVPUvoz15_12 \r\n";
                }

            }


        }

        private void SetPresoje(Int32 iIdKVP, KVPDocument kvpDoc)
        {
            CriteriaOperator filterCriteria = null;
            filterCriteria = CriteriaOperator.Parse("idKVPDocument = " + iIdKVP);

            XPCollection<KVPPresoje> collPresoje = new XPCollection<KVPPresoje>(session, filterCriteria);
            collPresoje.Sorting.Add(new SortProperty("ts", DevExpress.Xpo.DB.SortingDirection.Descending));

            if (collPresoje.Count > 0)
            {
                collPresoje[0].JeZadnjiPresojevalec = true;
                kvpDoc.LastPresojaID = collPresoje[0].Presojevalec;
                collPresoje[0].Save();
                kvpDoc.Save();
            }
        }

        protected void btnSetDatumZakljuceneIdeje_Click(object sender, EventArgs e)
        {
            XPCollection<KVPDocument> collKVPDoc = new XPCollection<KVPDocument>(session);
            collKVPDoc.Sorting.Add(new SortProperty("DatumVnosa", DevExpress.Xpo.DB.SortingDirection.Ascending));
            foreach (KVPDocument doc in collKVPDoc)
            {
                SetDatumZakljuceneIdeje(doc.idKVPDocument, doc);
            }


        }

        protected void btnSetDatumZakljuceneIdejeRK_Click(object sender, EventArgs e)
        {
            CriteriaOperator filterCriteria = null;
            filterCriteria = CriteriaOperator.Parse("LastStatusId = 15 and DatumZakljuceneIdeje is null");
            XPCollection<KVPDocument> collKVPDoc = new XPCollection<KVPDocument>(session, filterCriteria);
            collKVPDoc.Sorting.Add(new SortProperty("DatumVnosa", DevExpress.Xpo.DB.SortingDirection.Ascending));
            foreach (KVPDocument doc in collKVPDoc)
            {
                SetDatumZakljuceneIdeje(doc.idKVPDocument, doc);
            }


        }

        protected void btnIzplacila_Click(object sender, EventArgs e)
        {
            CriteriaOperator filterCriteria = null;
            filterCriteria = CriteriaOperator.Parse("ImePriimek is null");
            XPCollection<Izplacila> collIzplacila = new XPCollection<Izplacila>(session, filterCriteria);

            foreach (Izplacila izpl in collIzplacila)
            {
                string sImePriimek = izpl.IdUser.Firstname == null ? "" : izpl.IdUser.Firstname;
                sImePriimek += " " + (izpl.IdUser.Lastname == null ? "" : izpl.IdUser.Lastname);

                izpl.ImePriimek = sImePriimek;

                izpl.Save();

            }


        }

        private void SetDatumZakljuceneIdeje(Int32 iIdKVP, KVPDocument kvpDoc)
        {
            CriteriaOperator filterCriteria = null;
            filterCriteria = CriteriaOperator.Parse("idKVPDocument = " + iIdKVP);

            XPCollection<KVP_Status> collStatus = new XPCollection<KVP_Status>(session, filterCriteria);
            collStatus.Sorting.Add(new SortProperty("ts", DevExpress.Xpo.DB.SortingDirection.Descending));

            if (collStatus.Count > 0)
            {
                if (collStatus[0].idStatus.idStatus == 15)
                {
                    kvpDoc.DatumZakljuceneIdeje = collStatus[0].ts;
                    kvpDoc.Save();
                }
            }
        }



        protected void btnCheckStatusOnKVP_Click(object sender, EventArgs e)
        {
            CriteriaOperator filterCriteria = null;
            //filterCriteria = CriteriaOperator.Parse("idKVPDocument = 6160");
            //filterCriteria = CriteriaOperator.Parse("1 = 1");
            XPCollection<KVPDocument> collKVPDoc = new XPCollection<KVPDocument>(session, filterCriteria);
            collKVPDoc.Sorting.Add(new SortProperty("ts", DevExpress.Xpo.DB.SortingDirection.Descending));

            foreach (KVPDocument doc in collKVPDoc)
            {
                KVP_Status lastKVPS = GetLastStatusFromKVP_Status(doc.idKVPDocument);

                if (lastKVPS.idStatus != doc.LastStatusId)
                {
                    memNotes.Text += "Update KVPDocument set LastStatusId = " + doc.LastStatusId.idStatus.ToString() + " where idKVPDocument = " + doc.idKVPDocument.ToString() + "; update  KVP_Status set idStatus = " + lastKVPS.idStatus.idStatus.ToString() + " where idKVPDocument = " + doc.idKVPDocument + " ----  KVP s številko: " + doc.StevilkaKVP + " ima različen LaststatusID - KVP_Status = " + lastKVPS.idStatus.Koda + " - KVPDocument.LastStatusId = " + doc.LastStatusId.Koda + "\r\n";
                }
            }


        }

        private KVP_Status GetLastStatusFromKVP_Status(Int32 iIdKVP)
        {
            CriteriaOperator filterCriteria = null;
            filterCriteria = CriteriaOperator.Parse("idKVPDocument = " + iIdKVP);

            XPCollection<KVP_Status> collStatus = new XPCollection<KVP_Status>(session, filterCriteria);
            collStatus.Sorting.Add(new SortProperty("ts", DevExpress.Xpo.DB.SortingDirection.Descending));
            collStatus.Sorting.Add(new SortProperty("idKVP_Status", DevExpress.Xpo.DB.SortingDirection.Descending));

            if (collStatus.Count > 0)
            {
                return collStatus[0];
            }

            return null;
        }

        protected void btnIzplacilaUpdateMesec_Click(object sender, EventArgs e)
        {
            CriteriaOperator filterCriteria = null;
            filterCriteria = CriteriaOperator.Parse("DatumZakljuceneIdeje >= ? and DatumZakljuceneIdeje <= ? and LastStatusId =? ", new DateTime(2019, 3, 1), new DateTime(2019, 3, 28, 23, 59, 59), 11);
            XPCollection<KVPDocument> collKVPDoc = new XPCollection<KVPDocument>(session, filterCriteria);

            foreach (KVPDocument doc in collKVPDoc)
            {
                CompleteKVPDocument(doc);
            }


        }

        protected void btnUpdateIzplacilaMarec_Click(object sender, EventArgs e)
        {
            try
            {


                CriteriaOperator filterCriteria = null;
                filterCriteria = CriteriaOperator.Parse("DatumZakljuceneIdeje >= ? and DatumZakljuceneIdeje <= ? and LastStatusId =? ", new DateTime(2019, 1, 1), new DateTime(2019, 3, 31, 23, 59, 59), 11);
                XPCollection<KVPDocument> collKVPDoc = new XPCollection<KVPDocument>(session, filterCriteria);
                collKVPDoc.Sorting.Add(new SortProperty("DatumZakljuceneIdeje", DevExpress.Xpo.DB.SortingDirection.Ascending));
                Int32 iCnt = 0;
                Int32 iSumCnt = 0;
                foreach (KVPDocument doc in collKVPDoc)
                {
                    iSumCnt = collKVPDoc.Count();

                    CompleteKVPDocument(doc);
                    iCnt++;


                }


            }
            catch (Exception ex)
            {

            }
        }


        private void CompleteKVPDocument(KVPDocument model)
        {
            string month = CommonMethods.GetDateTimeMonthByNumber(model.DatumZakljuceneIdeje.Date.Month);

            //Pridobimo izplačila za mesec in leto realizacije
            Izplacila userProposerPayoutRecord = payoutRepo.UserPayoutRecordForMonthAndYear(model.Predlagatelj.Id, month, model.DatumZakljuceneIdeje.Date.Year);



            //će obstaja izplačilo posodobimo njegove vrednosti
            if (model.Predlagatelj.UpravicenDoKVP)
            {
                if (userProposerPayoutRecord != null)
                {
                    decimal vsotaTProposer = 0;
                    userProposerPayoutRecord.PredlagateljT += model.idTip.TockePredlagatelj;
                    vsotaTProposer = userProposerPayoutRecord.PredlagateljT + userProposerPayoutRecord.RealizatorT + userProposerPayoutRecord.PrenosIzPrejsnjegaMeseca;
                    userProposerPayoutRecord.VsotaT = vsotaTProposer;
                    //userProposerPayoutRecord.PrenosTvNaslednjiMesec += vsotaTProposer;

                    payoutRepo.SavePayout(userProposerPayoutRecord);
                }
                else//če ne obstaja ustvarimo novo izplačilo - vrednosti prenesemo iz prejšnjega meseca in jih prištejemo novim
                {
                    CreateNewPayout(model, false);
                }
            }

            Izplacila userRealizatorPayoutRecord = payoutRepo.UserPayoutRecordForMonthAndYear(model.Realizator.Id, month, model.DatumZakljuceneIdeje.Date.Year);
            //isto kot za predlagatelja (zgoraj) velja tudi za realizatorja
            if (model.Realizator.UpravicenDoKVP)
            {
                if (userRealizatorPayoutRecord != null)
                {
                    decimal vsotaTRealizator = 0;
                    userRealizatorPayoutRecord.RealizatorT += model.idTip.TockeRealizator;
                    vsotaTRealizator = userRealizatorPayoutRecord.PredlagateljT + userRealizatorPayoutRecord.RealizatorT + userRealizatorPayoutRecord.PrenosIzPrejsnjegaMeseca;
                    userRealizatorPayoutRecord.VsotaT = vsotaTRealizator;
                    //userRealizatorPayoutRecord.PrenosTvNaslednjiMesec += vsotaTRealizator;

                    payoutRepo.SavePayout(userRealizatorPayoutRecord);
                }
                else
                {
                    CreateNewPayout(model, true);
                }
            }
        }

        private void CreateNewPayout(KVPDocument model, bool isRealizator = false)
        {
            if (model.DatumZakljuceneIdeje < new DateTime(2000, 1, 1)) model.DatumZakljuceneIdeje = DateTime.Now;

            DateTime previousMonthDate = model.DatumZakljuceneIdeje.Date.AddMonths(-1);
            string previousMonth = CommonMethods.GetDateTimeMonthByNumber(previousMonthDate.Date.Month);
            Izplacila payoutPreviousMonth = payoutRepo.UserPayoutRecordForMonthAndYear((!isRealizator ? model.Predlagatelj.Id : model.Realizator.Id), previousMonth, previousMonthDate.Year);


            Izplacila payout = new Izplacila(session);
            payout.DatumOd = CommonMethods.GetFirstDayOfMonth(model.DatumZakljuceneIdeje);
            payout.DatumDo = CommonMethods.GetLastDayOfMonth(model.DatumZakljuceneIdeje);
            //payout.DatumIzracuna
            payout.IdUser = employeeRepo.GetEmployeeByID((!isRealizator ? model.Predlagatelj.Id : model.Realizator.Id), model.Session);
            payout.IzplaciloVMesecu = 0;
            payout.Leto = model.DatumZakljuceneIdeje.Date.Year;
            payout.Mesec = CommonMethods.GetDateTimeMonthByNumber(model.DatumZakljuceneIdeje.Date.Month);
            payout.PredlagateljT = !isRealizator ? model.idTip.TockePredlagatelj : 0;
            payout.PrenosIzPrejsnjegaMeseca = payoutPreviousMonth != null ? payoutPreviousMonth.PrenosTvNaslednjiMesec : 0;
            payout.PrenosTvNaslednjiMesec = 0;
            payout.RealizatorT = !isRealizator ? 0 : model.idTip.TockeRealizator;
            payout.VsotaT = (payout.PrenosIzPrejsnjegaMeseca + payout.PredlagateljT + payout.RealizatorT);
            string sImePriimek = payout.IdUser.Firstname == null ? "" : payout.IdUser.Firstname;
            sImePriimek += " " + (payout.IdUser.Lastname == null ? "" : payout.IdUser.Lastname);

            payout.ImePriimek = sImePriimek;
            // pripravi izplačilo za priimenk in ime
            payoutRepo.SavePayout(payout);

        }

        //protected void btnPreveriIzplacila_Click(object sender, EventArgs e)
        //{
        //    CriteriaOperator filterCriteria = null;

        //    string filt = "UpravicenDoKVP = 1";
        //    //filt += "and Id = 1112";

        //    errorRepo.SaveErrorLog("Start preveri izplačila", PrincipalHelper.GetUserPrincipal().ID);
        //    filterCriteria = CriteriaOperator.Parse(filt);
        //    XPCollection<Users> usrSes = new XPCollection<Users>(session, filterCriteria);
        //    errorRepo.SaveErrorLog("Load users", PrincipalHelper.GetUserPrincipal().ID);

        //    foreach (Users usr in usrSes)
        //    {

        //        decimal dPrenosIzPrejMeseca = 0;
        //        decimal dVsotaCMesec = 0;
        //        decimal pointsToTransfer = 0;
        //        decimal dPrenosVnaslednjiMesec = 0;

        //        bool bExistDate = false;
        //        bool bPrenos = false;

        //        filterCriteria = CriteriaOperator.Parse("IdUser = " + usr.Id);


        //        errorRepo.SaveErrorLog("Load user " + usr.Id, PrincipalHelper.GetUserPrincipal().ID);

        //        XPCollection<Izplacila> collection_Izplacila = new XPCollection<Izplacila>(session, filterCriteria);
        //        collection_Izplacila.Sorting.Add(new SortProperty("DatumOd", DevExpress.Xpo.DB.SortingDirection.Ascending));

        //        if (collection_Izplacila.Count == 0) memRezultat.Text += "Uporabnik: " + usr.Lastname + " nima izplacil " + usr.UpravicenDoKVP.ToString() + "\r\n";

        //        if (collection_Izplacila.Count > 0)
        //        {
        //            bExistDate = ExistRecordForDate(new DateTime(2019, 3, 1), collection_Izplacila);
        //        }

        //        foreach (Izplacila izpl in collection_Izplacila)
        //        {
        //            dPrenosIzPrejMeseca = bPrenos ? dPrenosVnaslednjiMesec : izpl.PrenosIzPrejsnjegaMeseca;
        //            if (izpl.DatumOd.Month == 1)
        //            {
        //                dVsotaCMesec = dPrenosIzPrejMeseca + izpl.PredlagateljT + izpl.RealizatorT;
        //            }
        //            else
        //            {
        //                dVsotaCMesec = izpl.PredlagateljT + izpl.RealizatorT + dPrenosVnaslednjiMesec;
        //                izpl.PrenosIzPrejsnjegaMeseca = dPrenosVnaslednjiMesec;
        //            }

        //            izpl.VsotaT = dVsotaCMesec;
        //            izpl.Save();
        //            decimal paymentMultiplicator = dVsotaCMesec / 500;
        //            int payment = CommonMethods.ParseInt(Math.Floor(paymentMultiplicator));
        //            decimal remainigPoints = (paymentMultiplicator - payment);
        //            pointsToTransfer = remainigPoints * 500;

        //            izpl.PrenosTvNaslednjiMesec = pointsToTransfer;
        //            izpl.ts = DateTime.Now;
        //            izpl.Save();
        //            dPrenosVnaslednjiMesec = pointsToTransfer;

        //            if (pointsToTransfer != izpl.PrenosTvNaslednjiMesec)
        //            {
        //                //memNotes.Text += "Izplačilo za uporabnika: " + usr.Lastname + " je različno v mesecu " + izpl.Mesec + " Izračunano: " + pointsToTransfer.ToString() + " / " + izpl.PrenosTvNaslednjiMesec + "\r\n";                        
        //            }

        //            if (izpl.VsotaT >= 500 && izpl.Mesec == "Februar" && izpl.IzplaciloVMesecu == 0)
        //            {
        //                dPrenosVnaslednjiMesec = dVsotaCMesec;
        //                izpl.PrenosTvNaslednjiMesec = dVsotaCMesec;
        //                izpl.PrenosIzPrejsnjegaMeseca = dVsotaCMesec;
        //                izpl.PredlagateljT = 0;
        //                izpl.RealizatorT = 0;
        //                izpl.VsotaT = 0;
        //                izpl.Save();
        //                bPrenos = true;
        //                if (!bExistDate)
        //                {
        //                    izpl.VsotaT = dVsotaCMesec;
        //                    CreateNewPayoutsForMonth(izpl, new DateTime(2019, 3, 1));

        //                    decimal paymentMultiplicator1 = izpl.VsotaT / 500;
        //                    int payment1 = CommonMethods.ParseInt(Math.Floor(paymentMultiplicator));
        //                    decimal remainigPoints1 = (paymentMultiplicator - payment);

        //                    pointsToTransfer = remainigPoints1 * 500;
        //                    dPrenosVnaslednjiMesec = pointsToTransfer;
        //                }
        //                memNotes.Text += "Prenos za mesec FEB uporabnika: " + izpl.ImePriimek + " Znesek: " + dVsotaCMesec + "\r\n";
        //            }

        //            if (izpl.VsotaT >= 1000 && izpl.VsotaT < 1500 && izpl.Mesec == "Februar" && izpl.IzplaciloVMesecu != 110)
        //            {

        //                izpl.VsotaT = izpl.VsotaT - 500;
        //                izpl.PrenosTvNaslednjiMesec = izpl.PrenosTvNaslednjiMesec + 500;
        //                dPrenosVnaslednjiMesec = izpl.PrenosTvNaslednjiMesec;
        //                izpl.Save();
        //                bPrenos = true;
        //                if (!bExistDate)
        //                {
        //                    CreateNewPayoutsForMonth(izpl, new DateTime(2019, 3, 1));

        //                    decimal paymentMultiplicator1 = izpl.VsotaT / 500;
        //                    int payment1 = CommonMethods.ParseInt(Math.Floor(paymentMultiplicator));
        //                    decimal remainigPoints1 = (paymentMultiplicator - payment);

        //                    pointsToTransfer = remainigPoints1 * 500;
        //                    dPrenosVnaslednjiMesec = pointsToTransfer;
        //                }

        //                memNotes.Text += "Prenos za preveč mesec FEB uporabnika: " + izpl.ImePriimek + " Znesek: " + dVsotaCMesec + "\r\n";
        //            }

        //            if (izpl.VsotaT >= 1500 && izpl.VsotaT < 2000 && izpl.Mesec == "Februar" && izpl.IzplaciloVMesecu != 165)
        //            {

        //                izpl.VsotaT = izpl.VsotaT - 500;
        //                izpl.PrenosTvNaslednjiMesec = izpl.PrenosTvNaslednjiMesec + 500;
        //                dPrenosVnaslednjiMesec = izpl.PrenosTvNaslednjiMesec;
        //                izpl.Save();
        //                bPrenos = true;

        //                if (!bExistDate)
        //                {
        //                    CreateNewPayoutsForMonth(izpl, new DateTime(2019, 3, 1));

        //                    decimal paymentMultiplicator1 = izpl.VsotaT / 500;
        //                    int payment1 = CommonMethods.ParseInt(Math.Floor(paymentMultiplicator));
        //                    decimal remainigPoints1 = (paymentMultiplicator - payment);

        //                    pointsToTransfer = remainigPoints1 * 500;
        //                    dPrenosVnaslednjiMesec = pointsToTransfer;
        //                }

        //                memNotes.Text += "Prenos za preveč mesec FEB uporabnika: " + izpl.ImePriimek + " Znesek: " + dVsotaCMesec + "\r\n";
        //            }
        //        }
        //    }

        //}

        private decimal GetSumForMonth(DateTime fromDate, DateTime toDate, Int32 iUser, bool isPredlagatelj)
        {
            CriteriaOperator filterCriteria = null;
            string sSQL = "DatumZakljuceneIdeje >= ? and DatumZakljuceneIdeje <= ? and LastStatusId =? ";
            sSQL += (isPredlagatelj) ? "and Predlagatelj = ?" : "and Realizator = ?";
            toDate = toDate.AddHours(23);
            toDate = toDate.AddMinutes(59);
            toDate = toDate.AddSeconds(59);

            filterCriteria = CriteriaOperator.Parse(sSQL, fromDate, toDate, 11, iUser);
            XPCollection<KVPDocument> collKVPDoc = new XPCollection<KVPDocument>(session, filterCriteria);
            collKVPDoc.Sorting.Add(new SortProperty("DatumZakljuceneIdeje", DevExpress.Xpo.DB.SortingDirection.Ascending));
            Int32 iCnt = 0;
            Int32 iSumCnt = 0;

            decimal sSum = 0;

            foreach (KVPDocument doc in collKVPDoc)
            {
                if (isPredlagatelj)
                {
                    sSum += doc.idTip.TockePredlagatelj;
                }
                else
                {
                    sSum += doc.idTip.TockeRealizator;
                }

            }
            return sSum;
        }

        protected void btnAnalizaVsehKVPInTock_Click(object sender, EventArgs e)
        {
            try
            {
                decimal dVseTocke = 0;

                CriteriaOperator filterCriteria = null;
                memNotes.Text = "";
                string filt = "UpravicenDoKVP = 1";
                //filt += "and Id in (1146)";
                //filt += "and Id in (1148)";
                //filt += "and Id in (454 ,146 ,151 ,193 ,218 ,241 ,298 ,304 ,316 ,345 ,409 ,425 ,426 ,440 ,461 ,479 ,499 ,557 ,574 ,609 ,624 ,629 ,643 ,645 ,670 ,676 ,700 ,719 ,724 ,748 ,810 ,895 ,1014,1041,1134,1148,1173,1217,1235,1245,1247,1357,1458,1486,1525,1555,1566,1715,5421,5457)";
                //filt += "and Id in (34, 72, 75, 104, 110, 117, 156, 161, 183, 193)";

                errorRepo.SaveErrorLog("Start preveri izplačila", PrincipalHelper.GetUserPrincipal().ID);
                filterCriteria = CriteriaOperator.Parse(filt);
                XPCollection<Users> usrSes = new XPCollection<Users>(session, filterCriteria);
                errorRepo.SaveErrorLog("Load users", PrincipalHelper.GetUserPrincipal().ID);

                foreach (Users usr in usrSes)
                {

                    decimal dVsotaCMesecIzplacila = 0;

                    decimal dTockePredlagateljKVP = 0;
                    decimal dTockeRealizatorKVP = 0;
                    decimal dRazlika = 0;
                    decimal dNePreneseneTocke = 0;
                    string sMeseci = "";



                    Izplacila iLastIzpl = null;

                    bool bExistNote = false;

                    filterCriteria = CriteriaOperator.Parse("IdUser = " + usr.Id);
                    XPCollection<Izplacila> collection_Izplacila = new XPCollection<Izplacila>(session, filterCriteria);
                    collection_Izplacila.Sorting.Add(new SortProperty("DatumOd", DevExpress.Xpo.DB.SortingDirection.Ascending));

                    if (collection_Izplacila.Count == 0) memRezultat.Text += "Uporabnik: " + usr.Lastname + " nima izplacil " + usr.UpravicenDoKVP.ToString() + "\r\n";



                    foreach (Izplacila izpl in collection_Izplacila)
                    {
                        if (izpl.DatumOd >= new DateTime(2019, 12, 1))
                        {
                            dTockePredlagateljKVP = GetSumForMonth(izpl.DatumOd, izpl.DatumDo, izpl.IdUser.Id, true);
                            dTockeRealizatorKVP = GetSumForMonth(izpl.DatumOd, izpl.DatumDo, izpl.IdUser.Id, false);


                            if (dTockePredlagateljKVP != izpl.PredlagateljT)
                            {
                                //memNotes.Text += "Mesec: " + izpl.Mesec + ", Točki predlagatelja " + izpl.IdUser.Id.ToString() + ", " + izpl.ImePriimek + " sta različni: PredlagateljT: " + izpl.PredlagateljT.ToString() + " Predlagatelj KVP: " + dTockePredlagateljKVP.ToString() + "\r\n";
                            }

                            if (dTockeRealizatorKVP != izpl.RealizatorT)
                            {
                                //memNotes.Text += "Mesec: " + izpl.Mesec + ", Točki Realizatorja " + izpl.IdUser.Id.ToString() + ", " + izpl.ImePriimek + " sta različni: RealizatorT: " + izpl.RealizatorT.ToString() + " Realizator KVP: " + dTockeRealizatorKVP.ToString() + "\r\n";
                            }

                            dVsotaCMesecIzplacila = dTockePredlagateljKVP + dTockeRealizatorKVP + izpl.PrenosIzPrejsnjegaMeseca;

                            if ((dVsotaCMesecIzplacila != izpl.VsotaT) && (izpl.Notes == null))
                            {
                                //memNotes.Text += "Mesec: " + izpl.Mesec + ", Vsoti Realizatorja " + izpl.IdUser.Id.ToString() + ", " + izpl.ImePriimek + " sta različni: VsotaT: " + izpl.VsotaT.ToString() + " Vsota KVP: " + dVsotaCMesecIzplacila.ToString() + "\r\n";
                            }

                            if (iLastIzpl != null)
                            {
                                if ((iLastIzpl.PrenosTvNaslednjiMesec != izpl.PrenosIzPrejsnjegaMeseca) && (izpl.Notes == null))
                                {
                                    //memNotes.Text += "User: " + izpl.IDPrijave.ToString() + " - NAPAČEN PRENOS IZ PREJŠNJEGA MESECA Userja " + izpl.IdUser.Id.ToString() + ", " + izpl.ImePriimek + " sta različni: Prenos iz prejšnjega meseca: " + iLastIzpl.Mesec + " = " + iLastIzpl.PrenosTvNaslednjiMesec.ToString() + " v naslednji mesec: " + izpl.Mesec + " = " + izpl.PrenosIzPrejsnjegaMeseca.ToString() + "\r\n";
                                    if (izpl.DatumDo.Month != 11)
                                    {
                                        sMeseci += izpl.Mesec + ",";
                                        bExistNote = true;
                                        dRazlika = iLastIzpl.PrenosTvNaslednjiMesec - izpl.PrenosIzPrejsnjegaMeseca;
                                        izpl.NePreneseneTocke = dRazlika;
                                        izpl.Notes = "PRENOS Na November: " + dRazlika;
                                        dNePreneseneTocke += dRazlika;
                                        payoutRepo.SavePayout(izpl);
                                    }
                                    else
                                    {
                                        bExistNote = true;
                                        izpl.PrenosIzPrejsnjegaMeseca = iLastIzpl.PrenosTvNaslednjiMesec;
                                        izpl.VsotaT += iLastIzpl.PrenosTvNaslednjiMesec;
                                        payoutRepo.SavePayout(izpl);
                                    }
                                }
                            }

                            iLastIzpl = izpl;
                        }
                    }

                    if (iLastIzpl != null && bExistNote)
                    {
                        if (iLastIzpl.DatumDo.Month != 11)
                        {
                            iLastIzpl.Notes = "Dodane Točke " + dNePreneseneTocke + "  iz Mesecev: " + sMeseci;
                            iLastIzpl.VsotaT = 0;
                            iLastIzpl.VsotaT = dNePreneseneTocke + iLastIzpl.PrenosTvNaslednjiMesec;
                            iLastIzpl.NePreneseneTocke = dNePreneseneTocke;
                            iLastIzpl.PrenosIzPrejsnjegaMeseca = dNePreneseneTocke;
                            CreateNewPayoutsForMonth(iLastIzpl, new DateTime(2019, 11, 1), true);
                        }
                        else
                        {
                            if (iLastIzpl.Notes == null)
                            {
                                if (dNePreneseneTocke > 0)
                                {
                                    iLastIzpl.Notes = "Dodane Točke " + dNePreneseneTocke + "  iz Mesecev: " + sMeseci;
                                    iLastIzpl.VsotaT += dNePreneseneTocke;
                                    iLastIzpl.NePreneseneTocke = dNePreneseneTocke;
                                    payoutRepo.SavePayout(iLastIzpl);
                                }
                            }
                        }



                        dVseTocke += dNePreneseneTocke;
                    }



                    memNotes.Text += (bExistNote) ? "\r\n" : "";


                }
            }
            catch (Exception ex)
            {
                errorRepo.SaveErrorLog(ex.ToString(), PrincipalHelper.GetUserPrincipal().ID);
                memRezultat.Text = ex.ToString();
            }

        }

        protected void btnUpdateDecember_Click(object sender, EventArgs e)
        {
            try
            {
                decimal dVseTocke = 0;

                CriteriaOperator filterCriteria = null;
                memNotes.Text = "";
                string filt = "UpravicenDoKVP = 1";
                //filt += "and Id in (1146)";
                //filt += "and Id in (1148)";
                //filt += "and Id in (454 ,146 ,151 ,193 ,218 ,241 ,298 ,304 ,316 ,345 ,409 ,425 ,426 ,440 ,461 ,479 ,499 ,557 ,574 ,609 ,624 ,629 ,643 ,645 ,670 ,676 ,700 ,719 ,724 ,748 ,810 ,895 ,1014,1041,1134,1148,1173,1217,1235,1245,1247,1357,1458,1486,1525,1555,1566,1715,5421,5457)";
                //filt += "and Id in (34, 72, 75, 104, 110, 117, 156, 161, 183, 193)";

                errorRepo.SaveErrorLog("Start preveri izplačila", PrincipalHelper.GetUserPrincipal().ID);
                filterCriteria = CriteriaOperator.Parse(filt);
                XPCollection<Users> usrSes = new XPCollection<Users>(session, filterCriteria);
                errorRepo.SaveErrorLog("Load users", PrincipalHelper.GetUserPrincipal().ID);

                foreach (Users usr in usrSes)
                {

           

                    decimal dTockePredlagateljKVP = 0;
                    decimal dTockeRealizatorKVP = 0;
          




                    filterCriteria = CriteriaOperator.Parse("IdUser = " + usr.Id);
                    XPCollection<Izplacila> collection_Izplacila = new XPCollection<Izplacila>(session, filterCriteria);
                    collection_Izplacila.Sorting.Add(new SortProperty("DatumOd", DevExpress.Xpo.DB.SortingDirection.Ascending));

                    if (collection_Izplacila.Count == 0) memRezultat.Text += "Uporabnik: " + usr.Lastname + " nima izplacil " + usr.UpravicenDoKVP.ToString() + "\r\n";


                    dTockePredlagateljKVP = GetSumForMonth(new DateTime(2019, 12,1), new DateTime(2019,12,31,23,23,23), usr.Id, true);
                    dTockeRealizatorKVP = GetSumForMonth(new DateTime(2019, 12, 1), new DateTime(2019, 12, 31, 23, 23, 23), usr.Id, false);

                    if ((dTockePredlagateljKVP != 0) || (dTockeRealizatorKVP != 0))
                    {
                        Izplacila izpl = GetIzplacilaByUserId(usr.Id, "December");



                        if (izpl == null)
                        {
                            Izplacila izpA = new Izplacila(session);

                            izpA.IdUser = usr;
                            izpA.DatumDo = new DateTime(2019, 12, 31);
                            izpA.DatumOd = new DateTime(2019, 12, 1);
                            izpA.Mesec = "December";
                            izpA.Leto = 2019;
                            izpA.PrenosIzPrejsnjegaMeseca = payoutRepo.GetPayoutsForMonthAndYearAndUserId("November", 2019, izpA.IdUser).PrenosIzPrejsnjegaMeseca;
                            izpA.PredlagateljT = dTockePredlagateljKVP;
                            izpA.RealizatorT = dTockeRealizatorKVP;
                            izpA.VsotaT = dTockeRealizatorKVP + dTockePredlagateljKVP + izpA.PrenosIzPrejsnjegaMeseca;
                            izpA.IzplaciloVMesecu = Convert.ToDecimal(0);
                            izpA.PrenosTvNaslednjiMesec = Convert.ToDecimal(0);
                            izpA.DatumIzracuna = DateTime.Now;
                            izpA.IDPrijave = 0;
                            izpA.ts = DateTime.Now;

                            string sImePriimek = izpA.IdUser.Firstname == null ? "" : izpA.IdUser.Firstname;
                            sImePriimek += " " + (izpA.IdUser.Lastname == null ? "" : izpA.IdUser.Lastname);

                            izpA.ImePriimek = sImePriimek;

                            izpA.Save();
                        }
                        else
                        {
                            izpl.PredlagateljT = dTockePredlagateljKVP;
                            izpl.RealizatorT = dTockeRealizatorKVP;
                            izpl.VsotaT = dTockeRealizatorKVP + dTockePredlagateljKVP + izpl.PrenosIzPrejsnjegaMeseca;
                            izpl.IDPrijave = 3517;
                            izpl.ts = DateTime.Now;
                            izpl.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorRepo.SaveErrorLog(ex.ToString(), PrincipalHelper.GetUserPrincipal().ID);
                memRezultat.Text = ex.ToString();
            }

        }

        protected void btnCheckPointSumWithPayout_Click(object sender, EventArgs e)
        {
            try
            {


                CriteriaOperator filterCriteria = null;
                memNotes.Text = "";
                string filt = "UpravicenDoKVP = 1";
                //filt += "and Id in (39)";
                //filt += "and Id in (1148)";
                //filt += "and Id in (454 ,146 ,151 ,193 ,218 ,241 ,298 ,304 ,316 ,345 ,409 ,425 ,426 ,440 ,461 ,479 ,499 ,557 ,574 ,609 ,624 ,629 ,643 ,645 ,670 ,676 ,700 ,719 ,724 ,748 ,810 ,895 ,1014,1041,1134,1148,1173,1217,1235,1245,1247,1357,1458,1486,1525,1555,1566,1715,5421,5457)";
                //filt += "and Id in (34, 72, 75, 104, 110, 117, 156, 161, 183, 193)";

                errorRepo.SaveErrorLog("Start preveri izplačila", PrincipalHelper.GetUserPrincipal().ID);
                filterCriteria = CriteriaOperator.Parse(filt);
                XPCollection<Users> usrSes = new XPCollection<Users>(session, filterCriteria);
                errorRepo.SaveErrorLog("Load users", PrincipalHelper.GetUserPrincipal().ID);
                Int32 iCnt = 0;

                foreach (Users usr in usrSes)
                {

                    decimal dTockePredlagateljKVP = 0;
                    decimal dTockeRealizatorKVP = 0;
                    decimal dVseTocke = 0;
                    decimal dVsaIzplacilaVEUR = 0;

                    Izplacila iLastIzpl = null;

                    bool bExistNote = false;

                    filterCriteria = CriteriaOperator.Parse("IdUser = " + usr.Id);
                    XPCollection<Izplacila> collection_Izplacila = new XPCollection<Izplacila>(session, filterCriteria);
                    collection_Izplacila.Sorting.Add(new SortProperty("DatumOd", DevExpress.Xpo.DB.SortingDirection.Ascending));

                    if (collection_Izplacila.Count == 0) memRezultat.Text += "Uporabnik: " + usr.Lastname + " nima izplacil " + usr.UpravicenDoKVP.ToString() + "\r\n";

                    iCnt++;

                    foreach (Izplacila izpl in collection_Izplacila)
                    {
                        Int32 iCurrentMonthPredlagatelj = 0;
                        Int32 iCurrentMonthRealizator = 0;

                        if (izpl.DatumOd >= new DateTime(2019, 1, 1))
                        {
                            dTockePredlagateljKVP = GetSumForMonth(izpl.DatumOd, izpl.DatumDo, izpl.IdUser.Id, true);
                            dTockeRealizatorKVP = GetSumForMonth(izpl.DatumOd, izpl.DatumDo, izpl.IdUser.Id, false);

                            // preveri kaj je prenesel iz starega sistema
                            if (izpl.DatumDo.Month == 1) dVseTocke += izpl.PrenosIzPrejsnjegaMeseca;

                            if ((dTockePredlagateljKVP != izpl.PredlagateljT) && (izpl.DatumDo.Month != 11))
                            {
                                bExistNote = true;
                                iCurrentMonthPredlagatelj = izpl.DatumDo.Month;
                                izpl.PredlagateljT = dTockePredlagateljKVP;
                                izpl.Notes = "Uskladitev točk Predlagatelj: " + dTockePredlagateljKVP;
                                memNotes.Text += "Mesec: " + izpl.Mesec + ", Točki predlagatelja " + izpl.IdUser.Id.ToString() + ", " + izpl.ImePriimek + " sta različni: PredlagateljT: " + izpl.PredlagateljT.ToString() + " Predlagatelj KVP: " + dTockePredlagateljKVP.ToString() + "\r\n";
                            }

                            if ((dTockeRealizatorKVP != izpl.RealizatorT) && (izpl.DatumDo.Month != 11))
                            {
                                bExistNote = true;
                                iCurrentMonthRealizator = izpl.DatumDo.Month;
                                izpl.RealizatorT = dTockeRealizatorKVP;
                                if (iCurrentMonthPredlagatelj == iCurrentMonthRealizator)
                                {
                                    izpl.Notes += ", Uskladitev točk Realizator: " + dTockeRealizatorKVP;
                                }
                                else
                                {
                                    izpl.Notes = "Uskladitev točk Realizator: " + dTockeRealizatorKVP;
                                }
                                memNotes.Text += "Mesec: " + izpl.Mesec + ", Točki Realizatorja " + izpl.IdUser.Id.ToString() + ", " + izpl.ImePriimek + " sta različni: RealizatorT: " + izpl.RealizatorT.ToString() + " Realizator KVP: " + dTockeRealizatorKVP.ToString() + "\r\n";
                            }

                            //if (izpl.DatumDo.Month != 11)
                            //{
                            dVseTocke += dTockePredlagateljKVP + dTockeRealizatorKVP;
                            //}
                            dVsaIzplacilaVEUR += izpl.IzplaciloVMesecu;

                            payoutRepo.SavePayout(izpl);
                        }
                    }


                    decimal dIzp = Math.Floor(dVseTocke / 500);

                    decimal dIzpl2 = 55 * dIzp;

                    if (dIzpl2 != dVsaIzplacilaVEUR)
                    {
                        memNotes.Text += "Oseba " + usr.Id.ToString() + ", " + usr.Firstname + " " + usr.Lastname + " je dobil napačno izplačilo glede na točke: " + dIzpl2 + " Izplačilo v EUR: " + dVsaIzplacilaVEUR.ToString() + "\r\n";
                    }

                    memNotes.Text += (bExistNote) ? "\r\n" : "";

                    if (iCnt % 100 == 0)
                    {
                        errorRepo.SaveErrorLog("Current Cnt " + iCnt, PrincipalHelper.GetUserPrincipal().ID);
                    }

                }

            }
            catch (Exception ex)
            {
                errorRepo.SaveErrorLog(ex.ToString(), PrincipalHelper.GetUserPrincipal().ID);
                memRezultat.Text = ex.ToString();
            }

        }
        protected void bntUskladitev_Click(object sender, EventArgs e)
        {
            try
            {
                decimal dVseTocke = 0;

                CriteriaOperator filterCriteria = null;
                memNotes.Text = "";
                string filt = "UpravicenDoKVP = 1";
                //filt += "and Id in (44)";
                //filt += "and Id in (1148)";
                //filt += "and Id in (454 ,146 ,151 ,193 ,218 ,241 ,298 ,304 ,316 ,345 ,409 ,425 ,426 ,440 ,461 ,479 ,499 ,557 ,574 ,609 ,624 ,629 ,643 ,645 ,670 ,676 ,700 ,719 ,724 ,748 ,810 ,895 ,1014,1041,1134,1148,1173,1217,1235,1245,1247,1357,1458,1486,1525,1555,1566,1715,5421,5457)";
                //filt += "and Id in (34, 72, 75, 104, 110, 117, 156, 161, 183, 193)";

                errorRepo.SaveErrorLog("Start preveri izplačila", PrincipalHelper.GetUserPrincipal().ID);
                filterCriteria = CriteriaOperator.Parse(filt);
                XPCollection<Users> usrSes = new XPCollection<Users>(session, filterCriteria);
                errorRepo.SaveErrorLog("Load users", PrincipalHelper.GetUserPrincipal().ID);
                Int32 iCnt = 0;

                foreach (Users usr in usrSes)
                {


                    decimal dVsotaCMesecIzplacila = 0;

                    decimal dTockePredlagateljKVP = 0;
                    decimal dTockeRealizatorKVP = 0;
                    decimal dRazlika = 0;
                    decimal dNePreneseneTocke = 0;
                    string sMeseci = "";




                    Izplacila iLastIzpl = null;

                    bool bExistNote = false;

                    filterCriteria = CriteriaOperator.Parse("IdUser = " + usr.Id);
                    XPCollection<Izplacila> collection_Izplacila = new XPCollection<Izplacila>(session, filterCriteria);
                    collection_Izplacila.Sorting.Add(new SortProperty("DatumOd", DevExpress.Xpo.DB.SortingDirection.Ascending));

                    if (collection_Izplacila.Count == 0) memRezultat.Text += "Uporabnik: " + usr.Lastname + " nima izplacil " + usr.UpravicenDoKVP.ToString() + "\r\n";

                    iCnt++;

                    foreach (Izplacila izpl in collection_Izplacila)
                    {
                        Int32 iCurrentMonthPredlagatelj = 0;
                        Int32 iCurrentMonthRealizator = 0;

                        if (izpl.DatumOd >= new DateTime(2019, 1, 1))
                        {
                            dTockePredlagateljKVP = GetSumForMonth(izpl.DatumOd, izpl.DatumDo, izpl.IdUser.Id, true);
                            dTockeRealizatorKVP = GetSumForMonth(izpl.DatumOd, izpl.DatumDo, izpl.IdUser.Id, false);


                            if ((dTockePredlagateljKVP != izpl.PredlagateljT) && (izpl.DatumDo.Month != 11))
                            {
                                bExistNote = true;
                                iCurrentMonthPredlagatelj = izpl.DatumDo.Month;
                                izpl.PredlagateljT = dTockePredlagateljKVP;
                                izpl.Notes = "Uskladitev točk Predlagatelj: " + dTockePredlagateljKVP;
                                memNotes.Text += "Mesec: " + izpl.Mesec + ", Točki predlagatelja " + izpl.IdUser.Id.ToString() + ", " + izpl.ImePriimek + " sta različni: PredlagateljT: " + izpl.PredlagateljT.ToString() + " Predlagatelj KVP: " + dTockePredlagateljKVP.ToString() + "\r\n";
                            }

                            if ((dTockeRealizatorKVP != izpl.RealizatorT) && (izpl.DatumDo.Month != 11))
                            {
                                bExistNote = true;
                                iCurrentMonthRealizator = izpl.DatumDo.Month;
                                izpl.RealizatorT = dTockeRealizatorKVP;
                                if (iCurrentMonthPredlagatelj == iCurrentMonthRealizator)
                                {
                                    izpl.Notes += ", Uskladitev točk Realizator: " + dTockeRealizatorKVP;
                                }
                                else
                                {
                                    izpl.Notes = "Uskladitev točk Realizator: " + dTockeRealizatorKVP;
                                }



                                memNotes.Text += "Mesec: " + izpl.Mesec + ", Točki Realizatorja " + izpl.IdUser.Id.ToString() + ", " + izpl.ImePriimek + " sta različni: RealizatorT: " + izpl.RealizatorT.ToString() + " Realizator KVP: " + dTockeRealizatorKVP.ToString() + "\r\n";
                            }

                            payoutRepo.SavePayout(izpl);
                        }
                    }




                    memNotes.Text += (bExistNote) ? "\r\n" : "";

                    if (iCnt % 100 == 0)
                    {
                        errorRepo.SaveErrorLog("Current Cnt " + iCnt, PrincipalHelper.GetUserPrincipal().ID);
                    }

                }
            }
            catch (Exception ex)
            {
                errorRepo.SaveErrorLog(ex.ToString(), PrincipalHelper.GetUserPrincipal().ID);
                memRezultat.Text = ex.ToString();
            }

        }

        protected void btnPreveriIzplacila_Click(object sender, EventArgs e)
        {
            CriteriaOperator filterCriteria = null;

            string filt = "UpravicenDoKVP = 1";
            //filt += "and Id in (961)";
            filt += "and Id in (557)";
            //filt += "and Id in (133 ,146 ,151 ,193 ,218 ,241 ,298 ,304 ,316 ,345 ,409 ,425 ,426 ,440 ,461 ,479 ,499 ,557 ,574 ,609 ,624 ,629 ,643 ,645 ,670 ,676 ,700 ,719 ,724 ,748 ,810 ,895 ,1014,1041,1134,1148,1173,1217,1235,1245,1247,1357,1458,1486,1525,1555,1566,1715,5421,5457)";

            errorRepo.SaveErrorLog("Start preveri izplačila", PrincipalHelper.GetUserPrincipal().ID);
            filterCriteria = CriteriaOperator.Parse(filt);
            XPCollection<Users> usrSes = new XPCollection<Users>(session, filterCriteria);
            errorRepo.SaveErrorLog("Load users", PrincipalHelper.GetUserPrincipal().ID);

            foreach (Users usr in usrSes)
            {

                decimal dPrenosIzPrejMeseca = 0;
                decimal dVsotaCMesec = 0;
                decimal dRazlikaTock = 0;
                decimal dPrenosVnaslednjiMesec = 0;
                decimal dLastmonth = 0;
                Izplacila iLastIzpl = null;

                bool bExistDate = false;


                filterCriteria = CriteriaOperator.Parse("IdUser = " + usr.Id);

                dVsotaCMesec = 0;

                errorRepo.SaveErrorLog("Load user " + usr.Id, PrincipalHelper.GetUserPrincipal().ID);

                XPCollection<Izplacila> collection_Izplacila = new XPCollection<Izplacila>(session, filterCriteria);
                collection_Izplacila.Sorting.Add(new SortProperty("DatumOd", DevExpress.Xpo.DB.SortingDirection.Ascending));

                if (collection_Izplacila.Count == 0) memRezultat.Text += "Uporabnik: " + usr.Lastname + " nima izplacil " + usr.UpravicenDoKVP.ToString() + "\r\n";



                foreach (Izplacila izpl in collection_Izplacila)
                {
                    if ((izpl.PrenosIzPrejsnjegaMeseca != dPrenosIzPrejMeseca) && (izpl.DatumOd.Month != 1) && (izpl.PrenosIzPrejsnjegaMeseca == 0))
                    {
                        if (dRazlikaTock == 0) dRazlikaTock += dPrenosIzPrejMeseca;
                        dRazlikaTock += (izpl.VsotaT >= 500) ? izpl.PrenosTvNaslednjiMesec : izpl.VsotaT;
                    }

                    dPrenosVnaslednjiMesec = izpl.PrenosTvNaslednjiMesec != 0 ? izpl.PrenosTvNaslednjiMesec : 0;

                    dVsotaCMesec = izpl.PrenosIzPrejsnjegaMeseca + izpl.PredlagateljT + izpl.RealizatorT;



                    dPrenosIzPrejMeseca = dPrenosVnaslednjiMesec;
                    dLastmonth = izpl.DatumDo.Month;
                    iLastIzpl = izpl;
                }

                if (dRazlikaTock != 0)
                {
                    if (dLastmonth != 6) CreateNewPayout(usr, dRazlikaTock, new DateTime(2019, 6, 1), session, iLastIzpl);
                    if (dLastmonth == 6) UpdatePayout(dRazlikaTock, iLastIzpl);
                    memNotes.Text += "Premalo izplačilo za Id: " + usr.Id + " Ime: " + usr.Lastname + " " + usr.Firstname + ", št. Točk: " + dRazlikaTock + "\r\n";
                }
            }
        }

        private void UpdatePayout(decimal tocke, Izplacila iLastIzpl)
        {
            iLastIzpl.PrenosIzPrejsnjegaMeseca = tocke - iLastIzpl.VsotaT;
            iLastIzpl.VsotaT = tocke;
            iLastIzpl.ts = DateTime.Now;
            iLastIzpl.Save();
        }


        private void CreateNewPayout(Users user, decimal tocke, DateTime datumRealizacije, Session uowSession, Izplacila iLastIzpl)
        {

            Izplacila payout = new Izplacila(uowSession);
            payout.DatumOd = CommonMethods.GetFirstDayOfMonth(datumRealizacije);
            payout.DatumDo = CommonMethods.GetLastDayOfMonth(datumRealizacije);
            //payout.DatumIzracuna
            payout.IdUser = employeeRepo.GetEmployeeByID(user.Id, uowSession);
            payout.IzplaciloVMesecu = 0;
            payout.Leto = datumRealizacije.Date.Year;
            payout.Mesec = CommonMethods.GetDateTimeMonthByNumber(datumRealizacije.Date.Month);
            payout.PredlagateljT = 0;
            payout.PrenosIzPrejsnjegaMeseca = tocke;
            payout.PrenosTvNaslednjiMesec = 0;
            payout.RealizatorT = 0;
            payout.VsotaT = (payout.PrenosIzPrejsnjegaMeseca + payout.PredlagateljT + payout.RealizatorT);
            payout.ts = DateTime.Now;
            payout.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
            payout.Save();
            //payoutRepo.SavePayout(payout);
        }



        protected void CreateNewPayoutsForMonth(Izplacila izpl, DateTime dt, bool bAddBlankRecord, bool SetIzplacilo = false)
        {
            payoutRepo.SavePayoutsForNewMonth(new List<Izplacila>() { izpl }, dt, bAddBlankRecord, SetIzplacilo);
        }


        private bool ExistRecordForDate(DateTime dtExistDate, XPCollection<Izplacila> collection_Izplacila)
        {
            foreach (Izplacila izpl in collection_Izplacila)
            {
                if (izpl.DatumOd == dtExistDate) return true;
            }

            return false;
        }


        protected void btnUpdateZapadliRK_Click(object sender, EventArgs e)
        {
            kvpDocRepo.AutomaticOverDueRKChangeStatus();
        }

        protected void bntSendMail_Click(object sender, EventArgs e)
        {
            messageRepo.ProceesKVPsToSendEmployeeStatistic();
        }

        protected void btnDeleteDuplicateUsers_Click(object sender, EventArgs e)
        {
            CriteriaOperator filterCriteria = null;

            XPCollection<Users> usrColl = new XPCollection<Users>(session);

            for (int i = usrColl.Count - 1; i >= 0; i--)
            {
                Users usr = usrColl[i];
                {
                    usr.ExternalId = usr.ExternalId == null ? "" : usr.ExternalId;
                    filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + usr.Firstname.Trim().ToUpper() + "' and Upper(Lastname)='" + usr.Lastname.Trim().ToUpper() + "' and Upper(ExternalId)='" + usr.ExternalId.Trim().ToUpper() + "'");
                    XPCollection<Users> usrduplColl = new XPCollection<Users>(session, filterCriteria);

                    if (usrduplColl.Count > 1)
                    {

                        for (int j = usrduplColl.Count - 1; j >= 0; j--)
                        {
                            Users usrDbl = usrduplColl[j];
                            if (CheckIfUserCanBeDeleted(usrDbl))
                            {
                                usrDbl.Delete();
                                i--;
                            }
                        }

                    }
                }
            }
        }

        private bool CheckIfUserCanBeDeleted(Users usr)
        {
            CriteriaOperator filterCriteria = null;
            XPCollection<KVPDocument> KVPColl = null;

            filterCriteria = CriteriaOperator.Parse("Predlagatelj = ?", usr.Id);
            KVPColl = new XPCollection<KVPDocument>(session, filterCriteria);

            // predlagatelj
            if (KVPColl.Count > 0)
            {
                return false;
            }

            filterCriteria = CriteriaOperator.Parse("Realizator = ?", usr.Id);
            KVPColl = new XPCollection<KVPDocument>(session, filterCriteria);

            // realizator
            if (KVPColl.Count > 0)
            {
                return false;
            }

            filterCriteria = CriteriaOperator.Parse("IdUser = ?", usr.Id);
            XPCollection<Izplacila> KVPIzplacila = new XPCollection<Izplacila>(session, filterCriteria);
            // Izplacila
            if (KVPIzplacila.Count > 0)
            {
                return false;
            }

            filterCriteria = CriteriaOperator.Parse("IdUser = ?", usr.Id);
            XPCollection<KVPSkupina_Users> KVPSkupina = new XPCollection<KVPSkupina_Users>(session, filterCriteria);
            // Izplacila
            if (KVPSkupina.Count > 0)
            {

                for (int i = KVPSkupina.Count - 1; i >= 0; i--)
                {
                    KVPSkupina_Users usrSk = KVPSkupina[i];
                    usrSk.Delete();
                }
            }
            return true;
        }

        protected void btnGenerateListItemsEmail_Click(object sender, EventArgs e)
        {
            messageRepo = new MessageProcessorRepository(session);
            messageRepo.ProceesKVPsToSendEmployeeStatistic(session);
        }
    }





}