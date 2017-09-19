using BankApi.App_Start;
using System.Web.Http;
using System.Web.Routing;

namespace BankApi
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            WebApiConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}