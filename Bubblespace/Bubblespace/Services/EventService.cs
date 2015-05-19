using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Services
{
	public class EventService
	{
       /* <summary>Gets all events</summary>
        * <param>Takes no parameter</param>
        * <returns>Returns a list of events</returns>
        * <author>Andri Rafn</author>
        */
        static public List<events> GetAllEvents()
		{
            var db = new VERK2015_H17Entities1();
            return db.events.ToList();
		}

       /* <summary>Creates an Event</summary>
        * <param name="ev">Event model</param>
        * <returns>New Event</returns>
        * <author>Sveinbjorn</author>
        */
        static public events CreateEvent(events ev)
		{
            var db = new VERK2015_H17Entities1();
            db.events.Add(ev);
            db.SaveChanges();

            return ev;
		}

      /* <summary>Returns an event with the given id</summary>
       * <param name="user">Takes an int</param>
       * <returns>Event object</returns>
       * <author>Andri Rafn</author>
       */
        static public events GetEventById(int id)
        {
            var db = new VERK2015_H17Entities1();
            var eve = (from x in db.events.Where(x => x.C_ID == id)
                            select x).SingleOrDefault();
            return eve;
        }
      /* <summary>Helper function to parse datetime string</summary>
       * <param name="date">Takes an string</param>
       * <returns>Datetime object</returns>
       * <author>Janus þór</author>
       */
        public static DateTime? ParseDate(string date)
        {
            if(date == String.Empty)
            {
                return null;
            }
            DateTime dt = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            return dt;
        }
	}
	
}
