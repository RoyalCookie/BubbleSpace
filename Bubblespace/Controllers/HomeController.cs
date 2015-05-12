using Bubblespace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Bubblespace! (edit this in HomeController.cs)";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Bubblespace! (edit this in HomeController.cs)";

            return View();
        }

        public ActionResult Home()
        {
            ViewBag.Message = "This is the main page! (edit this in HomeController.cs)";
            return View();
        }
    }
}