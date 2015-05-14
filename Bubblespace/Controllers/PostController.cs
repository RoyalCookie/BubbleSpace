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
        //
        // GET: /Post/

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
            // TODO: File Handling 

            // Check For Authentication
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home", "Home");
            }

            // Get The Current User So We Can Reference Him As A Post Owner
            AspNetUsers userModel = UserService.GetUserByEmail(User.Identity.Name);

            // Create the post we insert, And fill in relative information below
            posts postToInsert = new posts();

            postToInsert.content_text = collection["content_text"];
            postToInsert.content_is_video = Convert.ToByte(0);
            postToInsert.time_inserted = DateTime.Now;
            postToInsert.FK_posts_users = userModel.Id;
            postToInsert.FK_posts_bubble_groups = null;
            //Upload image
            postToInsert.content_picture = FileUploadService.UploadImage(contentImage, "Posts");

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
                PostService.SaveLikePost(likeToInsert);
            }
            catch (Exception)
            {
                return Json("{\"Error\": \"Couldn't Insert Into Database\",\"Code\": 2}");
            }

            return Json(likeToInsert);
        }



        /* <summary>
        * Adds a comment to a post
        * </summary>
        * <param name="collection">List of paramters from the form request</param>
        * <returns>A Json error object on failure Or a json object of the post on success</returns>
        * <author></author>
        */
        [HttpPost]
        public ActionResult CommentPost(FormCollection collection)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json("{\"Error\": \"Bad Authentication\",\"Code\": 1}");
            }

            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);

            post_comments postComment = new post_comments();

            postComment.comment = collection["comment"];
            postComment.comment_time = DateTime.Now;
            postComment.FK_post_comments_users = user.Id;
            postComment.FK_post_comments_posts = Convert.ToInt32(collection["postId"]);

            try
            {
                PostService.SaveCommentOnPost(postComment);
            }
            catch (Exception)
            {
                return Json("{\"Error\": \"Couldn't Insert Into Database\",\"Code\": 2}");
            }

            return Json(postComment);
        }

        /* <summary>
        * Likes a comment
        * </summary>
        * <param name="collection">List of paramters from the form request</param>
        * <returns>A Json error object on failure Or a json object of the post on success</returns>
        * <author></author>
        */
        [HttpPost]
        public ActionResult LikeComment(FormCollection collection)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json("{\"Error\": \"Bad Authentication\",\"Code\": 1}");
            }

            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);

            like_comments likeToInsert = new like_comments();

            likeToInsert.FK_like_comments_users = user.Id;
            likeToInsert.FK_like_comments_post_comments = Convert.ToInt32(collection["commentId"]);
            likeToInsert.comment_like = Convert.ToBoolean(1);
            likeToInsert.comment_burst = null;
            try
            {
                PostService.SaveLikeComment(likeToInsert);
            }
            catch (Exception)
            {
                return Json("{\"Error\": \"Couldn't Insert Into Database\",\"Code\": 2}");
            }

            return Json(likeToInsert);
        }

        /* <summary>
        * Bursts a comment
        * </summary>
        * <param name="collection">List of paramters from the form request</param>
        * <returns>A Json error object on failure Or a json object of the post on success</returns>
        * <author>Janus</author>
        */
        [HttpPost]
        public ActionResult BurstComment(FormCollection collection)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json("{\"Error\": \"Bad Authentication\",\"Code\": 1}");
            }

            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);

            like_comments likeToInsert = new like_comments();

            likeToInsert.FK_like_comments_users = user.Id;
            likeToInsert.FK_like_comments_post_comments = Convert.ToInt32(collection["commentId"]);
            likeToInsert.comment_like = null;
            likeToInsert.comment_burst = Convert.ToBoolean(1);
            try
            {
                PostService.SaveLikeComment(likeToInsert);
            }
            catch (Exception)
            {
                return Json("{\"Error\": \"Couldn't Insert Into Database\",\"Code\": 2}");
            }

            return Json(likeToInsert);
        }

        [HttpPost]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CommentBurstCount(FormCollection fc) 
        {
            posts post = new posts();
            post.C_ID = Convert.ToInt32(fc["post_id"]);

            return Json(PostService.GetBurstCount(post));
        }
        [HttpPost]
        public ActionResult PostBurstCount(FormCollection fc)
        {
            post_comments postComment = new post_comments();
            postComment.C_ID = Convert.ToInt32(fc["post_id"]);

            return Json(PostService.GetBurstCount(postComment));
        }

        [HttpPost]
        public ActionResult Sort(FormCollection collection)
        {
            string orderByField = collection["orderBy"];

            List<posts> sortedPosts = PostService.GetAllPosts(orderByField);
            var returnObject = new
            {
                posterNames = ( from post in sortedPosts
                                select post.AspNetUsers.NickName).ToList(),
                postBody = (    from post in sortedPosts
                                select post.content_text).ToList(),
                profileImage = (from post in sortedPosts
                                select post.AspNetUsers.profile_image).ToList()
            };

            return Json(returnObject);
        }
        [HttpPost]
        public ActionResult GetPostLikes(FormCollection collection)
        {

            return View();
        }
        [HttpPost]
        public ActionResult GetAllPosts()
        {
            var allPosts = PostService.GetAllPosts();
            var posterNames = (from post in allPosts
                               select post.AspNetUsers.NickName).ToList();
            var postBody = (from post in allPosts
                            select post.content_text).ToList();
            var profileImage = (from post in allPosts
                                select post.AspNetUsers.profile_image).ToList();
            var posterId = (from post in allPosts
                            select post.AspNetUsers.Id).ToList();

            List<List<string>> returnJson = new List<List<string>>();
            returnJson.Add(posterNames);
            returnJson.Add(postBody);
            returnJson.Add(profileImage);
            returnJson.Add(posterId);
            return Json(returnJson);
        }
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
                                 select post.post_likes.Count.ToString()).ToList();

            List<List<string>> returnJson = new List<List<string>>();

            returnJson.Add(posterNames);
            returnJson.Add(postBody);
            returnJson.Add(profileImage);
            returnJson.Add(posterId);
            returnJson.Add(postId);
            returnJson.Add(postLikeCount);
            return Json(returnJson);
        }

    }
}