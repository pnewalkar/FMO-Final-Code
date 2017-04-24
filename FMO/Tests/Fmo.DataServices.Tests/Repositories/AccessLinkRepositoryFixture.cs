using System;

namespace Fmo.DataServices.Tests.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using Common.AsyncEnumerator;
    using Entities;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories;
    using Fmo.DataServices.Repositories.Interfaces;
    using Moq;
    using NUnit.Framework;

    public class AccessLinkRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IAccessLinkRepository testCandidate;
        private string coordinates;
        private Guid ID = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid unit1Guid = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid unit2Guid = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid unit3Guid = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid user1Id;
        private Guid user2Id;

        [Test]
        public void Test_GetAccessLinks()
        {
            coordinates = "POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))";
            var actualResult = testCandidate.GetAccessLinks(coordinates, unit1Guid);
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

            var accessLink = new List<AccessLink>()
            {
               new AccessLink()
               {
                 AccessLinkLine = unitBoundary,
                NetworkLink_Id = 55
                      }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<AccessLink>(accessLink);
            var mockAccessLinkRepository = MockDbSet(accessLink);
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<AccessLink>()).Returns(mockAccessLinkRepository.Object);
            mockFmoDbContext.Setup(x => x.AccessLinks).Returns(mockAccessLinkRepository.Object);
            mockFmoDbContext.Setup(c => c.AccessLinks.AsNoTracking()).Returns(mockAccessLinkRepository.Object);
            mockAccessLinkRepository.Setup(x => x.Include(It.IsAny<string>())).Returns(mockAccessLinkRepository.Object);
            var mockAsynEnumerable2 = new DbAsyncEnumerable<UnitLocation>(unitLocation);
            var mockAccessLinkRepository2 = MockDbSet(unitLocation);
            mockFmoDbContext.Setup(x => x.Set<UnitLocation>()).Returns(mockAccessLinkRepository2.Object);
            mockFmoDbContext.Setup(x => x.UnitLocations).Returns(mockAccessLinkRepository2.Object);
            mockFmoDbContext.Setup(c => c.UnitLocations.AsNoTracking()).Returns(mockAccessLinkRepository2.Object);
            mockAccessLinkRepository2.Setup(x => x.Include(It.IsAny<string>())).Returns(mockAccessLinkRepository2.Object);
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new AccessLinkRepository(mockDatabaseFactory.Object);
        }
    }
}