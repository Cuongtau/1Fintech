using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace PayWallet.Utils
{
    public class Telco
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }

        /// <summary>
        /// Lấy ra danh sách telco
        /// </summary>
        /// <returns></returns>
        public static List<Telco> GetList()
        {
            var json = System.Configuration.ConfigurationManager.AppSettings["TELCO"];
            var telcos = new JavaScriptSerializer().Deserialize<IList<Telco>>(json);
            var list = (from telco in telcos
                        let arr = telco.Prefix.Split(',')
                        from s in arr
                        select new Telco {Code = telco.Code, Name = telco.Name, Prefix = s }).ToList();

            return list;
        }

        /// <summary>
        /// Lấy ra telco từ prefix
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static Telco Get(string prefix)
        {
            return GetList().Find(t => t.Prefix == prefix);
        }

        /// <summary>
        /// Kiểm tra số điện thoại hợp lệ
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>
        /// true: Hợp lệ
        /// false: Không hợp lệ
        /// </returns>
        public static bool CheckPhoneNumber(string phoneNumber)
        {
            try
            {
                NLogLogger.LogInfo("phoneNumber: " + phoneNumber + "|phoneNumberLength:" + phoneNumber.Length);
                if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 10 || phoneNumber.Length > 11)
                    return false;

                if (!Config.IsNumber(phoneNumber))
                    return false;

                var prefix = phoneNumber.Substring(0, phoneNumber.Length - 7);
                NLogLogger.LogInfo("prefix:" + prefix);
                var listTel = GetList();
                NLogLogger.LogInfo("ListTelco:" + new JavaScriptSerializer().Serialize(listTel));
                return GetList().Exists(t => t.Prefix == prefix);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return false;
            }
        }

        public static void CheckGtelAccountRequest(string accountName)
        {
            if (string.IsNullOrEmpty(accountName) || accountName.Length < 10)
            {
                NLogLogger.LogInfo("sdt ko dung dinh dang:" + accountName);
                return;
            }

            var prefix = accountName.Substring(0, accountName.Trim().Length - 7);

            var telco = Telco.Get(prefix);

            if (telco == null)
            {
                NLogLogger.LogInfo("prefix:" + prefix + "telco == null");
                return;
            }

            if (telco.Code.ToUpper() != "BEELINE" && telco.Code.ToUpper() != "GTEL")
            {
                NLogLogger.LogInfo("telco.Code.ToUpper():" + telco.Code.ToUpper());
                return;
            }
            var msidns = accountName;
            var active = 1; // trạng thái kích hoạt
            var extrafeild = "Vi dien tu VTC Pay " + accountName + " dang ky thanh cong, kich hoat dich vu gtel";
            var result = new GtelAccountRequest().registerAccountGTEl(msidns, active, extrafeild);
            NLogLogger.LogInfo("registerAccountGTEl-->msidns:" + msidns + "|active:" + active + "|extrafeild:" + extrafeild + "|result" + result);
        }
    }

    public class TelcoCard
    {
        public string Code { get; set; }
        public int Cate { get; set; }

        public static List<TelcoCard> GetList()
        {
            var json = System.Configuration.ConfigurationManager.AppSettings["TELCO.CARD"];
            var telcoCards = new JavaScriptSerializer().Deserialize<IList<TelcoCard>>(json);
            //var list = (from telcoCard in telcoCards
            //            select new TelcoCard { Code = telcoCard.Code, Cate = telcoCard.Cate }).ToList();
            
            //return list;
            return telcoCards.ToList();
        }

        public static TelcoCard Get(string code)
        {
            return GetList().Find(tc => tc.Code == code);
        }
    }

    public class TelcoPrepaid
    {
        public string Code { get; set; }
        public int Cate { get; set; }

        public static List<TelcoPrepaid> GetList()
        {
            var json = System.Configuration.ConfigurationManager.AppSettings["TELCO.PREPAID"];
            var telcoPrepaids = new JavaScriptSerializer().Deserialize<IList<TelcoPrepaid>>(json);
            return telcoPrepaids.ToList();
        }

        public static TelcoPrepaid Get(string code)
        {
            return GetList().Find(tp => tp.Code == code);
        }
    }

    public class TelcoPostpaid
    {
        public string Code { get; set; }
        public int Cate { get; set; }

        public static List<TelcoPostpaid> GetList()
        {
            var json = System.Configuration.ConfigurationManager.AppSettings["TELCO.POSTPAID"];
            var telcoPostpaids = new JavaScriptSerializer().Deserialize<IList<TelcoPostpaid>>(json);
            return telcoPostpaids.ToList();
        }

        public static TelcoPostpaid Get(string code)
        {
            return GetList().Find(tp => tp.Code == code);
        }
    }

    public class TelcoSMS
    {
        public string Code { get; set; }

        public static List<TelcoSMS> GetList()
        {
            var json = System.Configuration.ConfigurationManager.AppSettings["TELCO.SMS"];
            var telcoSMSs = new JavaScriptSerializer().Deserialize<IList<TelcoSMS>>(json);
            //return telcoSMSs.ToList();

            var list = (from ts in telcoSMSs
                        let arr = ts.Code.Split(',')
                        from c in arr
                        select new TelcoSMS { Code = c }).ToList();
            return list;
        }

        public static TelcoSMS Get(string code)
        {
            return GetList().Find(ts => ts.Code == code);
        }
    }
}
