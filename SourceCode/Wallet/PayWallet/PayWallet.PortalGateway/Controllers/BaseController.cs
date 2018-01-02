using System.Web.Mvc;

namespace PayWallet.PortalGateway.Controllers
{
    public class BaseController : Controller
    {
        protected override void ExecuteCore()
        {
            PayWallet.PortalGateway.Utils.LanguageHelper.SetLanguage();
            base.ExecuteCore();
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
    }
}