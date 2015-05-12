using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Services
{
	public class ErrorService
	{
       /* <summary>Logs error into database</summary>
        * <param name="error">Obj of error</param>
        * <returns>no return</returns>
        * <author>Valgeir</author>
        */
		public void LogErrorToDB(bubble_errors error)
		{
			var db = new VERK2015_H17Entities1();
			
			db.bubble_errors.Add(error);
			db.SaveChanges();
		}
	}	
}