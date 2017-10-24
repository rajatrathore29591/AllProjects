using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace iCanSpeakAdminPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                //namespaces: new[] { "iCanSpeakAdminPortal.Controllers" }
                namespaces: new[] { "iCanSpeakAdminPortal.Controllers" }
            );
           // routes.MapRoute(
           //     "Default",
           //     "{controller}/{action}/{id}",
           //     new { controller = "Account", action = "Login", id = UrlParameter.Optional },
           //     new string[] { "iCanSpeakAdminPortal.Admin.Controllers" }
               
           //);
        }
    }
}