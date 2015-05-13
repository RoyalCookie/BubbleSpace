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

        public ActionResult FriendRequest(FormCollection fc)
        {
            AspNetUsers currentUser = UserService.GetUserByEmail(User.Identity.Name);
            AspNetUsers possibleFriend = UserService.GetUserById(fc["user_id"]);


            return Json(UserService.AddFriend(currentUser, possibleFriend));
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
        public ActionResult GetFriends()
        {
            /*
             * ToDo: Change JsonList Into A Var Object
             */
            
            // For now this returns all users, not just friends.
            // We need to change this to accept an id and return only friends. - Andri
            AspNetUsers currentUser = UserService.GetUserByEmail(User.Identity.Name);
            var allUsers = UserService.GetAllFriends(currentUser);
            var usernames = ( from user in allUsers 
                              select user.NickName).ToList();
            var images = (from user in allUsers 
                          select user.profile_image).ToList();
            var userId = (from user in allUsers
                          select user.Id).ToList();

            List<List<string>> returnJson = new List<List<string>>();
            returnJson.Add(usernames);
            returnJson.Add(images);
            returnJson.Add(userId);
            return Json(returnJson);
        }

        // Should remain here for testing misc code
        [HttpPost]
        public ActionResult Test()
        {
            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);
            AspNetUsers userToAdd = UserService.GetUserByEmail("j@h.com");
            System.Diagnostics.Debug.WriteLine(userToAdd.Id);
            var chats = ChatService.GetAllChats(user);
            foreach (chats chat in chats) {
                System.Diagnostics.Debug.WriteLine(chat.chat_name);
            }
            chats chatter = (from chate in chats
                         where chate.C_ID == 1
                         select chate).Single();
            ChatService.AddChatUsers(chatter, userToAdd);
            return Json("It Ran");
        }

        [HttpPost]
        public ActionResult GetUserInformation(FormCollection collection) 
        {
            AspNetUsers user = UserService.GetUserById(collection["userId"]);
            List<posts> userPosts = UserService.GetUsersPosts(user);

            var userInformation = new
            {
                userName = user.NickName,
                profileImage = user.profile_image,               
                posts = (from post in userPosts
                         select post.content_text).ToList(),
                Id = user.Id
            };
            return Json(userInformation);
        }

        [HttpPost]
        public ActionResult GetLoggedInUserInfo()
        {
            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);
            var username = user.NickName;
            var image = user.profile_image;

            List<string> returnJson = new List<string>();
            returnJson.Add(username);
            returnJson.Add(image);
            return Json(returnJson);
        }
	}
}