using System;
using Fmo.Common.TestSupport;
using System.Collections.Generic;
using Fmo.Entities;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DataServices.DBContext;
using Moq;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories;

namespace Fmo.DataServices.Tests.Repositories
{
    public class DeliveryRouteRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IDeliveryRouteRepository testCandidate;

        protected override void OnSetup()
        {
            var deliveryRoute = new List<DeliveryRoute>()
            {
                new DeliveryRoute() { OperationalStatus_Id = 1, DeliveryScenario_Id = 1 }
            };

            var mockDeliveryRouteDBSet = MockDbSet(deliveryRoute);

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<DeliveryRoute>()).Returns(mockDeliveryRouteDBSet.Object);
            mockFmoDbContext.Setup(x => x.DeliveryRoutes).Returns(mockDeliveryRouteDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);

            testCandidate = new DeliveryRouteRepository(mockDatabaseFactory.Object);
        }
    }
}
