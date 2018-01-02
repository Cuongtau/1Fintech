using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;



/// <summary>
/// Summary description for PostGetHelper
/// </summary>
namespace PayWallet.Utils
{
    public class PostGetHelper
    {
        private HttpResponse Response;
        private HttpRequest Request;
        private NameValueCollection values = new NameValueCollection();

        public string FormName
        {
            get
            {
                return Get("__TransferData");
            }
            set
            {
                Add("__TransferData", value);
            }
        }

        public PostGetHelper(HttpRequest request, HttpResponse response)
        {
            Request = request;
            Response = response;
            FormName = "DefaultForm";
        }

        public void Add(string key, string value)
        {
            if (key != null && value != null)
            {
                values.Set(key, value);
            }
        }

        public string Get(string key)
        {
            return values[key];
        }

        public void Clear()
        {
            values.Clear();
        }

        public void ReadPostedData()
        {
            if (Request.Form != null && this.Request.Form["__TransferData"] != null)
            {
                foreach (string key in Request.Form.Keys)
                {
                    Add(key, HttpUtility.HtmlDecode(Request.Form[key]));
                }
            }
        }

              
        
        // Dang dung voi Cyber - Sacom
        public void PostAndRedirectWithData(string urlMerchantPost)
        {
            string contentPost = string.Empty;
            StringBuilder htmlPost = new StringBuilder();

            htmlPost.Append("<html><head>");
            htmlPost.Append("</head><body onload=\"document.FormTransfer.submit()\">");
            htmlPost.Append(string.Format("<form name=\"FormTransfer\" id=\"FormTransfer\" method=\"post\" action=\"{0}\" >", urlMerchantPost));

            foreach (string name in values.AllKeys)
            {
                htmlPost.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\"> ", name, HttpUtility.HtmlEncode(values[name])));
            }

            htmlPost.Append("</form></body></html>");

            contentPost = htmlPost.ToString();
            HttpContext.Current.Response.Write(contentPost);
                       
            HttpContext.Current.Response.End();
        }


    }
}