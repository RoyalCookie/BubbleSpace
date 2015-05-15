using Bubblespace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace Bubblespace.Controllers
{
    public class EventController : Controller
    {
        [HttpPost]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase contentImage)
        {
            //05/16/2015
            events eventToAdd = new events();
            eventToAdd.event_description = collection["event-description"];
            eventToAdd.event_end_time = EventService.ParseDate(collection["end-time"]);
            eventToAdd.event_start_time = EventService.ParseDate(collection["start-time"]);
            eventToAdd.event_name = collection["event-name"];
            eventToAdd.event_end_time = EventService.ParseDate(collection["end-time"]);
            eventToAdd.FK_events_owner = UserService.GetUserByEmail(User.Identity.Name).Id;

            if (contentImage != null)
            {
                // Setting the image name
                eventToAdd.event_profile_image = FileUploadService.UploadImage(contentImage, "Events");
            }

            EventService.CreateEvent(eventToAdd);

            return RedirectToAction("Home", "Home");
        }
        [HttpPost]
        public ActionResult Events()
        { 
            var allEvents = EventService.GetAllEvents();
            var eventNames = (from eve in allEvents select eve.event_name).ToList();
            var eventImages = (from eve in allEvents select eve.event_profile_image).ToList();
            var eventId = (from eve in allEvents select eve.C_ID.ToString()).ToList();

            List<List<string>> returnJson = new List<List<string>>();
            returnJson.Add(eventNames);
            returnJson.Add(eventImages);
            returnJson.Add(eventId);

            return Json(returnJson);
        }
        [HttpPost]
        public ActionResult GetEventById(FormCollection collection)
        {
            events eve = EventService.GetEventById(Convert.ToInt32(collection["eventId"]));
            List<string> returnJson = new List<string>();
            returnJson.Add(eve.event_name);
            returnJson.Add(eve.event_description);
            returnJson.Add(eve.event_profile_image);
            returnJson.Add(eve.event_start_time.ToString());
            returnJson.Add(eve.event_end_time.ToString());

            return Json(returnJson);
        }
	}
}