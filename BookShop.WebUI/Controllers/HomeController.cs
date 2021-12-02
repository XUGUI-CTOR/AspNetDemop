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
            string[] names = { "Apple", "Orange", "Pear" };
            return View(names);
        }
        [HandleError(ExceptionType = typeof(Exception),View = "RangeError")]
        public ActionResult RangeTest(int id)
        {
            if(id > 100)
                return Content($"This value is :{id}");
            throw new ArgumentOutOfRangeException("id", id, "{0}低于限定长度");
        }
        [ProfileAction]
        public string FilterTest()
        {
            return "This is the ActionFilterTest action";
        }
    }
}