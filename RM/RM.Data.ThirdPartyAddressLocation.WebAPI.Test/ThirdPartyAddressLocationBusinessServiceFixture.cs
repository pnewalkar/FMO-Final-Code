using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.FileProcessing;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.ThirdPartyAddressLocation.WebAPI.BusinessService;
using RM.DataManagement.ThirdPartyAddressLocation.WebAPI.IntegrationService;

namespace RM.Data.ThirdPartyAddressLocation.WebAPI.Test
{
    [TestFixture]
    public class ThirdPartyAddressLocationBusinessServiceFixture : TestFixtureBase
    {
        private ThirdPartyAddressLocationBusinessService testCandidate;
        private Mock<IAddressLocationDataService> addressLocationDataServiceMock;
        private Mock<IThirdPartyAddressLocationIntegrationService> ThirdPartyAddressLocationIntegrationServiceMock;
        private Mock<ILoggingHelper> loggingHelperMock;
        private Mock<IConfigurationHelper> configurationHelperMock;

        /// <summary>
        /// Test the method get Address location by Udprn.
        /// </summary>
        [Test]
        public async Task Test_Address_Location_By_Udprn()
        {
            int Udprn = 0;

            Exception mockException = It.IsAny<Exception>();
            var result = await testCandidate.GetAddressLocationByUDPRN(Udprn);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test the method get Address Location By Udprn Json.
        /// </summary>
        [Test]
        public async Task Test_Address_Location_By_UDPRN_Json()
        {
            int Udprn = 0;
            Exception mockException = It.IsAny<Exception>();
            var result = testCandidate.GetAddressLocationByUDPRNJson(Udprn);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test the method get Address location by Udprn.
        /// </summary>
        [Test]
        public async Task Test_Save_USR_Details_Valid_Scenario()
        {
            List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos = new List<AddressLocationUSRPOSTDTO>() { new AddressLocationUSRPOSTDTO { UDPRN = 1234, XCoordinate = 12, YCoordinate = 10 } };
            ThirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).Returns(Task.FromResult(true));
            ThirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), Constants.USRACTION)).Returns(Task.FromResult(true));
            Exception mockException = It.IsAny<Exception>();

