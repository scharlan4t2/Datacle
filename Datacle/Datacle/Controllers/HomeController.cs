using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Datacle.BusLogic;

namespace Datacle.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if( Request.IsAuthenticated){
                return RedirectToAction("Index", "View");
            }
            return View("Index");
        }
    }
}
