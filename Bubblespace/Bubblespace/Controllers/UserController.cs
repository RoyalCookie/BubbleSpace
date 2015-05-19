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

        /* <summary>
         * adds a given user as a friend of the current user
        * </summary>
        * <param name="user_id">user to add as friend</param>
        * <author>Sveinbjörn</author>
        */
        [HttpPost]
        public ActionResult FriendRequest(FormCollection fc)
        {
            AspNetUsers currentUser = UserService.GetUserByEmail(User.Identity.Name);
            AspNetUsers possibleFriend = UserService.GetUserById(fc["user_id"]);


            return Json(UserService.ToggleFriendship(currentUser, possibleFriend));
        }

        /* <summary>
        * Removes a given friend from the current users friendlist
        * </summary>
        * <param name="user_id">the user to be removed</param>
        * <returns>json object true or false</returns>
        * <author>Sveinbjörn</author>
        */
        [HttpPost]
        public ActionResult FriendRemove(FormCollection fc)
        {
            AspNetUsers currentUser = UserService.GetUserByEmail(User.Identity.Name);
            AspNetUsers possibleFriend = UserService.GetUserById(fc["user_id"]);

            return Json(UserService.RemoveFriend(currentUser, possibleFriend));
        }

        /* <summary>
        * Get all friends of the current user
        * </summary>
        * <returns>JSON object of all information on friends of the user</returns>
        * <author>Sveinbjörn</author>
        */
        [HttpPost]
        public ActionResult GetFriends()
        {
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

        /* <summary>
        * gets information from the userId given
        * </summary>
        * <param name="userId">The id of the user to get information about</param>
        * <returns>JSON object of the information related to the user</returns>
        * <author>Janus</author>
        */

        [HttpPost]
        public ActionResult GetUserInformation(FormCollection collection) 
        {
            // Gets all the user information
            AspNetUsers user = UserService.GetUserById(collection["userId"]);
            List<posts> allUserPosts = UserService.GetUsersPosts(user);
            List<posts> userPosts = (from pst in allUserPosts
                                         where pst.FK_posts_bubble_groups == null
                                         select pst).ToList();
           
            // puts all the information into a single object
            var userInformation = new
            {
                userName = user.NickName,
                profileImage = user.profile_image,
                postBody = (from post in userPosts
                         select post.content_text).ToList(),
                postId = (from post in userPosts
                          select post.C_ID.ToString()).ToList(),
                postLikeCount = (from post in userPosts
                        select post.post_likes.Where(y => y.post_like == true).Count().ToString()).ToList(),
                postBurstcount = (from post in userPosts
                                  select post.post_likes.Where(y => y.post_burst == true).Count().ToString()).ToList(),
                postImage = (from post in userPosts
                            select post.content_picture),
                Id = user.Id

            };

            // Returns that object as a Json string
            return Json(userInformation);
        }

        /* <summary>
        * Gets all info about the logged in user
        * </summary>
        * <returns>JSON object of thelogged in user</returns>
        * <author>Sveinbjörn</author>
        */

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

        /* <summary>
        * Updates the profileImage of the logged in user
        * </summary>
        * <param name="contentImage">the image to replace the current image</param>
        * <returns>Redirects</returns>
        * <author>Sveinbjörn</author>
        */
        
        [HttpPost]
        public ActionResult UpdateProfileImage(HttpPostedFileBase contentImage)
        {
            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);
            if(contentImage != null)
            {
                user.profile_image = FileUploadService.UploadImage(contentImage, "Users");
                UserService.UpdateUserProfileImage(user);
                return RedirectToAction("Home", "Home");
            }
            return RedirectToAction("Home", "Home");
        }
	}
}