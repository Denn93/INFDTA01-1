using System.Web.Mvc;
using System.Web.Routing;

namespace BrianDennis.INFDTA01.Opdracht1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("matrix", "Matrix/{action}", new { controller = "Matrix", action = "Index", view = "Matrix" });

            routes.MapRoute("userItem", "{view}/{action}", new { controller = "Main", action = "Index", view = "userItemCsv" });
        }
    }
}
