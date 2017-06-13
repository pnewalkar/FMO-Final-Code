using Moq;
using NUnit.Framework;
using RM.Common.Notification.WebAPI.BusinessService;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.Common.Notification.WebAPI.Test
{
    [TestFixture]
    public class NotificationBusinessServiceFixture : TestFixtureBase
    {
        private Mock<INotificationDataService> mockNotificationDataService;
        private INotificationBusinessService testCandidate;

        [Test]
        public void Test_AddNewNotification()
        {
            var result = testCandidate.AddNewNotification(new NotificationDTO() { });
            Assert.NotNull(result);
            Assert.AreEqual(result.Result, 1);
        }

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

            mockNotificationDataService.Setup(x => x.AddNewNotification(It.IsAny<NotificationDTO>())).ReturnsAsync(1);
            mockNotificationDataService.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            mockNotificationDataService.Setup(x => x.DeleteNotificationbyUDPRNAndAction(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(1);
            mockNotificationDataService.Setup(x => x.GetNotificationByUDPRN(It.IsAny<int>())).ReturnsAsync(new NotificationDTO() { });

            testCandidate = new NotificationBusinessService(mockNotificationDataService.Object);
        }
    }
}