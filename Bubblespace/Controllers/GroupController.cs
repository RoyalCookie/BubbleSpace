using Bubblespace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        [HttpPost]
        public ActionResult Create(FormCollection fc, HttpPostedFileBase contentImage)
        {
            bubble_groups bGroup = new bubble_groups();
            bGroup.group_description = fc["group-description"];
            bGroup.group_name = fc["group-name"];
            bGroup.FK_bubble_groups_users = UserService.GetUserByEmail(User.Identity.Name).Id;

            if(contentImage != null)
            {
                string pic = System.IO.Path.GetFileName(contentImage.FileName);

                // Generate a random filename
                var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var result = new string(
                    Enumerable.Repeat(chars, 64)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());


                // We extract the file ending and combine it with the generated filename
                Regex regex = new Regex(@"\.\w{1,3}");
                result = result + regex.Match(pic).Value.ToLower();

                // Creating an absolute path
                string path = System.IO.Path.Combine(Server.MapPath("~/Images/Groups"), result);

                // File is uploaded
                contentImage.SaveAs(path);

                // Setting the image name
                bGroup.group_profile_image = result;
            }
            var newGroup = GroupService.CreateGroup(bGroup);

            return Json(newGroup);
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

            returnJson.Add(group.group_name);
            returnJson.Add(group.group_description);
            returnJson.Add(group.group_profile_image);

            return Json(returnJson);
        }

        public ActionResult GetGroupPosts(FormCollection collection)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json("No Authentication");
            }

            bubble_groups group = new bubble_groups();
            group.C_ID = Convert.ToInt32(collection["groupId"]);
            List<posts> allPosts = GroupService.GetAllGroupPosts(group);
            var allContent = (from pst in allPosts
                              select pst.content_text).ToList();
            return Json(allContent);
        }
	}
}