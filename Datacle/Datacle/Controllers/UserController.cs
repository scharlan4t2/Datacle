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
    public class UserController : ApiController
    {
        // GET api/user
        public JsonResult Get()
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var service = new UserService();
            return service.GetDisplayUser();
        }
        // GET api/user/Shares
        public JsonResult Get(Guid id)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var service = new UserService();
            return service.GetDisplayShare();
        }
        // POST api/user
        public JsonResult Post(UserDisplay value)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            value.id = Guid.NewGuid();
            var service = new UserService();
            return service.AddUser(value);
        }
        // PUT api/user/5
        public JsonResult Put(Guid id, UserDisplay value)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var service = new UserService();
            return service.UpdateUser(id, value);
        }
        // DELETE api/user/5
        public void Delete(Guid id)
        {
            var service = new UserService();
            service.DeleteUser(id);
        }
    }
}
