using System.Web;
using System.Web.Optimization;

namespace WebApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        //<!-- jQuery 3 -->
                        "~/Content/AdminLTE-2.4.5/bower_components/jquery/dist/jquery.min.js",
                        //<!-- jQuery UI 1.11.4 -->
                        "~/Content/AdminLTE-2.4.5/bower_components/jquery-ui/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jlibrary").Include(
                        //<!-- Bootstrap 3.3.7 -->
                        "~/Content/AdminLTE-2.4.5/bower_components/bootstrap/dist/js/bootstrap.min.js",
                        //<!-- Morris.js charts -->
                        "~/Content/AdminLTE-2.4.5/bower_components/raphael/raphael.min.js",
                        "~/Content/AdminLTE-2.4.5/bower_components/morris.js/morris.min.js",
                        //<!-- Sparkline -->
                        "~/Content/AdminLTE-2.4.5/bower_components/jquery-sparkline/dist/jquery.sparkline.min.js",
                        //<!-- jvectormap -->
                        "~/Content/AdminLTE-2.4.5/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                        "~/Content/AdminLTE-2.4.5/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                        //<!-- jQuery Knob Chart -->
                        "~/Content/AdminLTE-2.4.5/bower_components/jquery-knob/dist/jquery.knob.min.js",
                        //<!-- daterangepicker -->
                        "~/Content/AdminLTE-2.4.5/bower_components/moment/min/moment.min.js",
                        "~/Content/AdminLTE-2.4.5/bower_components/bootstrap-daterangepicker/daterangepicker.js",
                        //<!-- datepicker -->
                        "~/Content/AdminLTE-2.4.5/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
                        //<!-- Bootstrap WYSIHTML5 -->
                        "~/Content/AdminLTE-2.4.5/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                        //<!-- Slimscroll -->
                        "~/Content/AdminLTE-2.4.5/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                        //<!-- FastClick -->
                        "~/Content/AdminLTE-2.4.5/bower_components/fastclick/lib/fastclick.js",
                        //<!-- AdminLTE App -->
                        "~/Content/AdminLTE-2.4.5/dist/js/adminlte.min.js",
                        //<!-- AdminLTE dashboard demo (This is only for demo purposes) -->
                        "~/Content/AdminLTE-2.4.5/dist/js/pages/dashboard.js",
                        //<!-- AdminLTE for demo purposes -->
                        "~/Content/AdminLTE-2.4.5/dist/js/demo.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        //<!-- Bootstrap 3.3.7 -->
                        "~/Content/AdminLTE-2.4.5/bower_components/bootstrap/dist/css/bootstrap.min.css",
                        //<!-- Font Awesome -->
                        "~/Content/AdminLTE-2.4.5/bower_components/font-awesome/css/font-awesome.min.css",
                        //<!-- Ionicons -->
                        "~/Content/AdminLTE-2.4.5/bower_components/Ionicons/css/ionicons.min.css",
                        //<!-- Theme style -->
                        "~/Content/AdminLTE-2.4.5/dist/css/AdminLTE.min.css",
                        //<!-- AdminLTE Skins. Choose a skin from the css/skins
                        //     folder instead of downloading all of them to reduce the load. -->
                        "~/Content/AdminLTE-2.4.5/dist/css/skins/_all-skins.min.css",
                        //<!-- Morris chart -->
                        "~/Content/AdminLTE-2.4.5/bower_components/morris.js/morris.css",
                        //<!-- jvectormap -->
                        "~/Content/AdminLTE-2.4.5/bower_components/jvectormap/jquery-jvectormap.css",
                        //<!-- Date Picker -->
                        "~/Content/AdminLTE-2.4.5/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css",
                        //<!-- Daterange picker -->
                        "~/Content/AdminLTE-2.4.5/bower_components/bootstrap-daterangepicker/daterangepicker.css",
                        //<!-- bootstrap wysihtml5 - text editor -->
                        "~/Content/AdminLTE-2.4.5/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"));

        }
    }
}
