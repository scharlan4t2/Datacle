using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.WebHost;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Datacle
{
    public class SessionableControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionableControllerHandler(RouteData routeData)
            : base(routeData)
        { }
    }
    public class SessionStateRouteHandler : IRouteHandler
    {
        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return new SessionableControllerHandler(requestContext.RouteData);
        }
    }  
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Views",
                url: "View/{login}/{name}",
                defaults: new { controller = "Window", login = "Login", action = "Index", name = "Views" }
            );
            routes.MapRoute(
                name: "Account",
                url: "Account/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "API",
                url: "API/{controller}/{action}/{id}",
                defaults: new { controller = "List", action = "Get", id = UrlParameter.Optional }
            ).RouteHandler = new SessionStateRouteHandler();
            routes.MapRoute(
                name: "Group",
                url: "Group/{group}/{login}/{name}",
                defaults: new { controller = "Window", action = "Group", group = "Index", login = "Login", name = "Views" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}