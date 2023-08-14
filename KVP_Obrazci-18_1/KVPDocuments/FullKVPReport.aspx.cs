using DevExpress.Data.Filtering;
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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.KVPDocuments
{
    public partial class FullKVPReport : ServerMasterPage
    {
        IKVPGroupsRepository kvpGroupsRepo = null;
        IEmployeeRepository employeeRepo = null;
        IKVPAuditorRepository auditorsRepo = null;
        IKVPStatusRepository kvpStatusRepo = null;
        IKVPDocumentRepository kvpDocRepo = null;

        Session session;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            session = XpoHelper.GetNewSession();

            kvpGroupsRepo = new KVPGroupsRepository(session);
            employeeRepo = new EmployeeRepository(session);
            auditorsRepo = new KVPAuditorRepository(session);
            kvpDocRepo = new KVPDocumentRepository(session);
            kvpStatusRepo = new KVPStatusRepository(session);

            XpoDSFullKVPReport.Session = session;

            ASPxGridViewFullKVPReport.Settings.GridLines = GridLines.Both;
        }
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateEditDateFrom.Date = new DateTime(2019, 1, 1);
                DateEditDateTo.Date = new DateTime(2019, 12, 31);
                RemoveSession("FullKVPReportDataSource");
            }

        }

        protected void FullKVPReportCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "CreateReport")
            {
                DateTime dtFilterFrom = DateTime.MinValue;
                DateTime dtFilterTo = DateTime.MaxValue;
                KVPFullReport nrKVPFullReport = null;
                int kVPAuditorID = 0;

                if (DateEditDateFrom.Text != "")
                {
                    dtFilterFrom = DateTime.Parse(DateEditDateFrom.Text);
                }

                if (DateEditDateTo.Text != "")
                {
                    dtFilterTo = DateTime.Parse(DateEditDateTo.Text);
                }

                CriteriaOperator operator1 = CriteriaOperator.Parse("DatumVnosa >= ? and DatumVnosa <= ?", dtFilterFrom.AddHours(23), dtFilterTo.AddHours(23));

                XPCollection<KVPFullReport> collectionKVPFullReport = new XPCollection<KVPFullReport>(session);
                SortingCollection sortCollection = new SortingCollection(session);
                sortCollection.Add(new SortProperty("DatumVnosa", DevExpress.Xpo.DB.SortingDirection.Descending));
                collectionKVPFullReport.Sorting = sortCollection;


                session.Delete(new XPCollection(session, typeof(KVPFullReport)));
                session.ExecuteNonQuery("dbcc checkident (KVPFullReport, reseed,0);");

                XPCollection<KVPDocument> kvpDocs = new XPCollection<KVPDocument>(session, operator1);
                foreach (KVPDocument dok in kvpDocs)
                {
                    nrKVPFullReport = new KVPFullReport(session);
                    nrKVPFullReport.Leto = dok.DatumVnosa.Year;
                    nrKVPFullReport.Mesec = dok.DatumVnosa.Month;
                    nrKVPFullReport.TrenutniStatus = dok.LastStatusId.Naziv;
                    nrKVPFullReport.StevilkaKVP = dok.StevilkaKVP;
                    if (dok.Predlagatelj == null) continue;

                    KVPSkupina_Users obj = kvpGroupsRepo.GetKVPGroupUserByUserID(dok.Predlagatelj.Id);
                    if (obj != null)
                    {
                        nrKVPFullReport.KVPSkupinaKoda = obj.idKVPSkupina.Koda;
                        nrKVPFullReport.KVPSkupinaNaziv = obj.idKVPSkupina.Naziv;
                    }

                    nrKVPFullReport.DatumVnosa = dok.DatumVnosa;
                    nrKVPFullReport.Uporabnik = CommonMethods.GetNameByUser(dok.Predlagatelj);
                    nrKVPFullReport.KVPProblem = dok.OpisProblem;
                    nrKVPFullReport.KVPPredlogIzboljsave = dok.PredlogIzboljsave;

                    int departmentHeadID = employeeRepo.GetDepartmentByID(dok.Predlagatelj.DepartmentId.Id).DepartmentHeadId;

                    if (departmentHeadID > 0)
                    {
                        nrKVPFullReport.VodjaZaOdobritev = CommonMethods.GetNameByUser(employeeRepo.GetEmployeeByID(departmentHeadID));
                    }

                    if (dok.LastStatusId.Koda == Enums.KVPStatuses.POSLANO_V_PRESOJO.ToString())
                    {
                        nrKVPFullReport.Presoja = CommonMethods.GetNameByUser(dok.LastPresojaID);
                    }

                    if ((dok.LastStatusId.Koda == Enums.KVPStatuses.V_REALIZACIJI.ToString() || dok.LastStatusId.Koda == Enums.KVPStatuses.REALIZIRANO.ToString()) && dok.Realizator != null)
                    {
                        nrKVPFullReport.Realizator = CommonMethods.GetNameByUser(dok.Realizator);
                    }

                    if ((dok.LastStatusId.Koda == Enums.KVPStatuses.ZAKLJUCENO.ToString()) || (dok.LastStatusId.Koda == Enums.KVPStatuses.ZAVRNJEN.ToString()))
                    {
                        nrKVPFullReport.DatumZakljuceneIdeje = dok.DatumZakljuceneIdeje;
                    }

                    nrKVPFullReport.CasDoZakljuceneIdeje = CommonMethods.ParseInt(kvpDocRepo.GetNumberOfElapsedDayFromSubmitingKVP(dok));
                    nrKVPFullReport.TipIdeje = dok.idTip.Naziv;
                    if (dok.LastStatusId.Koda == Enums.KVPStatuses.ZAKLJUCENO.ToString())
                    {
                        nrKVPFullReport.TockePredlagatelj = CommonMethods.ParseInt(dok.idTip.TockePredlagatelj);
                        nrKVPFullReport.TockeRealizator = CommonMethods.ParseInt(dok.idTip.TockeRealizator);
                    }

                    nrKVPFullReport.ExternalIDaposlenega = CommonMethods.ParseInt(dok.Predlagatelj.ExternalId);


                    collectionKVPFullReport.Add(nrKVPFullReport);
                    nrKVPFullReport.Save();
                }

                AddValueToSession("FullKVPReportDataSource", collectionKVPFullReport);
                ASPxGridViewFullKVPReport.DataBind();
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ASPxGridViewFullKVPReport.DataBind();
            FullKVPReportExporter.FileName = "PorociloKVP_" + DateTime.Now.ToString("dd.MM.yyyy_hh:mm");
            FullKVPReportExporter.WriteCsvToResponse();
        }

        protected void ASPxGridViewFullKVPReport_DataBinding(object sender, EventArgs e)
        {
            if (SessionHasValue("FullKVPReportDataSource"))
                (sender as ASPxGridView).DataSource = (XPCollection<KVPFullReport>)GetValueFromSession("FullKVPReportDataSource");
        }
    }
}