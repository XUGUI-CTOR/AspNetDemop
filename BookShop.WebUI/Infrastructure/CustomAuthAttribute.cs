using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace BookShop.WebUI.Infrastructure
{
    public class CustomAuthAttribute : AuthorizeAttribute
    {
        private readonly bool allowedParam;

        public CustomAuthAttribute(bool allowedParam)
        {
            this.allowedParam = allowedParam;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!CustomAuthorize(httpContext))
            {
                httpContext.Response.StatusCode = 404;
                httpContext.Response.Write("this request is not allowed");
                return false;
            }
            return true;
        }

        private bool CustomAuthorize(HttpContextBase httpContext)
        {
            if (httpContext.Request.IsLocal)
                return allowedParam;
            return false;
        }
    }
}