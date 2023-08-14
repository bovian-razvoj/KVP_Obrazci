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

namespace KVP_Obrazci.Employees
{
    public partial class EmployeesForm : ServerMasterPage
    {
        int action = -1;
        int employeeID = -1;
        Users model = null;
        Session session = null;
        IEmployeeRepository employeeRepo = null;
        IKVPGroupsRepository kvpGroupRepo;
        IMessageProcessorRepository messageRepo = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserSuperAdmin() && !PrincipalHelper.IsUserAdmin()) RedirectHome();

            Master.DisableNavBar = true;

            action = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
            employeeID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());

            session = XpoHelper.GetNewSession();
            XpoDSRoles.Session = session;

            employeeRepo = new EmployeeRepository(session);
            kvpGroupRepo = new KVPGroupsRepository(session);
            messageRepo = new MessageProcessorRepository(session);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();

                if (action != (int)Enums.UserAction.Add)
                {
                    if (employeeID > 0)
                    {
                        if (GetEmployeeProvider().GetEmployeeModel() != null)
                            model = GetEmployeeProvider().GetEmployeeModel();
                        else
                            model = employeeRepo.GetEmployeeByID(employeeID);

                        if (model != null)
                        {
                            GetEmployeeProvider().SetEmployeeModel(model);
                            FillForm();
                        }
                    }
                }
                else //User action => Add
                {
                    //navTabs.Attributes["class"] = navTabs.Attributes["class"].ToString() + " disabled";

                    SetFormDefaultValues();
                }
            }
            else
            {
                if (GetEmployeeProvider().GetEmployeeModel() != null)
                    model = GetEmployeeProvider().GetEmployeeModel();
            }
        }

        private void FillForm()
        {
            txtPersonalID.Text = model.PersonalId;
            txtCard.Text = model.Card;
            txtFirstname.Text = model.Firstname;
            txtLastname.Text = model.Lastname;
            txtDepartment.Text = model.DepartmentId != null ? model.DepartmentId.Name : "";
            txtEmail.Text = model.Email;
            ASPxGridLookupRoles.Value = model.RoleID != null ? model.RoleID.VlogaID : 0;
            ASPxMemoCustomField1.Text = model.CustomField1;
            txtUsername.Text = model.Username;
            txtPassword.Text = model.Password;
            txtKVPGroupName.Text = "Ni dodeljen KVP skupini";
            txtKVPGroupChampion.Text = "Skupina nima champion-a";

            KVPSkupina_Users obj = kvpGroupRepo.GetKVPGroupUserByUserID(employeeID);
            if (obj != null)
            {
                txtKVPGroupName.Text = obj.idKVPSkupina.Naziv;

                List<Users> champions = kvpGroupRepo.GetKVPGroupChampionsByKVPGroupID(obj.idKVPSkupina.idKVPSkupina);

                txtKVPGroupChampion.Text = ConcatenateChampions(champions);
            }

            if (!String.IsNullOrEmpty(model.Email))
                btnSendCredentials.ClientEnabled = true;

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SetPass", String.Format("clientTxtPassword.SetText('{0}');", model.Password), true);

            Departments department = employeeRepo.GetParentDepartment(model);
            if (department != null)
            {
                Users headEmp = employeeRepo.GetEmployeeByID(department.DepartmentHeadId);
                if (headEmp != null)
                    txtDepartmentHead.Text = headEmp.Firstname + " " + headEmp.Lastname;

                Users deputyEmp = employeeRepo.GetEmployeeByID(department.DepartmentHeadDeputyId);
                if (deputyEmp != null)
                    txtDepartmentDeputy.Text = deputyEmp.Firstname + " " + deputyEmp.Lastname;

                Departments parentDepartment = employeeRepo.GetDepartmentByID(model.DepartmentId.ParentId);
                if (parentDepartment != null)
                    txtParentID.Text = parentDepartment.Name;
            }
        }

        #region Initialization
        private void Initialize()
        {

        }

        private void SetFormDefaultValues()
        {

        }

        #endregion

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new Users(session);
                model.Id = 0;
            }
            else if (model == null && !add)
            {
                model = GetEmployeeProvider().GetEmployeeModel();
                model.Id = model.Id;
            }


            int roleId = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRoles));
            if (model.RoleID != null)
                model.RoleID = employeeRepo.GetRoleByID(roleId, model.RoleID.Session);
            else
                model.RoleID = employeeRepo.GetRoleByID(roleId);

            model.Firstname = txtFirstname.Text;
            model.Lastname = txtLastname.Text;
            model.Email = txtEmail.Text;

            model.Username = txtUsername.Text;
            model.Password = txtPassword.Text;

            model.CustomField1 = ASPxMemoCustomField1.Text;


            model.Id = employeeRepo.SaveEmployee(model);
            GetEmployeeProvider().SetEmployeeModel(model);

            return true;
        }

        private void ClearSessionsAndRedirect(bool isIDDeleted = false)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = employeeID.ToString() }
            };

            if (isIDDeleted)
                redirectString = "Employees.aspx";
            else
                redirectString = GenerateURI("Employees.aspx", queryStrings);

            List<Enums.Employee> list = Enum.GetValues(typeof(Enums.Employee)).Cast<Enums.Employee>().ToList();
            ClearAllSessions(list, redirectString);
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
                    employeeRepo.DeleteEmployee(employeeID);
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

        protected void btnSendCredentials_Click(object sender, EventArgs e)
        {
            bool add = (action == (int)Enums.UserAction.Add);
            AddOrEditEntityObject(add);
            messageRepo.ProcessNewKVPCredentialsToSend(model.Id, model.Session);
            ClearSessionsAndRedirect();
        }

        private string ConcatenateChampions(List<Users> champions)
        {
            string returnS = "";
            if (champions != null)
            {
                foreach (var item in champions)
                    returnS += item.Firstname + " " + item.Lastname + " ,";
            }

            if (String.IsNullOrEmpty(returnS))
                returnS = "KVP skupina nima champion-a";
            else
                returnS = returnS.EndsWith(" ,") ? returnS.Remove(returnS.LastIndexOf(" ,")) : returnS;

            return returnS;
        }
    }
}