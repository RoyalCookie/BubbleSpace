
using Bubblespace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        [HttpPost]
        public ActionResult Create(FormCollection fc, HttpPostedFileBase contentImage)
        {
            events ev = new events();
            ev.event_description = fc["event-description"];
            ev.event_end_time = Convert.ToDateTime(fc["end-time"]);
            ev.event_name = fc["event-name"];
            ev.event_start_time = Convert.ToDateTime(fc["start-time"]);
            ev.FK_events_owner = User.Identity.Name;

            if (contentImage != null)
            {
                string pic = System.IO.Path.GetFileName(contentImage.FileName);

                // Generate a random filename
                var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var result = new string(
                    Enumerable.Repeat(chars, 64)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());


                // We extract the file ending and combine it with the generated filename
                Regex regex = new Regex(@"\.\w{1,3}");
                result = result + regex.Match(pic).Value.ToLower();

                // Creating an absolute path
                string path = System.IO.Path.Combine(Server.MapPath("~/Images/Events"), result);

                // File is uploaded
                contentImage.SaveAs(path);

                // Setting the image name
                ev.event_profile_image = result;
            }

            return Json(EventService.CreateEvent(ev));
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