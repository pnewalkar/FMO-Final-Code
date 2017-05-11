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
        private Guid refDataId = new Guid("4A2DE1A4-BC19-4DB8-A8D9-DAA828E1B526");
        private Guid scenarioId = new Guid("4A2DE1A4-BC19-4DB8-A8D9-DAA828E1B527");
        private Guid deliveryRouteId = new Guid("B13D545D-2DE7-4E62-8DAD-00EC2B7FF8B8");
        private Guid operationalObjectTypeForDP = new Guid("9F82733D-C72C-4111-815D-8813790B5CFB");
        private Guid operationalObjectTypeForDPCommercial = new Guid("990B86A2-9431-E711-83EC-28D244AEF9ED");
        private Guid operationalObjectTypeForDPResidential = new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED");

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

        //[Test]
        //public void FetchDeliveryRouteDetailsforPDF()
        //{
        //    SetUp();
        //    var actualResultCount = testCandidate.FetchDeliveryRouteDetailsforPDF(deliveryRouteId,operationalObjectTypeForDP,operationalObjectTypeForDPCommercial,operationalObjectTypeForDPResidential);
        //    Assert.IsNotNull(actualResultCount);
        //    Assert.IsTrue(actualResultCount.Count == 1);
        //}

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

        protected void SetUp()
        {
            var scenario = new List<Scenario>() { new Scenario() { ScenarioName = "ScanarioName", ID = scenarioId } };
            var referenceData = new List<ReferenceData>() { new ReferenceData() { DisplayText = "shared van", ID = refDataId } };
            var deliveryRoute = new List<DeliveryRoute>()
            { new DeliveryRoute()
                {
                    ID=deliveryRouteId,
                    RouteName = "Linkroad",
                    RouteNumber = "1",
                    Scenario = scenario[0],
                    ReferenceData = referenceData[0],
                    RouteMethodType_GUID = refDataId,
                    DeliveryScenario_GUID = scenarioId
                }
            };

            var mockDeliveryRouteDBSet = MockDbSet(deliveryRoute);
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<DeliveryRoute>()).Returns(mockDeliveryRouteDBSet.Object);
            mockFmoDbContext.Setup(x => x.DeliveryRoutes).Returns(mockDeliveryRouteDBSet.Object);
            mockFmoDbContext.Setup(c => c.DeliveryRoutes.AsNoTracking()).Returns(mockDeliveryRouteDBSet.Object);

            var mockReferenceDataDBSet = MockDbSet(referenceData);
            mockFmoDbContext.Setup(x => x.Set<ReferenceData>()).Returns(mockReferenceDataDBSet.Object);
            mockFmoDbContext.Setup(x => x.ReferenceDatas).Returns(mockReferenceDataDBSet.Object);
            mockFmoDbContext.Setup(c => c.ReferenceDatas.AsNoTracking()).Returns(mockReferenceDataDBSet.Object);

            var mockScenarioDBSet = MockDbSet(scenario);
            mockFmoDbContext.Setup(x => x.Set<Scenario>()).Returns(mockScenarioDBSet.Object);
            mockFmoDbContext.Setup(x => x.Scenarios).Returns(mockScenarioDBSet.Object);
            mockFmoDbContext.Setup(c => c.Scenarios.AsNoTracking()).Returns(mockScenarioDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new DeliveryRouteRepository(mockDatabaseFactory.Object);
        }
    }
}