using Datacle.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Datacle.BusLogic;
using Newtonsoft.Json;
using System.Web;

namespace Datacle.Controllers
{
    public class ViewController : ApiController
    {
        // GET api/View
        public JsonResult Get()
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var service = new ViewService();
            return service.GetDisplayViews();
        }

        // GET api/View/5
        public JsonResult Get(Guid id)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var service = new ViewService();
            return service.GetDisplayView(id);
        }

        // POST api/View
        public JsonResult Post(ViewDisplay value)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var service = new ViewService();
            if (value.id == Guid.Empty) { 
                return service.AddView(value);
            }
            else
            {
                return service.UpdateView((Guid)value.id, value);
            }
        }

        // PUT api/View/5
        public void Put(Guid id, ViewDisplay value)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var service = new ViewService();
            service.UpdateView(id, value);
        }

        // DELETE api/View/5
        public void Delete(Guid id)
        {
            var service = new ViewService();
            service.DeleteView(id);
        }


    }
}
