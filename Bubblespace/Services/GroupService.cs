using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Services
{
	public class GroupService
	{
       /* <summary>Creates a group</summary>
        * <param name="group">bubble group model</param>
        * <returns>New group</returns>
        * <author>Sveinbjorn</author>
        */
        static public bubble_groups CreateGroup(bubble_groups group)
		{
            var db = new VERK2015_H17Entities1();
            db.bubble_groups.Add(group);
            db.SaveChanges();
            return group;
		}

        /* <summary>User joins a group</summary>
         * <param name="gr">group model</param>
         * <param name="user">user</param>
         * <returns>nothing</returns>
         * <author>Sveinbjorn</author>
         */
        static public void UserJoinGroup(bubble_groups gr, AspNetUsers user)
		{
            var db = new VERK2015_H17Entities1();
            group_users groupUser = new group_users();
            groupUser.FK_group_users_bubble_group = gr.C_ID;
            groupUser.FK_group_users_users = user.UserName;
            groupUser.group_admin = false;

            db.group_users.Add(groupUser);
		}


       /* <summary>Make a group description</summary>
        * <param name="gr">Group model</param>
        * <returns>Nothing</returns>
        * <author>Sveinbjorn</author>
        */
        static public void InsertGroupDescription(bubble_groups gr)
		{
            var db = new VERK2015_H17Entities1();
            var getGroup = (from x in db.bubble_groups.Where(y => y.C_ID == gr.C_ID)
                            select x).SingleOrDefault();
            getGroup.group_description = gr.group_description;
            db.SaveChanges();
		}

       /* <summary>Inserts an image for a group</summary>
        * <param name="gr">Group model</param>
        * <returns>Nothing</returns>
        * <author>Sveinbjorn</author>
        */
        static public void InsertGroupProfileImage(bubble_groups gr)
		{
            var db = new VERK2015_H17Entities1();
            var getGroup = (from x in db.bubble_groups.Where(y => y.C_ID == gr.C_ID)
                            select x).SingleOrDefault();
            getGroup.group_profile_image = gr.group_profile_image;
            db.SaveChanges();
		}

       /* <summary>Makes a user group admin</summary>
        * <param name="user">user model</param>
        * <returns>nothing</returns>
        * <author>Sveinbjorn</author>
        */
        static public void SetAdminStatus(AspNetUsers user)
		{
            var db = new VERK2015_H17Entities1();
            var getUsr = (from x in db.group_users.Where(y => y.FK_group_users_users == user.UserName)
                          select x).SingleOrDefault();
            getUsr.group_admin = true;
            db.SaveChanges();
		}

       /* <summary>Creates a group post</summary>
        * <param name="newPost">Post model</param>
        * <returns>Nothing</returns>
        * <author>Sveinbjorn</author>
        */
        static public void CreateGroupPost(posts newPost)
		{
            var db = new VERK2015_H17Entities1();
            db.posts.Add(newPost);
            db.SaveChanges();			
		}

        /* <summary>Gets a list of posts in group</summary>
        * <param name="bubbleGroup">Group model</param>
        * <returns>List of posts in the group</returns>
        * <author>Sveinbjorn</author>
        */
        static public List<posts> GetAllGroupPosts(bubble_groups bubbleGroup)
        {
            var db = new VERK2015_H17Entities1();
            var groupPosts = (from x in db.posts.Where(y => y.FK_posts_bubble_groups == bubbleGroup.C_ID)
                              select x).ToList();

            return groupPosts;
        }

        /* <summary>Gets a list of users in the group</summary>
         * <param name="bubbleGroup">Group model</param>
         * <returns>List of users in the group</returns>
         * <author>Sveinbjorn</author>
         */
        static public List<group_users> GetAllGroupUsers(bubble_groups bubbleGroup)
        {
            var db = new VERK2015_H17Entities1();
            var groupUsers = (from x in db.group_users.Where(y => y.FK_group_users_bubble_group == bubbleGroup.C_ID)
                              select x).ToList();
            return groupUsers;
        }

       /* <summary>Gets all groups</summary>
        * <param name=""></param>
        * <returns>A list of all groups</returns>
        * <author>Sveinbjorn</author>
        */
        static public List<bubble_groups> GetAllGroups()
		{
            var db = new VERK2015_H17Entities1();
            var groups = (from x in db.bubble_groups
                         select x).ToList();
			return groups;
		}
        
        /*
         * <summary>Gets a bubble_group by id</summary>
         * <param name="id"></param>
         * <returns>a bubble_groups object</returns>
         * <author>Andri Rafn</author>
         */
        static public bubble_groups GetGroupById(int id)
        {
            var db = new VERK2015_H17Entities1();
            bubble_groups group = (db.bubble_groups.ToList().Where(x => x.C_ID == id)).Single();
            return group;
        }

        /*
        * <summary> returns a list of group_users </summary>
        * <param name="id"></param>
        * <returns>a list of group users</returns>
        * <author>Andri Rafn</author>
        */
        static public List<group_users> GetGroupUsersByGroupId(int id)
        {
            var db = new VERK2015_H17Entities1();
            var users = (db.group_users.ToList().Where(x => x.FK_group_users_bubble_group == id)).ToList();
            return users;
        }
	}	
}