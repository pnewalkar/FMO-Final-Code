using System;
using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.Interface;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.DTO.Model;
using Fmo.Entities;
using Moq;
using NUnit.Framework;

namespace Fmo.BusinessServices.Tests.Services
{
    [TestFixture]
    public class DeliveryPointBusinessServiceFixture : TestFixtureBase
    {
        private IDeliveryPointBusinessService testCandidate;
        private Mock<IDeliveryPointsRepository> mockDeliveryPointsRepository;
        private Mock<IAddressLocationRepository> mockaddressLocationRepository;
        private Mock<IConfigurationHelper> mockConfigurationRepository;
        private Mock<ILoggingHelper> mockLoggingRepository;
        private Mock<IAddressRepository> mockAddressRepository;
        private Mock<IReferenceDataCategoryRepository> referenceDataCategoryRepository;
        private Guid unitGuid = Guid.NewGuid();
        private AddDeliveryPointDTO addDeliveryPointDTO;
        private AddDeliveryPointDTO addDeliveryPointDTO1;
        private List<PostalAddressDTO> postalAddressesDTO;

        [Test]
        public void Test_GetDeliveryPoints()
        {
            string coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";

            var result = testCandidate.GetDeliveryPoints(coordinates, unitGuid);
            mockDeliveryPointsRepository.Verify(x => x.GetDeliveryPoints(It.IsAny<string>(), It.IsAny<Guid>()), Times.Once);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointByUDPRN()
        {
            int udprn = 10875813;
            List<DeliveryPointDTO> lstDeliveryPointDTO = new List<DeliveryPointDTO>();
            DeliveryPointDTO objdeliverypointDTO = new DeliveryPointDTO();
            objdeliverypointDTO.ID = Guid.NewGuid();
            objdeliverypointDTO.LocationXY = System.Data.Entity.Spatial.DbGeometry.PointFromText("POINT (487431 193658)", 27700);
            objdeliverypointDTO.PostalAddress = new PostalAddressDTO();
            lstDeliveryPointDTO.Add(objdeliverypointDTO);

            mockDeliveryPointsRepository.Setup(x => x.GetDeliveryPointListByUDPRN(It.IsAny<int>())).Returns(It.IsAny<List<DeliveryPointDTO>>);
            var coordinates = testCandidate.GetDeliveryPointByUDPRN(udprn);
            mockDeliveryPointsRepository.Verify(x => x.GetDeliveryPointListByUDPRN(It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(coordinates);

            // Assert.AreEqual(lstDeliveryPointDTO, coordinates);
        }

        [Test]
        public void Test_CreateDeliveryPoint()
        {
            var result = testCandidate.CreateDeliveryPoint(addDeliveryPointDTO);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Message == "Delivery Point created successfully");
        }

        [Test]
        public void Test_CreateDeliveryPoint_Duplicate()
        {
            mockAddressRepository.Setup(x => x.CheckForDuplicateAddressWithDeliveryPoints(It.IsAny<PostalAddressDTO>())).Returns(true);
            var result = testCandidate.CreateDeliveryPoint(addDeliveryPointDTO1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Message == "There is a duplicate of this Delivery Point in the system");
        }

        [Test]
        public void Test_CreateDeliveryPoint_Duplicate_WithPostCode()
        {
            mockAddressRepository.Setup(x => x.CheckForDuplicateAddressWithDeliveryPoints(It.IsAny<PostalAddressDTO>())).Returns(false);
            var result = testCandidate.CreateDeliveryPoint(addDeliveryPointDTO1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Message == "This address is in the NYB file under the postcode 123");
        }

        protected override void OnSetup()
        {
            mockDeliveryPointsRepository = CreateMock<IDeliveryPointsRepository>();
            mockaddressLocationRepository = CreateMock<IAddressLocationRepository>();
            mockConfigurationRepository = CreateMock<IConfigurationHelper>();
            mockLoggingRepository = CreateMock<ILoggingHelper>();
            mockAddressRepository = CreateMock<IAddressRepository>();
            referenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();

            List<DeliveryPointDTO> lstDeliveryPointDTO = new List<DeliveryPointDTO>();
            List<DeliveryPoint> lstDeliveryPoint = new List<DeliveryPoint>();
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>()
            {
                            new PostalAddressDTO
                            {
                                BuildingName = "Bldg 1",
                                BuildingNumber = 23,
                                Postcode = "123"
                            }
            };
            DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO()
            {
                ID = Guid.NewGuid()
            };

            postalAddressesDTO = new List<PostalAddressDTO>()
            {
                new PostalAddressDTO()
                {
                    BuildingName = "bldg1",
                    BuildingNumber = 1,
                    SubBuildingName = "subbldg",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    Postcode = "PostcodeNew",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 12345,
                    PostcodeType = "xyz",
                    SmallUserOrganisationIndicator = "indicator",
                    DeliveryPointSuffix = "DeliveryPointSuffix",
                    PostCodeGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A12")
            },
                new PostalAddressDTO()
                {
                    BuildingName = "bldg1",
                    BuildingNumber = 1,
                    SubBuildingName = "subbldg",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    Postcode = "Postcode",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 12345,
                    PostcodeType = "xyz",
                    SmallUserOrganisationIndicator = "indicator",
                    DeliveryPointSuffix = "DeliveryPointSuffix",
                    PostCodeGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                    ID = Guid.Empty
            }
            };

            addDeliveryPointDTO = new AddDeliveryPointDTO()
            {
                PostalAddressDTO = postalAddressesDTO[0],
                DeliveryPointDTO = deliveryPointDTO
            };
            addDeliveryPointDTO1 = new AddDeliveryPointDTO()
            {
                PostalAddressDTO = postalAddressesDTO[1],
                DeliveryPointDTO = deliveryPointDTO
            };
  
            mockDeliveryPointsRepository.Setup(x => x.GetDeliveryPoints(It.IsAny<string>(), It.IsAny<Guid>())).Returns(It.IsAny<List<DeliveryPointDTO>>);
            mockAddressRepository.Setup(x => x.CheckForDuplicateNybRecords(It.IsAny<PostalAddressDTO>())).Returns("123");
            mockAddressRepository.Setup(x => x.CreateAddressAndDeliveryPoint(It.IsAny<AddDeliveryPointDTO>())).Returns(new CreateDeliveryPointModelDTO() { ID = Guid.NewGuid(), IsAddressLocationAvailable = true });
            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsRepository.Object, mockaddressLocationRepository.Object, mockAddressRepository.Object, mockLoggingRepository.Object, mockConfigurationRepository.Object, referenceDataCategoryRepository.Object);
        }
    }
}