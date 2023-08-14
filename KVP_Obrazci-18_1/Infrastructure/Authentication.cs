using KVP_Obrazci.Domain.KVPOdelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using DevExpress.Xpo;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Common;
using KVP_Obrazci.Resources;
using System.Configuration;
using Newtonsoft.Json;
using KVP_Obrazci.Helpers;

namespace KVP_Obrazci.Infrastructure
{
    public class Authentication
    {
        private IUserRepository userRepo;
        public Authentication(Session session)
        {
            userRepo = new UserRepository(session);
        }
        public bool Authenticate(string username, string password, bool rememberMe)
        {
            UserModel user = null;

            user = userRepo.UserLogIn(username, password);
            SerializeUser(user);

            if (rememberMe)
            {
                if (HttpContext.Current.Request.Cookies["RememberMeCookie"] != null)
                    InfrastructureHelper.TryRemoveCookie("RememberMeCookie");

                string jsonUser = JsonConvert.SerializeObject(new UserCredentials { Password = password, Username = username });
                FormsAuthenticationTicket rememberMeTicket = new FormsAuthenticationTicket(2, username, DateTime.Now, DateTime.Now.AddDays(30), true, jsonUser);
                string encyptTicket = FormsAuthentication.Encrypt(rememberMeTicket);
                HttpCookie rememberMeCookie = new HttpCookie("RememberMeCookie", encyptTicket) { HttpOnly = false, Expires = DateTime.Now.AddMonths(1) };
                HttpContext.Current.Response.Cookies.Add(rememberMeCookie);
            }

            return true;
        }

        public bool AuthenticateWithCard(string token)
        {
            UserModel user = null;

            user = userRepo.UserLogInCard(token);
            SerializeUser(user);
            
            return true;
        }

        private void SerializeUser(UserModel user)
        {
            if (user != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                string userData = serializer.Serialize(user);
                string sessionExpires = ConfigurationManager.AppSettings["SessionTimeoutInMinutes"].ToString();
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                     (!String.IsNullOrEmpty(user.Card) ? user.Card : user.username),
                     DateTime.Now,
                     DateTime.Now.AddMinutes(CommonMethods.ParseDouble(sessionExpires)),
                     false,
                     userData);

                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket) { HttpOnly = false, Expires = DateTime.Now.AddMonths(1) };
                HttpContext.Current.Response.Cookies.Add(faCookie);

                InfrastructureHelper.SetCookieValue(Enums.Cookies.SessionExpires.ToString(), sessionExpires);
            }
            else
                throw new EmployeeCredentialsException(AuthenticationValidation_Exception.res_01);
        }

        public UserCredentials GetUsernameAndPassword()
        {
            var rememberhCookie = HttpContext.Current.Request.Cookies["RememberMeCookie"];
            if (rememberhCookie != null)
            {
                FormsAuthenticationTicket rememberTicket = FormsAuthentication.Decrypt(rememberhCookie.Value);

                UserCredentials obj = JsonConvert.DeserializeObject<UserCredentials>(rememberTicket.UserData);

                return obj;
            }
            return null;
        }
    }

    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}