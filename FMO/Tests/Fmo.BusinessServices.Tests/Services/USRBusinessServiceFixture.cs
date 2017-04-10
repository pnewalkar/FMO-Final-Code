using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.Enums;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using Fmo.Common.Interface;
using System.Data.Entity.Spatial;
using Fmo.DTO;

namespace Fmo.BusinessServices.Tests.Services
{
    [TestFixture]
    public class USRBusinessServiceFixture : TestFixtureBase
    {
        private IUSRBusinessService testCandidate;
        private Mock<IAddressLocationRepository> addressLocationRepositoryMock;
        private Mock<IDeliveryPointsRepository> deliveryPointsRepositoryMock;
        private Mock<INotificationRepository> notificationRepositoryMock;
        private Mock<IPostCodeSectorRepository> postCodeSectorRepositoryMock;
        private Mock<IReferenceDataCategoryRepository> referenceDataCategoryRepositoryMock;
        private Mock<IEmailHelper> emailHelperMock;
        private Mock<IConfigurationHelper> configurationHelperMock;

        protected override void OnSetup()
        {
            addressLocationRepositoryMock = CreateMock<IAddressLocationRepository>();
            deliveryPointsRepositoryMock = CreateMock<IDeliveryPointsRepository>();
            notificationRepositoryMock = CreateMock<INotificationRepository>();
            postCodeSectorRepositoryMock = CreateMock<IPostCodeSectorRepository>();
            referenceDataCategoryRepositoryMock = CreateMock<IReferenceDataCategoryRepository>();
            configurationHelperMock = CreateMock<IConfigurationHelper>();
            testCandidate = new USRBusinessService(
                                            addressLocationRepositoryMock.Object,
                                            deliveryPointsRepositoryMock.Object,
                                            notificationRepositoryMock.Object,
                                            postCodeSectorRepositoryMock.Object,
                                            referenceDataCategoryRepositoryMock.Object,
                                            emailHelperMock.Object, 
                                            configurationHelperMock.Object);
        }

        public async Task SaveUSRDetails_Check_New_Address_Location()
        {
            /*addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(new DTO.AddressLocationDTO());
            deliveryPointsRepositoryMock.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).Returns(new DTO.DeliveryPointDTO());
            deliveryPointsRepositoryMock.Setup(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DbGeometry>()));
            notificationRepositoryMock.Setup(x => x.GetNotificationByUDPRN(It.IsAny<int>())).Returns(new DTO.NotificationDTO());
            notificationRepositoryMock.Setup(x => x.AddNewNotification(It.IsAny<NotificationDTO>())).Returns(Task.FromResult(10));
            notificationRepositoryMock.Setup(x => x.AddNewNotification(It.IsAny<NotificationDTO>())).Returns(Task.FromResult(10));*/
        }

    }
}
