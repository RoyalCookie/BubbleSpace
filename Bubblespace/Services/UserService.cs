using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Services
{
    //<summary>
    // Takes care of the logic for the UserController
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
        
        /* <summary>Gets a user by his email</summary>
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
         * <param name="friends">object of the model friends_added</param>
         * <returns>no return</returns>
         * <author>Valgeir</author>
         */
        static public void AddFriend(AspNetUsers userAdder, AspNetUsers userFriended)
        {
            //TODO: Add trigger function for if the friend was a friend before and then removed
              var db = new VERK2015_H17Entities1();
              
             friends_added addFriend = new friends_added();
             addFriend.FK_friends_added_users_Added = userAdder.Id;
             addFriend.FK_friends_added_users_Addee = userFriended.Id;
             addFriend.friended = true;
             
             db.friends_added.Add(addFriend);
             db.SaveChanges();     
        }

        /* <summary>user removes a friend</summary>
         * <param name="userAdder">obj of the user that removes friend</param>
         * <param name="userFriend>obj of the user that was removed</param>
         * <returns>no return</returns>
         * <author>Valgeir</author>
         */
        static public void RemoveFriend(AspNetUsers userAdder, AspNetUsers userFriend)
        {
            var db = new VERK2015_H17Entities1();
            var friendRemoved = (from x in db.friends_added.Where(y => y.FK_friends_added_users_Added == userAdder.Id).Where(z => z.FK_friends_added_users_Addee == userFriend.Id) 
                                 select x ).SingleOrDefault();
            var friendAddeRemoved = (from x in db.friends_added.Where (y => y.FK_friends_added_users_Added == userFriend.Id).Where(z => z.FK_friends_added_users_Addee == userAdder.Id)
                                     select x).SingleOrDefault();
            
            if (friendRemoved.C_ID != 0 && friendRemoved.FK_friends_added_users_Added != null)
            {
                friendRemoved.friended = false;
                db.SaveChanges();
            }
            else
            {
                friendAddeRemoved.friended = false;
                db.SaveChanges();
            }
        }

        /* <summary>Admin bans a user from BubbleSpace</summary>
         * <param name="user">takes in obj of the user</param>
         * <returns>no return</returns>
         * <author>Valgeir</author>
         */
        static public void BanUser(AspNetUsers user)
        {
            var db = new VERK2015_H17Entities1();
            var userBan = (from x in db.AspNetUsers.Where(y => y.Id == user.Id)
                           select x).SingleOrDefault();
            userBan.user_status = true;
            
            db.SaveChanges();
        }
        

        /* <summary>Upgrade a user to admin of BubbleSpace</summary>
         * <param name="email">Takes in the email of user</param>
         * <returns>no return</returns>
         * <author>Valgeir</author>
         */
        static public void UpgradeUserToAdmin(AspNetUsers user)
        {

            var db = new VERK2015_H17Entities1();

            var userToAdmin = (from x in db.AspNetUsers.Where(y => y.Id == user.Id) 
                               select x).SingleOrDefault();
            userToAdmin.FK_users_userrank = 2;
            
            db.SaveChanges();
        }

        /* <summary>Gets all events for a specified user</summary>
         * <param name="email">Takes in the email/username of user</param>
         * <returns>list of events for the user</returns>
         * <author>Valgeir</author>
         */
        static public List<events> GetAllUsersEvents(AspNetUsers user)
        {
            //TODO: Change from string to object of user
            
            var db = new VERK2015_H17Entities1();

            var userEvents = (from events in db.events.Where( )
                              select x).ToList();

            return userEvents;
        }

        /* <summary>Gets all the groups a specified user is in</summary>
         * <param name="email">Takes in the email/username of the user</param>
         * <returns>list of groups for the user</returns>
         * <author>Valgeir</author>
         */
        static public List<bubble_groups> GetAllUserGroups(string email)
        {
            //TODO: Change from string to object of user
            
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
         * * <param name="user">Takes in obj of user</param>
         * <returns>list of chats for the user</returns>
         * <author>Valgeir</author>
         */
        static public List<chats> GetAllChats(AspNetUsers user)
        {
            //TODO: Change from string to object of user
            
            var db = new VERK2015_H17Entities1();

            
            return null;
        }

        /* <summary>Gets all the friends of a specified user</summary>
         * <param name="user">Takes in obj of user</param>
         * <returns>list of friends of the user</returns>
         * <author>Valgeir</author>
         */
        static public List<friends_added> GetAllFriends(AspNetUsers user)
        {   
            var db = new VERK2015_H17Entities1();
            
            //Gets a list of friends that user added
            var friendsAdded = (from friend in db.friends_added.Where(y => y.FK_friends_added_users_Added == user.Id)
                                select friend).ToList(); 
            //Gets a list of friends that added the user
            var friendsAddee = (from friend in db.friends_added.Where(y => y.FK_friends_added_users_Addee == user.Id)
                                select friend).ToList();
            //Combines the two lists together                   
            friendsAdded.AddRange(friendsAddee); 
            
            return friendsAdded;
        }
    }
}