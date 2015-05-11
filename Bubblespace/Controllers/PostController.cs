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
            if (!User.Identity.IsAuthenticated)
            {
                return Json("{\"Error\": \"Bad Authentication\",\"Code\": 1}");
            }

            // Get The Current User So We Can Reference Him As A Post Owner
            AspNetUsers userModel = Bubblespace.Services.UserService.GetUserByEmail(User.Identity.Name);

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
                Bubblespace.Services.PostService.SavePostToDB(postToInsert);
            }
            catch (Exception)
            {
                return Json("{\"Error\": \"Couldn't Insert Into Database\",\"Code\": 2}");
            }

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