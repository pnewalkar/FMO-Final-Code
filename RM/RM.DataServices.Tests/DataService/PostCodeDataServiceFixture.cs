using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.DataServices.Tests.DataService
{
    [TestFixture]
    public class PostCodeDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IPostCodeDataService testCandidate;
        private Guid deliveryUnitID = new Guid("8534AA41-391F-4579-A18D-D7EDF5B5F918");
        private Mock<ILoggingHelper> loggingHelperMock;

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
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            loggingHelperMock = CreateMock<ILoggingHelper>();

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
                new Postcode()
                {
                    PostcodeUnit = "search12",
                PostcodeSector = postcodeSectorSingle1,
                SectorGUID = new Guid("0C65F088-874F-40FB-A753-8F29C371821F"),
                InwardCode = "1",
                OutwardCode = "2",
                Sector = "test1"
                }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<Postcode>(postcode);
            var mockPostCodeDataService = MockDbSet(postcode);
            mockPostCodeDataService.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockPostCodeDataService.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockPostCodeDataService.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockPostCodeDataService.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Postcode>)mockAsynEnumerable).GetAsyncEnumerator());
            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<Postcode>()).Returns(mockPostCodeDataService.Object);
            mockRMDBContext.Setup(x => x.Postcodes).Returns(mockPostCodeDataService.Object);
            mockRMDBContext.Setup(c => c.Postcodes.AsNoTracking()).Returns(mockPostCodeDataService.Object);

            var mockAsynEnumerable2 = new DbAsyncEnumerable<UnitPostcodeSector>(unitPostcodeSectors);
            var mockPostCodeDataService2 = MockDbSet(unitPostcodeSectors);
            mockPostCodeDataService2.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockPostCodeDataService2.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockPostCodeDataService2.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockPostCodeDataService2.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<UnitPostcodeSector>)mockAsynEnumerable2).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<UnitPostcodeSector>()).Returns(mockPostCodeDataService2.Object);
            mockRMDBContext.Setup(x => x.UnitPostcodeSectors).Returns(mockPostCodeDataService2.Object);
            mockRMDBContext.Setup(c => c.UnitPostcodeSectors.AsNoTracking()).Returns(mockPostCodeDataService2.Object);

            var mockAsynEnumerable3 = new DbAsyncEnumerable<PostcodeSector>(postcodeSector);
            var mockPostCodeDataService3 = MockDbSet(postcodeSector);
            mockPostCodeDataService3.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable3.AsQueryable().Provider);
            mockPostCodeDataService3.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable3.AsQueryable().Expression);
            mockPostCodeDataService3.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable3.AsQueryable().ElementType);
            mockPostCodeDataService3.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostcodeSector>)mockAsynEnumerable3).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<PostcodeSector>()).Returns(mockPostCodeDataService3.Object);
            mockRMDBContext.Setup(x => x.PostcodeSectors).Returns(mockPostCodeDataService3.Object);
            mockRMDBContext.Setup(c => c.PostcodeSectors.AsNoTracking()).Returns(mockPostCodeDataService3.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new PostCodeDataService(mockDatabaseFactory.Object, loggingHelperMock.Object);
        }
    }
}