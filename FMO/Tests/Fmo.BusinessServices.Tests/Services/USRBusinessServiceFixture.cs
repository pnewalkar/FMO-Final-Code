namespace Fmo.BusinessServices.Tests.Services
{
    using System.Collections.Generic;
    using System.Net.Mail;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.BusinessServices.Services;
    using Fmo.Common.Interface;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.DTO.FileProcessing;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class USRBusinessServiceFixture : TestFixtureBase
    {
        private IUSRBusinessService testCandidate;
        private Mock<IAddressLocationRepository> addressLocationRepositoryMock;
        private Mock<IAddressRepository> addressRepositoryMock;
        private Mock<IDeliveryPointsRepository> deliveryPointsRepositoryMock;
        private Mock<INotificationRepository> notificationRepositoryMock;
        private Mock<IPostCodeSectorRepository> postCodeSectorRepositoryMock;
        private Mock<IReferenceDataCategoryRepository> referenceDataCategoryRepositoryMock;
        private Mock<IConfigurationHelper> configurationHelperMock;
        private Mock<ILoggingHelper> loggingHelperMock;

        [Test]
        public void SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Null_Location_Existing_Notification()
        {
            this.addressLocationRepositoryMock = this.CreateMock<IAddressLocationRepository>();
            this.addressRepositoryMock = this.CreateMock<IAddressRepository>();
            this.deliveryPointsRepositoryMock = this.CreateMock<IDeliveryPointsRepository>();
            this.notificationRepositoryMock = this.CreateMock<INotificationRepository>();
            this.postCodeSectorRepositoryMock = this.CreateMock<IPostCodeSectorRepository>();
            this.referenceDataCategoryRepositoryMock = this.CreateMock<IReferenceDataCategoryRepository>();

            this.configurationHelperMock = this.CreateMock<IConfigurationHelper>();
            this.loggingHelperMock = this.CreateMock<ILoggingHelper>();
            this.testCandidate = new USRBusinessService(
                                            this.addressLocationRepositoryMock.Object,
                                            this.addressRepositoryMock.Object,
                                            this.deliveryPointsRepositoryMock.Object,
                                            this.notificationRepositoryMock.Object,
                                            this.postCodeSectorRepositoryMock.Object,
                                            this.referenceDataCategoryRepositoryMock.Object,
                                            this.configurationHelperMock.Object,
                                            this.loggingHelperMock.Object);
            AddressLocationDTO addressDTOMockNull = null;

            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRFromEmail")).Returns("sriram.kandade@capgemini.com");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRSubject")).Returns("USR file error");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRBody")).Returns("UDPRN - {0} already exists");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRToEmail")).Returns("sriram.kandade@capgemini.com");
            this.addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(addressDTOMockNull);
            this.deliveryPointsRepositoryMock.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).Returns(new DeliveryPointDTO());
            this.deliveryPointsRepositoryMock.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).Returns(true);
            this.notificationRepositoryMock.Setup(x => x.GetNotificationByUDPRN(It.IsAny<int>())).Returns(new NotificationDTO());
            this.notificationRepositoryMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            this.testCandidate.SaveUSRDetails(new List<AddressLocationUSRPOSTDTO> { new AddressLocationUSRPOSTDTO { UDPRN = 0, XCoordinate = 0, YCoordinate = 0 } });

            this.addressLocationRepositoryMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDTO>()), Times.Once);
            this.deliveryPointsRepositoryMock.Verify(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDTO>()), Times.Once);
            this.notificationRepositoryMock.Verify(x => x.DeleteNotificationbyUDPRNAndAction(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Null_Location_Non_Existing_Notification()
        {
            this.addressLocationRepositoryMock = this.CreateMock<IAddressLocationRepository>();
            this.deliveryPointsRepositoryMock = this.CreateMock<IDeliveryPointsRepository>();
            this.addressRepositoryMock = this.CreateMock<IAddressRepository>();
            this.notificationRepositoryMock = this.CreateMock<INotificationRepository>();
            this.postCodeSectorRepositoryMock = this.CreateMock<IPostCodeSectorRepository>();
            this.referenceDataCategoryRepositoryMock = this.CreateMock<IReferenceDataCategoryRepository>();

            this.configurationHelperMock = this.CreateMock<IConfigurationHelper>();
            this.loggingHelperMock = this.CreateMock<ILoggingHelper>();
            this.testCandidate = new USRBusinessService(
                                            this.addressLocationRepositoryMock.Object,
                                            this.addressRepositoryMock.Object,
                                            this.deliveryPointsRepositoryMock.Object,
                                            this.notificationRepositoryMock.Object,
                                            this.postCodeSectorRepositoryMock.Object,
                                            this.referenceDataCategoryRepositoryMock.Object,
                                            this.configurationHelperMock.Object,
                                            this.loggingHelperMock.Object);
            AddressLocationDTO addressDTOMockNull = null;
            NotificationDTO notificationDTOMockNull = null;

            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRFromEmail")).Returns("sriram.kandade@capgemini.com");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRSubject")).Returns("USR file error");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRBody")).Returns("UDPRN - {0} already exists");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRToEmail")).Returns("sriram.kandade@capgemini.com");
            this.addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(addressDTOMockNull);
            this.addressRepositoryMock.Setup(x => x.GetPostalAddress(It.IsAny<int>())).Returns(It.IsAny<PostalAddressDTO>());
            this.addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(new AddressLocationDTO()
            {
                Lattitude = It.IsAny<decimal>(),
                Longitude = It.IsAny<decimal>(),
                LocationXY = null,
                UDPRN = It.IsAny<int>()
            });
            this.deliveryPointsRepositoryMock.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).Returns(true);
            this.notificationRepositoryMock.Setup(x => x.GetNotificationByUDPRN(It.IsAny<int>())).Returns(notificationDTOMockNull);
            this.deliveryPointsRepositoryMock.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).Returns(new DeliveryPointDTO());
            this.testCandidate.SaveUSRDetails(new List<AddressLocationUSRPOSTDTO> { new AddressLocationUSRPOSTDTO { UDPRN = 0, XCoordinate = 0, YCoordinate = 0 } });

            this.addressLocationRepositoryMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDTO>()), Times.Once);
            this.deliveryPointsRepositoryMock.Verify(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDTO>()), Times.Once);
            this.notificationRepositoryMock.Verify(x => x.DeleteNotificationbyUDPRNAndAction(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            this.addressRepositoryMock.Verify(x => x.GetPostalAddress(It.IsAny<PostalAddressDTO>()), Times.Never);
        }

        [Test]
        public void SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Existing_Location_Within_Range_Non_Existing_Notification()
        {
            this.addressLocationRepositoryMock = this.CreateMock<IAddressLocationRepository>();
            this.deliveryPointsRepositoryMock = this.CreateMock<IDeliveryPointsRepository>();
            this.addressRepositoryMock = this.CreateMock<IAddressRepository>();
            this.notificationRepositoryMock = this.CreateMock<INotificationRepository>();
            this.postCodeSectorRepositoryMock = this.CreateMock<IPostCodeSectorRepository>();
            this.referenceDataCategoryRepositoryMock = this.CreateMock<IReferenceDataCategoryRepository>();
            this.configurationHelperMock = this.CreateMock<IConfigurationHelper>();
            this.loggingHelperMock = this.CreateMock<ILoggingHelper>();
            this.testCandidate = new USRBusinessService(
                                            this.addressLocationRepositoryMock.Object,
                                            this.addressRepositoryMock.Object,
                                            this.deliveryPointsRepositoryMock.Object,
                                            this.notificationRepositoryMock.Object,
                                            this.postCodeSectorRepositoryMock.Object,
                                            this.referenceDataCategoryRepositoryMock.Object,
                                            this.configurationHelperMock.Object,
                                            this.loggingHelperMock.Object);
            AddressLocationDTO addressDTOMockNull = null;
            NotificationDTO notificationDTOMockNull = null;

            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRFromEmail")).Returns("sriram.kandade@capgemini.com");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRSubject")).Returns("USR file error");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRBody")).Returns("UDPRN - {0} already exists");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRToEmail")).Returns("sriram.kandade@capgemini.com");
            this.addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(addressDTOMockNull);
            this.addressRepositoryMock.Setup(x => x.GetPostalAddress(It.IsAny<int>())).Returns(It.IsAny<PostalAddressDTO>());
            this.addressLocationRepositoryMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(new AddressLocationDTO()
            {
                Lattitude = It.IsAny<decimal>(),
                Longitude = It.IsAny<decimal>(),
                LocationXY = null,
                UDPRN = It.IsAny<int>()
            });

            this.notificationRepositoryMock.Setup(x => x.GetNotificationByUDPRN(It.IsAny<int>())).Returns(notificationDTOMockNull);
            this.deliveryPointsRepositoryMock.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).Returns(new DeliveryPointDTO());
            this.deliveryPointsRepositoryMock.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).Returns(true);
            this.testCandidate.SaveUSRDetails(new List<AddressLocationUSRPOSTDTO> { new AddressLocationUSRPOSTDTO { UDPRN = 0, XCoordinate = 0, YCoordinate = 5 } });
            this.addressLocationRepositoryMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDTO>()), Times.Once);
            this.deliveryPointsRepositoryMock.Verify(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDTO>()), Times.Once);
            this.addressRepositoryMock.Verify(x => x.GetPostalAddress(It.IsAny<PostalAddressDTO>()), Times.Never);
            this.notificationRepositoryMock.Verify(x => x.DeleteNotificationbyUDPRNAndAction(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        protected override void OnSetup()
        {
        }
    }
}