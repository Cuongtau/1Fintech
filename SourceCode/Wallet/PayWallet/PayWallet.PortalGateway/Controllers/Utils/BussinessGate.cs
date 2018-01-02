using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayWallet.Utils;
using System.Configuration;
using System.Web.Script.Serialization;
using PayWallet.PortalGateway.Models;
using PayWallet.Utils.Security;
namespace PayWallet.PortalGateway.Utils
{
    public class BussinessGate
    {
        string LinkPayment_Api = ConfigurationManager.AppSettings["UrlServerPayPortalAPI"] ?? string.Empty;
        
        public GetListBankPolicyResponseData GetBankPolicy(string MerchantCode, long Amount)
        {
            var response = new GetListBankPolicyResponseData();
            try
            {
                var keysign = ConfigurationManager.AppSettings["PaygateKeySign"];
                var sign = Security.Md5Encrypt($"{MerchantCode}|{Amount}|{keysign}");
                var urlreq = LinkPayment_Api + "GetBankPolicy?merchantCode=" + MerchantCode + "&amount=" + Amount + "&sign=" + sign;
                string returnPost = WebPost.GetData(urlreq, "json");
                NLogLogger.LogInfo("response:" + response);
                if (!string.IsNullOrEmpty(returnPost))
                    response = new JavaScriptSerializer().Deserialize<GetListBankPolicyResponseData>(returnPost);
                return response;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo("Exception >> LoadBankList :" + ex.Message);
                response.ResponseCode = -99;
                return response;
            }
        }
        public MerchantInfo GetMerchantInfo(string MerchantCode)
        {
            var merchantInfo = new MerchantInfo();
            try
            {
                var keysign = ConfigurationManager.AppSettings["PaygateKeySign"];
                var sign = Security.Md5Encrypt($"{MerchantCode}|{keysign}");
                var urlreq = LinkPayment_Api + "GetMerchantInfo?merchantCode=" + MerchantCode + "&sign=" + sign;
                string response = WebPost.GetData(urlreq, "json");
                NLogLogger.LogInfo("response:" + response);

                if (!string.IsNullOrEmpty(response))
                    merchantInfo = new JavaScriptSerializer().Deserialize<MerchantInfo>(response);
                return merchantInfo;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo("Exception >> LoadBankList :" + ex.Message);
                return merchantInfo;
            }
        }
        //xác thực tài khoản ngân hàng thực hiện giao dịch với các ngân hàng nội địa cho phép 
        public PaymentAPI_ResponseData PaymentByBank(VerifyCardRequestData requestData)
        {
            var response = new PaymentAPI_ResponseData
            {
                ResponseCode = -8999,
                Message = Resources.Response.Message.SystemBusy
            };
            try
            {
                var urlreq = LinkPayment_Api + "GetMerchantInfo";
                string dataPost = new JavaScriptSerializer().Serialize(requestData);
                string result = WebPost.SendPost(dataPost, urlreq);

                if (!string.IsNullOrEmpty(result))
                {
                    //response.ResponseCode = result.ResponseCode;
                    //response.OrderBillingId = result.OrderID;
                    //response.Message = result.Description;
                    //response.RedirectUrl = result.BankRedirectURL ?? string.Empty;
                    //response.PostData = result.BankPostData;

                    //if (result.ResponseCode < 0)
                    //{
                    //    var returnData = ReturnData.GetReturnData(result.ResponseCode, result.Description);
                    //    response.Message = returnData.Description;
                    //}
                }
                NLogLogger.LogInfo("requestData:" + new JavaScriptSerializer().Serialize(requestData) + "|response:" + new JavaScriptSerializer().Serialize(response));
                return response;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return response;
            }
        }

        //Trường hợp thanh toán tại cổng -> step 2 (sacombank)
        public PaymentAPI_ResponseData PaymentByBankConfirm(ConfirmBankDirectRequestData requestData)
        {
            var response = new PaymentAPI_ResponseData
            {
                ResponseCode = -8999,
                Message = Resources.Response.Message.SystemBusy
            };
            try
            {
                
                NLogLogger.LogInfo("requestData:" + new JavaScriptSerializer().Serialize(requestData) + "|response:" + new JavaScriptSerializer().Serialize(response));
                return response;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return response;
            }
        }


