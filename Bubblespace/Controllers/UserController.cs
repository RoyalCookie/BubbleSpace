using Bubblespace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Controllers
{   
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Ban()
        {
            return View();
        }

        public ActionResult Promote()
        {
            return View();
        }

        public ActionResult FriendRequest()
        {
            return View();
        }

        public ActionResult FriendRemove()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Events()
        {
            return View();
        }

        public ActionResult Groups()
        {
            return View();
        }

        public ActionResult Chats()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Friends()
        {
            // For now this returns all users, not just friends.
            // We need to change this to accept an id and return only friends. - Andri
            UserService us = new UserService();
            var allUsers = us.GetAllUsers();
            var usernames = ( from user in allUsers select user.NickName).ToList();
            var images = (from user in allUsers select user.profile_image).ToList();

            List<List<string>> returnJson = new List<List<string>>();
            returnJson.Add(usernames);
            returnJson.Add(images);
            return Json(returnJson);
        }
	}
}