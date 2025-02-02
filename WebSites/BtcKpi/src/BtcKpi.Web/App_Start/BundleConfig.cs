﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace BtcKpi.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Default - using bootstrap
            //bundles.Add(new ScriptBundle("~/bootstrap/js").Include("~/js/bootstrap.js", "~/js/site.js"));
            //bundles.Add(new StyleBundle("~/bootstrap/css").Include("~/css/bootstrap.css", "~/css/site.css"));
            #endregion

            #region BitexcoThemes style
            RegisterLayout(bundles);

            RegisterShared(bundles);

            RegisterAccount(bundles);

            RegisterHome(bundles);

            RegisterCharts(bundles);

            RegisterWidgets(bundles);

            RegisterElements(bundles);

            RegisterForms(bundles);

            RegisterTables(bundles);

            RegisterCalendar(bundles);

            RegisterMailbox(bundles);

            RegisterExamples(bundles);

            RegisterMainStyle(bundles);

            RegisterIpf(bundles);

            RegisterUpf(bundles);

            RegisterReport(bundles);

            RegisterPerformance(bundles);
            #endregion

            BundleTable.EnableOptimizations = true;
        }

        private static void RegisterDocumentation(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Documentation/menu").Include(
                "~/Scripts/Documentation/Documentation-menu.js"));
        }

        private static void RegisterExamples(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Examples/Blank/menu").Include(
                "~/Scripts/Examples/Blank-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Invoice/menu").Include(
                "~/Scripts/Examples/Invoice-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Lockscreen/menu").Include(
                "~/Scripts/Examples/Lockscreen-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Login").Include(
                "~/Scripts/Examples/Login.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Login/menu").Include(
                "~/Scripts/Examples/Login-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/P404/menu").Include(
                "~/Scripts/Examples/P404-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/P500/menu").Include(
                "~/Scripts/Examples/P500-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Pace").Include(
                "~/Scripts/Examples/Pace.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Pace/menu").Include(
                "~/Scripts/Examples/Pace-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/ProfileEx/menu").Include(
                "~/Scripts/Examples/ProfileEx-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Register").Include(
                "~/Scripts/Examples/Register.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Examples/Register/menu").Include(
                "~/Scripts/Examples/Register-menu.js"));
        }

        private static void RegisterMailbox(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Inbox").Include(
                "~/Scripts/Mailbox/Inbox.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Inbox/menu").Include(
                "~/Scripts/Mailbox/Inbox-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Compose").Include(
                "~/Scripts/Mailbox/Compose.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Compose/menu").Include(
               "~/Scripts/Mailbox/Compose-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Read/menu").Include(
                "~/Scripts/Mailbox/Read-menu.js"));
        }

        private static void RegisterCalendar(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Calendar").Include(
                "~/Scripts/Calendar/Calendar.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Calendar/menu").Include(
                "~/Scripts/Calendar/Calendar-menu.js"));
        }

        private static void RegisterTables(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Tables/Simple/menu").Include(
                "~/Scripts/Tables/Simple-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Tables/Data").Include(
                "~/Scripts/Tables/Data.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Tables/Data/menu").Include(
                "~/Scripts/Tables/Data-menu.js"));
        }

        private static void RegisterForms(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Forms/Advanced").Include(
                "~/Scripts/Forms/Advanced.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Forms/Advanced/menu").Include(
                "~/Scripts/Forms/Advanced-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Forms/Editors").Include(
                "~/Scripts/Forms/Editors.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Forms/Editors/menu").Include(
                "~/Scripts/Forms/Editors-menu.js"));


            bundles.Add(new ScriptBundle("~/Scripts/Forms/General/menu").Include(
                "~/Scripts/Forms/General-menu.js"));
        }

        private static void RegisterElements(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Elements/Buttons/menu").Include(
                "~/Scripts/Elements/Buttons-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Elements/General/menu").Include(
                "~/Scripts/Elements/General-menu.js"));

            bundles.Add(new StyleBundle("~/Styles/Elements/General").Include(
                "~/Styles/Elements/General.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Elements/Icons/menu").Include(
                "~/Scripts/Elements/Icons-menu.js"));

            bundles.Add(new StyleBundle("~/Styles/Elements/Icons").Include(
                "~/Styles/Elements/Icons.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Elements/Modals/menu").Include(
                "~/Scripts/Elements/Modals-menu.js"));

            bundles.Add(new StyleBundle("~/Styles/Elements/Modals").Include(
                "~/Styles/Elements/Modals.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Elements/Sliders").Include(
                "~/Scripts/Elements/Sliders.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Elements/Sliders/menu").Include(
                "~/Scripts/Elements/Sliders-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Elements/Timeline/menu").Include(
                "~/Scripts/Elements/Timeline-menu.js"));
        }

        private static void RegisterWidgets(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Widgets/menu").Include(
                "~/Scripts/Widgets/Widgets-menu.js"));
        }

        private static void RegisterCharts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Charts/ChartsJs").Include(
                "~/Scripts/Charts/ChartsJs.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Charts/ChartsJs/menu").Include(
                            "~/Scripts/Charts/ChartsJs-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Charts/Morris").Include(
                "~/Scripts/Charts/Morris.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Charts/Morris/menu").Include(
                "~/Scripts/Charts/Morris-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Charts/Flot").Include(
                "~/Scripts/Charts/Flot.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Charts/Flot/menu").Include(
                "~/Scripts/Charts/Flot-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Charts/Inline").Include(
                "~/Scripts/Charts/Inline.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Charts/Inline/menu").Include(
                "~/Scripts/Charts/Inline-menu.js"));
        }

        private static void RegisterHome(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Home/DashboardV1").Include(
                "~/Scripts/Home/DashboardV1.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Home/DashboardV1/menu").Include(
                "~/Scripts/Home/DashboardV1-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Home/DashboardV2").Include(
                "~/Scripts/Home/DashboardV2.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Home/DashboardV2/menu").Include(
                "~/Scripts/Home/DashboardV2-menu.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Home/Index").Include(
                "~/Scripts/Home/HomeIndex.js"));
        }

        private static void RegisterAccount(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Account/Login").Include(
                "~/Scripts/Account/Login.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Account/Register").Include(
                "~/Scripts/Account/Register.js"));
        }

        private static void RegisterShared(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Shared/_Layout").Include(
                "~/Scripts/Shared/_Layout.js"));
        }

        private static void RegisterLayout(BundleCollection bundles)
        {
            // bootstrap
            bundles.Add(new ScriptBundle("~/BitexcoThemes/bootstrap/js").Include(
                "~/Admin/bootstrap/js/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/bootstrap/css").Include(
                "~/Admin/bootstrap/css/bootstrap.min.css"));

            // dist
            bundles.Add(new ScriptBundle("~/BitexcoThemes/dist/js").Include(
                "~/Admin/dist/js/app.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/dist/css").Include(
                "~/Admin/dist/css/admin-lte.min.css"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/dist/css/skins").Include(
                "~/Admin/dist/css/skins/_all-skins.min.css"));

            // documentation
            bundles.Add(new ScriptBundle("~/BitexcoThemes/documentation/js").Include(
                "~/Admin/documentation/js/docs.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/documentation/css").Include(
                "~/Admin/documentation/css/style.css"));

            // plugins | bootstrap-slider
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/bootstrap-slider/js").Include(
                                        "~/Admin/plugins/bootstrap-slider/js/bootstrap-slider.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/bootstrap-slider/css").Include(
                                        "~/Admin/plugins/bootstrap-slider/css/slider.css"));

            // plugins | bootstrap-wysihtml5
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/bootstrap-wysihtml5/js").Include(
                                         "~/Admin/plugins/bootstrap-wysihtml5/js/bootstrap3-wysihtml5.all.min.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/bootstrap-wysihtml5/css").Include(
                                        "~/Admin/plugins/bootstrap-wysihtml5/css/bootstrap3-wysihtml5.min.css"));

            // plugins | chartjs
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/chartjs/js").Include(
                                         "~/Admin/plugins/chartjs/js/chart.min.js"));

            // plugins | ckeditor
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/ckeditor/js").Include(
                                         "~/Admin/plugins/ckeditor/js/ckeditor.js"));

            // plugins | colorpicker
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/colorpicker/js").Include(
                                         "~/Admin/plugins/colorpicker/js/bootstrap-colorpicker.min.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/colorpicker/css").Include(
                                        "~/Admin/plugins/colorpicker/css/bootstrap-colorpicker.css"));

            // plugins | datatables
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/datatables/js").Include(
                                         "~/Admin/plugins/datatables/js/jquery.dataTables.min.js",
                                         "~/Admin/plugins/datatables/js/dataTables.bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/datatables/css").Include(
                                        "~/Admin/plugins/datatables/css/dataTables.bootstrap.css"));

            // plugins | datepicker
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/datepicker/js").Include(
                                         "~/Admin/plugins/datepicker/js/bootstrap-datepicker.js",
                                         "~/Admin/plugins/datepicker/js/locales/bootstrap-datepicker*"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/datepicker/css").Include(
                                        "~/Admin/plugins/datepicker/css/datepicker3.css"));

            // plugins | daterangepicker
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/daterangepicker/js").Include(
                                         "~/Admin/plugins/daterangepicker/js/moment.min.js",
                                         "~/Admin/plugins/daterangepicker/js/daterangepicker.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/daterangepicker/css").Include(
                                        "~/Admin/plugins/daterangepicker/css/daterangepicker-bs3.css"));

            // plugins | fastclick
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/fastclick/js").Include(
                                         "~/Admin/plugins/fastclick/js/fastclick.min.js"));

            // plugins | flot
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/flot/js").Include(
                                         "~/Admin/plugins/flot/js/jquery.flot.min.js",
                                         "~/Admin/plugins/flot/js/jquery.flot.resize.min.js",
                                         "~/Admin/plugins/flot/js/jquery.flot.pie.min.js",
                                         "~/Admin/plugins/flot/js/jquery.flot.categories.min.js"));

            // plugins | font-awesome
            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/font-awesome/css").Include(
                                        "~/Admin/plugins/font-awesome/css/font-awesome.min.css"));

            // plugins | fullcalendar
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/fullcalendar/js").Include(
                                         "~/Admin/plugins/fullcalendar/js/fullcalendar.min.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/fullcalendar/css").Include(
                                        "~/Admin/plugins/fullcalendar/css/fullcalendar.min.css"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/fullcalendar/css/print").Include(
                                        "~/Admin/plugins/fullcalendar/css/print/fullcalendar.print.css"));

            // plugins | icheck
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/icheck/js").Include(
                                         "~/Admin/plugins/icheck/js/icheck.min.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/icheck/css").Include(
                                        "~/Admin/plugins/icheck/css/all.css"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/icheck/css/flat").Include(
                                        "~/Admin/plugins/icheck/css/flat/flat.css"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/icheck/css/sqare/blue").Include(
                                        "~/Admin/plugins/icheck/css/sqare/blue.css"));

            // plugins | input-mask
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/input-mask/js").Include(
                                         "~/Admin/plugins/input-mask/js/jquery.inputmask.js",
                                         "~/Admin/plugins/input-mask/js/jquery.inputmask.date.extensions.js",
                                         "~/Admin/plugins/input-mask/js/jquery.inputmask.extensions.js"));

            // plugins | ionicons
            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/ionicons/css").Include(
                                        "~/Admin/plugins/ionicons/css/ionicons.min.css"));

            // plugins | ionslider
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/ionslider/js").Include(
                                         "~/Admin/plugins/ionslider/js/ion.rangeSlider.min.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/ionslider/css").Include(
                                        "~/Admin/plugins/ionslider/css/ion.rangeSlider.css",
                                        "~/Admin/plugins/ionslider/css/ion.rangeSlider.skinNice.css"));

            // plugins | jquery
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/jquery/js").Include(
                                         "~/Admin/plugins/jquery/js/jQuery-2.1.4.min.js"));

            // plugins | jquery-validate
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/jquery-validate/js").Include(
                                         "~/Admin/plugins/jquery-validate/js/jquery.validate*"));

            // plugins | jquery-ui
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/jquery-ui/js").Include(
                                         "~/Admin/plugins/jquery-ui/js/jquery-ui.min.js"));

            // plugins | jvectormap
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/jvectormap/js").Include(
                                         "~/Admin/plugins/jvectormap/js/jquery-jvectormap-1.2.2.min.js",
                                         "~/Admin/plugins/jvectormap/js/jquery-jvectormap-world-mill-en.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/jvectormap/css").Include(
                                        "~/Admin/plugins/jvectormap/css/jquery-jvectormap-1.2.2.css"));

            // plugins | knob
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/knob/js").Include(
                                         "~/Admin/plugins/knob/js/jquery.knob.js"));

            // plugins | morris
            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/morris/css").Include(
                                        "~/Admin/plugins/morris/css/morris.css"));

            // plugins | momentjs
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/momentjs/js").Include(
                                         "~/Admin/plugins/momentjs/js/moment.min.js"));

            // plugins | pace
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/pace/js").Include(
                                         "~/Admin/plugins/pace/js/pace.min.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/pace/css").Include(
                                        "~/Admin/plugins/pace/css/pace.min.css"));

            // plugins | slimscroll
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/slimscroll/js").Include(
                                         "~/Admin/plugins/slimscroll/js/jquery.slimscroll.min.js"));

            // plugins | sparkline
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/sparkline/js").Include(
                                         "~/Admin/plugins/sparkline/js/jquery.sparkline.min.js"));

            // plugins | timepicker
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/timepicker/js").Include(
                                         "~/Admin/plugins/timepicker/js/bootstrap-timepicker.min.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/timepicker/css").Include(
                                        "~/Admin/plugins/timepicker/css/bootstrap-timepicker.min.css"));

            // plugins | raphael
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/raphael/js").Include(
                                         "~/Admin/plugins/raphael/js/raphael-min.js"));

            // plugins | select2
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/select2/js").Include(
                                         "~/Admin/plugins/select2/js/select2.full.min.js"));

            bundles.Add(new StyleBundle("~/BitexcoThemes/plugins/select2/css").Include(
                                        "~/Admin/plugins/select2/css/select2.min.css"));

            // plugins | morris
            bundles.Add(new ScriptBundle("~/BitexcoThemes/plugins/morris/js").Include(
                                         "~/Admin/plugins/morris/js/morris.min.js"));

        }

        private static void RegisterMainStyle(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Styles/Main").Include(
                "~/Styles/MainStyle.css"));
        }

        private static void RegisterIpf(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Ipf").Include(
                "~/Scripts/Ipf/Ipf.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Ipf/pending").Include(
                "~/Scripts/Ipf/Ipf-pending.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Ipf/done").Include(
                "~/Scripts/Ipf/Ipf-done.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Ipf/all").Include(
                "~/Scripts/Ipf/Ipf-all.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ipf/create").Include(
                "~/Scripts/Ipf/Ipf-create.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Ipf/edit").Include(
                "~/Scripts/Ipf/Ipf-update.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Ipf/approve").Include(
                "~/Scripts/Ipf/Ipf-approve.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Ipf/bodapprove").Include(
                "~/Scripts/Ipf/Ipf-bod-approve.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Ipf/view").Include(
                "~/Scripts/Ipf/Ipf-view.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Ipf/delete").Include(
                "~/Scripts/Ipf/Ipf-delete.js"));

            bundles.Add(new ScriptBundle("~/Scripts/IpfDone/edit").Include(
                "~/Scripts/Ipf/Ipf-done-edit.js"));
            bundles.Add(new ScriptBundle("~/Scripts/IpfDone/approve").Include(
                "~/Scripts/Ipf/Ipf-done-approve.js"));
            bundles.Add(new ScriptBundle("~/Scripts/IpfDone/view").Include(
                "~/Scripts/Ipf/Ipf-done-view.js"));
        }

        private static void RegisterUpf(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Department/list").Include(
                "~/Scripts/Department/Department-list.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Department/create").Include(
                "~/Scripts/Department/Department-create.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Upf").Include(
                "~/Scripts/Upf/Upf.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Upf/cross").Include(
                "~/Scripts/Upf/Upf-cross.js"));

            bundles.Add(new ScriptBundle("~/Scripts/UpfCross/create").Include(
                "~/Scripts/Upf/Upf-cross-create.js"));
            bundles.Add(new ScriptBundle("~/Scripts/UpfCross/edit").Include(
                "~/Scripts/Upf/Upf-cross-edit.js"));
            bundles.Add(new ScriptBundle("~/Scripts/UpfCross/approve").Include(
                "~/Scripts/Upf/Upf-cross-approve.js"));
            bundles.Add(new ScriptBundle("~/Scripts/UpfCross/view").Include(
                "~/Scripts/Upf/Upf-cross-view.js"));
        }

        private static void RegisterReport(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Report/Base").Include(
                "~/Scripts/Report/Report.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/IpfYear").Include(
                "~/Scripts/Report/Report-IpfYear.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/UpfMonth").Include(
                "~/Scripts/Report/Report-UpfMonth.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/UpfMonthView").Include(
                "~/Scripts/Report/Report-UpfMonthView.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/UpfYearView").Include(
                "~/Scripts/Report/Report-UpfYearView.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/UpfYear").Include(
                "~/Scripts/Report/Report-UpfYear.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/Upf").Include(
                "~/Scripts/Report/Report-Upf.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/UpfCross").Include(
                "~/Scripts/Report/Report-Upf-Cross.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/UpfCross/Year").Include(
                "~/Scripts/Report/Report-Upf-Cross-Year.js"));
        }

        private static void RegisterPerformance(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Performance/list").Include(
                "~/Scripts/Performance/Performance-list.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Performance/create").Include(
                "~/Scripts/Performance/Performance-create.js"));
        }
    }
}
