using System;
using System.Web;
using System.Web.Caching;

namespace PayWallet.Utils
{
    public class CacheUtility
    {
        #region --- Khai báo tên các session ---
        //Danh sách chức năng hiển thị trên paygate
        public const string CachePaygateControl = "PaygateControls";

        //Danh sách chức năng hiển thị trên ví điện tử
        public const string CacheWalletControl = "WalletControls";

        //Danh sách chức năng hiển thị trên hệ thống phân phối
        public const string CacheDisutributionControl = "DistributionControls";

        public const string CacheInvalidAccount = "InvalidAccount";

        public const string BlockAccount = "BlockAccount";

        #endregion
        /// <summary>
        /// Lấy giá trị cache theo tên
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetValue(string name)
        {
            return HttpRuntime.Cache.Get(name.Trim().ToLower());
        }

        /// <summary>
        /// Hủy một giá trị cache có tên = name
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            HttpRuntime.Cache.Remove(name.Trim().ToLower());
        }

        /// <summary>
        /// Nạp giá trị cho cache
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetValue(string name, object value)
        {
            HttpRuntime.Cache.Insert(name.Trim().ToLower(), value, null, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.High, null);
        }

        /// <summary>
        /// Nạp giá trị cho cache
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetValue(string name, object value, int expireTime)
        {
            HttpRuntime.Cache.Insert(name.Trim().ToLower(), value, null, DateTime.Now.AddMinutes(expireTime), TimeSpan.Zero, CacheItemPriority.High, null);
        }

        /// <summary>
        /// Check IP
        /// </summary>
        /// <returns></returns>
        public static long CheckIpValid()
        {
            try
            {
                string ip = RequestUtility.IpClient();
                object cacheCounter = GetValue(ip.ToLower());

                if (cacheCounter == null)
                    return 0;
                NLogLogger.LogInfo(cacheCounter.ToString());
                return Convert.ToInt64(cacheCounter);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return 0;
            }
        }

        //private const int HourExprize = 4 * 60; // không hiểu lắm

        /// <summary>
        /// CheckLogin
        /// </summary>/// <summary>
        /// CheckLogin
        /// </summary>
        public static long CheckLogin(string name)
        {
            //CheckIpValid();
            var login = GetValue(name);
            if (login == null) return 0;
            var array = login.ToString().Split('|');
            long logcount;

            long.TryParse(array[0], out logcount);
            DateTime date;
            DateTime.TryParse(array[1], out date);
            if (date.Year < 2011) return 0;
            var exprize = DateTime.Now - date;
            if (exprize.Hours > 240)
            {
                new CacheUtility().Remove(name);
                return 0;
            }
            return logcount;
        }

        /// <summary>
        /// Login fail
        /// </summary>
        /// <param name="name"></param>
        public static void LoginFailed(string name)
        {
            //var ip = RequestUtility.IpClient();
            //var countIp = CheckIpValid();
            //new CacheUtility().SetValue(ip, string.Format("{0}|{1}", countIp + 1, DateTime.Now));
            var loginName = CheckLogin(name);
            new CacheUtility().SetValue(name, string.Format("{0}|{1}", loginName + 1, DateTime.Now));
        }

        /// <summary>
        /// Login fail
        /// </summary>
        /// <param name="name"></param>
        public static void IPLoginFaied()
        {
            var ip = RequestUtility.IpClient();
            var countIp = CheckIpValid();
            //new CacheUtility().SetValue(ip, string.Format("{0}|{1}", countIp + 1, 5, DateTime.Now));
            new CacheUtility().SetValue(ip, countIp + 1, 10);
        }

        /// <summary>
        /// Login fail
        /// </summary>
        /// <param name="name"></param>
        public static void UserLoginFailed(string name)
        {
            var loginName = CheckLogin(name);
            //new CacheUtility().SetValue(name, string.Format("{0}|{1}", loginName + 1, 30, DateTime.Now));
            new CacheUtility().SetValue(name, loginName + 1, 30);
        }

        /// <summary>
        /// Login fail
        /// </summary>
        /// <param name="name"></param>
        public static void BlockUserLoginFailed(string blockname)
        {
            new CacheUtility().SetValue(blockname, DateTime.Now.ToString());
        }

        /// <summary>
        /// ThangNN: Check trạng thái account bị block tạm
        /// </summary>
        /// <param name="blockname">Tên cached:BlockAccount+username </param>
        /// <param name="username">username</param>
        /// <returns></returns>
        public static bool CheckAccountBlock(string blockname, string username)
        {
            //CheckIpValid();
            var login = GetValue(blockname);
            //Get cache by name, ko tồn tại trả về false
            if (login == null) return false;
            DateTime date;
            //Giá trị của cached chính là thời gian accname bị block
            DateTime.TryParse(login.ToString(), out date);
            if (date.Year < 2011) return false;
            var exprize = DateTime.Now - date;
            //Nếu hết 15p thì remove cached block
            if (exprize.Minutes > 15)
            {
                new CacheUtility().Remove(blockname);
                new CacheUtility().Remove(CacheInvalidAccount+ username);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check tần suất post dữ liệu authen của cùng 1 địa chỉ IP
        /// </summary>
        /// <returns></returns>
        public static int CheckIPPostFrequency()
        {
            string ip = RequestUtility.IpClient();
            System.Runtime.Caching.ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;
            System.Runtime.Caching.CacheItemPolicy policy = new System.Runtime.Caching.CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddSeconds(3) };

            object cacheCounter = cache.Get("@Post" + ip.ToLower());// DataCaching.GetCache("@Post" + ip.ToLower());

            if (cacheCounter == null)
            {
                //DataCaching.InsertCache("@Post" + ip.ToLower(), 1, 0.001666);
                cache.Set("@Post" + ip.ToLower(), 1, policy);
                return 0;
            }

            //DataCaching.InsertCache("@Post" + ip.ToLower(), Convert.ToInt32(cacheCounter) + 1, 0.001666);
            cache.Set("@Post" + ip.ToLower(), Convert.ToInt32(cacheCounter) + 1, policy);
            NLogLogger.LogInfo(string.Format("IP:{0}\t{1}", ip, cacheCounter));
            return Convert.ToInt32(cacheCounter);
        }

        /// <summary>
        /// Login success
        /// </summary>
        /// <param name="name"></param>
        public static void LoginSuccess(string name)
        {
            new CacheUtility().Remove(name);
            //var ip = RequestUtility.IpClient();
            //new CacheUtility().Remove(ip);
        }

        public void SetValueByMinutesExpire(string name, object value, int minutesExpire)
        {
            HttpRuntime.Cache.Insert(name.Trim().ToLower(), value, null, DateTime.Now.AddMinutes(minutesExpire), TimeSpan.Zero, CacheItemPriority.High, null);
        }

        public void SetValueByMinutesExpired(string name, object value, double minutesExpire)
        {
            HttpRuntime.Cache.Insert(name.Trim().ToLower(), value, null, DateTime.Now.AddMinutes(minutesExpire), TimeSpan.Zero, CacheItemPriority.High, null);
        }
    }
}
