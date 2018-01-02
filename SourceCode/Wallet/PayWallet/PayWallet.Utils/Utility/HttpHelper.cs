using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace PayWallet.Utils
{
    public static class HttpHelper
    {
        public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
        {
            public TrustAllCertificatePolicy()
            { }

            public bool CheckValidationResult(ServicePoint sp,
              X509Certificate cert, WebRequest req, int problem)
            {
                return true;
            }
        }

        public static object GetHttpResponse(string url)
        {
            object response = null;
            try
            {
                ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

                myRequest.Method = "GET";
                //myRequest.ContentLength = data.Length;
                myRequest.CookieContainer = new CookieContainer();
                //myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentType = "text/xml; encoding='utf-8'";
                myRequest.KeepAlive = false;

                using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
                {
                    using (var reader = new StreamReader(myResponse.GetResponseStream()))
                    {
                        if (reader != null)
                        {
                            response = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // Graph API Errors or general web exceptions 

                response = ex.Message;

            }
            catch (Exception)
            { }

            return response;
        }

        /// <summary>Send the Message to PalPal Checkout</summary>
        public static string SendPost(string postData, string url)
        {
            bool success = false;
            string resp;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);
            System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };
            System.Net.ServicePointManager.Expect100Continue = false;
            // Gan tap hop cac security protocal se ho tro ( ssl3, tsl, tsl11, tsl 12) . Toan tu | tra ra mot enum 
            // Neu khong gan mac dinh se chi co SSL3, TSL (voi framswork 4.5)
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            CookieContainer cookie = new CookieContainer();
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "POST";
            myRequest.ContentLength = data.Length;
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.KeepAlive = false;
            myRequest.CookieContainer = cookie;
            myRequest.AllowAutoRedirect = false;
            using (Stream requestStream = myRequest.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
            }
            string responseXml = string.Empty;
            try
            {
                using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
                {
                    if (myResponse.StatusCode != HttpStatusCode.OK)
                        success = false;
                    else
                        success = true;
                    using (Stream respStream = myResponse.GetResponseStream())
                    {
                        using (StreamReader respReader = new StreamReader(respStream))
                        {
                            responseXml = respReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                if (webEx.Response != null)
                {
                    using (HttpWebResponse exResponse = (HttpWebResponse)webEx.Response)
                    {
                        using (StreamReader sr = new StreamReader(exResponse.GetResponseStream()))
                        {
                            responseXml = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo("Xay ra loi. Data:" + postData
                    + Environment.NewLine + "urlpost:" + url
                    + "Ex:" + ex);
            }

            if (success)
            {
                resp = responseXml;
            }
            else
            {
                resp = responseXml;

            }

            return resp;
        }


        /// <summary>Send the Message to Paygate Checkout</summary>
        public static string SendGetRequest(string ur)
        {
            bool success = false;
            string resp;


            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(ur);
            myRequest.Method = "GET";
            myRequest.Timeout = 600000;
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.KeepAlive = false;

            string responseXml = string.Empty;
            try
            {
                using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
                {
                    if (myResponse.StatusCode != HttpStatusCode.OK)
                        success = false;
                    else
                        success = true;
                    using (Stream respStream = myResponse.GetResponseStream())
                    {
                        using (StreamReader respReader = new StreamReader(respStream))
                        {
                            responseXml = respReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                if (webEx.Response != null)
                {
                    using (HttpWebResponse exResponse = (HttpWebResponse)webEx.Response)
                    {
                        using (StreamReader sr = new StreamReader(exResponse.GetResponseStream()))
                        {
                            responseXml = sr.ReadToEnd();
                        }
                    }
                }
            }



            if (success)
            {
                resp = responseXml;
            }
            else
            {
                resp = responseXml;

            }

            return resp;
        }

        

        public static string GetHttpResponseYH(string url, string accessToken)
        {
            string contactResponse = "";
            try
            {
                HttpWebRequest myRequest = WebRequest.Create(url) as HttpWebRequest;
                
                myRequest.Method = "GET";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                //myRequest.ContentType = "text/xml; encoding='utf-8'";
                myRequest.Headers["Authorization"] = "Bearer " + accessToken;
                using (HttpWebResponse response = myRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    contactResponse = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                // Graph API Errors or general web exceptions 

                contactResponse = ex.Message;

            }

            return contactResponse;
            
        }


        public static string GetClientIP()
        {
            string IP = string.Empty;

            try
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                if (IP == "")
                {
                    IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            catch
            {
            }

            return IP;
        }


        public static bool PostResultToMerchant(byte[] postData, string postUrl)
        {
            bool success = false;
            try
            {
                CookieContainer cookie = new CookieContainer();
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };
                System.Net.ServicePointManager.Expect100Continue = false;
                // Gan tap hop cac security protocal se ho tro ( ssl3, tsl, tsl11, tsl 12) . Toan tu | tra ra mot enum 
                // Neu khong gan mac dinh se chi co SSL3, TSL (voi framswork 4.5)
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(postUrl);
                myRequest.Method = "POST";
                myRequest.ContentLength = postData.Length;
                myRequest.CookieContainer = cookie;
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.KeepAlive = false;

                using (Stream requestStream = myRequest.GetRequestStream())
                {
                    requestStream.Write(postData, 0, postData.Length);
                }

                string responseXml = string.Empty;
                using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
                {
                    if (myResponse.StatusCode != HttpStatusCode.OK)
                    {
                        success = false;
                    }
                    else
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                NLogLogger.Info(string.Format("postUrl:{0},postToMerchant:{1}", postUrl, System.Text.Encoding.ASCII.GetString(postData)));
            }
            return success;
        }



        /// <summary>
        /// Mã hóa normal text về HTML Text
        /// </summary>
        /// <param name="unicodeText">Xâu cần mã hóa</param>
        /// <returns>Xâu mã hóa</returns>
        public static string HtmlEncode(string unicodeText)
        {
            string encoded = String.Empty;
            if (unicodeText == null) { return string.Empty; }
            foreach (char c in unicodeText)
            {
                switch (c)
                {
                    case '&':
                        encoded += "&amp;";
                        break;
                    case '<':
                        encoded += "&lt;";
                        break;
                    case '>':
                        encoded += "&gt;";
                        break;
                    case '"':
                        encoded += "&quot;";
                        break;
                    default:
                        encoded += c;
                        break;
                }
            }
            encoded = encoded.Replace("\r\n", "<br/>");
            encoded = encoded.Replace("\n", "<br/>");
            return encoded;
        }

        public static string CreateSignRSA(string data, string privateKey)
        {
            System.Security.Cryptography.RSACryptoServiceProvider.UseMachineKeyStore = true;
            RSACryptoServiceProvider rsaCryptoIPT = new RSACryptoServiceProvider(1024);
            rsaCryptoIPT.FromXmlString(privateKey);
            return Convert.ToBase64String(rsaCryptoIPT.SignData(new ASCIIEncoding().GetBytes(data), new SHA1CryptoServiceProvider()));
        }

        public static bool CheckSignRSA(string data, string sign, string publicKey)
        {
            System.Security.Cryptography.RSACryptoServiceProvider.UseMachineKeyStore = true;
            RSACryptoServiceProvider rsacp = new RSACryptoServiceProvider();
            rsacp.FromXmlString(publicKey);
            return rsacp.VerifyData(Encoding.UTF8.GetBytes(data), "SHA1", Convert.FromBase64String(sign));
        }

        /// <summary>
        /// Hàm đọc nội dung File
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            System.IO.StreamReader streamReader = null;
            try
            {
                streamReader = new System.IO.StreamReader(filePath);
                return streamReader.ReadToEnd();
            }
            finally
            {
                streamReader.Close();
            }
        }


        /// <summary>
        /// Giải mã text HTML --> Normal Text
        /// </summary>
        /// <param name="encodedText">Xâu cần giải mã</param>
        /// <returns>Xâu mã hóa</returns>
        public static string HtmlDecode(string encodedText)
        {
            if (encodedText == null) { return string.Empty; }
            encodedText = encodedText.Replace("<br/>", "\r\n");
            return entityResolver.Replace(encodedText, new MatchEvaluator(ResolveEntity));
        }


        /// <summary>
        /// Static Regular Expression to match Html Entities in encoded text
        /// </summary>
        private static Regex entityResolver =
            new Regex(@"([&][#](?'unicode'\d+);)|([&](?'html'\w+);)");


        /// <summary>
        /// List of entities from here
        /// http://www.vigay.com/inet/acorn/browse-html2.html#entities
        /// </summary>
        private static string[,] entityLookupArray = {
	        {"aacute", Convert.ToChar(0x00C1).ToString() }, {"aacute", Convert.ToChar(0x00E1).ToString() }, {"acirc", Convert.ToChar(0x00E2).ToString() }, {"acirc", Convert.ToChar(0x00C2).ToString() }, {"acute", Convert.ToChar(0x00B4).ToString() }, {"aelig", Convert.ToChar(0x00C6).ToString() }, {"aelig", Convert.ToChar(0x00E6).ToString() },
	        {"agrave", Convert.ToChar(0x00C0).ToString() }, {"agrave", Convert.ToChar(0x00E0).ToString() }, {"alefsym", Convert.ToChar(0x2135).ToString() }, {"alpha", Convert.ToChar(0x0391).ToString() }, {"alpha", Convert.ToChar(0x03B1).ToString() }, {"amp", Convert.ToChar(0x0026).ToString() }, {"and", Convert.ToChar(0x2227).ToString() },
	        {"ang", Convert.ToChar(0x2220).ToString() }, {"aring", Convert.ToChar(0x00E5).ToString() }, {"aring", Convert.ToChar(0x00C5).ToString() }, {"asymp", Convert.ToChar(0x2248).ToString() }, {"atilde", Convert.ToChar(0x00C3).ToString() }, {"atilde", Convert.ToChar(0x00E3).ToString() }, {"auml", Convert.ToChar(0x00E4).ToString() },
	        {"auml", Convert.ToChar(0x00C4).ToString() }, {"bdquo", Convert.ToChar(0x201E).ToString() }, {"beta", Convert.ToChar(0x0392).ToString() }, {"beta", Convert.ToChar(0x03B2).ToString() }, {"brvbar", Convert.ToChar(0x00A6).ToString() }, {"bull", Convert.ToChar(0x2022).ToString() }, {"cap", Convert.ToChar(0x2229).ToString() }, {"ccedil", Convert.ToChar(0x00C7).ToString() },
	        {"ccedil", Convert.ToChar(0x00E7).ToString() }, {"cedil", Convert.ToChar(0x00B8).ToString() }, {"cent", Convert.ToChar(0x00A2).ToString() }, {"chi", Convert.ToChar(0x03C7).ToString() }, {"chi", Convert.ToChar(0x03A7).ToString() }, {"circ", Convert.ToChar(0x02C6).ToString() }, {"clubs", Convert.ToChar(0x2663).ToString() }, {"cong", Convert.ToChar(0x2245).ToString() },
	        {"copy", Convert.ToChar(0x00A9).ToString() }, {"crarr", Convert.ToChar(0x21B5).ToString() }, {"cup", Convert.ToChar(0x222A).ToString() }, {"curren", Convert.ToChar(0x00A4).ToString() }, {"dagger", Convert.ToChar(0x2020).ToString() }, {"dagger", Convert.ToChar(0x2021).ToString() }, {"darr", Convert.ToChar(0x2193).ToString() }, {"darr", Convert.ToChar(0x21D3).ToString() },
	        {"deg", Convert.ToChar(0x00B0).ToString() }, {"delta", Convert.ToChar(0x0394).ToString() }, {"delta", Convert.ToChar(0x03B4).ToString() }, {"diams", Convert.ToChar(0x2666).ToString() }, {"divide", Convert.ToChar(0x00F7).ToString() }, {"eacute", Convert.ToChar(0x00E9).ToString() }, {"eacute", Convert.ToChar(0x00C9).ToString() }, {"ecirc", Convert.ToChar(0x00CA).ToString() },
	        {"ecirc", Convert.ToChar(0x00EA).ToString() }, {"egrave", Convert.ToChar(0x00C8).ToString() }, {"egrave", Convert.ToChar(0x00E8).ToString() }, {"empty", Convert.ToChar(0x2205).ToString() }, {"emsp", Convert.ToChar(0x2003).ToString() }, {"ensp", Convert.ToChar(0x2002).ToString() }, {"epsilon", Convert.ToChar(0x03B5).ToString() }, {"epsilon", Convert.ToChar(0x0395).ToString() },
	        {"equiv", Convert.ToChar(0x2261).ToString() }, {"eta", Convert.ToChar(0x0397).ToString() }, {"eta", Convert.ToChar(0x03B7).ToString() }, {"eth", Convert.ToChar(0x00F0).ToString() }, {"eth", Convert.ToChar(0x00D0).ToString() }, {"euml", Convert.ToChar(0x00CB).ToString() }, {"euml", Convert.ToChar(0x00EB).ToString() }, {"euro", Convert.ToChar(0x20AC).ToString() }, {"exist", Convert.ToChar(0x2203).ToString() },
	        {"fnof", Convert.ToChar(0x0192).ToString() }, {"forall", Convert.ToChar(0x2200).ToString() }, {"frac12", Convert.ToChar(0x00BD).ToString() }, {"frac14", Convert.ToChar(0x00BC).ToString() }, {"frac34", Convert.ToChar(0x00BE).ToString() }, {"frasl", Convert.ToChar(0x2044).ToString() }, {"gamma", Convert.ToChar(0x03B3).ToString() }, {"gamma", Convert.ToChar(0x393).ToString() },
	        {"ge", Convert.ToChar(0x2265).ToString() }, {"gt", Convert.ToChar(0x003E).ToString() }, {"harr", Convert.ToChar(0x21D4).ToString() }, {"harr", Convert.ToChar(0x2194).ToString() }, {"hearts", Convert.ToChar(0x2665).ToString() }, {"hellip", Convert.ToChar(0x2026).ToString() }, {"iacute", Convert.ToChar(0x00CD).ToString() }, {"iacute", Convert.ToChar(0x00ED).ToString() }, {"icirc", Convert.ToChar(0x00EE).ToString() },
	        {"icirc", Convert.ToChar(0x00CE).ToString() }, {"iexcl", Convert.ToChar(0x00A1).ToString() }, {"igrave", Convert.ToChar(0x00CC).ToString() }, {"igrave", Convert.ToChar(0x00EC).ToString() }, {"image", Convert.ToChar(0x2111).ToString() }, {"infin", Convert.ToChar(0x221E).ToString() }, {"int", Convert.ToChar(0x222B).ToString() }, {"iota", Convert.ToChar(0x0399).ToString() },
	        {"iota", Convert.ToChar(0x03B9).ToString() }, {"iquest", Convert.ToChar(0x00BF).ToString() }, {"isin", Convert.ToChar(0x2208).ToString() }, {"iuml", Convert.ToChar(0x00EF).ToString() }, {"iuml", Convert.ToChar(0x00CF).ToString() }, {"kappa", Convert.ToChar(0x03BA).ToString() }, {"kappa", Convert.ToChar(0x039A).ToString() }, {"lambda", Convert.ToChar(0x039B).ToString() },
	        {"lambda", Convert.ToChar(0x03BB).ToString() }, {"lang", Convert.ToChar(0x2329).ToString() }, {"laquo", Convert.ToChar(0x00AB).ToString() }, {"larr", Convert.ToChar(0x2190).ToString() }, {"larr", Convert.ToChar(0x21D0).ToString() }, {"lceil", Convert.ToChar(0x2308).ToString() }, {"ldquo", Convert.ToChar(0x201C).ToString() }, {"le", Convert.ToChar(0x2264).ToString() },
	        {"lfloor", Convert.ToChar(0x230A).ToString() }, {"lowast", Convert.ToChar(0x2217).ToString() }, {"loz", Convert.ToChar(0x25CA).ToString() }, {"lrm", Convert.ToChar(0x200E).ToString() }, {"lsaquo", Convert.ToChar(0x2039).ToString() }, {"lsquo", Convert.ToChar(0x2018).ToString() }, {"lt", Convert.ToChar(0x003C).ToString() }, {"macr", Convert.ToChar(0x00AF).ToString() },
	        {"mdash", Convert.ToChar(0x2014).ToString() }, {"micro", Convert.ToChar(0x00B5).ToString() }, {"middot", Convert.ToChar(0x00B7).ToString() }, {"minus", Convert.ToChar(0x2212).ToString() }, {"mu", Convert.ToChar(0x039C).ToString() }, {"mu", Convert.ToChar(0x03BC).ToString() }, {"nabla", Convert.ToChar(0x2207).ToString() }, {"nbsp", Convert.ToChar(0x00A0).ToString() },
	        {"ndash", Convert.ToChar(0x2013).ToString() }, {"ne", Convert.ToChar(0x2260).ToString() }, {"ni", Convert.ToChar(0x220B).ToString() }, {"not", Convert.ToChar(0x00AC).ToString() }, {"notin", Convert.ToChar(0x2209).ToString() }, {"nsub", Convert.ToChar(0x2284).ToString() }, {"ntilde", Convert.ToChar(0x00F1).ToString() }, {"ntilde", Convert.ToChar(0x00D1).ToString() }, {"nu", Convert.ToChar(0x039D).ToString() },
	        {"nu", Convert.ToChar(0x03BD).ToString() }, {"oacute", Convert.ToChar(0x00F3).ToString() }, {"oacute", Convert.ToChar(0x00D3).ToString() }, {"ocirc", Convert.ToChar(0x00D4).ToString() }, {"ocirc", Convert.ToChar(0x00F4).ToString() }, {"oelig", Convert.ToChar(0x0152).ToString() }, {"oelig", Convert.ToChar(0x0153).ToString() }, {"ograve", Convert.ToChar(0x00F2).ToString() },
	        {"ograve", Convert.ToChar(0x00D2).ToString() }, {"oline", Convert.ToChar(0x203E).ToString() }, {"omega", Convert.ToChar(0x03A9).ToString() }, {"omega", Convert.ToChar(0x03C9).ToString() }, {"omicron", Convert.ToChar(0x039F).ToString() }, {"omicron", Convert.ToChar(0x03BF).ToString() }, {"oplus", Convert.ToChar(0x2295).ToString() }, {"or", Convert.ToChar(0x2228).ToString() },
	        {"ordf", Convert.ToChar(0x00AA).ToString() }, {"ordm", Convert.ToChar(0x00BA).ToString() }, {"oslash", Convert.ToChar(0x00D8).ToString() }, {"oslash", Convert.ToChar(0x00F8).ToString() }, {"otilde", Convert.ToChar(0x00F5).ToString() }, {"otilde", Convert.ToChar(0x00D5).ToString() }, {"otimes", Convert.ToChar(0x2297).ToString() }, {"ouml", Convert.ToChar(0x00D6).ToString() },
	        {"ouml", Convert.ToChar(0x00F6).ToString() }, {"para", Convert.ToChar(0x00B6).ToString() }, {"part", Convert.ToChar(0x2202).ToString() }, {"permil", Convert.ToChar(0x2030).ToString() }, {"perp", Convert.ToChar(0x22A5).ToString() }, {"phi", Convert.ToChar(0x03A6).ToString() }, {"phi", Convert.ToChar(0x03C6).ToString() }, {"pi", Convert.ToChar(0x03A0).ToString() },
	        {"pi", Convert.ToChar(0x03C0).ToString() }, {"piv", Convert.ToChar(0x03D6).ToString() }, {"plusmn", Convert.ToChar(0x00B1).ToString() }, {"pound", Convert.ToChar(0x00A3).ToString() }, {"prime", Convert.ToChar(0x2033).ToString() }, {"prime", Convert.ToChar(0x2032).ToString() }, {"prod", Convert.ToChar(0x220F).ToString() }, {"prop", Convert.ToChar(0x221D).ToString() },
	        {"psi", Convert.ToChar(0x03C8).ToString() }, {"psi", Convert.ToChar(0x03A8).ToString() }, {"quot", Convert.ToChar(0x0022).ToString() }, {"radic", Convert.ToChar(0x221A).ToString() }, {"rang", Convert.ToChar(0x232A).ToString() }, {"raquo", Convert.ToChar(0x00BB).ToString() }, {"rarr", Convert.ToChar(0x2192).ToString() }, {"rarr", Convert.ToChar(0x21D2).ToString() }, {"rceil", Convert.ToChar(0x2309).ToString() },
	        {"rdquo", Convert.ToChar(0x201D).ToString() }, {"real", Convert.ToChar(0x211C).ToString() }, {"reg", Convert.ToChar(0x00AE).ToString() }, {"rfloor", Convert.ToChar(0x230B).ToString() }, {"rho", Convert.ToChar(0x03C1).ToString() }, {"rho", Convert.ToChar(0x03A1).ToString() }, {"rlm", Convert.ToChar(0x200F).ToString() }, {"rsaquo", Convert.ToChar(0x203A).ToString() },
	        {"rsquo", Convert.ToChar(0x2019).ToString() }, {"sbquo", Convert.ToChar(0x201A).ToString() }, {"scaron", Convert.ToChar(0x0160).ToString() }, {"scaron", Convert.ToChar(0x0161).ToString() }, {"sdot", Convert.ToChar(0x22C5).ToString() }, {"sect", Convert.ToChar(0x00A7).ToString() }, {"shy", Convert.ToChar(0x00AD).ToString() }, {"sigma", Convert.ToChar(0x03C3).ToString() },
	        {"sigma", Convert.ToChar(0x03A3).ToString() }, {"sigmaf", Convert.ToChar(0x03C2).ToString() }, {"sim", Convert.ToChar(0x223C).ToString() }, {"spades", Convert.ToChar(0x2660).ToString() }, {"sub", Convert.ToChar(0x2282).ToString() }, {"sube", Convert.ToChar(0x2286).ToString() }, {"sum", Convert.ToChar(0x2211).ToString() }, {"sup", Convert.ToChar(0x2283).ToString() },
	        {"sup1", Convert.ToChar(0x00B9).ToString() }, {"sup2", Convert.ToChar(0x00B2).ToString() }, {"sup3", Convert.ToChar(0x00B3).ToString() }, {"supe", Convert.ToChar(0x2287).ToString() }, {"szlig", Convert.ToChar(0x00DF).ToString() }, {"tau", Convert.ToChar(0x03A4).ToString() }, {"tau", Convert.ToChar(0x03C4).ToString() }, {"there4", Convert.ToChar(0x2234).ToString() },
	        {"theta", Convert.ToChar(0x03B8).ToString() }, {"theta", Convert.ToChar(0x0398).ToString() }, {"thetasym", Convert.ToChar(0x03D1).ToString() }, {"thinsp", Convert.ToChar(0x2009).ToString() }, {"thorn", Convert.ToChar(0x00FE).ToString() }, {"thorn", Convert.ToChar(0x00DE).ToString() }, {"tilde", Convert.ToChar(0x02DC).ToString() }, {"times", Convert.ToChar(0x00D7).ToString() },
	        {"trade", Convert.ToChar(0x2122).ToString() }, {"uacute", Convert.ToChar(0x00DA).ToString() }, {"uacute", Convert.ToChar(0x00FA).ToString() }, {"uarr", Convert.ToChar(0x2191).ToString() }, {"uarr", Convert.ToChar(0x21D1).ToString() }, {"ucirc", Convert.ToChar(0x00DB).ToString() }, {"ucirc", Convert.ToChar(0x00FB).ToString() }, {"ugrave", Convert.ToChar(0x00D9).ToString() },
	        {"ugrave", Convert.ToChar(0x00F9).ToString() }, {"uml", Convert.ToChar(0x00A8).ToString() }, {"upsih", Convert.ToChar(0x03D2).ToString() }, {"upsilon", Convert.ToChar(0x03A5).ToString() }, {"upsilon", Convert.ToChar(0x03C5).ToString() }, {"uuml", Convert.ToChar(0x00DC).ToString() }, {"uuml", Convert.ToChar(0x00FC).ToString() }, {"weierp", Convert.ToChar(0x2118).ToString() },
	        {"xi", Convert.ToChar(0x039E).ToString() }, {"xi", Convert.ToChar(0x03BE).ToString() }, {"yacute", Convert.ToChar(0x00FD).ToString() }, {"yacute", Convert.ToChar(0x00DD).ToString() }, {"yen", Convert.ToChar(0x00A5).ToString() }, {"yuml", Convert.ToChar(0x0178).ToString() }, {"yuml", Convert.ToChar(0x00FF).ToString() }, {"zeta", Convert.ToChar(0x03B6).ToString() }, {"zeta", Convert.ToChar(0x0396).ToString() },
	        {"zwj", Convert.ToChar(0x200D).ToString() }, {"zwnj", Convert.ToChar(0x200C).ToString()}
                                              };



        private static string ResolveEntity(System.Text.RegularExpressions.Match matchToProcess)
        {

            // ## HARDCODED ##

            string x = ""; // default 'char placeholder' if cannot be resolved - shouldn't occur
            if (matchToProcess.Groups["unicode"].Success)
            {
                x = Convert.ToChar(Convert.ToInt32(matchToProcess.Groups["unicode"].Value)).ToString();
            }
            else
            {
                if (matchToProcess.Groups["html"].Success)
                {
                    string entity = matchToProcess.Groups["html"].Value.ToLower();
                    switch (entity)
                    {
                        case "amp":
                            x = "&";
                            break;
                        case "lt":
                            x = "<";
                            break;
                        case "gt":
                            x = ">";
                            break;
                        case "quot":
                            x = "\"";
                            break;
                        default:
                            x = entity;
                            break;
                    }
                }
            }
            return x;
        }

        
    }

    public class NVPCodec : NameValueCollection
    {
        private const string AMPERSAND = "&";
        private const string EQUALS = "=";
        private static readonly char[] AMPERSAND_CHAR_ARRAY = AMPERSAND.ToCharArray();
        private static readonly char[] EQUALS_CHAR_ARRAY = EQUALS.ToCharArray();

        /// <summary>
        /// Returns the built NVP string of all name/value pairs in the Hashtable
        ///             NVPCodec decoder = new NVPCodec();
        ///        decoder.Decode(pStresponsenvp);
        /// </summary>
        /// <returns></returns>
        public string Encode()
        {
            StringBuilder sb = new StringBuilder();
            bool firstPair = true;
            foreach (string kv in AllKeys)
            {
                string name = UrlEncode(kv);
                string value = UrlEncode(this[kv]);
                if (!firstPair)
                {
                    sb.Append(AMPERSAND);
                }
                sb.Append(name).Append(EQUALS).Append(value);
                firstPair = false;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Decoding the string
        /// </summary>
        /// <param name="nvpstring"></param>
        public void Decode(string nvpstring)
        {
            Clear();
            foreach (string nvp in nvpstring.Split(AMPERSAND_CHAR_ARRAY))
            {
                string[] tokens = nvp.Split(EQUALS_CHAR_ARRAY);
                if (tokens.Length >= 2)
                {
                    string name = UrlDecode(tokens[0]);
                    string value = UrlDecode(tokens[1]);
                    Add(name, value);
                }
            }
        }

        private static string UrlDecode(string s) { return HttpUtility.UrlDecode(s); }
        private static string UrlEncode(string s) { return HttpUtility.UrlEncode(s.Trim()); }

        #region Array methods
        public void Add(string name, string value, int index)
        {
            this.Add(GetArrayName(index, name), value);
        }

        public void Remove(string arrayName, int index)
        {
            this.Remove(GetArrayName(index, arrayName));
        }

        /// <summary>
        /// 
        /// </summary>
        public string this[string name, int index]
        {
            get
            {
                return this[GetArrayName(index, name)];
            }
            set
            {
                this[GetArrayName(index, name)] = value;
            }
        }

        private static string GetArrayName(int index, string name)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "index can not be negative : " + index);
            }
            return name + index;
        }
        #endregion
    }

}