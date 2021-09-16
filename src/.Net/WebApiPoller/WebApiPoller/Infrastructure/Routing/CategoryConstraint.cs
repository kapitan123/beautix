using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using WebApiPoller.Entities;

namespace WebApiPoller.Infrastructure.Routing
{
    public class CategoryConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // retrieve the candidate value
            var candidate = values[routeKey]?.ToString();
            // attempt to parse the candidate to the required Enum type, and return the result
            return Enum.TryParse(candidate, out Category result);
        }
    }
}
