using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bubblespace.Services
{
    public class SearchService
    {
        /* <summary></summary>
         * <param name="ID"></param>
         * <returns></returns>
         * <author></author>
         */
        public static List<AspNetUsers> SearchUsersByName(AspNetUsers user)
        {
            var db = new VERK2015_H17Entities1();
            var users = (from x in db.AspNetUsers.Where(y => y.NickName.Contains(user.NickName))
                         select x).ToList();
            return users;
        }

        /* <summary></summary>
         * <param name="ID"></param>
         * <returns></returns>
         * <author></author>
         */
        public static List<bubble_groups> SearchGroupByName(bubble_groups gr)
        {
            var db = new VERK2015_H17Entities1();
            var groups = (from x in db.bubble_groups.Where(y => y.group_name.Contains(gr.group_name))
                          select x).ToList();
            return groups;
        }

        /* <summary></summary>
         * <param name="ID"></param>
         * <returns></returns>
         * <author></author>
         */
        public static List<events> SearchEventsByName(events eventName)
        {
            var db = new VERK2015_H17Entities1();
            var bEvent = (from x in db.events.Where(y => y.event_name.Contains(eventName.event_name))
                          select x).ToList();
            return bEvent;
        }

        /* <summary></summary>
         * <param name="ID"></param>
         * <returns></returns>
         * <author></author>
         */
        public static List<chats> SearchChatByName(chats chat)
        {
            var db = new VERK2015_H17Entities1();
            var chatRes = (from x in db.chats.Where(y => y.chat_name.Contains(chat.chat_name))
                           select x).ToList();
            return chatRes;
        }
    }
}