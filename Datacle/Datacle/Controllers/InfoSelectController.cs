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
    public class InfoSelectController : ApiController
    {
        // GET api/Desc
        public JsonResult Get()
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var service = new SelectService();
            return service.GetInfoSelects();
        }

        // GET api/Desc/5
        public JsonResult Get(Guid id)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var service = new SelectService();
            return service.GetInfoSelect(id);
        }

        // POST api/Desc
        public void Post(SelectInfo value)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            SelectService.AddSelect(value);
        }

        // PUT api/Desc/5
        public void Put(int id, SelectInfo value)
        {
        }

        // DELETE api/Desc/5
        public void Delete(Guid id)
        {
            var service = new SelectService();
            service.DeleteSelect(id);
        }


    }
}
