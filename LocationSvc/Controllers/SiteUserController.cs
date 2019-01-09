using LocationSvc.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LocationSvc.Controllers
{
    public class SiteUserController : BaseApiController
    {
        public void GetSiteUsers(string siteUrl)
        {
            // https://localhost:44300/api/siteuser?siteUrl=https://tektaniuminc.sharepoint.com/sites/spdev

            var us = TheUserSupport;
            var siteUsers = us.GetSiteUsers(siteUrl);

        }
    }
}
