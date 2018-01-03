using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PayWallet.PortalGateway.Controllers.Utils;
using PayWallet.PortalGateway.Models;
using PayWallet.Utils;

namespace PayWallet.PortalGateway.Controllers
{
    public class HomeController : BaseController
    {
        string LinkPayment_Api = ConfigurationManager.AppSettings["UrlServerPayPortalAPI"] ?? string.Empty;
        string _paymentMethod = string.Empty;
        string _merchantLogo = "images/Pay365.png";
        const string _bank = "bank";
        const string _visa = "visa";
        const string _vtcpay = "vtcpay";
        string _paymentBy = string.Empty;
        string _sessionLogValue = string.Empty;
        public ActionResult Index()
        {
            var request = Request.Url.Query;
            var ModelPayInfo = new InfoPaymentModel
            {
                CheckRequest = new PaymentAPI_ResponseData { ResponseCode = -99, Message = Resources.Checkout.OrderInfoInvalid },
                OrderInfo = new MerchantRequestData(),
                MerchantLogo = _merchantLogo
            };

            return View(ModelPayInfo);
        }
        //Kiểm tra thông tin đơn hàng truyền sang
        public ActionResult CheckOutOrder()
        {

            var ModelPayInfo = new InfoPaymentModel
            {
                OrderInfo = new MerchantRequestData(),
                CheckRequest = new PaymentAPI_ResponseData(),
                MerchantLogo = _merchantLogo
            };
            NLogLogger.LogInfo("Request url: " + Request.Url.AbsoluteUri + Environment.NewLine + "Method: " + Request.HttpMethod + "|IP:" + Config.GetIP());
            var merchantID = Request.QueryString["merchant_id"] == null ? string.Empty : Request.QueryString["merchant_id"].ToString();
            var amount = Request.QueryString["amount"] == null ? string.Empty : Request.QueryString["amount"].ToString();
            
            var order_number = Request.QueryString["order_number"] == null ? string.Empty : Request.QueryString["order_number"].ToString();
            var trans_type = Request.QueryString["trans_type"] == null ? string.Empty : Request.QueryString["trans_type"].ToString();
            //var currency = Request.QueryString["currency"] == null ? string.Empty : Request.QueryString["currency"].ToString();
            var signature = Request.QueryString["signature"] == null ? string.Empty : Request.QueryString["signature"].ToString();
            var language = Request.QueryString["lang"] == null ? string.Empty : Request.QueryString["lang"].ToString();
            var url_return = Request.QueryString["url_return"] == null ? string.Empty : Request.QueryString["url_return"].ToString();
            var payment_type = Request.QueryString["payment_type"] == null ? string.Empty : Request.QueryString["payment_type"].ToString();
            
            var email = Request.QueryString["email"] == null ? string.Empty : Request.QueryString["email"].ToString();
            var phone = Request.QueryString["phone"] == null ? string.Empty : Request.QueryString["phone"].ToString();
            var description = Request.QueryString["description"] == null ? string.Empty : Request.QueryString["description"].ToString();
            var content_focus = Request.QueryString["content_focus"] == null ? string.Empty : Request.QueryString["content_focus"].ToString();
            var languageUrl = Request.QueryString["l"] == null ? string.Empty : Request.QueryString["l"].ToString();
            var lang = language.ToLower() == "en" ? "en" : "vn";

            if (string.IsNullOrEmpty(languageUrl))
            {
                LanguageHelper.SetLanguage(lang);
            }
            //var urlRedirect = Request.QueryString["urlRedirect"] == null ? string.Empty : Request.QueryString["urlRedirect"].ToString();
            if (string.IsNullOrEmpty(merchantID) || string.IsNullOrEmpty(amount)
                || string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(order_number))
            {
                //Tham số ko hợp lệ
                ModelPayInfo.CheckRequest.ResponseCode = -600;
                ModelPayInfo.CheckRequest.Message = Resources.Response.Message.RequestInvalid;
                return View("~/Views/home/index.cshtml", ModelPayInfo);
            }

            //currency = currency.ToLower() == "usd" ? currency.ToUpper() : "VND";
            int merchant_id = 0;
            bool isInt = int.TryParse(merchantID, out merchant_id);
            
            double amount_payment = 0;

            var ListCard = new List<BankInfo>();
            var ListBank = new List<BankInfo>();
            amount_payment = double.Parse(amount, System.Globalization.CultureInfo.InvariantCulture);
            if (!isInt || amount_payment <= 0)
            {
                //Request sai kiểu dữ liệu
                ModelPayInfo.CheckRequest.ResponseCode = -600;
                ModelPayInfo.CheckRequest.Message = Resources.Response.Message.RequestInvalid;
                return View("~/Views/home/index.cshtml", ModelPayInfo);
            }
            try
            {
                var orderInfo = new MerchantRequestData
                {
                    merchant_id = merchant_id,
                    amount = amount_payment,
                    payType = payment_type,
                    ordercode = order_number,
                    currency = "VND",
                    email = email,
                    phone = phone,
                    lang = language,
                    transType = trans_type,
                    signature = signature,
                    urlReturn = url_return,
                    description = description,
                    focus = content_focus
                };
                NLogLogger.LogInfo("Thông tin Order Request : " + new JavaScriptSerializer().Serialize(orderInfo));
                ModelPayInfo.OrderInfo = orderInfo;
                //Lưu thông tin order
                var indexParam = -1;
                var queryParam = Request.Url.Query;
                if (queryParam.Contains("&content_focus"))
                {
                    indexParam = queryParam.IndexOf("&content_focus");
                    queryParam = queryParam.Substring(0, indexParam);
                }
                else if (queryParam.Contains("&l="))
                {
                    indexParam = queryParam.IndexOf("&l=");
                    queryParam = queryParam.Substring(0, indexParam);
                }

                var merchantInfo = new BussinessGate().GetMerchantInfo(merchant_id.ToString());
                NLogLogger.LogInfo("merchantInfo:" + new JavaScriptSerializer().Serialize(merchantInfo));
                if (merchantInfo == null || merchantInfo.MerchantCode <= 0)
                {
                    NLogLogger.LogInfo(string.Format("{0}_{1}_Khong ton tai merchant tich hop", order_number, merchant_id));

                    ModelPayInfo.CheckRequest.ResponseCode = -8002;
                    ModelPayInfo.CheckRequest.Message = Resources.Checkout.WebsiteNotExits;
                    return View("~/Views/home/index.cshtml", ModelPayInfo);
                }
                if (!string.IsNullOrEmpty(merchantInfo.Logo))
                {
                    _merchantLogo = merchantInfo.Logo;
                }

                NLogLogger.LogInfo("checkRequestMerchant");
                ModelPayInfo.CheckRequest = new BussinessGate().CheckRequestMerchant(queryParam);
                ModelPayInfo.CheckRequest.RedirectUrl = ModelPayInfo.CheckRequest.RedirectUrl;
                ModelPayInfo.MerchantLogo = _merchantLogo;

                ModelPayInfo.ProviderName = merchantInfo.MerchantName;
                ModelPayInfo.QueryUrl = queryParam;
                if (ModelPayInfo.CheckRequest.ResponseCode >= 0)
                {
                    if (!string.IsNullOrEmpty(orderInfo.payType) && !new string[] { Constants.WALLET_PAYMENT_TYPE, BankType.InternationalCard, BankType.DomesticBank }.Contains(orderInfo.payType.ToUpper()))
                    {
                        //short bankType = 1; //NH nội địa
                        //var bankInternationalCard = Constants.International_BankCode; //master,visa , jcb, paypal
                        //var exists = bankInternationalCard.Contains(orderInfo.payType.ToUpper()) || (orderInfo.payType.ToUpper() == BankType.InternationalCard);
                        //if (exists)
                        //{
                        //    bankType = 2;
                        //}
                        var hdnBankIdNotRedirect = ConfigurationManager.AppSettings["BankIDVerifyCard"];
                        //neu la BIDV, OCB Thi khong tao GD ma sang trang paygateorder moi tao, de su ly truong hop back
                        string bankNotRedirect = hdnBankIdNotRedirect.Split(';').FirstOrDefault(b => b.StartsWith(orderInfo.payType.ToUpper()));
                        if (!string.IsNullOrEmpty(bankNotRedirect))
                        {
                            var objectExend = new ObjectExtend
                            {
                                content_focus = ModelPayInfo.OrderInfo.focus,
                                providerName = ModelPayInfo.ProviderName,
                                queryUrl = ModelPayInfo.QueryUrl
                            };
                            string urlRedirect = Config.Domain + "onlinebanking.html" + Request.Url.Query + "&bankCode=" + orderInfo.payType;
                            //Response.Redirect(urlRedirect);
                            return Redirect(urlRedirect);
                        }
                    }

                    long AmountVND = (long)amount_payment;

                    if (string.IsNullOrEmpty(orderInfo.payType) || orderInfo.payType.ToUpper() == BankType.DomesticBank)
                    {
                        var getListBank = new BussinessGate().GetBankPolicy(merchant_id.ToString(), AmountVND);
                        ListBank = getListBank.ResponseCode >= 0 && getListBank.ListBankPolicy.Count > 0 ? getListBank.ListBankPolicy : new List<BankInfo>();
                    }
                }
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.Message);
            }
            ViewBag.ListBank = ListBank;
            ViewBag.ListCard = ListCard;
            return View("~/Views/home/index.cshtml", ModelPayInfo);
        }
        

