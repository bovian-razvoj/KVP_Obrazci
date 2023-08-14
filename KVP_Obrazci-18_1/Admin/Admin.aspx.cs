using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Admin
{
    public partial class Admin : ServerMasterPage
    {
        IKVPDocumentRepository kvpDocRepo;
        IPayoutsRepository payoutRepo;
        IKodeksToEKVPRepository kodeksRepo;
        ILocationRepository locationRepo;
        IKVPGroupsRepository kvpGroupRepo = null;

        Session session = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserSuperAdmin() && !PrincipalHelper.IsUserAdmin()) RedirectHome();

            session = XpoHelper.GetNewSession();

            kvpDocRepo = new KVPDocumentRepository(session);
            payoutRepo = new PayoutsRepository(session);
            kodeksRepo = new KodeksToEKVPRepository();
            locationRepo = new LocationRepository(session);
            kvpGroupRepo = new KVPGroupsRepository(session);
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

                    Izplacila izpl = GetIzplacilaByUserId(cUsrR.Id);

                    if (izpl == null)
                    {



                        // izplačilo Avgust 2018
                        Izplacila izpA = new Izplacila(session);

                        izpA.IdUser = cUsrR;
                        izpA.DatumDo = new DateTime(2019, 1, 31);
                        izpA.DatumOd = new DateTime(2019, 1, 1);
                        izpA.Mesec = "Januar";
                        izpA.Leto = 2019;
                        izpA.PrenosIzPrejsnjegaMeseca = Convert.ToDecimal(zt.Prenos);
                        izpA.PredlagateljT = Convert.ToDecimal(0);
                        izpA.RealizatorT = Convert.ToDecimal(0);
                        izpA.VsotaT = Convert.ToDecimal(0);
                        izpA.IzplaciloVMesecu = Convert.ToDecimal(0);
                        izpA.PrenosTvNaslednjiMesec = Convert.ToDecimal(0);
                        izpA.DatumIzracuna = DateTime.Now;
                        izpA.IDPrijave = 0;
                        izpA.ts = DateTime.Now;

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

        private Izplacila GetIzplacilaByUserId(Int32 iUser)
        {
            CriteriaOperator filterCriteria = null;

            filterCriteria = CriteriaOperator.Parse("IdUser = " + iUser + " and Mesec='Januar' ");

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
                        if (KVPexcel.Realiziral == "0")
                        {
                            if (KVPexcel.Zavrnil == "0")
                            {

                                KVPDocument KVPdoc = new KVPDocument(session);

                                //if (isInserted)
                                //{
                                //    KVPEx = GetByZaporednaStevilka(KVPexcel.ZapSt.ToString());

                                //    if (KVPEx == null) continue;
                                //}


                                KVPdoc.DatumVnosa = (KVPexcel.DatumVnosa == null) ? new DateTime(2018, 1, 1) : KVPexcel.DatumVnosa;
                                KVPdoc.StevilkaKVP = KVPexcel.StKVP;




                                if ((KVPexcel.Uporabnik != null) && (KVPexcel.Uporabnik.Length > 0))
                                {
                                    //if (isInserted)
                                    //{
                                    //    if (KVPEx.Uporabnik == null) KVPEx.Uporabnik = "";
                                    //    if (KVPEx.Uporabnik.Trim().ToUpper() != KVPexcel.Uporabnik.Trim().ToUpper())
                                    //    {
                                    //        memNotes.Text += "Razlika Predlagatelj: " + KVPEx.Uporabnik.Trim().ToUpper() + " / " + KVPexcel.Uporabnik.Trim().ToUpper() + " \r\n";
                                    //        KVPexcel.Uporabnik = KVPEx.Uporabnik.Trim().ToUpper();
                                    //    }
                                    //}


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

                                        if ((KVPexcel.Realiziral != null) && (KVPexcel.Realiziral == "1"))
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




                                if ((KVPexcel.Zavrnil != null) && (KVPexcel.Zavrnil == "1"))
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
            if (KVPexcel.Realiziral == null) KVPexcel.Realiziral = "0";
            if (KVPexcel.Realiziral.Trim().Length == 0) KVPexcel.Realiziral = "0";
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
            if (KVPexcel.Zavrnil == " ") KVPexcel.Zavrnil = "0";
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

            InsrtKVPDocuments(false);


            //Label_Error.Text = "Število spremenjenih zapisov InvoiceUpdateIzdajnoSkladisceDobropisi: " + iSteviloSpremenjenihZapisov;

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
            kodeksRepo.MergeKodeks_eKVP(session);
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
            filterCriteria = CriteriaOperator.Parse("LastStatusId = 8");

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

        private void SetDatumZakljuceneIdeje(Int32 iIdKVP, KVPDocument kvpDoc)
        {
            CriteriaOperator filterCriteria = null;
            filterCriteria = CriteriaOperator.Parse("idKVPDocument = " + iIdKVP);

            XPCollection<KVP_Status> collStatus = new XPCollection<KVP_Status>(session, filterCriteria);
            collStatus.Sorting.Add(new SortProperty("ts", DevExpress.Xpo.DB.SortingDirection.Descending));

            if (collStatus.Count > 0)
            {
                if ((collStatus[0].idStatus.idStatus == 9) || (collStatus[0].idStatus.idStatus == 11))
                {
                    kvpDoc.DatumZakljuceneIdeje = collStatus[0].ts;
                    kvpDoc.Save();
                }
            }
        }

    }
}