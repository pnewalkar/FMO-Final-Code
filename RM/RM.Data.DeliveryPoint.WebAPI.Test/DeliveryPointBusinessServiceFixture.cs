using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
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
        private Mock<IDeliveryPointsDataService> mockDeliveryPointsDataService;
        private Mock<IConfigurationHelper> mockConfigurationDataService;
        private Mock<ILoggingHelper> mockLoggingDataService;
        private Mock<IDeliveryPointIntegrationService> mockDeliveryPointIntegrationService;
        private Guid unitGuid = Guid.NewGuid();
        private AddDeliveryPointDTO addDeliveryPointDTO;
        private AddDeliveryPointDTO addDeliveryPointDTO1;
        private List<PostalAddressDTO> postalAddressesDTO;
        private DeliveryPointModelDTO deliveryPointModelDTO;
        private DeliveryPointDTO deliveryPointDTO;

        [Test]
        public void Test_GetDeliveryPoints()
        {
            string coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";

            var result = testCandidate.GetDeliveryPoints(coordinates, unitGuid);
            mockDeliveryPointsDataService.Verify(x => x.GetDeliveryPoints(It.IsAny<string>(), It.IsAny<Guid>()), Times.Once);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointByUDPRN()
        {
            Guid id = Guid.NewGuid();
            List<DeliveryPointDTO> lstDeliveryPointDTO = new List<DeliveryPointDTO>();
            DeliveryPointDTO objdeliverypointDTO = new DeliveryPointDTO();
            objdeliverypointDTO.ID = Guid.NewGuid();
            objdeliverypointDTO.LocationXY = System.Data.Entity.Spatial.DbGeometry.PointFromText("POINT (487431 193658)", 27700);
            objdeliverypointDTO.PostalAddress = new PostalAddressDTO();
            lstDeliveryPointDTO.Add(objdeliverypointDTO);

            // mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).Returns(It.IsAny<List<DeliveryPointDTO>>);
            // var coordinates = testCandidate.GetDeliveryPointByUDPRN(udprn);
            // mockDeliveryPointsDataService.Verify(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>()), Times.Once);
           // Assert.IsNotNull(coordinates);
        }

        [Test]
        public async Task Test_CreateDeliveryPoint()
        {
            var result = await testCandidate.CreateDeliveryPoint(addDeliveryPointDTO);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Message == "Delivery Point created successfully");
        }

        [Test]
        public void Test_CreateDeliveryPoint_Duplicate()
        {
            mockDeliveryPointIntegrationService.Setup(x => x.CheckForDuplicateAddressWithDeliveryPoints(It.IsAny<PostalAddressDTO>())).ReturnsAsync(true);
            var result = testCandidate.CreateDeliveryPoint(addDeliveryPointDTO1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.Message == "There is a duplicate of this Delivery Point in the system");
        }

        [Test]
        public void Test_CreateDeliveryPoint_Duplicate_WithPostCode()
        {
            OnSetup();
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

        [Test]
        public void Test_GetDetailDeliveryPointByUDPRN()
        {
            var result = testCandidate.GetDetailDeliveryPointByUDPRN(12345);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_FetchDeliveryPointsForBasicSearch()
        {
            var result = testCandidate.FetchDeliveryPointsForBasicSearch("abc", Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointsCount()
        {
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointsCount(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(5);
            var result = testCandidate.GetDeliveryPointsCount("abc", Guid.NewGuid());
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result == 5);
        }

        [Test]
        public void Test_FetchDeliveryPointsForAdvanceSearch()
        {
            mockDeliveryPointsDataService.Setup(x => x.FetchDeliveryPointsForAdvanceSearch(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new List<DeliveryPointDTO>() { });
            var result = testCandidate.FetchDeliveryPointsForAdvanceSearch("abc", Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_InsertDeliveryPoint()
        {
            mockDeliveryPointsDataService.Setup(x => x.InsertDeliveryPoint(It.IsAny<DeliveryPointDTO>())).ReturnsAsync(true);
            var result = testCandidate.InsertDeliveryPoint(deliveryPointDTO);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Test_DeliveryPointExists()
        {
            mockDeliveryPointsDataService.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).ReturnsAsync(true);
            var result = testCandidate.DeliveryPointExists(12345);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Test_GetDeliveryPointByUDPRNforBatch()
        {
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).ReturnsAsync(new DeliveryPointDTO() { });
            var result = testCandidate.GetDeliveryPointByUDPRNforBatch(12345);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_UpdateDeliveryPointLocationOnUDPRN()
        {
            mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDTO>())).ReturnsAsync(5);
            var result = testCandidate.UpdateDeliveryPointLocationOnUDPRN(deliveryPointDTO);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result == 5);
        }

        [Test]
        public void Test_UpdateDeliveryPointLocationOnID()
        {
            mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointLocationOnID(It.IsAny<DeliveryPointDTO>())).ReturnsAsync(Guid.NewGuid());
            var result = testCandidate.UpdateDeliveryPointLocationOnID(deliveryPointDTO);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointByPostalAddress()
        {
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointByPostalAddress(It.IsAny<Guid>())).Returns(new DeliveryPointDTO() { });
            var result = testCandidate.GetDeliveryPointByPostalAddress(Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPoint()
        {
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPoint(It.IsAny<Guid>())).Returns(new DeliveryPointDTO() { });
            var result = testCandidate.GetDeliveryPoint(Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_UpdateDeliveryPointAccessLinkCreationStatus()
        {
            mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointAccessLinkCreationStatus(It.IsAny<DeliveryPointDTO>())).Returns(true);
            var result = testCandidate.UpdateDeliveryPointAccessLinkCreationStatus(deliveryPointDTO);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_GetDeliveryPointsCrossingOperationalObject()
        {
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointsCrossingOperationalObject(It.IsAny<string>(), It.IsAny<DbGeometry>())).Returns(new List<DeliveryPointDTO>() { });
            var coordinates = "POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))";
            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);
            var result = testCandidate.GetDeliveryPointsCrossingOperationalObject(coordinates, unitBoundary);
            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {
            mockDeliveryPointsDataService = CreateMock<IDeliveryPointsDataService>();
            mockConfigurationDataService = CreateMock<IConfigurationHelper>();
            mockLoggingDataService = CreateMock<ILoggingHelper>();
            mockDeliveryPointIntegrationService = CreateMock<IDeliveryPointIntegrationService>();

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingDataService.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

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

            deliveryPointDTO = new DeliveryPointDTO()
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

            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPoints(It.IsAny<string>(), It.IsAny<Guid>())).Returns(It.IsAny<List<DeliveryPointDTO>>);
            mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDTO>())).Returns(Task.FromResult(1));
            mockDeliveryPointsDataService.Setup(x => x.GetRouteForDeliveryPoint(It.IsAny<Guid>())).Returns("ABC");
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointRowVersion(It.IsAny<Guid>())).Returns(BitConverter.GetBytes(1));
            mockDeliveryPointsDataService.Setup(x => x.GetDPUse(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns("Org");
            mockDeliveryPointsDataService.Setup(x => x.GetDetailDeliveryPointByUDPRN(It.IsAny<int>())).Returns(new AddDeliveryPointDTO() { });
            mockDeliveryPointsDataService.Setup(x => x.FetchDeliveryPointsForBasicSearch(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new List<DeliveryPointDTO>() { });

            mockDeliveryPointIntegrationService.Setup(x => x.CheckForDuplicateNybRecords(It.IsAny<PostalAddressDTO>())).Returns(Task.FromResult("ABC"));
            mockDeliveryPointIntegrationService.Setup(x => x.CreateBlockSequenceForDeliveryPoint(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockDeliveryPointIntegrationService.Setup(x => x.GetReferenceDataGuId(It.IsAny<string>(), It.IsAny<string>())).Returns(Guid.NewGuid());
            mockDeliveryPointIntegrationService.Setup(x => x.CreateAddressAndDeliveryPoint(It.IsAny<AddDeliveryPointDTO>())).ReturnsAsync(new CreateDeliveryPointModelDTO() { ID = Guid.NewGuid(), IsAddressLocationAvailable = true });
            mockDeliveryPointIntegrationService.Setup(x => x.CreateAccessLink(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(true);
            mockDeliveryPointIntegrationService.Setup(x => x.CheckForDuplicateNybRecords(It.IsAny<PostalAddressDTO>())).ReturnsAsync("123");

            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsDataService.Object, mockLoggingDataService.Object, mockConfigurationDataService.Object, mockDeliveryPointIntegrationService.Object);
        }
    }
}