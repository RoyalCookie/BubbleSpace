using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Services
{
    public class PostService
    {
        /* <summary>
         * Saves a given post to the database
         * </summary>
         * <param name="post">The post to be saved to the database</param>
         * <returns>Nothing</returns>
         * <author>Janus</author>
         */
        static public void SavePostToDB(posts post)
        {
            var db = new VERK2015_H17Entities1();
            db.posts.Add(post);
            db.SaveChanges();
        }

        /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
        public void SaveLikePost(post_likes postLikes)
        {
            var db = new VERK2015_H17Entities1();
            db.post_likes.Add(postLikes);
            db.SaveChanges();
        }

        /* <summary></summary>
         * <param name="ID"></param>
         * <returns></returns>
         * <author></author>
         */
        public void SaveBurstPost()
        {

        }

        /* <summary></summary>
         * <param name="ID"></param>
         * <returns></returns>
         * <author></author>
         */
        public void SaveCommentOnPost()
        {

        }

        /* <summary></summary>
         * <param name="ID"></param>
         * <returns></returns>
         * <author></author>
         */
        public void SaveLikeComment()
        {

        }

        /* <summary></summary>
         * <param name="ID"></param>
         * <returns></returns>
         * <author></author>
         */
        public void BurstComment()
        {

        }
    }
}