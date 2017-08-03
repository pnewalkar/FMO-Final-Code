using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryPoint.WebAPI.DataDTO;
using RM.Data.DeliveryPoint.WebAPI.Entities;
using RM.DataManagement.DeliveryPoint.WebAPI.DataService;

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
        private Mock<ILoggingHelper> mockLoggingHelper;

        private DeliveryPointDataDTO deliveryPointDataDTO = null;

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
            List<DeliveryPointDataDTO> expectedResult = await testCandidate.GetDeliveryPointsForAdvanceSearch(search, unitGuid);
            Assert.IsNotNull(expectedResult);
            Assert.IsTrue(expectedResult.Count == 1);
        }

        [Test]
        public async Task Test_GetDeliveryPointsForBasicSearch()
        {
            search = "road";
            noOfRecords = 5;
            List<DeliveryPointDataDTO> expectedResult = await testCandidate.GetDeliveryPointsForBasicSearch(search, noOfRecords, unitGuid);
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

        [Test]
        public void Test_UpdatePAFIndicator()
        {
            var result = testCandidate.UpdatePAFIndicator(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"), new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A14"));
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointPositiveScenario()
        {
            var result = testCandidate.GetDeliveryPoint(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"));
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.NetworkNode);
            Assert.IsNotNull(result.PostalAddress);
            Assert.IsNotNull(result.NetworkNode.Location);
        }

        [Test]
        public void Test_GetDeliveryPointNegativeScenario()
        {
            var result = testCandidate.GetDeliveryPoint(new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA"));
            Assert.IsNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointsPositiveScenario()
        {
            var result = testCandidate.GetDeliveryPoints("POINT (488938 197021)", new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
        }

        [Test]
        public void Test_GetDeliveryPointsNegativeScenario()
        {
            var result = testCandidate.GetDeliveryPoints("POINT (488938 197021)", new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA"));
            Assert.IsNull(result);
        }

        [Test]
        public async Task Test_DeliveryPointExistsPositiveScenario()
        {
            var result = await testCandidate.DeliveryPointExists(12345);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_DeliveryPointExistsNegativeScenario()
        {
            var result = await testCandidate.DeliveryPointExists(54321);
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_GetDeliveryPointByPostalAddressPositiveScenario()
        {
            var result = testCandidate.GetDeliveryPointByPostalAddress(new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA"));
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointByPostalAddressNegativeScenario()
        {
            var result = testCandidate.GetDeliveryPointByPostalAddress(new Guid("D90B6833-7247-4E98-AAA5-94B31C5D264E"));
            Assert.IsNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointListByUDPRNPositiveScenario()
        {
            var result = testCandidate.GetDeliveryPointListByUDPRN(12345);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
        }

        [Test]
        public void Test_GetDeliveryPointListByUDPRNNegativeScenario()
        {
            var result = testCandidate.GetDeliveryPointListByUDPRN(54321);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public void Test_GetDeliveryPointDistancePositiveScenario()
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
                NetworkNode = new NetworkNodeDataDTO()
                {
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                    Location = new LocationDataDTO()
                    {
                        Shape = DbGeometry.PointFromText("POINT (488938 197021)", 27700)
                    }
                },
            };
            DbGeometry point = DbGeometry.PointFromText("POINT (488938 197021)", 27700);
            var result = testCandidate.GetDeliveryPointDistance(deliveryPointDataDTO, point);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result == 0);
        }

        [Test]
        public void Test_GetDeliveryPointDistanceNegativeScenario()
        {
            try
            {
                DbGeometry point = DbGeometry.PointFromText("POINT (488938 197021)", 27700);
                var result = testCandidate.GetDeliveryPointDistance(deliveryPointDataDTO, point);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Following arguments for the method are null Parameter name: deliveryPointDTO", ae.Message.Replace("\r\n", string.Empty));
            }
        }

        [Test]
        public void Test_GetDeliveryPointRowVersionPositiveScenario()
        {
            var actualResult = testCandidate.GetDeliveryPointRowVersion(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"));
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Test_GetDeliveryPointRowVersionNegativeScenario()
        {
            var actualResult = testCandidate.GetDeliveryPointRowVersion(new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA"));
            Assert.IsNull(actualResult);
        }

        [Test]
        public void Test_GetDeliveryPointsCrossingOperationalObject()
        {
            DbGeometry operationalObjectPoint = DbGeometry.PointFromText("POINT (488938 197021)", 27700);
            var actualResult = testCandidate.GetDeliveryPointsCrossingOperationalObject("POINT (488938 197021)", operationalObjectPoint);
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.Count == 0);
        }

        [Test]
        public async Task Test_DeleteDeliveryPointPositiveScenario()
        {
            Guid id = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13");
            var actualResult = await testCandidate.DeleteDeliveryPoint(id);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult);
        }

        [Test]
        public async Task Test_DeleteDeliveryPointNegativeScenario()
        {
            Guid id = new Guid("915DE34F-59E7-49AD-985E-541A76A634FF");
            var actualResult = await testCandidate.DeleteDeliveryPoint(id);
            Assert.IsNotNull(actualResult);
            Assert.IsFalse(actualResult);
        }

        [Test]
        public async Task Test_GetDeliveryPointByPostalAddressWithLocation()
        {
            DeliveryPointDataDTO expectedResult = await testCandidate.GetDeliveryPointByPostalAddressWithLocation(new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA"));
            Assert.IsNotNull(expectedResult);
        }

        /// <summary>
        /// Delievery point exists for given UDPRN
        /// </summary>
        [Test]
        public async Task Test_UpdateDPUse()
        {
            bool result = await testCandidate.UpdateDPUse(12345, new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED"));
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Delievery point does not exist for given UDPRN
        /// </summary>
        [Test]
        public async Task Test_UpdateDPUse_NegativeScenario()
        {
            bool result = await testCandidate.UpdateDPUse(1234, new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED"));
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        protected override void OnSetup()
        {
            var deliveryPoint = new List<DeliveryPoint>()
            {
               new DeliveryPoint()
               {
                   ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                   PostalAddressID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA"),
                   DeliveryPointUseIndicatorGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A14"),
                   RowVersion = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 },
                   PostalAddress = new PostalAddress()
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
                    ID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA"),
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
                         ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
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
                      Shape = DbGeometry.PointFromText("POINT (488938 197021)", 27700),
                      RowCreateDateTime = DateTime.Now,
                      NetworkNode = new NetworkNode()
                {
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                    DeliveryPoint = new DeliveryPoint()
               {
                   ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                   PostalAddressID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA"),
                   DeliveryPointUseIndicatorGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A14")
                }
                }
                }
            };

            var networkNode = new List<NetworkNode>()
            {
                new NetworkNode()
                {
                ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                Location = new Location()
                {
                    Shape = DbGeometry.PointFromText("POINT (488938 197021)", 27700)
                }
                }
            };

            var postalAddress = new List<PostalAddress>
            {
                new PostalAddress()
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
                    ID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA")
                },
            };

            var deliveryPointStatus = new List<DeliveryPointStatus>()
                {
                    new DeliveryPointStatus()
                    {
                        LocationID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13")
                    }
                };

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockRMDBContext = CreateMock<DeliveryPointDBContext>();

            // setup for Deliverypoint
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

            // setup for location
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

            // setup for networknode
            var mockAsynNetworkNode = new DbAsyncEnumerable<NetworkNode>(networkNode);
            var mocknetworkNode = MockDbSet(networkNode);
            mocknetworkNode.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynNetworkNode.AsQueryable().Provider);
            mocknetworkNode.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynNetworkNode.AsQueryable().Expression);
            mocknetworkNode.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynNetworkNode.AsQueryable().ElementType);
            mocknetworkNode.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<NetworkNode>)mockAsynNetworkNode).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<NetworkNode>()).Returns(mocknetworkNode.Object);
            mockRMDBContext.Setup(x => x.NetworkNodes).Returns(mocknetworkNode.Object);
            mocknetworkNode.Setup(x => x.Include(It.IsAny<string>())).Returns(mocknetworkNode.Object);
            mocknetworkNode.Setup(x => x.AsNoTracking()).Returns(mocknetworkNode.Object);

            // setup for PostalAdress
            var mockAsynPostalAdress = new DbAsyncEnumerable<PostalAddress>(postalAddress);
            var mocknpostalAddresse = MockDbSet(postalAddress);
            mocknpostalAddresse.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynPostalAdress.AsQueryable().Provider);
            mocknpostalAddresse.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynPostalAdress.AsQueryable().Expression);
            mocknpostalAddresse.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynPostalAdress.AsQueryable().ElementType);
            mocknpostalAddresse.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostalAddress>)mockAsynPostalAdress).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<PostalAddress>()).Returns(mocknpostalAddresse.Object);
            mockRMDBContext.Setup(x => x.PostalAddresses).Returns(mocknpostalAddresse.Object);
            mocknpostalAddresse.Setup(x => x.Include(It.IsAny<string>())).Returns(mocknpostalAddresse.Object);
            mocknpostalAddresse.Setup(x => x.AsNoTracking()).Returns(mocknpostalAddresse.Object);

            // setup for delivery point status
            var mockAsyndpStatus = new DbAsyncEnumerable<DeliveryPointStatus>(deliveryPointStatus);
            var mockdpStatus = MockDbSet(deliveryPointStatus);
            mockdpStatus.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsyndpStatus.AsQueryable().Provider);
            mockdpStatus.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsyndpStatus.AsQueryable().Expression);
            mockdpStatus.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsyndpStatus.AsQueryable().ElementType);
            mockdpStatus.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryPointStatus>)mockAsyndpStatus).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<DeliveryPointStatus>()).Returns(mockdpStatus.Object);
            mockRMDBContext.Setup(x => x.DeliveryPointStatus).Returns(mockdpStatus.Object);
            mockdpStatus.Setup(x => x.Include(It.IsAny<string>())).Returns(mockdpStatus.Object);
            mockdpStatus.Setup(x => x.AsNoTracking()).Returns(mockdpStatus.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            // mockConfigurationHelper = new Mock<IConfigurationHelper>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<DeliveryPointDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            mockRMDBContext.Setup(n => n.SaveChangesAsync()).ReturnsAsync(1);
            testCandidate = new DeliveryPointsDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}