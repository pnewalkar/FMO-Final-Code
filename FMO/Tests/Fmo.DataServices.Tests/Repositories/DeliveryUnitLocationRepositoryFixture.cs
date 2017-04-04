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
    public class DeliveryUnitLocationRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IDeliveryUnitLocationRepository testCandidate;

        protected override void OnSetup()
        {
            var deliveryUnitLocation = new List<DeliveryUnitLocation>()
            {
                new DeliveryUnitLocation() { UnitName = "unitone", ExternalId = "unitone" }
            };

            var mockDeliveryUnitLocationDBSet = MockDbSet(deliveryUnitLocation);

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<DeliveryUnitLocation>()).Returns(mockDeliveryUnitLocationDBSet.Object);
            mockFmoDbContext.Setup(x => x.DeliveryUnitLocations).Returns(mockDeliveryUnitLocationDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);

            testCandidate = new DeliveryUnitLocationRespository(mockDatabaseFactory.Object);
        }
    }
}
