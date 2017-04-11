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

namespace Fmo.DataServices.Tests.Repositories
{
    [TestFixture]
    public class StreetNetworkRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IStreetNetworkRepository testCandidate;

        [Test]
        public async Task Test_FetchStreetNamesForBasicSearch_Valid()
        {
            var actualResult = await testCandidate.FetchStreetNamesForBasicSearch("Test");
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task Test_FetchStreetNamesForBasicSearch_inValid()
        {
            var actualResult = await testCandidate.FetchStreetNamesForBasicSearch("invalid_Test");
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 0);
        }

        [Test]
        public async Task Test_FetchStreetNamesForBasicSearch_null()
        {
            var actualResult = await testCandidate.FetchStreetNamesForBasicSearch(null);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task Test_GetStreetNameCount_Valid()
        {
            var actualResultCount = await testCandidate.GetStreetNameCount("Test");
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        [Test]
        public async Task Test_GetStreetNameCount_inValid()
        {
            var actualResultCount = await testCandidate.GetStreetNameCount("invalid_Test");
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 0);
        }

        [Test]
        public async Task Test_GetStreetNameCount_null()
        {
            var actualResultCount = await testCandidate.GetStreetNameCount(null);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        protected override void OnSetup()
        {
            var streetNames = new List<StreetName>()
            {
                new StreetName() { NationalRoadCode = "Testroad1", DesignatedName = "XYZ1" },
                new StreetName() { NationalRoadCode = "Testroad2", DesignatedName = "XYZ2" },
                new StreetName() { NationalRoadCode = "Testroad3", DesignatedName = "XYZ3" },
                new StreetName() { NationalRoadCode = "Testroad4", DesignatedName = "XYZ4" },
                new StreetName() { NationalRoadCode = "Testroad5", DesignatedName = "XYZ5" },
                new StreetName() { NationalRoadCode = "Testroad6", DesignatedName = "XYZ6" },
                new StreetName() { NationalRoadCode = "Testroad7", DesignatedName = "XYZ7" }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<StreetName>(streetNames);

            var mockStreetNetworkRepository = MockDbSet(streetNames);
            mockStreetNetworkRepository.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockStreetNetworkRepository.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockStreetNetworkRepository.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockStreetNetworkRepository.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<StreetName>)mockAsynEnumerable).GetAsyncEnumerator());

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<StreetName>()).Returns(mockStreetNetworkRepository.Object);
            mockFmoDbContext.Setup(x => x.StreetNames).Returns(mockStreetNetworkRepository.Object);
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new StreetNetworkRepository(mockDatabaseFactory.Object);
        }
    }
}
