﻿using System.Web.Mvc;

namespace BtcKpi.Web.Controllers
{
    public class ChartsController : Controller
    {
        public ActionResult ChartJs()
        {
            return View();
        }

        public ActionResult Morris()
        {
            return View();
        }

        public ActionResult Flot()
        {
            return View();
        }

        public ActionResult Inline()
        {
            return View();
        }
    }
}