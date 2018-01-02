using System;
using System.Web;

namespace PayWallet.Utils
{
    public class SessionUtility
    {
        #region public variable
        public const string SessionAccount = "Account";
        #endregion
        public static object GetValue(string name)
        {
            return HttpContext.Current.Session == null ? null : HttpContext.Current.Session[name];
        }


        /// <summary>
        /// hủy một session
        /// </summary>
        /// <param name="name"></param>
        public static void Remove(string name)
        {
            HttpContext.Current.Session.Remove(name);

        }




        /// <summary>
        /// Nạp giá trị cho session
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetValue(string name, object value)
        {
            HttpContext.Current.Session[name] = value;
        }


        /// <summary>
        /// Remove tất cả session hiện có
        /// </summary>
        public static void RemoveAll()
        {
            HttpContext.Current.Session.Abandon();
        }
    }
}
