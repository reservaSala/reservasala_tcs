using System.Web;
using System.Web.Optimization;

namespace ReservaSala
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                        "~/Scripts/dataTables.js",
                        "~/Scripts/dataTables-bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.timepicker").Include(
                        "~/Scripts/jquery.timepicker.js",
                        "~/Scripts/jquery.timepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                        "~/Scripts/fullcalendar.js",
                        "~/Scripts/daygrid.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepair").Include(
               "~/Scripts/jquery.datepair.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/sweetalert").Include(
                "~/Scripts/sweetalert.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/bundles/site").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/bundles/dataTables-css").Include(
                      "~/Content/dataTables.css"));


            bundles.Add(new StyleBundle("~/bundles/jquery.timepicker-css").Include(
                      "~/Content/jquery.timepicker.css"));

            bundles.Add(new StyleBundle("~/bundles/fullcalendar-css").Include(
                      "~/Content/fullcalendar.css",
                      "~/Content/daygrid.css"));

        }
    }
}
