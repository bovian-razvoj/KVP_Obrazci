using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using DevExpress.Web;
using System.Web.Script.Serialization;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Infrastructure;
using System.Security.Principal;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Common;
using KVP_Obrazci.Helpers;

namespace KVP_Obrazci
{
    public class Global_asax : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxWebControl.CallbackError += new EventHandler(Application_Error);
            DevExpress.Data.Helpers.ServerModeCore.DefaultForceCaseInsensitiveForAnySource = true;
        }

        void Application_End(object sender, EventArgs e)
        {
            // Code that runs on application shutdown
        }

        void Application_Error(object sender, EventArgs e)
        {
            string error = "";
            if (Context != null && Server.GetLastError() != null)
                CommonMethods.getError(Context.Error, ref error);

            if (HttpContext.Current.Error != null)
                CommonMethods.getError(HttpContext.Current.Error, ref error);

            //if is there error on client side we need aditional information about error

            error += "\r\n \r\n" + sender.GetType().FullName + "\r\n" + HttpContext.Current.Request.UrlReferrer.AbsoluteUri + "\r\n";

            string body = "Pozdravljeni! \r\n Uporabnik " + PrincipalHelper.GetUserPrincipal().firstName + " " +
                    PrincipalHelper.GetUserPrincipal().lastName + " je dne " + DateTime.Now.ToLongDateString() + " ob " + DateTime.Now.ToLongTimeString() +
                    " naletel na napako! \r\n Podrobnosti napake so navedene spodaj: \r\n \r\n" + error + "\r\n";

            bool isSent = CommonMethods.SendEmailToDeveloper("KVP - NAPAKA", "Napaka aplikacije", body);

            CommonMethods.LogThis(body);

            if(Context != null)
            Context.ClearError();
           

            Server.ClearError();

            if (error.Contains("&%") && error.Contains("messageType"))
            {
                string[] split = error.Split('&');
                int startIndex = error.IndexOf("&%") + 2;//eliminiramo znaka &%
                int length = error.Substring(startIndex).IndexOf("&%");
                string newString = error.Substring(startIndex, length);
                string messagetype = newString.Substring(newString.IndexOf("=") + 1);

                bool isCallback = CommonMethods.IsCallbackRequest(HttpContext.Current.Request);
                if (isCallback)
                    ASPxWebControl.RedirectOnCallback("~/Home.aspx?messageType=" + messagetype);
                else
                    HttpContext.Current.Response.Redirect("~/Home.aspx?messageType=" + messagetype);
            }
            else
                HttpContext.Current.Response.Redirect("~/Home.aspx?unhandledExp=true");
        }

        

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
        }

        void Session_End(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                UserModel serializeModel = serializer.Deserialize<UserModel>(authTicket.UserData);

                UserPrincipal userPrincipal = new UserPrincipal();

                userPrincipal.Identity = new GenericIdentity(authTicket.Name);
                userPrincipal.ID = serializeModel.ID;
                userPrincipal.firstName = serializeModel.firstName;
                userPrincipal.lastName = serializeModel.lastName;
                userPrincipal.email = serializeModel.email;
                userPrincipal.ProfileImage = serializeModel.profileImage;
                userPrincipal.DepartmentName = serializeModel.DepartmentName;
                userPrincipal.Card = serializeModel.Card;
                userPrincipal.Champion = serializeModel.Champion;
                userPrincipal.GroupName = serializeModel.GroupName;
                userPrincipal.GroupId = serializeModel.GroupID;
                userPrincipal.Supervisor = serializeModel.Supervisor;

                userPrincipal.Role = serializeModel.Role;
                userPrincipal.RoleId = serializeModel.RoleID;
                userPrincipal.RoleName = serializeModel.RoleName;

                HttpContext.Current.User = userPrincipal;
            }
        }
    }
}