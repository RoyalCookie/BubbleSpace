using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Services  
{
    //<summary>
    // Takes care of the logic for the usercontroller
    //</summary>
	public class UserService
	{

        /* <summary>gets all users in the system</summary>
         * <param name="ID">takes no param</param>
         * <returns>returns a list of all users</returns>
         * <author>Valgeir</author>
         */
		public List<AspNetUsers>GetAllUsers()
		{
			var db = new VERK2015_H17Entities1();
			var allUsers = db.AspNetUsers.ToList();
			
			return allUsers;
		}
		
        /* <summary></summary>
         * <param name="userID">ID of user that adds a friend</param>
         * <param name="friendID> ID of the user that was friended</param>
         * <returns>no return</returns>
         * <author>Valgeir</author>
         */
		public void AddFriend(string userID, string friendID)
		{
			
		}

        /* <summary></summary>
         * <param name="userID">ID of the user that removes friend</param>
         * <param name="friendID> ID of the user that was removed</param>
         * <returns>no return</returns>
         * <author>Valgeir</author>
         */
        public void RemoveFriend(string userID, string friendID)
		{

		}


        public void BanUser(string ID)
		{
		}

        /* <summary>Upgrade a user to admin in the system</summary>
         * <param name="ID">Takes in ID of the user</param>
         * <returns>Returns nothing</returns>
         * <author>Valgeir</author>
         */
        public void UpgradeUserToAdmin(string ID)
		{
            var db = new VERK2015_H17Entities1();
            var allUsers = db.AspNetUsers.ToList();

            var userToAdmin = (from user in allUsers where user.Id == ID select user).SingleOrDefault();
            userToAdmin.FK_users_userrank = 2;

            db.SaveChanges();
		}
		
        /* <summary>Gets all events for a specified user </summary>
         * <param name="ID">Takes in ID of the user</param>
         * <returns>Returns a list of events for the user</returns>
         * <author>Valgeir</author>
         */
		public List<events>GetAllUsersEvents(string ID)
		{
			var db = new VERK2015_H17Entities1();
			var userEventsList = db.event_users.ToList();

			var eventLists = db.events.ToList();			
			var userEvents = (from eventUser in userEventsList
							  join eve in eventLists on eventUser.FK_event_users_events equals eve.C_ID
							  where eventUser.FK_event_users_users == ID
							  select eve).ToList();
			return userEvents;
		}

        public List<group_users> GetAllUserGroups(string ID)
		{
			return null;
		}

        public List<chats> GetAllChats(string ID)
		{
			return null;
		}

        public List<friends_added> GetAllFriends(string ID)
		{
			return null;
		}
	}
}