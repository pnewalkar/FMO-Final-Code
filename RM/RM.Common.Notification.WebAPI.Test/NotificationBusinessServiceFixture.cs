﻿using System;
using Moq;
using NUnit.Framework;
using RM.Common.Notification.WebAPI.BusinessService;
using RM.Common.Notification.WebAPI.DataService.Interface;
using RM.Common.Notification.WebAPI.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.Common.Notification.WebAPI.Test
{
    [TestFixture]
    public class NotificationBusinessServiceFixture : TestFixtureBase
    {
        private Mock<INotificationDataService> mockNotificationDataService;
        private INotificationBusinessService testCandidate;

        private Mock<ILoggingHelper> mockLoggingHelper;

        /*[Test]
        public void Test_AddNewNotification()
        {
            var result = testCandidate.AddNewNotification(new NotificationDTO() { });
            Assert.NotNull(result);
            Assert.AreEqual(result.Result, 1);
        }*/

        [Test]
        public void Test_CheckIfNotificationExists()
        {
            var result = testCandidate.CheckIfNotificationExists(1, "abc");
            Assert.NotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Test_DeleteNotificationbyUDPRNAndAction()
        {
            var result = testCandidate.DeleteNotificationbyUDPRNAndAction(1, "abc");
            Assert.NotNull(result);
            Assert.AreEqual(result.Result, 1);
        }

        [Test]
        public void Test_GetNotificationByUDPRN()
        {
            var result = testCandidate.GetNotificationByUDPRN(12345);
            Assert.NotNull(result);
        }

        protected override void OnSetup()
        {
            mockNotificationDataService = CreateMock<INotificationDataService>();
            mockLoggingHelper = CreateMock<ILoggingHelper>();

            // mockNotificationDataService.Setup(x => x.AddNewNotification(It.IsAny<NotificationDTO>())).ReturnsAsync(1);
            mockNotificationDataService.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            mockNotificationDataService.Setup(x => x.DeleteNotificationbyUDPRNAndAction(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(1);
            // mockNotificationDataService.Setup(x => x.GetNotificationByUDPRN(It.IsAny<int>())).ReturnsAsync(new NotificationDTO() { });

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new NotificationBusinessService(mockNotificationDataService.Object, mockLoggingHelper.Object);
        }
    }
}