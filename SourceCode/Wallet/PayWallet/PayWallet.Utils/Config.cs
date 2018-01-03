using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using PayWallet.Utils.Security;

namespace PayWallet.Utils
{
    public static class Config
    {
        #region enum
        public enum AccountType : byte
        {
            PERSONAL_ACCOUNT = 1,         //Tài khoản Cá nhân
            CORPORATE_ACCOUNT = 2,         // Tài khoản doanh nghiep
        }
        #endregion

        #region[AppSetting]

        public static string VersionOfJs
        {
            get
            {
                return GetAppsetting("VERSIONJS");
            }
        }

        #endregion

        #region[Url]
        public static string ApplicationUrl
        {
            get
            {
                var url = ConfigurationManager.AppSettings["DOMAIN"] ?? "//" + HttpContext.Current.Request.Url.Authority +
                   HttpContext.Current.Request.ApplicationPath;
                return url.EndsWith("/") ? url : url + "/";
            }
        }

        public static string FilePathAppsetting
        {
            get { return GetAppsetting("FILE.PATH"); }
        }

        public static string GetCurrentLanguage()
        {
            var language = RequestUtility.GetParam(RequestUtility.REQUEST_LANGUAGE) ?? "vi";
            switch (language.ToString())
            {
                case "vi":
                    return "vi";
                case "en":
                    return "en";
                default:
                    return "vi";
            }
        }
        public static string GetCurrentUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        public static string GetRawUrl()
        {
            var rawUrl = HttpContext.Current.Request.ApplicationPath != "/"
                             ? HttpContext.Current.Request.RawUrl.Replace(HttpContext.Current.Request.ApplicationPath.ToLower(), string.Empty)
                             : HttpContext.Current.Request.RawUrl;
            //NLogLogger.LogInfo(HttpContext.Current.Request.ApplicationPath + "|" + HttpContext.Current.Request.RawUrl + "|" + rawUrl);
            var url = string.Format("{0}", rawUrl);
            return url.IndexOf("?") > 0 ? url.Substring(0, url.IndexOf("?")) : url;
        }

        public static string GetRawUrl(int type)
        {
            var applicationUrl = string.Empty;
            switch (type)
            {
                case 1:
                    applicationUrl = ApplicationUrl;
                    break;
            }
            applicationUrl = applicationUrl.Substring(0, applicationUrl.Length - 1);
            var displayUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            var rawUrl = displayUrl.Replace(applicationUrl.ToLower(), string.Empty);
            var url = string.Format("{0}", rawUrl);
            return url.IndexOf("?") > 0 ? url.Substring(0, url.IndexOf("?")) : url;
        }

        /// <summary>
        /// Domain website
        /// </summary>
        public static string Domain { get { return GetAppsetting("DOMAIN"); } }

        /// <summary>
        /// ThangNN: link api cac ham xu ly
        /// </summary>
        public static string DomainAPI { get { return GetAppsetting("DOMAIN_API"); } }

        public static string GetRawUrlNonQueryString()
        {
            var rawUrl = HttpContext.Current.Request.RawUrl;
            var prefixUrl = "/";
            var url = string.Format("{0}", rawUrl);
            if (rawUrl.Contains("home"))
                prefixUrl = rawUrl.Substring(0, rawUrl.IndexOf("home")) + "home/";
            else if (rawUrl.Contains("eshop"))
                prefixUrl = rawUrl.Substring(0, rawUrl.IndexOf("eshop")) + "eshop/";
            else if (rawUrl.Contains("nap-vcoin"))
                prefixUrl = rawUrl.Substring(0, rawUrl.IndexOf("/nap-vcoin-")) + "/";
            else if (rawUrl.Contains("tin-tuc"))
                prefixUrl = rawUrl.Substring(0, rawUrl.IndexOf("tin-tuc")) + "tin-tuc/";

            url = rawUrl.Replace(prefixUrl, "/");
            //NLogLogger.LogInfo(rawUrl + "|" + prefixUrl + "|" + url);
            var eshopurl = url.IndexOf("?") > 0 ? url.Substring(0, url.IndexOf("?")) : url;

            //Xử lý TH có url trong url
            var post_html = eshopurl.IndexOf(".html");
            if (post_html < 0) return eshopurl;
            var url1 = eshopurl.Substring(0, post_html);
            var url2 = eshopurl.Substring(post_html);
            //
            var arr = url1.Split('-');
            var i = arr.Length - 1;
            while (i >= 0)
            {
                int a;
                if (!int.TryParse(arr[i], out a))
                {
                    break;
                }
                var tmp = new List<string>(arr);
                tmp.RemoveAt(i);
                arr = tmp.ToArray();
                i--;
            }
            var link = arr.Aggregate((current, next) => current + "-" + next) + url2;
            return link;
        }

