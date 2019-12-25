using System.Web;
using System.Web.Optimization;

namespace GreekHealthcareNetwork
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-migrate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/regnaLibs").Include(
                        "~/Scripts/RegnaScripts/libs/easing.min.js",
                        "~/Scripts/RegnaScripts/libs/wow.min.js",
                        "~/Scripts/RegnaScripts/libs/waypoints.min.js",
                        "~/Scripts/RegnaScripts/libs/counterup.min.js",
                        "~/Scripts/RegnaScripts/libs/hoverIntent.js",
                        "~/Scripts/RegnaScripts/libs/superfish.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/regnaScripts").Include(
                        "~/Scripts/RegnaScripts/main.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/animate.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/site.css"));
        }
    }
}
