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
    public partial class DashboardRedCards : ServerMasterPage
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

                XpoDataSourceKVPDOcumentRedCard.Session = session;
                XpoDSRCsToConfirm.Session = session;
                XpoDSRCsRealizator.Session = session;
                XPODSRealizedRCs.Session = session;
                XpoDSAllRC.Session = session;

                CustommizeByUserRole();

                if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
                    kVPDocIDFocusedRowIndex = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());


                ASPxGridViewRCsToConfirm.Settings.GridLines = GridLines.Both;
                ASPxGridViewKVPDocumentRedCard.Settings.GridLines = GridLines.Both;
                ASPxGridViewRKRealizator.Settings.GridLines = GridLines.Both;
                ASPxGridViewRealizedRC.Settings.GridLines = GridLines.Both;
                ASPxGridViewAllRedCards.Settings.GridLines = GridLines.Both;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeGridViewsColumns();
                ASPxGridViewRCsToConfirm.SettingsBehavior.AllowEllipsisInText = true;

                if (kVPDocIDFocusedRowIndex > 0)
                {
                    //ASPxGridViewKVPDocument.FocusedRowIndex = ASPxGridViewKVPDocument.FindVisibleIndexByKeyValue(kVPDocIDFocusedRowIndex);
                    //ASPxGridViewKVPDocument.ScrollToVisibleIndexOnClient = ASPxGridViewKVPDocument.FindVisibleIndexByKeyValue(kVPDocIDFocusedRowIndex);
                }

                lblUsersName.Text = PrincipalHelper.GetUserPrincipal().firstName + " " + PrincipalHelper.GetUserPrincipal().lastName;
                RCNumber.InnerText = kvpDocRepo.GetRCCountByUserID(PrincipalHelper.GetUserPrincipal().ID).ToString();
                lblDepartmentParagraph.Text = PrincipalHelper.GetUserPrincipal().DepartmentName;
                lblDepartment.Text = PrincipalHelper.GetUserPrincipal().DepartmentName;
                lblChampion.Text = PrincipalHelper.GetUserPrincipal().Champion;
                lblSupervisor.Text = PrincipalHelper.GetUserPrincipal().Supervisor;
                lblKvpGroup.Text = PrincipalHelper.GetUserPrincipal().GroupName;


                RCCompleted.InnerText = kvpDocRepo.GetCompletedRCsForUser(PrincipalHelper.GetUserPrincipal().ID).ToString();

                if (Session["MojiKVPFilter"] != null)
                {
                    //ASPxGridViewKVPDocument.FilterExpression = Session["MojiKVPFilter"].ToString();
                    Session["MojiKVPFilter"] = "";
                }


            }
            /*else
                Session["MojiKVPFilter"] = ASPxGridViewKVPDocument.FilterExpression;*/
        }

        protected void btnAddNewKvp_Click(object sender, EventArgs e)
        {
            RedirectWithCustomURI("KVPDocuments/KVPDocumentForm.aspx", (int)Enums.UserAction.Add, 0);
        }

        protected void ASPxGridViewKVPDocumentRedCard_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            List<int> currentPageKVPsID = new List<int>();
            string[] split = e.Parameters.Split(';');

            List<Enums.KVPDocumentSession> list = Enum.GetValues(typeof(Enums.KVPDocumentSession)).Cast<Enums.KVPDocumentSession>().ToList();
            ClearAllSessions(list);

            if (split.Length == 3)
                AddValueToSession(Enums.CommonSession.activeTab, split[2]);

            if (split[0].Equals("DblClickRedCard") && !String.IsNullOrEmpty(split[1]))
                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocuments/KVPDocumentRedCardForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            else if (split[0].Equals("DblClickRCsToConfirm") && !String.IsNullOrEmpty(split[1]))
            {
                currentPageKVPsID = ASPxGridViewRCsToConfirm.GetCurrentPageRowValues("idKVPDocument").OfType<int>().ToList();
                GetKVPDocumentProvider().SetKVPDocumentsIDsList(currentPageKVPsID.OrderBy(x => x).ToList());

                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocuments/KVPDocumentRedCardForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
            else if (split[0].Equals("DblClickRCsToRealize") && !String.IsNullOrEmpty(split[1]))
            {
                //currentPageKVPsID = ASPxGridViewKVPToRealize.GetCurrentPageRowValues("idKVPDocument").OfType<int>().ToList();
                //GetKVPDocumentProvider().SetKVPDocumentsIDsList(currentPageKVPsID.OrderBy(x => x).ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocuments/KVPDocumentRedCardForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
            else if (split[0].Equals("DblClickRealizedRCs") && !String.IsNullOrEmpty(split[1]))
            {
                currentPageKVPsID = ASPxGridViewRealizedRC.GetCurrentPageRowValues("idKVPDocument").OfType<int>().ToList();
                GetKVPDocumentProvider().SetKVPDocumentsIDsList(currentPageKVPsID.OrderBy(x => x).ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocuments/KVPDocumentRedCardForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
            else if (split[0].Equals("DblClickAllRCs") && !String.IsNullOrEmpty(split[1]))
            {
                currentPageKVPsID = ASPxGridViewRealizedRC.GetCurrentPageRowValues("idKVPDocument").OfType<int>().ToList();
                GetKVPDocumentProvider().SetKVPDocumentsIDsList(currentPageKVPsID.OrderBy(x => x).ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocuments/KVPDocumentRedCardForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
        }

        protected void ASPxGridViewKVPDocumentRedCard_DataBound(object sender, EventArgs e)
        {
            RedCardsBadge.InnerText = ASPxGridViewKVPDocumentRedCard.VisibleRowCount.ToString();
        }

        protected void btnAddNewredCard_Click(object sender, EventArgs e)
        {
            RedirectWithCustomURI("KVPDocuments/KVPDocumentRedCardForm.aspx", (int)Enums.UserAction.Add, 0);
        }

        protected void ASPxGridViewRCsToConfirm_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            RCsToConfirmBadge.InnerText = ASPxGridViewRCsToConfirm.VisibleRowCount.ToString();
        }

        protected void ASPxGridViewRCsToConfirm_DataBound(object sender, EventArgs e)
        {
            RCsToConfirmBadge.InnerText = ASPxGridViewRCsToConfirm.VisibleRowCount.ToString();
        }


        private void CustommizeByUserRole()
        {
            if (PrincipalHelper.IsUserSuperAdmin() || PrincipalHelper.IsUserAdmin())
            {
                XpoDSRCsRealizator.Criteria += " AND [LastStatusId.Koda] = '" + Enums.RedCardStatus.RK_V_REALIZACIJI.ToString() + "'";
                XpoDSRCsToConfirm.Criteria += " AND [LastStatusId.Koda] = '" + Enums.RedCardStatus.ODPRTO.ToString() + "'";
                XPODSRealizedRCs.Criteria += " AND [LastStatusId.Koda] = '" + Enums.RedCardStatus.RK_REALIZIRAN.ToString() + "'";
                //XpoDataSourceKVPDOcumentRedCard.Criteria += " AND [Predlagatelj.Id] = " + PrincipalHelper.GetUserPrincipal().ID;
                return;
            }

            List<string> hideTabs = new List<string> { "rcToConfirm", "rcRealized" };
            XpoDSRCsRealizator.Criteria += " AND [Realizator.Id] = " + PrincipalHelper.GetUserPrincipal().ID + " AND [LastStatusId.Koda] = '" + Enums.RedCardStatus.RK_V_REALIZACIJI.ToString() + "'";

            if (PrincipalHelper.IsUserTpmAdmin() || PrincipalHelper.IsUserAlsoTpmAdmin())
            {
                XpoDSRCsToConfirm.Criteria += " AND [LastStatusId.Koda] = '" + Enums.RedCardStatus.ODPRTO.ToString() + "'";
                XPODSRealizedRCs.Criteria += " AND [LastStatusId.Koda] = '" + Enums.RedCardStatus.RK_REALIZIRAN.ToString() + "'";

                TabsVisible(new List<string>() { "AllRC" });
            }
            else
            {
                XpoDataSourceKVPDOcumentRedCard.Criteria += " AND [Predlagatelj.Id] = " + PrincipalHelper.GetUserPrincipal().ID;
                TabsVisible(hideTabs);
            }
        }

        protected void btnRealizeSelected_Click(object sender, EventArgs e)
        {
            List<object> selectedRows = new List<object>();
            /*selectedRows = ASPxGridViewKVPToRealize.GetSelectedFieldValues("idKVPDocument");
            kvpDocRepo.AutomaticUpdateStatusKVPDocument(selectedRows);
            ASPxGridViewKVPToRealize.DataBind();
            ASPxGridViewKVPDocument.DataBind();*/
        }

        protected void ASPxGridViewKVPDocument_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            if (e.GetValue("LastStatusId.Koda").ToString() == Enums.RedCardStatus.CEZ_TERMIN.ToString())
            {
                e.Row.BackColor = Color.Tomato;
            }
            else if (e.GetValue("LastStatusId.Koda").ToString() == Enums.RedCardStatus.RK_REALIZIRAN.ToString())
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9A33");
            }
            else if (e.GetValue("LastStatusId.Koda").ToString() == Enums.RedCardStatus.RK_V_REALIZACIJI.ToString())
            {
                e.Row.BackColor = Color.LightYellow;
            }
            else if (e.GetValue("LastStatusId.Koda").ToString() == Enums.RedCardStatus.IZVRSEN.ToString())
            {
                e.Row.BackColor = Color.LightGreen;
            }

            if ((e.GetValue("RokOdziva") != null) && (e.GetValue("LastStatusId.Koda").ToString() != Enums.RedCardStatus.IZVRSEN.ToString()))
            {
                DateTime dtROkOdziva = Convert.ToDateTime(e.GetValue("RokOdziva"));
               
                //if ((dtROkOdziva < DateTime.Now) && e.GetValue("LastStatusId.Koda").ToString() != Enums.RedCardStatus.RK_V_REALIZACIJI.ToString() && e.GetValue("LastStatusId.Koda").ToString() != Enums.RedCardStatus.RK_REALIZIRAN.ToString())
                if (dtROkOdziva < DateTime.Now)
                    {
                    e.Row.BackColor = Color.Tomato;
                }
            }

            //if (e.)
            //
            //{
            //    e.Row.BackColor = Color.LightGreen;
            //}

        }

        private Int32 GetNumbersOfDaysForRCType(Int32 iTip)
        {
            if (iTip == 1) return 14;
            if (iTip == 2) return 28;
            return 14;
        }

        private void InitializeGridViewsColumns()
        {
            //if (PrincipalHelper.IsUserChampion() || PrincipalHelper.IsUserAdmin() || PrincipalHelper.IsUserSuperAdmin() || PrincipalHelper.IsUserLeader())
            //{
            //    ASPxGridViewRCsToConfirm.Columns["LokacijaID.Opis"].Visible = true;
            //    ASPxGridViewRCsToConfirm.Columns["LinijaID.Opis"].Visible = true;
            //    ASPxGridViewRCsToConfirm.Columns["StrojID.Opis"].Visible = true;
            //    ASPxGridViewRCsToConfirm.Columns["StrojStevilka"].Visible = true;

            //    ASPxGridViewKVPDocumentRedCard.Columns["LokacijaID.Opis"].Visible = true;
            //    ASPxGridViewKVPDocumentRedCard.Columns["LinijaID.Opis"].Visible = true;
            //    ASPxGridViewKVPDocumentRedCard.Columns["StrojID.Opis"].Visible = true;
            //    ASPxGridViewKVPDocumentRedCard.Columns["StrojStevilka"].Visible = true;

            //    ASPxGridViewRKRealizator.Columns["LokacijaID.Opis"].Visible = true;
            //    ASPxGridViewRKRealizator.Columns["LinijaID.Opis"].Visible = true;
            //    ASPxGridViewRKRealizator.Columns["StrojID.Opis"].Visible = true;
            //    ASPxGridViewRKRealizator.Columns["StrojStevilka"].Visible = true;
            //}
        }

        protected void ASPxGridViewRealizedRC_DataBound(object sender, EventArgs e)
        {
            RealizedRCBadge.InnerText = ASPxGridViewRealizedRC.VisibleRowCount.ToString();
        }

        #region Export to Excel

        private void SetColumnExportVisibility(ASPxGridView grid, bool isPresoja)
        {
            //if (!PrincipalHelper.IsUserSuperAdmin() && !PrincipalHelper.IsUserAdmin())
            //{
            //    grid.Columns["ZaporednaStevilka"].Visible = false;
            //    grid.Columns["Priloga"].Visible = false;

            //    grid.Columns["Realizator.Lastname"].Visible = false;
            //    grid.Columns["Realizator.Firstname"].Visible = false;

            //    grid.Columns["LastStatusId.Koda"].Visible = false;
            //    grid.Columns["LastStatusId.Naziv"].Visible = false;
            //}
        }

        protected void btnExportToExcelRCsToConfirm_Click(object sender, EventArgs e)
        {
            ASPxGridViewRCsToConfirmExporter.FileName = "RdeciKartoniZaPotrditev_" + CommonMethods.GetTimeStamp();
            SetColumnExportVisibility(ASPxGridViewRCsToConfirm, false);
            ASPxGridViewRCsToConfirmExporter.WriteXlsxToResponse();
        }

        protected void btnExportToExcelKVPDocumentRedCard_Click(object sender, EventArgs e)
        {
            ASPxGridViewKVPDocumentRedCardExporter.FileName = "MojiRdečiKartoni_" + CommonMethods.GetTimeStamp();
            SetColumnExportVisibility(ASPxGridViewKVPDocumentRedCard, false);
            ASPxGridViewKVPDocumentRedCardExporter.WriteXlsxToResponse();
        }

        protected void btnExportToExcelRKRealizator_Click(object sender, EventArgs e)
        {
            ASPxGridViewRKRealizatorExporter.FileName = "RdeciKartoniZaRealizacijo_" + CommonMethods.GetTimeStamp();
            ASPxGridViewRKRealizatorExporter.WriteCsvToResponse();
        }

        protected void btnExportToExcelRealizedRC_Click(object sender, EventArgs e)
        {
            ASPxGridViewRealizedRCExporter.FileName = "RealiziraniRdeciKartoni_" + CommonMethods.GetTimeStamp();
            SetColumnExportVisibility(ASPxGridViewRealizedRC, false);
            ASPxGridViewRealizedRCExporter.WriteXlsxToResponse();
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
            if ((e.DataColumn.FieldName == "OpisNapakeRK") || (e.DataColumn.FieldName == "AktivnostRK"))
                if (e.CellValue != null)
                    e.Cell.ToolTip = e.CellValue.ToString();
        }

        protected void ASPxGridViewKVPDocument_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if (e.CallbackName == "APPLYFILTER")
            {
                Session["MojiKVPFilter"] = "";
            }
        }

        protected void ASPxGridViewRKRealizator_DataBound(object sender, EventArgs e)
        {
            RKRealizatorBadge.InnerText = ASPxGridViewRKRealizator.VisibleRowCount.ToString();
        }

        protected void btnExportToExcelAllRedCards_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporterAllRedCards.FileName = "VsiRdeciKartoni_" + CommonMethods.GetTimeStamp();
            SetColumnExportVisibility(ASPxGridViewAllRedCards, false);
            ASPxGridViewExporterAllRedCards.WriteXlsxToResponse();
        }

        protected void ASPxGridViewAllRedCards_DataBound(object sender, EventArgs e)
        {
            AllRCBadge.InnerText = ASPxGridViewAllRedCards.VisibleRowCount.ToString();
        }

        protected void ASPxGridViewAllRedCards_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            AllRCBadge.InnerText = ASPxGridViewAllRedCards.VisibleRowCount.ToString();
        }

        protected void ASPxGridViewKVPDocumentRedCard_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID != "Print") return;

            object valueID = ASPxGridViewKVPDocumentRedCard.GetRowValues(e.VisibleIndex, "idKVPDocument");

            ASPxGridViewKVPDocumentRedCard.JSProperties["cpPrintID"] = ConcatenateURLForPrint(valueID, "KVPDocumentRedCard", true);
        }
    }
}