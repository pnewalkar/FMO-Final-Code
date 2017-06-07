using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.DataServices.Tests.DataService
{
    public class RoadNameDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IRoadNameDataService testCandidate;
        private string coordinates;
        private Guid unit1Guid = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid unit2Guid = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid unit3Guid = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid user1Id;
        private Guid user2Id;

        [Test]
        public void Test_GetRoadRoutes()
        {
            coordinates = "POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))";
            var actualResult = testCandidate.GetRoadRoutes(coordinates, unit1Guid);
            Assert.IsNotNull(actualResult);
        }

        protected override void OnSetup()
        {
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

            var roadName = new List<OSRoadLink>()
            {
               new OSRoadLink()
               {
                  CentreLineGeometry = unitBoundary,
                  RoadName = "Road 001"
                      }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<OSRoadLink>(roadName);
            var mockRoadNameDataService = MockDbSet(roadName);
            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<OSRoadLink>()).Returns(mockRoadNameDataService.Object);
            mockRMDBContext.Setup(x => x.OSRoadLinks).Returns(mockRoadNameDataService.Object);
            mockRMDBContext.Setup(c => c.OSRoadLinks.AsNoTracking()).Returns(mockRoadNameDataService.Object);
            mockRoadNameDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockRoadNameDataService.Object);
            var mockAsynEnumerable2 = new DbAsyncEnumerable<UnitLocation>(unitLocation);
            var mockRoadNameDataService2 = MockDbSet(unitLocation);
            mockRMDBContext.Setup(x => x.Set<UnitLocation>()).Returns(mockRoadNameDataService2.Object);
            mockRMDBContext.Setup(x => x.UnitLocations).Returns(mockRoadNameDataService2.Object);
            mockRMDBContext.Setup(c => c.UnitLocations.AsNoTracking()).Returns(mockRoadNameDataService2.Object);
            mockRoadNameDataService2.Setup(x => x.Include(It.IsAny<string>())).Returns(mockRoadNameDataService2.Object);
            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new RoadNameDataService(mockDatabaseFactory.Object);
        }
    }
}