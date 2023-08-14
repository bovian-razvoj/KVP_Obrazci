using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Pdf;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
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
        KVPDocumentReport kvpReport = null;
        string uploadDirectory = "~/UploadControl/UploadDocuments/";

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
            String employeeName = PrincipalHelper.GetUserPrincipal().firstName + " " + PrincipalHelper.GetUserPrincipal().lastName;

            switch (printReport)
            {
                case "MonthPayout":
                    MonthPayout report = new MonthPayout();
                    report.PrinterName = "Xerox Phaser 6500DN";
                    SetReportPreview(showPreview, report);
                    break;
                case "KVPDocument":
                    kvpReport = new KVPDocumentReport(printID, employeeName);
                    kvpReport.PrinterName = "Xerox Phaser 6500DN";
                    SetReportPreview(showPreview, kvpReport);
                    break;
                case "KVPDocumentRedCard":
                    KVPDocumentRedCardReport redCard = new KVPDocumentRedCardReport(printID, employeeName);
                    redCard.PrinterName = "Xerox Phaser 6500DN";
                    SetReportPreview(showPreview, redCard);
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
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    PdfExportOptions opts = new PdfExportOptions();
                //    opts.ShowPrintDialogOnOpen = true;
                //    report.ExportToPdf(ms, opts);

                //    WriteDocumentToResponse(ms.ToArray(), "pdf", true, "Report.pdf");
                //}

                // save KVP dokument
                PdfExportOptions opts = new PdfExportOptions();
                string path = Server.MapPath(uploadDirectory);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string fn = "KVPDoc" + DateTime.Now.ToString();
                fn = CommonMethods.ReplaceSumniki(fn).Trim().Replace(" ", "_");
                fn = CommonMethods.ReplaceSumniki(fn).Trim().Replace(".", "");
                fn = CommonMethods.ReplaceSumniki(fn).Trim().Replace(":", "");
                fn = path + fn;
                fn = fn + ".PDF";
                report.ExportToPdf(fn);

                PdfDocumentProcessor pdfDocumentProcessor = new PdfDocumentProcessor();
                pdfDocumentProcessor.LoadDocument(fn);

                // load vseh priponk
                if (kvpReport.kvpDoc != null)
                {
                    KVPDocument model = kvpReport.kvpDoc;

                    if (model != null && !String.IsNullOrEmpty(model.Priloge))
                    {
                        List<DocumentEntity> list = new List<DocumentEntity>();
                        DocumentEntity document = null;
                        string[] split = model.Priloge.Split('|');

                        foreach (var item in split)
                        {
                            string[] fileData = item.Split(';');
                            document = new DocumentEntity();
                            document.Url = fileData[0].Split('/')[3];
                            document.Name = fileData[1];
                            string exstension = document.Url.Split('.')[1].ToString();
                            document.Name = document.Url.Split('.')[0].ToString();
                            document.Url = path + document.Url;
                            string sConvertPDF = ExportToPDFbyExtension(exstension, document.Url, document.Name);
                            if (File.Exists(sConvertPDF)) pdfDocumentProcessor.AppendDocument(sConvertPDF);


                        }
                    }
                }

                using (MemoryStream ms = new MemoryStream())
                {

                    opts.ShowPrintDialogOnOpen = true;
                    pdfDocumentProcessor.SaveDocument(ms);

                    WriteDocumentToResponse(ms.ToArray(), "pdf", true, "Report.pdf");
                }
            }
        }

        private string ExportToPDFbyExtension(string sExtension, string sOriginalFilePath, string sFileNameToConvert)
        {
            switch (sExtension.ToUpper())
            {
                case "TXT":
                case "CSV":
                case "DOC":
                case "LOG":
                case "XML":
                case "TEX":
                    return ExportTextFileToPDF(sOriginalFilePath, sFileNameToConvert, sExtension);

                case "BMP":
                case "JPG":
                case "TIF":
                case "PNG":
                case "GIF":
                case "ICO":
                    return ExportPicturesToPDF(sOriginalFilePath, sFileNameToConvert);
                case "XLS":
                case "XLR":
                case "XLSX":
                    return ExportXLSToPDF(sOriginalFilePath, sFileNameToConvert);
                case "PDF":
                    return sOriginalFilePath;
                default:
                    break;
            }
            return "";
        }

        static DevExpress.XtraRichEdit.DocumentFormat ReturnDocumentFormatByExtension(string sExtension)
        {
            switch (sExtension.ToUpper())
            {
                case "TXT":
                    return DevExpress.XtraRichEdit.DocumentFormat.PlainText;
                case "DOC":
                    return DevExpress.XtraRichEdit.DocumentFormat.Rtf;
                case "LOG":
                    return DevExpress.XtraRichEdit.DocumentFormat.PlainText;
                case "XML":
                    return DevExpress.XtraRichEdit.DocumentFormat.OpenXml;
                case "TEX":
                    return DevExpress.XtraRichEdit.DocumentFormat.PlainText;
                default:
                    break;
            }
            return DevExpress.XtraRichEdit.DocumentFormat.PlainText;
        }

        private string ExportTextFileToPDF(string OriginalFilePath, string targetPDFFileName, string sExtension)
        {
            RichEditDocumentServer serverConv = new RichEditDocumentServer();
            #region #ExportToPDF
            serverConv.LoadDocument(OriginalFilePath, ReturnDocumentFormatByExtension(sExtension));
            //Specify export options:
            PdfExportOptions options = new PdfExportOptions();
            options.DocumentOptions.Author = "Mark Jones";
            options.Compressed = false;
            options.ImageQuality = PdfJpegImageQuality.Highest;
            //Export the document to the stream: 
            string path = Server.MapPath(uploadDirectory);
            string sTargetPDF = path + targetPDFFileName + ".pdf";
            using (FileStream pdfFileStream = new FileStream(sTargetPDF, FileMode.Create))
            {
                serverConv.ExportToPdf(pdfFileStream, options);
            }
            return sTargetPDF;

            #endregion #ExportToPDF
        }

        private string ExportXLSToPDF(string OriginalFilePath, string targetPDFFileName)
        {
            DevExpress.Spreadsheet.Workbook workbook = new DevExpress.Spreadsheet.Workbook();

            // Load a workbook from the file.
            workbook.LoadDocument(OriginalFilePath, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
            string path = Server.MapPath(uploadDirectory);
            string sTargetPDF = path + targetPDFFileName + ".pdf";

            using (FileStream pdfFileStream = new FileStream(sTargetPDF, FileMode.Create))
            {
                workbook.ExportToPdf(pdfFileStream);
            }

            return sTargetPDF;
        }

        private string ExportPicturesToPDF(string OriginalFilePath,  string targetPDFFileName)
        {
            PdfDocumentProcessor pdfDocumentProcessor = new PdfDocumentProcessor();
            string sTargetPDF = "";

            using (RichEditDocumentServer server = new RichEditDocumentServer())
            {
                //Insert an image 
                DocumentImage docImage = server.Document.Images.Append(DocumentImageSource.FromFile(OriginalFilePath));

                //Adjust the page width and height to the image's size 
                server.Document.Sections[0].Page.Width = docImage.Size.Width + server.Document.Sections[0].Margins.Right + server.Document.Sections[0].Margins.Left;
                server.Document.Sections[0].Page.Height = docImage.Size.Height + server.Document.Sections[0].Margins.Top + server.Document.Sections[0].Margins.Bottom;

                //Export the result to PDF 
                string path = Server.MapPath(uploadDirectory);
                sTargetPDF = path + targetPDFFileName + ".pdf";

                using (FileStream fs = new FileStream(sTargetPDF, FileMode.OpenOrCreate))
                {
                    server.ExportToPdf(fs);
                }
            }
            return sTargetPDF;
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