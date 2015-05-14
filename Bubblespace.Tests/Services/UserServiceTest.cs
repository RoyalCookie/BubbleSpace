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

        }

        [TestMethod]
        public void TestGetAllFriendsForDroken()
        {
            // Arrange:
            var userDroken = UserService.GetUserByEmail("asdasd@asda.sa");
            
            // Act:
            var friends = UserService.GetAllFriends(userDroken);

            // Assert:
            Assert.AreEqual(3, friends.Count);
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