            var result = testCandidate.SaveUSRDetails(addressLocationUsrpostdtos);
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_Save_USR_Details_Invalid_Scenario()
        {
            List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos = new List<AddressLocationUSRPOSTDTO>() { new AddressLocationUSRPOSTDTO { UDPRN = 1234, XCoordinate = 12, YCoordinate = 10 } };
            ThirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).Returns(Task.FromResult(true));
            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);
            ThirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetDeliveryPointByUDPRNForThirdParty(It.IsAny<int>())).Returns(Task.FromResult(new DeliveryPointDTO() { LocationXY = unitBoundary }));
            ThirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), Constants.USRACTION)).Returns(Task.FromResult(false));

            Exception mockException = It.IsAny<Exception>();
            var result = testCandidate.SaveUSRDetails(addressLocationUsrpostdtos);
            Assert.NotNull(result);
        }

        [Test]
        public void SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Null_Location_Existing_Notification()
        {
            addressLocationDataServiceMock.Setup(x => x.AddressLocationExists(It.IsAny<int>())).Returns(Task.FromResult(false));
            configurationHelperMock = this.CreateMock<IConfigurationHelper>();
            this.loggingHelperMock = this.CreateMock<ILoggingHelper>();
            AddressLocationDTO addressDTOMockNull = null;

            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRFromEmail")).Returns("sriram.kandade@capgemini.com");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRSubject")).Returns("USR file error");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRBody")).Returns("UDPRN - {0} already exists");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRToEmail")).Returns("sriram.kandade@capgemini.com");
            this.testCandidate.SaveUSRDetails(new List<AddressLocationUSRPOSTDTO> { new AddressLocationUSRPOSTDTO { UDPRN = 0, XCoordinate = 0, YCoordinate = 0 } });
        }

        [Test]
        public void SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Null_Location_Non_Existing_Notification()
        {
            addressLocationDataServiceMock.Setup(x => x.AddressLocationExists(It.IsAny<int>())).Returns(Task.FromResult(false));
            this.configurationHelperMock = this.CreateMock<IConfigurationHelper>();
            this.loggingHelperMock = this.CreateMock<ILoggingHelper>();

            AddressLocationDTO addressDTOMockNull = null;
            NotificationDTO notificationDTOMockNull = null;

            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRFromEmail")).Returns("sriram.kandade@capgemini.com");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRSubject")).Returns("USR file error");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRBody")).Returns("UDPRN - {0} already exists");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRToEmail")).Returns("sriram.kandade@capgemini.com");

            this.testCandidate.SaveUSRDetails(new List<AddressLocationUSRPOSTDTO> { new AddressLocationUSRPOSTDTO { UDPRN = 0, XCoordinate = 0, YCoordinate = 0 } });
        }

        [Test]
        public void SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Existing_Location_Within_Range_Non_Existing_Notification()
        {
            addressLocationDataServiceMock.Setup(x => x.AddressLocationExists(It.IsAny<int>())).Returns(Task.FromResult(false));
            this.configurationHelperMock = this.CreateMock<IConfigurationHelper>();
            this.loggingHelperMock = this.CreateMock<ILoggingHelper>();

            AddressLocationDTO addressDTOMockNull = null;
            NotificationDTO notificationDTOMockNull = null;

            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRFromEmail")).Returns("sriram.kandade@capgemini.com");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRSubject")).Returns("USR file error");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRBody")).Returns("UDPRN - {0} already exists");
            this.configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("USRToEmail")).Returns("sriram.kandade@capgemini.com");

            this.testCandidate.SaveUSRDetails(new List<AddressLocationUSRPOSTDTO> { new AddressLocationUSRPOSTDTO { UDPRN = 0, XCoordinate = 0, YCoordinate = 5 } });
        }

        protected override void OnSetup()
        {
            addressLocationDataServiceMock = new Mock<IAddressLocationDataService>();
            ThirdPartyAddressLocationIntegrationServiceMock = new Mock<IThirdPartyAddressLocationIntegrationService>();
            loggingHelperMock = new Mock<ILoggingHelper>();

            addressLocationDataServiceMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(Task.FromResult(new AddressLocationDTO() { }));

            addressLocationDataServiceMock.Setup(x => x.AddressLocationExists(It.IsAny<int>())).Returns(Task.FromResult(true));
            addressLocationDataServiceMock.Setup(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDTO>())).Returns(Task.FromResult(1));
            ThirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetDeliveryPointByUDPRNForThirdParty(It.IsAny<int>())).Returns(Task.FromResult(new DeliveryPointDTO() { LocationProvider = "abcd" }));
            var Guid = System.Guid.Parse("B13D545D-2DE7-4E62-8DAD-00EC2B7FF8B8");
            ThirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetReferenceDataId(Constants.NETWORKLINKDATAPROVIDER, Constants.EXTERNAL)).Returns(Task.FromResult(Guid));
            ThirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDTO>())).Returns(Task.FromResult(1));

            ThirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.DeleteNotificationbyUDPRNAndAction(It.IsAny<int>(), Constants.USRACTION)).Returns(Task.FromResult(1));
            ThirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetPostalAddress(It.IsAny<int>())).Returns(Task.FromResult(new PostalAddressDTO() { BuildingName = "abcd" }));
            ThirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.AddNewNotification(It.IsAny<NotificationDTO>())).Returns(Task.FromResult(1));

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new ThirdPartyAddressLocationBusinessService(addressLocationDataServiceMock.Object, ThirdPartyAddressLocationIntegrationServiceMock.Object, loggingHelperMock.Object);
        }
    }
}