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
		public List<events>GetAllEvents()
		{
            var db = new VERK2015_H17Entities1();
            return db.events.ToList();
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public void CreateEvent()
		{
			
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public void JoinEvent()
		{
			
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public void EditEvent()
		{
			
		}
				
	}
	
}
