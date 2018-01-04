using DAL.IRepository;
using System.Web.Http;

namespace SMSGatewayAPI.Controllers
{
    public class HomeController : ApiController
    {
        private IDemo _iDemo;

        public HomeController(IDemo iDemo)
        {
            _iDemo = iDemo;
        }

        [HttpGet]
        public string Test()
        {
            return _iDemo.DemoDI();
        }
    }
}
