using Datacle.BusLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web;

namespace Datacle.Controllers
{
    public class ReferenceController : ApiController
    {
        // GET api/reference
        public JsonResult Get()
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            var geturi = Request.RequestUri.ToString();
            if (geturi.Contains("ViewType"))
            {
                var service = new ViewService();
                return service.GetDisplayViewType();
            }
            if (geturi.Contains("ListType"))
            {
                var service = new ListService();
                return service.GetDisplayListType();
            }
            var userService = new UserService();
            return userService.GetDisplayUserType();
        }

        // GET api/reference/5
        public JsonResult Get(string type)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            switch (type)
            {
                case "ListType":
                    var lservice = new ListService();
                    return lservice.GetDisplayListType();
                case "ViewType":
                    var vservice = new ViewService();
                    return vservice.GetDisplayViewType();
            }
            return null;
        }

        // POST api/reference
        public JsonResult Post(TypeDisplay value)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            switch (value.type)
            {
                case "list":
                    var listservice = new ListService();
                    return listservice.AddListType(value);
                case "view":
                    var viewservice = new ViewService();
                    return viewservice.AddViewType(value);
                case "user":
                    var userservice = new UserService();
                    return userservice.AddUserType(value);
            }
            throw new Exception("type not found");
        }

        // PUT api/reference/5
        public JsonResult Put(Guid id, TypeDisplay value)
        {
            HttpContext.Current.Session["Login"] = User.Identity.Name;
            switch (value.type)
            {
                case "list":
                    var listservice = new ListService();
                    return listservice.UpdateListType(id, value);
                case "role":
                    var userservice = new UserService();
                    return userservice.UpdateUserType(id, value);
                case "view":
                    var viewservice = new ViewService();
                    return viewservice.AddViewType(value);
            }
            throw new Exception("type not found");
        }

        // DELETE api/reference/5
        public void Delete(Guid id)
        {
            var listservice = new ListService();
            listservice.DeleteListType(id);
            var userservice = new UserService();
            userservice.DeleteUserType(id);
            var viewservice = new ViewService();
            viewservice.DeleteViewType(id);
        }
    }
}
