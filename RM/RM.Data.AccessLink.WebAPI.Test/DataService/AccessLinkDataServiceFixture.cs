using System;

namespace RM.DataServices.Tests.DataService
{
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using CommonLibrary.DataMiddleware;
    using CommonLibrary.LoggingMiddleware;
    using Moq;
    using NUnit.Framework;
    using CommonLibrary.HelperMiddleware;
    using DataManagement.AccessLink.WebAPI.DataService.Interfaces;
    using DataManagement.AccessLink.WebAPI.DTOs;
    using Data.AccessLink.WebAPI.DataDTOs;
    using DataManagement.AccessLink.WebAPI.DataService.Implementation;
    using DataManagement.AccessLink.WebAPI.Entities;

    public class AccessLinkDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<AccessLinkDBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<AccessLinkDBContext>> mockDatabaseFactory;
        private IAccessLinkDataService testCandidate;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private string coordinates;
        private Guid unit1Guid = new Guid("B51AA229-C984-4CA6-9C12-510187B81050");
        private Guid unit2Guid = new Guid("5B9C7207-3D20-E711-9F8C-28D244AEF9ED");
        private Guid unit3Guid = new Guid("092C69AE-4382-4183-84FF-BA07543D9C75");
        private Guid user1Id;
        private Guid user2Id;
        private AccessLinkDataDTO accessLinkDataDto;
        private NetworkLinkDataDTO netWorkLinkDataDto;

        [Test]
        public void Test_GetAccessLinks()
        {
            coordinates = "POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))";
            var actualResult = testCandidate.GetAccessLinks(coordinates, unit1Guid);
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Test_CreateAccessLink()
        {
            var actualResult = testCandidate.CreateAccessLink(netWorkLinkDataDto);
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Test_CreateManualAccessLink()
        {
            var actualResult = testCandidate.CreateManualAccessLink(netWorkLinkDataDto);
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Test_GetIntersectionCountForDeliveryPoint()
        {
            DbGeometry operationalObjectPoint = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);
            DbGeometry accessLinkLine = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);
            var actualResult = testCandidate.GetIntersectionCountForDeliveryPoint(operationalObjectPoint, accessLinkLine);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(actualResult, 0);
        }

        [Test]
        public void Test_GetAccessLinkCountForCrossesorOverLaps()
        {
            DbGeometry operationalObjectPoint = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);
            DbGeometry accessLinkLine = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);
            var actualResult = testCandidate.GetAccessLinkCountForCrossesorOverLaps(operationalObjectPoint, accessLinkLine);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(actualResult, 0);
        }

        [Test]
        public void Test_GetAccessLinksCrossingOperationalObject()
        {
            string coordinates = "POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))";
            DbGeometry accessLinkLine = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);
            var actualResult = testCandidate.GetAccessLinksCrossingOperationalObject(coordinates, accessLinkLine);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(actualResult.Count, 0);
        }

        protected override void OnSetup()
        {
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            unit1Guid = Guid.NewGuid();
            unit2Guid = Guid.NewGuid();
            unit3Guid = Guid.NewGuid();
            user1Id = System.Guid.NewGuid();
            user2Id = System.Guid.NewGuid();

            var unitBoundary = DbGeometry.PolygonFromText("POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))", 27700);
            var Location = new List<Location>() { new Location() { ID = unit1Guid, Shape = unitBoundary } };
            var networkLinks = new List<NetworkLink>() { new NetworkLink() { LinkGeometry = unitBoundary, } };
            var networkLink = new NetworkLink() { LinkGeometry = unitBoundary, };
            var accessLink = new List<AccessLink>() { new AccessLink() { ID = Guid.NewGuid(), NetworkLink = networkLink } };
            var deliveryPoint = new List<DeliveryPoint>() { new DeliveryPoint() { NetworkNode = new NetworkNode() { Location = new Location() { Shape = unitBoundary } } } };

            netWorkLinkDataDto = new NetworkLinkDataDTO()
            {
                ID = Guid.NewGuid(),
                AccessLinkDataDTOs = new AccessLinkDataDTO()
                {
                    Approved = true,
                    ID = Guid.NewGuid(),
                    WorkloadLengthMeter = 40,
                    LinkDirectionGUID = Guid.NewGuid(),
                    ConnectedNetworkLinkID = Guid.NewGuid(),
                    AccessLinkStatusDataDTO = new AccessLinkStatusDataDTO()
                    {
                        ID = Guid.NewGuid(),
                        NetworkLinkID = Guid.NewGuid(),
                        AccessLinkStatusGUID = Guid.NewGuid(),
                        StartDateTime = DateTime.UtcNow,
                        RowCreateDateTime = DateTime.UtcNow
                    }
                }
            };


            var mockAsynEnumerable = new DbAsyncEnumerable<AccessLink>(accessLink);
            mockFmoDbContext = CreateMock<AccessLinkDBContext>();
            mockFmoDbContext.Setup(x => x.SaveChanges()).Returns(1);
            var mockAccessLinkDataService = MockDbSet(accessLink);
            mockFmoDbContext.Setup(x => x.Set<AccessLink>()).Returns(mockAccessLinkDataService.Object);
            mockFmoDbContext.Setup(x => x.AccessLinks).Returns(mockAccessLinkDataService.Object);
            //  mockFmoDbContext.Setup(c => c.AccessLinks.AsNoTracking()).Returns(mockAccessLinkDataService.Object);
            mockAccessLinkDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockAccessLinkDataService.Object);
            mockAccessLinkDataService.Setup(x => x.AsNoTracking()).Returns(mockAccessLinkDataService.Object);

            var mockAccessLinkDataService1 = MockDbSet(networkLinks);
            mockFmoDbContext.Setup(x => x.Set<NetworkLink>()).Returns(mockAccessLinkDataService1.Object);
            mockFmoDbContext.Setup(x => x.NetworkLinks).Returns(mockAccessLinkDataService1.Object);
            //  mockFmoDbContext.Setup(c => c.NetworkLinks.AsNoTracking()).Returns(mockAccessLinkDataService1.Object);
            mockAccessLinkDataService1.Setup(x => x.AsNoTracking()).Returns(mockAccessLinkDataService1.Object);

            var mockAccessLinkDataService4 = MockDbSet(deliveryPoint);
            mockFmoDbContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockAccessLinkDataService4.Object);
            mockFmoDbContext.Setup(x => x.DeliveryPoints).Returns(mockAccessLinkDataService4.Object);
            mockAccessLinkDataService4.Setup(x => x.AsNoTracking()).Returns(mockAccessLinkDataService4.Object);

            var mockAsynEnumerable2 = new DbAsyncEnumerable<Location>(Location);
            var mockAccessLinkDataService2 = MockDbSet(Location);
            mockFmoDbContext.Setup(x => x.Set<Location>()).Returns(mockAccessLinkDataService2.Object);
            mockFmoDbContext.Setup(x => x.Locations).Returns(mockAccessLinkDataService2.Object);
            mockFmoDbContext.Setup(c => c.Locations.AsNoTracking()).Returns(mockAccessLinkDataService2.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<AccessLinkDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new AccessLinkDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}