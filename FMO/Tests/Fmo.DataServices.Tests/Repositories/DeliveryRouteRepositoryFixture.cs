using System.Collections.Generic;
using Fmo.Common.TestSupport;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;
using Moq;
using NUnit.Framework;

namespace Fmo.DataServices.Tests.Repositories
{
    public class DeliveryRouteRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IDeliveryRouteRepository testCandidate;

        [Test]
        public void Test_FetchDeliveryRoute()
        {

            var actualResult = testCandidate.FetchDeliveryRoute(1, 1);
            Assert.IsNotNull(actualResult);
        }

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
