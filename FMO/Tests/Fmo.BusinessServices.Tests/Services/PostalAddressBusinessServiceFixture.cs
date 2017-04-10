using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.Interface;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.MessageBrokerCore.Messaging;
using Fmo.NYBLoader;
using Fmo.NYBLoader.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.BusinessServices.Tests.Services
{
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
        public void Test_SavePAFDetails_ValidData()
        {
            PostalAddressDTO objPostalAddressDTO = new PostalAddressDTO()
                    {
                        Time = "7/19/2016",
                        Date = "8:37:00",
                        AmendmentType = "I",
                        AmendmentDesc = "new insert",
                        Postcode = "YO23 1DQ",
                        PostTown = "York",
                        //DependentLocality = "",
                        //DoubleDependentLocality = "",
                        //Thoroughfare = "",
                        DependentThoroughfare = "Bishopthorpe Road",
                        //BuildingNumber = "",
                        BuildingName = "The Residence",
                        //SubBuildingName,
                        //POBoxNumber,
                        //DepartmentName,
                        //OrganisationName,
                        UDPRN = 54162429,
                        //PostcodeType,//S
                        SmallUserOrganisationIndicator = "",
                        DeliveryPointSuffix = "1A",
                        //Address_Id,
                        AddressType_Id = 1, // "PAF",
                        //AMUApproved,
                        AddressStatus_Id = 4 // "L"
            };
            var result = testCandidate.SavePAFDetails(objPostalAddressDTO);
            mockrefDataRepository.Verify(x => x.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeast(2));
            mockAddressRepository.Verify(x => x.GetPostalAddress(It.IsAny<int?>()), Times.Once());
            mockAddressRepository.Verify(x => x.GetPostalAddress(It.IsAny<PostalAddressDTO>()), Times.Once());
            mockAddressRepository.Verify(x => x.UpdateAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>()), Times.Once());
            mockAddressRepository.Verify(x => x.SaveAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_SaveDeliveryPoint_ValidData()
        {
            PostalAddressDTO objPostalAddressDTO = new PostalAddressDTO() { UDPRN = 14856, Postcode = "AB10 1AB" };
            testCandidate.SaveDeliveryPointProcess(objPostalAddressDTO);
            mockrefDataRepository.Verify(x => x.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeast(2));
            mockdeliveryPointsRepository.Verify(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>()), Times.Once());
            mockaddressLocationRepository.Verify(x => x.GetAddressLocationByUDPRN(It.IsAny<int>()), Times.Once());
            mockdeliveryPointsRepository.Verify(x => x.InsertDeliveryPoint(It.IsAny<DeliveryPointDTO>()), Times.Once());
        }

        protected override void OnSetup()
        {
            mockAddressRepository = CreateMock<IAddressRepository>();
            mockrefDataRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockdeliveryPointsRepository = CreateMock<IDeliveryPointsRepository>();
            mockaddressLocationRepository = CreateMock<IAddressLocationRepository>();
            mocknotificationRepository = CreateMock<INotificationRepository>();
            mockfileProcessingLogRepository = CreateMock<IFileProcessingLogRepository>();
            mockloggingHelper = CreateMock<ILoggingHelper>();

            mockrefDataRepository.Setup(x => x.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>())).Returns(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            mockAddressRepository.Setup(x => x.SaveAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>())).Returns(true);
            mockAddressRepository.Setup(x => x.DeleteNYBPostalAddress(It.IsAny<List<int>>(), It.IsAny<Guid>())).Returns(true);

            mockAddressRepository.Setup(x => x.GetPostalAddress(It.IsAny<int?>())).Returns(It.IsAny<PostalAddressDTO>());
            mockAddressRepository.Setup(x => x.GetPostalAddress(It.IsAny<PostalAddressDTO>())).Returns(It.IsAny<PostalAddressDTO>());
            mockAddressRepository.Setup(x => x.UpdateAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>())).Returns(true);
            mockAddressRepository.Setup(x => x.SaveAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>())).Returns(true);
            mockdeliveryPointsRepository.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).Returns(It.IsAny<DeliveryPointDTO>());
            mockaddressLocationRepository.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(It.IsAny<AddressLocationDTO>());
            mockdeliveryPointsRepository.Setup(x => x.InsertDeliveryPoint(It.IsAny<DeliveryPointDTO>())).Returns(true);

            testCandidate = new PostalAddressBusinessService(
                                        mockAddressRepository.Object,
                                        mockrefDataRepository.Object,
                                        mockdeliveryPointsRepository.Object,
                                        mockaddressLocationRepository.Object,
                                        mocknotificationRepository.Object,
                                        mockfileProcessingLogRepository.Object,
                                        mockloggingHelper.Object);
        }

    }
}