        /// <summary>
        /// ThangNN: Chuyển có dấu thành ko dấu
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertUCS2(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        #endregion

        #region[Configuration]
        public static string GetAppsetting(string appSettingName)
        {
            return ConfigurationManager.AppSettings[appSettingName] ?? string.Empty;
        }

        #endregion

        #region[Session]
        public static string GetRandomTockenKey(int l)
        {
            const string key = "123456789ABCDEFGHIJKLMNPQRSTUVXYZ";
            var keyLenght = key.Length;
            var rnd = new Random();
            var s = "";
            for (var i = 0; i < l; i++)
            {
                s = s + key[rnd.Next(keyLenght)];
            }
            return s;
        }

        public static string GetRandomCharacter(int length)
        {
            const string key = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var keyLenght = key.Length;
            var rnd = new Random();
            var s = "";
            for (var i = 0; i < length; i++)
            {
                s = s + key[rnd.Next(keyLenght)];
            }
            return s;
        }

        public static bool CheckAccountChar(string userName)
        {
            // Độ dài từ 4-16
            if (userName.Length < 4 || userName.Length > 16) return false;

            //Kí tự đầu tiên phải là chữ cái
            string fillterChar = "abcdefghijklmnopqrstuvxyzw";
            if (fillterChar.IndexOf(userName[0]) < 0) return false;

            //Kí tự '.' không được xuất hiện liền nhau
            if (userName.IndexOf("..") >= 0) return false;

            //Kí tự '_' không được xuất hiện liền nhau
            if (userName.IndexOf("__") >= 0) return false;

            //Kí tự '._' không được xuất hiện liền nhau
            if (userName.IndexOf("._") >= 0) return false;

            //Kí tự '-.' không được xuất hiện liền nhau
            if (userName.IndexOf("_.") >= 0) return false;

            // Ký tự '.' không được ở sau cùng
            if (userName.EndsWith(".")) return false;

            // Ký tự '_' không được ở sau cùng
            if (userName.EndsWith("_")) return false;

            //Chuỗi hợp lệ   abcdefghijklmnopqrstuvxyzw012345678.
            fillterChar = "abcdefghijklmnopqrstuvxyzw0123456789._";
            for (int i = 0; i < userName.Length; i++)
            {
                if (fillterChar.IndexOf(userName[i]) < 0) return false;
            }

            return true;
        }


        /// <summary>
        /// Kiểm tra email có đúng định dạng
        /// </summary>
        /// <param name="email"></param>
        public static bool CheckEmail(string email)
        {
            //Kiểm tra định dạng email
            return Regex.IsMatch(email, @"^([0-9a-z]+[-._+&])*[0-9a-z]+@([-0-9a-z]+[.])+[a-z]{2,6}$", RegexOptions.IgnoreCase);
        }

        public static bool isAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9]*$");
            return rg.IsMatch(strToCheck);
        }

        #endregion

