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
                bGroup.group_profile_image = FileUploadService.UploadImage(contentImage, "Groups");
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
            var posterNames = (from post in allPosts
                               select post.AspNetUsers.NickName).ToList();
            var postBody = (from post in allPosts
                            select post.content_text).ToList();
            var profileImage = (from post in allPosts
                                select post.AspNetUsers.profile_image).ToList();
            var posterId = (from post in allPosts
                            select post.AspNetUsers.Id).ToList();
            var postId = (from post in allPosts
                          select post.C_ID.ToString()).ToList();
            var postLikeCount = (from post in allPosts
                                 select post.post_likes.Where(y => y.post_like == true).Count().ToString()).ToList();
            var postBurstcount = (from post in allPosts
                                  select post.post_likes.Where(y => y.post_burst == true).Count().ToString()).ToList();
            var postImage = (from post in allPosts
                             select post.content_picture).ToList();

            List<List<string>> returnJson = new List<List<string>>();

            returnJson.Add(posterNames);
            returnJson.Add(postBody);
            returnJson.Add(profileImage);
            returnJson.Add(posterId);
            returnJson.Add(postId);
            returnJson.Add(postLikeCount);
            returnJson.Add(postBurstcount);
            returnJson.Add(postImage);

            return Json(returnJson);
        }
	}
}