namespace Fmo.DataServices.Tests.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using Fmo.Common.AsyncEnumerator;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DeliveryPointsRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IDeliveryPointsRepository testCandidate;

        [Test]
        public async Task TestFetchDeliveryPointsForBasicSearchValid()
        {
            var actualResult = await testCandidate.FetchDeliveryPointsForBasicSearch("Org");
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task TestFetchDeliveryPointsForBasicSearchInvalid()
        {
            var actualResult = await testCandidate.FetchDeliveryPointsForBasicSearch("invalid_org");
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 0);
        }

        [Test]
        public async Task TestFetchDeliveryPointsForBasicSearchNull()
        {
            var actualResult = await testCandidate.FetchDeliveryPointsForBasicSearch(null);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task TestGetDeliveryPointsCountValid()
        {
            var actualResultCount = await testCandidate.GetDeliveryPointsCount("Org");
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        [Test]
        public async Task TestGetDeliveryPointsCountInvalid()
        {
            var actualResultCount = await testCandidate.GetDeliveryPointsCount("invalid_Org");
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 0);
        }

        [Test]
        public async Task TestGetDeliveryPointsCountNull()
        {
            var actualResultCount = await testCandidate.GetDeliveryPointsCount(null);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        protected override void OnSetup()
        {
            var deliveryPoint = new List<DeliveryPoint>()
            {
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg1", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 1, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    }
               },
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg2", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 2, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    }
               },
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg3", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 3, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    }
               },
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg4", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 4862, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    }
               },
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg5", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 1, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    }
               },
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg6", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 1, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    }
               },
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg7", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 1, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    }
               }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<DeliveryPoint>(deliveryPoint);

            var mockDeliveryPointRepository = MockDbSet(deliveryPoint);

            mockDeliveryPointRepository.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockDeliveryPointRepository.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockDeliveryPointRepository.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockDeliveryPointRepository.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryPoint>)mockAsynEnumerable).GetAsyncEnumerator());

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointRepository.Object);
            mockFmoDbContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointRepository.Object);

            mockFmoDbContext.Setup(c => c.DeliveryPoints.AsNoTracking()).Returns(mockDeliveryPointRepository.Object);

            mockDeliveryPointRepository.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointRepository.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new DeliveryPointsRepository(mockDatabaseFactory.Object);
        }
    }
}