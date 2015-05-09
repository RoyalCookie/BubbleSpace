
using Bubblespace.Services;
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

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult PostToEvent()
        {
            return View();
        }

        public ActionResult Join()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Events()
        { 
            // For now this returns all users, not just friends.
            // We need to change this to accept an id and return only friends. - Andri
            EventService es = new EventService();

            var allEvents = es.GetAllEvents();
            var eventNames = (from eve in allEvents select eve.event_name).ToList();
            var eventImages = (from eve in allEvents select eve.event_profile_image).ToList();

            List<List<string>> returnJson = new List<List<string>>();
            returnJson.Add(eventNames);
            returnJson.Add(eventImages);
            return Json(returnJson);
        }
	}
}