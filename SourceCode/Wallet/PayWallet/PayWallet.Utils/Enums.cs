using System.Collections;
using System.Collections.Generic;
namespace PayWallet.Utils
{
    public class Enums
    {
        /// <summary>
        /// Loại tài khoản trên VTCid
        /// </summary>
        public enum AccountTypeID
        {
            Personal = 1,
            Coporate = 2
        }
        public enum FunctionId
        {
            System = 1,
            Function = 2,
            User = 3,
            UserLog = 4,
            ErrorSystem = 5,
            Service = 6,
            Report = 7,
            GrantUser = 12,
            GrantService = 16,
            ReportAccLogin = 29, // Tổng Hợp tài khoản đăng nhập
            ReportAccSecure = 30, // Tổng Hợp Tài Khoản Bảo Mật
            ReportAccVerify = 31, // Tổng Hợp Xác Nhận Thông tin tài khoản
            ReportAccRegister = 32, //Tông Hợp Tại Khoản Đăng Ký
            ReportTransactionInput = 34, //[GD-Nạp] Tổng hợp giao dịch
            ReportDynamicTransactionInput = 35, //Báo cáo tùy chỉnh GD nạp
            ReportTransactionOutput = 37, //[GD-Tiêu] Tổng hợp giao dịch
            ReportAccMonitor = 38, //Tổng Hợp Tài Khoản Khách Hàng
            ReportDynamicTransactionOut = 39, //Báo cáo tùy chỉnh GD tiêu
            TransactionInputByMerchant = 41, //[GD-Nap] Theo Bộ Phận
            TransactionInputByProduct = 43, //[GD-Nap] Theo Sản Phẩm
            TransactionInputByProvider = 44, //[GD-Nap] Theo Nhà Cung Cấp
            TransactionOutputByMerchant = 45, //[GD-Tiêu] Theo Bộ Phận
            TransactionOtherGeneral = 47, //[GD-Khác] Tổng hợp
            ReportDynamicAccount = 47, //[Tài Khoản] Báo cáo TK tùy chỉnh
            TransactionSummaryImExIn = 49, //Tổng hợp dữ liệu Xuất Nhập Tồn của TK
            ReportGeneralSystem = 50 //Tổng hợp báo cáo hệ thống tài khoản VTC
        }

        public enum PayType
        {
            TopupByBank = 1,//Nạp online  qua ngân hàng tại VTCPay
            TopupByInternationalCard = 2,//Nạp online qua thẻ quốc tế tại VTCPay
            TopupByOffice = 3,//Nạp chuyển khoản tại ngân hàng (ATM/Internet Banking..)
            TopupByCard = 4, //Nạp từ thẻ cào
            TopupByVnPost = 5, //Nạp từ VNPOST
            TopupeOther = 6, //Nạp từ KHÁC
            PaymentByBank = 7,//Thanh toán qua ngân hàng nội địa
            PaymentByInternationalCard = 8,//Thanh toán qua thẻ quốc tế
            TransferMoney = 9,//Chuyển tiền thông thường
            PaymentByWallet = 10,//Thanh toán bằng ví
            Cashout = 11, ////Rút tiền ra ngân hàng online
            CashoutOffline = 12,//Rút tiền ra ngân hàng offline
            TransferMoneyService = 13, //Chuyển tiền dịch vụ
            PaymentBalanceService = 14, //Thanh toán số dư dịch vụ
            RefundMoney = 15,//Hoàn tiền
            RecoveryMoney = 16, //Truy thu
            Freeze_UnFreeze = 17, //Đóng mở băng
            SubFee_ReceiveFee = 18, //Trừ phí, hứng phí
            TopupByLinkedBank = 19, // Nap qua the gan ket
            PaymentLinkedWallet = 20,//Thanh toan qua the gan ket
            CashoutByLinkedBank = 21 // Rut qua the gan ket
        }

        public enum PaymenObjecType
        {
            VTC365 = 0,
            Button = 2,
            Website = 1
        }

        public enum CurrencyType
        {
            VND = 1,
            USD = 2,
            EU = 3
        }
        public enum ActionSecure
        {
            DELETE = 0,
            REGISTER = 1,
            CHANGE_MIN_AMOUNT = 2,
            CHANGE_STATUS = 3
        }
        public enum SecureType
        {
            ODPSMS = 1,
            ODPEmail = 2,
            OTPApp = 3,
            OTPMatrix = 4,
            NOTI_TOPUP = 5,
            NOTI_TRANFER = 6,
            NOTI_CASHOUT = 7,
            NOTI_PAYMENT = 8,
            NOTI_PAYMENT365 = 9,
            NOTI_RESETPASS = 10,
            NOTI_RECEIVE_TRANFER = 11,
            NOTI_DEBIT = 12,
            NOTI_RECEIVE_PAYMENT = 13,
            NOTI_LOGIN_DEVICE = 14,
            NOTI_RESETPASS_PERIODIC = 16,
            CHECK_DEVICE_LOGIN = 15
        }

