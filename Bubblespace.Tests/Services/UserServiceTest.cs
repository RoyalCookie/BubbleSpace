using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bubblespace.Services;
using Bubblespace.Models;

namespace Bubblespace.Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        [TestMethod]
        public void TestGetAllFriendsForDroken()
        {
            // Arrange:
            var db = new Bubblespace.VERK2015_H17Entities1();
            AspNetUsers userDroken = new AspNetUsers
            {
                Id = "a52bb3e1-9554-4ffa-b398-b635945bcddb",
                Email = "asdasd@asda.sa",
                NickName = "Droken",
                UserName = "asdasd@asda.sa"
            };

            // Act:
            var friends = UserService.GetAllFriends(userDroken);

            // Assert:
            Assert.AreEqual(2, friends.Count);
        }
    }
}
