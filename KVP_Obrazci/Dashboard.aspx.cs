using DevExpress.Web;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci
{
    public partial class Dashboard : ServerMasterPage
    {
        Session session = null;
        IKVPDocumentRepository kvpDocRepo = null;
        IKVPGroupsRepository kvpGroupsRepo = null;
        IPayoutsRepository payoutRepo = null;
        int kVPDocIDFocusedRowIndex = 0;


        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();
            else
            {
                session = XpoHelper.GetNewSession();

                kvpDocRepo = new KVPDocumentRepository(session);
                kvpGroupsRepo = new KVPGroupsRepository(session);
                payoutRepo = new PayoutsRepository(session);



                XpoDataSourceKVPDOcument.Session = session;

                XpoDSKVPsToConfirm.Session = session;
                XpoDSKVPsToCheck.Session = session;
                XpoDataSourceKVPDOcumentToRealize.Session = session;
                XpoDataSourceAuditorKVPs.Session = session;
                XPODSRealizedKVPs.Session = null;
                XPODSChampionAllKVPs.Session = session;

                CustommizeByUserRole();

                if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
                    kVPDocIDFocusedRowIndex = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());

                ASPxGridViewKVPDocument.Settings.GridLines = GridLines.Both;
                ASPxGridViewKVPToRealize.Settings.GridLines = GridLines.Both;
                ASPxGridViewKVPsToConfirm.Settings.GridLines = GridLines.Both;
                ASPxGridViewKVPsToCheck.Settings.GridLines = GridLines.Both;
                ASPxGridViewAuditorKVPs.Settings.GridLines = GridLines.Both;
                ASPxGridViewRealizedKVP.Settings.GridLines = GridLines.Both;
                ASPxGridViewChampionAllKVPs.Settings.GridLines = GridLines.Both;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeGridViewsColumns();

                if (kVPDocIDFocusedRowIndex > 0)
                {
                    ASPxGridViewKVPDocument.FocusedRowIndex = ASPxGridViewKVPDocument.FindVisibleIndexByKeyValue(kVPDocIDFocusedRowIndex);
                    ASPxGridViewKVPDocument.ScrollToVisibleIndexOnClient = ASPxGridViewKVPDocument.FindVisibleIndexByKeyValue(kVPDocIDFocusedRowIndex);
                }

                lblUsersName.Text = PrincipalHelper.GetUserPrincipal().firstName + " " + PrincipalHelper.GetUserPrincipal().lastName;
                //KVPNumber.InnerText = kvpDocRepo.GetKVPCountByUserID(PrincipalHelper.GetUserPrincipal().ID).ToString();
                lblDepartmentParagraph.Text = PrincipalHelper.GetUserPrincipal().DepartmentName;
                lblDepartment.Text = PrincipalHelper.GetUserPrincipal().DepartmentName;
                lblChampion.Text = PrincipalHelper.GetUserPrincipal().Champion;
                lblSupervisor.Text = PrincipalHelper.GetUserPrincipal().Supervisor;
                lblKvpGroup.Text = PrincipalHelper.GetUserPrincipal().GroupName;

                CollectedPoints.InnerText = payoutRepo.GetActualPointsForUser(PrincipalHelper.GetUserPrincipal().ID).ToString("N0");
                KVPCompleted.InnerText = kvpDocRepo.GetCompletedKVPsForUser(PrincipalHelper.GetUserPrincipal().ID).ToString();

                KVPInfoUserModel model = kvpDocRepo.GetKVPDocForMonthByGroupID(PrincipalHelper.GetUserPrincipal().GroupId);
                // lblPlanRealizationCurrentMonth.Text =  + " (" + model.OdstotekKVP.ToString() + " %)";
                kvpRealCount.Style.Add("font-weight", "700");
                kvpRealCount.InnerHtml = model.PlanKVP.ToString() + "/" + model.PodaniKVP.ToString();
                kvpRealPrecentage.Style.Add("font-weight", "700");
                kvpRealPrecentage.InnerHtml = " (" + model.OdstotekKVP.ToString() + " %)";

                if (model.OdstotekKVP < 51)
                {
                    kvpRealPrecentage.Style.Add("color", "LightSalmon");
                }
                else if (model.OdstotekKVP < 99)
                {
                    kvpRealPrecentage.Style.Add("color", "LemonChiffon");
                }
                else if (model.OdstotekKVP >= 100)
                {
                    kvpRealPrecentage.Style.Add("color", "LightGreen");
                }


                ClearGridViewFilters();
            }
            else
                SetGridViewFilters();
        }

        private void ClearGridViewFilters()
        {
            if (Session["MojiKVPFilter"] != null)
            {
                ASPxGridViewKVPDocument.FilterExpression = Session["MojiKVPFilter"].ToString();
                Session["MojiKVPFilter"] = "";
            }

            if (Session["SemRealizatorKVPFilter"] != null)
            {
                ASPxGridViewKVPToRealize.FilterExpression = Session["SemRealizatorKVPFilter"].ToString();
                Session["SemRealizatorKVPFilter"] = "";
            }

            if (Session["ZaPotrditevKVPFilter"] != null)
            {
                ASPxGridViewKVPsToConfirm.FilterExpression = Session["ZaPotrditevKVPFilter"].ToString();
                Session["ZaPotrditevKVPFilter"] = "";
            }

            if (Session["SemPresojevalecKVPFilter"] != null)
            {
                ASPxGridViewAuditorKVPs.FilterExpression = Session["SemPresojevalecKVPFilter"].ToString();
                Session["SemPresojevalecKVPFilter"] = "";
            }

            if (Session["RealiziraniKVPFilter"] != null)
            {
                ASPxGridViewRealizedKVP.FilterExpression = Session["RealiziraniKVPFilter"].ToString();
                Session["RealiziraniKVPFilter"] = "";
            }

            if (Session["VsiKVPFilter"] != null)
            {
                ASPxGridViewChampionAllKVPs.FilterExpression = Session["VsiKVPFilter"].ToString();
                Session["VsiKVPFilter"] = "";
            }

        }

        private void SetGridViewFilters()
        {
            Session["MojiKVPFilter"] = ASPxGridViewKVPDocument.FilterExpression;
            Session["SemRealizatorKVPFilter"] = ASPxGridViewKVPToRealize.FilterExpression;
            Session["ZaPotrditevKVPFilter"] = ASPxGridViewKVPsToConfirm.FilterExpression;
            Session["SemPresojevalecKVPFilter"] = ASPxGridViewAuditorKVPs.FilterExpression;
            Session["VsiKVPFilter"] = ASPxGridViewChampionAllKVPs.FilterExpression;
            Session["RealiziraniKVPFilter"] = ASPxGridViewRealizedKVP.FilterExpression;
        }

        protected void btnAddNewKvp_Click(object sender, EventArgs e)
        {
            RedirectWithCustomURI("KVPDocuments/KVPDocumentForm.aspx", (int)Enums.UserAction.Add, 0);
        }

        protected void ASPxGridViewKVPDocument_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            List<int> currentPageKVPsID = new List<int>();
            string[] split = e.Parameters.Split(';');

            List<Enums.KVPDocumentSession> list = Enum.GetValues(typeof(Enums.KVPDocumentSession)).Cast<Enums.KVPDocumentSession>().ToList();
            ClearAllSessions(list);

            if (split.Length == 3)
                AddValueToSession(Enums.CommonSession.activeTab, split[2]);

            if (split[0].Equals("DblClick") && !String.IsNullOrEmpty(split[1]))
            {
                currentPageKVPsID = ASPxGridViewKVPDocument.GetCurrentPageRowValues("idKVPDocument").OfType<int>().ToList();
                GetKVPDocumentProvider().SetKVPDocumentsIDsList(currentPageKVPsID.OrderBy(x => x).ToList());

                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocuments/KVPDocumentForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
            else if (split[0].Equals("DblClickKVPsToConfirm") && !String.IsNullOrEmpty(split[1]))
            {
                currentPageKVPsID = ASPxGridViewKVPsToConfirm.GetCurrentPageRowValues("idKVPDocument").OfType<int>().ToList();
                GetKVPDocumentProvider().SetKVPDocumentsIDsList(currentPageKVPsID.OrderBy(x => x).ToList());

                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocuments/KVPDocumentForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
            else if (split[0].Equals("DblClickKVPsToCheck") && !String.IsNullOrEmpty(split[1]))
            {
                currentPageKVPsID = ASPxGridViewKVPsToCheck.GetCurrentPageRowValues("idKVPDocument").OfType<int>().ToList();
                GetKVPDocumentProvider().SetKVPDocumentsIDsList(currentPageKVPsID.OrderBy(x => x).ToList());

                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocuments/KVPDocumentForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
            else if (split[0].Equals("DblClickAuditorsKVPs") && !String.IsNullOrEmpty(split[1]))
            {
                currentPageKVPsID = ASPxGridViewAuditorKVPs.GetCurrentPageRowValues("idKVPDocument").OfType<int>().ToList();
                GetKVPDocumentProvider().SetKVPDocumentsIDsList(currentPageKVPsID.OrderBy(x => x).ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocuments/KVPDocumentForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
            else if (split[0].Equals("DblClickKVPsToRealize") && !String.IsNullOrEmpty(split[1]))
            {
                currentPageKVPsID = ASPxGridViewKVPToRealize.GetCurrentPageRowValues("idKVPDocument").OfType<int>().ToList();
                GetKVPDocumentProvider().SetKVPDocumentsIDsList(currentPageKVPsID.OrderBy(x => x).ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocuments/KVPDocumentForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
            else if (split[0].Equals("DblClickRealizedKVPs") && !String.IsNullOrEmpty(split[1]))
            {
                currentPageKVPsID = ASPxGridViewRealizedKVP.GetCurrentPageRowValues("idKVPDocument").OfType<int>().ToList();
                GetKVPDocumentProvider().SetKVPDocumentsIDsList(currentPageKVPsID.OrderBy(x => x).ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocuments/KVPDocumentForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
            else if (split[0].Equals("DblClickChampionAllKVPs") && !String.IsNullOrEmpty(split[1]))
            {
                currentPageKVPsID = ASPxGridViewChampionAllKVPs.GetCurrentPageRowValues("idKVPDocument").OfType<int>().ToList();
                GetKVPDocumentProvider().SetKVPDocumentsIDsList(currentPageKVPsID.OrderBy(x => x).ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocuments/KVPDocumentForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
        }

        protected void ASPxGridViewKVPDocument_DataBound(object sender, EventArgs e)
        {
            string rowCount = ASPxGridViewKVPDocument.VisibleRowCount.ToString();
            MyKVPsBadge.InnerText = rowCount;
            KVPNumber.InnerText = rowCount;
        }



        protected void btnAddNewredCard_Click(object sender, EventArgs e)
        {
            RedirectWithCustomURI("KVPDocuments/KVPDocumentRedCardForm.aspx", (int)Enums.UserAction.Add, 0);
        }

        protected void ASPxGridViewKVPsToConfirm_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //KVPsToConfirmBadge.InnerText = ASPxGridViewKVPsToConfirm.VisibleRowCount.ToString();
        }

        protected void ASPxGridViewKVPsToCheck_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //KVPsToConfirmBadge.InnerText = ASPxGridViewKVPsToConfirm.VisibleRowCount.ToString();
        }

        protected void ASPxGridViewKVPsToConfirm_DataBound(object sender, EventArgs e)
        {
            KVPsToConfirmBadge.InnerText = ASPxGridViewKVPsToConfirm.VisibleRowCount.ToString();
        }

        protected void ASPxGridViewKVPsToCheck_DataBound(object sender, EventArgs e)
        {
            KVPsToCheckBadge.InnerText = ASPxGridViewKVPsToCheck.VisibleRowCount.ToString();
        }

        private void CustommizeByUserRole()
        {
            List<string> hideTabs = new List<string> { "rkRealizator", "kvpRealized", "kvpToCheck" };
            bool isDefaultTabsHidden = false;

            if (PrincipalHelper.GetUserPrincipal() == null)
                return;

            if (!PrincipalHelper.IsUserSuperAdmin() && !PrincipalHelper.IsUserAdmin())
            {
                XpoDataSourceKVPDOcument.Criteria = "[idTipRdeciKarton] is null AND [Predlagatelj.Id] = " + PrincipalHelper.GetUserPrincipal().ID;
            }

            XpoDSKVPsToConfirm.Criteria = "[LastStatusId.Koda] = '" + Enums.KVPStatuses.ODOBRITEV_VODJA.ToString() + "'";
            XpoDSKVPsToCheck.Criteria = "[LastStatusId.Koda] = '" + Enums.KVPStatuses.V_PREVERJANJE.ToString() + "'";
            XpoDataSourceKVPDOcumentToRealize.Criteria = "[Realizator.Id] = " + PrincipalHelper.GetUserPrincipal().ID + " AND [LastStatusId.Koda]='" + Enums.KVPStatuses.V_REALIZACIJI.ToString() + "'";

            XpoDataSourceAuditorKVPs.Criteria = "[<KVPPresoje>][^.idKVPDocument=idKVPDocument.idKVPDocument AND Presojevalec.Id=" + PrincipalHelper.GetUserPrincipal().ID + " AND JeZadnjiPresojevalec = 1] AND [LastStatusId.Koda]='" + Enums.KVPStatuses.POSLANO_V_PRESOJO + "'";

            if (PrincipalHelper.IsUserEmployee())
            {
                TabsVisible(new List<string> { "kvpToConfirm", "kvpToCheck", "rkRealizator", "kvpRealized" });
                isDefaultTabsHidden = true;
            }
            else if (PrincipalHelper.IsUserChampion())
            {
                KVPSkupina_Users obj = kvpGroupsRepo.GetKVPGroupUserByUserID(PrincipalHelper.GetUserPrincipal().ID);
                if (obj != null && obj.Champion)
                {
                    List<Users> list = kvpGroupsRepo.GetUsersFromKVPGroupByID(obj.idKVPSkupina.idKVPSkupina);
                    string criteria = "";
                    string criteriaProposer = "([Predlagatelj.Id] IN (";
                    string criteriaRealizator = "(([Realizator.Id] IN (";
                    string criteriaKVPToCheck = criteriaProposer;
                    foreach (var item in list)
                    {
                        criteria += item.Id + ",";
                    }
                    criteria = criteria.Remove(criteria.Length - 1);

                    criteriaRealizator += criteria + ")) OR " + criteriaProposer + criteria + "))) AND ([LastStatusId.Koda] = '" + Enums.KVPStatuses.REALIZIRANO.ToString() + "')";
                    criteriaProposer += criteria + ")) AND ([LastStatusId.Koda] <> '" + Enums.KVPStatuses.VNOS.ToString() + "')";
                    criteriaKVPToCheck += criteria + ")) AND ([LastStatusId.Koda] = '" + Enums.KVPStatuses.V_PREVERJANJE.ToString() + "')";

                    XpoDataSourceKVPDOcument.Criteria = "[idTipRdeciKarton] is null AND " + criteriaProposer + " OR ([Predlagatelj.Id] =" + PrincipalHelper.GetUserPrincipal().ID + " AND [LastStatusId.Koda] = '" + Enums.KVPStatuses.VNOS.ToString() + "')";
                    XpoDSKVPsToCheck.Criteria = criteriaKVPToCheck;

                    XPODSRealizedKVPs.Session = session;
                    XPODSRealizedKVPs.Criteria = "[idTipRdeciKarton] is null AND " + criteriaRealizator;
                }
                else //Če ni Champion
                {

                }
                //XpoDSKVPsToConfirm.Criteria = "[vodja_teama] = -1";
                hideTabs.AddRange(new List<string> { "kvpToConfirm" });
                hideTabs.Remove("kvpRealized");
                hideTabs.Remove("kvpToCheck");

                //hideTabs.Remove("championAllKVPs");
                TabsVisible(hideTabs);
                isDefaultTabsHidden = true;
            }
            else if (PrincipalHelper.IsUserTpmAdmin())
            {
                TabsVisible(new List<string> { "kvpToConfirm", "kvpToCheck", "rkRealizator", "kvpRealized" });
                isDefaultTabsHidden = true;
            }
            else if (PrincipalHelper.IsUserLeader())
            {
                string deputyheadID = "[Predlagatelj.DepartmentId.DepartmentHeadDeputyId] =" + PrincipalHelper.GetUserPrincipal().ID.ToString();
                string allKVPDocCriteria = "[idTipRdeciKarton] is null AND ((([vodja_teama] = " + PrincipalHelper.GetUserPrincipal().ID.ToString() + " OR " + deputyheadID + ") AND [LastStatusId.Koda] <> '" + Enums.KVPStatuses.VNOS.ToString() + "') OR [Predlagatelj.Id] = " + PrincipalHelper.GetUserPrincipal().ID.ToString() + ")";

                XpoDSKVPsToConfirm.Criteria = "([vodja_teama] = " + PrincipalHelper.GetUserPrincipal().ID.ToString() + " OR " + deputyheadID + ") AND [LastStatusId.Koda] = '" + Enums.KVPStatuses.ODOBRITEV_VODJA.ToString() + "'";
                XpoDataSourceKVPDOcument.Criteria = allKVPDocCriteria;


            }

            //we hide tabs for red cards
            if (!isDefaultTabsHidden)
                TabsVisible(hideTabs);
        }

        protected void ASPxGridViewKVPToRealize_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void ASPxGridViewKVPToRealize_DataBound(object sender, EventArgs e)
        {
            KVPRealizatorBadge.InnerText = ASPxGridViewKVPToRealize.VisibleRowCount.ToString();
        }

        protected void ASPxGridViewAuditorKVPs_DataBound(object sender, EventArgs e)
        {
            KVPAuditorBadge.InnerText = ASPxGridViewAuditorKVPs.VisibleRowCount.ToString();
        }

        protected void btnRealizeSelected_Click(object sender, EventArgs e)
        {
            List<object> selectedRows = new List<object>();
            selectedRows = ASPxGridViewKVPToRealize.GetSelectedFieldValues("idKVPDocument");
            kvpDocRepo.AutomaticUpdateStatusKVPDocument(selectedRows);
            ASPxGridViewKVPToRealize.DataBind();
            ASPxGridViewKVPDocument.DataBind();
        }

        protected void ASPxGridViewKVPDocument_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            if (e.GetValue("LastStatusId.Koda").ToString() == Enums.KVPStatuses.ZAVRNJEN.ToString())
            {
                e.Row.BackColor = Color.Tomato;
            }
            else if (e.GetValue("LastStatusId.Koda").ToString() == Enums.KVPStatuses.REALIZIRANO.ToString())
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9A33");
            }
            else if (e.GetValue("LastStatusId.Koda").ToString() == Enums.KVPStatuses.POSLANO_V_PRESOJO.ToString())
            {
                e.Row.BackColor = Color.LightYellow;
            }
            else if (e.GetValue("LastStatusId.Koda").ToString() == Enums.KVPStatuses.ZAKLJUCENO.ToString())
            {
                e.Row.BackColor = Color.LightGreen;
            }

        }

        private void InitializeGridViewsColumns()
        {
            if (PrincipalHelper.IsUserChampion() || PrincipalHelper.IsUserAdmin() || PrincipalHelper.IsUserSuperAdmin() || PrincipalHelper.IsUserLeader())
            {
                ASPxGridViewKVPDocument.Columns["LokacijaID.Opis"].Visible = true;
                ASPxGridViewKVPDocument.Columns["LinijaID.Opis"].Visible = true;
                ASPxGridViewKVPDocument.Columns["StrojID.Opis"].Visible = true;
                ASPxGridViewKVPDocument.Columns["StrojStevilka"].Visible = true;

                ASPxGridViewKVPToRealize.Columns["LokacijaID.Opis"].Visible = true;
                ASPxGridViewKVPToRealize.Columns["LinijaID.Opis"].Visible = true;
                ASPxGridViewKVPToRealize.Columns["StrojID.Opis"].Visible = true;
                ASPxGridViewKVPToRealize.Columns["StrojStevilka"].Visible = true;

                ASPxGridViewKVPsToConfirm.Columns["LokacijaID.Opis"].Visible = true;
                ASPxGridViewKVPsToConfirm.Columns["LinijaID.Opis"].Visible = true;
                ASPxGridViewKVPsToConfirm.Columns["StrojID.Opis"].Visible = true;
                ASPxGridViewKVPsToConfirm.Columns["StrojStevilka"].Visible = true;

                ASPxGridViewKVPsToCheck.Columns["LokacijaID.Opis"].Visible = true;
                ASPxGridViewKVPsToCheck.Columns["LinijaID.Opis"].Visible = true;
                ASPxGridViewKVPsToCheck.Columns["StrojID.Opis"].Visible = true;
                ASPxGridViewKVPsToCheck.Columns["StrojStevilka"].Visible = true;


                ASPxGridViewAuditorKVPs.Columns["LokacijaID.Opis"].Visible = true;
                ASPxGridViewAuditorKVPs.Columns["LinijaID.Opis"].Visible = true;
                ASPxGridViewAuditorKVPs.Columns["StrojID.Opis"].Visible = true;
                ASPxGridViewAuditorKVPs.Columns["StrojStevilka"].Visible = true;
            }
        }

        protected void ASPxGridViewRealizedKVP_DataBound(object sender, EventArgs e)
        {
            RealizedKVPBadge.InnerText = ASPxGridViewRealizedKVP.VisibleRowCount.ToString();
        }

        protected void ASPxGridViewChampionAllKVPs_DataBound(object sender, EventArgs e)
        {
            ChampionAllKVPBadge.InnerText = ASPxGridViewChampionAllKVPs.VisibleRowCount.ToString();
        }

        #region Export to Excel

        private void SetVisiblePriloge(ASPxGridView gridView)
        {
            gridView.DataColumns["Priloga"].Visible = false;
            gridView.DataColumns["Priloge"].Visible = false;
        }

        protected void btnExportToExcelMyKVPDocuments_Click(object sender, EventArgs e)
        {
            ASPxGridViewKVPDocumentExporter.FileName = "MojiKVPPredlogi_" + CommonMethods.GetTimeStamp();

            //ASPxGridViewKVPDocumentExporter.GridView.DataColumns["KVPSkupina"].VisibleIndex = 3;
            SetVisiblePriloge(ASPxGridViewKVPDocument);

            SetColumnExportVisibility(ASPxGridViewKVPDocument, true);
            ASPxGridViewKVPDocumentExporter.WriteXlsxToResponse();
        }

        private void SetColumnExportVisibility(ASPxGridView grid, bool isPresoja)
        {
            //if (!PrincipalHelper.IsUserSuperAdmin() && !PrincipalHelper.IsUserAdmin())
            //{

            //    grid.Columns["ZaporednaStevilka"].Visible = false;
            //    grid.Columns["Priloga"].Visible = false;
            //    grid.Columns["PrihranekStroskiDA_NE"].Visible = false;
            //    grid.Columns["Datum zakl. ideje"].Visible = false;
            //    grid.Columns["idTip.Naziv"].Visible = false;
            //    if (isPresoja) grid.Columns["LastPresojaID.Lastname"].Visible = false;
            //    grid.Columns["Realizator.Lastname"].Visible = false;
            //    grid.Columns["Realizator.Firstname"].Visible = false;

            //    grid.Columns["LastStatusId.Koda"].Visible = false;
            //    grid.Columns["LastStatusId.Naziv"].Visible = false;
            //}
        }

        //void exportOptions_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs ea)
        //{
        //    if (ea.ColumnFieldName != "UnitsOnOrder") return;
        //    if (Convert.ToInt32(ea.Value) == 0)
        //        ea.Formatting.BackColor = Color.Salmon;
        //    else
        //        ea.Formatting.BackColor = Color.LightGray;
        //    ea.Handled = true;
        //}


        protected void btnExportToExcelKVPToRealize_Click(object sender, EventArgs e)
        {
            ASPxGridViewKVPToRealizeExporter.FileName = "KVPPredlogiZaRealizacijo_" + CommonMethods.GetTimeStamp();
            SetColumnExportVisibility(ASPxGridViewKVPToRealize, false);
            SetVisiblePriloge(ASPxGridViewKVPToRealize);
            ASPxGridViewKVPToRealizeExporter.WriteXlsxToResponse();
        }

        protected void btnExportToExcelKVPsToConfirm_Click(object sender, EventArgs e)
        {
            ASPxGridViewKVPsToConfirmExporter.FileName = "KVPPredlogiZaPotrditev_" + CommonMethods.GetTimeStamp();
            SetColumnExportVisibility(ASPxGridViewKVPsToConfirm, false);
            SetVisiblePriloge(ASPxGridViewKVPsToConfirm);
            ASPxGridViewKVPsToConfirmExporter.WriteXlsxToResponse();
        }


        protected void btnExportToExcelKVPsToCheck_Click(object sender, EventArgs e)
        {
            ASPxGridViewKVPsToCheckExporter.FileName = "KVPPredlogiZaPreverjanje_" + CommonMethods.GetTimeStamp();
            SetColumnExportVisibility(ASPxGridViewKVPsToCheck, false);
            SetVisiblePriloge(ASPxGridViewKVPsToCheck);
            ASPxGridViewKVPsToCheckExporter.WriteXlsxToResponse();
        }




        protected void btnExportToExcelAuditorKVPs_Click(object sender, EventArgs e)
        {
            ASPxGridViewAuditorKVPsExporter.FileName = "KVPPredlogiZaPresojo_" + CommonMethods.GetTimeStamp();
            SetColumnExportVisibility(ASPxGridViewAuditorKVPs, false);
            SetVisiblePriloge(ASPxGridViewAuditorKVPs);
            ASPxGridViewAuditorKVPsExporter.WriteXlsxToResponse();
        }

        protected void btnExportToExcelRealizedKVP_Click(object sender, EventArgs e)
        {
            ASPxGridViewRealizedKVPExporter.FileName = "RealiziraniKVPPredlogi_" + CommonMethods.GetTimeStamp();
            SetColumnExportVisibility(ASPxGridViewRealizedKVP, false);
            SetVisiblePriloge(ASPxGridViewRealizedKVP);
            ASPxGridViewRealizedKVPExporter.WriteXlsxToResponse();
        }
        protected void btnExporttoExcelAllKVP_Click(object sender, EventArgs e)
        {
            ASPxGridViewRealizedKVPExporter.FileName = "VsiKVPPredlogi_" + CommonMethods.GetTimeStamp();
            SetColumnExportVisibility(ASPxGridViewChampionAllKVPs, false);
            SetVisiblePriloge(ASPxGridViewChampionAllKVPs);
            ASPxGridViewAllKVPExport.WriteXlsxToResponse();
        }


        #endregion

        protected Boolean GetChecked(object columnValue)
        {
            if (columnValue == null) return false;

            if (!String.IsNullOrEmpty(columnValue.ToString()))
                return true;
            else
                return false;
        }

        protected void ASPxGridViewKVPDocument_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if ((e.DataColumn.FieldName == "OpisProblem") || (e.DataColumn.FieldName == "PredlogIzboljsave"))
                if (e.CellValue != null)
                    e.Cell.ToolTip = e.CellValue.ToString();
        }

        protected void ASPxGridViews_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if (e.CallbackName == "APPLYFILTER")
            {
                Session["MojiKVPFilter"] = "";
                Session["SemRealizatorKVPFilter"] = "";
                Session["ZaPotrditevKVPFilter"] = "";
                Session["SemPresojevalecKVPFilter"] = "";
                Session["VsiKVPFilter"] = "";
                Session["RealiziraniKVPFilter"] = "";
            }
        }

        protected void ASPxGridViewKVPDocument_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridViewColumnDataEventArgs e)
        {
            if ((e.IsGetData) && (e.Column.FieldName == "PrilogaBit"))
            {
                if (e.GetListSourceFieldValue("Priloge") != null)
                {
                    string priloge = e.GetListSourceFieldValue("Priloge").ToString();

                    if (priloge.Length > 0)
                    {
                        e.Value = "TRUE";
                    }
                }
                else
                {
                    e.Value = "FALSE";
                }
            }
        }

        protected void ASPxGridViewKVPDocument_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID != "Print") return;

            object valueID = ASPxGridViewKVPDocument.GetRowValues(e.VisibleIndex, "idKVPDocument");

            ASPxGridViewKVPDocument.JSProperties["cpPrintID"] = ConcatenateURLForPrint(valueID, "KVPDocument", true);
            
        }
    }
}