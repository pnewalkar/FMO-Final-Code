﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.AccessLink.WebAPI.DataDTOs;
using RM.DataManagement.AccessLink.WebAPI.DataService.Implementation;
using RM.DataManagement.AccessLink.WebAPI.DataService.Interfaces;
using RM.DataManagement.AccessLink.WebAPI.Entities;

namespace RM.DataServices.Tests.DataService
{
    /// <summary>
    /// This class contains test methods for AccessLinkDataService.
    /// </summary>
    [TestFixture]
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
        private NetworkLinkDataDTO netWorkLinkDataDto;
        private AccessLinkDataDTO accessLinkDataDTO = default(AccessLinkDataDTO);

        /// <summary>
        /// Test for Load AccessLink.
        /// </summary>
        [Test]
        public void Test_GetAccessLinks()
        {
            coordinates = "POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))";
            var actualResult = testCandidate.GetAccessLinks(coordinates, unit1Guid);
            Assert.IsNotNull(actualResult);
        }

        /// <summary>
        /// Test for Create Automatic AccessLink.
        /// </summary>
        [Test]
        public void Test_CreateAccessLink()
        {
            var actualResult = testCandidate.CreateAccessLink(accessLinkDataDTO);
            Assert.IsNotNull(actualResult);
        }

        /// <summary>
        /// Test for Create Manual AccessLink.
        /// </summary>
        [Test]
        public void Test_CreateManualAccessLink()
        {
            var actualResult = testCandidate.CreateAccessLink(accessLinkDataDTO);
            Assert.IsNotNull(actualResult);
        }

