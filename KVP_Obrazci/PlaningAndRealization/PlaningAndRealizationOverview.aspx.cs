using DevExpress.Web;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.PlaningAndRealization
{
    public class PlaningHelperModel
    {
        public int Id { get; set; }
        public int IdKVPSKupina { get; set; }
        public string Mesec { get; set; }
        public string KVPSkKoda { get; set; }

    }

    public partial class PlaningAndRealizationOverview : ServerMasterPage
    {
        Session session = null;

        List<PlaningHelperModel> tmpColl = new List<PlaningHelperModel>();

        IPlaningAndRealizationRepository planRealRepo = null;

        XPCollection<PlanRealizacija> PlanRealData = null;

        bool bAktivnost = true;



        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            AllowUserWithRole(Enums.UserRole.SuperAdmin, Enums.UserRole.Admin, Enums.UserRole.Champion, Enums.UserRole.Leader);

            session = XpoHelper.GetNewSession();

            planRealRepo = new PlaningAndRealizationRepository(session);

            XpoDSPlaning.Session = session;
            ASPxGridViewPlaning.Settings.GridLines = GridLines.Both;
            ASPxGridViewRealization.Settings.GridLines = GridLines.Both;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["SelectedYear"] != null)
                {
                    ComboBoxYear.SelectedIndex = ComboBoxYear.Items.IndexOfValue(Session["SelectedYear"]);
                }
                else
                {
                    Session["SelectedYear"] = DateTime.Now.Year.ToString();
                    ComboBoxYear.SelectedIndex = ComboBoxYear.Items.IndexOfValue(Session["SelectedYear"]);
                }
            }

            planRealRepo.RefreshKVPRealizationPercentage();

            ASPxGridViewPlaning.DataBind();
            ASPxGridViewRealization.DataBind();
            ASPxGridViewKVPsPercentage.DataBind();


        }

        protected void ASPxGridViewPlaning_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            planRealRepo.SavePlaningFromBatchUpdate(e.UpdateValues);
            e.Handled = true;
            ASPxGridViewExporterPlaning.DataBind();
        }

        protected void ASPxGridViewPlaning_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void ASPxGridViewRealization_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void ASPxGridViewKVPsPercentage_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void btnExportCilji_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporterPlaning.FileName = "Plan_" + CommonMethods.GetTimeStamp();
            ASPxGridViewExporterPlaning.WriteCsvToResponse();
        }

        protected void btnExportRealizacija_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporterrealization.FileName = "Realizacija_" + CommonMethods.GetTimeStamp();
            ASPxGridViewExporterrealization.WriteCsvToResponse();
        }

        protected void btnExportPercentage_Click(object sender, EventArgs e)
        {
            ASPxGridViewExportPercentage.FileName = "Procenti_" + CommonMethods.GetTimeStamp();
            ASPxGridViewExportPercentage.WriteCsvToResponse();
        }


        protected void ASPxGridViewKVPsPercentage_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Leto" || e.DataColumn.FieldName == "idPlanRealizacija" || e.DataColumn.FieldName == "idKVPSkupina.Naziv")   return;

            string currentMonthNameShort = "";

            currentMonthNameShort = e.DataColumn.FieldName.Substring(5, 3);



            string gvName = ((ASPxGridView)sender).ClientInstanceName;

            if (AllowHighlightPercentage(currentMonthNameShort, DateTime.Now.Month))
            {
                if (gvName == "gridKVPsPercentage")
                {
                    if (CommonMethods.ParseDecimal(e.CellValue) < 51)
                    {
                        e.Cell.BackColor = Color.LightSalmon;
                        //e.Cell.ForeColor = Color.White;
                    }
                    else if (CommonMethods.ParseDecimal(e.CellValue) < 99)
                    {
                        e.Cell.BackColor = Color.LemonChiffon;
                    }
                    else if (CommonMethods.ParseDecimal(e.CellValue) >= 100)
                    {
                        e.Cell.BackColor = Color.LightGreen;
                        //e.Cell.ForeColor = Color.White;
                    }
                }

                if (gvName == "gridPlaning")
                {
                    if ((e.CellValue != null) && (CommonMethods.ParseDecimal(e.CellValue) == 0))
                    {
                        PlaningHelperModel phm = new PlaningHelperModel();
                        phm.Id = (int)(e.GetValue("idPlanRealizacija"));
                        phm.KVPSkKoda = (string)(e.GetValue("idKVPSkupina.Koda"));
                        phm.Mesec = (string)(e.DataColumn.FieldName);
                        tmpColl.Add(phm);

                    }
                }

                if ((gvName == "gridRealization") || (gvName == "gridPlaning"))
                {
                    if ((e.CellValue != null) && (CommonMethods.ParseDecimal(e.CellValue) == 0))
                    {
                        if (IsPlanKVPSkupineZero(tmpColl, e.DataColumn.FieldName, e)) e.Cell.BackColor = Color.LightGray;
                    }
                }

                if (gvName == "gridKVPsPercentage")
                {
                    if ((e.CellValue != null) && (CommonMethods.ParseDecimal(e.CellValue) == 0) && (Session["SelectedYear"].ToString() == DateTime.Now.Year.ToString()))
                    {
                        if (IsPlanKVPSkupineZero(tmpColl, e.DataColumn.FieldName, e)) e.Cell.BackColor = Color.White;
                    }
                }
            }
        }

        private bool IsPlanKVPSkupineZero(List<PlaningHelperModel> collPHM, string sMonth, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            string[] spl = sMonth.Split('_');
            sMonth = spl[1];
            string sKVPSk = e.GetValue("idKVPSkupina.Koda").ToString();
            foreach (PlaningHelperModel phm in collPHM)
            {
                spl = phm.Mesec.Split('_');
                if (spl.Length == 1) return false;
                string mPHM = spl[1];


                if ((mPHM == sMonth) && (sKVPSk == phm.KVPSkKoda)) return true;
            }

            return false;
        }

        private bool AllowHighlightPercentage(string MonthName, int iCurrMonth)
        {
            if (Session["SelectedYear"].ToString() == DateTime.Now.Year.ToString())
            {
                if ((MonthName == "Jan") && (iCurrMonth >= 1)) return true;
                if ((MonthName == "Feb") && (iCurrMonth >= 2)) return true;
                if ((MonthName == "Mar") && (iCurrMonth >= 3)) return true;
                if ((MonthName == "Apr") && (iCurrMonth >= 4)) return true;
                if ((MonthName == "Maj") && (iCurrMonth >= 5)) return true;
                if ((MonthName == "Jun") && (iCurrMonth >= 6)) return true;
                if ((MonthName == "Jul") && (iCurrMonth >= 7)) return true;
                if ((MonthName == "Avg") && (iCurrMonth >= 8)) return true;
                if ((MonthName == "Sep") && (iCurrMonth >= 9)) return true;
                if ((MonthName == "Okt") && (iCurrMonth >= 10)) return true;
                if ((MonthName == "Nov") && (iCurrMonth >= 11)) return true;
                if ((MonthName == "Dec") && (iCurrMonth >= 12)) return true;

                return false;
            }
            else
            {
                return true;
            }

        }

        protected void ASPxGridViewPlaning_DataBinding(object sender, EventArgs e)
        {
            PlanRealData = planRealRepo.GetPlanRealizationByKVPGroupAndYearWithSumAndYTD(CommonMethods.ParseInt(ComboBoxYear.Text));
            (sender as ASPxGridView).DataSource = PlanRealData;
        }

        private void SetStyleForSumAndYTD(ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            if ((e.GetValue("idKVPSkupina.Koda").ToString() != "SUMMONTH") && (e.GetValue("idKVPSkupina.Koda").ToString() != "YTDMONTH"))
            {
                bAktivnost = Convert.ToBoolean(e.GetValue("idKVPSkupina.Aktivnost"));
            }


            if ((e.GetValue("idKVPSkupina.Koda") != null) && (e.GetValue("idKVPSkupina.Koda").ToString() == "SUMMONTH"))
            {
                e.Row.Font.Bold = true;
            }

            if ((e.GetValue("idKVPSkupina.Koda") != null) && (e.GetValue("idKVPSkupina.Koda").ToString() == "YTDMONTH"))
            {
                e.Row.Font.Underline = true;
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9A33");
            }
        }

        protected void ASPxGridViewPlaning_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            SetStyleForSumAndYTD(e);
        }

        protected void ASPxGridViewRealization_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = PlanRealData;
        }

        protected void ASPxGridViewRealization_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            SetStyleForSumAndYTD(e);
        }

        protected void ASPxGridViewKVPsPercentage_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = PlanRealData;
        }

        protected void ASPxGridViewKVPsPercentage_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            SetStyleForSumAndYTD(e);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            planRealRepo.RefreshKVPRealizationPercentage();

            ASPxGridViewPlaning.DataBind();
            ASPxGridViewRealization.DataBind();
            ASPxGridViewKVPsPercentage.DataBind();

            this.Master.NavigationBarMain.DataBind();

        }

        protected void btnOpenYear_Click(object sender, EventArgs e)
        {
            planRealRepo.OpenNewYear(CommonMethods.ParseInt(ComboBoxYear.Text));

            Response.Redirect(Request.RawUrl);
        }

        protected void cbpCheckPlanAndRealizationForYear_Callback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter == "ValueChanged")
                PlanRealData = planRealRepo.GetPlanRealizationByKVPGroupAndYearWithSumAndYTD(CommonMethods.ParseInt(ComboBoxYear.Text));
            Session["SelectedYear"] = ComboBoxYear.Text;
            if (PlanRealData == null)
            {
                if (PrincipalHelper.IsUserAdmin() || PrincipalHelper.IsUserSuperAdmin())
                {
                    btnOpenYear.ClientVisible = true;
                }
            }
            else
            {
                btnOpenYear.ClientVisible = false;
            }

            ASPxGridViewPlaning.DataBind();
            ASPxGridViewRealization.DataBind();
            ASPxGridViewKVPsPercentage.DataBind();

            this.Master.NavigationBarMain.DataBind();

        }
    }
}