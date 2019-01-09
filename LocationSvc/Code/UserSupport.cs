using LocationSvc.Models;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using Microsoft.SharePoint.Client.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LocationSvc.Code
{
    public class UserSupport
    {
        public object GetSiteUsers(string siteUrl)
        {
            List<Models.User> siteUsers = new List<Models.User>();
            string realm = ConfigurationManager.AppSettings["ida:Audience"];
            string appId = ConfigurationManager.AppSettings["ida:ClientId"];
            string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];

            OfficeDevPnP.Core.AuthenticationManager authManager = new OfficeDevPnP.Core.AuthenticationManager();
            using (ClientContext context = authManager.GetAppOnlyAuthenticatedContext(siteUrl, appId, appSecret))
            {
                try
                {
                    Web web = context.Web;
                    var users = context.LoadQuery(context.Web.SiteUsers.Where(u => u.PrincipalType == PrincipalType.User && u.UserId.NameIdIssuer == "urn:federation:microsoftonline"));
                    context.ExecuteQuery();
                    foreach (Microsoft.SharePoint.Client.User user in users)
                    {

                    }

                    return users;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}