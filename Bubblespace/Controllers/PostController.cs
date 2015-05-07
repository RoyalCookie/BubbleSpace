using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Controllers
{
    public class PostController : Controller
    {
        //
        // GET: /Post/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult LikePost()
        {
            return View();
        }

        public ActionResult BurstPost()
        {
            return View();
        }

        public ActionResult CommentPost()
        {
            return View();
        }

        public ActionResult LikeComment()
        {
            return View();
        }

        public ActionResult BurstComment()
        {
            return View();
        }

        public ActionResult BurstCount()
        {
            return View();
        }

        public ActionResult Sort()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult CommentBurstCount()
        {
            return View();
        }
	}
}