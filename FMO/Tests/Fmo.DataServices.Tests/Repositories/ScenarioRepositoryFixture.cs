using System;
using Fmo.Common.TestSupport;
using Fmo.DataServices.DBContext;
using Moq;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;
using System.Collections.Generic;
using Fmo.DataServices.Repositories;

namespace Fmo.DataServices.Tests.Repositories
{
    public class ScenarioRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IScenarioRepository testCandidate;

        protected override void OnSetup()
        {
            var deliveryScenario = new List<Scenario>()
            {
                new Scenario() { OperationalState_Id = 1, DeliveryScenario_Id = 1 }
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
