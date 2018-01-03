using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PayWallet.PortalGateway.Models
{
    public class InfoPaymentModel
    {
        public MerchantRequestData OrderInfo { get; set; }
        public PaymentAPI_ResponseData CheckRequest { get; set; }
        public string MerchantLogo { get; set; }
        public string ProviderName { get; set; }
        public string QueryUrl { get; set; }
    }
    
    //Request của merchant
    public class MerchantRequestData
    {
        public int merchant_id { get; set; } //Mã website dối tác
        public double amount { get; set; } //Số tiền thanh toán
        public string ordercode { get; set; } //Mã tham chiếu (mã hóa đơn)
        public string currency { get; set; } //Loại tiền thanh toán (VND, USD...)
        public string transType { get; set; } //sale
        public string signature { get; set; } //chữ ký SHA(256)
        public string lang { get; set; } //Ngôn ngữ (mặc định tiếng việt) vi:Việt nam, en: English
        public string urlReturn { get; set; } //Url của merchant mà VTC sẽ redirect
        public string payType { get; set; } //Loại thanh toán(VTCPay, Vietcombank...)
        public string email { get; set; } //Email KH
        public string phone { get; set; } //SĐT KH

        public string description { get; set; }
        public string merchantType { get; set; } //Loại merchant tích hợp -> "Website" | "Button"
        public int quantity { get; set; }
        public string focus { get; set; }
        public int buttonType { get; set; } // Loại nút >> 1:thường, 2:tùy biến, 3: quyên góp, 4:ủng hộ
        public decimal amountUSD { get; set; }
    }
    
    public class ObjectExtend
    {
        public string queryUrl { get; set; }
        public string providerName { get; set; }
        public string content_focus { get; set; }
    }
    /// <summary>
    /// Dữ liệu trả về từ API thanh toán
    /// TRANSACTION_SUCCESS_FULL = 1	Giao dịch thành công
    /// MERCHANT_INTEGRATED_INVALID = -699	Thông số tích hợp của Merchant không hợp lệ
    /// INVALID_REQUEST = -700	Tham số request không hợp lệ
    /// MISSING_FIELD_REQUIRED = -701	Request truyền lên thiếu tham số
    /// PARAM_VALUE_INVALID = -702	Giá trị tham số truyền lên không hợp lệ
    /// WRONG_SIGNATURE = -703	Sai chữ ký
    /// TRANSACTION_AMOUNT_INVALID = -704	Giá trị giao dịch không hợp lệ
    /// RECEIVER_ACCOUNT_INVALID = -705	Tài khoản nhận tiền không hợp hoặc đang bị khóa
    /// ERROR_INTERNAL_QUERY = -730	Lỗi xảy ra trong hệ thống VTC khi truy vấn dữ liệu

    /// </summary>
    public class PaymentAPI_ResponseData
    {
        public int ResponseCode { get; set; }
        public long OrderBillingId { get; set; }
        public string Message { get; set; }
        public string RedirectUrl { get; set; }
        public string PostData { get; set; }
        public string Signature { get; set; }
    }
    
    public class GetResultBankRequestData
    {
        public string profileID { get; set; }
        public string data { get; set; } //Kết quả giao dịch từ Bank Redirect về trang đón
        public string sign { get; set; } //Chữ ký tương ứng với dữ liệu Bank Redirect về trang đón
    }
    public class PostResultBankRequestData
    {
        public string profileID { get; set; }
        public string response { get; set; } //Kết quả giao dịch từ Bank Post về trang đón
        public string sign { get; set; } //Chữ ký tương ứng với dữ liệu Bank Redirect về trang đón
    }

    public class BankPaymentRequestData
    {
        public int bankID { get; set; }
        public string bankCode { get; set; }
        public short bankType { get; set; }
        public string bankInfo { get; set; }
        public string extendData { get; set; }
    }

    public class PaymentBankFocusByBankCode
    {
        public string providerName { get; set; }
        public string bankCode { get; set; }
        public short bankType { get; set; }
        public string queryUrl { get; set; }
    }
    public class VerifyCardRequestData
    {
        public string BankCode { get; set; }
        public string BankFullName { get; set; }
        public string BankCardNumber { get; set; }
        public double Amount { get; set; }
        public string MerchantCode { get; set; }
        public long TransactionID { get; set; }
        public string ProductDescription { get; set; }
    }

    public class ConfirmBankDirectRequestData
    {
        public long OrderID { get; set; }
        public string OTP { get; set; }
        public string VerifySign { get; set; }
    }
    public class BankVerifyRequestData
    {
        public int bankID { get; set; }
        public string bankCode { get; set; }
        public string bankAccountHolder { get; set; }
        public string bankAccountNumber { get; set; }
        public string dateOpenCard { get; set; }
        public string extendData { get; set; }
    }

  
    public class BankDirectData
    {
        public long billingOrderId { get; set; }
        public string bankAccountNumber { get; set; }
    }

    //Object request thanh toán trên cổng (Sacombank)
    public class PaymentInfo
    {
        public string queryUrl { get; set; }
        public string MerchantFullName { get; set; }
        public string MerchantAdress { get; set; }
        public string MerchantEmail { get; set; }
        public string MerchantPhone { get; set; }

        public string TransactionID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PaymentType { get; set; }
        public byte Status { get; set; }// 1: Thanh cong; 0: Khoi tao; 7: Review; 3,4:Huy(That bai) 
        public long OrderAmount { get; set; }// Nguyen gia tri luc thanh toan
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; } // Gia mot san pham
        public int Fee { get; set; } // Phi thanh toan
        public long TotalAmount { get; set; } // Tong tien phai tra ngan hang

    }
    public class OrderPaymentInfo
    {
        public string MerchantFullName { get; set; }
        public string MerchantAdress { get; set; }
        public string MerchantEmail { get; set; }
        public string MerchantPhone { get; set; }

        public long OrderAmount { get; set; } // Nguyen gia tri luc thanh toan
        public DateTime CreatedTime { get; set; }
        public string TransactionID { get; set; }
        public DateTime EndTime { get; set; }
        public byte PayType { get; set; }
        public string PaymentType { get; set; }
        public int Status { get; set; } // 1: Thanh cong; 0: Khoi tao; 7: Review; 3,4:Huy(That bai) 

        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; } // Gia mot san pham
        public int Fee { get; set; } // Phi thanh toan
        public long TotalAmount { get; set; } // Tong tien phai tra ngan hang
        public byte CurrencyType { get; set; } //1 vnd , 2 usd , 3 eur
    }

    public class PaymentToken
    {
        public int AccountID { get; set; }
        public string CustomerAcc { get; set; }  
    }

    public class BankInfo
    {
        public string MerchantCode { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string Descriptions { get; set; }
        public string Logo { get; set; }
        public long TotalAmount { get; set; }
        public long Amount { get; set; }
        public string LinkLogo { set; get; }
        public int BankType { get; set; }
    }

    public class MerchantInfo
    {
        public int MerchantCode { get; set; }
        public string MerchantKey { get; set; }
        public string MerchantName { get; set; }
        public bool IsWallet { get; set; }
        public bool IsBank { get; set; }
        public bool IsBankGlobal { get; set; }
        public string UrlNotification { get; set; }
        [JsonProperty("Website", NullValueHandling = NullValueHandling.Ignore)]
        public string Website { get; set; }
        [JsonProperty("Logo", NullValueHandling = NullValueHandling.Ignore)]
        public string Logo { get; set; }
        [JsonProperty("Description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        public byte Status { get; set; }
    }

    public class GetListBankPolicyResponseData
    {
        public int ResponseCode { get; set; }
        public List<BankInfo> ListBankPolicy { get; set; }
    }
}