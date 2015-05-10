using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Controllers
{
    public class PostController : Controller
    {
        //
        // GET: /Post/
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            // Check For Authentication
            if(!User.Identity.IsAuthenticated){
                return Json("{\"Error\": \"Bad Authentication\",\"Code\": 1}");
            }

            // Create the DB Entity
            var db = new VERK2015_H17Entities1();

            // Get the authenticated users email
            string userEmail = User.Identity.Name;

            // Use the email to fill a AspNetUser model for the user
            AspNetUsers userModel = (db.AspNetUsers.ToList().Where(x => x.Email == userEmail)).Single();

            // Create the post we insert, And fill in relative information below
            posts postToInsert = new posts();
            
            
            
            /* 
             * TODO:
             * 1. Vista Myndir Inná Server
             * 2. Fá Path
             * 3. Setja Path Inn Í postToInsert.content_is_video
             */

            postToInsert.content_text = collection["content_text"];
            postToInsert.content_picture = "None";                      // <-- Breyta Path Fyrir Myndir
            postToInsert.content_is_video = Convert.ToByte(0);
            postToInsert.time_inserted = DateTime.Now;
            postToInsert.FK_posts_users = userModel.Id;
            postToInsert.FK_posts_bubble_groups = null;
            
            // Það Þarf Að Implementa Fyrir Myndir
            
            

            try
            {
                db.posts.Add(postToInsert);
                db.SaveChanges();
            }catch(Exception e){
                return Json("{\"Error\": \"Couldn't Insert Into Database\",\"Code\": 2}");
            }



            
            // Debug Info
            System.Diagnostics.Debug.WriteLine("\n\n\nYou've Called Post.Create()");
            System.Diagnostics.Debug.WriteLine("Current Data:");
            System.Diagnostics.Debug.WriteLine("Content: " + postToInsert.content_text);
            System.Diagnostics.Debug.WriteLine("User: " + userModel.Email);
            System.Diagnostics.Debug.WriteLine("UserId: " + userModel.Id);
            System.Diagnostics.Debug.WriteLine(Json(postToInsert).Data);

            return Json("{\"Error\": \"None\",\"Code\": 0}");
        }

        public ActionResult LikePost()
        {
            return View();
        }

        public ActionResult BurstPost()
        {
            return View();
        }

        public ActionResult CommentPost()
        {
            return View();
        }

        public ActionResult LikeComment()
        {
            return View();
        }

        public ActionResult BurstComment()
        {
            return View();
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