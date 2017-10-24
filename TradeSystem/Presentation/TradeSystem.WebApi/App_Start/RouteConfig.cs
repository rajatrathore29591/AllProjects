using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TradeSystem.MVCWeb
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
            //    name: "DefaultLocalized",
            //    url: "{lang}/{controller}/{action}/{id}",
            //    defaults: new
            //    {
            //        controller = "Home",
            //        action = "Index",
            //        id = UrlParameter.Optional,
            //        lang = "en"
            //    });

            //routes.MapRoute(
            //         name: "Localization", // Route name
            //         url: "{lang}/{controller}/{action}/{id}", // URL with parameters
            //         defaults: new { controller = "Home", action = "ChangeLanguage", id = UrlParameter.Optional,  lang = "en" } // Parameter defaults

            //            );



            ///Code for company user 

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Account", action = "Admin", id = UrlParameter.Optional }
            //);

            ///Code for customer login (Module)
            routes.MapRoute(
            "admin",
                 "{action}",
                //defaults: new { controller = "Account", action = "Admin", id = UrlParameter.Optional }
                 new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "Account", action = "Admin", id = UrlParameter.Optional }
                defaults: new { controller = "CustomerManagement", action = "Login", id = UrlParameter.Optional }
            );

           

        }
    }
}
