
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
    [Authorize]
    public class CalendarController : BaseApiController
    {
        public CalendarController() : base()
        {
            string dumbName = null;
        }

        public HttpResponseMessage Get(string siteUrl, string listName)
        {
            var results = TheCalendarSupport.GetCalendarItems(siteUrl, listName)
                .ToList()
                .Select(c => TheModelFactory.Create(c));
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
        public HttpResponseMessage Get(string siteUrl, string listName, int id)
        {
            var results = TheModelFactory.Create(TheCalendarSupport.GetCalendarItemById(siteUrl, listName, id));
            return Request.CreateResponse(HttpStatusCode.OK, results);
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

        public HttpResponseMessage Delete([FromBody] CalendarItemModel model)
        {
            try
            {
                var exist = TheCalendarSupport.GetCalendarItemById(model.SiteUrl, model.ListName, model.ID);
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
                if(entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Data to Post");
                }
                else
                {
                    var results = TheCalendarSupport.UpdateCalendarItem(model);
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
}
