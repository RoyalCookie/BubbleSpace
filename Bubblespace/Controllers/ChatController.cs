using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bubblespace.Services;

namespace Bubblespace.Controllers
{
    public class ChatController : Controller
    {

        /* <summary>
        *   Takes a chatId and message string, which it adds to the database
        *   relating it to the chatId that was given as a parameter
        * </summary>
        * <param name="chatId">The id of the chat the message belongs too</param>
        * <param name="message">Message to save to the server</param>
        * <returns>JSON object of the message it saved to teh database</returns>
        * <author>Janus</author>
        */

        [HttpPost]
        public ActionResult Send(FormCollection collection)
        {
            messages message = new messages();
            message.FK_messages_chat_id = Convert.ToInt32(collection["chatId"]);
            message.FK_messages_user = UserService.GetUserByEmail(User.Identity.Name).Id;
            message.message = collection["message"];
            message.time_stamp = DateTime.Now;
            
            try
            {
                ChatService.CreateMessage(message);
            }
            catch (Exception)
            {
                return Json(new { error = "Couldnt save message to server"});
            }
            AspNetUsers sender;
            try
            {
                sender = UserService.GetUserByEmail(User.Identity.Name);
            }
            catch (Exception)
            {
                return Json("Couldnt get user for message");
            }


            var retObj = new
            {
                id = message.C_ID,
                sender = sender.NickName,
                message = message.message,
                timeStamp = message.time_stamp
            };

            return Json(retObj);
        }

        /* <summary>
        *   Takes a chatId and a lastId
        *   Used to update the chat
        * </summary>
        * <param name="chatId">The Id of the chat we're getting messages for</param>
        * <param name="lastId">the id of the last message a.k.a the hight id in the chat</param>
        * <returns>JSON object of the messages that have an id higher than lastId and belong to the chat with id equals to chatId</returns>
        * <author>Janus</author>
        */

        [HttpPost]
        public ActionResult GetChatUpdates(FormCollection collection){
            chats chat = ChatService.GetChatById(Convert.ToInt32(collection["chatId"]));
            // Select all messages with that fulfill the requirement -> id > lastId | and belong to the chat that has the id == chatId
            List<messages> retMessages = ChatService.GetMessages(chat).Where(x => x.C_ID > Convert.ToInt32(collection["lastId"])).ToList();


            int highestId = (from x in retMessages
                             orderby x.C_ID descending
                             select x.C_ID).FirstOrDefault();
            // Create a new anonymous object with all message info to turn into a json string
            var retObj = new
            {
                id =        (from message in retMessages
                             select message.C_ID).ToList(),
                sender =    (from message in retMessages
                            select message.AspNetUsers.NickName).ToList(),
                message =   (from message in retMessages
                            select message.message).ToList(),
                timeStamp = (from message in retMessages
                            select message.time_stamp).ToList(),
                highest = highestId
                
            };
            return Json(retObj);
        }

        /* <summary>
        *   Gets all messages in the database that belong to the chat with the id equaling chatId parameter
        * </summary>
        * <param name="chatId">The id of the chat the message belongs too</param>
        * <returns>JSON object of the messages </returns>
        * <author>Janus</author>
        */

        [HttpPost]
        public ActionResult GetAllMessagesFromChat(FormCollection collection)
        {

            // Here we get all messeages from the chat with the id chatId
            chats chat = ChatService.GetChatById(Convert.ToInt32(collection["chatId"]));
            List<messages> messages = ChatService.GetMessages(chat);

            // Get highest value
            int? highestId = (from x in messages
                             orderby x.C_ID descending
                             select x.C_ID).FirstOrDefault();
            if (!highestId.HasValue) {
                highestId = 1;
            }

            // Create an anonymous object to return as a json string
            var retObj = new
            {
                id =        (from message in messages
                                select message.C_ID).ToList(),
                sender =    (from message in messages
                            select message.AspNetUsers.NickName).ToList(),
                message =   (from message in messages
                            select message.message).ToList(),
                timeStamp = (from message in messages
                             select message.time_stamp).ToList(),
                highestId = highestId
            };
           
            return Json(retObj);
        }

        /* <summary>
        * Gets all chats the user is a part of
        * </summary>
        * <param name="chatId"The i</param>
        * <returns>JSON object of the users </returns>
        * <author>Janus</author>
        */

        [HttpPost]
        public ActionResult GetUserChats()
        {
            // Identity Check
            if (!User.Identity.IsAuthenticated){
                return Json("No Authentication");
            }

            
            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);
            
            // Get all chats 
            List<chats> usersChats;
            usersChats = ChatService.GetAllChats(user);

            // Create a anonymous object to return as a json string
            var retObj = new
            {
                chatName = (from chat in usersChats
                            select chat.chat_name).ToList(),
                chatId = (from chat in usersChats
                          select chat.C_ID).ToList()
            };


            return Json(retObj);
        }

        /* <summary>
        *   Gets all messages in the database that belong to the chat with the id equaling chatId parameter
        * </summary>
        * <param name="chatId">The id of the chat the message belongs too</param>
        * <returns>JSON object of the messages </returns>
        * <author>Janus</author>
        */

        [HttpPost]
        public ActionResult GetChatUsers(FormCollection collection) 
        {
            int id = Convert.ToInt32(collection["chatId"]);
            chats chat = ChatService.GetChatById(id);
            
            //Get the users belonging to the chat we querried above
            List<AspNetUsers> chatUsers = ChatService.GetChatUsers(chat);
            
            // Create a anonymous object and return it as a json string
            var retObj = new
            {   
                userId = (from user in chatUsers
                         select user.Id).ToList(),
                userName = (from user in chatUsers
                            select user.NickName).ToList(),
                profileImage = (from user in chatUsers
                            select user.profile_image).ToList()
            };
            return Json(retObj);
        }


        /* <summary>
        * Creates a chat between to individuals
        * </summary>
        * <param name="friendId">The id of the friend in the chat</param>
        * <returns>JSON object of the chatId </returns>
        * <author>Janus</author>
        */

        [HttpPost]
        public ActionResult Create(FormCollection fc)
        {
            // Get both users as AspNetUsers, as required by the CreateChat function in ChatService
            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);
            AspNetUsers friend = UserService.GetUserById(fc["friendId"]);

            // Create a chat with the users and we get the new chat returned to us
            chats chat = ChatService.CreateChat(user, friend);

            // Return the chatId as an Json object
            return Json(new 
            { 
                chatId = chat.C_ID
            });
        }

        /* <summary>
        * Renames the chat in the database that has the given chatId
        * </summary>
        * <param name="chatId">The id of the chat to rename </param>
        * <returns>returns Nothing</returns>
        * <author>Janus</author>
        */

        [HttpPost]
        public ActionResult Rename(FormCollection fc)
        {
        	chats chat = new chats();
            chat.chat_name = fc["newName"];
            chat.C_ID = Convert.ToInt32(fc["chatId"]);

            try
            {
                ChatService.RenameChat(chat);
            }
            catch(Exception)
            {
            }
            return Json("");
        }
	}
}