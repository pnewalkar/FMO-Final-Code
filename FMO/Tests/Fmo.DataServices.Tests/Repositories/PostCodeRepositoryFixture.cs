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

namespace Fmo.DataServices.Tests.Repositories
{
    [TestFixture]
    public class PostCodeRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IPostCodeRepository testCandidate;
        private Guid deliveryUnitID = System.Guid.NewGuid();

        [Test]
        public async Task TestFetchPostCodeUnitForBasicSearchValid()
        {
            var actualResult = await testCandidate.FetchPostCodeUnitForBasicSearch("se", deliveryUnitID);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task TestFetchPostCodeUnitForBasicSearchInvalid()
        {
            var actualResult = await testCandidate.FetchPostCodeUnitForBasicSearch("invalid_searchtest", deliveryUnitID);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 0);
        }

        [Test]
        public async Task TestFetchPostCodeUnitForBasicSearchNull()
        {
            var actualResult = await testCandidate.FetchPostCodeUnitForBasicSearch(null, deliveryUnitID);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task TestGetPostCodeUnitCountValid()
        {
            var actualResultCount = await testCandidate.GetPostCodeUnitCount("search", deliveryUnitID);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        [Test]
        public async Task TestGetPostCodeUnitCountInvalid()
        {
            var actualResultCount = await testCandidate.GetPostCodeUnitCount("searchtest", deliveryUnitID);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 0);
        }

        [Test]
        public async Task TestGetPostCodeUnitCountNull()
        {
            var actualResultCount = await testCandidate.GetPostCodeUnitCount(null, deliveryUnitID);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        protected override void OnSetup()
        {
            var postcode = new List<Postcode>()
            {
                new Postcode() { PostcodeUnit = "search" },
                new Postcode() { PostcodeUnit = "searchsdfsd" },
                new Postcode() { PostcodeUnit = "searchsdgsg" },
                new Postcode() { PostcodeUnit = "searchhsrth" },
                new Postcode() { PostcodeUnit = "searchrthrth" },
                new Postcode() { PostcodeUnit = "searchrthrthew" },
                new Postcode() { PostcodeUnit = "searchrghrth" }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<Postcode>(postcode);

            var mockPostCodeRepository = MockDbSet(postcode);

            mockPostCodeRepository.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockPostCodeRepository.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockPostCodeRepository.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockPostCodeRepository.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Postcode>)mockAsynEnumerable).GetAsyncEnumerator());

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<Postcode>()).Returns(mockPostCodeRepository.Object);
            mockFmoDbContext.Setup(x => x.Postcodes).Returns(mockPostCodeRepository.Object);
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new PostCodeRepository(mockDatabaseFactory.Object);
        }
    }
}
