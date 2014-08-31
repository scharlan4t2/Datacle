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
    public class InfoAttribController : ApiController
    {
        // GET api/Attrib
        public JsonResult Get()
        {
            var service = new AttribService();
            return service.GetInfoAttribs();
        }

        // GET api/Attrib/5
        public JsonResult Get(Guid id)
        {
            var service = new AttribService();
            return service.GetInfoAttrib(id);
        }

        // POST api/Attrib
        public void Post(List<AttribInfo> value)
        {
            var service = new AttribService();
            foreach (var attrib in value)
            {
                service.UpdateAttrib((Guid)attrib.id, attrib);
            }
        }

        // PUT api/Attrib/5
        public void Put(Guid id, AttribInfo attrib)
        {
            var service = new AttribService();
            service.UpdateAttrib(id, attrib);
        }

        // DELETE api/Attrib/5
        public void Delete(Guid id)
        {
            var service = new AttribService();
            service.DeleteAttrib(id);
        }


    }
}
