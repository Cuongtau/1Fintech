using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SMSGatewayAPI
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //UnityConfig.RegisterComponents();
            Bootstrapper.Initialise();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            string sessionId = Session.SessionID;
            Session["SessionID"] = sessionId;
        }
        protected void Application_PostAuthorizeRequest()
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //var Host = Request.UrlReferrer != null ? Request.UrlReferrer.Host : string.Empty;
            //	string cookieDomain = FormsAuthentication.CookieDomain;

            //Common.Utilities.Log.NLogManager.LogMessage("Host:" + Host+":"+ cookieDomain);
            //if (!string.IsNullOrEmpty(Host) && !cookieDomain.Contains(Host)) return;

            string origin = Request.Headers["Origin"];
            if (!string.IsNullOrEmpty(origin))
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", origin);
                Response.Headers.Add("Access-Control-Allow-Credentials", "true");

            }
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS,PUT,DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization, Accept");
                HttpContext.Current.Response.End();
            }

            if (Request.Url.Host.StartsWith("www"))
            {
                UriBuilder builder = new UriBuilder(Request.Url);
                builder.Host = Request.Url.Host.Replace("www.", "");
                Response.StatusCode = 301;
                Response.AddHeader("Location", builder.ToString().Replace(":80", ""));
                Response.End();
            }
        }
    }
}
