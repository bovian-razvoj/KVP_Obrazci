using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Helpers
{
    public static class InfrastructureHelper
    {
        public static string GetCookieValue(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                return HttpContext.Current.Request.Cookies[cookieName].Value;
            }
            else
                return "";
        }

        public static void SetCookieValue(string cookieName, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Value = value;
            }
            else
            {
                cookie = new HttpCookie(cookieName);
                cookie.Value = value;
                //HttpContext.Current.Response.Cookies.Add(new HttpCookie(cookieName, value)
                //{
                //    HttpOnly = false,
                //    Expires = DateTime.Now.AddMonths(1)
                //});
            }

            cookie.Expires = DateTime.Now.AddMonths(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void TryRemoveCookie(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                HttpCookie myCookie = new HttpCookie(cookieName);
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }
    }
}