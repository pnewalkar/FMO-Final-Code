﻿using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryPoint.WebAPI.DataDTO;
using RM.Data.DeliveryPoint.WebAPI.DTO;
using RM.Data.DeliveryPoint.WebAPI.DTO.Model;
using RM.DataManagement.DeliveryPoint.WebAPI.BusinessService;
using RM.DataManagement.DeliveryPoint.WebAPI.DataService;
using RM.DataManagement.DeliveryPoint.WebAPI.Integration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;

namespace RM.Data.DeliveryPoint.WebAPI.Test
{
    [TestFixture]
    [Ignore("Ignored due to Data model Changes")]
    public class DeliveryPointBusinessServiceFixture : TestFixtureBase
    {
        private IDeliveryPointBusinessService testCandidate;
        private Mock<IDeliveryPointsDataService> mockDeliveryPointsDataService;
        private Mock<IConfigurationHelper> mockConfigurationDataService;
        private Mock<ILoggingHelper> mockLoggingDataService;
        private Mock<RMTraceManager> mockTraceManager;
        private Mock<IDeliveryPointIntegrationService> mockDeliveryPointIntegrationService;
        private Guid unitGuid = Guid.NewGuid();
        private Guid deliveryPointGuid = Guid.NewGuid();
        private AddDeliveryPointDTO addDeliveryPointDTO;
        private AddDeliveryPointDTO addDeliveryPointDTO1;
        private List<PostalAddressDTO> postalAddressesDTO;
        private DeliveryPointModelDTO deliveryPointModelDTO;
        private DeliveryPointDTO deliveryPointDTO;
        private List<DeliveryPointDataDTO> actualDeliveryPointDataDto = null;
        private DeliveryPointDataDTO actualDeliveryPointDTO = null;
        private RouteDTO routeDTO = null;

