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
    public partial class Department : ServerMasterPage
    {
        Session session;

        IEmployeeRepository employeeRepo;
        IDepartmentRepository departmentRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserSuperAdmin() && !PrincipalHelper.IsUserAdmin()) RedirectHome();

            session = XpoHelper.GetNewSession();

            XpoDSDepartments.Session = session;
            XpoDSEmployee.Session = session;

            ASPxGridViewDepartment.Settings.GridLines = GridLines.Both;

            employeeRepo = new EmployeeRepository(session);
            departmentRepo = new DepartmentRepository(session);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ASPxPopupControlDepartment_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.Department.DepartmentID);
            RemoveSession(Enums.Department.DepartmentModel);
        }

        protected void CallbackPanelDepartment_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "OpenPopup")
            {
                int id = CommonMethods.ParseInt(ASPxGridViewDepartment.GetRowValues(ASPxGridViewDepartment.FocusedRowIndex, "Id"));
                AddValueToSession(Enums.Department.DepartmentID, id);
                ASPxPopupControlDepartment.ShowOnPageLoad = true;
            }
        }

        protected void btnDeactiveChanges_Click(object sender, EventArgs e)
        {
            List<int> selectedDepartments = ASPxGridViewDepartment.GetSelectedFieldValues("Id").OfType<int>().ToList();

            departmentRepo.UpdateDepartment(selectedDepartments, false);

            ASPxGridViewDepartment.Selection.UnselectAll();
            ASPxGridViewDepartment.DataBind();
            Master.NavigationBarMain.DataBind();
        }

        protected void btnActiveChanges_Click(object sender, EventArgs e)
        {
            List<int> selectedDepartments = ASPxGridViewDepartment.GetSelectedFieldValues("Id").OfType<int>().ToList();

            departmentRepo.UpdateDepartment(selectedDepartments, true);

            ASPxGridViewDepartment.Selection.UnselectAll();
            ASPxGridViewDepartment.DataBind();
            Master.NavigationBarMain.DataBind();
        }

        protected void ASPxGridViewDepartment_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridViewColumnDataEventArgs e)
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
    }
}