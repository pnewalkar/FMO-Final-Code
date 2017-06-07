namespace RM.DataServices.Tests.DataService
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using RM.CommonLibrary.DataMiddleware;
    using RM.CommonLibrary.EntityFramework.DataService;
    using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
    using RM.CommonLibrary.EntityFramework.Entities;
    using RM.CommonLibrary.HelperMiddleware;
    using Moq;
    using NUnit.Framework;

    public class UnitLocationDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IUnitLocationDataService testCandidate;
        private Guid unit1Guid;
        private Guid unit2Guid;
        private Guid unit3Guid;
        private Guid user1Id;
        private Guid user2Id;
        private Guid postcodeGuid1;
        private Guid postcodeGuid2;
        private Guid sectorGuid;
        private Guid districtGuid;
        private Guid areaGuid;

        [Test]
        public void Test_FetchDeliveryUnitForUser1()
        {
            var actualResult = testCandidate.FetchDeliveryUnitsForUser(user1Id);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 2);
        }

        [Test]
        public void Test_FetchDeliveryUnitForUser2()
        {
            var actualResult = testCandidate.FetchDeliveryUnitsForUser(user2Id);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 1);
        }

        protected override void OnSetup()
        {
            unit1Guid = Guid.NewGuid();
            unit2Guid = Guid.NewGuid();
            unit3Guid = Guid.NewGuid();
            user1Id = Guid.NewGuid();
            user2Id = Guid.NewGuid();
            sectorGuid = Guid.NewGuid();
            districtGuid = Guid.NewGuid();
            areaGuid = Guid.NewGuid();
            postcodeGuid2 = Guid.NewGuid();
            postcodeGuid1 = Guid.NewGuid();

            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);

            var userRoleUnits = new List<UserRoleUnit>()
            {
                new UserRoleUnit { Unit_GUID = unit1Guid, User_GUID = user1Id },
                new UserRoleUnit { Unit_GUID = unit2Guid, User_GUID = user1Id },
                new UserRoleUnit { Unit_GUID = unit3Guid, User_GUID = user2Id }
            };

            var unitLocation = new List<UnitLocation>()
            {
                new UnitLocation() { UnitName = "unit1", UnitAddressUDPRN = 1, ExternalId = "extunit1", ID = unit1Guid, UnitBoundryPolygon = unitBoundary },
                new UnitLocation() { UnitName = "unit2", UnitAddressUDPRN = 2, ExternalId = "extunit2", ID = unit2Guid, UnitBoundryPolygon = unitBoundary },
                new UnitLocation() { UnitName = "unit3", UnitAddressUDPRN = 3, ExternalId = "extunit3", ID = unit3Guid, UnitBoundryPolygon = unitBoundary }
            };

            var mockDeliveryUnitLocationDbSet = MockDbSet(unitLocation);

            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<UnitLocation>()).Returns(mockDeliveryUnitLocationDbSet.Object);
            mockRMDBContext.Setup(x => x.UnitLocations).Returns(mockDeliveryUnitLocationDbSet.Object);
            mockRMDBContext.Setup(x => x.UnitLocations.AsNoTracking()).Returns(mockDeliveryUnitLocationDbSet.Object);

            //    mockDeliveryUnitLocationDbSet.Setup(x => x.SingleOrDefault(It.IsAny<Func<UnitLocation, bool>>())).Returns(unitLocation.FirstOrDefault(u => u.ID == unit1Guid));
            mockDeliveryUnitLocationDbSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryUnitLocationDbSet.Object);

            var postalAddresses = new List<PostalAddress>()
            {
                new PostalAddress() { UDPRN = 1, PostCodeGUID = postcodeGuid1 },
                new PostalAddress() { UDPRN = 2, PostCodeGUID = postcodeGuid2 },
                new PostalAddress() { UDPRN = 3, PostCodeGUID = postcodeGuid2 }
            };
            var mockPostalAddressesDbSet = MockDbSet(postalAddresses);
            mockRMDBContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressesDbSet.Object);
            mockRMDBContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressesDbSet.Object);
            mockRMDBContext.Setup(x => x.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressesDbSet.Object);

            var postcodes = new List<Postcode>()
            {
                new Postcode() { ID = postcodeGuid1, SectorGUID = sectorGuid },
                new Postcode() { ID = postcodeGuid2, SectorGUID = sectorGuid }
            };
            var mockPostCodesDbSet = MockDbSet(postcodes);
            mockRMDBContext.Setup(x => x.Set<Postcode>()).Returns(mockPostCodesDbSet.Object);
            mockRMDBContext.Setup(x => x.Postcodes).Returns(mockPostCodesDbSet.Object);
            mockRMDBContext.Setup(x => x.Postcodes.AsNoTracking()).Returns(mockPostCodesDbSet.Object);

            var postcodeSectors = new List<PostcodeSector>()
            {
                new PostcodeSector() { ID = sectorGuid, DistrictGUID = districtGuid }
            };
            var mockPostcodeSectorDbSet = MockDbSet(postcodeSectors);
            mockRMDBContext.Setup(x => x.Set<PostcodeSector>()).Returns(mockPostcodeSectorDbSet.Object);
            mockRMDBContext.Setup(x => x.PostcodeSectors).Returns(mockPostcodeSectorDbSet.Object);
            mockRMDBContext.Setup(x => x.PostcodeSectors.AsNoTracking()).Returns(mockPostcodeSectorDbSet.Object);

            var postcodeDistricts = new List<PostcodeDistrict>()
            {
                new PostcodeDistrict() { ID = districtGuid, AreaGUID = areaGuid }
            };
            var mockPostcodeDistrictDbSet = MockDbSet(postcodeDistricts);
            mockRMDBContext.Setup(x => x.Set<PostcodeDistrict>()).Returns(mockPostcodeDistrictDbSet.Object);
            mockRMDBContext.Setup(x => x.PostcodeDistricts).Returns(mockPostcodeDistrictDbSet.Object);
            mockRMDBContext.Setup(x => x.PostcodeDistricts.AsNoTracking()).Returns(mockPostcodeDistrictDbSet.Object);

            var postcodeAreas = new List<PostcodeArea>()
            {
                new PostcodeArea() { ID = areaGuid, Area = "Test Area" }
            };
            var mockPostcodeAreasDbSet = MockDbSet(postcodeAreas);
            mockRMDBContext.Setup(x => x.Set<PostcodeArea>()).Returns(mockPostcodeAreasDbSet.Object);
            mockRMDBContext.Setup(x => x.PostcodeAreas).Returns(mockPostcodeAreasDbSet.Object);
            mockRMDBContext.Setup(x => x.PostcodeAreas.AsNoTracking()).Returns(mockPostcodeAreasDbSet.Object);

            var mockUserRoleUnitDbSet = MockDbSet(userRoleUnits);
            mockRMDBContext.Setup(x => x.Set<UserRoleUnit>()).Returns(mockUserRoleUnitDbSet.Object);
            mockRMDBContext.Setup(x => x.UserRoleUnits).Returns(mockUserRoleUnitDbSet.Object);
            mockRMDBContext.Setup(x => x.UserRoleUnits.AsNoTracking()).Returns(mockUserRoleUnitDbSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);

            testCandidate = new UnitLocationDataService(mockDatabaseFactory.Object);
        }
    }
}