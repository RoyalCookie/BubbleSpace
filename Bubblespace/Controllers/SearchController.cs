using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bubblespace.Services;

namespace Bubblespace.Controllers
{
    public class SearchController : Controller
    {
        [HttpPost]
        public ActionResult Users(FormCollection fc)
        {
            AspNetUsers user = new AspNetUsers();
            user.NickName = fc["prefix_string"];
            var searchResults = SearchService.SearchUsersByName(user);
            List<List<string>> returnJson = new List<List<string>>();
            var userNames = (from usr in searchResults
                              select usr.NickName).ToList();
            var userImages = (from usr in searchResults
                              select usr.profile_image).ToList();
            returnJson.Add(userNames);
            returnJson.Add(userNames);
            return Json(returnJson);
        }
        [HttpPost]
        public ActionResult Groups(FormCollection fc)
        {
            bubble_groups bGroup = new bubble_groups();
            bGroup.group_name = fc["group_name"];
            var groups = SearchService.SearchGroupByName(bGroup);

            return Json(groups);
        }
        [HttpPost]
        public ActionResult Posts(FormCollection fc)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Events(FormCollection fc)
        {
            events sEvent = new events();
            sEvent.event_name = fc["event_name"];
            var eventRes = SearchService.SearchEventsByName(sEvent);

            return Json(eventRes);
        }
        [HttpPost]
        public ActionResult Chat(FormCollection fc)
        {
            chats ch = new chats();
            ch.chat_name = fc["chat_name"];
            var chats = SearchService.SearchChatByName(ch);

            return Json(chats);
        }

    }
}