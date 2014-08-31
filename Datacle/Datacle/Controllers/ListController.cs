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
    public class ListController : ApiController
    {
        // GET api/list
        public JsonResult Get()
        {
            var service = new ListService();
            return service.GetDisplayList();
        }

        // GET api/list/5
        public JsonResult Get(Guid id)
        {
            var service = new ListService();
            return service.GetDisplayList(id);
        }

        // POST api/list
        public JsonResult Post(ListDisplay value)
        {
            var service = new ListService();
            if (value.id == Guid.Empty) {
                value.id = Guid.NewGuid();
                return service.AddList(value);
            }
            else
            {
                return service.UpdateList((Guid)value.id, value);
            }
        }

        // PUT api/list/5
        public JsonResult Put(Guid id, ListDisplay value)
        {
            var service = new ListService();
            return service.UpdateList(id, value);
        }

        // DELETE api/list/5
        public void Delete(Guid id)
        {
            var service = new ListService();
            service.DeleteList(id);
        }

        private string login()
        {
            var login = "";
            if (HttpContext.Current.Session["Login"] != null)
                login = HttpContext.Current.Session["Login"].ToString();
            return login;
        }
    }
}
