using System.Linq;
using PayWallet.Utils.Security;

namespace PayWallet.Utils
{
    public class AsignUtility
    {
        public static bool CheckAsign(string asignName, string asign)
        {
            var sessionasign = SessionUtility.GetValue(Contants.SESSION_ASIGN + asignName.ToLower());
            return sessionasign != null && sessionasign.ToString().Equals(asign.Trim());
        }

        public static string GetAsign(string asignName, string[] param)
        {
            var key = param.Aggregate(string.Empty, (current, s) => current + s);
            key = Encrypt.MD5(key);
            SessionUtility.SetValue(Contants.SESSION_ASIGN + asignName.ToLower(), key);
            return key;
        }
    }
}
