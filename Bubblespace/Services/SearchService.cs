﻿using System;
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
            var users = (from x in db.AspNetUsers.Where(y => y.NickName.Contains(user.NickName))
                         select x).ToList();
            return users;
        }
        public static List<bubble_groups> SearchGroupByName(bubble_groups gr)
        {
            var db = new VERK2015_H17Entities1();
            var groups = (from x in db.bubble_groups.Where(y => y.group_name.Contains(gr.group_name))
                          select x).ToList();
            return groups;
        }
        public static List<events> SearchEventsByName(events eventName)
        {
            var db = new VERK2015_H17Entities1();
            var bEvent = (from x in db.events.Where(y => y.event_name.Contains(eventName.event_name))
                          select x).ToList();
            return bEvent;
        }
    }
}