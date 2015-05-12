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

        /* <summary>Gets a user by his email</summary>
         * <param name="email">The email string to search after</param>
         * <returns>Returns a single user that matches the email</returns>
         * <author>Janus</author>
         */
        static public AspNetUsers GetUserById(string id)
        {
            var db = new VERK2015_H17Entities1();
            AspNetUsers user = (db.AspNetUsers.ToList().Where(x => x.Id == id)).SingleOrDefault(null);
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
             
             //Checks if you have added that user
             var friendExist = (from x in db.friends_added.Where(y => y.FK_friends_added_users_Added == userAdder.Id).Where(z => z.FK_friends_added_users_Addee == userFriended.Id)
                                select x).SingleOrDefault();
             //Checks if the user added you
             var friendExistOther = (from x in db.friends_added.Where(y => y.FK_friends_added_users_Added == userFriended.Id).Where(z => z.FK_friends_added_users_Addee == userAdder.Id)
                                select x).SingleOrDefault();
             //If there already exist a record of your friendship where you added him, change friended to true                  
             if(friendExist.C_ID != 0 && friendExist.FK_friends_added_users_Added != null)
             {
                friendExist.friended = true;
                db.SaveChanges();
             }
             //If there exists a record of your friendship where user added you, change friended to true
             else if(friendExistOther.C_ID != 0 && friendExistOther.FK_friends_added_users_Added != null)
             {
                 friendExistOther.friended = true;
                 db.SaveChanges();
             }
             //Else create a new record of friendship
             else
             {
                friends_added addFriend = new friends_added();
                addFriend.FK_friends_added_users_Added = userAdder.Id;
                addFriend.FK_friends_added_users_Addee = userFriended.Id;
                addFriend.friended = true;
             
                db.friends_added.Add(addFriend);
                db.SaveChanges();     
             }
             
             
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
            
            //Selects friend that you added
            var friendRemoved = (from x in db.friends_added.Where(y => y.FK_friends_added_users_Added == userAdder.Id).Where(z => z.FK_friends_added_users_Addee == userFriend.Id) 
                                 select x ).SingleOrDefault();
            //Selects friend that added you
            var friendAddeRemoved = (from x in db.friends_added.Where (y => y.FK_friends_added_users_Added == userFriend.Id).Where(z => z.FK_friends_added_users_Addee == userAdder.Id)
                                     select x).SingleOrDefault();
            //If the friend was added by you, then change friended = false
            if (friendRemoved.C_ID != 0 && friendRemoved.FK_friends_added_users_Added != null)
            {
                friendRemoved.friended = false;
                db.SaveChanges();
            }
            //If the friend added you, then change friended to false
            else if(friendAddeRemoved.C_ID != 0 && friendAddeRemoved.FK_friends_added_users_Added != null)
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
            if (userBan.user_status == false)
            {
                userBan.user_status = true;
                db.SaveChanges();
            }
            else
            {
                userBan.user_status = false;
                db.SaveChanges();
            } 
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

        /* <summary>Gets all events a specified user created</summary>
         * <param name="email">Takes in the email/username of user</param>
         * <returns>list of events created by user</returns>
         * <author>Valgeir</author>
         */
        static public List<events> GetAllEventsCreatedByUser(AspNetUsers user)
        {
            var db = new VERK2015_H17Entities1();

            var userEvents = (from events in db.events.Where(x => x.FK_events_owner == user.Id)
                              select events).ToList();

            return userEvents;
        }

        /* <summary>Gets all the friends of a specified user</summary>
         * <param name="user">Takes in obj of user</param>
         * <returns>list of friends of the user</returns>
         * <author>Valgeir</author>
         */
        static public List<AspNetUsers> GetAllFriends(AspNetUsers user)
        {   
            var db = new VERK2015_H17Entities1();
            
            //Gets a list of friends that user added
            List<AspNetUsers>friendsAdded = (from friend in db.friends_added.Where(y => y.FK_friends_added_users_Added == user.Id)
                                select friend.AspNetUsers1).ToList(); 
            //Gets a list of friends that added the user
            List<AspNetUsers> friendsAddee = (from friend in db.friends_added.Where(y => y.FK_friends_added_users_Addee == user.Id)
                                select friend.AspNetUsers).ToList();
            //Combines the two lists together            
            friendsAdded.AddRange(friendsAddee);
            return friendsAdded;
        }

        static public List<posts> GetUsersPosts(AspNetUsers user)
        {
            return PostService.GetAllPosts(user);
        }

    }
}