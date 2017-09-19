using System.Web.Mvc;
using System.Web.Routing;

namespace BankApi
{
    public static class WebApiConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "CreateAccount",
                "api/accounts/create",
                new { controller = "Accounts", action = "CreateAccount" });

            routes.MapRoute(
                "WithdrawFromAccount",
                "api/accounts/withdraw",
                new { controller = "Accounts", action = "Withdraw" });


            routes.MapMvcAttributeRoutes();
        }
    }
}
