using KVP_Obrazci.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Xpo;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Infrastructure;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Common;
using System.Web.Security;
using KVP_Obrazci.Resources;
using System.Collections;
using DevExpress.Web;
using KVP_Obrazci.Widgets;

namespace KVP_Obrazci
{
    public partial class Home : ServerMasterPage
    {
        Authentication auth;
        IUserRepository userRepo;
        IKVPGroupsRepository kvpGroupsRepo;
        IEmployeeRepository employeeRepo;
        IPostRepository postRepo;
        Session session = null;
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            session = XpoHelper.GetNewSession();
            postRepo = new PostRepository(session);
            employeeRepo = new EmployeeRepository(session);

            if (Request.IsAuthenticated)
            {
                MasterPageFile = "~/MasterPages/Main.Master";
            }


        }
        protected void Page_Init(object sender, EventArgs e)
        {
            SetPostNews();
            userRepo = new UserRepository(session);
            kvpGroupsRepo = new KVPGroupsRepository(session);
            auth = new Authentication(session);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                ASPxFormLayoutLogin.Visible = false;
                FormLayoutWrap.Style.Add("display", "none");
                HomeContent.Style.Add("display", "block");

                if (PrincipalHelper.IsUserAdmin() || PrincipalHelper.IsUserSuperAdmin())
                {
                    kodeksChanges.Style.Add("display", "block");

                    int newEmployees = employeeRepo.GetNewEmployeesCount();
                    int deletedEmployeesInGroups = kvpGroupsRepo.GetDeletedUserCountWithKVPGroup();
                    int notEmployeedAnyMoreCount = employeeRepo.GetNotEmployeedAnymoreEmployeeesCount();
                    int employeeNameChanged = employeeRepo.GetEmployeesNameChangedCount();
                    int employeeDuplicated = employeeRepo.GetEmployeesDuplicatedCount();

                    if (newEmployees > 0)
                    {
                        btnNewEmployees.ClientVisible = true;
                        btnNewEmployees.Text = "<span class='badge' style='margin-bottom: 8px; color:white'>" + newEmployees.ToString() + "</span> <br /> Novi zaposleni";
                    }

                    if (deletedEmployeesInGroups > 0)
                    {
                        btnDeletedEmployees.ClientVisible = true;
                        btnDeletedEmployees.Text = "<span class='badge' style='margin-bottom: 8px; color:white'>" + deletedEmployeesInGroups.ToString() + "</span> <br /> Odstranjeni zaposleni";
                    }

                    if (notEmployeedAnyMoreCount > 0)
                    {
                        btnEmployeeLeft.ClientVisible = true;
                        btnEmployeeLeft.Text = "<span class='badge' style='margin-bottom: 8px; color:white'>" + notEmployeedAnyMoreCount.ToString() + "</span> <br /> Zaposleni zapustili <br /> podjetje";
                    }

                    if (employeeNameChanged > 0)
                    {
                        btnEmployeeNameChanged.ClientVisible = true;
                        btnEmployeeNameChanged.Text = "<span class='badge' style='margin-bottom: 8px; color:white'>" + employeeNameChanged.ToString() + "</span> <br /> Spremembe <br/>osebnih podatkov";
                    }

                    if (employeeDuplicated > 0)
                    {
                        btnEmployeeDuplicated.ClientVisible = true;
                        btnEmployeeDuplicated.Text = "<span class='badge' style='margin-bottom: 8px; color:white'>" + employeeDuplicated.ToString() + "</span> <br /> Zaposleni podvojeni";
                    }
                }
                Session["SessionEndModal"] = true;

                
            }
            else
            {
                HomeContent.Style.Add("display", "none");
                bool showWarning = CommonMethods.ParseBool(Session["SessionEndModal"]);
                if (showWarning)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SessionExparation", String.Format("$('#sessionEndModal').modal('show')"), true);
                    RemoveSession("SessionEndModal");
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();
                }

                UserCredentials obj = auth.GetUsernameAndPassword();
                if (obj != null)
                {
                    if (String.IsNullOrEmpty(txtUsername.Text) && String.IsNullOrEmpty(txtPassword.Text))
                    {
                        txtUsername.Text = obj.Username;
                        txtPassword.ClientSideEvents.Init = "function(s, e) {s.SetText('" + obj.Password + "');}";
                    }
                }
            }

        }

        protected void LoginCallback_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            bool signInSuccess = false;
            string message = "";
            string username = "";// CommonMethods.Trim(txtUsername.Text);
            string password = "";//CommonMethods.Trim(txtPassword.Text);
            string koda = "";

            try
            {

                if (e.Parameter.Contains("SignInUserCredentials"))
                {
                    username = CommonMethods.Trim(txtUsername.Text);
                    password = CommonMethods.Trim(txtPassword.Text);
                }
                else
                    koda = CommonMethods.ConvertEmployeeTokenToJantarCode(e.Parameter);



                if (username != "" && password != "")
                {
                    signInSuccess = auth.Authenticate(username, password, rememberMeCheckBox.Checked);
                }
                else
                    signInSuccess = auth.AuthenticateWithCard(koda);

            }
            catch (EmployeeCredentialsException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace);
                message = AuthenticationValidation_Exception.res_01;
            }


            if (signInSuccess)
            {
                Session.Remove("PreviousPage");

                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
                HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
                HttpContext.Current.Response.AddHeader("Expires", "0");
            }
            else
            {
                LoginCallback.JSProperties["cpResult"] = message;
            }
        }

        protected void ASPxPager_PageIndexChanged(object sender, EventArgs e)
        {
            MultiViewPost.ActiveViewIndex = (sender as ASPxPager).PageIndex;
        }

        private void SetPostNews()
        {
            List<PostModel> posts = postRepo.GetLastPosts(3);
            if (posts != null)
            {
                int index = 1;
                foreach (var item in posts)
                {
                    NewsPost postUC = (NewsPost)LoadControl("~/Widgets/NewsPost.ascx");

                    System.Web.UI.WebControls.View view = new System.Web.UI.WebControls.View();
                    view.ID = "View" + index.ToString();
                    view.Controls.Add(postUC);
                    MultiViewPost.Views.Add(view);// Controls.Add(postUC);

                    postUC.FillPostControl(item);
                    index++;
                }

                if (posts.Count > 0)
                {
                    ASPxPager.ItemCount = posts.Count;
                    MultiViewPost.ActiveViewIndex = 0;
                }
            }
        }
    }
}