        public enum SecureUtil_Status
        {
            DEACTIVE = 0,
            ACTIVE = 1
        }
        //Trạng thái cài đặt bảo mật OTPAPP phía Service Secure
        public enum Service_Setup_AppToken_Status
        {
            ACTIVE = 1,
            CANCEL = 9
        }

        //Trạng thái cài đặt bảo mật OTPMatrix phía Service Secure
        public enum Service_Setup_OTPMatrix_Status
        {
            REGISTER = 1,
            ACTIVE = 2,
            CANCEL = 0
        }
        //Config Gói Fee
        public enum ChargePacketFee
        {
            CUSTOMER_PAY_FEE = 5, //Gói NH1: Phí tính cho người mua
            MERCHANT_PAY_FEE = 6, //Gói NH2: Phí tính cho người bán
        }

        public enum BankID
        {
            VIETCOMBANK = 1,
            TECHCOMBANK = 2,
            MB = 3,
            VIETINBANK = 5,
            AGRIBANK = 6,
            DONGABANK = 7,
            OCEANBANK = 8,
            BIDV = 9,
            SHB = 10,
            VIB = 11,
            MARITIMEBANK = 18,
            EXIMBANK = 20,
            MASTER = 23,
            VISA = 24,
            JCB = 25,
            ACB = 26,
            HDBANK = 27,
            NAMABANK = 28,
            SAIGONBANK = 29,
            SACOMBANK = 31,
            VIETABANK = 33,
            VPBANK = 34,
            TIENPHONGBANK = 35,
            SEAABANK = 37,
            PGBANK = 38,
            NAVIBANK = 41,
            GPBANK = 43,
            BACABANK = 45,
            PHUONGDONG = 46,
            ABBANK = 47,
            DAIABANK = 48,
            LIENVIETPOSTBANK = 50,
            BVB = 51,
            SCBBank = 52,
            KIENLONGBANK = 54,
            VRB = 55,
            PVCombank = 56
        }
        public enum ServiceID
        {
            TransferServiceID = 400003, // Trừ tiền chuyển khoản
            ReceiveTransfer = 400002, //Nhận chuyển khoản
            Payment = 400001, //Thanh toán 
            Receive = 400000, //Nhan thanh toan
            Topup = 3000, //Nạp
            Cashout = 2000, //Rút
            ResetPassword = -100000,
            Warning_Login_Device = -100001,
            Warning_ResetPass_Periodic = 100002,
            Payment365 = 400004,
            CreateCardVTCPro = 600000,
            CreateCardVTCPro_V1 = 950
        }

        public enum Secure_ServiceID
        {
            ODP_SERVICE = 100, // ODP Thanh toán, chuyển khoản
            OTP_CASHOUT = 101, //OTP Rút tiền
            OTP_CHANGEPASS = 102, // OTP Đổi mật khẩu
            OTP_FORGOTPASS = 103, // OTP Quên mật khẩu
            OTP_ACTIVE_ACOUNT = 104, //OTP Dịch vụ kích hoạt
            OTP_SECURE_DEVICE = 105, //OTP Bảo mật thiết bị
            OTP_SECURE_QRCODE = 106, //OTP Check Bảo mật trên Hệ thống QRCODE
            OTP_SECURE_CHANGEEMAIL = 107,//OTP đổi Email
            OTP_SECURE_CUSTOM = 999
        }

        public enum NotifyType
        {
            WEBSITE = 1,
            APP = 2,
            SMS = 3
        }

        public enum Account_LogType
        {
            UPDATE_ACCOUNT_INFO = 1,
            CHANGE_EMAIL = 2,
            CHANGE_MOBILE = 3,
            CHANGE_PASS = 4,
            RESET_PASS_BY_SMS = 5,
            RESET_PASS_BY_EMAIL = 6,
            RESET_PASS_BY_SUPPORT = 7,
            CHANGE_ACCOUNT_TYPE = 8,
            CHANGE_AVATAR = 9
        }

        public enum DeviceType
        {
            FIREFOX = 6,
            CHROME = 7,
            IE = 8,
            COCCOC = 9,
            SAFARI = 10,
            OPERA = 11,
            OTHER = 12
        }

        public enum PartnerID
        {
            VTCQRCODE = 1,
            VTCMASTERCARD = 2,
            VTCPRO = 3
        }
    }
}