        // Thẻ ngân hàng nội địa (trường hợp không redirect sang ngân hàng)
        public ActionResult OnlineBanking()
        {
            var ListBank = new List<BankInfo>();
            var PaymentBank = new BankInfo();
            var orderInfo = new MerchantRequestData();
            var ObjectExtend = new ObjectExtend();
            var queryParam = Request.Url.Query;
            var indexParam = -1;
            
            if (string.IsNullOrEmpty(queryParam))
            {
                ViewBag.StatusView = -1;
                return View(PaymentBank);
            }
            if (queryParam.Contains("&content_focus"))
            {
                indexParam = queryParam.IndexOf("&content_focus");
                queryParam = queryParam.Substring(0, indexParam);
            }
            else if (queryParam.Contains("&l="))
            {
                indexParam = queryParam.IndexOf("&l=");
                queryParam = queryParam.Substring(0, indexParam);
            }
            var bankCode = Request.QueryString["bankCode"];
            if (string.IsNullOrEmpty(bankCode))
            {
                ViewBag.StatusView = -2;
                return View(PaymentBank);
            }
            try
            {
                ObjectExtend.queryUrl = queryParam;

                orderInfo = new BussinessGate().GetDataOrder(queryParam);

                var resultBank = new BussinessGate().GetBankPolicy(orderInfo.merchant_id.ToString(), (long)orderInfo.amount);
                if(resultBank.ResponseCode > 0 && resultBank.ListBankPolicy.Count > 0)
                {
                    PaymentBank = resultBank.ListBankPolicy.Find(pb => pb.BankCode.ToUpper() == bankCode.ToUpper());
                }
                if (!string.IsNullOrEmpty(orderInfo.payType) && orderInfo.payType.ToLower() != "domesticbank")
                    ListBank = new List<BankInfo>();
                else
                    ListBank = ListBank.FindAll(pb => pb.BankCode.ToUpper() != bankCode.ToUpper());


                var merchantInfo = new BussinessGate().GetMerchantInfo(orderInfo.merchant_id.ToString());
                NLogLogger.LogInfo("merchantInfo:" + new JavaScriptSerializer().Serialize(merchantInfo));
                if (merchantInfo != null && merchantInfo.MerchantCode > 0)
                {
                    _merchantLogo = merchantInfo.Logo;
                }
                ObjectExtend.providerName = merchantInfo.MerchantName;
                ViewBag.StatusView = 1;
            }
            catch (Exception ex)
            {
                ViewBag.StatusView = -1;
                NLogLogger.PublishException(ex);
            }
            TempData.Keep("objectExend");
            ViewBag.Logo = _merchantLogo;
            ViewBag.ListBank = ListBank;
            ViewBag.OrderInfo = orderInfo;
            ViewBag.ObjectExtend = ObjectExtend;
            return View(PaymentBank);
        }
    }
}