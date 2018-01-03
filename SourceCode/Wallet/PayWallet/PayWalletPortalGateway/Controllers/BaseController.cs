using System.Web.Mvc;
using PayWallet.PortalGateway.Controllers.Utils;

namespace PayWallet.PortalGateway.Controllers
{
    public class BaseController : Controller
    {
        protected override void ExecuteCore()
        {
            LanguageHelper.SetLanguage();
            base.ExecuteCore();
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
    }
}