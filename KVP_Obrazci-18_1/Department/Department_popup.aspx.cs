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

namespace KVP_Obrazci.Department
{
    public partial class Department_popup : ServerMasterPage
    {
        Session session;
        int departmentID;
        Departments model;

        IDepartmentRepository departmentRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            departmentID = CommonMethods.ParseInt(GetValueFromSession(Enums.Department.DepartmentID));

            session = XpoHelper.GetNewSession();

            departmentRepo = new DepartmentRepository(session);

            XpoDSEmployee.Session = session;
           // XpoDSDepartments.Session = session;
            
            ASPxGridLookupDepartmentHead.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupDepartmentHeadDeputy.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupDepartment.GridView.Settings.GridLines = GridLines.Both;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                model = departmentRepo.GetDepartmentByID(departmentID);

                if (model != null)
                {
                    AddValueToSession(Enums.Department.DepartmentModel, model);
                    FillForm();
                }
            }
            else
            {
                if (SessionHasValue(Enums.Department.DepartmentModel))
                    model = (Departments)GetValueFromSession(Enums.Department.DepartmentModel);
            }
        }

        public void FillForm()
        {
            txtGroupName.Text = model.Name;
            ASPxGridLookupDepartmentHead.Value = model.DepartmentHeadId;
            ASPxGridLookupDepartmentHeadDeputy.Value = model.DepartmentHeadDeputyId;
            ASPxGridLookupDepartment.Value = model.ParentId;
        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            if (model != null)
            {
                model.DepartmentHeadId = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupDepartmentHead));
                model.DepartmentHeadDeputyId = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupDepartmentHeadDeputy));
                model.ParentId = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupDepartment));

                departmentRepo.SaveDepartment(model);
            }

            RemoveSessionsAndClosePopUP(true);
        }

        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";


            RemoveSession(Enums.Department.DepartmentID);
            RemoveSession(Enums.Department.DepartmentModel);


            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "Department"), true);
        }

        protected void ASPxGridLookupDepartment_DataBinding(object sender, EventArgs e)
        {
           (sender as ASPxGridLookup).DataSource = departmentRepo.GetDepartmentsDataSource();
        }
    }
}