using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Services
{
	public class EventService
	{
		public List<events>GetAllEvents()
		{
            var db = new VERK2015_H17Entities1();
            return db.events.ToList();
		}
		
		public void CreateEvent()
		{
			
		}
		
		public void JoinEvent()
		{
			
		}
		
		public void EditEvent()
		{
			
		}
				
	}
	
}
