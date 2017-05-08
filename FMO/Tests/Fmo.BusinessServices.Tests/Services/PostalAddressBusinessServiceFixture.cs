namespace Fmo.BusinessServices.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.BusinessServices.Services;
    using Fmo.Common.Interface;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class PostalAddressBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IAddressRepository> mockAddressRepository;
        private Mock<IReferenceDataCategoryRepository> mockrefDataRepository;
        private Mock<IDeliveryPointsRepository> mockdeliveryPointsRepository;
        private Mock<IAddressLocationRepository> mockaddressLocationRepository;
        private Mock<INotificationRepository> mocknotificationRepository;
        private Mock<IFileProcessingLogRepository> mockfileProcessingLogRepository;
        private Mock<ILoggingHelper> mockloggingHelper;
        private Mock<IConfigurationHelper> configurationHelperMock;
        private IPostalAddressBusinessService testCandidate;

        [Test]
        public void Test_ValidPostalAddressData()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { Address_Id = 10, UDPRN = 14856 } };
            var result = testCandidate.SavePostalAddress(lstPostalAddressDTO, "NYB.CSV");
            mockrefDataRepository.Verify(x => x.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce());
            mockAddressRepository.Verify(x => x.SaveAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>()), Times.Once());
            mockAddressRepository.Verify(x => x.DeleteNYBPostalAddress(It.IsAny<List<int>>(), It.IsAny<Guid>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_InvalidPostalAddressData()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { Address_Id = 10 } };
            var result = testCandidate.SavePostalAddress(lstPostalAddressDTO, "NYB.CSV");
            mockrefDataRepository.Verify(x => x.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeast(2));
            mockAddressRepository.Verify(x => x.SaveAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>()), Times.Never());
            mockAddressRepository.Verify(x => x.DeleteNYBPostalAddress(It.IsAny<List<int>>(), It.IsAny<Guid>()), Times.Never());
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_GetPostalAddressDetails()
        {
            var result = testCandidate.GetPostalAddressDetails(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            Assert.IsNotNull(result);
        }

        [Test]
        public void SavePAFDetails_Check_MatchPostalAddressOnAddress()
        {
            PostalAddressDTO objPostalAddress = new PostalAddressDTO()
            {
                Time = "7/19/2016",
                Date = "8:37:00",
                AmendmentType = "I",
                AmendmentDesc = "new insert",
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
            };
            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
            lstPostalAddress.Add(objPostalAddress);
            AddressLocationDTO objAddressLocation = new AddressLocationDTO()
            {
                UDPRN = 54162428
            };

            mockrefDataRepository.Setup(n => n.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>())).Returns(new Guid("7A976FB6-A113-4F62-B366-10A19DB6DF01")); // for diif GUID
            mockAddressRepository.Setup(n => n.GetPostalAddress(It.IsAny<int>())).Returns(It.IsAny<PostalAddressDTO>());
            mockAddressRepository.Setup(n => n.GetPostalAddress(It.IsAny<PostalAddressDTO>())).Returns(objPostalAddress);
            mockAddressRepository.Setup(n => n.SaveAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>())).Returns(false);
            mockaddressLocationRepository.Setup(n => n.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(objAddressLocation);
            mockdeliveryPointsRepository.Setup(n => n.InsertDeliveryPoint(It.IsAny<DeliveryPointDTO>())).Returns(true);
            mocknotificationRepository.Setup(n => n.AddNewNotification(It.IsAny<NotificationDTO>())).Returns(Task.FromResult(It.IsAny<int>()));

            var result = testCandidate.SavePAFDetails(lstPostalAddress);

            mockrefDataRepository.Verify(n => n.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeast(3));
            mockAddressRepository.Verify(n => n.GetPostalAddress(It.IsAny<int>()), Times.Once());
            mockAddressRepository.Verify(n => n.GetPostalAddress(It.IsAny<PostalAddressDTO>()), Times.Once());
            mockAddressRepository.Verify(n => n.SaveAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>()), Times.Never);
            mockaddressLocationRepository.Verify(n => n.GetAddressLocationByUDPRN(It.IsAny<int>()), Times.Never());
            mockdeliveryPointsRepository.Verify(n => n.InsertDeliveryPoint(It.IsAny<DeliveryPointDTO>()), Times.Never());
            mocknotificationRepository.Verify(n => n.AddNewNotification(It.IsAny<NotificationDTO>()), Times.Never());
        }

        [Test]
        public void SavePAFDetails_Check_NotMatchPostalAddress()
        {
            PostalAddressDTO objPostalAddress = new PostalAddressDTO()
            {
                Time = "7/19/2016",
                Date = "8:37:00",
                AmendmentType = "I",
                AmendmentDesc = "new insert",
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
            };
            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
            lstPostalAddress.Add(objPostalAddress);
            AddressLocationDTO objAddressLocation = new AddressLocationDTO()
            {
                UDPRN = 54162428
            };

            mockrefDataRepository.Setup(n => n.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>())).Returns(new Guid("7A976FB6-A113-4F62-B366-10A19DB6DF01")); // for diif GUID
            mockAddressRepository.Setup(n => n.GetPostalAddress(It.IsAny<int>())).Returns(It.IsAny<PostalAddressDTO>());
            mockAddressRepository.Setup(n => n.GetPostalAddress(It.IsAny<PostalAddressDTO>())).Returns(It.IsAny<PostalAddressDTO>());
            mockAddressRepository.Setup(n => n.SaveAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>())).Returns(false);
            mockaddressLocationRepository.Setup(n => n.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(objAddressLocation);
            mockdeliveryPointsRepository.Setup(n => n.InsertDeliveryPoint(It.IsAny<DeliveryPointDTO>())).Returns(true);
            mocknotificationRepository.Setup(n => n.AddNewNotification(It.IsAny<NotificationDTO>())).Returns(Task.FromResult(It.IsAny<int>()));

            var result = testCandidate.SavePAFDetails(lstPostalAddress);

            mockrefDataRepository.Verify(n => n.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeast(3));
            mockAddressRepository.Verify(n => n.GetPostalAddress(It.IsAny<int>()), Times.AtMost(5));
            mockAddressRepository.Verify(n => n.GetPostalAddress(It.IsAny<PostalAddressDTO>()), Times.AtMost(5));
            mockAddressRepository.Verify(n => n.SaveAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>()), Times.Never);
            mockAddressRepository.Verify(n => n.InsertAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>()), Times.Once());
            mockaddressLocationRepository.Verify(n => n.GetAddressLocationByUDPRN(It.IsAny<int>()), Times.Once());
            mockdeliveryPointsRepository.Verify(n => n.InsertDeliveryPoint(It.IsAny<DeliveryPointDTO>()), Times.Once());
            mocknotificationRepository.Verify(n => n.AddNewNotification(It.IsAny<NotificationDTO>()), Times.Never());
        }

        protected override void OnSetup()
        {
            PostalAddressDTO postalAddress = new PostalAddressDTO()
            {
                Address_Id = 10,
                AddressType_Id = 2,
                UDPRN = 14856,
                BuildingName = "Building one",
                BuildingNumber = 123,
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15")
            };

            mockAddressRepository = CreateMock<IAddressRepository>();
            mockrefDataRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockdeliveryPointsRepository = CreateMock<IDeliveryPointsRepository>();
            mockaddressLocationRepository = CreateMock<IAddressLocationRepository>();
            mocknotificationRepository = CreateMock<INotificationRepository>();
            mockfileProcessingLogRepository = CreateMock<IFileProcessingLogRepository>();
            mockloggingHelper = CreateMock<ILoggingHelper>();
            configurationHelperMock = CreateMock<IConfigurationHelper>();
            mockrefDataRepository.Setup(x => x.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>())).Returns(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            mockAddressRepository.Setup(x => x.SaveAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>())).Returns(true);
            mockAddressRepository.Setup(x => x.DeleteNYBPostalAddress(It.IsAny<List<int>>(), It.IsAny<Guid>())).Returns(true);
            mockAddressRepository.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddress);
            testCandidate = new PostalAddressBusinessService(mockAddressRepository.Object, mockrefDataRepository.Object, mockdeliveryPointsRepository.Object, mockaddressLocationRepository.Object, mocknotificationRepository.Object, mockfileProcessingLogRepository.Object, mockloggingHelper.Object, configurationHelperMock.Object);
        }

        // private void SetUpdata(PostalAddressDTO objPostalAddress, DeliveryPointDTO objDeliveryPoint, AddressLocationDTO objAddressLocation)
        // {
        //    mockAddressRepository = CreateMock<IAddressRepository>();
        //    mockrefDataRepository = CreateMock<IReferenceDataCategoryRepository>();
        //    mockdeliveryPointsRepository = CreateMock<IDeliveryPointsRepository>();
        //    mockaddressLocationRepository = CreateMock<IAddressLocationRepository>();
        //    mocknotificationRepository = CreateMock<INotificationRepository>();
        //    mockfileProcessingLogRepository = CreateMock<IFileProcessingLogRepository>();
        //    mockloggingHelper = CreateMock<ILoggingHelper>();
        //    testCandidate = new PostalAddressBusinessService(mockAddressRepository.Object, mockrefDataRepository.Object, mockdeliveryPointsRepository.Object, mockaddressLocationRepository.Object, mocknotificationRepository.Object, mockfileProcessingLogRepository.Object, mockloggingHelper.Object);
        //    /*
        //    mockrefDataRepository.Setup(n => n.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>())).Returns(new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974"));
        //    mockAddressRepository.Setup(n => n.GetPostalAddress(It.IsAny<int>())).Returns(objPostalAddress);
        //    mockAddressRepository.Setup(n => n.GetPostalAddress(It.IsAny<PostalAddressDTO>())).Returns(It.IsAny<PostalAddressDTO>());
        //    mockAddressRepository.Setup(n => n.UpdateAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>())).Returns(true);
        //    mockdeliveryPointsRepository.Setup(n => n.GetDeliveryPointByUDPRN(It.IsAny<int>())).Returns(objDeliveryPoint);
        //    mockaddressLocationRepository.Setup(n => n.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(objAddressLocation);
        //    mockdeliveryPointsRepository.Setup(n => n.InsertDeliveryPoint(It.IsAny<DeliveryPointDTO>())).Returns(true);
        //    mocknotificationRepository.Setup(n => n.AddNewNotification(It.IsAny<NotificationDTO>())).Returns(Task.FromResult(It.IsAny<int>()));*/
        // }
    }
}