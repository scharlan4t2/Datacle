using Datacle.BusLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Datacle.Controllers
{
    public class WindowController : Controller
    {
        [Authorize]
        public ActionResult Index(string login, string name)
        {
            ViewData["Group"] = "Public";
            var username = User.Identity.Name;
            ViewData["Login"] = username;
            ViewData["View"] = name;
            HttpContext.Session["Login"] = Guid.Empty;
            if (login != null && login != "Login")
            {
                var service = new UserService();
                HttpContext.Session["Login"] = service.GetUserID(username);
            }
            if (name != null && name != "Views")
            {
                var service = new ViewService();
                var views = service.Views();
                var view = views.First(v => v.title == name);
                ViewData["id"] = view.id.ToString();
                return View("UserView");
            }
            if (login != null && login != "Login")
            {
                if (HttpContext.Session["Login"].Equals(Guid.Empty))
                {
                    return View("Index");
                }
                return View("UserViews");
            }
            return View("Index");
        }
        public ActionResult Group(string group, string login, string name)
        {
            ViewData["Group"] = group;
            ViewData["Login"] = login;
            ViewData["View"] = name;
            if (name != null && name != "Views")
            {
                return View("UserView");
            }
            if (login != null && login != "Login")
            {
                return View("UserViews");
            }
            return View("Index");
        }
        public PartialViewResult ViewWindow()
        {
            ViewData["Login"] = RouteData.Values["login"].ToString();
            return PartialView();
        }
    }
}
