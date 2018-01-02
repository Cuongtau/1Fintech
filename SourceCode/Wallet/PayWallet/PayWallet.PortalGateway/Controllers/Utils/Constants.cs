
namespace PayWallet.PortalGateway.Utils
{
    public class Constants
    {
        public static string[] International_BankCode = new string[] { "MASTER", "VISA", "JCB", "PAYPAL" };

        public static string WALLET_PAYMENT_TYPE = "VTCPAY";
        
        public enum PaymentType : int
        {
            VND = 1,
            USD = 2
        }
        public enum BankType : int
        {
            DomesticBank = 1,
            InternationBank = 2
        }

        // Response trả về cho phía gọi API (Web, App)
        public enum ResponseCode : int
        {
            TRANSACTION_SUCCESS_FULL = 1,
            TRANSACTION_REVIEW = -7, //GD nghi vấn chờ duyệt
            ACCOUNT_DUPLICATE = -55, //TK chuyển nhận trùng nhau
            ACCOUNT_NOT_EXISTS = -50, //TK ko tồn tại
            ACCOUNT_NOT_ENOUGH_BALANCE = -51, //số dư ví không đủ
            PASSWORD_IS_WRONG = -53, //Sai MK
            ACCOUNT_BLOCK_LOGIN_FAIL = -54, //Tk bị khóa do đăng nhập sai
            ACCOUNT_UNACTIVE = -49, //TK ko hoạt động
            ACCOUNT_ISBLOCK = -48, //TK bị Block
            ACCOUNT_INACTIVE = -33, //TK chưa kích hoạt
            //Mã lỗi GD từ Pay
            SERVICE_INVALID = -100,//Dịch vụ ko hợp lệ
            TRANSACTION_INVALID = -141, //GD ko hợp lệ

            MIN_AMOUNT_INVALID = -501, //Nhỏ hơn hạn mức dưới
            MAX_AMOUNT_INVALID = -502, //Lớn hơn hạn mức trên
            AMOUNT_DAY_INVALID = -503, //Vượt hạn mức ngày
            AMOUNT_MONTH_INVALID = -504, //Vượt hạn mức tháng
            ACCOUNT_NOT_LINK_BANK = -505, //ví chưa được gắn kết
            BANKPAYMENT_NOT_EXIST = -506, //Bank ko tồn tại
            BANKPAYMENT_INVALID = -507, //Bank thanh toán ko hợp lệ
            SEND_OTP_REQUIRE_INPUT = -508, //Giao dịch nghi vấn chờ duyệt
            OTP_CODE_INVALID = -509, //OTP ko hợp lệ
            TOTALAMOUNT_OVER_LIMIT_DAY = -510, //Tổng tiền thanh toán vượt hạn mức ngày
            TOTALTRANS_OVER_LIMIT_DAY = -511, //Tổng GD vượt hạn mức ngày
            BANKACCOUNT_NOT_ENOUGH_BALANCE = -512, //TK bank ko đủ số dư
            BANKACCOUNT_INACTIVE = -513, //TK bank ko hoạt động
            NOT_REGISTER_SERVICE_BANK = -514, //KH chưa đký dịch vụ tại Bank
            BANKACCOUNT_NOT_ALLOWED_PAYMENT = -515, //TK ko đc phép thanh toán
            //Mã từ BankService bên a Huân
            MERCHANT_INTEGRATED_INVALID = -699, // Thong so tich hop cua Merchant khong hop le
            INVALID_REQUEST = -700, //  Request khong hop le
            MISSING_FIELD_REQUIRED = -701,  // Request truyen lien thieu tham so
            PARAM_VALUE_INVALID = -702, // Gia tri tham so truyen len khong hop le
            WRONG_SIGNATURE = -703,  // Sai chu ky          
            TRANSACTION_AMOUNT_INVALID = -704,  // So tien khong hop le (Qua nho hoac khong hop le)
            RECEIVER_ACCOUNT_INVALID = -705,    // Tai khoan nhan tien khong hop le
            CUSTOMER_ACCOUNT_INVALID = -706,     // Tai khoan Pay khach hang khong dung
            AUTHEN_VTCPAY_FAIL = -707,   // Authen tai VTC Pay that bai
            TRANSACTION_FAIL = -708, //Giao dịch thất bại
            ERROR_INTERNAL_QUERY = -730,  // Loi noi bo he thong VTC, khi query thong tin, chua thuc hien giao dich (Fail)
            TRANSACTION_CONFUSE = -799, // Loi nhưng chua biet ro trang thai thanh cong hay that bai
            ORDERMERCHANT_DUPLICATE = -800, //Trùng mã order
            ORDERMERCHANT_NOT_EXISTS = -801, //Không tồn tại đơn hàng

