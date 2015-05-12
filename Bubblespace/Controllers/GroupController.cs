using Bubblespace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Controllers
{
    public class GroupController : Controller
    {
        //
        // GET: /Group/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Join()
        {
            return View();
        }

        public ActionResult SetAdmin()
        {
            return View();
        }

        public ActionResult Post()
        {
            return View();
        }

        public ActionResult Sort()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult GetAllGroups()
        {
            var allGroups = GroupService.GetAllGroups();
            var groupName = (from grp in allGroups
                              select grp.group_name).ToList();
            var groupImage = (from grp in allGroups
                              select grp.group_profile_image).ToList();
            var groupId = (from grp in allGroups
                           select grp.C_ID.ToString()).ToList();
            List<List<string>> returnJson = new List<List<string>>();
            returnJson.Add(groupName);
            returnJson.Add(groupImage);
            returnJson.Add(groupId);
            return Json(returnJson);
        }

        public ActionResult GetGroupById(FormCollection collection)
        {
            if(!User.Identity.IsAuthenticated) {
                return Json("No Authentication");
            } 
            bubble_groups group = GroupService.GetGroupById(Convert.ToInt32(collection["groupId"]));
            List<string> returnJson = new List<string>();

            // TODO get posts & users.

            returnJson.Add(group.group_name);
            returnJson.Add(group.group_description);
            returnJson.Add(group.group_profile_image);
            return Json(returnJson);
        }
	}
}