        /// <summary>
        /// Test for Get Intersection count for deliverypoint.
        /// </summary>
        [Test]
        public void Test_GetIntersectionCountForDeliveryPoint()
        {
            DbGeometry operationalObjectPoint = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);
            DbGeometry accessLinkLine = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);
            var actualResult = testCandidate.GetIntersectionCountForDeliveryPoint(operationalObjectPoint, accessLinkLine);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(actualResult, 0);
        }

        /// <summary>
        /// Test for Get AccessLink count for crossesor OverLaps.
        /// </summary>
        [Test]
        public void Test_GetAccessLinkCountForCrossesorOverLaps()
        {
            DbGeometry operationalObjectPoint = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);
            DbGeometry accessLinkLine = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);
            var actualResult = testCandidate.CheckAccessLinkCrossesorOverLaps(operationalObjectPoint, accessLinkLine);
            Assert.IsNotNull(actualResult);
            Assert.False(actualResult);
        }

        /// <summary>
        /// Test for Get AccessLink Crossing operational object.
        /// </summary>
        [Test]
        public void Test_GetAccessLinksCrossingOperationalObject()
        {
            string coordinates = "POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))";
            DbGeometry accessLinkLine = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);
            var actualResult = testCandidate.GetAccessLinksCrossingOperationalObject(coordinates, accessLinkLine);
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Test_GetCrossingNetworkLink()
        {
            string coordinates = "POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))";
            DbGeometry accessLinkLine = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);
            bool actualResult = testCandidate.GetCrossingNetworkLink(coordinates, accessLinkLine);
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Delete access link once Delivery point is deleted.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_DeleteAccessLink_PositiveScenario()
        {
            var actualResult = await testCandidate.DeleteAccessLink(unit1Guid, unit2Guid, unit3Guid);
            Assert.IsNotNull(actualResult);
        }

        /// <summary>
        /// Setup for Nunit Tests.
        /// </summary>
        protected override void OnSetup()
        {
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            unit1Guid = Guid.NewGuid();
            unit2Guid = Guid.NewGuid();
            unit3Guid = Guid.NewGuid();
            user1Id = System.Guid.NewGuid();
            user2Id = System.Guid.NewGuid();

            var unitBoundary = DbGeometry.PolygonFromText("POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))", 27700);
            var location = new List<Location>() { new Location() { ID = unit1Guid, Shape = unitBoundary } };
            var networkLinks = new List<NetworkLink>() { new NetworkLink() { LinkGeometry = unitBoundary, } };
            var networkLink = new NetworkLink() { LinkGeometry = unitBoundary, };
            var accessLink = new List<AccessLink>() { new AccessLink() { ID = Guid.NewGuid(), NetworkLink = networkLink } };
            var deliveryPoint = new List<DeliveryPoint>() { new DeliveryPoint() { NetworkNode = new NetworkNode() { Location = new Location() { Shape = unitBoundary } } } };

            var networkNodeDataDTO = new NetworkNodeDataDTO()
            {
                ID = Guid.NewGuid(),
                Location = new LocationDataDTO()
                {
                    ID = Guid.NewGuid(),
                    Shape = unitBoundary,
                    RowCreateDateTime = DateTime.UtcNow
                }
            };

            var accessLinkDataDTOs = new AccessLinkDataDTO()
            {
                Approved = true,
                ID = Guid.NewGuid(),
                WorkloadLengthMeter = 40,
                LinkDirectionGUID = Guid.NewGuid(),
                ConnectedNetworkLinkID = Guid.NewGuid(),
                AccessLinkTypeGUID = Guid.NewGuid()
            };

            var accessLinkStatus = new AccessLinkStatusDataDTO()
            {
                ID = Guid.NewGuid(),
                NetworkLinkID = Guid.NewGuid(),
                AccessLinkStatusGUID = Guid.NewGuid(),
                StartDateTime = DateTime.UtcNow,
                RowCreateDateTime = DateTime.UtcNow
            };

            netWorkLinkDataDto = new NetworkLinkDataDTO()
            {
                ID = Guid.NewGuid(),
                DataProviderGUID = Guid.NewGuid(),
                NetworkLinkTypeGUID = Guid.NewGuid(),
                StartNodeID = Guid.NewGuid(),
                EndNodeID = Guid.NewGuid(),
                LinkLength = 40,
                LinkGeometry = unitBoundary,
                RowCreateDateTime = DateTime.UtcNow,
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<AccessLink>(accessLink);
            mockFmoDbContext = CreateMock<AccessLinkDBContext>();
            mockFmoDbContext.Setup(x => x.SaveChanges()).Returns(1);
            var mockAccessLinkDataService = MockDbSet(accessLink);
            mockFmoDbContext.Setup(x => x.Set<AccessLink>()).Returns(mockAccessLinkDataService.Object);
            mockFmoDbContext.Setup(x => x.AccessLinks).Returns(mockAccessLinkDataService.Object);

            // mockFmoDbContext.Setup(c => c.AccessLinks.AsNoTracking()).Returns(mockAccessLinkDataService.Object);
            mockAccessLinkDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockAccessLinkDataService.Object);
            mockAccessLinkDataService.Setup(x => x.AsNoTracking()).Returns(mockAccessLinkDataService.Object);

            var mockAccessLinkDataService1 = MockDbSet(networkLinks);
            mockFmoDbContext.Setup(x => x.Set<NetworkLink>()).Returns(mockAccessLinkDataService1.Object);
            mockFmoDbContext.Setup(x => x.NetworkLinks).Returns(mockAccessLinkDataService1.Object);

            // mockFmoDbContext.Setup(c => c.NetworkLinks.AsNoTracking()).Returns(mockAccessLinkDataService1.Object);
            mockAccessLinkDataService1.Setup(x => x.AsNoTracking()).Returns(mockAccessLinkDataService1.Object);

            var mockAccessLinkDataService4 = MockDbSet(deliveryPoint);
            mockFmoDbContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockAccessLinkDataService4.Object);
            mockFmoDbContext.Setup(x => x.DeliveryPoints).Returns(mockAccessLinkDataService4.Object);
            mockAccessLinkDataService4.Setup(x => x.AsNoTracking()).Returns(mockAccessLinkDataService4.Object);

            var mockAsynEnumerable2 = new DbAsyncEnumerable<Location>(location);
            var mockAccessLinkDataService2 = MockDbSet(location);
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