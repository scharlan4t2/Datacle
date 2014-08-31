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
    public class ListItemController : ApiController
    {
        // GET api/list
        public JsonResult Get()
        {
            var service = new ListItemService();
            return service.GetDisplayListItems();
        }

        // GET api/list/5
        public JsonResult Get(Guid id)
        {
            var service = new ListItemService();
            return service.GetDisplayListItem(id);
        }

        // POST api/list
        public JsonResult Post(ListItemDisplay value)
        {
            value.id = Guid.NewGuid();
            var service = new ListItemService();
            return service.AddListItem(value);
        }

        // PUT api/list/5
        public JsonResult Put(Guid id, ListItemDisplay value)
        {
            var service = new ListItemService();
            return service.UpdateListItem(id, value);
        }

        // DELETE api/list/5
        public JsonResult Delete(Guid id)
        {
            var service = new ListItemService();
            return service.DeleteListItem(id);
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
