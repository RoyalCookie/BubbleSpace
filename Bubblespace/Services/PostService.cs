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
            int allowUserToLike = (from x in db.post_likes.Where(y => y.FK_group_post_like_users == postLike.FK_group_post_like_users && y.FK_group_post_likes_group_posts == postLike.FK_group_post_likes_group_posts && y.post_like == true)
                                   select x).Count();
            if(allowUserToLike == 0)
            {
                db.post_likes.Add(postLike);
                db.SaveChanges();
            }
            int totalPostLikes = (from x in db.post_likes.Where(y => y.FK_group_post_likes_group_posts == postLike.FK_group_post_likes_group_posts && y.post_like == true)
                                  select x).Count();
            
            return totalPostLikes;
        }

        /* <summary>Counts the burst on the bubble</summary>
         * <param name="burst">Post like model</param>
         * <returns>The number of bubble bursts</returns>
         * <author>Sveinbjorn</author>
         */
        static public int SaveBurstPost(post_likes burst)
        {
            var db = new VERK2015_H17Entities1();
            int allowUserToBurst = (from x in db.post_likes.Where(y => y.FK_group_post_like_users == burst.FK_group_post_like_users && y.FK_group_post_likes_group_posts == burst.FK_group_post_likes_group_posts && y.post_burst == true)
                                    select x).Count(); 
            if(allowUserToBurst == 0)
            {
                db.post_likes.Add(burst);
                db.SaveChanges();
            }
            
            var burstCount = (from x in db.post_likes.Where(y => y.FK_group_post_likes_group_posts == burst.FK_group_post_likes_group_posts && y.post_burst == true)
                              select x).Count();
            return burstCount;
        }

        /* <summary>Saves a comment by a user on a post</summary>
         * <param name="comment">Post comment model</param>
         * <returns>Nothing</returns>
         * <author>Sveinbjorn</author>
         */
        static public void SaveCommentOnPost(post_comments comment)
        {
            var db = new VERK2015_H17Entities1();
            db.post_comments.Add(comment);
            db.SaveChanges();
        }

        /* <summary>Counts the bursts on a post</summary>
         * <param name="postBurst">Post model</param>
         * <returns>Number of bursts on a post</returns>
         * <author>Sveinbjorn</author>
         */
        static public List<post_likes> GetBurstCount(posts postBurst)
        {
            var db = new VERK2015_H17Entities1();
            var burstCount = (from x in db.post_likes.Where(y => y.posts.C_ID == postBurst.C_ID && y.post_burst == true)
                        select x).ToList();

            return burstCount;
        }

        /* <summary>Counts the bursts on a comment</summary>
         * <param name="postComment">Post comment model</param>
         * <returns>Number of bursts on a comment</returns>
         * <author>Sveinbjorn</author>
         */
        static public List<post_likes> GetBurstCount(post_comments postComment)
        {
            var db = new VERK2015_H17Entities1();
            var burstCount = (from x in db.post_likes.Where(y => y.posts.C_ID == postComment.C_ID && y.post_burst == true)
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
            int allowUserToLike = (from x in db.post_likes.Where(y => y.FK_group_post_like_users == commentLike.AspNetUsers.UserName && y.FK_group_post_likes_group_posts == commentLike.FK_like_comments_post_comments)
                                   select x).Count();
            if(allowUserToLike == 0)
            {
                db.like_comments.Add(commentLike);
                db.SaveChanges();
            }
        }

        /* <summary>Gets a list of all posts</summary>
         * <param name=""></param>
         * <returns>A list of all posts</returns>
         * <author>Sveinbjorn</author>
         */
        static public List<posts> GetAllPosts()
        {
            var db = new VERK2015_H17Entities1();
            return db.posts.ToList();
        }

        /* <summary>Checks if URL is a youtube video with regex</summary>
         * <param name="url">URL string</param>
         * <returns>1 if the URL is a youtuve video, 0 if not</returns>
         * <author>Sveinbjorn</author>
         */
        static public Byte IsYoutubeVideo(string url)
        {
            if(Regex.IsMatch(url, @"(https:\/\/)*(www|m).youtube.com\/watch\?v=.+"))
            {
                return 1;
            }
            return 0;
        }

        /* <summary>Gets a list of post sorted by either name, date or likes </summary>
         * <param name="orderByField"></param>
         * <returns>A list of all posts in eiter name, date or likes order</returns>
         * <author>Janus</author>
         */
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

        /* <summary>Gets a list of posts by ID</summary>
         * <param name="user">user</param>
         * <returns>List of posts</returns>
         * <author>Janus</author>
         */
        static public List<posts> GetAllPosts(AspNetUsers user) 
        {
            List<posts> userPosts = (from post in GetAllPosts()
                                   where post.FK_posts_users == user.Id
                                   select post).ToList();
            return userPosts;
        }

        /* <summary>Gets all posts by a user</summary>
         * <param name="user">user model</param>
         * <returns>List of posts by a user</returns>
         * <author>Sveinbjorn</author>
         */
        static public List<posts> GetAllUserPosts(AspNetUsers user)
        {
            var db = new VERK2015_H17Entities1();
            var friendPosts = new List<posts>();
            var postRet = new List<posts>();

            var userFriends = UserService.GetAllFriends(user);
            var userPosts = (from x in db.posts.Where(y => y.FK_posts_users == user.Id && y.FK_posts_bubble_groups == null)
                             select x).ToList();

            foreach(AspNetUsers u in userFriends)
            {
                friendPosts.AddRange((from x in db.posts.Where(y => y.FK_posts_users == u.Id && y.FK_posts_bubble_groups == null)
                                      select x).ToList());
            }

            postRet.AddRange(friendPosts);
            postRet.AddRange(userPosts);

            return postRet;
        }

        
    }
}