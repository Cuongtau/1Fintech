using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Security;

namespace Utilities
{
    public class Config
    {
        public static string SmsGatewayConnectionString => GetConnectionString("SmsGatewayConnectionString");

        private static string GetConnectionString(string nameConnection)
        {
            return ConfigurationManager.ConnectionStrings[nameConnection] == null ? string.Empty : new RijndaelEnhanced("smsGateway", "@1B2c3D4e5F6g7H8").Decrypt(ConfigurationManager.ConnectionStrings[nameConnection].ConnectionString);
        }

        public static string GetAppSetting(string appSettingName)
        {
            return ConfigurationManager.AppSettings[appSettingName] ?? string.Empty;
        }

    }
}
