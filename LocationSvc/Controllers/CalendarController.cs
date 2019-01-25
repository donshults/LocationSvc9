
using LocationSvc.Code;
using LocationSvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LocationSvc.Controllers
{
    //[Authorize]
    public class CalendarController : BaseApiController
    {
        //[System.Web.Http.Authorize]
        [System.Web.Http.Route("api/calendar/timezones")]
        public HttpResponseMessage GetTimeZones()
        {
            var results = TheCalendarSupport.GetTimeZoneInfo();
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        public HttpResponseMessage Get(string siteUrl, string listName, string localTimeZone)
        {
            var results = TheCalendarSupport.GetCalendarItems(siteUrl, listName, localTimeZone);

            if (results != null)
            {
                results.ToList().Select(c => TheModelFactory.Create(c));
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Can't find calendar list");
            }

        }
        public HttpResponseMessage Get(string siteUrl, string listName, int id, string localTimeZone)
        {
            var results = TheModelFactory.Create(TheCalendarSupport.GetCalendarItemById(siteUrl, listName, id, localTimeZone));
            if (results != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Can't find calendar list");
            }
        }

        public HttpResponseMessage Post([FromBody] CalendarItemModel model)
        {
            try
            {
                var entity = TheModelFactory.Parse(model);
                if (entity == null)
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read Calendar Entry");
                //var calendarItem = TheCalendarSupport.GetCalendarItemById(calendarEntryModel.SiteUrl, calendarEntryModel.ListName, entity.Id);
                //if (calendarItem == null) Request.CreateResponse(HttpStatusCode.NotFound);
                var results = TheCalendarSupport.AddCalendarItem(entity);
                if (results == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data to Post");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Created, results);
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete([FromBody] CalendarItemModel model, string localTimeZone)
        {
            try
            {
                var exist = TheCalendarSupport.GetCalendarItemById(model.SiteUrl, model.ListName, model.ID, localTimeZone);
                var deleteRequest = false;
                if (exist == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                deleteRequest = TheCalendarSupport.DeleteCalendarItemById(model.SiteUrl, model.ListName, model.ID);

                if (deleteRequest)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
        public HttpResponseMessage Put([FromBody] CalendarItemModel model)
        {
            try
            {
                var entity = TheModelFactory.Parse(model);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data to Post");
                }
                else
                {
                    var results = TheCalendarSupport.UpdateCalendarItem(entity);
                    if (results == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data to Post");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Created, results);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
