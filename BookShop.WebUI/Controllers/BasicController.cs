using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookShop.WebUI.Controllers
{
    public class BasicController : IController
    {
        public virtual void Execute(RequestContext requestContext)
        {
            string controller = requestContext.RouteData.Values["controller"].ToString();
            string action = requestContext.RouteData.Values["action"].ToString();
            requestContext.HttpContext.Response.Write($"Controller:{controller},Action:{action}");
        }
    }
}