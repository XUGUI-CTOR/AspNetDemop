using ASP.NET.MVC.Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET.MVC.Demo.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            Product myProduct = new Product
            {
                ProductID = 1,
                Name = "苹果",
                Description = "又大又红的苹果",
                Category = "水果",
                Price = 5.9M
            };
            return View(myProduct);
        }
    }
}