using System.Web;

namespace PayWallet.Utils
{
    public class SessionsManager
    {
        public const string SESSION_USERID = "UserID";
        public const string SESSION_USERNAME = "UserName";
        public const string SESSION_USER = "User";
        public const string SESSION_FUNCTIONS = "Functions";
        public const string SESSION_LIST_FUNCTION_DETAIL = "List_Function_Detail";
        public const string SESSION_ISADMIN = "IsAdmin";
        public const string SESSION_ID = "ID";
        public const string SESSION_PERMISSION = "Permission";
        public const string SESSION_OBJECT = "Object";
        public const string SESSION_OBJECTTMP = "ObjectTmp";
        public const string SESSION_HISTORY = "History";
        public const string SESSION_LANGUAGE = "Language";
        public const string SESSION_PARAM = "Parameter";
        public const string SESSION_FILES = "FilesManager";
        public const string SESSION_NEWS_VI_EN = "NewsViEn";
        public const string SESSION_SYSTEM = "System";
        public const string SESSION_PAGECOUNT= "PageCount";
		public const string SessionListPhone = "ListPhone";
		public const string Session_FileName = "FileName";
        public const string Session_QueueOrderID = "QueueOrderID";
        public const string Session_QueueOrderFileID = "QueueOrderFileID";
        public const string SESSION_ACCOUNTID = "EPosAccountID";
        public const string SESSION_ACCOUNTNAME = "Mobile";
        public const string SESSION_STRINGVALUE = "stringValue";
        public const string SESSION_ACCOUNTFULL = "AccountFullInfo"; //Session xử lý back button
        /// <summary>
        /// lấy ra giá trị session
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetValue(string name)
		{
			return HttpContext.Current.Session[name];
		}

		/// <summary>
		/// hủy một session
		/// </summary>
		/// <param name="name"></param>
		public void Remove(string name)
		{
			HttpContext.Current.Session.Remove(name);

		}

		/// <summary>
		/// Nạp giá trị cho session
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void SetValue(string name, object value)
		{
			HttpContext.Current.Session[name] = value;
		}
 
    }
}
