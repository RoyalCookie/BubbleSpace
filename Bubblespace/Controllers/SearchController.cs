using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bubblespace.Services;

namespace Bubblespace.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Users()
        {
            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);
            var users = SearchService.SearchUsersByName(user);

            return Json(users);
        }
        [HttpPost]
        public ActionResult Groups(FormCollection fc)
        {
            bubble_groups bGroup = new bubble_groups();
            bGroup.group_name = fc["group_name"];
            var groups = SearchService.SearchGroupByName(bGroup);

            return View(groups);
        }
        [HttpPost]
        public ActionResult Posts(FormCollection fc)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Events(FormCollection fc)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Chat(FormCollection fc)
        {
            return View();
        }

    }
}