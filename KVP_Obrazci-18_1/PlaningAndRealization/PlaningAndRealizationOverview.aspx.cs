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
    public partial class PlaningAndRealizationOverview : ServerMasterPage
    {
        Session session = null;

        IPlaningAndRealizationRepository planRealRepo = null;

        XPCollection<PlanRealizacija> PlanRealData = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            session = XpoHelper.GetNewSession();

            planRealRepo = new PlaningAndRealizationRepository(session);

            XpoDSPlaning.Session = session;
            ASPxGridViewPlaning.Settings.GridLines = GridLines.Both;
            ASPxGridViewRealization.Settings.GridLines = GridLines.Both;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
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

        protected void ASPxGridViewKVPsPercentage_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Leto" || e.DataColumn.FieldName == "idKVPSkupina.Naziv" || e.DataColumn.FieldName == "idPlanRealizacija") return;

            string currentMonthNameShort = "";

            currentMonthNameShort = e.DataColumn.FieldName.Substring(5, 3);
           
            if (AllowHighlightPercentage(currentMonthNameShort, DateTime.Now.Month))
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
        }

        private bool AllowHighlightPercentage(string MonthName, int iCurrMonth)
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

        protected void ASPxGridViewPlaning_DataBinding(object sender, EventArgs e)
        {
            PlanRealData = planRealRepo.GetPlanRealizationByKVPGroupAndYearWithSumAndYTD(DateTime.Now.Year);
            (sender as ASPxGridView).DataSource = PlanRealData;
        }

        private void SetStyleForSumAndYTD(ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

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

        }
    }
}