using System.Web.Mvc;
using System.Web.Routing;

namespace BrianDennis.INFDTA01.Opdracht1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "{view}/{action}", new { controller = "Main", action = "Index", view = "UserItem"});

        }
    }
}
