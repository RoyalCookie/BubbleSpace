using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bubblespace.Services;
using Bubblespace.Models;

namespace Bubblespace.Tests.Services
{
    [TestClass]
    public class ChatServiceTest
    {
        [TestMethod]
        public void TestGetAllChatsForDroken()
        {
            // Arrange:
            var userDroken = UserService.GetUserByEmail("asdasd@asda.sa");

            // Act:
            var chats = ChatService.GetAllChats(userDroken);

            // Assert:
            Assert.AreEqual(1, chats.Count);
        }
    }
}