        [Test]
        public void Test_GetDeliveryPoints_PositiveScenario()
        {
            string coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";
            var result = testCandidate.GetDeliveryPoints(coordinates, unitGuid);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointByGuid_PositiveScenario()
        {
            var result = testCandidate.GetDeliveryPointByGuid(deliveryPointGuid);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDetailDeliveryPointByUDPRN_PositiveScenario()
        {
            var result = testCandidate.GetDetailDeliveryPointByUDPRN(12345);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Test_FetchDeliveryPointsForBasicSearch_PositiveScenario()
        {
            List<DeliveryPointDTO> result = await testCandidate.GetDeliveryPointsForBasicSearch("abc", Guid.NewGuid());
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
        }

        [Test]
        public async Task Test_GetDeliveryPointsCount_PositiveScenario()
        {
            int result = await testCandidate.GetDeliveryPointsCount("abc", Guid.NewGuid());
            Assert.IsNotNull(result);
            Assert.IsTrue(result == 2);
        }

        [Test]
        public async Task GetDeliveryPointsForAdvanceSearch_PositiveScenario()
        {
            List<DeliveryPointDTO> result = await testCandidate.GetDeliveryPointsForAdvanceSearch("abc", Guid.NewGuid());
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
        }

        [Test]
        public async Task Test_CreateDeliveryPoint_PositiveScenario()
        {
            CreateDeliveryPointModelDTO result = await testCandidate.CreateDeliveryPoint(addDeliveryPointDTO);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Message == "Delivery Point created successfully");
        }

        [Test]
        public async Task Test_UpdateDeliveryPointLocation_PositiveScenario()
        {
            UpdateDeliveryPointModelDTO result = await testCandidate.UpdateDeliveryPointLocation(deliveryPointModelDTO);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetRouteForDeliveryPoint_PositiveScenario()
        {
            var result = testCandidate.GetRouteForDeliveryPoint(Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Test_DeliveryPointExists()
        {
            bool result = await testCandidate.DeliveryPointExists(12345);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_UpdateDeliveryPointLocationOnUDPRN()
        {
            int expectedresult = await testCandidate.UpdateDeliveryPointLocationOnUDPRN(deliveryPointDTO);
            Assert.IsNotNull(expectedresult);
            Assert.IsTrue(expectedresult == 5);
        }

        //[Test]
        //public void Test_GetDeliveryPointByUDPRN()
        //{
        //    Guid id = Guid.NewGuid();
        //    List<DeliveryPointDTO> lstDeliveryPointDTO = new List<DeliveryPointDTO>();
        //    DeliveryPointDTO objdeliverypointDTO = new DeliveryPointDTO();
        //    objdeliverypointDTO.ID = Guid.NewGuid();
        //    objdeliverypointDTO.LocationXY = System.Data.Entity.Spatial.DbGeometry.PointFromText("POINT (487431 193658)", 27700);
        //    objdeliverypointDTO.PostalAddress = new PostalAddressDTO();
        //    lstDeliveryPointDTO.Add(objdeliverypointDTO);

        //    mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointListByUDPRN(It.IsAny<int>())).Returns(It.IsAny<List<DeliveryPointDTO>>);
        //    var coordinates = testCandidate.GetDeliveryPointByUDPRN(udprn);
        //    mockDeliveryPointsDataService.Verify(x => x.GetDeliveryPointListByUDPRN(It.IsAny<int>()), Times.Once);
        //    Assert.IsNotNull(coordinates);
        //}

        [Test]
        public void Test_InsertDeliveryPoint()
        {
            Guid expected = Guid.Parse("12345678-1234-1234-123456789012");

            var result = testCandidate.InsertDeliveryPoint(deliveryPointDTO);
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.Result);
        }

        [Test]
        public void Test_GetDeliveryPointByUDPRNforBatch()
        {
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).ReturnsAsync(new DeliveryPointDataDTO() { });
            var result = testCandidate.GetDeliveryPointByUDPRNforBatch(12345);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_UpdateDeliveryPointLocationOnID()
        {
            var result = testCandidate.UpdateDeliveryPointLocationOnID(deliveryPointDTO);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointByPostalAddress()
        {
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointByPostalAddress(It.IsAny<Guid>())).Returns(new DeliveryPointDataDTO() { });
            var result = testCandidate.GetDeliveryPointByPostalAddress(Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPoint()
        {
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPoint(It.IsAny<Guid>())).Returns(new DeliveryPointDataDTO() { });
            var result = testCandidate.GetDeliveryPoint(Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        //[Test]
        //public void Test_UpdateDeliveryPointAccessLinkCreationStatus()
        //{
        //    mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointAccessLinkCreationStatus(It.IsAny<DeliveryPointDataDTO>())).Returns(true);
        //    var result = testCandidate.UpdateDeliveryPointAccessLinkCreationStatus(deliveryPointDTO);
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result);
        //}

        [Test]
        public void Test_GetDeliveryPointsCrossingOperationalObject()
        {
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointsCrossingOperationalObject(It.IsAny<string>(), It.IsAny<DbGeometry>())).Returns(new List<DeliveryPointDataDTO>() { });
            var coordinates = "POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))";
            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);
            var result = testCandidate.GetDeliveryPointsCrossingOperationalObject(coordinates, unitBoundary);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_UpdatePAFIndicator()
        {
            var result = testCandidate.UpdatePAFIndicator(Guid.NewGuid(), Guid.NewGuid());
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        //[Test]
        //public void Test_GetDetailDeliveryPointByUDPRN()
        //{
        //    var result = testCandidate.GetDetailDeliveryPointByUDPRN(12345);
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void Test_FetchDeliveryPointsForBasicSearch()
        //{
        //    var result = testCandidate.FetchDeliveryPointsForBasicSearch("abc", Guid.NewGuid());
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void Test_GetDeliveryPointsCount()
        //{
        //    mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointsCount(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(5);
        //    var result = testCandidate.GetDeliveryPointsCount("abc", Guid.NewGuid());
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Result == 5);
        //}

        //[Test]
        //public void Test_FetchDeliveryPointsForAdvanceSearch()
        //{
        //    mockDeliveryPointsDataService.Setup(x => x.FetchDeliveryPointsForAdvanceSearch(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new List<DeliveryPointDTO>() { });
        //    var result = testCandidate.FetchDeliveryPointsForAdvanceSearch("abc", Guid.NewGuid());
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void Test_InsertDeliveryPoint()
        //{
        //    mockDeliveryPointsDataService.Setup(x => x.InsertDeliveryPoint(It.IsAny<DeliveryPointDTO>())).ReturnsAsync(true);
        //    var result = testCandidate.InsertDeliveryPoint(deliveryPointDTO);
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Result);
        //}

        //[Test]
        //public void Test_DeliveryPointExists()
        //{
        //    mockDeliveryPointsDataService.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).ReturnsAsync(true);
        //    var result = testCandidate.DeliveryPointExists(12345);
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Result);
        //}

        //[Test]
        //public void Test_GetDeliveryPointByUDPRNforBatch()
        //{
        //    mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).ReturnsAsync(new DeliveryPointDTO() { });
        //    var result = testCandidate.GetDeliveryPointByUDPRNforBatch(12345);
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void Test_UpdateDeliveryPointLocationOnUDPRN()
        //{
        //    mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDTO>())).ReturnsAsync(5);
        //    var result = testCandidate.UpdateDeliveryPointLocationOnUDPRN(deliveryPointDTO);
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Result == 5);
        //}

        //[Test]
        //public void Test_UpdateDeliveryPointLocationOnID()
        //{
        //    mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointLocationOnID(It.IsAny<DeliveryPointDTO>())).ReturnsAsync(Guid.NewGuid());
        //    var result = testCandidate.UpdateDeliveryPointLocationOnID(deliveryPointDTO);
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void Test_GetDeliveryPointByPostalAddress()
        //{
        //    mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointByPostalAddress(It.IsAny<Guid>())).Returns(new DeliveryPointDTO() { });
        //    var result = testCandidate.GetDeliveryPointByPostalAddress(Guid.NewGuid());
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void Test_GetDeliveryPoint()
        //{
        //    mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPoint(It.IsAny<Guid>())).Returns(new DeliveryPointDTO() { });
        //    var result = testCandidate.GetDeliveryPoint(Guid.NewGuid());
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void Test_UpdateDeliveryPointAccessLinkCreationStatus()
        //{
        //    mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointAccessLinkCreationStatus(It.IsAny<DeliveryPointDTO>())).Returns(true);
        //    var result = testCandidate.UpdateDeliveryPointAccessLinkCreationStatus(deliveryPointDTO);
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result);
        //}

        //[Test]
        //public void Test_GetDeliveryPointsCrossingOperationalObject()
        //{
        //    mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointsCrossingOperationalObject(It.IsAny<string>(), It.IsAny<DbGeometry>())).Returns(new List<DeliveryPointDTO>() { });
        //    var coordinates = "POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))";
        //    var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);
        //    var result = testCandidate.GetDeliveryPointsCrossingOperationalObject(coordinates, unitBoundary);
        //    Assert.IsNotNull(result);
        //}

        protected override void OnSetup()
        {
            mockDeliveryPointsDataService = CreateMock<IDeliveryPointsDataService>();
            mockConfigurationDataService = CreateMock<IConfigurationHelper>();
            mockLoggingDataService = CreateMock<ILoggingHelper>();
            mockDeliveryPointIntegrationService = CreateMock<IDeliveryPointIntegrationService>();

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingDataService.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            List<DeliveryPointDTO> lstDeliveryPointDTO = new List<DeliveryPointDTO>();
            List<RM.CommonLibrary.EntityFramework.Entities.DeliveryPoint> lstDeliveryPoint = new List<RM.CommonLibrary.EntityFramework.Entities.DeliveryPoint>();
            List<PostalAddressDataDTO> lstPostalAddressDTO = new List<PostalAddressDataDTO>()
            {
                            new PostalAddressDataDTO
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

            actualDeliveryPointDataDto = new List<DeliveryPointDataDTO>()
            {
                new DeliveryPointDataDTO() {
                ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                MailVolume = 5,PostalAddress= new PostalAddressDataDTO()
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
            },NetworkNode = new NetworkNodeDataDTO { ID = new Guid(),NetworkNodeType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19") ,Location = new LocationDataDTO() {Shape= DbGeometry.PointFromText("POINT(512722.70000000019 104752.6799999997)", 27700) } },DeliveryPointStatus= new List<DeliveryPointStatusDataDTO>() { new DeliveryPointStatusDataDTO { ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19") } } },
                 new DeliveryPointDataDTO() {
                ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A17"),
                MailVolume = 2,PostalAddress= new PostalAddressDataDTO()
                {
                    BuildingName = "bldg2",
                    BuildingNumber = 1,
                    SubBuildingName = "subbldg1",
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
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"),
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19")
            },NetworkNode = new NetworkNodeDataDTO { ID = new Guid(),NetworkNodeType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19") ,Location = new LocationDataDTO() {Shape= DbGeometry.PointFromText("POINT(512722.70000000019 104752.6799999997)", 27700) } },DeliveryPointStatus= new List<DeliveryPointStatusDataDTO>() { new DeliveryPointStatusDataDTO { ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19") } }
            } };

