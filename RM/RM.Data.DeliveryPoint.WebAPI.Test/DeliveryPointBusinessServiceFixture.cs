using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using Moq;
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
        private Guid deliveryPointGuid = Guid.NewGuid();
        private Guid addressGuid = Guid.NewGuid();
        private Guid pafIndicator = Guid.NewGuid();
        private Guid id = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11");
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
        public async Task Test_DeliveryPointExists_PositiveScenario()
        {
            bool result = await testCandidate.DeliveryPointExists(12345);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_UpdateDeliveryPointLocationOnUDPRN_PositiveScenario()
        {
            int expectedresult = await testCandidate.UpdateDeliveryPointLocationOnUDPRN(deliveryPointDTO);
            Assert.IsNotNull(expectedresult);
            Assert.IsTrue(expectedresult == 5);
        }

        [Test]
        public void Test_GetDeliveryPointByUDPRNforBatch_PositiveScenario()
        {
            var result = testCandidate.GetDeliveryPointByUDPRNforBatch(12345);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointByPostalAddress()
        {
            var result = testCandidate.GetDeliveryPointByPostalAddress(Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_InsertDeliveryPoint()
        {
            Guid expected = new Guid("{44037200-212c-41bb-a0d2-eed2900b357d}");

            var result = testCandidate.InsertDeliveryPoint(deliveryPointDTO);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointsCrossingOperationalObject()
        {
            var coordinates = "POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))";
            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);
            var result = testCandidate.GetDeliveryPointsCrossingOperationalObject(coordinates, unitBoundary);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPoint()
        {
            var result = testCandidate.GetDeliveryPoint(Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_UpdateDeliveryPointLocationOnID()
        {
            var result = testCandidate.UpdateDeliveryPointLocationOnID(deliveryPointDTO);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Test_GetDeliveryPointByPostalAddressWithLocation_PositiveScenario()
        {
            DeliveryPointDTO expectedresult = await testCandidate.GetDeliveryPointByPostalAddressWithLocation(addressGuid);
            Assert.IsNotNull(expectedresult);
        }

        [Test]
        public async Task Test_UpdatePAFIndicator_PositiveScenario()
        {
            bool expectedresult = await testCandidate.UpdatePAFIndicator(addressGuid, pafIndicator);
            Assert.IsNotNull(expectedresult);
            Assert.IsTrue(expectedresult);
        }

        [Test]
        public async Task Test_DeleteDeliveryPoint_PositiveScenario()
        {
            bool expectedresult = await testCandidate.DeleteDeliveryPoint(id);
            Assert.IsNotNull(expectedresult);
            Assert.IsTrue(expectedresult);
        }
        
        [Test]
        public async Task Test_UpdateDPUse()
        {            
            bool result = await testCandidate.UpdateDPUse(postalAddressesDTO[0]);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_UpdateDPUse_NegativeScenario1()
        {
            try
            {
                bool result = await testCandidate.UpdateDPUse(null);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Value cannot be null.Parameter name: postalAddressDetails", ae.Message.Replace("\r\n", string.Empty));
            }
        }

        /// <summary>
        /// Setup data
        /// </summary>
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
                MailVolume = 5, PostalAddress = new PostalAddressDataDTO()
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
            }, NetworkNode = new NetworkNodeDataDTO { ID = new Guid(), NetworkNodeType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"), Location = new LocationDataDTO() { Shape = DbGeometry.PointFromText("POINT(512722.70000000019 104752.6799999997)", 27700) } }, DeliveryPointStatus = new List<DeliveryPointStatusDataDTO>() { new DeliveryPointStatusDataDTO { ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19") } } },
                 new DeliveryPointDataDTO() {
                ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A17"),
                MailVolume = 2, PostalAddress = new PostalAddressDataDTO()
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
            }, NetworkNode = new NetworkNodeDataDTO { ID = new Guid(), NetworkNodeType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"), Location = new LocationDataDTO() { Shape = DbGeometry.PointFromText("POINT(512722.70000000019 104752.6799999997)", 27700) } }, DeliveryPointStatus = new List<DeliveryPointStatusDataDTO>() { new DeliveryPointStatusDataDTO { ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19") } }
            }
            };

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



            List<RM.CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> referenceData = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>()
            {
                new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO
                {
                    CategoryName = "Delivery Point"
                },
                new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO
                {                   
                    CategoryName = ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                    ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>()
                    {
                        new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO()
                        {
                            ID = new Guid("990B86A2-9431-E711-83EC-28D244AEF9ED"),
                            ReferenceDataValue = ReferenceDataValues.Organisation
                        }
                    }
                },
                new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO
                {                  
                    CategoryName = ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                    ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>()
                    {
                        new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO()
                        {
                            ID = new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED"),
                            ReferenceDataValue = ReferenceDataValues.Residential
                        }
                    }
                }
            };
            double xLocation = 399545.5590911182;
            double yLocation = 649744.6394892789;
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

            // mockDeliveryPointsDataService.Setup(x => x.GetRouteForDeliveryPoint(It.IsAny<Guid>())).Returns("ABC");

            //    mockDeliveryPointsDataService.Setup(x => x.GetDPUse(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns("Org");
            mockDeliveryPointsDataService.Setup(x => x.UpdatePAFIndicator(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            mockDeliveryPointIntegrationService.Setup(x => x.CheckForDuplicateNybRecords(It.IsAny<PostalAddressDTO>())).Returns(Task.FromResult("ABC"));
            mockDeliveryPointIntegrationService.Setup(x => x.CheckForDuplicateAddressWithDeliveryPoints(It.IsAny<PostalAddressDTO>())).Returns(Task.FromResult(true));
            mockDeliveryPointIntegrationService.Setup(x => x.CreateAddressForDeliveryPoint(It.IsAny<AddDeliveryPointDTO>())).ReturnsAsync(new CreateDeliveryPointModelDTO() { ID = Guid.NewGuid(), IsAddressLocationAvailable = true, Message = "Delivery Point Created", XCoordinate = xLocation, YCoordinate = yLocation });
            mockDeliveryPointIntegrationService.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).ReturnsAsync(referenceData);
            mockDeliveryPointIntegrationService.Setup(x => x.GetApproxLocation(It.IsAny<string>())).ReturnsAsync(location);
            mockDeliveryPointsDataService.Setup(x => x.InsertDeliveryPoint(It.IsAny<DeliveryPointDataDTO>())).ReturnsAsync(Guid.NewGuid());
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointRowVersion(It.IsAny<Guid>())).Returns(BitConverter.GetBytes(1));
            mockDeliveryPointIntegrationService.Setup(x => x.MapRouteForDeliveryPoint(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            mockDeliveryPointIntegrationService.Setup(x => x.GetReferenceDataGuId(It.IsAny<string>(), It.IsAny<string>())).Returns(Guid.NewGuid());
            mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointLocationOnID(It.IsAny<DeliveryPointDataDTO>())).ReturnsAsync(Guid.NewGuid());

            mockDeliveryPointIntegrationService.Setup(x => x.GetRouteForDeliveryPoint(It.IsAny<Guid>())).ReturnsAsync(routeDTO);

            mockDeliveryPointsDataService.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).ReturnsAsync(true);

            mockDeliveryPointsDataService.Setup(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDataDTO>())).ReturnsAsync(5);

            // mockDeliveryPointIntegrationService.Setup(x => x.CreateBlockSequenceForDeliveryPoint(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockDeliveryPointIntegrationService.Setup(x => x.CreateAccessLink(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(true);
            mockDeliveryPointIntegrationService.Setup(x => x.CheckForDuplicateNybRecords(It.IsAny<PostalAddressDTO>())).ReturnsAsync("123");
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointByUDPRN(It.IsAny<int>())).ReturnsAsync(actualDeliveryPointDTO);
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointByPostalAddress(It.IsAny<Guid>())).Returns(actualDeliveryPointDTO);

            // mockDeliveryPointIntegrationService.Setup(x => x.GetPostalAddress(It.IsAny<List<Guid>>())).ReturnsAsync(new List<PostalAddressDTO>() { new PostalAddressDTO() {ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11") } });
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointsCrossingOperationalObject(It.IsAny<string>(), It.IsAny<DbGeometry>())).Returns(actualDeliveryPointDataDto);
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPoint(It.IsAny<Guid>())).Returns(actualDeliveryPointDTO);
            mockDeliveryPointsDataService.Setup(x => x.GetDeliveryPointByPostalAddressWithLocation(It.IsAny<Guid>())).ReturnsAsync(actualDeliveryPointDTO);
            mockDeliveryPointsDataService.Setup(x => x.UpdateDPUse(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(true);

            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsDataService.Object, mockLoggingDataService.Object, mockConfigurationDataService.Object, mockDeliveryPointIntegrationService.Object);
        }
    }
}