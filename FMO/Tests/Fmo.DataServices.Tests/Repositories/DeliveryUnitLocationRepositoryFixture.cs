namespace Fmo.DataServices.Tests.Repositories
{
    using System.Collections.Generic;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using Moq;
    using NUnit.Framework;

    public class DeliveryUnitLocationRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IDeliveryUnitLocationRepository testCandidate;

        [Test]
        public void Test_FetchDeliveryUnit()
        {
            var actualResult = testCandidate.FetchDeliveryUnit();
            Assert.IsNotNull(actualResult);
        }

        protected override void OnSetup()
        {
            var deliveryUnitLocation = new List<UnitLocation>()
            {
                new UnitLocation() { UnitName = "unitone", ExternalId = "unitone" }
            };

            var mockDeliveryUnitLocationDBSet = MockDbSet(deliveryUnitLocation);

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<UnitLocation>()).Returns(mockDeliveryUnitLocationDBSet.Object);
            mockFmoDbContext.Setup(x => x.UnitLocations).Returns(mockDeliveryUnitLocationDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);

            testCandidate = new DeliveryUnitLocationRespository(mockDatabaseFactory.Object);
        }
    }
}
