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
        public ActionResult Send(FormCollection fc)
        {
            messages message = new messages();
            message.FK_messages_chat_id = Convert.ToInt32(fc["chat_id"]);
            message.FK_messages_user = fc["user"];
            message.message = fc["message"];
            message.time_stamp = DateTime.Now;
            try
            {
                ChatService.CreateMessage(message);
            }
            catch (Exception)
            {

            }

            return Json("");
        }

        [HttpPost]
        public ActionResult GetUserChats()
        {
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
        public ActionResult GetChatUsers(int id) 
        {
            chats chat = ChatService.GetChatById(id);
            List<AspNetUsers> chatUsers = ChatService.GetChatUsers(chat);
            var retObj = new
            {
                userId = (from user in chatUsers
                         select user.Id).ToList(),
                userName = (from user in chatUsers
                            select user.NickName).ToList(),
                userProfileImage = (from user in chatUsers
                            select user.profile_image).ToList()
            };
            return Json(retObj);
        }

        public ActionResult Create(FormCollection fc)
        {
            chats chat = new chats();
            //TODO: fix this
            //chat. = new chat_members
            //{

            //};
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