            PARAM_REQUEST_INVALID = -600, //Tham số ko hợp lệ
            CREATE_ORDER_FAIL = -603, //Không tạo được đơn hàng
            ORDER_INVALID = -606, //Đơn hàng ko hợp lệ
            ACCOUNT_BALANCE_INVALID = -660, //Số dư TK ko hợp lệ
            
            SECURECODE_INVALID = -6666, //Mã bảo mật không hợp lệ
            SECURECODE_WRONG_MANYTIME = -6667, //mã bảo mật sai quá số lần quy định
            SIGN_PAYMENT_INVALID = -1002, //Chữ ký ko hợp lệ hoặc hết hạn

            ACCOUNT_NOT_USE_FUNCTION = -8137, //TK ko đc phép truy cập chức năng
            NOT_USE_ACCOUNTSECURE = -8138, //Chưa sử dụng bảo mật
            EXCEPTION = -9669,

            
        }
        //Mã lỗi từ Bank Service
        public enum AppBankResponseCode : int
        {
            TRANSACTION_IN_PROCESS = 0, //Khởi tạo
            TRANSACTION_SEND = 1, //Đơn hàng đã được gửi sang Bank thành công hoặc đã tạo Url show đơn hàng thành công (Với các đơn hàng thực hiện tại VTC). Thẻ ATM khách chưa bị trừ tiền
            VERIFY_BANK_SUCCESS = 2, //Kiểm tra thông tin thẻ ngân hàng của khách hàng thành công
            TRANSACTION_REVIEW = 7, //TK bank của khách hàng đã bị trừ tiền, đơn hàng sẽ được Admin kiểm tra rủi ro rồi duyệt. Có thể duyệt thành công hoặc thất bại
            TRANSACTION_SUCCESS = 100, //Thanh toán tại Bank thành công
            TRANSACTION_FAIL = -1, //Đơn hàng thanh toán tại Bank thất bại
            TRANSACTION_CUSTOMER_CANCEL = -2, //Khách hàng hủy đơn hàng, Bank thông báo hủy đơn hàng (thông qua url thất bại)
            INFO_PAYMENT_INVALID = -3, //Thông tin thẻ thanh toán không hợp lệ ( Không tồn tại, bị khóa, không đủ điều kiện giao dịch ...
            INFO_CARD_INVALID_INPUT1 = -4, //Nhập sai thông tin thẻ, được nhập lại thêm hai lần
            INFO_CARD_INVALID_INPUT2 = -5, //Nhập sai thông thi thẻ, được nhập lại thêm một lần
            CARD_BLOCKED = -6, //Thẻ bị khóa
            CARD_TRADING_LIMIT_DAY = -7, //Thẻ đã giao dịch quá hạn mức trong ngày (Hạn mức về số tiền hoặc số lần)
            ACCOUNT_NOT_ENOUGH_MONEY = -8, //The/TK khach hang khong du tien de GD
            REFUSE_TRANSACTION = -20, //Từ chối GD
            VERIFY_CONFIRM_PAYMENT_INVALID = -24, //OTP SMS hoặc OTP Token confirm thanh toán không đúng
            BANK_RETURN_TRANSACTION_FAIL = -25, //FAIL	Loi biet chac that bai do ngan hang tra ve
            VTC_RETURN_TRANSACTION_FAIL = -26, //Loi khi thuc hien tai he thong VTC
            AMOUNT_TRANSACTION_INVALID = -91, //Giao dịch tại Bank đã thành công, nhưng số tiền Bank trả về không khớp với dữ liệu tại VTC
            ENOUGH_INFO_RESULT_TRANSACTION = -99, //Trường hợp không đủ thông tin để biết kết quả cuối cùng của GD là thành công hay thất bại
            REQUEST_INVALID = -700, //Request khong hop le
            MISSING_FIELD_REQUIRED = -701, //Request truyen len thieu tham so
            PARAM_VALUE_INVALID = -702, // Gia tri tham so truyen len khong hop le
            WRONG_SIGNATURE = -703,  // Sai chu ky          
            TRANSACTION_AMOUNT_INVALID = -704,  // So tien khong hop le (Qua nho hoac khong hop le)
            CUSTOMER_ACCOUNT_INVALID = -706,     // Tai khoan Pay khach hang khong dung
        }
        // Danh sách mã lỗi trả về cho Merchant (Đối tác tích hợp cổng)
        // Đối tác nhận được kết quả từ VTC, phải check chữ ký hợp lệ trước. Nếu chữ ký không hợp lệ thì kết quả VTC trả về không tin cậy. Cần kiểm tra từ báo cáo lịch sử GD hoặc phối hợp xử lý thủ công        
        public enum MerchantOrderStatus : int
        {
            TRANACTION_IN_PROCESS = 0,                  // GD ở trạng thái khởi tạo
            TRANSACTION_REVIEW = 7,                     // GD thẻ quốc tế ở trạng thái REVIEW, TK ngân hàng của người mua đã bị trừ tiền, TK merchant chưa được cộng tiền. Khi quản trị VTC duyệt thành công/thất bại kết quả sẽ post ngầm về cho Merchant
            TRANSACTION_SUCCESS_FULL = 1,               // LIST REPONSE CODE SUCCESSS

