using BookShop.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            if (Server.MachineName == "DESKTOP-8DIP9B2")
            {
                return new CustomRedirectResult("Basic/Index");
            }
            else
            {
                Response.Write(Server.MachineName);
                return null;
            }
        }
    }
}