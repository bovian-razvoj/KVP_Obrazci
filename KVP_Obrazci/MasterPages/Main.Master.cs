using DevExpress.Web;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace KVP_Obrazci.MasterPages
{
    public partial class Main : System.Web.UI.MasterPage
    {
        private bool disableNavBar;
        private IActiveUserRepository activeUserRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            activeUserRepo = new ActiveUserRepository();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CommonMethods.ParseBool(ConfigurationManager.AppSettings["TestMode"]))
                TestEnviromentLabel.Style.Add("display", "block");

            if (Request.IsAuthenticated)
            {
                btnAppVersion.Visible = false;

                SetMainMenuBySignInRole();
                btnSignOut.ImageUrl = "../Images/lock_unlock.png";
                lblLogin.Text = PrincipalHelper.GetUserPrincipal().lastName + ", " + PrincipalHelper.GetUserPrincipal().firstName;
                UserRoleLabel.Text = PrincipalHelper.GetUserPrincipal().RoleName;

                //Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                
                btnAppVersion.Text = "O verziji";// + ConfigurationManager.AppSettings["APP_Version"].ToString();
                //lblAppVersion.Text = "Verzija : " + version;

                InfrastructureHelper.SetCookieValue(Enums.Cookies.UserLastRequest.ToString(), DateTime.Now.ToString("dd M yyyy HH mm ss"));
                activeUserRepo.SaveLastRequest(PrincipalHelper.GetUserPrincipal().ID);
            }
            else
            {
                Session["PreviousPage"] = Request.RawUrl;
                InfrastructureHelper.SetCookieValue(Enums.Cookies.UserLastRequest.ToString(), "STOP");
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
            }
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            InfrastructureHelper.SetCookieValue(Enums.Cookies.UserLastRequest.ToString(), "STOP");
            FormsAuthentication.SignOut();

            
            activeUserRepo.SaveUserLoggedInActivity(false, PrincipalHelper.GetUserPrincipal().ID);

            Response.Redirect("~/Home.aspx");
        }

        private void SetMainMenuBySignInRole()
        {
            if (PrincipalHelper.IsUserSuperAdmin())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.SuperAdmin.ToString());
                btnAppVersion.Visible = true;
            }
            else if (PrincipalHelper.IsUserAdmin())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Admin.ToString());
                btnAppVersion.Visible = true;
            }
            else if (PrincipalHelper.IsUserEmployee())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Employee.ToString());
                ASPxPanelMenu.ClientVisible = false;
            }
            else if (PrincipalHelper.IsUserChampion())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Champion.ToString());
                //ASPxPanelMenu.ClientVisible = false;
            }
            else if (PrincipalHelper.IsUserLeader())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Leader.ToString());
                //ASPxPanelMenu.ClientVisible = false;
            }
            else if (PrincipalHelper.IsUserTpmAdmin())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Employee.ToString());
                ASPxPanelMenu.ClientVisible = false;
            }
        }

        private void SetXmlDataSourceSetttings(string userRole)
        {
            XmlMenuDataSource.XPath = "MainMenu/" + userRole + "/Group";

            if (!DisableNavBar)
                NavBarMainMenu.Enabled = true;
            else
                NavBarMainMenu.Enabled = false;

        }

        public bool DisableNavBar
        {
            get { return disableNavBar; }
            set { disableNavBar = value; }
        }

        public ASPxNavBar NavigationBarMain
        {
            get { return NavBarMainMenu; }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if(Request.IsAuthenticated)
                Response.Redirect("Credentials/ChangePassword.aspx?" + Enums.QueryStringName.recordId.ToString() + "=" + CommonMethods.Base64Encode(PrincipalHelper.GetUserPrincipal().ID.ToString()));
        }

        
        public static void GetData()
        {
            MessageBox.Show("Calling From Client Side");
            //Your Logic comes here
        }

        protected void btnAppVersion_Click(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
                Response.Redirect("~/CompanySettings/VersionDetails.aspx");
        }
    }
}