using DevExpress.Xpo;
using DevExpress.XtraPrinting;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Payouts
{
    public partial class PayoutOverview : ServerMasterPage
    {
        Session session;
        IPayoutsRepository payoutRepo;



        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            AllowUserWithRole(Enums.UserRole.SuperAdmin, Enums.UserRole.Admin);

            session = XpoHelper.GetNewSession();
            payoutRepo = new PayoutsRepository(session);

            XpoDSPayouts.Session = session;
            XpoDSEmployee.Session = session;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ComboBoxMonth.SelectedIndex = ComboBoxMonth.Items.IndexOfValue(DateTime.Now.AddMonths(-1).Month.ToString());
                ComboBoxYear.SelectedIndex = ComboBoxYear.Items.IndexOfValue(DateTime.Now.Year.ToString());
            }


            if (!String.IsNullOrEmpty(ComboBoxMonth.Text) && !String.IsNullOrEmpty(ComboBoxYear.Text))
                XpoDSPayouts.Criteria = "[Mesec]='" + ComboBoxMonth.Text + "' AND [Leto] = " + ComboBoxYear.Text + " AND [IdUser.Deleted] = 0";

            ASPxGridViewPayouts.Settings.GridLines = GridLines.Both;

        }

        protected void ASPxGridViewPayouts_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "ValueChanged")
            {
                if (String.IsNullOrEmpty(ComboBoxMonth.Text))
                {
                    XpoDSPayouts.Criteria = "[Leto] = " + ComboBoxYear.Text + (String.IsNullOrEmpty(ASPxGridLookupEmployee.Text) ? "" : " AND [ImePriimek]='" + ASPxGridLookupEmployee.Text + "'");
                }
                else
                {
                    XpoDSPayouts.Criteria = "[Mesec]='" + ComboBoxMonth.Text + "' AND [Leto] = " + ComboBoxYear.Text + (String.IsNullOrEmpty(ASPxGridLookupEmployee.Text) ? "" : " AND [ImePriimek]='" + ASPxGridLookupEmployee.Text + "'");
                }

                XpoDSPayouts.Criteria += "AND [IdUser.Deleted] = 0";

                ASPxGridViewPayouts.DataBind();
            }
            else if (e.Parameters == "StartPayoutProcedure")
            {
                //DateTime dtCurrentDate = DateTime.Now.AddMonths(-1);
                DateTime dtCurrentDate = DateTime.Now;

                string previousPreviousMonth = CommonMethods.GetDateTimeMonthByNumber(dtCurrentDate.AddMonths(-2).Month);
                int yearInPreviousPreviousMonth = dtCurrentDate.AddMonths(-2).Year;

                List<Izplacila> previousPreviousMonthPayouts = payoutRepo.GetPayoutsForMonthAndYear(previousPreviousMonth, yearInPreviousPreviousMonth);

                DateTime previousDateTimeMonth = dtCurrentDate.AddMonths(-1);
                string previousMonth = CommonMethods.GetDateTimeMonthByNumber(previousDateTimeMonth.Month);
                int yearInPreviousMonth = previousDateTimeMonth.Year;

                List<Izplacila> previousMonthPayouts = payoutRepo.GetPayoutsForMonthAndYear(previousMonth, yearInPreviousMonth);

                List<Izplacila> payoutsToAddInNewMonth = previousPreviousMonthPayouts.Where(ppmp => !previousMonthPayouts.Any(pmp => pmp.IdUser.Id == ppmp.IdUser.Id)).ToList();

                payoutRepo.UpdatePayoutsForNewMonth(previousMonthPayouts, previousDateTimeMonth);
                payoutRepo.SavePayoutsForNewMonth(payoutsToAddInNewMonth, previousDateTimeMonth, false);

                ASPxGridViewPayouts.DataBind();
            }
        }

        protected void btnGeneratePayouts_Click(object sender, EventArgs e)
        {
            string previousPreviousMonth = CommonMethods.GetDateTimeMonthByNumber(DateTime.Now.AddMonths(-2).Month);
            int yearInPreviousPreviousMonth = DateTime.Now.AddMonths(-2).Year;

            List<Izplacila> previousPreviousMonthPayouts = payoutRepo.GetPayoutsForMonthAndYear(previousPreviousMonth, yearInPreviousPreviousMonth);

            DateTime previousDateTimeMonth = DateTime.Now.AddMonths(-1);
            string previousMonth = CommonMethods.GetDateTimeMonthByNumber(previousDateTimeMonth.Month);
            int yearInPreviousMonth = previousDateTimeMonth.Year;

            List<Izplacila> previousMonthPayouts = payoutRepo.GetPayoutsForMonthAndYear(previousMonth, yearInPreviousMonth);

            // dobiš vse, ki niso v prejšnjem mesecu
            List<Izplacila> payoutsToAddInNewMonth = previousPreviousMonthPayouts.Where(ppmp => !previousMonthPayouts.Any(pmp => pmp.IdUser.Id == ppmp.IdUser.Id)).ToList();

            payoutRepo.UpdatePayoutsForNewMonth(previousMonthPayouts, previousDateTimeMonth);
            payoutRepo.SavePayoutsForNewMonth(payoutsToAddInNewMonth, previousDateTimeMonth, false);
        }

        string ReplaceDateForString(string sDate)
        {
            sDate = sDate.Replace(":", "-");
            sDate = sDate.Replace(" ", "");
            sDate = sDate.Replace(".", "-");
            sDate = sDate.Replace(",", "-");


            return sDate;

        }

        protected void btnCreateCSV_Click(object sender, EventArgs e)
        {

            ASPxGridViewPayouts.FilterExpression = "[IzplaciloVMesecu] > 0";
            ASPxGridViewPayouts.SettingsText.Title = ASPxGridViewPayouts.FilterExpression;
            ASPxGridViewPayouts.Columns["Notes"].Visible = false;


            string previousMonth = ComboBoxMonth.Items.FindByValue(DateTime.Now.AddMonths(-1).Month.ToString()).Text;
            int yearInPreviousMonth = CommonMethods.ParseInt(DateTime.Now.AddMonths(-1).Year.ToString());

            List<Izplacila> payouts = payoutRepo.GetPayoutsForMonthAndYear(previousMonth, yearInPreviousMonth);
            string sMonth = CommonMethods.GetDateTimeMonthByNumber(DateTime.Now.Month);

            var stringfile = @"PayOut_" + previousMonth + DateTime.Now.Year + "-" + ReplaceDateForString(DateTime.Now.ToString()) + ".csv";



            FileInfo fi = new FileInfo(HttpContext.Current.Server.MapPath(stringfile));
            if (!Directory.Exists(fi.DirectoryName)) Directory.CreateDirectory(fi.DirectoryName);

            File.AppendAllText(fi.FullName, "Šifra delavca;Priimek in ime;Znesek" + Environment.NewLine);


            ASPxGridViewExporterPayouts.WriteCsvToResponse(stringfile, new CsvExportOptionsEx() { ExportType = DevExpress.Export.ExportType.WYSIWYG });

            //foreach (Izplacila izpl in payouts)
            //{
            //    if (izpl.IzplaciloVMesecu > 0)
            //    {
            //        sPayout = izpl.IdUser.ExternalId.ToString() + ";" + izpl.IdUser.Lastname + " " + izpl.IdUser.Firstname + ";" + izpl.IzplaciloVMesecu + Environment.NewLine;
            //        File.AppendAllText(fi.FullName, sPayout);
            //    }
            //}

            //byte[] byteFile = File.ReadAllBytes(fi.FullName);
            //WriteDocumentToResponse(byteFile, "csv", false, stringfile);
        }

        private void WriteDocumentToResponse(byte[] documentData, string format, bool isInline, string fileName)
        {
            string contentType = "application/pdf";

            if (format == "png")
                contentType = "image/png";
            else if (format == "jpg" || format == "jpeg")
                contentType = "image/jpeg";

            string disposition = (isInline) ? "inline" : "attachment";

            Response.Clear();
            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", String.Format("{0}; filename={1}", disposition, fileName));
            Response.Charset = string.Empty;
            //Encoding for european characters
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            Response.BinaryWrite(documentData);
            Response.End();
            //Response.Flush(); // Sends all currently buffered output to the client.
            //Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            //HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
        }

        protected void btnExportPayouts_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporterPayouts.FileName = "Payouts_" + CommonMethods.GetTimeStamp();


            //XlsxExportOptionsEx ex = new XlsxExportOptionsEx();
            //ex.ExportType = DevExpress.Export.ExportType.DataAware;
            //ex.CustomizeCell += ex_CustomizeCell;
            ASPxGridViewExporterPayouts.WriteXlsxToResponse();



        }


        //void ex_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs ea)
        //{
        //    // Intentando colorear la fila
        //    if (ea.DataSourceRowIndex == 0)
        //    {

        //        ea.Formatting.BackColor = System.Drawing.Color.Green;
        //    }

        //    // generando el link para excell
        //    if (ea.ColumnFieldName == "IdUser.Firstname")
        //    {                                
        //        ea.Handled = true;
        //    }
        //}

    }
}