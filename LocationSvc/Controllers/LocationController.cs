using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using LocationSvc.Code;
namespace LocationSvc.Controllers
{
    // [Authorize]
    public class LocationController : ApiController
    {
        // https://localhost:44300/api/location?cityName=dc
        public Models.Location GetLocation(string cityName)
        {
            return new Models.Location() { Latitude = 10, Longitude = 20 };
        }

        public void GetCalendarItems(string siteUrl, string listName)
        {
            // https://localhost:44300/api/location?siteUrl=https://tektaniuminc.sharepoint.com/sites/spdev&listName=DemoVacationCalendar
            var cs = new CalendarSupport();
            var items = cs.GetCalendarItems(siteUrl, listName);
            
        }
        public void GetCalendarItemById(string siteUrl, string listName, int id)
        {
            // https://localhost:44300/api/location?siteUrl=https://tektaniuminc.sharepoint.com/sites/spdev&listName=DemoVacationCalendar&id=1
            var cs = new CalendarSupport();
            var items = cs.GetCalendarItemById(siteUrl, listName, id);

        }
    }
}