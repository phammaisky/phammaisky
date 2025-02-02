﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(DateTime?), new CustomDateModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime), new CustomDateModelBinder());
            ModelBinders.Binders.Add(typeof(double), new CustomDoubleModelBinder());
            ModelBinders.Binders.Add(typeof(double?), new CustomDoubleModelBinder());
        }
    }
}
