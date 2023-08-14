using DevExpress.Data.Filtering;
using DevExpress.Web;
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

namespace KVP_Obrazci.KVPGroups
{
    public partial class KVPGroupReportForm : ServerMasterPage
    {
        Session session;
        IKVPDocumentRepository kvpDocRepo;

        XPCollection<KVPGroupReport> collectionKVPGroupReport = null;

        protected void Page_Init(object sender, EventArgs e)
        {

            if (!Request.IsAuthenticated) RedirectHome();

            session = XpoHelper.GetNewSession();

            kvpDocRepo = new KVPDocumentRepository(session);

            ASPxGridViewKVPGroupReport1.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateEditDateFrom.Date = new DateTime(2019, 1, 1);
                DateEditDateTo.Date = new DateTime(2019, 12, 31);
                SelectionRadioButton.SelectedIndex = 0;
                RemoveSession("KVPGroupReportDataSource");
            }
        }

        protected void KVPGroupReportCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "CreateReport")
            {
                DateTime dtFilterFrom = DateTime.MinValue;
                DateTime dtFilterTo = DateTime.MaxValue;
                KVPGroupReport nrKVPGroupReport = null;


                if (DateEditDateFrom.Text != "")
                {
                    dtFilterFrom = DateTime.Parse(DateEditDateFrom.Text);
                }

                if (DateEditDateTo.Text != "")
                {
                    dtFilterTo = DateTime.Parse(DateEditDateTo.Text);
                }



                collectionKVPGroupReport = new XPCollection<KVPGroupReport>(session);
                session.Delete(new XPCollection(session, typeof(KVPGroupReport)));
                session.ExecuteNonQuery("dbcc checkident (KVPGroupReport, reseed,0);");

                List<KVPGroupReportModel> list = kvpDocRepo.GetKVPDocForDatePeriodLastStatusAndGroupID(dtFilterFrom, dtFilterTo, 0, Enums.KVPStatuses.VNOS, null, IsCompletedKVPSelected());

                foreach (KVPGroupReportModel kgm in list)
                {
                    nrKVPGroupReport = new KVPGroupReport(session);
                    if (kgm != null)
                    {
                        nrKVPGroupReport.SkupinaKoda = kgm.Koda;
                        nrKVPGroupReport.SkupinaNaziv = kgm.Naziv;
                        nrKVPGroupReport.Podani = kgm.Podani;
                        nrKVPGroupReport.Odprti = kgm.Odprti;
                        nrKVPGroupReport.Realizirani = kgm.Realizirani;
                        nrKVPGroupReport.Zavrnjeni = kgm.Zavrnjeni;
                    }

                    collectionKVPGroupReport.Add(nrKVPGroupReport);
                    nrKVPGroupReport.Save();
                }
                AddValueToSession("KVPGroupReportDataSource", collectionKVPGroupReport);
                //ASPxGridViewKVPGroupReport.DataSource = collectionKVPGroupReport;
                ASPxGridViewKVPGroupReport1.DataBind();
            }
            else if (e.Parameter == "HideColumns")
            {
                if (IsCompletedKVPSelected())
                {
                    ASPxGridViewKVPGroupReport1.Columns["Realizirani"].Visible = true;
                    ASPxGridViewKVPGroupReport1.Columns["Zavrnjeni"].Visible = true;
                    
                    ASPxGridViewKVPGroupReport1.Columns["Odprti"].Visible = false;
                    ASPxGridViewKVPGroupReport1.Columns["Podani"].Visible = false;

                }
                else
                {
                    ASPxGridViewKVPGroupReport1.Columns["Realizirani"].Visible = false;
                    ASPxGridViewKVPGroupReport1.Columns["Zavrnjeni"].Visible = false;

                    ASPxGridViewKVPGroupReport1.Columns["Odprti"].Visible = true;
                    ASPxGridViewKVPGroupReport1.Columns["Podani"].Visible = true;
                }
            }
        }



        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ASPxGridViewKVPGroupReport1.DataBind();
            KVPGroupReportxporter.GridViewID = "ASPxGridViewKVPGroupReport1";
            KVPGroupReportxporter.FileName = "KVPSkupineReport_" + DateTime.Now.ToString("dd.MM.yyy hh:mm");
            KVPGroupReportxporter.WriteCsvToResponse();
        }

        protected void ASPxGridViewKVPGroupReport1_DataBinding(object sender, EventArgs e)
        {
            if (collectionKVPGroupReport != null || SessionHasValue("KVPGroupReportDataSource"))
            {
                (sender as ASPxGridView).DataSource = collectionKVPGroupReport != null ? collectionKVPGroupReport : (XPCollection<KVPGroupReport>)GetValueFromSession("KVPGroupReportDataSource");
            }
        }

        private bool IsCompletedKVPSelected()
        { 
            return (SelectionRadioButton.SelectedItem.Value == "CompletedKVP");
        }
    }
}