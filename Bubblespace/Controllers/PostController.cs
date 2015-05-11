using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

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
            // Check For Authentication
            if (!User.Identity.IsAuthenticated)
            {
                return Json("{\"Error\": \"Bad Authentication\",\"Code\": 1}");
            }

            // Get The Current User So We Can Reference Him As A Post Owner
            AspNetUsers userModel = Bubblespace.Services.UserService.GetUserByEmail(User.Identity.Name);

            
            // Create the post we insert, And fill in relative information below
            posts postToInsert = new posts();

            postToInsert.content_text = collection["content_text"];
            postToInsert.content_picture = "";                      // <-- Breyta Path Fyrir Myndir
            postToInsert.content_is_video = Convert.ToByte(0);
            postToInsert.time_inserted = DateTime.Now;
            postToInsert.FK_posts_users = userModel.Id;
            postToInsert.FK_posts_bubble_groups = null;

            // Inside this if statement we handle the image if one is uploaded
            if(contentImage != null)
            {
                
                System.Diagnostics.Debug.WriteLine("We have a file");

                // Retreiving the file name
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

                // Debug Print
                System.Diagnostics.Debug.WriteLine("The String: " + result);

                // Creating an absolute path
                string path = System.IO.Path.Combine(Server.MapPath("~/Images"), result);

                // Debug Print
                System.Diagnostics.Debug.WriteLine("The Path: " + path);
                
                // File is uploaded
                contentImage.SaveAs(path);

                // Setting the image name
                postToInsert.content_picture = result;
            }


            try
            {
                Bubblespace.Services.PostService.SavePostToDB(postToInsert);
            }
            catch (Exception)
            {
                return Json("{\"Error\": \"Couldn't Insert Into Database\",\"Code\": 2}");
            }

            return Json(postToInsert);
        }


        /* <summary>
        * Likes or Bursts a comment
        * </summary>
        * <param name="collection">List of paramters from the form request</param>
        * <returns>A Json error object on failure Or a json object of the post on success</returns>
        * <author></author>
        */
        public ActionResult LikePost(FormCollection collection)
        {
            return Json("Fuck this shit");
        }

        public ActionResult BurstPost(FormCollection collection)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json("{\"Error\": \"Bad Authentication\",\"Code\": 1}");
            }

            AspNetUsers user = Bubblespace.Services.UserService.GetUserByEmail(User.Identity.Name);

            post_likes likeToInsert = new post_likes();

            likeToInsert.FK_group_post_like_users = user.Id;
            likeToInsert.FK_group_post_likes_group_posts = Convert.ToInt32(collection["postId"]);
            likeToInsert.post_burst = true;
            likeToInsert.post_like = false;
            try
            {
                Bubblespace.Services.PostService.SaveLikePost(likeToInsert);
            }
            catch (Exception)
            {
                return Json("{\"Error\": \"Couldn't Insert Into Database\",\"Code\": 2}");
            }

            return Json(likeToInsert);
        }

        public ActionResult CommentPost()
        {
            return View();
        }

        public ActionResult LikeComment(FormCollection collection)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json("{\"Error\": \"Bad Authentication\",\"Code\": 1}");
            }

            AspNetUsers user = Bubblespace.Services.UserService.GetUserByEmail(User.Identity.Name);

            like_comments likeToInsert = new like_comments();

            likeToInsert.FK_like_comments_users = user.Id;
            likeToInsert.FK_like_comments_post_comments = Convert.ToInt32(collection["commentId"]);
            likeToInsert.comment_like = Convert.ToBoolean(1);
            likeToInsert.comment_burst = null;
            try
            {
                Bubblespace.Services.PostService.SaveLikeComment(likeToInsert);
            }
            catch (Exception)
            {
                return Json("{\"Error\": \"Couldn't Insert Into Database\",\"Code\": 2}");
            }

            return Json(likeToInsert);
        }

        public ActionResult BurstComment(FormCollection collection)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json("{\"Error\": \"Bad Authentication\",\"Code\": 1}");
            }

            AspNetUsers user = Bubblespace.Services.UserService.GetUserByEmail(User.Identity.Name);

            like_comments likeToInsert = new like_comments();

            likeToInsert.FK_like_comments_users = user.Id;
            likeToInsert.FK_like_comments_post_comments = Convert.ToInt32(collection["commentId"]);
            likeToInsert.comment_like = null;
            likeToInsert.comment_burst = Convert.ToBoolean(1);
            try
            {
                Bubblespace.Services.PostService.SaveLikeComment(likeToInsert);
            }
            catch (Exception)
            {
                return Json("{\"Error\": \"Couldn't Insert Into Database\",\"Code\": 2}");
            }

            return Json(likeToInsert);
        }

        public ActionResult BurstCount()
        {
            return View();
        }

        public ActionResult Sort()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult CommentBurstCount()
        {
            return View();
        }
	}
}