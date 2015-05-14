using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bubblespace.Services;

namespace Bubblespace.Tests.Services
{
    [TestClass]
    public class PostServiceTest
    {
        [TestMethod]
        public void TestGetAllPostsFromDroken()
        {
            // Arrange:
            var userDroken = UserService.GetUserByEmail("asdasd@asda.sa");

            // Act: 
            var posts = PostService.GetAllPosts(userDroken);

            // Assert:
            Assert.AreEqual(1, posts.Count);
        }
    }
}
