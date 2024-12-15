using System.Web;
using System.Web.Optimization;

namespace Gestreino
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Assets/javascript/jquery.min.js",
                      "~/Assets/javascript/jquery.min2-unobtrusive-ajax.min.js",
                      "~/Assets/javascript/bootstrap.bundle.min.js",
                      "~/Assets/javascript/toastr.min.js",
                      "~/Assets/javascript/custom.js",
                      "~/Assets/javascript/application.js",

                      "~/Assets/javascript/dataTables.min.js",
                      "~/Assets/javascript/dataTables.checkboxes.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Assets/css/bootstrap.min.css",
                       "~/Content/css/toastr.min.css",
                       "~/Content/css/dataTables.dataTables.min.css",
                      "~/Assets/css/custom.css",
                      "~/Assets/css/app.css"));
        }
    }
}
