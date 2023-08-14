using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
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
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Employees
{
    public partial class Employees : ServerMasterPage
    {
        Session session = null;
        int employeeFocusedIndex = -1;
        IRoleRepository roleRepo = null;
        IEmployeeRepository employeeRepo = null;
        int employeeIDFocusedRowIndex = -1;
        bool notEmployedAnyMoreEmployees = false;
        bool nameChanged = false;
        bool isDuplicated = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserSuperAdmin() && !PrincipalHelper.IsUserAdmin()) RedirectHome();

            session = XpoHelper.GetNewSession();
            XpoDSEmployees.Session = session;
            XpoDSRoles.Session = session;
            roleRepo = new RoleRepository(session);
            employeeRepo = new EmployeeRepository(session);

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
                employeeFocusedIndex = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());

            if (Request.QueryString["notEmployedAnymore"] != null)
                notEmployedAnyMoreEmployees = CommonMethods.ParseBool(Request.QueryString["notEmployedAnymore"].ToString());

            if (Request.QueryString["nameChanged"] != null)
                nameChanged = CommonMethods.ParseBool(Request.QueryString["nameChanged"].ToString());

            if (Request.QueryString["isDuplicated"] != null)
                isDuplicated = CommonMethods.ParseBool(Request.QueryString["isDuplicated"].ToString());

            ASPxGridViewEmployees.Settings.GridLines = GridLines.Both;

            if (!PrincipalHelper.IsUserSuperAdmin())
                XpoDSEmployees.Criteria = "[RoleID.Koda]<> '" + Enums.UserRole.SuperAdmin.ToString() + "'";

            if (notEmployedAnyMoreEmployees)
            {
                XpoDSEmployees.Criteria = "[NotEmployedAnymore] = 1";
                (ASPxGridViewEmployees.Columns["Izberi"] as GridViewColumn).Visible = true;
            }

            if (nameChanged)
            {
                XpoDSEmployees.Criteria = "[NameChanged] = 1";
                (ASPxGridViewEmployees.Columns["Izberi"] as GridViewColumn).Visible = true;
            }

            if (isDuplicated)
            {
                XpoDSEmployees.Criteria = "[IsDuplicated] = 1";
                (ASPxGridViewEmployees.Columns["Izberi"] as GridViewColumn).Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (employeeIDFocusedRowIndex > 0)
                {
                    ASPxGridViewEmployees.FocusedRowIndex = ASPxGridViewEmployees.FindVisibleIndexByKeyValue(employeeIDFocusedRowIndex);
                    ASPxGridViewEmployees.ScrollToVisibleIndexOnClient = ASPxGridViewEmployees.FindVisibleIndexByKeyValue(employeeIDFocusedRowIndex);
                }
            }
            else
            { }
        }

        protected void ASPxGridViewEmployees_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');
            if (split[0].Equals("DblClick") && !String.IsNullOrEmpty(split[1]))
            {
                ASPxWebControl.RedirectOnCallback(GenerateURI("EmployeesForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
        }

        protected void ASPxGridViewEmployees_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "RoleID.Naziv")
            {
                ASPxComboBox box = e.Editor as ASPxComboBox;
                box.DataSource = roleRepo.GetAllRoles();
                box.ValueField = "VlogaID";
                box.ValueType = typeof(Int32);
                box.TextField = "Naziv";
                box.ReadOnly = false;
                box.DataBind();
            }
        }

        protected void ASPxGridViewEmployees_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            employeeRepo.SaveEmployeeFromBatchUpdate(e.UpdateValues);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            var obj = ASPxGridViewEmployees.GetRowValues(ASPxGridViewEmployees.FocusedRowIndex, "Id");
            RedirectWithCustomURI("EmployeesForm.aspx", (int)Enums.UserAction.Edit, obj);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var obj = ASPxGridViewEmployees.GetRowValues(ASPxGridViewEmployees.FocusedRowIndex, "Id");
            RedirectWithCustomURI("EmployeesForm.aspx", (int)Enums.UserAction.Add, obj);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var obj = ASPxGridViewEmployees.GetRowValues(ASPxGridViewEmployees.FocusedRowIndex, "Id");
            RedirectWithCustomURI("EmployeesForm.aspx", (int)Enums.UserAction.Delete, obj);
        }

        protected void ASPxGridViewEmployees_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "UpravicenDoKVP")
            {
                ASPxComboBox cb = e.Editor as ASPxComboBox;
                cb.Items[0].Text = "DA";
                cb.Items[1].Text = "NE";
            }
            else if (e.Column.FieldName == "Deleted")
            {
                ASPxComboBox cb = e.Editor as ASPxComboBox;
                cb.Items[0].Text = "DA";
                cb.Items[1].Text = "NE";
            }
        }

        protected void btnConfirmChanges_Click(object sender, EventArgs e)
        {
            List<int> selectedEmployees = ASPxGridViewEmployees.GetSelectedFieldValues("Id").OfType<int>().ToList();

            if (notEmployedAnyMoreEmployees)
            {
                employeeRepo.UpdateEmployees(selectedEmployees, new List<UpdateColumnWithValueModel>(){
                    new UpdateColumnWithValueModel { ColumnName = "NotEmployedAnymore", ColumnValue = 0 },
                    new UpdateColumnWithValueModel { ColumnName = "Deleted", ColumnValue = true }
                });
            }
            else if (nameChanged)
            {
                employeeRepo.UpdateEmployees(selectedEmployees, new List<UpdateColumnWithValueModel>(){
                    new UpdateColumnWithValueModel { ColumnName = "NameChanged", ColumnValue = 0 }
                });
            }
            else if (isDuplicated)
            {
                employeeRepo.UpdateEmployees(selectedEmployees, new List<UpdateColumnWithValueModel>(){
                    new UpdateColumnWithValueModel { ColumnName = "IsDuplicated", ColumnValue = 0 }
                });
            }

            ASPxGridViewEmployees.Selection.UnselectAll();
            ASPxGridViewEmployees.DataBind();
            Master.NavigationBarMain.DataBind();
        }

    }
}