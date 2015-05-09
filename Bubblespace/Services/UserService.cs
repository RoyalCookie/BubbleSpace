using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Bubblespace.Services
{
	public class UserService
	{
		public List<AspNetUsers>GetAllUsers()
		{
			var db = new VERK2015_H17Entities1();
			var AllUsers = db.AspNetUsers.ToList();
			
			return AllUsers;
		}
		
		public void AddFriend()
		{
		
		}
		
		public void RemoveFriend()
		{
		}
		
		public void BanUser()
		{
		}
		
		public void UpgradeUserToAdmin()
		{
		}
		
		public List<event_users>GetAllUsersEvents()
		{
			return null;
		}
		
		public List<group_users>GetAllUserGroups()
		{
			return null;
		}
		
		public List<chats>GetAllChats()
		{
			return null;
		}
		
		public List<friends_added>GetAllFriends()
		{
			return null;
		}
	}
}