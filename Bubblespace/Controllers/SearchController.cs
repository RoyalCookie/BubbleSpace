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
        public ActionResult GetResults(FormCollection fc)
        {            
            List<List<List<string>>> returnJson = new List<List<List<string>>>();
            /*
                [0] = Users
                [1] = Groups
                [2] = Events
            */

            // Users
            List<List<string>> userInfo = new List<List<string>>();
            AspNetUsers user = new AspNetUsers();
            user.NickName = fc["search_string"];
            var userSearchResults = SearchService.SearchUsersByName(user);
            var userNames = (from usr in userSearchResults
                              select usr.NickName).ToList();
            var userImages = (from usr in userSearchResults
                              select usr.profile_image).ToList();
            var userId = (from usr in userSearchResults
                          select usr.Id).ToList();
            userInfo.Add(userNames);
            userInfo.Add(userImages);
            userInfo.Add(userId);

            // Groups
            List<List<string>> groupInfo = new List<List<string>>();
            bubble_groups bGroup = new bubble_groups();
            bGroup.group_name = fc["search_string"];
            var groupSearchResults = SearchService.SearchGroupByName(bGroup);
            var groupNames = (from grp in groupSearchResults
                              select grp.group_name).ToList();
            var groupImages = (from grp in groupSearchResults
                               select grp.group_profile_image).ToList();
            var groupId = (from grp in groupSearchResults
                           select grp.C_ID.ToString()).ToList();
            groupInfo.Add(groupNames);
            groupInfo.Add(groupImages);
            groupInfo.Add(groupId);

            // Events
            List<List<string>> eventInfo = new List<List<string>>();
            events evnts = new events();
            evnts.event_name = fc["search_string"];
            var eventSearchResults = SearchService.SearchEventsByName(evnts);
            var eventNames = (from eve in eventSearchResults
                              select eve.event_name).ToList();
            var eventImages = (from eve in eventSearchResults
                               select eve.event_profile_image).ToList();
            var eventId = (from eve in eventSearchResults
                           select eve.C_ID.ToString()).ToList();
            eventInfo.Add(eventNames);
            eventInfo.Add(eventImages);
            eventInfo.Add(eventId);

            returnJson.Add(userInfo);
            returnJson.Add(groupInfo);
            returnJson.Add(eventInfo);

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