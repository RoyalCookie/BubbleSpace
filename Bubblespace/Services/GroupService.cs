using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Services
{
	public class GroupService
	{
       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public void CreateGroup(bubble_groups group)
		{
            var db = new VERK2015_H17Entities1();
            db.bubble_groups.Add(group);
            db.SaveChanges();
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
 		public void UserJoinGroup()
		{	
			
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public List<posts> SortGroupPostBy(string sortingsorter, bubble_groups gr)
		{
            var db = new VERK2015_H17Entities1();
            var sorted = (from x in db.posts.Where(y => y.FK_posts_bubble_groups == gr.C_ID)
                          orderby x.time_inserted ascending
                          select x).ToList();
            return sorted;
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public void InsertGroupDescription()
		{
			
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public void InsertGroupProfileImage()
		{
			
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public void SetAdminStatus(AspNetUsers user)
		{
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public void CreateGroupPost()
		{
			
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public List<bubble_groups>GetAllGroups()
		{
			return null;
		}
	}	
}