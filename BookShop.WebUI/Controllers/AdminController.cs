using BookShop.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookShop.WebUI.Controllers
{
    public class AdminController : BasicController
    {
        // GET: Admin
        public ActionResult Index()
        {
            return null;
        }

        public ActionResult www() => null;

        public override void Execute(RequestContext requestContext)
        {
            var action = requestContext.RouteData.Values["action"];
            requestContext.HttpContext.Response.Write(action);
        }
    }
}