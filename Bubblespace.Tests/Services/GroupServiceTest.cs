using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bubblespace.Models;
using Bubblespace.Services;

namespace Bubblespace.Tests.Services
{
    [TestClass]
    public class GroupServiceTest
    {
        [TestMethod]
        public void TestGetAllGroupPostForPrufuGroup()
        {
            // Arrange:
            var grpPrufuGroup = new bubble_groups
            {
                C_ID = 14,
                group_name = "PrufuGroup",
            };

            // Act:
            var grpPosts = GroupService.GetAllGroupPosts(grpPrufuGroup);

            // Assert:
            Assert.AreEqual(1, grpPosts.Count);
        }
    }
}
