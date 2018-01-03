using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace UtilLibraries.Http
{
    public static class HttpUtils
    {
        public static HttpResponseMessage CreateResponse(HttpStatusCode statusCode, string content, string reason)
        {
            return new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = new StringContent(content),
                ReasonPhrase = reason
            };
        }

        public static HttpResponseMessage PostProxy(object model, string api)
        {
            HttpClientHandler handler = new HttpClientHandler();
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsync(api, new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")).Result;
            return response;
        }

        public static HttpResponseMessage GetProxy(string api)
        {
            HttpClientHandler handler = new HttpClientHandler();

            var Client = new HttpClient(handler);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = Client.GetAsync(api).Result;
            return response;

        }


        public static string SendPost(string postData, string url)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] data = encoding.GetBytes(postData);
            System.Net.ServicePointManager.Expect100Continue = false;
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
            return responseXml;
        }

        /// <summary>Send the Message to Merchant</summary>
        public static string GetStringHttpResponse(string url)
        {
            string response = null;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "GET";
                //myRequest.ContentLength = data.Length;
                myRequest.CookieContainer = new CookieContainer();
                //myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentType = "text/xml; encoding='utf-8'";
                //myRequest.ContentType = "application/x-www-form-urlencoded";
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
                throw ex;
            }
            catch (Exception)
            {
            }

            return response;
        }

        /// <summary>Send the Message to Merchant</summary>
        public static object GetHttpResponse(string url)
        {
            object response = null;
            try
            {
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
            catch (WebException)
            {
            }
            catch (Exception)
            {
            }

            return response;
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
                    int firstIndexEQUALS = nvp.IndexOf(EQUALS);
                    if (firstIndexEQUALS > 0)
                    {
                        string name = UrlDecode(nvp.Substring(0, firstIndexEQUALS));
                        string value = UrlDecode(nvp.Substring(firstIndexEQUALS + 1));
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
}
