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
        IEmployeeRepository employeeRepo;
        IKVPDocumentRepository kvpDocRepo = null;

        bool isDataBinding = false;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            departmentID = CommonMethods.ParseInt(GetValueFromSession(Enums.Department.DepartmentID));

            session = XpoHelper.GetNewSession();

            departmentRepo = new DepartmentRepository(session);
            kvpDocRepo = new KVPDocumentRepository(session);
            employeeRepo = new EmployeeRepository(session);

            XpoDSEmployee.Session = session;
            XpoDSDepartments.Session = session;
            
            ASPxGridLookupDepartmentHead.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupDepartmentHeadDeputy.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupDepartment.GridView.Settings.GridLines = GridLines.Both;

            ASPxGridLookupDepartment.GridView.CustomUnboundColumnData += GridView_CustomUnboundColumnData;
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

                isDataBinding = true;
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
                int iChangedHeadId = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupDepartmentHead));



                if (model.DepartmentHeadId != iChangedHeadId)
                {
                    // check if there is any KVP to confirm by last HeadID
                    int iCountKVPToConfirm = kvpDocRepo.GetNotFinishedKVPsForUser(model.DepartmentHeadId);
                    if (iCountKVPToConfirm > 0)
                    {
                        // sent info on display, check if confirm
                        //Vodja Ime in Priimek ima iCountKVPToConfirm odprtih KVP-jev ali ga želiš vseeno zamenjati? DA / NE

                        kvpDocRepo.ChangeLeaderOnKVPDocument(model.DepartmentHeadId, iChangedHeadId);

                    }

                }
                model.DepartmentHeadId = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupDepartmentHead));               
                model.DepartmentHeadDeputyId = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupDepartmentHeadDeputy));

               Users head = employeeRepo.GetEmployeeByID(model.DepartmentHeadId);
                if (head != null) head.RoleID = employeeRepo.GetRoleByID(5, session);
                Users deputyHead = employeeRepo.GetEmployeeByID(model.DepartmentHeadDeputyId);
                if (deputyHead != null) deputyHead.RoleID = employeeRepo.GetRoleByID(5, session);

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

       /* protected void ASPxGridLookupDepartment_DataBinding(object sender, EventArgs e)
        {
            if (isDataBinding)
                (sender as ASPxGridLookup).DataSource = departmentRepo.GetDepartmentsDataSource();
        }*/

        void GridView_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if ((e.IsGetData) && (e.Column.FieldName == "DepartmentHeadName"))
            {
                int idHead = Convert.ToInt32(e.GetListSourceFieldValue("DepartmentHeadId"));
                if (idHead > 0)
                {
                    Users usrHead = employeeRepo.GetEmployeeByID(idHead);
                    if (usrHead != null)
                    {
                        e.Value = usrHead.Firstname + " " + usrHead.Lastname;
                    }
                }
            }

            if ((e.IsGetData) && (e.Column.FieldName == "DepartmentHeadDeputyName"))
            {
                int idHeadDepudy = Convert.ToInt32(e.GetListSourceFieldValue("DepartmentHeadDeputyId"));
                if (idHeadDepudy > 0)
                {
                    Users usrHeadDep = employeeRepo.GetEmployeeByID(idHeadDepudy);
                    e.Value = usrHeadDep.Firstname + " " + usrHeadDep.Lastname;
                    if (usrHeadDep != null)
                    {
                        e.Value = usrHeadDep.Firstname + " " + usrHeadDep.Lastname;
                    }
                }
            }

            if ((e.IsGetData) && (e.Column.FieldName == "DepartmentSupName"))
            {
                int idDepartSup = Convert.ToInt32(e.GetListSourceFieldValue("ParentId"));
                if (idDepartSup > 0)
                {
                    Departments depart = departmentRepo.GetDepartmentByID(idDepartSup);
                    if (depart != null)
                    {
                        e.Value = depart.Name;
                    }

                }
            }
        }

        /*protected void ASPxGridLookupDepartment_DataBound(object sender, EventArgs e)
        {
            isDataBinding = false;
        }*/
    }
}