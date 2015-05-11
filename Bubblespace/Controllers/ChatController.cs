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
        //
        // GET: /Chat/
        public ActionResult Index()
        {
            return View();
        }

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

            return View();
        }

        public ActionResult Get(FormCollection fc)
        {
            chats chat = new chats();
            chat.C_ID = Convert.ToInt32(fc["chat_id"]);
            List<JsonResult> retStr = new List<JsonResult>();
            try
            {
                retStr.Add(Json(ChatService.GetChatUsers(chat)));
                retStr.Add(Json(ChatService.GetMessages(chat)));
            }
            catch(Exception)
            {

            }
            return View(retStr);
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