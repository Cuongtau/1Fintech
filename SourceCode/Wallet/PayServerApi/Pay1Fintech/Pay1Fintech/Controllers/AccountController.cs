using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using Pay1Fintech.Model.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UtilLibraries;
using UtilLibraries.Securities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Principal;
using Services.IService;
using DAL.Model;
using Microsoft.Extensions.Logging;
using UtilLibraries.IpAddress;
using Microsoft.AspNetCore.Http;

namespace Pay1Fintech.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _appConfiguration;
        private readonly IdentityOptions _identityOptions;
        private readonly IIPAddressHelper _ipHelper;

        private IAccountService _iAccountService;
        public AccountController(ILogger<AccountController> logger, IConfiguration appConfiguration, IOptions<IdentityOptions> identityOptions, IIPAddressHelper ipHelper, IAccountService iAccountService)
        {
            _logger = logger;
            _appConfiguration = appConfiguration;
            _identityOptions = identityOptions.Value;
            _iAccountService = iAccountService;
            _ipHelper = ipHelper;
        }

        /// <summary>
        /// Register account
        /// </summary>
        /// <remarks>This API will create an account</remarks>
        /// <param name="account">Object account, param at Data Type column</param>
        /// <returns></returns>
        [HttpPost]
        public RegisterResultModel Register([FromBody]Account account)
        {
            var responseData = new RegisterResultModel();
            try
            {
                //var result = HttpUtils.PostProxy(new { MerchantCode = "1", Amount = 9700 }, $"http://api.pay365.vn/paygate/api/Payment/GetBankPolicy");
                var ip = _ipHelper.ClientIP();
                _logger.LogInformation("123454");
                if (string.IsNullOrEmpty(account.AccountName))
                {
                    responseData.ResponseCode = CommonError.AccountNameEmpty;
                    responseData.Description = CommonError.Description(responseData.ResponseCode);
                    return responseData;
                }
                if (string.IsNullOrEmpty(account.Password))
                {
                    responseData.ResponseCode = CommonError.PasswordEmpty;
                    responseData.Description = CommonError.Description(responseData.ResponseCode);
                    return responseData;
                }
                if (string.IsNullOrEmpty(account.Email))
                {
                    responseData.ResponseCode = CommonError.EmailEmpty;
                    responseData.Description = CommonError.Description(responseData.ResponseCode);
                    return responseData;
                }
                if (string.IsNullOrEmpty(account.PhoneNumber))
                {
                    responseData.ResponseCode = CommonError.PhoneNumberEmpty;
                    responseData.Description = CommonError.Description(responseData.ResponseCode);
                    return responseData;
                }
                account.Password = PasswordHelper.HashPassword(account.Password);
                int response = _iAccountService.Register(account);
                responseData.ResponseCode = response;
                if (response > 0)
                {
                    responseData.Description = "Đăng ký tài khoản thành công";
                }
                responseData.Description = CommonError.Description(responseData.ResponseCode); ;
                return responseData;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, ex.Message);
                responseData.ResponseCode = -9999;
                responseData.Description = CommonError.Description(responseData.ResponseCode);
                return responseData;
            }
        }

        [HttpPost]
        public AuthenticateResultModel Authentication([FromBody]Account account)
        {

            var handler = new JwtSecurityTokenHandler();
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(account.AccountName, "TokenAuth"),
                new[] { new Claim("ID", "1") }
            );

            var configKey = _appConfiguration.GetValue<string>("Authentication:JwtBearer:SecurityKey");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appConfiguration.GetValue<string>("Authentication:JwtBearer:Issuer"),
                Audience = _appConfiguration.GetValue<string>("Authentication:JwtBearer:Audience"),
                SigningCredentials = creds,
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(30)
            });
            return new AuthenticateResultModel
            {
                AccessToken = handler.WriteToken(securityToken)
            };
            //return Ok(new { token = handler.WriteToken(securityToken) });
        }
        // GET api/values
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<string> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return new string[] { "value1", "value2" };
        }
    }
}
