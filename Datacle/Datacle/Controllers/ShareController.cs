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
    public class ShareController : ApiController
    {
        // GET api/user
        public JsonResult Get()
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var service = new ShareService();
            return service.GetDisplayShare();
        }
        // GET api/user/Shares
        public JsonResult Get(Guid id)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var service = new ShareService();
            return service.GetDisplayShareList(id);
        }
        // POST api/user
        public JsonResult Post(UserRequestDisplay value)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            value.id = Guid.NewGuid();
            var service = new ShareService();
            return service.AddShare(value);
        }
        // PUT api/user/5
        public JsonResult Put(Guid id, ShareDisplay value)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var service = new ShareService();
            return service.UpdateShare(id, value);
        }
        // DELETE api/user/5
        public void Delete(Guid id)
        {
            var service = new ShareService();
            service.DeleteShare(id);
        }
    }
}
