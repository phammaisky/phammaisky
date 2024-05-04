﻿using System.Web;
using System.Web.Optimization;

namespace AMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                                    "~/Scripts/jquery.validate*", "~/Scripts/jquery.unobtrusive-ajax.min.js", "~/Scripts/globalize/globalize.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/modalform").Include(
                      "~/Scripts/modalform.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                                "~/Scripts/bootstrap.js",
                                "~/Scripts/jquery-ui-1.11.2.js",
                                "~/Scripts/jquery.cookie.js",
                                "~/Scripts/jquery.timer.js",
                                "~/Scripts/bootstrap-datepicker.js",
                                "~/Scripts/locales/bootstrap-datepicker.vi.js",
                                "~/Scripts/respond.js",
                                "~/Scripts/DocSo.js"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
                        "~/Scripts/jquery.inputmask/jquery.inputmask.js",
                        "~/Scripts/jquery.inputmask/jquery.inputmask.extensions.js",
                        "~/Scripts/jquery.inputmask/jquery.inputmask.date.extensions.js",
                //and other extensions you want to include
                        "~/Scripts/jquery.inputmask/jquery.inputmask.numeric.extensions.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css", "~/Content/datepicker3.css",
                      "~/Content/site.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}