            actualDeliveryPointDTO = new DeliveryPointDataDTO()
            {
                ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A17"),
                MailVolume = 2,
                PostalAddress = new PostalAddressDataDTO()
                {
                    BuildingName = "bldg2",
                    BuildingNumber = 1,
                    SubBuildingName = "subbldg1",
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
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"),
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19")
                },
                NetworkNode = new NetworkNodeDataDTO { ID = new Guid(), NetworkNodeType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"), Location = new LocationDataDTO() { Shape = DbGeometry.PointFromText("POINT(512722.70000000019 104752.6799999997)", 27700) } },
                DeliveryPointStatus = new List<DeliveryPointStatusDataDTO>() { new DeliveryPointStatusDataDTO { ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19") } }
            };

            AddDeliveryPointDTO actualAddDeliveryPointDTO = new AddDeliveryPointDTO()
            {
                PostalAddressDTO = new PostalAddressDTO()
                {
                    BuildingName = "bldg2",
                    BuildingNumber = 1,
                    SubBuildingName = "subbldg1",
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
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"),
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19")
                }
            };
            var locationXy = DbGeometry.PointFromText("POINT(512722.70000000019 104752.6799999997)", 27700);
            DbGeometry location = DbGeometry.PointFromText("POINT (488938 197021)", 27700);

            List<RM.CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> ReferenceData = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Delivery Point" } };
            Double xLocation = 399545.5590911182;
            Double yLocation = 649744.6394892789;
            routeDTO = new RouteDTO
            {
                RouteName = "Route-001",
                RouteNumber = "001"
            };

            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPoints(It.IsAny<string>(), It.IsAny<Guid>())).Returns(actualDeliveryPointDataDto);
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPoint(It.IsAny<Guid>())).Returns(actualDeliveryPointDTO);
            mockDeliveryPointsDataService.Setup(x => x.GetDetailDeliveryPointByUDPRN(It.IsAny<int>())).Returns(actualAddDeliveryPointDTO);
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointsForBasicSearch(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(actualDeliveryPointDataDto);
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointsCount(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(actualDeliveryPointDataDto.Count);
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointsForAdvanceSearch(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(actualDeliveryPointDataDto);

            // GetDeliveryPointsForAdvanceSearch
            mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDataDTO>())).Returns(Task.FromResult(1));
            //   mockDeliveryPointsDataService.Setup(x => x.GetRouteForDeliveryPoint(It.IsAny<Guid>())).Returns("ABC");

            //    mockDeliveryPointsDataService.Setup(x => x.GetDPUse(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns("Org");

            mockDeliveryPointsDataService.Setup(x => x.UpdatePAFIndicator(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            mockDeliveryPointIntegrationService.Setup(x => x.CheckForDuplicateNybRecords(It.IsAny<PostalAddressDTO>())).Returns(Task.FromResult("ABC"));
            mockDeliveryPointIntegrationService.Setup(x => x.CheckForDuplicateAddressWithDeliveryPoints(It.IsAny<PostalAddressDTO>())).Returns(Task.FromResult(true));
            mockDeliveryPointIntegrationService.Setup(x => x.CreateAddressForDeliveryPoint(It.IsAny<AddDeliveryPointDTO>())).ReturnsAsync(new CreateDeliveryPointModelDTO() { ID = Guid.NewGuid(), IsAddressLocationAvailable = true, Message = "Delivery Point Created", XCoordinate = xLocation, YCoordinate = yLocation });
            mockDeliveryPointIntegrationService.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).ReturnsAsync(ReferenceData);
            mockDeliveryPointIntegrationService.Setup(x => x.GetApproxLocation(It.IsAny<string>())).ReturnsAsync(location);
            mockDeliveryPointsDataService.Setup(x => x.InsertDeliveryPoint(It.IsAny<DeliveryPointDataDTO>())).ReturnsAsync(Guid.NewGuid());
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointRowVersion(It.IsAny<Guid>())).Returns(BitConverter.GetBytes(1));
            mockDeliveryPointIntegrationService.Setup(x => x.MapRouteForDeliveryPoint(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            mockDeliveryPointIntegrationService.Setup(x => x.GetReferenceDataGuId(It.IsAny<string>(), It.IsAny<string>())).Returns(Guid.NewGuid());
            mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointLocationOnID(It.IsAny<DeliveryPointDataDTO>())).ReturnsAsync(Guid.NewGuid());

            mockDeliveryPointIntegrationService.Setup(x => x.GetRouteForDeliveryPoint(It.IsAny<Guid>())).ReturnsAsync(routeDTO);

            mockDeliveryPointsDataService.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).ReturnsAsync(true);

            mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDataDTO>())).ReturnsAsync(5);
            //   mockDeliveryPointIntegrationService.Setup(x => x.CreateBlockSequenceForDeliveryPoint(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));

            mockDeliveryPointIntegrationService.Setup(x => x.CreateAccessLink(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(true);
            mockDeliveryPointIntegrationService.Setup(x => x.CheckForDuplicateNybRecords(It.IsAny<PostalAddressDTO>())).ReturnsAsync("123");
            // mockDeliveryPointIntegrationService.Setup(x => x.GetPostalAddress(It.IsAny<List<Guid>>())).ReturnsAsync(new List<PostalAddressDTO>() { new PostalAddressDTO() {ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11") } });

            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsDataService.Object, mockLoggingDataService.Object, mockConfigurationDataService.Object, mockDeliveryPointIntegrationService.Object);
        }
    }
}