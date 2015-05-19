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
        /* <summary>
        * Grabs all users, groups and events based on a search string
        * and returns it to the user.
        * </summary>
        * <param name="event-name">search string</param>
        * <returns>Json object containing the search results</returns>
        * <author>Andri Rafn</author>
        */
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
    }
}