namespace RM.DataServices.Tests.DataService
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using RM.CommonLibrary.DataMiddleware;
    using RM.CommonLibrary.EntityFramework.DataService;
    using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
    using RM.CommonLibrary.EntityFramework.Entities;
    using RM.CommonLibrary.HelperMiddleware;

    public class ScenarioDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IScenarioDataService testCandidate;
        private Guid deliveryUnitID = System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
        private Guid operationalStateID = System.Guid.Parse("9C1E56D7-5397-4984-9CF0-CD9EE7093C88");

        [Test]
        public void Test_FetchDeliveryScenario()
        {
            var actualResult = testCandidate.FetchScenario(operationalStateID, deliveryUnitID);
            Assert.IsNotNull(actualResult);
        }

        protected override void OnSetup()
        {
            var deliveryScenario = new List<Scenario>()
            {
                new Scenario() { ScenarioName = "Worthing Delivery Office - Baseline weekday",  Unit_GUID = System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050"), OperationalState_GUID = System.Guid.Parse("9C1E56D7-5397-4984-9CF0-CD9EE7093C88") },
                new Scenario() { ScenarioName = "High Wycombe North Delivery Office - Baseline week",  Unit_GUID = System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050"), OperationalState_GUID = System.Guid.Parse("9C1E56D7-5397-4984-9CF0-CD9EE7093C88") },
                new Scenario() { ScenarioName = "High Wycombe South Delivery Office - Baseline week",  Unit_GUID = System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050"), OperationalState_GUID = System.Guid.Parse("9C1E56D7-5397-4984-9CF0-CD9EE7093C88") }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<Scenario>(deliveryScenario);

            var mockDeliveryScenarioDBSet = MockDbSet(deliveryScenario);

            mockDeliveryScenarioDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockDeliveryScenarioDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockDeliveryScenarioDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockDeliveryScenarioDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Scenario>)mockAsynEnumerable).GetAsyncEnumerator());

            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<Scenario>()).Returns(mockDeliveryScenarioDBSet.Object);
            mockRMDBContext.Setup(x => x.Scenarios).Returns(mockDeliveryScenarioDBSet.Object);

            mockRMDBContext.Setup(c => c.Scenarios.AsNoTracking()).Returns(mockDeliveryScenarioDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new ScenarioDataService(mockDatabaseFactory.Object);
        }
    }
}