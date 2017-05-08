namespace Fmo.DataServices.Tests.Repositories
{
    using System;
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
    public class DeliveryRouteRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IDeliveryRouteRepository testCandidate;
        private Guid deliveryUnitID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44");
        private Guid deliveryScenarioID = new Guid("D771E341-8FE8-4980-BE96-A339AE014B4E");
        private Guid operationalStateID = new Guid("1FC7DAB1-D4D7-45BD-BA2E-E8AE6737E1EB");
        private Guid invalidId = new Guid("4A2DE1A4-BC19-4DB8-A8D9-DAA828E1B526");

        [Test]
        public void TestFetchDeliveryRoute()
        {
            var actualResult = testCandidate.FetchDeliveryRoute(operationalStateID, deliveryScenarioID, deliveryUnitID);
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public async Task TestFetchDeliveryRouteForBasicSearchValid()
        {
            var actualResult = await testCandidate.FetchDeliveryRouteForBasicSearch("test", deliveryUnitID);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task TestFetchDeliveryRouteForBasicSearchInvalid()
        {
            var actualResult = await testCandidate.FetchDeliveryRouteForBasicSearch("invalid_testsearch", invalidId);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 0);
        }

        [Test]
        public async Task TestFetchDeliveryRouteForBasicSearchNull()
        {
            var actualResult = await testCandidate.FetchDeliveryRouteForBasicSearch(null, deliveryUnitID);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task TestGetDeliveryRouteCountValid()
        {
            var actualResultCount = await testCandidate.GetDeliveryRouteCount("testsearch", deliveryUnitID);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        [Test]
        public async Task TestGetDeliveryRouteCountInvalid()
        {
            var actualResultCount = await testCandidate.GetDeliveryRouteCount("invalid_testsearch", invalidId);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 0);
        }

        [Test]
        public async Task TestGetDeliveryRouteCountNull()
        {
            var actualResultCount = await testCandidate.GetDeliveryRouteCount(null, deliveryUnitID);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        protected override void OnSetup()
        {
            var deliveryRoute = new List<DeliveryRoute>()
            {
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch1jbcjkdsghfjks", RouteNumber = "testsearch1jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } },
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch2jbcjkdsghfjks", RouteNumber = "testsearch2jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } },
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch3jbcjkdsghfjks", RouteNumber = "testsearch3jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } },
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch4jbcjkdsghfjks", RouteNumber = "testsearch4jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } },
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch5jbcjkdsghfjks", RouteNumber = "testsearch5jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } },
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch6jbcjkdsghfjks", RouteNumber = "testsearch6jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } },
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch7jbcjkdsghfjks", RouteNumber = "testsearch7jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<DeliveryRoute>(deliveryRoute);

            var mockDeliveryRouteDBSet = MockDbSet(deliveryRoute);

            mockDeliveryRouteDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockDeliveryRouteDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockDeliveryRouteDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockDeliveryRouteDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryRoute>)mockAsynEnumerable).GetAsyncEnumerator());

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<DeliveryRoute>()).Returns(mockDeliveryRouteDBSet.Object);
            mockFmoDbContext.Setup(x => x.DeliveryRoutes).Returns(mockDeliveryRouteDBSet.Object);

            mockFmoDbContext.Setup(c => c.DeliveryRoutes.AsNoTracking()).Returns(mockDeliveryRouteDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new DeliveryRouteRepository(mockDatabaseFactory.Object);
        }
    }
}