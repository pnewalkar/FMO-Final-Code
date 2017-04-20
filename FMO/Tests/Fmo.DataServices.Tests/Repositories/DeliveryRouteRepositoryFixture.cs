﻿namespace Fmo.DataServices.Tests.Repositories
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
        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid deliveryScenarioID = System.Guid.NewGuid();
        private Guid operationalStateID = System.Guid.NewGuid();

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
            var actualResult = await testCandidate.FetchDeliveryRouteForBasicSearch("invalid_testsearch", deliveryUnitID);
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
            var actualResultCount = await testCandidate.GetDeliveryRouteCount("invalid_testsearch", deliveryUnitID);
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
                new DeliveryRoute() { DeliveryRoute_Id = 1, OperationalStatus_Id = 1, DeliveryScenario_Id = 1, RouteName = "testsearch1jbcjkdsghfjks", RouteNumber = "testsearch1jbcjkdsghfjks" },
                new DeliveryRoute() { DeliveryRoute_Id = 2, OperationalStatus_Id = 2, DeliveryScenario_Id = 2, RouteName = "testsearch2jbcjkdsghfjks", RouteNumber = "testsearch2jbcjkdsghfjks" },
                new DeliveryRoute() { DeliveryRoute_Id = 3, OperationalStatus_Id = 3, DeliveryScenario_Id = 3, RouteName = "testsearch3jbcjkdsghfjks", RouteNumber = "testsearch3jbcjkdsghfjks" },
                new DeliveryRoute() { DeliveryRoute_Id = 4, OperationalStatus_Id = 4, DeliveryScenario_Id = 4, RouteName = "testsearch4jbcjkdsghfjks", RouteNumber = "testsearch4jbcjkdsghfjks" },
                new DeliveryRoute() { DeliveryRoute_Id = 5, OperationalStatus_Id = 5, DeliveryScenario_Id = 5, RouteName = "testsearch5jbcjkdsghfjks", RouteNumber = "testsearch5jbcjkdsghfjks" },
                new DeliveryRoute() { DeliveryRoute_Id = 6, OperationalStatus_Id = 6, DeliveryScenario_Id = 6, RouteName = "testsearch6jbcjkdsghfjks", RouteNumber = "testsearch6jbcjkdsghfjks" },
                new DeliveryRoute() { DeliveryRoute_Id = 7, OperationalStatus_Id = 7, DeliveryScenario_Id = 7, RouteName = "testsearch7jbcjkdsghfjks", RouteNumber = "testsearch7jbcjkdsghfjks" }
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
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new DeliveryRouteRepository(mockDatabaseFactory.Object);
        }
    }
}