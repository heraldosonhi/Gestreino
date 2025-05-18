using System.Web.Optimization;

namespace Gestreino
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/javascript").Include(
                      "~/Assets/javascript/jquery.min.js",
                      "~/Assets/javascript/jquery.min2-unobtrusive-ajax.min.js",
                      "~/Assets/javascript/bootstrap.bundle.min.js",
                      "~/Assets/javascript/toastr.min.js",
                      "~/Assets/javascript/dataTables.min.js",
                      "~/Assets/javascript/dataTables.checkboxes.min.js",
                       "~/Assets/javascript/moment.min.js",
                      "~/Assets/javascript/daterangepicker.min.js",
                       "~/Assets/lib/select2/select2.full.min.js",
                      "~/Assets/javascript/custom.js",
                      "~/Assets/javascript/application.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Assets/css/bootstrap.min.css",
                      "~/Assets/lib/select2/select2.min.css",
                      "~/Assets/css/custom.css",
                      "~/Assets/css/app.css"));
        }
    }
}
