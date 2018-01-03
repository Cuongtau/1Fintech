using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PayWallet.Utils;

namespace PayWallet.PortalGateway
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Web.Optimization.BundleTable.EnableOptimizations = false;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            //if (HttpContext.Current.Request.Url.AbsoluteUri.StartsWith("http://sandbox.vtcpay.vn/pay2.0"))
            //{
            //    var uri = HttpContext.Current.Request.Url.AbsoluteUri;
            //    NLogLogger.LogInfo("AbsoluteUri : " + uri);
            //    string param = string.Empty;
            //    var urlRedirect = HttpContext.Current.Request.Url.AbsoluteUri.Replace("sandbox.vtcpay.vn/pay2.0", "sandbox1.vtcpay.vn");//HttpContext.Current.Request.Url.AbsoluteUri.Replace("http://", "https://").Replace(":8001", "").Replace(":80", "");
                
            //    HttpContext.Current.Response.Redirect(urlRedirect);
            //    return;
            //}

            string Origin = Request.Headers["Origin"];
            if (!string.IsNullOrEmpty(Origin))
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", Origin);
                Response.Headers.Add("Access-Control-Allow-Credentials", "true");

            }


            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS,PUT,DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization, Accept");
                HttpContext.Current.Response.End();
            }

        }
        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();
            NLogLogger.PublishException(exception);
            HttpException httpException = exception as HttpException;
            string action = string.Empty;
            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 400://Badrequest
                        action = "ErrorBadRequest";
                        break;
                    case 401://UnAuthorize
                        action = "ErrorUnAuthorize";
                        break;
                    case 403://Permission
                        action = "ErrorPermission";
                        break;
                    case 404:
                        // page not found 
                        action = "ErrorNotPage";
                        break;
                    case 500:
                        action = "ErrorInternalServer";
                        break;
                    default:
                        action = "ErrorPermission";
                        break;
                }
            }
            else
            {
                //TODO: Define action for other exception types
                action = "ErrorPermission";
            }
            Server.ClearError();
            var notfound_page = String.Format("{0}Error/{1}", Config.Domain, action);
            Response.Redirect(notfound_page, true);
        }

        protected void Session_Start() { }

        protected void Session_End() { }
    }
}
