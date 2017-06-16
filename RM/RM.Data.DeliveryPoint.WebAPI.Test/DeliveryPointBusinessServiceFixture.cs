using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.DeliveryPoint.WebAPI.BusinessService;
using RM.DataManagement.DeliveryPoint.WebAPI.Integration;

namespace RM.Data.DeliveryPoint.WebAPI.Test
{
    [TestFixture]
    public class DeliveryPointBusinessServiceFixture : TestFixtureBase
    {
        private IDeliveryPointBusinessService testCandidate;
        private Mock<IDeliveryPointsDataService> mockDeliveryPointsRepository;

        private Mock<IConfigurationHelper> mockConfigurationRepository;
        private Mock<ILoggingHelper> mockLoggingRepository;
        private Mock<RMTraceManager> mockTraceManager;
        private Guid unitGuid = Guid.NewGuid();
        private AddDeliveryPointDTO addDeliveryPointDTO;
        private AddDeliveryPointDTO addDeliveryPointDTO1;
        private List<PostalAddressDTO> postalAddressesDTO;
        private DeliveryPointModelDTO deliveryPointModelDTO;

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
        }

        [Test]
        public void Test_CreateDeliveryPoint()
        {
            var result = testCandidate.CreateDeliveryPoint(addDeliveryPointDTO);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.Message == "Delivery Point created successfully");
        }

        [Test]
        public void Test_CreateDeliveryPoint_Duplicate()
        {
            var result = testCandidate.CreateDeliveryPoint(addDeliveryPointDTO1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.Message == "There is a duplicate of this Delivery Point in the system");
        }

        [Test]
        public void Test_CreateDeliveryPoint_Duplicate_WithPostCode()
        {
            var result = testCandidate.CreateDeliveryPoint(addDeliveryPointDTO1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.Message == "This address is in the NYB file under the postcode 123");
        }

        [Test]
        public void Test_UpdateDeliveryPointLocation()
        {
            var result = testCandidate.UpdateDeliveryPointLocation(deliveryPointModelDTO);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetRouteForDeliveryPoint()
        {
            var result = testCandidate.GetRouteForDeliveryPoint(Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {
            mockDeliveryPointsRepository = CreateMock<IDeliveryPointsDataService>();

            mockConfigurationRepository = CreateMock<IConfigurationHelper>();
            mockLoggingRepository = CreateMock<ILoggingHelper>();


            List<DeliveryPointDTO> lstDeliveryPointDTO = new List<DeliveryPointDTO>();
            List<RM.CommonLibrary.EntityFramework.Entities.DeliveryPoint> lstDeliveryPoint = new List<RM.CommonLibrary.EntityFramework.Entities.DeliveryPoint>();
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
                    ID = 
                    "019DBBBB-03FB-489C-8C8D-F1085E0D2A12")
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

            deliveryPointModelDTO = new DeliveryPointModelDTO()
            {
                ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                XCoordinate = 12,
                YCoordinate = 10,
                UDPRN = 123,
                Latitude = 1,
                Longitude = 2,
                RowVersion = BitConverter.GetBytes(1)
            };

            mockDeliveryPointsRepository.Setup(x => x.GetDeliveryPoints(It.IsAny<string>(), It.IsAny<Guid>())).Returns(It.IsAny<List<DeliveryPointDTO>>);
            //      mockDeliveryPointsRepository.Setup(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDTO>())).Returns(Task.FromResult(deliveryPointModelDTO.ID));
            mockDeliveryPointsRepository.Setup(x => x.GetRouteForDeliveryPoint(It.IsAny<Guid>())).Returns("ABC");
            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsRepository.Object, mockLoggingRepository.Object, mockConfigurationRepository.Object, CreateMock<IDeliveryPointIntegrationService>().Object);
        }
    }
}
