using DevExpress.Data;
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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.KVPGroups
{
    public partial class KVPGroupsForm : ServerMasterPage
    {
        Session session = null;
        KVPSkupina modelKvpGroups = null;

        int action = -1;
        int kvpGroupID = -1;

        IKVPGroupsRepository kvpGroupRepo = null;
        IEmployeeRepository employeeRepo = null;
        IKVPDocumentRepository documentRepo = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserSuperAdmin() && !PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserChampion()) RedirectHome();

            Master.DisableNavBar = true;

            action = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
            kvpGroupID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());

            session = XpoHelper.GetNewSession();
            XpoDSEmployee.Session = session;
            XpoDSGroupEmployee.Session = session;
            XpoDSGroupEmployeeChampion.Session = session;
            kvpGroupRepo = new KVPGroupsRepository(session);
            employeeRepo = new EmployeeRepository(session);
            documentRepo = new KVPDocumentRepository(session);

            ASPxGridViewEmployees.Settings.GridLines = GridLines.Both;
            ASPxGridViewGroupEmployeeChampion.Settings.GridLines = GridLines.Both;

            CustomizeByUserRole();

            ASPxSummaryItem totalSummary = new ASPxSummaryItem();
            totalSummary.FieldName = "StKVP";
            totalSummary.ShowInColumn = "StKVP";
            totalSummary.SummaryType = SummaryItemType.Average;
            ASPxGridViewEmployees.TotalSummary.Add(totalSummary);
        }

        private void CustomizeByUserRole()
        {
            if (PrincipalHelper.IsUserChampion())
            {
                btnConfirm.ClientVisible = false;
                btnAddEmployee.ClientVisible = false;
                btnRemoveEmployee.ClientVisible = false;

                btnAddChampion.ClientVisible = false;
                btnRemoveChampion.ClientVisible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                if (action != (int)Enums.UserAction.Add)
                {
                    if (kvpGroupID > 0)
                    {
                        if (GetKVPGroupsProvider().GetKVPGroupModel() != null)
                            modelKvpGroups = GetKVPGroupsProvider().GetKVPGroupModel();
                        else
                            modelKvpGroups = kvpGroupRepo.GetKVPGroupByID(kvpGroupID);

                        if (modelKvpGroups != null)
                        {
                            GetKVPGroupsProvider().SetKVPGroupModel(modelKvpGroups);
                            FillForm();
                        }
                    }
                }
                else //User action => Add
                {
                    navTabs.Attributes["class"] = navTabs.Attributes["class"].ToString() + " disabled";

                    //SetFormDefaultValues();
                }
            }
            else
            {
                if (modelKvpGroups == null && SessionHasValue(Enums.KVPGroups.KVPGroupModel))
                    modelKvpGroups = GetKVPGroupsProvider().GetKVPGroupModel();
            }
        }

        private void FillForm()
        {
            txtKoda.Text = modelKvpGroups.Koda;
            txtNaziv.Text = modelKvpGroups.Naziv;
            /*ASPxGridLookupAuditor1.Value = modelKvpGroups.Potrjevalec1 != null ? modelKvpGroups.Potrjevalec1.Id : 0;
            ASPxGridLookupAuditor2.Value = modelKvpGroups.Potrjevalec2 != null ? modelKvpGroups.Potrjevalec2.Id : 0;
            ASPxGridLookupAuditor3.Value = modelKvpGroups.Potrjevalec3 != null ? modelKvpGroups.Potrjevalec3.Id : 0;*/

            HtmlGenericControl control = (HtmlGenericControl)championItem.FindControl("championBadge");
            control.InnerText = kvpGroupRepo.GetKVPGroupUsersChampionsByGroupID(kvpGroupID).Count.ToString();

        }

        private void Initialize()
        {

        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                modelKvpGroups = new KVPSkupina(session);
                modelKvpGroups.idKVPSkupina = 0;

            }
            else if (modelKvpGroups == null && !add)
            {
                modelKvpGroups = GetKVPGroupsProvider().GetKVPGroupModel();
                modelKvpGroups.idKVPSkupina = modelKvpGroups.idKVPSkupina;

            }

            /*int auditor1ID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupAuditor1));
            if (auditor1ID > 0)
            {
                if (modelKvpGroups.Potrjevalec1 != null)
                    modelKvpGroups.Potrjevalec1 = employeeRepo.GetEmployeeByID(auditor1ID, modelKvpGroups.Potrjevalec1.Session);
                else
                    modelKvpGroups.Potrjevalec1 = employeeRepo.GetEmployeeByID(auditor1ID);
            }

            int auditor2ID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupAuditor2));
            if (auditor2ID > 0)
            {
                if (modelKvpGroups.Potrjevalec2 != null)
                    modelKvpGroups.Potrjevalec2 = employeeRepo.GetEmployeeByID(auditor2ID, modelKvpGroups.Potrjevalec2.Session);
                else
                    modelKvpGroups.Potrjevalec2 = employeeRepo.GetEmployeeByID(auditor2ID);
            }

            int auditor3ID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupAuditor3));
            if (auditor3ID > 0)
            {
                if (modelKvpGroups.Potrjevalec3 != null)
                    modelKvpGroups.Potrjevalec3 = employeeRepo.GetEmployeeByID(auditor3ID, modelKvpGroups.Potrjevalec3.Session);
                else
                    modelKvpGroups.Potrjevalec3 = employeeRepo.GetEmployeeByID(auditor3ID);
            }*/

            modelKvpGroups.Koda = txtKoda.Text;
            modelKvpGroups.Naziv = txtNaziv.Text;

            kvpGroupID = modelKvpGroups.idKVPSkupina = kvpGroupRepo.SaveKVPGroup(modelKvpGroups);

            GetKVPGroupsProvider().SetKVPGroupModel(modelKvpGroups);

            return true;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            bool isValid = false;
            bool isDeleteing = false;

            switch (action)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true);
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject();
                    break;
                case (int)Enums.UserAction.Delete:
                    kvpGroupRepo.DeleteKVPGroup(kvpGroupID);
                    isValid = true;
                    isDeleteing = true;
                    break;
            }

            if (isValid)
            {
                ClearSessionsAndRedirect(isDeleteing);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearSessionsAndRedirect();
        }

        private void ClearSessionsAndRedirect(bool isIDDeleted = false)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = kvpGroupID.ToString() }
            };

            if (isIDDeleted)
                redirectString = "KVPGroups.aspx";
            else
                redirectString = GenerateURI("KVPGroups.aspx", queryStrings);

            List<Enums.KVPGroups> list = Enum.GetValues(typeof(Enums.KVPGroups)).Cast<Enums.KVPGroups>().ToList();
            ClearAllSessions(list, redirectString);
        }

        protected void ASPxGridViewEmployees_DataBound(object sender, EventArgs e)
        {

        }

        protected void CallbackPanelEmployees_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string tabShow = "";
            string[] array = e.Parameter.Split(';');
            if (array[0] == "RefreshGrid")
            {
                ASPxGridViewEmployees.DataBind();
                ASPxGridViewGroupEmployeeChampion.DataBind();
                tabShow = array[1];
            }
            else if (e.Parameter == "AddEmployee" || e.Parameter == "AddEmployeeChampion")
            {
                if (action == (int)Enums.UserAction.Add)
                    AddOrEditEntityObject(true);

                AddValueToSession(Enums.KVPGroups.KVPGroupID, kvpGroupID);
                tabShow = "KVPGroupBasicData";
                if (e.Parameter == "AddEmployeeChampion")
                {
                    AddValueToSession(Enums.KVPGroups.KVPGroupUsersCriteriaPopUp, "[Id]");
                    ASPxPopupControlEmployee.HeaderText = "ZAPOSLENI - CHAMPION";
                    tabShow = "Champion";
                }

                ASPxPopupControlEmployee.ShowOnPageLoad = true;
            }
            else if (e.Parameter == "RemoveEmployee" || e.Parameter == "RemoveEmployeeChampion")
            {
                if (action == (int)Enums.UserAction.Add) return;
                List<object> selectedRows = new List<object>();

                if (e.Parameter == "RemoveEmployee")
                {
                    selectedRows = ASPxGridViewEmployees.GetSelectedFieldValues("idKVPSkupina_Zaposleni");
                    tabShow = "KVPGroupBasicData";
                }
                else
                {
                    selectedRows = ASPxGridViewGroupEmployeeChampion.GetSelectedFieldValues("idKVPSkupina_Zaposleni");
                    tabShow = "Champion";
                }


                List<int> converted = selectedRows.Select(i => Convert.ToInt32(i)).ToList();
                kvpGroupRepo.DeleteEmployeesFromKVPGroupUsers(converted);
            }
            CallbackPanelEmployees.JSProperties["cpTabShow"] = tabShow;
        }

        protected void ASPxPopupControlEmployee_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {

            //Remove sessions from popup
            RemoveSession(Enums.KVPGroups.KVPGroupID);
            RemoveSession(Enums.KVPGroups.KVPGroupUsersCriteriaPopUp);
        }

        protected void ASPxGridViewGroupEmployeeChampion_DataBound(object sender, EventArgs e)
        {
            HtmlGenericControl control = (HtmlGenericControl)championItem.FindControl("championBadge");
            control.InnerText = ASPxGridViewGroupEmployeeChampion.VisibleRowCount.ToString();
        }

        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            if (action == (int)Enums.UserAction.Add)
                AddOrEditEntityObject(true);

            ASPxPopupControlEmployee.ShowOnPageLoad = true;
            RedirectWithCustomURI("KVPGroupsForm.aspx", (int)Enums.UserAction.Edit, kvpGroupID);
        }

        protected void ASPxGridViewEmployees_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridViewColumnDataEventArgs e)
        {
            if ((e.IsGetData) && (e.Column.FieldName == "StKVP"))
            {
                int idUser = Convert.ToInt32(e.GetListSourceFieldValue("IdUser.Id"));
                if (idUser > 0)
                {
                    e.Value = documentRepo.GetSubmittedKVPCountByUserIdAndYear(idUser, DateTime.Now.Year);
                }
            }
        }
    }
}