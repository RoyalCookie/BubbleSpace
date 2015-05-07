
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Controllers
{
    public class EventController : Controller
    {
        //
        // GET: /Event/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateEvent()
        {
            return View();
        }

        public ActionResult PostToEvent()
        {
            return View();
        }

        public ActionResult JoinEvent()
        {
            return View();
        }

        public ActionResult EditEvent()
        {
            return View();
        }

        public ActionResult DeleteEvent()
        {
            return View();
        }
	}
}