using BookShop.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookShop.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("NewRoute", "App/{action}", new { controller = "Home" });
            //routes.MapRoute("defaultRoute", "{controller}/{action}/{Id}/{*catchall}",new {controller="home",action="Index",Id=UrlParameter.Optional });
            routes.MapRoute("defaultRoute", "{controller}/{action}/{Id}", new { controller = "home", action = "Index", Id = UrlParameter.Optional },new {controller="^H.*", custom = new UserAgentConstraint("Chrome") });
            //"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.77 Safari/537.36"
        }
    }
}
