using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using KVP_Obrazci.Common;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Reports
{
    public partial class ReportPreview : ServerMasterPage
    {
        string printReport = "";
        int printID = -1;
        bool showPreview = false;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.DisableNavBar = true;

            printReport = CommonMethods.Trim(Request.QueryString[Enums.QueryStringName.printReport.ToString()].ToString());
            printID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.printId.ToString()] != null ? Request.QueryString[Enums.QueryStringName.printId.ToString()].ToString() : "-1");
            showPreview = CommonMethods.ParseBool(Request.QueryString[Enums.QueryStringName.showPreviewReport.ToString()].ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ShowReport();
        }

        private void ShowReport()
        {
            switch (printReport)
            {
                case "MonthPayout":
                    MonthPayout report = new MonthPayout();
                    report.PrinterName = "Xerox Phaser 6500DN";
                    SetReportPreview(showPreview, report);
                    break;
            }
        }

        private void SetReportPreview(bool preview, XtraReport report, bool createDocument = true)
        {
            if (createDocument)
                report.CreateDocument();

            if (preview)
                WebReportViewer.OpenReport(report);
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    PdfExportOptions opts = new PdfExportOptions();
                    opts.ShowPrintDialogOnOpen = true;
                    report.ExportToPdf(ms, opts);

                    WriteDocumentToResponse(ms.ToArray(), "pdf", true, "Report.pdf");
                }
            }
        }

        private void WriteDocumentToResponse(byte[] documentData, string format, bool isInline, string fileName)
        {
            //string contentType;
            string disposition = (isInline) ? "inline" : "attachment";

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", String.Format("{0}; filename={1}", disposition, fileName));
            Response.BinaryWrite(documentData);
            Response.End();
        }
    }
}