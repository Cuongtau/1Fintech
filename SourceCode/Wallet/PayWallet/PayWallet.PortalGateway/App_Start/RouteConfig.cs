using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PayWallet.PortalGateway
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "PaymentWebsite",
                url: "checkout.html",
                defaults: new { controller = "Home", action = "CheckOutOrder", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "OrderQRCode",
                url: "orderqrcode.html",
                defaults: new { controller = "Home", action = "OrderQRCode", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "ThanhToanNutDonHang",
               url: "thanh-toan-nut-don-hang.html",
               defaults: new { controller = "Home", action = "ButtonOrder", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "PaymentButton",
               url: "order-payment-button.html",
               defaults: new { controller = "Home", action = "ButtonOrder", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "ConfirmOrderButton",
               url: "order-infomation.html",
               defaults: new { controller = "Home", action = "ConfirmOrder", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "XacNhanDonHang",
               url: "xac-nhan-don-hang.html",
               defaults: new { controller = "Home", action = "ConfirmOrder", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "BillElectric",
               url: "electronic-invoice.html",
               defaults: new { controller = "Home", action = "BillElectricPayment", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "HoaDonDienTu",
               url: "hoa-don-dien-tu.html",
               defaults: new { controller = "Home", action = "BillElectricPayment", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Paywallet",
                url: "paywallet.html",
                defaults: new { controller = "Home", action = "vtcpay_payment", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "onlinebanking",
                url: "onlinebanking.html",
                defaults: new { controller = "Home", action = "onlinebanking", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "bankdone",
                url: "bankdone",
                defaults: new { controller = "payportal", action = "bankdone", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "postbank",
               url: "post-bank.html",
               defaults: new { controller = "payportal", action = "PostToBank", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "resultView",
               url: "resultview.html",
               defaults: new { controller = "Home", action = "ResultPaymentView", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
