using System.Collections.Generic;
using Fmo.Common.TestSupport;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;
using Moq;
using NUnit.Framework;
using System;

namespace Fmo.DataServices.Tests.Repositories
{
    public class ScenarioRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IScenarioRepository testCandidate;
        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid operationalStateID = System.Guid.NewGuid();

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
                new Scenario() { OperationalState_Id = 1, ID = Guid.NewGuid() }
            };

            var mocdeliveryScenarioDBSet = MockDbSet(deliveryScenario);

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<Scenario>()).Returns(mocdeliveryScenarioDBSet.Object);
            mockFmoDbContext.Setup(x => x.Scenarios).Returns(mocdeliveryScenarioDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);

            testCandidate = new ScenarioRepository(mockDatabaseFactory.Object);
        }
    }
}