        public PaymentAPI_ResponseData CheckRequestMerchant(string RequestQuery)
        {
            var response = new PaymentAPI_ResponseData
            {
                ResponseCode = -8999,
                Message = Resources.Response.Message.SystemBusy
            };
            try
            {
                RequestQuery = RequestQuery.Replace("?", "");

                NLogLogger.LogInfo("CheckRequestMerchant > Request:" + RequestQuery);
                var urlreq = LinkPayment_Api + "CheckRequestPayment?listParamUrl=" + HttpUtility.UrlEncode(RequestQuery);
                string result = WebPost.SendPost(string.Empty, urlreq);
                NLogLogger.LogInfo("CheckRequestMerchant > Response:" + result);

                if (!string.IsNullOrEmpty(result))
                {
                    var data = new JavaScriptSerializer().Deserialize<ReturnData>(result);
                    response.ResponseCode = data.ResponseCode;
                    response.Message = data.Description;
                    response.RedirectUrl = data.Extend ?? string.Empty;
                    if (response.ResponseCode < 0)
                    {
                        var returnData = ReturnData.GetReturnData(data.ResponseCode, data.Extend);
                        response.Message = returnData.Description;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return response;
            }
        }
        

        public MerchantRequestData GetDataOrder(string query)
        {
            var result = new MerchantRequestData();
            if (string.IsNullOrEmpty(query))
                return result;

            query = query.Replace("?", "");
            NVPCodec paramQueryList = new NVPCodec();
            paramQueryList.Decode(query);
            if (paramQueryList == null && paramQueryList.Count <= 0)
            {
                return result;
            }
            result.merchant_id = 0;
            string amount = "";
            result.merchant_id = (object.Equals(paramQueryList["merchant_id"], null)) ? 0 : Convert.ToInt32(paramQueryList["merchant_id"]);
            amount = paramQueryList["amount"] == null ? string.Empty : paramQueryList["amount"].ToString();
            
            result.ordercode = paramQueryList["order_number"] == null ? string.Empty : paramQueryList["order_number"].ToString();
            result.transType = paramQueryList["trans_type"] == null ? string.Empty : paramQueryList["trans_type"].ToString();
            result.signature = paramQueryList["signature"] == null ? string.Empty : paramQueryList["signature"].ToString();
            result.lang = paramQueryList["language"] == null ? string.Empty : paramQueryList["language"].ToString();
            result.urlReturn = paramQueryList["url_return"] == null ? string.Empty : paramQueryList["url_return"].ToString();
            result.payType = paramQueryList["payment_type"] == null ? string.Empty : paramQueryList["payment_type"].ToString();
            result.email = paramQueryList["email"] == null ? string.Empty : paramQueryList["email"].ToString();
            result.phone = paramQueryList["phone"] == null ? string.Empty : paramQueryList["phone"].ToString();
            result.description = paramQueryList["description"] == null ? string.Empty : paramQueryList["description"].ToString();
            result.focus = paramQueryList["content_focus"] == null ? string.Empty : paramQueryList["content_focus"].ToString();
            result.currency = "VND";
            
            result.amount = double.Parse(amount, System.Globalization.CultureInfo.InvariantCulture);
            NLogLogger.LogInfo("Thông tin GetDataOrder : " + new JavaScriptSerializer().Serialize(result));

            return result;
        }

        public static int GetBankIDByBankCode(string bankCode)
        {
            if (string.IsNullOrEmpty(bankCode))
                return -1;
            bankCode = bankCode.TrimEnd().ToUpper();
            Dictionary<string, int> bankList = new Dictionary<string, int>()
            {
                {"PAYGATE",0},
                {"VIETCOMBANK",1},
                {"TECHCOMBANK",2},
                {"MB",3},
                {"PAYPAL",4},
                {"VIETINBANK",5},
                {"AGRIBANK", 6},
                {"DONGABANK", 7},
                {"OCEANBANK", 8},
                {"BIDV", 9},
                {"SHB", 10},
                {"VIB", 11},
                {"MARITIMEBANK", 18},
                {"EXIMBANK", 20},
                {"EBANK",21},
                {"MASTER",23},
                {"VISA",24},
                {"JCB",25},
                {"ACB", 26},
                {"HDBANK", 27},
                {"NAMABANK", 28},
                {"SAIGONBANK", 29},
                {"SACOMBANK", 31},
                {"VIETABANK", 33},
                {"VPBANK", 34},
                {"TIENPHONGBANK", 35},
                {"SEABANK", 37},
                {"PGBANK", 38},
                {"NAVIBANK", 41},
                {"GPBANK", 43},
                {"BACABANK", 45},
                {"PHUONGDONG", 46},
                {"ABBANK", 47},
                {"DAIABANK", 48},
                {"LIENVIETPOSTBANK", 50},
                {"BVB", 51},
                {"SCBBANK", 52},
                {"KIENLONGBANK", 54},
                {"VRB", 55},
                {"PVCOMBANK", 56}
            };
            var Bank = bankList.Where(c => c.Key == bankCode).ToList();
            if (Bank != null && Bank.Count > 0)
                return 1;//Bank[0].Value;
            return -1;
        }

    }
    public class AllowReponse
    {
        public int responseCode { get; set; }
        public string description { get; set; }
        public string extend { get; set; }
    }

}