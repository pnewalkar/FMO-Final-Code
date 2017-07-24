using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using System.Linq;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryPoint.WebAPI.DataDTO;
using RM.Data.DeliveryPoint.WebAPI.Entities;
using RM.DataManagement.DeliveryPoint.WebAPI.DataService;
using System.Threading.Tasks;

namespace RM.DataServices.Tests.DataService
{
    [TestFixture]
    public class DeliveryPointDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<DeliveryPointDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<DeliveryPointDBContext>> mockDatabaseFactory;
        private IDeliveryPointsDataService testCandidate;
        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid unitGuid = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13");
        private string search = null;
        private int noOfRecords = 0;
        private Guid unit2Guid;
        private Guid unit3Guid;
        private Guid user1Id;
        private Guid user2Id;
        private Mock<ILoggingHelper> mockLoggingHelper;

        private DeliveryPointDataDTO deliveryPointDataDTO = null;

        //[Test]
        //public void Test_UpdateDeliveryPointByAddressId()
        //{
        //    var result = testCandidate.UpdateDeliveryPointByAddressId(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"), 1);
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result);
        //}

        [Test]
        public void Test_GetDeliveryPointByUDPRN()
        {
            var result = testCandidate.GetDeliveryPointByUDPRN(12345);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_InsertDeliveryPoint()
        {
            deliveryPointDataDTO = new DeliveryPointDataDTO()
            {
                ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                MailVolume = 5,
                DeliveryPointStatus = new List<DeliveryPointStatusDataDTO>()
                {
                    new DeliveryPointStatusDataDTO()
                    {
                        LocationID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13")

                    }
                },
                NetworkNode = new NetworkNodeDataDTO()
                {
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13")
                }

            };
            var result = testCandidate.InsertDeliveryPoint(deliveryPointDataDTO);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Test_UpdateDeliveryPointLocationOnUDPRN()
        {
            deliveryPointDataDTO = new DeliveryPointDataDTO()
            {
                ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                MailVolume = 5,
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
                DeliveryPointStatus = new List<DeliveryPointStatusDataDTO>()
                {
                    new DeliveryPointStatusDataDTO()
                    {
                        LocationID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13")

                    }
                },
                NetworkNode = new NetworkNodeDataDTO()
                {
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                    Location = new LocationDataDTO()
                    {
                        Shape = DbGeometry.PointFromText("POINT (488938 197021)", 27700)
                    }

                },


            };
            var result = await testCandidate.UpdateDeliveryPointLocationOnUDPRN(deliveryPointDataDTO);
            Assert.IsTrue(result == 1);
        }

        [Test]
        public async Task Test_GetDeliveryPointsForAdvanceSearch()
        {
            search = "road";
            List<DeliveryPointDataDTO> expectedResult = await testCandidate.GetDeliveryPointsForAdvanceSearch(search,unitGuid);
            Assert.IsNotNull(expectedResult);
            Assert.IsTrue(expectedResult.Count==1);
        }

        [Test]
        public async Task Test_GetDeliveryPointsForBasicSearch()
        {
            search = "road";
            noOfRecords = 5;
            List<DeliveryPointDataDTO> expectedResult = await testCandidate.GetDeliveryPointsForBasicSearch(search, noOfRecords,unitGuid);
            Assert.IsNotNull(expectedResult);
            Assert.IsTrue(expectedResult.Count == 1);
        }

        [Test]
        public async Task Test_GetDeliveryPointsCount()
        {
            search = "road";
            noOfRecords = 5;
            int expectedResult = await testCandidate.GetDeliveryPointsCount(search, unitGuid);
            Assert.IsNotNull(expectedResult);
            Assert.IsTrue(expectedResult == 1);
        }

        

        //GetDeliveryPointsForBasicSearch
        [Test]
        public void Test_UpdatePAFIndicator()
        {
            var result = testCandidate.UpdatePAFIndicator(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"), new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A14"));
            Assert.IsNotNull(result);
        }

        //[Test]
        //public void Test_GetDeliveryPoints()
        //{
        //    //TODO
        //   // string coordinates = "POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))";
        //   // var result = testCandidate.GetDeliveryPoints(coordinates, new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A14"));
        //  //  Assert.IsNotNull(result);
        //}

        [Test]
        public void Test_DeliveryPointExists()
        {
            var result = testCandidate.DeliveryPointExists(1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Test_GetDeliveryPointDistance()
        {
            DbGeometry point = DbGeometry.PointFromText("POINT (488938 197021)", 27700);
            var result = testCandidate.GetDeliveryPointDistance(deliveryPointDataDTO, point);
            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {
            //var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);

            //deliveryPointDataDTO = new DeliveryPointDataDTO()
            //{
            //    // OperationalStatus_GUID = Guid.NewGuid(),

            //    NetworkNode = new NetworkNodeDataDTO
            //    {
            //        Location = new LocationDataDTO
            //        {
            //            Shape = unitBoundary
            //        }
            //    }
            //};

            var deliveryPoint = new List<DeliveryPoint>()
            {
               new DeliveryPoint()
               {
                   PostalAddressID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                   DeliveryPointUseIndicatorGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A14"),
                   PostalAddress= new PostalAddress()
                   {
                    BuildingName = "road bldg2",
                    BuildingNumber = 1,
                    SubBuildingName = "road subbldg1",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "road ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    Postcode = "road PostcodeNew",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 12345,
                    PostcodeType = "xyz",
                    SmallUserOrganisationIndicator = "indicator",
                    DeliveryPointSuffix = "DeliveryPointSuffix",  
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"),
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19")
                   },
                   DeliveryPointStatus = new List<DeliveryPointStatus>()
                {
                    new DeliveryPointStatus()
                    {
                        LocationID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13")

                    }
                },
                NetworkNode = new NetworkNode()
                {
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                    Location = new Location()
                    {
                        Shape = DbGeometry.PointFromText("POINT (488938 197021)", 27700)
                    }

                },
               }
            };

            var location = new List<Location>()
            {
                new Location()
                {
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                      Shape = DbGeometry.PointFromText("POINT (488938 197021)", 27700)
                }

            };

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockRMDBContext = CreateMock<DeliveryPointDBContext>();

            //setup for Deliverypoint
            var mockAsynEnumerable = new DbAsyncEnumerable<DeliveryPoint>(deliveryPoint);
            var mockDeliveryPointDataService = MockDbSet(deliveryPoint);
            mockDeliveryPointDataService.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockDeliveryPointDataService.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockDeliveryPointDataService.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockDeliveryPointDataService.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryPoint>)mockAsynEnumerable).GetAsyncEnumerator());

            mockRMDBContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointDataService.Object);
            mockRMDBContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointDataService.Object);
            mockDeliveryPointDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointDataService.Object);
            mockDeliveryPointDataService.Setup(x => x.AsNoTracking()).Returns(mockDeliveryPointDataService.Object);

            //setup for location
            var mockAsynEnumerable1 = new DbAsyncEnumerable<Location>(location);
            var mockLocation = MockDbSet(location);
            mockLocation.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable1.AsQueryable().Provider);
            mockLocation.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable1.AsQueryable().Expression);
            mockLocation.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable1.AsQueryable().ElementType);
            mockLocation.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Location>)mockAsynEnumerable1).GetAsyncEnumerator());

            mockRMDBContext.Setup(x => x.Set<Location>()).Returns(mockLocation.Object);
            mockRMDBContext.Setup(x => x.Locations).Returns(mockLocation.Object);
            mockLocation.Setup(x => x.Include(It.IsAny<string>())).Returns(mockLocation.Object);
            mockLocation.Setup(x => x.AsNoTracking()).Returns(mockLocation.Object);

            //var mockAsynEnumerable2 = new DbAsyncEnumerable<RM.Data.DeliveryPoint.WebAPI.Entities.Location>(location);
            //var mockLocation = MockDbSet(location);
            //mockLocation.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            //mockLocation.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            //mockLocation.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            //mockLocation.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.Data.DeliveryPoint.WebAPI.Entities.Location>)mockAsynEnumerable2).GetAsyncEnumerator());
            //mockRMDBContext.Setup(x => x.Set<RM.Data.DeliveryPoint.WebAPI.Entities.Location>()).Returns(mockLocation.Object);
            //mockRMDBContext.Setup(c => c.Locations.AsNoTracking()).Returns(mockLocation.Object);
            //mockRMDBContext.Setup(x => x.Locations).Returns(mockLocation.Object);
            //mockLocation.Setup(x => x.Include(It.IsAny<string>())).Returns(mockLocation.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            //mockConfigurationHelper = new Mock<IConfigurationHelper>();

            mockDatabaseFactory = CreateMock<IDatabaseFactory<DeliveryPointDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            mockRMDBContext.Setup(n => n.SaveChangesAsync()).ReturnsAsync(1);
            testCandidate = new DeliveryPointsDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}