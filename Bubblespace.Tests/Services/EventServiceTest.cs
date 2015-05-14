using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Bubblespace.Services;
using Bubblespace.Models;

namespace Bubblespace.Tests.Services
{
    [TestClass]
    public class EventServiceTest
    {
        [TestMethod]
        public void TestGetAllEvents()
        {
            // Arrange:
            const int totalEvents = 2;

            // Act:
            var events = EventService.GetAllEvents();

            // Assert:
            Assert.AreEqual(totalEvents, events.Count);
        }
    }
}
