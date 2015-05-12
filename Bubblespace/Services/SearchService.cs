using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bubblespace.Services
{
    public class SearchService
    {
        public static List<chats> SearchChatByUser(AspNetUsers user)
        {
            var db = new VERK2015_H17Entities1();
            var chats = (from x in db.chats.Where(y => y.chat_members == user)
                         select x).ToList();
            return chats;
        }
        public static List<AspNetUsers> SearchUsersByName(AspNetUsers user)
        {
            var db = new VERK2015_H17Entities1();
            var users = (from x in db.AspNetUsers.Where(y => y.NickName == user.NickName)
                         select x).ToList();
            return users;
        }
    }
}