using System;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;

namespace PayWallet.Utils
{
    public static class WebPost
    {
        /// <summary>
        /// Check Validate khi xác minh website tích hợp trong chức năng tích hợp website Paygate 3 - Comment by: NhatND 2011/11/25
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static public bool CheckValidUrl(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "GET";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TURE if the Status code == 200
                return (response.StatusCode != HttpStatusCode.RequestTimeout &&
                   response.StatusCode != HttpStatusCode.BadGateway &&
                   response.StatusCode != HttpStatusCode.GatewayTimeout);
            }
            catch (WebException webEx)
            {
                if (webEx.Response != null)
                {
                    using (HttpWebResponse exResponse = (HttpWebResponse)webEx.Response)
                    {
                        return (exResponse.StatusCode != HttpStatusCode.RequestTimeout &&
                          exResponse.StatusCode != HttpStatusCode.BadGateway &&
                          exResponse.StatusCode != HttpStatusCode.GatewayTimeout);
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                NLogLogger.Info(url + "\t" + ex);
                return false;
            }
        }

        // Post ket qua ve cho doi tac tich hop cong
        static public bool PostToMerchant(string data, string sign, string url)
        {
            bool SendResult = false;
            try
            {
                string contents = "data=" + HttpUtility.UrlEncode(data);
                contents += "&sign=" + HttpUtility.UrlEncode(sign);
                byte[] contentPost = System.Text.Encoding.UTF8.GetBytes(contents);

                SendResult = SendNotifyStateChangeOrderNew(url, contentPost);

            }
            catch (Exception ex)
            {
                NLogLogger.Info("Error post.Data: " + data + "|sign: " + sign
                 + Environment.NewLine + "url: " + url
                 + Environment.NewLine + "Exception detail: " + ex);
            }

            if (!SendResult)
            {
                NLogLogger.Info("Cannot post. Data: " + data + "|sign: " + sign
                    + Environment.NewLine + "url: " + url);
            }
            return SendResult;
        }

        // Post ket qua ve cho doi tac tich hop cong
        static public bool PostToMerchantV2(string data, string sign, string url, ref string contents)
        {
            contents = string.Empty;
            bool SendResult = false;

            try
            {
                contents = "data=" + HttpUtility.UrlEncode(data);
                contents += "&signature=" + HttpUtility.UrlEncode(sign);
                byte[] contentPost = System.Text.Encoding.UTF8.GetBytes(contents);

                SendResult = SendNotifyStateChangeOrderNew(url, contentPost);
            }
            catch (Exception ex)
            {
                NLogLogger.Info("Error post.Data: " + data + "|sign: " + sign
                 + Environment.NewLine + "url: " + url
                 + Environment.NewLine + "Exception detail: " + ex);
            }

            if (!SendResult)
            {
                NLogLogger.Info("Cannot post. Data: " + data + "|sign: " + sign
                    + Environment.NewLine + "url: " + url);
            }
            return SendResult;
        }

        static public bool SendNotifyStateChangeOrder(string _NotifyURL, byte[] contentPost)
        {
            bool success = false;

            try
            {
                CookieContainer cookie = new CookieContainer();

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };
                System.Net.ServicePointManager.Expect100Continue = false;
                // Gan tap hop cac security protocal se ho tro ( ssl3, tsl, tsl11, tsl 12) . Toan tu | tra ra mot enum 
                // Neu khong gan mac dinh se chi co SSL3, TSL (voi framswork 4.5)
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(_NotifyURL);
                myRequest.Method = "POST";
                myRequest.ContentLength = contentPost.Length;
                myRequest.CookieContainer = cookie;
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.KeepAlive = false;

                using (Stream requestStream = myRequest.GetRequestStream())
                {
                    requestStream.Write(contentPost, 0, contentPost.Length);
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
                NLogLogger.Info(string.Format("UrpPost: {0}, DataPost:{1}", _NotifyURL, System.Text.Encoding.ASCII.GetString(contentPost))
                    + Environment.NewLine + "Exception:" + ex);
            }
            return success;
        }
        static public bool SendNotifyStateChangeOrderNew(string _NotifyURL, byte[] bytes)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };
                System.Net.ServicePointManager.Expect100Continue = false;
                // Gan tap hop cac security protocal se ho tro ( ssl3, tsl, tsl11, tsl 12) . Toan tu | tra ra mot enum 
                // Neu khong gan mac dinh se chi co SSL3, TSL (voi framswork 4.5)
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_NotifyURL);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo("Excetpion post. Url: " + _NotifyURL
                    + Environment.NewLine + ex);
            }

            return false;
        }
        /// <summary>Send the Message to PalPal Checkout</summary>
        public static string SendPost(string postData, string url)
        {
            string resp;
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] data = encoding.GetBytes(postData);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };
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
            resp = responseXml;
            return resp;
        }

        public static string GetData(string url, string type = "json")
        {
            string response = string.Empty;
            try
            {
                System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "GET";
                //myRequest.ContentLength = data.Length;
                var cookieJar = new CookieContainer();
                myRequest.CookieContainer = cookieJar;
                //myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentType = type == "json" ? "application/json; charset=UTF-8" : "text/xml; encoding='utf-8'";
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

        /// <summary>
        ///    Classed used to bypass self-signed server certificate
        /// </summary>
        /// <remarks>
        ///     To be used in development only.
        /// </remarks>
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

        public static string AddParam<T>(this T objectCheck)
        {
            var paramString = new System.Text.StringBuilder();
            foreach (PropertyInfo property in objectCheck.GetType().GetProperties())
            {
                var objectValue = property.GetValue(objectCheck, null);
                if (objectValue != null)
                {
                    if (paramString.Length > 0)
                        paramString.Append("&");
                    paramString.Append(property.Name);
                    paramString.Append("=");
                    paramString.Append(HttpUtility.UrlEncode(objectValue.ToString()));
                }
            }
            return paramString.ToString();
        }
    }


    public class ParamBuilder
    {
        System.Text.StringBuilder paramString;

        public ParamBuilder()
        {
            paramString = new System.Text.StringBuilder();
        }
       
        public void AddParam(string name, string value)
        {
            if (paramString.Length > 0)
                paramString.Append("&");

            paramString.Append(name);
            paramString.Append("=");
            paramString.Append(HttpUtility.UrlEncode(value));
        }

        public string GetParamString()
        {
            return paramString.ToString();
        }

        public string BuildRequestData(NameValueCollection paramCollection)
        {
            foreach (string name in paramCollection.AllKeys)
            {
                this.AddParam(name, paramCollection[name]);
            }
            return this.GetParamString();
        }
    }
}
