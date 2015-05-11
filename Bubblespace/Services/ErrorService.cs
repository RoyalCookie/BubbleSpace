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
			
			bubble_errors errorLog = new bubble_errors();
			errorLog.error_msg = error.error_msg;
			errorLog.error_owner = error.error_owner;
			errorLog.time_of_error = error.time_of_error;
			
			db.bubble_errors.Add(errorLog);
			db.SaveChanges();
		}
	}	
}