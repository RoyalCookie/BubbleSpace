using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Controllers
{
    public class ChatController : Controller
    {
        //
        // GET: /Chat/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendMessage()
        {
            return View();
        }

        public ActionResult GetMessage()
        {
            return View();
        }

        public ActionResult CreateChat()
        {
            return View();
        }

        public ActionResult RenameChat()
        {
            return View();
        }
	}
}