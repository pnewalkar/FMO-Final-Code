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
        private Guid deliveryUnitID = new Guid("8534AA41-391F-4579-A18D-D7EDF5B5F918");

        [Test]
        public async Task TestFetchPostCodeUnitForBasicSearchValid()
        {
            var actualResult = await testCandidate.FetchPostCodeUnitForBasicSearch("se", deliveryUnitID);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 1);
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
            Assert.IsTrue(actualResult.Count == 1);
        }

        [Test]
        public async Task TestGetPostCodeUnitCountValid()
        {
            var actualResultCount = await testCandidate.GetPostCodeUnitCount("search", deliveryUnitID);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 1);
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
            Assert.IsTrue(actualResultCount == 1);
        }

        protected override void OnSetup()
        {
            var unitPostcodeSectors = new List<UnitPostcodeSector>()
                    {
                        new UnitPostcodeSector()
                        {
                            Unit_GUID = new Guid("8534AA41-391F-4579-A18D-D7EDF5B5F918"),
                            PostcodeSector_GUID = new Guid("0C65F088-874F-40FB-A753-8F29C371821F")
                        }
                    };

            var postcodeSectorSingle1 = new PostcodeSector
            {
                ID = new Guid("0C65F088-874F-40FB-A753-8F29C371821F"),
                UnitPostcodeSectors = unitPostcodeSectors
            };

            var postcodeSector = new List<PostcodeSector>()
            {
               new PostcodeSector
               {
                    ID = new Guid("0C65F088-874F-40FB-A753-8F29C371821F"),
                    UnitPostcodeSectors = unitPostcodeSectors
               }
            };

            var postcode = new List<Postcode>()
            {
                new Postcode() { PostcodeUnit = "search12",
                PostcodeSector = postcodeSectorSingle1,
                SectorGUID = new Guid("0C65F088-874F-40FB-A753-8F29C371821F"),
                InwardCode = "1",
                OutwardCode = "2",
                Sector = "test1"
                }
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
            mockFmoDbContext.Setup(c => c.Postcodes.AsNoTracking()).Returns(mockPostCodeRepository.Object);

            var mockAsynEnumerable2 = new DbAsyncEnumerable<UnitPostcodeSector>(unitPostcodeSectors);
            var mockPostCodeRepository2 = MockDbSet(unitPostcodeSectors);
            mockPostCodeRepository2.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockPostCodeRepository2.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockPostCodeRepository2.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockPostCodeRepository2.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<UnitPostcodeSector>)mockAsynEnumerable2).GetAsyncEnumerator());
            mockFmoDbContext.Setup(x => x.Set<UnitPostcodeSector>()).Returns(mockPostCodeRepository2.Object);
            mockFmoDbContext.Setup(x => x.UnitPostcodeSectors).Returns(mockPostCodeRepository2.Object);
            mockFmoDbContext.Setup(c => c.UnitPostcodeSectors.AsNoTracking()).Returns(mockPostCodeRepository2.Object);

            var mockAsynEnumerable3 = new DbAsyncEnumerable<PostcodeSector>(postcodeSector);
            var mockPostCodeRepository3 = MockDbSet(postcodeSector);
            mockPostCodeRepository3.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable3.AsQueryable().Provider);
            mockPostCodeRepository3.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable3.AsQueryable().Expression);
            mockPostCodeRepository3.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable3.AsQueryable().ElementType);
            mockPostCodeRepository3.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostcodeSector>)mockAsynEnumerable3).GetAsyncEnumerator());
            mockFmoDbContext.Setup(x => x.Set<PostcodeSector>()).Returns(mockPostCodeRepository3.Object);
            mockFmoDbContext.Setup(x => x.PostcodeSectors).Returns(mockPostCodeRepository3.Object);
            mockFmoDbContext.Setup(c => c.PostcodeSectors.AsNoTracking()).Returns(mockPostCodeRepository3.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new PostCodeRepository(mockDatabaseFactory.Object);
        }
    }
}