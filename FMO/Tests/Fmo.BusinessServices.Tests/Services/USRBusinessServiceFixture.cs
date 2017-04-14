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
using System.Net.Mail;
using Fmo.Common.Constants;
using Fmo.DTO.FileProcessing;

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
        private Mock<ILoggingHelper> loggingHelperMock;

        [Test]
        public void SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Null_Location_Existing_Notification()
        {
            //Setup mock objects
            AddressLocationDTO addressDTOMockNull = null;

            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRFromEmail")).Returns("sriram.kandade@capgemini.com");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRSubject")).Returns("USR file error");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRBody")).Returns("UDPRN - {0} already exists");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRToEmail")).Returns("sriram.kandade@capgemini.com");
            addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(addressDTOMockNull);
            deliveryPointsRepositoryMock.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).Returns(new DeliveryPointDTO());

            notificationRepositoryMock.Setup(x => x.GetNotificationByUDPRN(It.IsAny<int>())).Returns(new NotificationDTO());
            testCandidate.SaveUSRDetails(new AddressLocationUSRPOSTDTO { udprn = 0, xCoordinate = 0, yCoordinate = 0});

            emailHelperMock.Verify(x => x.SendMessage(It.IsAny<MailMessage>()), Times.Never);
            addressLocationRepositoryMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDTO>()), Times.Once);
            deliveryPointsRepositoryMock.Verify(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDTO>()), Times.Once);
            notificationRepositoryMock.Verify(x => x.DeleteNotificationbyUDPRNAndAction(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Null_Location_Non_Existing_Notification()
        {
            //Setup mock objects
            AddressLocationDTO addressDTOMockNull = null;
            NotificationDTO notificationDTOMockNull = null;

            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRFromEmail")).Returns("sriram.kandade@capgemini.com");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRSubject")).Returns("USR file error");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRBody")).Returns("UDPRN - {0} already exists");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRToEmail")).Returns("sriram.kandade@capgemini.com");
            addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(addressDTOMockNull);
            addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(new AddressLocationDTO()
            {
                Lattitude = It.IsAny<decimal>(),
                Longitude = It.IsAny<decimal>(),
                LocationXY = null,
                UDPRN = It.IsAny<int>()
            });

            notificationRepositoryMock.Setup(x => x.GetNotificationByUDPRN(It.IsAny<int>())).Returns(notificationDTOMockNull);
            deliveryPointsRepositoryMock.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).Returns(new DeliveryPointDTO());
            testCandidate.SaveUSRDetails(new AddressLocationUSRPOSTDTO { udprn = 0, xCoordinate = 0, yCoordinate = 0 });

            emailHelperMock.Verify(x => x.SendMessage(It.IsAny<MailMessage>()), Times.Never);
            addressLocationRepositoryMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDTO>()), Times.Once);
            deliveryPointsRepositoryMock.Verify(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDTO>()), Times.Once);
            notificationRepositoryMock.Verify(x => x.DeleteNotificationbyUDPRNAndAction(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Existing_Location_Within_Range_Non_Existing_Notification()
        {
            //Setup mock objects
            AddressLocationDTO addressDTOMockNull = null;
            NotificationDTO notificationDTOMockNull = null;

            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRFromEmail")).Returns("sriram.kandade@capgemini.com");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRSubject")).Returns("USR file error");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRBody")).Returns("UDPRN - {0} already exists");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRToEmail")).Returns("sriram.kandade@capgemini.com");
            addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(addressDTOMockNull);
            addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(new AddressLocationDTO()
            {
                Lattitude = It.IsAny<decimal>(),
                Longitude = It.IsAny<decimal>(),
                LocationXY = null,
                UDPRN = It.IsAny<int>()
            });

            notificationRepositoryMock.Setup(x => x.GetNotificationByUDPRN(It.IsAny<int>())).Returns(notificationDTOMockNull);
            deliveryPointsRepositoryMock.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).Returns(new DeliveryPointDTO());
            testCandidate.SaveUSRDetails(new AddressLocationUSRPOSTDTO { udprn = 0, xCoordinate = 0, yCoordinate = 5 });

            emailHelperMock.Verify(x => x.SendMessage(It.IsAny<MailMessage>()), Times.Never);
            addressLocationRepositoryMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDTO>()), Times.Once);
            deliveryPointsRepositoryMock.Verify(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDTO>()), Times.Once);
            notificationRepositoryMock.Verify(x => x.DeleteNotificationbyUDPRNAndAction(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        /*public void SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Location_Within_Tolerance_Existing_Notification()
        {
            //Setup mock objects
            AddressLocationDTO addressDTOMockNull = null;
            NotificationDTO notificationDTOMockNull = null;

            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRFromEmail")).Returns("sriram.kandade@capgemini.com");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRSubject")).Returns("USR file error");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRBody")).Returns("UDPRN - {0} already exists");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRToEmail")).Returns("sriram.kandade@capgemini.com");
            addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(addressDTOMockNull);
            addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(new AddressLocationDTO()
            {
                Latitude = It.IsAny<decimal>(),
                Longitude = It.IsAny<decimal>(),
                LocationXY = null,
                UDPRN = It.IsAny<int>()
            });

            notificationRepositoryMock.Setup(x => x.GetNotificationByUDPRN(It.IsAny<int>())).Returns(notificationDTOMockNull);

            emailHelperMock.Verify(x => x.SendMessage(It.IsAny<MailMessage>()), Times.Never);
            addressLocationRepositoryMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDTO>()), Times.Once);
            deliveryPointsRepositoryMock.Verify(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DbGeometry>()), Times.Once);
            notificationRepositoryMock.Verify(x => x.DeleteNotificationbyUDPRNAndAction(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }*/

        protected override void OnSetup()
        {
            addressLocationRepositoryMock = CreateMock<IAddressLocationRepository>();
            deliveryPointsRepositoryMock = CreateMock<IDeliveryPointsRepository>();
            notificationRepositoryMock = CreateMock<INotificationRepository>();
            postCodeSectorRepositoryMock = CreateMock<IPostCodeSectorRepository>();
            referenceDataCategoryRepositoryMock = CreateMock<IReferenceDataCategoryRepository>();
            emailHelperMock = CreateMock<IEmailHelper>();
            configurationHelperMock = CreateMock<IConfigurationHelper>();
            loggingHelperMock = CreateMock<ILoggingHelper>();
            testCandidate = new USRBusinessService(
                                            addressLocationRepositoryMock.Object,
                                            deliveryPointsRepositoryMock.Object,
                                            notificationRepositoryMock.Object,
                                            postCodeSectorRepositoryMock.Object,
                                            referenceDataCategoryRepositoryMock.Object,
                                            emailHelperMock.Object,
                                            configurationHelperMock.Object,
                                            loggingHelperMock.Object
                                            );
        }

    }
}
