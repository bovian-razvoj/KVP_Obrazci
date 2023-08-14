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
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Credentials
{
    public partial class ChangePassword : ServerMasterPage
    {
        string user;
        int userID;
        Session session;
        IEmployeeRepository employeeRepo;
        Users model;
        protected void Page_Init(object sender, EventArgs e)
        {
            user = Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString();

            session = XpoHelper.GetNewSession();
            employeeRepo = new EmployeeRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(GetHiddenValue()))
                {
                    SetHiddenValue(user);
                }
                else
                {
                    if (HasQueryStringChanged())
                        return;
                }

                userID = CommonMethods.ParseInt(CommonMethods.Base64Decode(user));
                if (userID > 0)
                {
                    model = employeeRepo.GetEmployeeByID(userID);

                    if (model != null)
                    {
                        lblFullName.Text = model.Firstname + " " + model.Lastname;
                        //lblUsername.Text = model.Username;
                        txtUsername.Text = model.Username;   
                    }
                }
            }
        }

        protected void ChangePasswordCallback_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "ChangePassword")
            {
                if (HasQueryStringChanged())
                    return;

                if (txtPassword.Text.CompareTo(txtReEnterPassword.Text) == 0)
                {
                    userID = CommonMethods.ParseInt(CommonMethods.Base64Decode(user));
                    if (userID > 0)
                    {
                        model = employeeRepo.GetEmployeeByID(userID);

                        if (model != null)
                        {
                            model.Password = txtPassword.Text;
                            employeeRepo.SaveEmployee(model);
                            ChangePasswordCallback.JSProperties["cpResult"] = true;

                            if(Request.IsAuthenticated)
                                FormsAuthentication.SignOut();
                            RemoveAllSesions();
                        }
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Exception", String.Format("$('#expModal').modal('show')"), true);
                    return;
                }

            }
        }

        private string GetHiddenValue()
        {
            object value = null;
            try
            {
                value = hiddenUserID["UserID"];
            }
            catch
            { }

            if (value == null)
                return "";
            else
                return value.ToString();
        }

        private void SetHiddenValue(string value)
        {
            hiddenUserID.Set("UserID", value);
        }

        private bool HasQueryStringChanged()
        {
            string hiddenValue = GetHiddenValue();

            if (hiddenValue.CompareTo(user) != 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Exception", String.Format("$('#expModal').modal('show')"), true);
                return true;
            }

            return false;
        }
    }
}