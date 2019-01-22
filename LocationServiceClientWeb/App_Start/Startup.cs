using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using System.Configuration;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Owin.Security;

[assembly: OwinStartup(typeof(LocationServiceClientWeb.App_Start.Startup))]
namespace LocationServiceClientWeb.App_Start
{
    public class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string appKey = ConfigurationManager.AppSettings["ida:AppSecret"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUrl"];

        private static string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        AuthenticationFailed = context =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Home/Error");
                            return Task.FromResult(0);
                        }
                    }

                });
        }
    }
}

