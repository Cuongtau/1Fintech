namespace UtilLibraries
{
    public class Pay1FintechConsts
    {
        public const string ConnectionStringName = "Default";

        public const string DefaultPassPhrase = "E546C8DF278CD5931069B522E695D4F2";
    }

    public class CommonError
    {
        public const int AccountNameEmpty = -1001;
        public const int EmailEmpty = -1002;
        public const int PhoneNumberEmpty = -1003;
        public const int PasswordEmpty = -1004;


        public const int Systembusy = -9999;
        public static string Description(int errCode)
        {
            string desc = string.Empty;
            switch (errCode)
            {
                case AccountNameEmpty:
                    desc = "Thông tin tài khoản không được bỏ trống";
                    break;
                case EmailEmpty:
                    desc = "Email không được bỏ trống";
                    break;
                case PhoneNumberEmpty:
                    desc = "Số điện thoại không được bỏ trống";
                    break;
                case PasswordEmpty:
                    desc = "Mật khẩu không được bỏ trống";
                    break;
                default:
                    desc = "Lỗi hệ thống";
                    break;
            }
            return desc;
        }
    }
}
