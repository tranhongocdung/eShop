using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using MVCWeb.Cores.Security;
using Newtonsoft.Json;

namespace MVCWeb
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                var newUser = new CustomPrincipal(authTicket.Name)
                {
                    UserId = serializeModel.UserId,
                    DisplayName = serializeModel.DisplayName,
                    Roles = serializeModel.Roles
                };
                HttpContext.Current.User = newUser;
            }
        }
    }
}
