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
	}	
}