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
    public class ViewItemController : ApiController
    {
        // GET api/View
        public JsonResult Get()
        {
            var service = new ViewConnService();
            return service.GetDisplayViewConns();
        }

        // GET api/View/5
        public JsonResult Get(Guid id)
        {
            var service = new ViewConnService();
            return service.GetDisplayViewConns();
        }

        // POST api/View
        public void Post(ViewDisplay value)
        {
            value.id = Guid.NewGuid();
            var service = new ViewService();
            service.AddView(value);
        }

        // PUT api/View/5
        public void Put(int id, ViewDisplay value)
        {
        }

        // DELETE api/View/5
        public void Delete(Guid id)
        {
            var service = new ViewService();
            //service.DeleteViewConnection(id);
        }


    }
}
