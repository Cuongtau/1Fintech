using System.Web;
using System.Threading;
using System.Globalization;
namespace PayWallet.PortalGateway.Utils
{
    public class LanguageHelper
    {
        public static void SetLanguage()
        {
            string CurrentURL_Path = HttpContext.Current.Request.Path.ToLower();
            if (!CurrentURL_Path.StartsWith("/api"))
            {
                if (CurrentURL_Path.Contains("/api"))
                {
                }
                else if (CurrentURL_Path.Contains(".aspx") || CurrentURL_Path.Contains(".ashx"))
                {
                    return;
                }
                else
                {
                    CultureInfo culture = new CultureInfo("vi-VN");
                    var language = HttpContext.Current.Request.QueryString["l"] ?? "";
                    if (string.IsNullOrEmpty(language))
                    {
                        language = "vn";
                    }
                    switch (language)
                    {
                        case "vn":
                            culture = new CultureInfo("vi-VN");
                            break;
                        case "en":
                            culture = new CultureInfo("en-US");
                            break;
                        default:
                            culture = new CultureInfo("vi-VN");
                            break;
                    }
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;

                    HttpContext.Current.Response.Cookies.Set(new HttpCookie("language", language));
                    HttpContext.Current.Request.Cookies.Set(new HttpCookie("language", language));
                }
            }
        }

        public static void SetLanguage(string language)
        {
            string CurrentURL_Path = HttpContext.Current.Request.Path.ToLower();
            if (!CurrentURL_Path.StartsWith("/api"))
            {
                if (CurrentURL_Path.Contains("/api"))
                {
                }
                else if (CurrentURL_Path.Contains(".aspx") || CurrentURL_Path.Contains(".ashx"))
                {
                    return;
                }
                else
                {
                    CultureInfo culture = new CultureInfo("vi-VN");
                    switch (language)
                    {
                        case "vn":
                            culture = new CultureInfo("vi-VN");
                            break;
                        case "en":
                            culture = new CultureInfo("en-US");
                            break;
                        default:
                            culture = new CultureInfo("vi-VN");
                            break;
                    }
                    HttpContext.Current.Request.Cookies.Set(new HttpCookie("language", language));
                    HttpContext.Current.Response.Cookies.Set(new HttpCookie("language", language));
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
            }
        }

        public static string GetCurrentLanguage()
        {
            var culture = new CultureInfo("vi-VN");
            string language = "vi";
            var langCurrent = HttpContext.Current.Request.Cookies["language"]; 
            if (langCurrent != null && !string.IsNullOrEmpty(langCurrent.Value))
            {
                switch (langCurrent.Value)
                {
                    case "vn":
                        language = "vi";
                        culture = new CultureInfo("vi-VN");
                        break;
                    case "en":
                        language = "en";
                        culture = new CultureInfo("en-US");
                        break;
                }
            }
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            return language;
        }

        public static int GetLanguageId(string language)
        {
            int langId;
            switch (language)
            {
                case "vi":
                    langId = 1;
                    break;
                case "en":
                    langId = 2;
                    break;
                default:
                    langId = 1;
                    break;
            }
            return langId;
        }

        public static void SetLangApi()
        {
            CultureInfo culture = new CultureInfo("vi-VN");
            var langCurrent = HttpContext.Current.Request.Cookies["language"] ?? HttpContext.Current.Response.Cookies["language"];
            if (langCurrent != null && !string.IsNullOrEmpty(langCurrent.Value))
            {
                switch (langCurrent.Value)
                {
                    case "vn":
                        culture = new CultureInfo("vi-VN");
                        break;
                    case "en":
                        culture = new CultureInfo("en-US");
                        break;
                    default:
                        culture = new CultureInfo("vi-VN");
                        break;
                }
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                HttpContext.Current.Request.Cookies.Set(new HttpCookie("language", langCurrent.Value));
                HttpContext.Current.Response.Cookies.Set(new HttpCookie("language", langCurrent.Value));
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                HttpContext.Current.Request.Cookies.Set(new HttpCookie("language", "vn"));
                HttpContext.Current.Response.Cookies.Set(new HttpCookie("language", "vn"));
            }
        }
    }

}