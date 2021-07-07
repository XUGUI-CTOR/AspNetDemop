using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace BookShop.WebUI.Infrastructure
{
    public class UserAgentConstraint : IRouteConstraint
    {
        private readonly string agentParam;
        public UserAgentConstraint(string agentParam)
        {
            this.agentParam = agentParam;
        }
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return httpContext.Request.UserAgent != null && httpContext.Request.UserAgent.Contains(agentParam);
        }
    }
}