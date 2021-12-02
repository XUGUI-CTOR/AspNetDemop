using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShop.WebUI.Infrastructure
{
    public class ProfileActionAttribute : FilterAttribute, IActionFilter
    {
        private Stopwatch watch;
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            watch.Stop();
            if(filterContext.Exception == null)
            {
                filterContext.HttpContext.Response.Write($"<div>Action method elapsed time : {watch.Elapsed.TotalMilliseconds}</div>");
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            watch = Stopwatch.StartNew();
        }
    }
}