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
    public class ViewListController : ApiController
    {
        // GET api/ViewList
        public JsonResult Get()
        {
            var service = new ViewService();
            return service.GetDisplayViews();
        }

        // GET api/ViewList/5
        public JsonResult Get(Guid id)
        {
            var service = new ViewService();
            return service.GetDisplayView(id);
        }

        // POST api/ViewList
        public void Post(ViewDisplay value)
        {
            value.id = Guid.NewGuid();
            var service = new ViewService();
            service.AddView(value);
        }

        // PUT api/ViewList/5
        public void Put(int id, ViewDisplay value)
        {
        }

        // DELETE api/View/5
        public void Delete(Guid id)
        {
            var service = new ViewService();
            //service.DeleteViewList(id);
        }


    }
}
