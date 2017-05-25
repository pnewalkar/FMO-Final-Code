using System;

namespace Fmo.DataServices.Tests.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using Common.AsyncEnumerator;
    using Common.Interface;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DeliveryPointRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IDeliveryPointsRepository testCandidate;
        private string coordinates;
        private Guid userId = System.Guid.NewGuid();
        private Guid unit1Guid;
        private Guid unit2Guid;
        private Guid unit3Guid;
        private Guid user1Id;
        private Guid user2Id;
        private Mock<ILoggingHelper> mockLoggingHelper;

        [Test]
        public void Test_GetDeliveryPoints()
        {
            OnSetup();
            coordinates = "POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))";
            var actualResult = testCandidate.GetDeliveryPoints(coordinates, unit1Guid);
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Test_GetDeliveryPointRowVersion()
        {
            DeliveryPointRowVersionSetup();
            var actualResult = testCandidate.GetDeliveryPointRowVersion(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"));
            Assert.IsNotNull(actualResult);
        }

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

            var deliveryPoint = new List<DeliveryPoint>()
            {
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg1", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 1, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    },
                   LocationXY = unitBoundary
               }
            };
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));

            var mockAsynEnumerable = new DbAsyncEnumerable<DeliveryPoint>(deliveryPoint);
            var mockDeliveryPointRepository = MockDbSet(deliveryPoint);
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointRepository.Object);
            mockFmoDbContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointRepository.Object);
            mockFmoDbContext.Setup(c => c.DeliveryPoints.AsNoTracking()).Returns(mockDeliveryPointRepository.Object);
            mockDeliveryPointRepository.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointRepository.Object);
            var mockAsynEnumerable2 = new DbAsyncEnumerable<UnitLocation>(unitLocation);
            var mockDeliveryPointRepository2 = MockDbSet(unitLocation);
            mockFmoDbContext.Setup(x => x.Set<UnitLocation>()).Returns(mockDeliveryPointRepository2.Object);
            mockFmoDbContext.Setup(x => x.UnitLocations).Returns(mockDeliveryPointRepository2.Object);
            mockFmoDbContext.Setup(c => c.UnitLocations.AsNoTracking()).Returns(mockDeliveryPointRepository2.Object);
            mockDeliveryPointRepository2.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointRepository2.Object);
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new DeliveryPointsRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }

        protected void DeliveryPointRowVersionSetup()
        {
            var deliveryPoint = new List<DeliveryPoint>()
            {
               new DeliveryPoint()
               {
                   ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                   RowVersion = BitConverter.GetBytes(1)
               }
            };

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));

            var mockDeliveryPointRepository = MockDbSet(deliveryPoint);
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointRepository.Object);
            mockFmoDbContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointRepository.Object);
            mockDeliveryPointRepository.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointRepository.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new DeliveryPointsRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}