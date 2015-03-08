using System.Web.Mvc;
using System.Web.Routing;
using BrianDennis.INFDTA01.Opdracht1.Services;

namespace BrianDennis.INFDTA01.Opdracht1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.UserItem);
            UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.UserItemEdited);
        }
    }
}
