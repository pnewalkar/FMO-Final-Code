namespace Fmo.DataServices.Tests.Repositories
{
    using System.Collections.Generic;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using Moq;
    using NUnit.Framework;

    public class DeliveryPointRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IDeliveryPointsRepository testCandidate;
        private string coordinates;

        [Test]
        public void Test_GetDeliveryPoints()
        {
            coordinates = "1234.87";
            var actualResult = testCandidate.GetDeliveryPoints1(coordinates);
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Test_GetData()
        {
            coordinates = "1234.87";
            var actualResult = testCandidate.GetData(coordinates);
            Assert.IsNotNull(actualResult);
        }

        protected override void OnSetup()
        {
            List<DeliveryPoint> lstDeliveryPoint = new List<DeliveryPoint>();
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>()
                        {
                            new PostalAddressDTO
                            {
                                BuildingName = "Bldg 1",
                                BuildingNumber = 23,
                                Postcode = "123"

        }

    };
            var mocdeliveryPointDBSet = MockDbSet(lstPostalAddressDTO);

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new DeliveryPointsRepository(mockDatabaseFactory.Object);
        }
    }
}
