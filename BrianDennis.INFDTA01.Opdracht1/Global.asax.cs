using System.Web.Mvc;
using System.Web.Routing;

namespace BrianDennis.INFDTA01.Opdracht1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
