using System;

namespace RM.DataServices.Tests.DataService
{
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using CommonLibrary.DataMiddleware;
    using CommonLibrary.EntityFramework.DataService;
    using CommonLibrary.EntityFramework.DataService.Interfaces;
    using CommonLibrary.EntityFramework.DTO;
    using CommonLibrary.EntityFramework.Entities;
    using CommonLibrary.LoggingMiddleware;
    using Moq;
    using NUnit.Framework;
    using RM.CommonLibrary.HelperMiddleware;

    public class AccessLinkDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IAccessLinkDataService testCandidate;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private string coordinates;
        private Guid unit1Guid = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid unit2Guid = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid unit3Guid = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid user1Id;
        private Guid user2Id;
        private AccessLinkDTO accessLinkDto;

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
            var actualResult = testCandidate.CreateAccessLink(accessLinkDto);
            Assert.IsNotNull(actualResult);
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

            var unitLocation = new List<UnitLocation>()
            {
                new UnitLocation() { UnitName = "unit1", ExternalId = "extunit1", ID = unit1Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit1Guid, User_GUID = user1Id } } },
                new UnitLocation() { UnitName = "unit2", ExternalId = "extunit2", ID = unit2Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit2Guid, User_GUID = user1Id } } },
                new UnitLocation() { UnitName = "unit3", ExternalId = "extunit2", ID = unit3Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit3Guid, User_GUID = user2Id } } }
            };

            var accessLink = new List<AccessLink>()
            {
               new AccessLink()
               {
                 AccessLinkLine = unitBoundary,
                ID = Guid.NewGuid()
                      }
            };

            accessLinkDto = new AccessLinkDTO()
            {
                OperationalObjectPoint = DbGeometry.PointFromText("POINT (488938 197021)", 27700),
                NetworkIntersectionPoint = null,
                AccessLinkLine = null,
                ActualLengthMeter = 3,
                WorkloadLengthMeter = 5,
                Approved = true,
                OperationalObject_GUID = Guid.NewGuid()
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<AccessLink>(accessLink);
            var mockAccessLinkDataService = MockDbSet(accessLink);
            mockFmoDbContext = CreateMock<RMDBContext>();
            mockFmoDbContext.Setup(x => x.Set<AccessLink>()).Returns(mockAccessLinkDataService.Object);
            mockFmoDbContext.Setup(x => x.AccessLinks).Returns(mockAccessLinkDataService.Object);
            mockFmoDbContext.Setup(c => c.AccessLinks.AsNoTracking()).Returns(mockAccessLinkDataService.Object);
            mockAccessLinkDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockAccessLinkDataService.Object);
            var mockAsynEnumerable2 = new DbAsyncEnumerable<UnitLocation>(unitLocation);
            var mockAccessLinkDataService2 = MockDbSet(unitLocation);
            mockFmoDbContext.Setup(x => x.Set<UnitLocation>()).Returns(mockAccessLinkDataService2.Object);
            mockFmoDbContext.Setup(x => x.UnitLocations).Returns(mockAccessLinkDataService2.Object);
            mockFmoDbContext.Setup(c => c.UnitLocations.AsNoTracking()).Returns(mockAccessLinkDataService2.Object);
            mockAccessLinkDataService2.Setup(x => x.Include(It.IsAny<string>())).Returns(mockAccessLinkDataService2.Object);
            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new AccessLinkDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}