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

        [HttpPost]
        public ActionResult GetLiveMessages(FormCollection collection){
            chats chat = ChatService.GetChatById(Convert.ToInt32(collection["chatId"]));
            List<messages> retMessages = ChatService.GetMessages(chat).Where(x => x.C_ID > Convert.ToInt32(collection["lastId"])).ToList();
            var retObj = new
            {
                id =        (from message in retMessages
                            select message.C_ID).ToList(),
                sender =    (from message in retMessages
                            select message.AspNetUsers.NickName).ToList(),
                message =   (from message in retMessages
                            select message.message).ToList(),
                timeStamp = (from message in retMessages
                            select message.time_stamp).ToList()
            };
            return Json(retObj);
        }
        
        [HttpPost]
        public ActionResult GetAllMessagesFromChat(FormCollection collection) 
        {
            chats chat = ChatService.GetChatById(Convert.ToInt32(collection["chatId"]));
            List<messages> messages = ChatService.GetMessages(chat);
            var retObj = new 
            {
                id =        (from message in messages
                            select message.C_ID).ToList(),
                sender =    (from message in messages
                            select message.AspNetUsers.NickName).ToList(),
                message =   (from message in messages
                            select message.message).ToList(),
                timeStamp = (from message in messages
                            select message.time_stamp).ToList()
            };
            return Json(retObj);
        }

        [HttpPost]
        public ActionResult GetUserChats()
        {
            if (!User.Identity.IsAuthenticated){
                return Json("No Authentication");
            }

            AspNetUsers user = UserService.GetUserByEmail(User.Identity.Name);
            List<chats> usersChats;
            try 
            {
                usersChats = ChatService.GetAllChats(user);
            }
            catch(Exception)
            {
                return Json("Error");
            }
            System.Diagnostics.Debug.WriteLine("Line 1");
            var retObj = new
            {
                chatName = (from chat in usersChats
                            select chat.chat_name).ToList(),
                chatId = (from chat in usersChats
                          select chat.C_ID).ToList()
            };

            System.Diagnostics.Debug.WriteLine("Line 2");
            return Json(retObj);
        }

        [HttpPost]
        public ActionResult GetChatUsers(FormCollection collection) 
        {
            int id = Convert.ToInt32(collection["chatId"]);
            chats chat = ChatService.GetChatById(id);
            List<AspNetUsers> chatUsers = ChatService.GetChatUsers(chat);
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

        public ActionResult Create(FormCollection fc)
        {
            chats chat = new chats();
            chat.chat_name = fc["chat_name"];

            try
            {
                ChatService.CreateChat(chat);
            }
            catch(Exception)
            {

            }
            return View();
        }

        public ActionResult Rename(FormCollection fc)
        {
        	chats chat = new chats();
            chat.chat_name = fc["new_chat_name"];

            try
            {
                ChatService.RenameChat(chat);
            }
            catch(Exception)
            {

            }

            return View();
        }
	}
}