namespace Fmo.DataServices.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using Moq;
    using NUnit.Framework;
    using NUnit.Framework.Internal;

    public class UnitLocationRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IUnitLocationRepository testCandidate;
        private Guid unit1Guid;
        private Guid unit2Guid;
        private Guid unit3Guid;
        private Guid user1Id;
        private Guid user2Id;


        //[Test]
        //public void Test_FetchDeliveryUnit()
        //{
        //    var actualResult = testCandidate.FetchDeliveryUnit(unit1Guid);
        //    Assert.IsNotNull(actualResult);
        //    Assert.AreEqual(actualResult.UnitName, "unit1");
        //    Assert.AreEqual(actualResult.ExternalId, "extunit1");
        //    Assert.AreEqual(actualResult.ID, unit1Guid);
        //}

        //[Test]
        //public void Test_FetchDeliveryUnitForUsers()
        //{
        //    var actualResult = testCandidate.FetchDeliveryUnitsForUser(user1Id);
        //    Assert.IsNotNull(actualResult);
        //    Assert.IsTrue(actualResult.Count == 2);
        //}

        protected override void OnSetup()
        {
            unit1Guid = Guid.NewGuid();
            unit2Guid = Guid.NewGuid();
            unit3Guid = Guid.NewGuid();
            user1Id = System.Guid.NewGuid();
            user2Id = System.Guid.NewGuid();
            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);

            var unitLocation = new List<UnitLocation>()
            {
                new UnitLocation() { UnitName = "unit1", ExternalId = "extunit1", ID = unit1Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit1Guid, User_GUID = user1Id } } },
                new UnitLocation() { UnitName = "unit2", ExternalId = "extunit2", ID = unit2Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit2Guid, User_GUID = user1Id } } },
                new UnitLocation() { UnitName = "unit3", ExternalId = "extunit2", ID = unit3Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit3Guid, User_GUID = user2Id } } }
            };

            var mockDeliveryUnitLocationDBSet = MockDbSet(unitLocation);

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<UnitLocation>()).Returns(mockDeliveryUnitLocationDBSet.Object);
            mockFmoDbContext.Setup(x => x.UnitLocations).Returns(mockDeliveryUnitLocationDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);

            mockFmoDbContext.Setup(x => x.UnitLocations).Returns(It.IsAny<DbSet<UnitLocation>>());
            mockFmoDbContext.Setup(x => x.UnitLocations.AsNoTracking()).Returns(It.IsAny<DbSet<UnitLocation>>());
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(It.IsAny<DbSet<PostalAddress>>());
            mockFmoDbContext.Setup(x => x.PostalAddresses.AsNoTracking()).Returns(It.IsAny<DbSet<PostalAddress>>());
            mockFmoDbContext.Setup(x => x.Postcodes).Returns(It.IsAny<DbSet<Postcode>>());
            mockFmoDbContext.Setup(x => x.Postcodes.AsNoTracking()).Returns(It.IsAny<DbSet<Postcode>>());
            mockFmoDbContext.Setup(x => x.PostcodeSectors).Returns(It.IsAny<DbSet<PostcodeSector>>());
            mockFmoDbContext.Setup(x => x.PostcodeSectors.AsNoTracking()).Returns(It.IsAny<DbSet<PostcodeSector>>());
            mockFmoDbContext.Setup(x => x.PostcodeDistricts).Returns(It.IsAny<DbSet<PostcodeDistrict>>());
            mockFmoDbContext.Setup(x => x.PostcodeDistricts.AsNoTracking()).Returns(It.IsAny<DbSet<PostcodeDistrict>>());
            mockFmoDbContext.Setup(x => x.PostcodeAreas).Returns(It.IsAny<DbSet<PostcodeArea>>());
            mockFmoDbContext.Setup(x => x.PostcodeAreas.AsNoTracking()).Returns(It.IsAny<DbSet<PostcodeArea>>());
            mockFmoDbContext.Setup(x => x.UserRoleUnits).Returns(It.IsAny<DbSet<UserRoleUnit>>());
            mockFmoDbContext.Setup(x => x.UserRoleUnits.AsNoTracking()).Returns(It.IsAny<DbSet<UserRoleUnit>>());

            testCandidate = new UnitLocationRepository(mockDatabaseFactory.Object);

        }
    }
}