using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bubblespace.Services;
using Bubblespace.Tests;

namespace Bubblespace.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private UserService _service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();
            AspNetUsers userDroken = new AspNetUsers
            {
                Id = "a52bb3e1-9554-4ffa-b398-b635945bcddb",
                Email = "asdasd@asda.sa",
                NickName = "Droken",
                UserName = "asdasd@asda.sa"
            };
            mockDb.User.Add(userDroken);
            AspNetUsers userKayui = new AspNetUsers
            {
                Id = "55cef877-2f03-4295-920c-6cffa5472329",
                Email = "andrirafna@gmail.com",
                NickName = "Kayui",
                UserName = "andrirafna@gmail.com"
            };
            mockDb.User.Add(userKayui);


            _service = new UserService();
        }

        [TestMethod]
        public void TestGetAllFriendsForDroken()
        {
            // Arrange:
            
            // Act:

            // Assert:
        }
    }
}
