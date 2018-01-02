using System;
using System.Web;
using System.Web.Security;

namespace PayWallet.Utils
{
    public class CookieUtility
    {
        public static void RemoveAllCookie()
        {
            for (var i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
            {
                var cookies = HttpContext.Current.Request.Cookies.Get(i);

                if (cookies.Name == FormsAuthentication.FormsCookieName) continue;
                //if (cookies.Name == Config.COOKIE_EVENT_NAME) continue;

                cookies.Expires = DateTime.Now.AddDays(-1);
                cookies.Value = string.Empty;
                HttpContext.Current.Response.Cookies.Set(cookies);
            }
        }

        public static void SetCookie(string name, string value, int minutes)
        {
            var cookies = new HttpCookie(name) { Value = value, Expires = DateTime.Now.AddMinutes(minutes) };
            HttpContext.Current.Response.Cookies.Set(cookies);
        }

        public static HttpCookie GetCookie(string name)
        {
            var cookies = HttpContext.Current.Request.Cookies[name];
            return cookies;
        }

        public static void RemoveCookie(string name)
        {
            var cookies = HttpContext.Current.Request.Cookies[name];
            if (cookies == null) return;
            cookies.Value = string.Empty;
            cookies.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Set(cookies);
        }
    }
}
