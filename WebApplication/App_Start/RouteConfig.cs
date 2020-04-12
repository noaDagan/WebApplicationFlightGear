using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Exersice3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Second",
                url: "{action}/{ip}/{port}/{time}",
                defaults: new { controller = "Products", action = "display", time = "0" }
            );
            routes.MapRoute(
                name: "Save",
                url: "{action}/{ip}/{port}/{time}/{timeOut}/{name}",
                defaults: new { controller = "Products" }
            );
            routes.MapRoute(
                        name: "Default",
                        url: "{action}/{id}",
                        defaults: new { controller = "Products", action = "Index", id = UrlParameter.Optional }
                    );


        }
    }
}
