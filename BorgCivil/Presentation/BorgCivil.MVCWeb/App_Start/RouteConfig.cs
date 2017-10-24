using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BorgCivil.MVCWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //enabling attribute routing
            routes.MapMvcAttributeRoutes();

            // Localization route - it will be used as a route of the first priority 
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            //    //defaults: new { controller = "CustomerManagement", action = "Login", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
               name: "Default",
               url: "index.html"
           );

        }
    }
}
