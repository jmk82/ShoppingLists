using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ostoslista
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "",
                url: "lists/{id}",
                defaults: new { controller = "ShoppingLists", action = "ViewList" });

            routes.MapRoute(
                name: "",
                url: "lists",
                defaults: new { controller = "ShoppingLists", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index" , id = UrlParameter.Optional }
            );
        }
    }
}