        #region Utility
        public static bool IsMobileBrowser()
        {
            //GETS THE CURRENT USER CONTEXT
            HttpContext context = HttpContext.Current;

            //FIRST TRY BUILT IN ASP.NT CHECK
            if (context.Request.Browser.IsMobileDevice)
            {
                return true;
            }
            //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }
            //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return true;
            }
            //AND FINALLY CHECK THE HTTP_USER_AGENT 
            //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                //Create a list of all mobile types
                var mobiles =
                    new[]
                {
                    "midp", "j2me", "avant", "docomo",
                    "novarra", "palmos", "palmsource",
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/",
                    "blackberry", "mib/", "symbian",
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio",
                    "SIE-", "SEC-", "samsung", "HTC",
                    "mot-", "mitsu", "sagem", "sony"
                    , "alcatel", "lg", "eric", "vx",
                    "NEC", "philips", "mmm", "xx",
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java",
                    "pt", "pg", "vox", "amoi",
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo",
                    "sgh", "gradi", "jb", "dddi",
                    "moto", "iphone"
                };

                //Loop through each item in the list created above 
                //and check if the header contains that text
                return mobiles.Any(s => context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()));
            }

            return false;
        }

        public static bool IsNumber(string strNumber)
        {
            return !new Regex("[^0-9]").IsMatch(strNumber);
            //const string MatchNumber = @"^[0-9]$";
            //return System.Text.RegularExpressions.Regex.IsMatch(strNumber, MatchNumber);
        }

        public static bool IsEmail(string email)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                   @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                   @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(email))
                return (true);
            else
                return (false);
        }

        /// <summary>
        /// Get ra kiểu email (yahoo, gmail, msn) từ địa chỉ email
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns>
        /// 0: Loại khác
        /// 1: Yahoo
        /// 2: gmail
        /// 3: msn
        /// </returns>
        public static int GetEmailType(string emailAddress)
        {
            var listyahoo = new List<string> { "yahoo", "ymail.com", "rocketmail.com" };
            var listgmail = new List<string> { "gmail.com", "googlemail.com" };
            var listmsn = new List<string> { "hotmail.com", "msn.com", "live.com" };
            var domain = emailAddress.Substring(emailAddress.IndexOf('@') + 1);
            if (listyahoo.Exists(e => domain.ToLower().StartsWith(e)))
                return 1;
            if (listgmail.Exists(e => domain.ToLower().StartsWith(e)))
                return 2;
            return listmsn.Exists(e => domain.ToLower().StartsWith(e)) ? 3 : 0;
        }



        /// <summary>
        /// Lấy ip
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            var ip = "";
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (ip == "")
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        /// <summary>
        /// Kiểm tra độ mạnh của password
        /// pass phải từ 6-18 ký tự, bao gồm cả chữ hoa, thường và số
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsPasswordStrong(string password)
        {
            //ThangNN: Sửa theo rule pay 2.0, password chỉ cần số và ký tự thường
            //return Regex.IsMatch(password, @"^(?=.{6,18})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$");
            return Regex.IsMatch(password, @"^(?=.{6,18})(?=.*\d)(?=.*[a-z]).*$");
        }

        public static string ConvertToDbString(string originString)
        {
            //ThangNN: Remove các ký tự nhạy cảm trong db : |,#,' thành space
            Regex pattern = new Regex("[#|']");
            originString = pattern.Replace(originString, " ");
            //Convert multiple space to single space
            originString = Regex.Replace(originString, @"\s+", " ");
            return originString;
        }

        public static bool CheckXSSInput(string input, bool usingLib = true)
        {
            try
            {
                if (usingLib)
                    return !(new AntiXssValidator().Validate(input));
                else
                    return !CustomCheckXSSInput(input);

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return false;
            }
        }

        public static bool CustomCheckXSSInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            string[] XSSTagStringsForDoubleOpening = {"iframe","script","style","input","a","abbr",
                "acronym", "address", "applet", "area", "article","aside","audio","b","base","basefont","bdi","bdo",
                "big","blockquote","body","br","button","canvas","caption","center","cite","code","col","colgroup","datalist",
                "dd","del","details","dfn","dialog","dir","div","dl","dt","em","embed","fieldset","figcaption","figure","font",
                "footer","form","frame","frameset","h1","head","header","hr","html","i","img","ins","kbd","keygen","label","legend","li",
                "link","main","map","mark","menu","menuitem","meta","meter","nav","noframes","noscript","object","ol","optgroup","option",
                "output","p","param","picture","pre","progress","q","rp","rt","ruby","s","samp","section","select","small","source","span",
                "strike","strong","strong","sub","summary","sup","table","tbody","td","textarea","tfoot","th","thead","time","title","tr","track",
                "tt","u","ul","var","video","wbr"};

            string[] XSSHtmlEntities = {"&excl;","&#33;","&#x21;","U+00021", "&quot;", "&num;", "&percnt;", "&amp;", "&apos;", "&lpar;", "&rpar;", "&ast;", "&comma;",
                "&period;","&sol;","&colon;","&semi;","&quest;","&commat;","&lbrack;","&bsol;", "&rbrack;","&Hat","&lowbar;","&grave;", "&lbrace;","&vert;",
                "&rbrace;","&tilde;","&acute;","&lsquo;","&rsquo;","&sbquo;","&ldquo;","&rdquo;", "&#34;","&#x22;","U+00022","U+00023","&#x23;","&#35;","U+00026",
                "&#x26;","&#38;","&#39;","&#x27;","U+00027","U+00028","&#x28;","&#40;","&#41;","&#x29;","U+00029","&#58;","&#x3a;","U+0003A","&#59;","&#x3b;",
                "U+0003B","U+0007B","&#x7b;","&#123;","U+0007D","&#x7d;","&#125;","&lt;","&gt;","&#60;","&#62;"};

            string[] XSSJSFunction = { "onclick","oncontextmenu","ondblclick","onmousedown","onmouseenter","onmouseleave","onmousemove","onmouseover","onmouseout","onmouseup","onkeydown","onkeypress","onkeyup","onabort","onbeforeunload","onerror","onhashchange","onload","onpageshow",
                "onpagehide","onresize","onscroll","onunload","onblur","onchange","onfocus","onfocusin","onfocusout","oninput","oninvalid","onreset","onsearch","onselect","onsubmit","ondrag","ondragend","ondragenter","ondragleave","ondragover","ondragstart",
                "ondrop","oncopy","oncut","onpaste" };
            var checkWords = input.ToLower().Trim().Replace(" ", ""); //take out all space to protect < iframe

            return (checkWords.StartsWith("<") 
                || XSSHtmlEntities.Any(ent => checkWords.Contains(ent)) 
                || XSSTagStringsForDoubleOpening.Any(tag => checkWords.Contains("<" + tag)) 
                || XSSJSFunction.Any(evt => checkWords.Contains(evt + "=")));
        }


        #endregion Utility

        public static string GetServerIP()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress address in ipHostInfo.AddressList)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return address.ToString();
            }

            return string.Empty;
        }

        public static int AccountId
        {
            get
            {
                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && !HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        SessionUtility.Remove(Contants.SESSION_ACCOUNT);
                        return 0;
                    }

                    var current = SessionUtility.GetValue(Contants.SESSION_ACCOUNT);
                    if (current == null)
                    {
                        var isLogin = HttpContext.Current.User.Identity.Name;
                        if (!string.IsNullOrEmpty(isLogin))
                        {
                            current = isLogin;
                            SessionUtility.SetValue(Contants.SESSION_ACCOUNT, isLogin);
                        }
                    }
                    else
                    {
                        var isLogin = HttpContext.Current.User.Identity.Name;
                        if (current.ToString() != isLogin)
                        {
                            HttpContext.Current.Session.RemoveAll(); //Remove toàn bộ Session
                            current = isLogin;
                            SessionUtility.SetValue(Contants.SESSION_ACCOUNT, isLogin);
                        }
                    }
                    if (current == null) return 0;
                    var arrCurrent = current.ToString().Split('|');
                    return arrCurrent.Length < 6 ? 0 : Convert.ToInt32(arrCurrent[0]);
                }
                catch (Exception ex)
                {
                    NLogLogger.PublishException(ex);
                    return 0;
                }
            }
        }
    }
}
