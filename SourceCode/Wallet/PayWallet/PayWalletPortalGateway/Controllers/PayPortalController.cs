﻿using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PayWallet.PortalGateway.Controllers.Utils;
using PayWallet.Utils;

namespace PayWallet.PortalGateway.Controllers
{
    public class PayPortalController : Controller
    {

        public ActionResult VerifyBankNoDirect(string BankCode)
        {
            ViewBag.BankCode = BankCode;
            return PartialView();
        }

        public ActionResult pup_ConfirmCodeBank(string orderId, string sign)
        {
            LanguageHelper.GetCurrentLanguage();
            ViewBag.OrderId = orderId;
            ViewBag.Sign = sign;
            return PartialView();
        }

        private bool ValidateRequestHeader(HttpRequestBase request)
        {
            string cookieToken = string.Empty;
            string formToken = string.Empty;
            try
            {
                string tokenValue = request.Headers["VerificationToken"]; // read the header key and validate the tokens.
                if (!string.IsNullOrEmpty(tokenValue))
                {
                    string[] tokens = tokenValue.Split(':');
                    if (tokens.Length == 2)
                    {
                        cookieToken = tokens[0].Trim();
                        formToken = tokens[1].Trim();
                    }
                }
                AntiForgery.Validate(cookieToken, formToken); // this validates the request token.
                return true;
            }
            catch (Exception exception)
            {
                NLogLogger.PublishException(exception);
                return false;
            }
        }

    }



}