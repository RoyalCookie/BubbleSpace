using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bubblespace.Services;
using Bubblespace.Models;

namespace Bubblespace.Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        [TestInitialize]
        public void Initialize()
        {
            var db = new Bubblespace.VERK2015_H17Entities1();

        }

        [TestMethod]
        public void TestGetAllFriendsForDroken()
        {
            // Arrange:
            var userDroken = UserService.GetUserByEmail("asdasd@asda.sa");
            
            // Act:
            var friends = UserService.GetAllFriends(userDroken);

            // Assert:
            Assert.AreEqual(2, friends.Count);
        }

        [TestMethod]
        public void TestGetUserPostsByDroken()
        {
            // Arrange:
            var userDroken = UserService.GetUserByEmail("asdasd@asda.sa");

            // Act: 
            var posts = UserService.GetUsersPosts(userDroken);

            // Assert:
            Assert.AreEqual(2, posts.Count);
        }

        [TestMethod]
        public void TestGetEventsCreatedByDroken()
        {
            // Arrange:
            var userDroken = UserService.GetUserByEmail("asdasd@asda.sa");

            // Act: 
            var events = UserService.GetAllEventsCreatedByUser(userDroken);

            // Assert:
            Assert.AreEqual(0, events.Count);
        }
    }
}
