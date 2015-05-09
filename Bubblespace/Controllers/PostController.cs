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
            // Get The Currently Logged In Users Email
            string identity = User.Identity.Name;

            // Create A  Database Entity
            var db = new VERK2015_H17Entities1();
            
            // Get a List Of All Users
            var dbUsers = db.AspNetUsers.ToList();
            
            // Get The User Data
            AspNetUsers user = (from a in dbUsers
                       where a.Email == Convert.ToString(identity)
                       select a).Single();
            
            posts postToInsert = new posts();
            postToInsert.content_text = collection["content_text"];
            postToInsert.content_is_video = Convert.ToByte(0);

            
            postToInsert.time_inserted = DateTime.Now;
            // Real Below In Comments - Above For Testing
            //postToInsert.time_inserted = Convert.ToDateTime(collection["time"]);
           
            postToInsert.FK_posts_users = user.Id;

            
            postToInsert.FK_posts_bubble_groups = null;
            // Real Below Using Null Currently As Testing
            //postToInsert.FK_posts_bubble_groups = Convert.ToInt32(collection["FK_posts_bubble_groups"]);


            db.posts.Add(postToInsert);
            db.SaveChanges();
            
            // Debug Info
            System.Diagnostics.Debug.WriteLine("You've Called Post.Create()");
            System.Diagnostics.Debug.WriteLine("Current Data:");
            System.Diagnostics.Debug.WriteLine("Content: " + postToInsert.content_text);
            System.Diagnostics.Debug.WriteLine("User: " + user.Email);
            System.Diagnostics.Debug.WriteLine("UserId: " + user.Id);

            return Json("asd");
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