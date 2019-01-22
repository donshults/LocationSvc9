using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LocationServiceClientWeb.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {

        private static string audience = ConfigurationManager.AppSettings["ida:Audience"];
        private static string appKey = ConfigurationManager.AppSettings["ida:AppSecret"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];

        private static string authority = string.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
        private static AuthenticationContext authContext = new AuthenticationContext(authority);
        private static ClientCredential clientCredential = new ClientCredential(audience, appKey);

        private static string serviceResourceId =  ConfigurationManager.AppSettings["ida:ServiceResourceID"];
        private static string serviceBaseAddress = "https://localhost:44300/";

        // GET: Location
        public async Task<ActionResult> Index()
        {
            AuthenticationResult result = null;
            int retryCount = 0;
            bool retry = false;
            do
            {
                retry = false;
                try
                {
                    result = authContext.AcquireToken(serviceResourceId, clientCredential);
                }
                catch (AdalException ex)
                {
                    if(ex.ErrorCode == "temporarily_unavailable")
                    {
                        retry = true;
                        retryCount++;
                        Thread.Sleep(3000);
                    }
                 }

            } while ((retry == true) && (retryCount < 3));

            if(result == null)
            {
                ViewBag.ErrorMessage = "UnexpectedError";
                return View("Index");
            }
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, serviceBaseAddress + "api/location?cityName=dc");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string r = await response.Content.ReadAsStringAsync();
                ViewBag.Results = r;
                return View("Index");
            }
            else
            {
                string r = await response.Content.ReadAsStringAsync();
                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    authContext.TokenCache.Clear();
                }
                ViewBag.ErrorMessage = "AuthorizationRequired";
                return View("Index");
            }
            
        }
    }
}