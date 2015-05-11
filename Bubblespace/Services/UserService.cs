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
    static public class UserService
    {

        /* <summary>gets all users in the system</summary>
         * <param name="ID">takes no param</param>
         * <returns>list of all users</returns>
         * <author>Valgeir</author>
         */
        static public List<AspNetUsers> GetAllUsers()
        {
            var db = new VERK2015_H17Entities1();
            var allUsers = db.AspNetUsers.ToList();

            return allUsers;
        }
        /* <summary>
         * Gets a user by his email
         * </summary>
         * <param name="email">The email string to search after</param>
         * <returns>Returns a single user that matches the email</returns>
         * <author>Janus</author>
         */
        static public AspNetUsers GetUserByEmail(string email)
        {
            var db = new VERK2015_H17Entities1();
            AspNetUsers user = (db.AspNetUsers.ToList().Where(x => x.Email == email)).Single();
            return user;
        }

        /* <summary>user adds a friend</summary>
         * <param name="userID">ID of user that adds a friend</param>
         * <param name="friendID> ID of the user that was friended</param>
         * <returns>no return</returns>
         * <author>Valgeir</author>
         */
        static public void AddFriend(string userID, string friendID)
        {
            var db = new VERK2015_H17Entities1();
        }

        /* <summary>user removes a friend</summary>
         * <param name="userID">ID of the user that removes friend</param>
         * <param name="friendID> ID of the user that was removed</param>
         * <returns>no return</returns>
         * <author>Valgeir</author>
         */
        static public void RemoveFriend(string userID, string friendID)
        {
            var db = new VERK2015_H17Entities1();

        }

        /* <summary>Admin bans a user from BubbleSpace</summary>
         * <param name="ID"></param>
         * <returns></returns>
         * <author></author>
         */
        static public void BanUser(string ID)
        {
            var db = new VERK2015_H17Entities1();
            var allUsers = db.AspNetUsers.ToList();
            var userBan = (from user in allUsers where user.Id == ID select user).SingleOrDefault();
            userBan.user_status = true;

            db.SaveChanges();
        }

        /* <summary>Upgrade a user to admin of BubbleSpace</summary>
         * <param name="ID">Takes in ID of the user</param>
         * <returns>no return</returns>
         * <author>Valgeir</author>
         */
        static public void UpgradeUserToAdmin(string ID)
        {
            var db = new VERK2015_H17Entities1();
            var allUsers = db.AspNetUsers.ToList();

            var userToAdmin = (from user in allUsers where user.Id == ID select user).SingleOrDefault();
            userToAdmin.FK_users_userrank = 2;

            db.SaveChanges();
        }

        /* <summary>Gets all events for a specified user</summary>
         * <param name="ID">Takes in ID of the user</param>
         * <returns>list of events for the user</returns>
         * <author>Valgeir</author>
         */
        static public List<events> GetAllUsersEvents(string ID)
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

        /* <summary>Gets all the groups a specified user is in</summary>
         * <param name="email">Takes in the email/username of the user</param>
         * <returns>list of groups for the user</returns>
         * <author>Valgeir</author>
         */
        static public List<bubble_groups> GetAllUserGroups(string email)
        {
            var db = new VERK2015_H17Entities1();
            var groupsList = db.bubble_groups.ToList();
            var usersList = db.AspNetUsers.ToList();
            
            var userGroups = (from userGroup in groupsList
                              join user in usersList on userGroup.FK_bubble_groups_users equals user.Id
                              where user.Email == email
                              select userGroup).ToList();
            
            return userGroups;
        }

        /* <summary>Gets all the chats for a specified user</summary>
         * <param name="email">Takes in the email/username of the user</param>
         * <returns>list of chats for the user</returns>
         * <author>Valgeir</author>
         */
        static public List<chats> GetAllChats(string email)
        {
            //TODO: figure out how to connect it by email/username of user - Valgeir

            var db = new VERK2015_H17Entities1();
            var userList = db.AspNetUsers.ToList();
            var userChatsList = db.chats.ToList();
            var userMessagesList = db.messages.ToList();
            var chatMembersList = db.chat_members.ToList();

            /*var userChats = (from userChat in userChatsList
                             join chatMember in chatMembersList on userChat.C_ID equals chatMember.FK_chat_members_chat
                             join userMessage in userMessagesList on userChat.C_ID equals userMessage.FK_messages_chat_id
                             where chatMember.FK_chat_members_user == ID || userMessage.FK_messages_user == ID
                             select userChat).ToList();*/

            var userChats = (from userChat in userChatsList
                             join chatMember in chatMembersList on userChat.C_ID equals chatMember.FK_chat_members_chat
                             join user in userList on chatMember.FK_chat_members_user equals user.Id
                             join userMessage in userMessagesList on userChat.C_ID equals userMessage.FK_messages_chat_id
                             where user.UserName == email
                             select userChat).ToList();
            
            return userChats;
        }

        /* <summary>Gets all the friends of a specified user</summary>
         * <param name="ID">Takes in the ID of the user</param>
         * <returns>list of friends of the user</returns>
         * <author>Valgeir</author>
         */
        static public List<friends_added> GetAllFriends(string ID)
        {
            var db = new VERK2015_H17Entities1();
            return null;
        }
    }
}