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

        /* <summary>
        * Creates an event from the given information
        * </summary>
        * <param name="event-description">The description of the event</param>
        * <param name="end-time">End time of the event</param>
        * <param name="start-time">The start time of the event</param>
        * <param name="event-name">Name of the event</param>
        * <returns>JSON object of the message it saved to teh database</returns>
        * <author>Sveinbjörn</author>
        */

        [HttpPost]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase contentImage)
        {
            // Filling in the information to a new event
            events eventToAdd = new events();
            eventToAdd.event_description = collection["event-description"];
            eventToAdd.event_end_time = EventService.ParseDate(collection["end-time"]);
            eventToAdd.event_start_time = EventService.ParseDate(collection["start-time"]);
            eventToAdd.event_name = collection["event-name"];
            eventToAdd.FK_events_owner = UserService.GetUserByEmail(User.Identity.Name).Id;


            if (contentImage != null)
            {
                // Setting the image name
                eventToAdd.event_profile_image = FileUploadService.UploadImage(contentImage, "Events");
            }

            // Saving the event
            EventService.CreateEvent(eventToAdd);

            return RedirectToAction("Home", "Home");
        }

        /* <summary>
        * Returns all events as a json object
        * </summary>
        * <returns>Returns all events as a json object</returns>
        * <author>Sveinbjörn</author>
        */

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

        /* <summary>
        * Gets all information about an event from the given id
        * </summary>
        * <param name="eventId">The description of the event</param>
        * <returns>JSON object of the event with all relevant information   </returns>
        * <author>Sveinbjörn</author>
        */

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