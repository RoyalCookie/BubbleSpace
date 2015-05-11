using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Services
{
	public class ChatService
	{
       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public List<messages>GetMessages(chats chat)
		{
            var db = new VERK2015_H17Entities1();
            var messages = (from x in db.messages.Where(y => y.FK_messages_chat_id == chat.C_ID)
                            select x).ToList();
            return messages;
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public List<chat_members>GetChatUsers(chats chat)
		{
            var db = new VERK2015_H17Entities1();
            var chatUsers = (from x in db.chat_members.Where(y => y.FK_chat_members_chat == chat.C_ID)
                             select x).ToList();
			return chatUsers;
		}

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		public List<messages>GetMessagesSince(chats chat, DateTime date)
		{
            var db = new VERK2015_H17Entities1();
            var messages = (from x in db.messages.Where(y => y.FK_messages_chat_id == chat.C_ID).Where(y => y.time_stamp >= date)
                            select x).ToList();
			return messages;
		}
	}	
}