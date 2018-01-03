using System;
using System.Configuration;
using PayWallet.PortalGateway.Controllers.Utils;
using PayWallet.Utils;

namespace PayWallet.PortalGateway.Models
{
    public class ReturnData
    {
        public int ResponseCode { get; set; }
        public string Description { get; set; }
        public string Extend { get; set; }


        #region Trả về dữ liệu

        /// <summary>
        /// Tổng hợp dữ liệu trả về client
        /// </summary>
        /// <param name="responseCode">ResponseCode trả về</param>
        /// <returns></returns>
        public static ReturnData GetReturnData(int responseCode)
        {
            return GetReturnData(responseCode, string.Empty);
        }


        /// <summary>
        /// Tổng hợp dữ liệu trả về client
        /// </summary>
        /// <param name="responseCode">ResponseCode trả về</param>
        /// <param name="extend">Option mở rộng</param>
        /// <returns></returns>
        public static ReturnData GetReturnData(int responseCode, string extend)
        {
            LanguageHelper.SetLangApi();

            try
            {
                var returnData = new ReturnData { ResponseCode = responseCode, Extend = extend };
                if (responseCode > 0)
                {
                    returnData.Description = Resources.Response.Message.TransactionSuccess;
                    return returnData;
                }
                switch (responseCode)
                {
                    #region Danh sách mã lỗi chung -80xx
                    case -7004: // Có sử dụng dịch vụ bảo mật ODP, OTP
                        returnData.Description = Resources.Response.Message.UsingSecurity;
                        break;
                    case -7005: // Mã odp, otp ko hợp lệ
                        returnData.Description = Resources.Response.Message.SecureCodeInvalid;
                        break;
                    case -7006: // Mã  otp hết hạn cần reload lại vị trí mới
                        returnData.Description = Resources.Response.Message.OtpExpired;
                        break;
                    case -7007: // Số dư trong tài khoản của bạn không đủ để thực hiện thao tác này
                        returnData.Description = Resources.Response.Message.NotEnoughMoney;
                        break;
                    case -7008: // Bạn đã nhận mã ODP quá 3 lần
                        returnData.Description = Resources.Response.Message.OdpOver3;
                        break;
                    case -7009: // ODP đang được gửi, bạn vui lòng chờ
                        returnData.Description = Resources.Response.Message.OdpSending;
                        break;
                    case -7010:
                        returnData.Description = Resources.Response.Message.OdpSent;
                        break;
                    case -7011:
                        returnData.Description = string.Format(Resources.Response.Message.OdpEmaiSent, extend);
                        break;
                    case -7012: //chưa đăng nhập thì redirect sang cổng thanh toán
                        returnData.Description = Resources.Response.Message.UnloginGotoGate;
                        break;
                    case -7013: //Nếu không đủ số dư thì redirct sang cổng thanh toán
                        const string urlRecharge = "https://paygate.vtc.vn/vi-dien-tu/ho-so/nap-tien/nap-tien-vao-tai-khoan.html";
                        var url365 = Config.Domain + "/#";
                        returnData.Description = string.Format(Resources.Response.Message.NotMoneyGotoGate, extend, urlRecharge, url365);
                        break;

                    case -7014: //Số di động không hợp lệ
                        returnData.Description = Resources.Response.Message.PhoneNumberInvalid;
                        break;
                    case -7015: //Gửi mã thẻ không thành công
                        returnData.Description = Resources.Response.Message.SentCardCodeFail;
                        break;

                    case -7016:// 1 ngày mã 3 ODP SMS
                        returnData.Description = Resources.Response.Message.Max3ODP;
                        break;
                    case -7017: //Mỗi lần nhận lại ODP SMS cách nhau 30s
                        returnData.Description = Resources.Response.Message.ODPOther30s;
                        break;
                    case -7018:
                        returnData.Description = Resources.Response.Message.SentSecureCodeFail;
                        break;
                    #endregion Danh sách mã lỗi chung 70xx

                    #region Danh sách thông báo lỗi Đăng ký tài khoản -81xx
                    
                    case -8100:
                        returnData.Description = Resources.Response.Message.InputUserName;
                        break;
                    case -8101:
                        returnData.Description = Resources.Response.Message.UserNameInvalid;
                        break;
                    case -8102:
                        returnData.Description = Resources.Response.Message.InputPassword;
                        break;
                    case -8103:
                        returnData.Description = Resources.Response.Message.PasswordPolicy;
                        break;
                    case -8108:
                        returnData.Description = Resources.Response.Message.RetypePassword;
                        break;
                    case -8104:
                        returnData.Description = Resources.Response.Message.RetypePassInvalid;
                        break;
                    case -8105:
                        returnData.Description = Resources.Response.Message.InputEmail;
                        break;
                    case -8106:
                        returnData.Description = Resources.Response.Message.EmailInvalid;
                        break;
                    case -8107:
                        returnData.Description = Resources.Response.Message.EmailExisted;
                        break;
                    case -8109:
                        returnData.Description = Resources.Response.Message.InputFullName;
                        break;
                    case -8110:
                        returnData.Description = Resources.Response.Message.InputPassport;
                        break;
                    case -8111:
                        returnData.Description = Resources.Response.Message.InputCaptcha;
                        break;
                    case -8112:
                        returnData.Description = Resources.Response.Message.InputBussinessName;
                        break;
                    case -8113:
                        returnData.Description = Resources.Response.Message.InputBussinessRegister;
                        break;
                    case -8114:
                        returnData.Description = Resources.Response.Message.InputTaxCode;
                        break;
                    case -8115:
                        returnData.Description = Resources.Response.Message.InputHeadOffice;
                        break;
                    case -8116:
                        returnData.Description = Resources.Response.Message.InputMobileContact;
                        break;
                    case -8117:
                        returnData.Description = Resources.Response.Message.SelectCity;
                        break;
                    case -8118:
                        returnData.Description = Resources.Response.Message.SelectDistrict;
                        break;
                    case -8119:
                        returnData.Description = Resources.Response.Message.InputAddressBussiness;
                        break;
                    case -8120:
                        returnData.Description = Resources.Response.Message.AccountExisted;
                        break;
                    case -8121:// TK ko hoạt động
                        returnData.Description = Resources.Response.Message.AccountUnActive;
                        break;
                    case -8122:
                        returnData.Description = Resources.Response.Message.ActiveCodeInvalid;
                        break;
                    case -8123:
                        returnData.Description = Resources.Response.Message.ActiveAccNotExist;
                        break;
                    case -8124:
                        returnData.Description = Resources.Response.Message.VerifyCodeInvalid;
                        break;
                    case -8125:
                        returnData.Description = Resources.Response.Message.NoteTelcoFastReg;
                        break;
                    case -8126:
                        returnData.Description = Resources.Response.Message.DataNotValid;
                        break;
                    case -8127:
                        returnData.Description = Resources.Response.Message.ServiceInvalid;
                        break;
                    case -8128:
                        returnData.Description = Resources.Response.Message.CaptchaExpires;
                        break;
                    case -8129:
                        returnData.Description = Resources.Response.Message.CaptchaInvalid;
                        break;
                    case -8130:
                        returnData.Description = Resources.Profile.Account.AccountNotExists;
                        break;
                    case -8131:
                        returnData.Description = Resources.Profile.Account.AccountLocked;
                        break;
                    case -8132: //chưa kích hoạt
                        returnData.Description = Resources.Response.Message.AccountInActive;
                        break;
                    case -8133:
                        returnData.Description = Resources.Response.Message.UserOrPasswordInvalid;
                        break;
                    case -8134:
                        returnData.Description = Resources.Response.Message.RequestInvalid;
                        break;
                    case -8135:
                        returnData.Description = Resources.Checkout.NotAllowLogin;
                        break;
                    case -8136:
                        returnData.Description = Resources.Response.Message.PasswordOldNeedChange;
                        break;
                    case -8137: //TK không thể thực hiện chức năng
                        returnData.Description = Resources.Response.Message.AccountNotUseFunction;
                        break;
                    case -8138: //TK chưa đăng ký bảo mật
                        returnData.Description = Resources.Checkout.MissSecure;
                        returnData.Extend = ConfigurationManager.AppSettings["LinkWebPaySecure"];
                        break;
                    case -8139:
                        returnData.Description = Resources.Response.Message.EmailNotVerify;
                        break;
                    #endregion Danh sách thông báo lỗi mua mã thẻ 71xx

                    


                    #region DS thông báo lỗi THanh toán
                    case -1: //Đơn hàng thanh toán tại ngân hàng thất bại
                        returnData.Description = Resources.Response.Message.OrderBankFail; break;
                    case -2: //Khách hàng hủy đơn hàng, Bank thông báo hủy đơn hàng
                        returnData.Description = Resources.Response.Message.CustomerOrderCancel; break;
                    case -3: //Thông tin thẻ thanh toán không hợp lệ
                    case -4: //Nhập sai thông tin thẻ. Bạn được nhập lại thêm 2 lần nữa !
                    case -5: //Nhập sai thông tin thẻ. Bạn được nhập lại thêm 1 lần nữa !
                        returnData.Description = Resources.OnlineBanking.CardInfoInvalid; break;
                    case -6: //Thẻ của bạn đã bị khóa !
                        returnData.Description = Resources.OnlineBanking.CardIsLocked; break;
                    case -8: //Tài khoản khách hàng không đủ điều kiện giao dịch
                        returnData.Description = Resources.Response.Message.BankAccountNotEligible; break;
                    case -20: //Giao dịch đã bị từ chối !
                        returnData.Description = Resources.Response.Message.TransactionReject; break;
                    case -25: //Giao dịch lỗi tại ngân hàng
                        returnData.Description = Resources.Response.Message.TransactionBankFail; break;
                    case -26: //Lỗi khi thực hiện tại hệ thống VTC Pay
                        returnData.Description = Resources.Response.Message.TransactionVTCFail; break;
                    case -7: //Giao dịch Review 
                    case -799: //nghi vấn
                        returnData.Description = Resources.Response.Message.TransactionReview;
                        break;
                    case -33: //chưa kích hoạt
                        returnData.Description = string.Format(Resources.Response.Message.AccountInActive, "<a href='" + Config.Domain + "register/activeaccount?userName=" + extend
                            + "&act=paygate' target='_blank'>" + Resources.Checkout.Here + "</a>");
                        break;
                    case -48: //TK bị block
                        returnData.Description = Resources.Profile.Account.AccountLocked;
                        break;
                    case -49:// TK ko hoạt động

                        returnData.Description = Resources.Response.Message.AccountUnActive;
                        break;
                    case -50://TK ko tồn tại
                        returnData.Description = Resources.Profile.Account.AccountNotExists;
                        break;
                    case -51: // Không đủ số dư
                    case -660: //Số dư TK ko hợp lệ
                        returnData.Description = Resources.Response.Message.NotEnoughMoney;
                        break;
                    case -53://Mật khẩu không đúng
                        returnData.Description = Resources.Response.Message.UserOrPasswordInvalid;
                        break;
                    case -54: //TK bị block do login lỗi 
                        returnData.Description = Resources.Response.Message.BlockByLoginFail;
                        break;
                    case -55: //TK thanh toán và TK nhận không được trùng nhau
                        returnData.Description = Resources.Response.Message.AccountDuplicate;
                        break;
                    case -91: //Giao dịch tại Bank đã thành công, nhưng số tiền Bank trả về không khớp với dữ liệu tại VTC
                        returnData.Description = Resources.Response.Message.TransactionNotMatch; break;
                    case -99: //Không đủ thông tin xác định trạng thái của giao dịch từ ngân hàng
                        returnData.Description = Resources.Response.Message.TransactionUnknow; break;
                    case -100:
                        returnData.Description = Resources.Response.Message.ServiceInvalid;
                        break;
                    case -141: //GD ko hợp lệ
                        returnData.Description = Resources.Response.Message.TransactionInvalid;
                        break;
                    case -501:
                        returnData.Description = Resources.Response.Message.AmountPayMinLitmit + extend;
                        break;
                    case -502:
                        returnData.Description = Resources.Response.Message.AmountPayMaxLimit + extend;
                        break;
                    case -503:
                        returnData.Description = Resources.Response.Message.AmountPayLimitDay + extend;
                        break;
                    case -504:
                        returnData.Description = Resources.Response.Message.AmountPayLimitMonth + extend;
                        break;
                    case -505://chưa gắn kết Ngân hàng
                        returnData.Description = Resources.Response.Message.AccountNotLinkBank;
                        break;
                    case -506://Ngân hàng ko tồn tại
                        returnData.Description = Resources.Response.Message.BankNotExist;
                        break;
                    case -507://Ngân hàng ko hợp lệ
                        returnData.Description = Resources.Response.Message.BankPaymentInvalid;
                        break;
                    case -508:
                        returnData.Description = Resources.Response.Message.UsingSecurity;
                        break;
                    case -509: // Mã odp, otp ko hợp lệ
                    case -6666:  //Mã bảo mật không hợp lệ
                        returnData.Description = Resources.Response.Message.SecureCodeInvalid;
                        break;
                    case -510: //Tổng tiền thanh toán vượt hạn mức ngày
                        returnData.Description = Resources.OnlineBanking.BankAmountOverLitmitDay;
                        break;
                    case -511: //Tổng GD vượt hạn mức ngày
                        returnData.Description = Resources.OnlineBanking.BankTransOverLimitDay;
                        break;
                    case -512: //TK bank ko đủ số dư
                        returnData.Description = Resources.OnlineBanking.BankNotEnoughBalance;
                        break;
                    case -513: //TK bank ko hoạt động
                        returnData.Description = Resources.OnlineBanking.BankAccInActive;
                        break;
                    case -514: //KH chưa đký dịch vụ tại Bank
                        returnData.Description = Resources.OnlineBanking.BankNotRegisterService;
                        break;
                    case -515: //TK ko đc phép thanh toán
                        returnData.Description = Resources.OnlineBanking.BankAccNotAlowedPayment;
                        break;
                    case -600: //dữ liệu ko hợp lệ
                    case -701:
                    case -702:
                        returnData.Description = Resources.Response.Message.DataNotValid;
                        break;
                    case -603: //Không tạo được đơn hàng
                        returnData.Description = Resources.Response.Message.CreateOrderFail;
                        break;
                    case -606: //Đơn hàng ko hợp lệ
                        returnData.Description = Resources.Checkout.OrderInfoInvalid;
                        break;
                    case -699: //Mã tích hợp ko hợp lệ
                        returnData.Description = Resources.Checkout.WebsiteIntegratedInvalid;
                        break;
                    case -700: //Yêu cầu không hơp lệ
                        returnData.Description = Resources.Response.Message.RequestInvalid;
                        break;
                    case -703://Sai chữ ký 
                        returnData.Description = Resources.Response.Message.WrongSignal;
                        break;
                    case -704://Số tiền không hợp lệ
                        returnData.Description = Resources.Response.Message.AmountPaymentInvalid;
                        break;
                    case -705: //TK nhận tiền ko tồn tại
                        returnData.Description = Resources.Checkout.AccountReceiveNotExist;
                        break;
                    case -708: //GD thất bại
                        returnData.Description = Resources.Response.Message.TransactionFail;
                        break;
                    case -800: //Trùng mã GD
                        returnData.Description = Resources.Checkout.DuplicateOrderCode;
                        break;
                    case -801: //Đơn hàng ko tồn tại
                        returnData.Description = Resources.Checkout.OrderNotExist;
                        break;
                    case -1002: //Sai chữ ký
                        returnData.Description = Resources.Checkout.WrongSignature;
                        break;
                    case -3000: //chonj NH thanh toans
                        returnData.Description = Resources.Checkout.SectionBank;
                        break;
                    case -3001: //ko the thanh toán NH nội địa
                        returnData.Description = Resources.Checkout.MessageErrorPayment;
                        break;
                    case -3002: //ko thể thanh toán thẻ quốc tế
                        returnData.Description = Resources.Checkout.MessageErrorPayInternalCard;
                        break;
                    case -6667: //mã bảo mật sai quá số lần quy định
                        returnData.Description = Resources.Response.Message.SecureCodeWrongManyTime;
                        break;

                    case -8999: // Exception
                        returnData.Description = Resources.Response.Message.SystemBusy;
                        break;
                    #endregion
                    default:
                        //NLogLogger.LogInfo("Mã lỗi chưa định nghĩa:" + responseCode);
                        return GetReturnData(-8999, string.Empty);
                }

                return returnData;
            }
            catch (Exception exception)
            {
                NLogLogger.PublishException(exception);
                return GetReturnData(-8999, string.Empty);
            }
        }

        #endregion Trả về dữ liệu
    }


    /// <summary>
    /// Dữ liệu trả về khi load trang dịch vụ bảo mật
    /// </summary>
    public class ReturnDataChangePass : ReturnData
    {

        public int Count { get; set; }
    }
    public class PaymentQRCodeResponse : ReturnData
    {
        public int AccountID { get; set; }
        public byte UtilServiceId { get; set; } //Mã bảo mật (nếu sử dụng)
        public string SecureCode { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Sign { get; set; } //Chữ ký thực hiện confirm thanh toán
    }

    public class ConfirmWalletQRCodeResponse : ReturnData
    {
        public string Fullname { get; set; }
    }
}