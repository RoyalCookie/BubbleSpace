using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bubblespace.Services
{
	public class ChatService
	{
        static public void CreateMessage(messages message)
        {
            var db = new VERK2015_H17Entities1();
            db.messages.Add(message);
            db.SaveChanges();
        }
        static public void CreateChat(chats chat)
        {
            var db = new VERK2015_H17Entities1();
            db.chats.Add(chat);
            db.SaveChanges();
        }
        static public void RenameChat(chats chat)
        {
            var db = new VERK2015_H17Entities1();
            var getChat = (from x in db.chats.Where(y => y.C_ID == chat.C_ID)
                           select x).SingleOrDefault();
            getChat.chat_name = chat.chat_name;
            db.SaveChanges(); 
        }
       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
		static public List<messages>GetMessages(chats chat)
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
        static public List<chat_members> GetChatUsers(chats chat)
		{
            var db = new VERK2015_H17Entities1();
            var chatUsers = (from x in db.chat_members.Where(y => y.FK_chat_members_chat == chat.C_ID)
                             select x).ToList();
			return chatUsers;
		}

        /* <summary></summary>
        * <param name="chat">The chat we add the user too</param>
        * <param name="user">The user we add to the chat</param>
        * <returns>nothing</returns>
        * <author></author>
        */
        static public void AddChatUsers(chats chat, AspNetUsers user)
        {
            var db = new VERK2015_H17Entities1();
            
            List<chat_members> allChatMemberEntries = GetAllChatMemberEntries();

            bool addUser = (from isMember in allChatMemberEntries
                           where isMember.FK_chat_members_user == user.Id && isMember.FK_chat_members_chat == chat.C_ID
                           select false).DefaultIfEmpty(true).Single();
            
            if(addUser){
                chat_members member = new chat_members();
                member.FK_chat_members_chat = chat.C_ID;
                member.FK_chat_members_user = user.Id;
                db.chat_members.Add(member);
                db.SaveChanges();
            }

        }

       /* <summary></summary>
        * <param name="ID"></param>
        * <returns></returns>
        * <author></author>
        */
        static public List<messages> GetMessagesSince(chats chat, DateTime date)
		{
            var db = new VERK2015_H17Entities1();
            var messages = (from x in db.messages.Where(y => y.FK_messages_chat_id == chat.C_ID || y.time_stamp >= date)
                            select x).ToList();
			return messages;
		}
        /* <summary>Gets all the chats for a specified user</summary>
         * <param name="user">Takes in obj of user</param>
         * <returns>list of chats for the user</returns>
         * <author>Valgeir</author>
         */
        static public List<chats> GetAllChats(AspNetUsers user)
        {
            //TODO: Change from string to object of user

            var db = new VERK2015_H17Entities1();

            return db.chats.ToList();
        }

        static public List<chat_members> GetAllChatMemberEntries()
        {
            var db = new VERK2015_H17Entities1();

            return db.chat_members.ToList();
        }
	}	
}