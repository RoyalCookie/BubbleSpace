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

            var dbUser = db.AspNetUsers.ToList().Where(x => x.Email == "janus@tviund.com");

            foreach(AspNetUsers a in dbUser){
                System.Diagnostics.Debug.WriteLine(a.Email);
            }

            // Get The User Data
            AspNetUsers user = (from a in dbUsers
                       where a.Email == Convert.ToString(identity)
                       select a).Single();
            
            posts postToInsert = new posts();
            /*
            postToInsert.content_text = collection["content_text"];
            postToInsert.content_picture = "None";
            postToInsert.content_is_video = Convert.ToByte(0);
            postToInsert.time_inserted = DateTime.Now;
            // Real Below In Comments - Above For Testing
            //postToInsert.time_inserted = Convert.ToDateTime(collection["time"]);
            postToInsert.FK_posts_users = user.Id;
            postToInsert.FK_posts_bubble_groups = 0;
            // Real Below Using Null Currently As Testing
            //postToInsert.FK_posts_bubble_groups = Convert.ToInt32(collection["FK_posts_bubble_groups"]);
            */

            postToInsert.FK_posts_bubble_groups = null;
            postToInsert.FK_posts_users = user.Id;
            postToInsert.content_picture = "none";
            postToInsert.content_text = "this is a test";
            postToInsert.content_is_video = Convert.ToByte(0);
            postToInsert.time_inserted = DateTime.Now;

            try
            {
                db.posts.Add(postToInsert);
                db.SaveChanges();
            }catch(Exception e){
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.InnerException);
                System.Diagnostics.Debug.WriteLine(e.HelpLink);
            }
            
            // Debug Info
            System.Diagnostics.Debug.WriteLine("\n\n\nYou've Called Post.Create()");
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
            var db = new VERK2015_H17Entities1();
            
            

            try
            {
                db.user_ranks.Add(rankToAdd);
                db.SaveChanges();            
            }catch (Exception e){
                System.Diagnostics.Debug.WriteLine("Villa");
            }
            
            return Json("asd");
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