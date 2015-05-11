using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;

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

        /* <summary>
         * Creates a like or a burst into the database depending on the values in the parameter
         * </summary>
        * <param name="postLike">post_likes object, either with the burst or like value set as 1</param>
        * <returns>Nothing</returns>
        * <author>Janus</author>
        */
        static public void SaveLikePost(post_likes postLike)
        {
            var db = new VERK2015_H17Entities1();
            db.post_likes.Add(postLike);
            db.SaveChanges();
        }

        /* <summary></summary>
         * <param name="ID"></param>
         * <returns></returns>
         * <author></author>
         */
        static public void SaveCommentOnPost(post_comments comment)
        {
            var db = new VERK2015_H17Entities1();
            db.post_comments.Add(comment);
            db.SaveChanges();
        }

        /* <summary>
         * Creates a like or a burst into the database depending on the values in the parameter
         * </summary>
        * <param name="postLike">likes_comments object, either with the burst or like value set as 1</param>
        * <returns>Nothing</returns>
        * <author>Janus</author>
        */
        static public void SaveLikeComment(like_comments commentLike)
        {
            var db = new VERK2015_H17Entities1();
            db.like_comments.Add(commentLike);
            db.SaveChanges();
        }

        static public List<posts> GetAllPosts()
        {
            var db = new VERK2015_H17Entities1();
            return db.posts.ToList();
        }

        static public List<posts> GetAllPosts(string orderByField) 
        {
            List<posts> allPosts = PostService.GetAllPosts();
            List<AspNetUsers> allUsers = UserService.GetAllUsers();
            List<posts> sortedPosts;
            switch (orderByField)
            {
                case "Name":
                    sortedPosts = (from post in allPosts
                                   orderby post.AspNetUsers.NickName
                                   select post).ToList();
                    break;
                case "Date":
                    sortedPosts = (from post in allPosts
                                   orderby post.time_inserted
                                   select post).ToList();
                    break;
                case "Likes":
                    sortedPosts = (from post in allPosts
                                   orderby post.post_likes.Count descending
                                   select post).ToList();
                    break;
                default:
                    sortedPosts = (from post in allPosts
                                   orderby post.AspNetUsers.NickName
                                   select post).ToList();
                    break;
            }
            return sortedPosts;
        }

        
    }
}