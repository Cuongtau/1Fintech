using System.Web.Mvc;

namespace PayWallet.PortalGateway.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        //400
        public ActionResult ErrorBadRequest()
        {
            return PartialView();
        }

        //401
        public ActionResult ErrorUnAuthorize()
        {
            return PartialView();
        }

        //403
        public ActionResult ErrorPermission()
        {
            return PartialView();
        }
        //404
        public ActionResult ErrorNotPage()
        {
            return PartialView();
        }
        //500
        public ActionResult ErrorInternalServer()
        {
            return PartialView();
        }
    }
}