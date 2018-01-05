using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Utilities.CommonModel
{
    public class ErrorCodes
    {

        #region SMS gateway error -100x
        public const int DONE = 1;//Thanh cong
        public const int SYSTEM_ERROR = -99;//Lỗi hệ thống
        public const int INVALID_DATA = -600;//Param ko hop le
        public const int PHONE_NUMBER_INVALID = -1001; //So dien thoai ko hop le
        public const int NETWORK_SUPLIER_INVALID = -1002; // nha mang ko xac dinh
        public const int SEND_TOO_FAST = -1003; //khoang cach giua 2 lan nhan tin >=5s
        public const int REQUEST_ID_DUPLICATE = -1004; //Trung request id

        public const int MO_NOT_EXIST = -1005; //Ko ton tai mo
        #endregion
    }
}