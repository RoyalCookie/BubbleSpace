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
        static public int SaveLikePost(post_likes postLike)
        {
            var db = new VERK2015_H17Entities1();
            int allowUserToLike = (from x in db.post_likes.Where(y => y.FK_group_post_like_users == postLike.AspNetUsers.UserName || y.FK_group_post_likes_group_posts == postLike.FK_group_post_likes_group_posts)
                                   select x).Count();
            if(allowUserToLike == 0)
            {
                db.post_likes.Add(postLike);
                db.SaveChanges();
            }
            return db.post_likes.Count();
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
        static public List<post_likes> GetBurstCount(posts postBurst)
        {
            var db = new VERK2015_H17Entities1();
            var burstCount = (from x in db.post_likes.Where(y => y.posts.C_ID == postBurst.C_ID || y.post_burst == true)
                        select x).ToList();

            return burstCount;
        }
        static public List<post_likes> GetBurstCount(post_comments postComment)
        {
            var db = new VERK2015_H17Entities1();
            var burstCount = (from x in db.post_likes.Where(y => y.posts.C_ID == postComment.C_ID || y.post_burst == true)
                              select x).ToList();

            return burstCount;
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
            int allowUserToLike = (from x in db.post_likes.Where(y => y.FK_group_post_like_users == commentLike.AspNetUsers.UserName || y.FK_group_post_likes_group_posts == commentLike.FK_like_comments_post_comments)
                                   select x).Count();
            if(allowUserToLike == 0)
            {
                db.like_comments.Add(commentLike);
                db.SaveChanges();
            }
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

        static public List<posts> GetAllPosts(AspNetUsers user) 
        {
            List<posts> userPosts = (from post in GetAllPosts()
                                   where post.FK_posts_users == user.Id
                                   select post).ToList();
            return userPosts;
        }
        static public List<posts> GetAllUserPosts(AspNetUsers user)
        {
            var db = new VERK2015_H17Entities1();
            var friendPosts = new List<posts>();
            var postRet = new List<posts>();

            var userFriends = UserService.GetAllFriends(user);
            var userPosts = (from x in db.posts.Where(y => y.FK_posts_users == user.Id)
                             select x).ToList();

            foreach(AspNetUsers u in userFriends)
            {
                friendPosts.AddRange((from x in db.posts.Where(y => y.FK_posts_users == u.Id)
                                      select x).ToList());
            }

            postRet.AddRange(friendPosts);
            postRet.AddRange(userPosts);

            return postRet;
        }

        
    }
}