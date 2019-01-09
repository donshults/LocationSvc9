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
    public abstract class BaseApiController : ApiController
    {
        private CalendarSupport calendarSupport;
        private ModelFactory modelFactory;
        private UserSupport userSupport;

        public BaseApiController()
        {
            calendarSupport = new CalendarSupport();
            userSupport = new UserSupport();
        }

        protected UserSupport TheUserSupport
        {
            get
            {
                return userSupport;
            }
        }
        protected CalendarSupport TheCalendarSupport
        {
            get
            {
                return calendarSupport;
            }
        }
        protected ModelFactory TheModelFactory {
            get
            {
                if(modelFactory == null)
                {
                    modelFactory = new ModelFactory(this.Request);
                }
                return modelFactory;
            }
        }

    }
}
