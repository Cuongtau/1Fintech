namespace PayWallet.Utils
{
    public class Contants
    {
        public const string SESSION_ACCOUNT = "Ebank.Account";              //AccountID|AccountName|AccountTypeID|AccessToken
        public const string SESSION_USERID = "Ebank.UserId";                //AccountID
        public const string SESSION_USERNAME = "Ebank.UserName";            //AccountName
        public const string SESSION_PASSWORD = "Ebank.Password";            //Password
        public const string SESSION_BORROWSTATUS = "Ebank.BorrowStatus";    //BorrowStatus
        public const string SESSION_AMOUNTOWED = "Ebank.AmountOwed";        // ???
        public const string SESSION_ISBLOCKED = "Ebank.IsBlocked";        // ???
        
        public const string SESSION_ASIGN = "Ebank.Asign";                  //Sign
        public const string SESSION_IMAGECODE = "Ebank.Captcha";            //Captcha
        public const string SESSION_LASTLOGINTIME = "Ebank.LastLoginedTime";//LastLoginTime
        
        public const string SESSION_CACHE_INVALIDACCOUNT = "Ebank.Account.CacheInvalidAccount";
        public const string SESSION_ACCOUNT_HASCAPTCHA = "Ebank.Account.HasCaptcha";

        // serviceCode mobile khi trả trước, trả sau
        public const string ServiceCode_Mobile = "MOBILE";
        //
        // MarkerCode của VTC truyền sang đối tác khi gọi service topup, check acc
        public const string MarkerCode= "VTCShop";
        //
        public const string SESSION_ACCOUNTTMP = "AccountTmp";// danh cho truong hop dang ky tai khoan. he thong lưu tam thoi
        public const string SESSION_MOBILESMSBANKING = "MobileTmp";// so dien thoai moi luu tam khi doi so dien thoai smsbanking
        public const string SESSION_BORROWVCOIN = "OrderBorrowTmp";// Thông tin vay Vcoin được lưu tạm thời để check OTP
        public const string SESSION_SSO_ACCOUNTINFO = "SESSION_SSO_ACCOUNTINFO";// SESSION_SSO_ACCOUNTINFO. he thong lưu tam thoi
        public const string SESSION_OPENIDEMAIL = "SESSION_OPENIDEMAIL";// SESSION_OPENIDEMAIL. session register Account from OpenID or facebook
        public const string SESSION_OPENIDEMAIL_PARTNER = "SESSION_OPENIDEMAIL_PARTNER";// SESSION_OPENIDEMAIL. session register Account from OpenID or facebook

        public const string BANKCODE_BIDV = "BIDV";
        public const string BANKCODE_TPBank = "TienPhongBank";
        public const string BANKCODE_SACOMBANK = "Sacombank";
        public const string BANKCODE_VIETINBANK = "Vietinbank";
        public const string BANKCODE_VISA = "Visa";
        public const string BANKCODE_MASTER = "Master";
        public const string BANKCODE_JCB = "Jcb";
        public const string BANKCODE_PAYPAL = "PAYPAL";

        public const int ButtonSupport = 3;
        public const int ButtonDonations = 4;
        public const int ButtonCustomize = 2;
        public const int ButtonNormal = 1;

        public const string MerchantType_Website = "WEBSITE";
        public const string MerchantType_Button = "BUTTON";
        public const string MerchantType_App = "APP";
        /// <summary>
        /// Định nghĩa mã lỗi thường dùng
        /// </summary>
        public enum ErrorCode
        {
            SecureCodeRequired = -1000,// Yêu cầu xác thực bảo mật
            SecureCodeInvalid = -1001,// mã xác thực không hợp lệ
            SignInvalid = -1002,// Chữ ký không hợp lệ hoặc hết hạn
            DataInvalidOrEmpty = -600,// dữ liệu không hợp lệ hoặc để trống
            SystemError=-99,// loi he thong
            AccountExisted=-46,//tai khoan da ton tai
            AccountInvalid=-1004,// tai khoan khong hop  le
            EmailExisted=-41,//Email da ton tai
            EmailInvalid=-1005,//Email khogn hop le
            FullNameInvalid=-30,//FullName null
            CaptchaInvalid=-1006,// captcha khogn chinh xac
            SendMailError=-1007,// send mail loi
            SecurityCodeInvalid=-1008,//so bi mat smsbankong null
            NewMobileTimeOut=-1009,// time out so dien thoai khi doi so dien thoai smsbanking
            VcoinBorrowMin=-1010,// Số Vcoin vay quá nhỏ
            VconiBorrowOutOff=-1011,//Số Vcoin vay vượt quá cho phép
            AccountNotSettupOTP=-1012,//Tai khoan chua cai dat OTP
            NoneDebit=-1013,//Tai khoan khong no tien
        }

        /// <summary>
        /// Định nghĩa nhóm tài khoản eBank - ở CMS
        /// </summary>
        public enum GroupID
        {
            Guest = 35,
            Personal = 36,
            Coporate = 37,
        }

        /// <summary>
        /// Danh sách  serviceCode các Game, dịch vụ đối tác
        /// </summary>
        public enum ServiceCode_GamePayment
        {
            AsiaSoft_TamQuoc=5008,
            AsiaSoft_PlayPark = 5011,
            AsiaSoft_ThienTu = 5069,
            FptGate = 5100,
            CallWorld_Register=5038,
            CallWorld_TopUp = 5037,
            Learn=5002,
            GlobalEducation=5003,
            Document=5058,
            MyCard_FB=5086,
            VTCDigital=5090
        }

        /// <summary>
        /// Danh sách các service SMS Banking
        /// </summary>
        public enum SMS_ServiceID
        {
            QueryInfo=1,    // Dịch vụ truy vấn thông tin
            Ad=2,           // Dịch vụ tin nhắn quảng cáo
            DynamicMsg=3,   // Tin nhắn chủ động
            Epayment=4,     // Dịch vụ thanh toán điện tử
            OTP=5,          // Dịch vụ bảo mật OTP
            ODP=6           // Dịch vụ bảo mật ODP
        }

        /// <summary>
        /// Danh sách các trạng thái SMS Banking
        /// </summary>
        public enum SMS_Service_Status
        {
            NotRegister=-1,  // Chưa đăng ký
            UnVerify=0,      // Chưa active
            Active=1,        // Đã active
            UActive = 2,    // Chưa active
        }
    }
}
