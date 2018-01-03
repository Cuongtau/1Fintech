using System.Web.Optimization;

namespace PayWallet.PortalGateway
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootbox.min.js",
                      "~/Scripts/chosen.jquery.js",
                      "~/Scripts/datetimepicker/moment.js",
                      "~/Scripts/datetimepicker/bootstrap-datetimepicker.js",
                      "~/Scripts/respond.js",
                       "~/Scripts/validation/bootstrapValidator.js",
                        "~/Scripts/pdfmake.min.js",
                        "~/Scripts/vfs_fonts.js"));

            bundles.Add(new ScriptBundle("~/bundles/libjquery").Include(
                     "~/Scripts/utils.js",
                     "~/Scripts/common.js",
                     "~/Scripts/i18next-1.6.3.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/common.css",
                      "~/Content/bootstrap-chosen.css",
                      "~/Content/datetimepicker/bootstrap-datetimepicker.css",
                      "~/Content/Validation/bootstrapValidator.css"));
        }
    }
}
