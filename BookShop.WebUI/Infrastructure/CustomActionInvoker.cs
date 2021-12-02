﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShop.WebUI.Infrastructure
{
    public class CustomActionInvoker : IActionInvoker
    {
        public bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            if(actionName.ToLower() == "index")
            {
                controllerContext.HttpContext.Response.Write("This is output from the Index action");
                return true;
            }
            return false;
        }
    }
}