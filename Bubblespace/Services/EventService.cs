using System;
using System.Collections.Generic;
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

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
        static public events CreateEvent(events ev)
		{
            var db = new VERK2015_H17Entities1();
            db.events.Add(ev);
            db.SaveChanges();

            return ev;
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
        static public void JoinEvent(AspNetUsers user, events ev)
		{
            var db = new VERK2015_H17Entities1();
            var theEvent = (from x in db.events.Where(x => x.C_ID == ev.C_ID)
                           select x).SingleOrDefault();

            event_users tempusr = new event_users();
            tempusr.event_admin = false;
            tempusr.FK_event_users_events = ev.C_ID;
            tempusr.FK_event_users_users = user.UserName;

            db.event_users.Add(tempusr);
            db.SaveChanges();
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
        static public void EditEvent(events ev)
		{
            var db = new VERK2015_H17Entities1();
            var theEvent = (from x in db.events.Where(x => x.C_ID == ev.C_ID)
                            select x).SingleOrDefault();

            theEvent.event_description = ev.event_description;
            theEvent.event_end_time = ev.event_end_time;
            theEvent.event_name = ev.event_name;
            theEvent.event_profile_image = ev.event_profile_image;
            theEvent.event_start_time = ev.event_start_time;
            theEvent.event_users = ev.event_users;
            theEvent.FK_events_owner = ev.FK_events_owner;

            db.SaveChanges();
		}
        
       /* <summary>Returns all events user is following</summary>
        * <param name="user">Takes in obj of user</param>
        * <returns>list of events</returns>
        * <author></author>
        */
		static public List<events> eventsUserFollowing(AspNetUsers user)
        {
            return null;
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
	}
	
}
