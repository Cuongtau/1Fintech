using System.Configuration;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PayWallet.PortalGateway.Controllers.Utils;
using PayWallet.PortalGateway.Models;
using PayWallet.Utils;
using PayWallet.Utils.Security;

namespace PayWallet.PortalGateway.Controllers
{
    public class CommonController : Controller
    {
        //
        // type 1; Success, type 2: Fail
        [HttpGet, ValidateInput(false)]
        public ActionResult PopupView(string content, string btnText, int type, string executeFunction="")
        {
            LanguageHelper.GetCurrentLanguage();
            content = HttpUtility.HtmlDecode(content);
            executeFunction = executeFunction ?? string.Empty;
            executeFunction = HttpUtility.HtmlDecode(executeFunction);
            ViewBag.Description = content;
            ViewBag.BtnText = btnText;
            ViewBag.ExcuteFunction = executeFunction;
            ViewBag.type = type;
            return PartialView();
        }

        [HttpGet, ValidateInput(false)] 
        public ActionResult PopupViewCommon(string content)
        {
            content = HttpUtility.HtmlDecode(content);
            ViewBag.Description = content;
            LanguageHelper.GetCurrentLanguage();
            ViewBag.btntext = Resources.Response.Message.BtnClose;
            return PartialView();
        }

        //public ActionResult SearchBank(string extendData)
        //{
        //    LanguageHelper.GetCurrentLanguage();
        //    List<BankInfo> ListBankSearch = new List<BankInfo>();
   
        //    var secretKey = ConfigurationManager.AppSettings["SecretKey_TransactionRequest"];
        //    var decode64Data = Security.Base64Decode(extendData);
        //    var orderJsEncrypt = Encrypt.DecryptUTF8(secretKey, decode64Data);
        //    NLogLogger.LogInfo("SearchBank >> orderData:" + orderJsEncrypt);
        //    var ObjectExtend = new JavaScriptSerializer().Deserialize<ObjectExtend>(orderJsEncrypt);
        //    var orderInfo = new BussinessGate().GetDataOrder(ObjectExtend.queryUrl);
        //    var ListBank = new List<BankInfo>();
        //    var ListCard = new List<BankInfo>();
        //    bool IsAllowPayCard = false;
        //    var getIntegrationCheck = new TransactionOutput_PaymentOnlineServiceClient().GetConfigPaymentIntegrationCheck(orderInfo.merchantType.ToUpper(), orderInfo.website_id);
        //    if (getIntegrationCheck != null && getIntegrationCheck.ID > 0)
        //    {
        //        IsAllowPayCard = getIntegrationCheck.VisaMasterIsEnable ? true : false;
        //    }

        //    if (IsAllowPayCard && (string.IsNullOrEmpty(orderInfo.payType) || orderInfo.payType.ToUpper() == BankType.InternationalCard))
        //        ListCard = new BussinessGate().LoadBankList(2);

        //    if (string.IsNullOrEmpty(orderInfo.payType) || orderInfo.payType.ToLower() == BankType.DomesticBank)
        //        ListBank = new BussinessGate().LoadBankList(1);
                
        //    if (ListBank != null && ListBank.Count > 0)
        //    {
        //        ListBankSearch.AddRange(ListBank);
        //    }
        //    if (ListCard != null && ListCard.Count > 0)
        //    {
        //        ListBankSearch.AddRange(ListCard);
        //    }
        //    ViewBag.extendData = extendData;
        //    return PartialView(ListBankSearch);
        //}

        //public ActionResult OtherPaymentMethods(int Type, ObjectExtend ObjectExtend)
        //{
        //    LanguageHelper.GetCurrentLanguage();
        //    var orderinfo = new BussinessGate().GetDataOrder(ObjectExtend.queryUrl);
        //    var UrlRequest = ObjectExtend.queryUrl;
        //    bool IsAllowPayCard = false;
        //    var getIntegrationCheck = new TransactionOutput_PaymentOnlineServiceClient().GetConfigPaymentIntegrationCheck(orderinfo.merchantType.ToUpper(), orderinfo.website_id);
        //    if (getIntegrationCheck != null && getIntegrationCheck.ID > 0)
        //    {
        //        IsAllowPayCard = getIntegrationCheck.VisaMasterIsEnable ? true : false;
        //    }

        //    var ListCard = new List<BankInfo>();
        //    if (IsAllowPayCard && (string.IsNullOrEmpty(orderinfo.payType) || orderinfo.payType.ToUpper() == BankType.InternationalCard))
        //        ListCard = new BussinessGate().LoadBankList(2);
        //    Type = Type == 2 ? Type : 1;
        //    ViewBag.Type = Type;
        //    ViewBag.UrlRequest = UrlRequest;
        //    ViewBag.WebsiteID = orderinfo.website_id;
        //    return PartialView(ListCard);
        //}
        public ActionResult PopupInputMoneySupport(string orderData)
        {
            LanguageHelper.GetCurrentLanguage();
            var secretKey = ConfigurationManager.AppSettings["SecretKey_TransactionRequest"];
            var decode64Data = Security.Base64Decode(orderData);
            var orderJsEncrypt = Encrypt.DecryptUTF8(secretKey, decode64Data);
            NLogLogger.LogInfo("SearchBank >> orderData:" + orderJsEncrypt);
            var ObjectExtend = new JavaScriptSerializer().Deserialize<ObjectExtend>(orderJsEncrypt);

            var orderInfo = new BussinessGate().GetDataOrder(ObjectExtend.queryUrl);
            ViewBag.ExtendData = orderData;
            return PartialView(orderInfo);
        }
        public static string TokenHeaderValue()
        {
            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return cookieToken + ":" + formToken;
        }

       
    }
}