using Microsoft.AspNetCore.Http;
namespace UtilLibraries.IpAddress
{
    public class IPAddressHelper: IIPAddressHelper
    {
        private readonly IHttpContextAccessor _accessor;
        public IPAddressHelper(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public string ClientIP()
        {
            return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
