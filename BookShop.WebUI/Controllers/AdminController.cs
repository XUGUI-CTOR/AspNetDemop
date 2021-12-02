using BookShop.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookShop.WebUI.Controllers
{
    [CustomAuth(false)]
    public class AdminController : Controller
    {
        public AdminController()
        {
            //base.ActionInvoker = new CustomActionInvoker();
        }
        // GET: Admin
        public ActionResult Index()
        {
            return Json(nameof(AdminController.Index), JsonRequestBehavior.AllowGet);
        }
        [Local]
        [ActionName("Index")]
        public ActionResult LocalIndex()
        {
            return Json(nameof(AdminController.LocalIndex),JsonRequestBehavior.AllowGet);
        }

        public ActionResult www() => null;



        //protected override void Execute(RequestContext requestContext)
        //{
        //    var action = requestContext.RouteData.Values["action"];
        //    requestContext.HttpContext.Response.Write(action);
        //}
    }
}