            // LIST REPONSE CODE FAIL          
            TRANSACTION_FAIL = -1,                      // FAIL, Giao dịch thất bại chung chung
            CUSTOMER_CANCEL_TRANSACTION = -9,           // FAIL, KHACH HANG TU HUY GIAO DICH
            VTC_CANCEL_TRANACTION = -3,                 // FAIL, GIAO DICH DO QUAN TRI VTC HUY
            CUSTOMER_BALANCE_NOT_ENOUGH = -5,           // FAIL, Số dư TK VTC Pay hoặc thẻ/tài khoản ngân hàng của khách hàng không đủ để thực hiện giao dịch
            VTC_INTERNAL_ERROR_DEFIND = -6,             // FAIL, Lỗi xảy ra tại VTC và biết chắc kết quả GD thất bại
            CUSTOMER_PAYMENT_INFO_INVALIE = -7,         // FAIL, Khách hàng nhập thông tin TK thanh toán hoặc OTP sai dẫn đến GD thất bại

            ORDER_CODE_DUPLICATE = -21,                 // Trùng mã giao dịch, Có thể do xử lý duplicate không tốt nên mạng chậm hoặc khách hàng nhấn F5 bị, hoặc cơ chế sinh mã GD của đối tác không tốt nên sinh bị trùng, DOI TAC CAN KIEM TRA LAI TRANG THAI GIAO DICH NAY (QUA LICH SU GD HOAC QUA API VTC CAP)
            AMOUNT_IS_TOO_SMALL = -22,                  // FAIL, Số tiền thanh toán đơn hàng của đối tác quá nhỏ
            WEBSITE_ID_NOT_EXITS = -23,                 // Website không tồn tại sẽ dẫn đến không có chữ ký, do vậy không verify được kết quả trả về từ VTC. Đối tác cần phối hợp kiểm tra mới xác định được kết quả GD
            CURRENCY_INVALID = -24,                     // FAIL, Đơn vị tiền tệ thanh toán không hợp lệ
            RECEIVER_ACCOUNT_NOT_EXITS = -25,           // FAIL, Tài khoản nhận tiền của Merchant không tồn tại

            MISSING_REQUIRED_PARAM = -28,               // FAIL, Thiếu tham số bắt buộc phải có trong một đơn hàng thanh toán online
            REQUEST_PARAM_INVALID = -29,                // FAIL, Một hoặc nhiều tham số request không hợp lệ
            // END LIST FAIL
            ACCOUNT_INACTIVE = -49,                     //TK chưa kích hoạt
            TRANSACTION_EXPIRED = -50,                  //Quá hạn GD
            BANKACCOUNT_NOTEXISTS = -51,                //THÔNG TIN THẺ KO TỒN TẠI
            BANKACCOUNT_INVALID = -52,                  //SÔ TK ko hợp lệ
            ERROR_UN_CONTROL = -99                      // CONFUSE, LOI KHONG KIEM SOAT, CAN KIEM TRA để biết trạng thái giao dịch cuối cùng thành công hay thất bại            
        }  
    }

    public static class PackageFee
    {
        public static string CUSTOMER_PAY_FEE = "NH1";   // Nguoi mua phai chiu phi giao dich
        public static string MERCHANT_PAY_FEE = "NH2";   // Nguoi ban chiu phi 
        public static string MERCHANT_PAY_FEE_PG1 = "PG1";  // Goi phi 1.1% + 1760
        public static string MERCHANT_PAY_FEE_PG2 = "PG2";  // Goi phi3%
    }

    public static class Currency
    {
        public static string VND = "VND";
        public static string USD = "USD";
    }

    public static class MerchantType
    {
        public static string Website = "WEBSITE";
        public static string Button = "BUTTON";
        public static string App = "APP";
    }
    public static class BankType
    {
        public static string DomesticBank = "DOMESTICBANK";
        public static string InternationalCard = "INTERNATIONALCARD";
    }
    

}