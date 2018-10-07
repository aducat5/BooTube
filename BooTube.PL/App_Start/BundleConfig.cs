using System.Web;
using System.Web.Optimization;

namespace BooTube.PL
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            //Css for AdminLTE
            bundles.Add(new StyleBundle("~/adminLTE/css").Include(
                "~/dist/css/AdminLTE.min.css",
                "~/dist/css/skins/_all-skins.min.css"
                ));

            //BowerCSS including Bootstrap
            bundles.Add(new StyleBundle("~/bowers/css").Include(

                "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
                "~/bower_components/Ionicons/css/ionicons.min.css"

                ));

            //Scripts for AdminLTE
            bundles.Add(new ScriptBundle("~/bowers/js").Include(
                "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
                "~/dist/js/adminlte.min.js"
                ));
        }
    }
}
