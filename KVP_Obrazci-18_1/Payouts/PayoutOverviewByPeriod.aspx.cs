using DevExpress.Web;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Payouts
{
    public partial class PayoutOverviewByPeriod : ServerMasterPage
    {
        Session session;
        IPayoutsRepository payoutRepo = null;
        List<PayoutOverviewByPeriodModel> list = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            session = XpoHelper.GetNewSession();
            payoutRepo = new PayoutsRepository(session);
            list = new List<PayoutOverviewByPeriodModel>();
        }

        private List<Field> GenerateFields(List<string> months)
        {
            List<Field> fields = new List<Field>();
            Field field = null;

            field = new Field();
            field.FieldName = "KVPSKupinaNaziv";
            field.FieldType = typeof(string);
            fields.Add(field);

            field = new Field();
            field.FieldName = "Zaposlen";
            field.FieldType = typeof(String);
            fields.Add(field);

            field = new Field();
            field.FieldName = "IdPregleda";
            field.FieldType = typeof(int);
            fields.Add(field);

            //if there is more than one month than is useful to create this field
            if (months.Count > 1)
            {
                field = new Field();
                field.FieldName = "VsotaTockObdobje";
                field.FieldType = typeof(decimal);
                fields.Add(field);
            }

            foreach (var item in months)
            {
                field = new Field();
                field.FieldName = item + "_" + "PrenosIzPrejsnjegaMeseca";
                field.FieldType = typeof(decimal);
                fields.Add(field);

                field = new Field();
                field.FieldName = item + "_" + "PredlagateljT";
                field.FieldType = typeof(decimal);
                fields.Add(field);

                field = new Field();
                field.FieldName = item + "_" + "RealizatorT";
                field.FieldType = typeof(decimal);
                fields.Add(field);

                field = new Field();
                field.FieldName = item + "_" + "VsotaT";
                field.FieldType = typeof(decimal);
                fields.Add(field);

                field = new Field();
                field.FieldName = item + "_" + "PrenosTvNaslednjiMesec";
                field.FieldType = typeof(decimal);
                fields.Add(field);
            }

            return fields;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RemoveSession("Seznam");
                RemoveSession("SummarySeznam");
            }
        }


        private void GenerateColumns(bool createDataSource = true)
        {
            List<string> selectedMonthsInyear = CommonMethods.GetListOfMonths(DateEditDateFrom.Date, DateEditDateTo.Date, true);
            bool showSumPointsThroughPeriod = (selectedMonthsInyear.Count > 1);
            List<Field> newFields = GenerateFields(selectedMonthsInyear);

            if (createDataSource)
            {
                list = payoutRepo.GetPayoutsByPeriod(DateEditDateFrom.Date, DateEditDateTo.Date);

                List<PayoutOverviewSummary> summaryList = new List<PayoutOverviewSummary>();

                PropertyInfo info = null;
                Type newType = MyTypeBuilder.CompileResultType(newFields);
                object obj = Activator.CreateInstance(newType);
                List<object> dataSource = new List<object>();


                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(newType);
                var instanceList = (IList)Activator.CreateInstance(constructedListType);

                decimal vsotaTockSkoziObdobje = 0;

                int i = 1;
                foreach (var item in list)
                {
                    vsotaTockSkoziObdobje = 0;
                    List<PropertyInfo> myPropInfo = obj.GetType().GetProperties().ToList();
                    info = myPropInfo.Where(prop => prop.Name.Equals("KVPSKupinaNaziv")).FirstOrDefault();
                    if (info != null) info.SetValue(obj, item.KVPSkupinaNaziv.ToString());

                    info = myPropInfo.Where(prop => prop.Name.Equals("Zaposlen")).FirstOrDefault();
                    if (info != null) info.SetValue(obj, item.Zaposlen.ToString());

                    info = myPropInfo.Where(prop => prop.Name.Equals("IdPregleda")).FirstOrDefault();
                    if (info != null) info.SetValue(obj, i);

                    foreach (var element in item.payoutOverviewList)
                    {

                        PayoutOverviewSummary sum = summaryList.Where(sl => sl.Mesec.Equals(element.Mesec) && sl.Leto.Equals(element.Leto)).FirstOrDefault();

                        if (sum == null)
                        {
                            sum = new PayoutOverviewSummary() { Mesec = element.Mesec, Leto = element.Leto/*, Items = new List<PayoutOverviewSummaryItem>()*/ };
                            summaryList.Add(sum);
                        }

                        sum.VsotaPrenosIzprejsnjegaMeseca += element.PrenosIzprejsnjegaMeseca;
                        info = myPropInfo.Where(prop => prop.Name.Contains(element.Mesec + "_" + element.Leto + "_PrenosIzPrejsnjegaMeseca")).FirstOrDefault();
                        if (info != null) info.SetValue(obj, element.PrenosIzprejsnjegaMeseca);

                        sum.VsotaPredlagateljT += element.PredlagateljT;
                        info = myPropInfo.Where(prop => prop.Name.Contains(element.Mesec + "_" + element.Leto + "_PredlagateljT")).FirstOrDefault();
                        if (info != null) info.SetValue(obj, element.PredlagateljT);

                        sum.VsotaRealizatorT += element.RealizatorT;
                        info = myPropInfo.Where(prop => prop.Name.Contains(element.Mesec + "_" + element.Leto + "_RealizatorT")).FirstOrDefault();
                        if (info != null) info.SetValue(obj, element.RealizatorT);

                        if (showSumPointsThroughPeriod) vsotaTockSkoziObdobje += element.VsotaT;
                        sum.VsotaVsotaT += element.VsotaT;
                        info = myPropInfo.Where(prop => prop.Name.Contains(element.Mesec + "_" + element.Leto + "_VsotaT")).FirstOrDefault();
                        if (info != null) info.SetValue(obj, element.VsotaT);

                        sum.VsotaPrenosVNaslednjiMesec += element.PrenosVNaslednjiMesec;
                        info = myPropInfo.Where(prop => prop.Name.Contains(element.Mesec + "_" + element.Leto + "_PrenosTvNaslednjiMesec")).FirstOrDefault();
                        if (info != null) info.SetValue(obj, element.PrenosVNaslednjiMesec);
                    }

                    if (showSumPointsThroughPeriod)
                    {
                        info = myPropInfo.Where(prop => prop.Name.Equals("VsotaTockObdobje")).FirstOrDefault();
                        if (info != null) info.SetValue(obj, vsotaTockSkoziObdobje);
                    }

                    i++;
                    instanceList.Add(obj);

                    obj = Activator.CreateInstance(newType);
                }
                Session["SummarySeznam"] = summaryList;
                Session["Seznam"] = instanceList;

            }
            ASPxGridViewPayoutOverViewByPeriod.Columns.Clear();

            GridViewDataTextColumn textColumn = new GridViewDataTextColumn();
            textColumn.Caption = "KVP Skupina";
            textColumn.FieldName = "KVPSKupinaNaziv";
            textColumn.Width = Unit.Pixel(220);

            ASPxGridViewPayoutOverViewByPeriod.Columns.Add(textColumn);

            textColumn = new GridViewDataTextColumn();
            textColumn.Caption = "Zaposlen";
            textColumn.FieldName = "Zaposlen";
            textColumn.Width = Unit.Pixel(240);

            ASPxGridViewPayoutOverViewByPeriod.Columns.Add(textColumn);

            if (showSumPointsThroughPeriod)
            {
                textColumn = new GridViewDataTextColumn();
                textColumn.Caption = "Vsota točko skozi obdobje";
                textColumn.FieldName = "VsotaTockObdobje";
                textColumn.Width = Unit.Pixel(140);

                ASPxGridViewPayoutOverViewByPeriod.Columns.Add(textColumn);
            }

            foreach (var item in selectedMonthsInyear)
            {
                GridViewBandColumn bandColumn = new GridViewBandColumn(item.Replace("_", " "));

                textColumn = new GridViewDataTextColumn();
                textColumn.Caption = "Prenos iz prej. meseca";
                textColumn.FieldName = item + "_PrenosIzPrejsnjegaMeseca";
                textColumn.Width = Unit.Pixel(150);

                bandColumn.Columns.Add(textColumn);

                textColumn = new GridViewDataTextColumn();
                textColumn.Caption = "Predlagatelj točke";
                textColumn.FieldName = item + "_PredlagateljT";
                textColumn.Width = Unit.Pixel(150);

                bandColumn.Columns.Add(textColumn);

                textColumn = new GridViewDataTextColumn();
                textColumn.Caption = "Relizator točke";
                textColumn.FieldName = item + "_RealizatorT";
                textColumn.Width = Unit.Pixel(150);

                bandColumn.Columns.Add(textColumn);

                textColumn = new GridViewDataTextColumn();
                textColumn.Caption = "Vsota točk";
                textColumn.FieldName = item + "_VsotaT";
                textColumn.Width = Unit.Pixel(150);

                bandColumn.Columns.Add(textColumn);

                textColumn = new GridViewDataTextColumn();
                textColumn.Caption = "Prenos v naslj. mesec";
                textColumn.FieldName = item + "_PrenosTvNaslednjiMesec";
                textColumn.Width = Unit.Pixel(150);

                bandColumn.Columns.Add(textColumn);

                ASPxGridViewPayoutOverViewByPeriod.Columns.Add(bandColumn);
            }

            if (createDataSource)
                ASPxGridViewPayoutOverViewByPeriod.DataBind();
        }

        protected void ASPxGridViewPayoutOverViewByPeriod_DataBinding(object sender, EventArgs e)
        {


            (sender as ASPxGridView).DataSource = Session["Seznam"];
            ASPxGridViewPayoutOverViewByPeriod.Settings.GridLines = GridLines.Both;

            #region table
            List<PayoutOverviewSummary> list = (List<PayoutOverviewSummary>)Session["SummarySeznam"];
            SummaryContentTable.Rows.Clear();
            if (list != null && list.Count > 0)
            {
                HtmlTableRow row = null;

                row = new HtmlTableRow();
                HtmlTableCell cell = new HtmlTableCell();

                cell.InnerText = "";
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.InnerText = "Vsota prenos iz prejšnjega meseca";
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.InnerText = "Vsota predlagatelji";
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.InnerText = "Vsota realizatorji";
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.InnerText = "Vsote vseh točk";
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.InnerText = "Vsota prenos točk v naslednji mesec";
                row.Cells.Add(cell);

                SummaryContentTable.Rows.Add(row);

                foreach (var item in list)
                {
                    row = new HtmlTableRow();

                    cell = new HtmlTableCell();
                    cell.InnerText = item.Mesec + " " + item.Leto.ToString();
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.InnerText = item.VsotaPrenosIzprejsnjegaMeseca.ToString();
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.InnerText = item.VsotaPredlagateljT.ToString();
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.InnerText = item.VsotaRealizatorT.ToString();
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.InnerText = item.VsotaVsotaT.ToString();
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.InnerText = item.VsotaPrenosVNaslednjiMesec.ToString();
                    row.Cells.Add(cell);


                    SummaryContentTable.Rows.Add(row);
                }
            }
            #endregion
        }

        protected void ASPxGridViewPayoutOverViewByPeriod_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            /*if (e.Parameters == "GetData")
            {
                RemoveSession("Seznam");
                RemoveSession("SummarySeznam");
                GenerateColumns();
            }*/

            //SummaryContentTable.Rows.Add()
        }

        protected void PayoutOverViewByPeriodCallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "GetData")
            {
                RemoveSession("Seznam");
                RemoveSession("SummarySeznam");
                GenerateColumns();
                PayoutOverViewByPeriodCallbackPanel.JSProperties["cpGridViewRowCount"] = ASPxGridViewPayoutOverViewByPeriod.VisibleRowCount;
            }
        }

        protected void ASPxGridViewPayoutOverViewByPeriod_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if (e.CallbackName == "APPLYCOLUMNFILTER")
                GenerateColumns(false);
        }
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            PayoutOverviewByPeriodExporter.WriteCsvToResponse();
        }


    }
}