using System;
using System.Web;

namespace PayWallet.Utils
{
    public class RequestUtility
    {
        public const string REQUEST_LANGUAGE = "l";//param language 

        public static object GetParam(string paramName)
        {
            try
            {
                var param = HttpContext.Current.Request.QueryString[paramName];
                return param != null ? (object)param : null;
            }
            catch (Exception exception)
            {
                return null;
            }
        }


        public static string IpClient()
        {

            //IP client khi sử dụng load balancing (citrix)
            if (HttpContext.Current.Request.ServerVariables["HTTP_CITRIX"] != null)
                return HttpContext.Current.Request.ServerVariables["HTTP_CITRIX"];
            //IP client khi dùng qua proxy
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //IP client thường 
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] ??
                   HttpContext.Current.Request.UserHostAddress;
        }

        public static string AuthenOpenId(string host, string email, int actionType, string redirect_uri)
        {
            var type = Config.GetEmailType(email);
            if (type == 0) return string.Empty; //Loại khác thì Return
            var urlOpenID = string.Empty;
            switch (type)
            {
                //ThangNN: Bỏ authenopenid = yahoo
                //case 1: //Yahoo
                //    urlOpenID = host + "api/OpenID/OpenIdCheck?openid_identifier=http://yahoo.com/" + "&ReturnUrl=" + redirect_uri;
                //    break;
                case 2: //Gmail
                    urlOpenID = host + "api/oauth/request_auth?login_type=google&actionType=" + actionType + "&redirect_uri=" + HttpUtility.UrlEncode(redirect_uri);
                    break;               
                default:
                    break;
            }
            return urlOpenID;
        }

        public static string LogoutReAuthenOpenId(string host, string email, int actionType, string accountName, string redirect_uri)
        {
            var type = Config.GetEmailType(email);
            if (type == 0) return string.Empty; //Loại khác thì Return
            var urlOpenID = string.Empty;
            switch (type)
            {
                //ThangNN: Bỏ authenopenid = yahoo
                //case 1: //Yahoo
                //    urlOpenID = host + "api/OpenID/OpenIdCheck?openid_identifier=http://yahoo.com/" + "&ReturnUrl=" + redirect_uri;
                //    break;
                case 2: //Gmail
                    var obj = new
                    {
                        login_type = "google",
                        actionType = actionType,
                        redirect_uri = redirect_uri,
                        username = accountName
                    };
                    string JsonObj = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);
                    string base64obj = Security.Security.Base64Encode(JsonObj);
                    urlOpenID = host + "api/oauth/request_auth?stateObjExtend=" + base64obj;
                    break;
                default:
                    break;
            }
            return urlOpenID;
        }

        public bool CheckUrl(string domain, string url)
        {
            if (url.ToLower().Trim().Contains(domain.ToLower().Trim()))
                return true;
            if (url.ToLower().Trim().Contains(HttpUtility.UrlEncode(domain.ToLower().Trim())))
                return true;
            return false;
        }
    }
}
