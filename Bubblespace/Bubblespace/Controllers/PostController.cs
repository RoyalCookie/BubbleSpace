using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Bubblespace.Services;

namespace Bubblespace.Controllers
{
    public class PostController : Controller
    {
        /* <summary>
         * Takes a few parameters and creates a post
         * </summary>
        * <param name="content_text">Text of the post</param>
        * <param name="content_picture">Picture File to be saved and then the path saved to DB</param>
        * <returns>A Json error object on failure Or a json object of the post on success</returns>
        * <author></author>
        */
        [HttpPost]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase contentImage)
        {
            // Check For Authentication
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home", "Home");
            }

            // Get The Current User So We Can Reference Him As A Post Owner
            AspNetUsers userModel = UserService.GetUserByEmail(User.Identity.Name);

            // Create the post we insert, And fill in relative information below
            posts postToInsert = new posts();

            postToInsert.content_text = Regex.Replace(collection["content_text"], "janus", "#TakkJanus", RegexOptions.IgnoreCase);
            postToInsert.content_is_video = PostService.IsYoutubeVideo(collection["content_text"]);
            postToInsert.time_inserted = DateTime.Now;
            postToInsert.FK_posts_users = userModel.Id;

            var groupId = collection["group-id"];
            if(groupId != null)
            {
                postToInsert.FK_posts_bubble_groups = Convert.ToInt32(collection["group-id"]);
            }
            else
            {
                postToInsert.FK_posts_bubble_groups = null;
            }
            
            if(contentImage != null)
            {
                //Upload image
                postToInsert.content_picture = FileUploadService.UploadImage(contentImage, "Posts"); 
            }
            
            try
            {
                PostService.SavePostToDB(postToInsert);
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "Home");
            }
            return RedirectToAction("Home", "Home");
        }


        /* <summary>
        * Likes a post
        * </summary>
        * <param name="collection">List of paramters from the form request</param>
        * <returns>A Json error object on failure Or a json object of the post on success</returns>
        * <author></author>
        */
        [HttpPost]
        public ActionResult LikePost(FormCollection collection)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return Json("{\"Error\": \"Bad Authentication\",\"Code\": 1}");
            }

            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);

            post_likes likeToInsert = new post_likes();

            likeToInsert.FK_group_post_like_users = user.Id;
            likeToInsert.FK_group_post_likes_group_posts = Convert.ToInt32(collection["postId"]);
            likeToInsert.post_burst = false;
            likeToInsert.post_like = true;
            try
            {
                int likeCount = PostService.SaveLikePost(likeToInsert);
                return Json(likeCount);
            }
            catch (Exception)
            {
                return Json("{\"Error\": \"Couldn't Insert Into Database\",\"Code\": 2}");
            }            
        }

        /* <summary>
        *  Bursts a post
        * </summary>
        * <param name="collection">List of paramters from the form request</param>
        * <returns>A Json error object on failure Or a json object of the post on success</returns>
        * <author></author>
        */
        [HttpPost]
        public ActionResult BurstPost(FormCollection collection)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json("{\"Error\": \"Bad Authentication\",\"Code\": 1}");
            }

            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);

            post_likes likeToInsert = new post_likes();

            likeToInsert.FK_group_post_like_users = user.Id;
            likeToInsert.FK_group_post_likes_group_posts = Convert.ToInt32(collection["postId"]);
            likeToInsert.post_burst = true;
            likeToInsert.post_like = false;
            try
            {
                 return Json(PostService.SaveBurstPost(likeToInsert));
            }
            catch (Exception)
            {
                return Json("{\"Error\": \"Couldn't Insert Into Database\",\"Code\": 2}");
            }
        }

        /* <summary>
        *   Gets all posts that belong to the user that is logged on
        * </summary>
        * <returns>Returns all posts belonging to the user</returns>
        * <author>Janus</author>
        */
        [HttpPost]
        public ActionResult GetAllUserPosts()
        {
            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);
            var allPosts = PostService.GetAllUserPosts(user);
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
            var postYoutube = (from post in allPosts
                               select post.content_is_video.ToString()).ToList();

            List<List<string>> returnJson = new List<List<string>>();

            returnJson.Add(posterNames);
            returnJson.Add(postBody);
            returnJson.Add(profileImage);
            returnJson.Add(posterId);
            returnJson.Add(postId);
            returnJson.Add(postLikeCount);
            returnJson.Add(postBurstcount);
            returnJson.Add(postImage);
            returnJson.Add(postYoutube);

            return Json(returnJson);
        }
    }
}