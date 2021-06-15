using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShop.WebUI.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index(string id)
        {
            ViewBag.Controller = "Admin";
            ViewBag.Action = "Index";
            ViewBag.Id = id;
            return View("ActionName");
        }
    }
}