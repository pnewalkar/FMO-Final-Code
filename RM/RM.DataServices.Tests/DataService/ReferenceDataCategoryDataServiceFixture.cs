namespace RM.DataServices.Tests.DataService
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using RM.CommonLibrary.DataMiddleware;
    using RM.CommonLibrary.EntityFramework.DataService;
    using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
    using RM.CommonLibrary.EntityFramework.Entities;
    using RM.CommonLibrary.HelperMiddleware;

    [TestFixture]
    public class ReferenceDataCategoryDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IReferenceDataCategoryDataService testCandidate;

        [Test]
        public void Test_RouteLogStatus()
        {
            var actualResult = testCandidate.RouteLogStatus();
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Test_ValidGetReferenceDataId()
        {
            var actualResult = testCandidate.GetReferenceDataId("Postal Address Type", "PAF");
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult == new Guid("4A6F8F72-AE47-4EC4-8FCB-EFCFEB900ADD"));
        }

        [Test]
        public void Test_InValidGetReferenceDataId()
        {
            var actualResult = testCandidate.GetReferenceDataId("Postal Address Type", "NYB");
            Assert.IsNotNull(actualResult);
            Assert.IsFalse(actualResult == new Guid("4A6F8F72-AE47-4EC4-8FCB-EFCFEB900ADD"));
        }

        protected override void OnSetup()
        {
            var referenceDataCategory = new List<ReferenceDataCategory>()
            {
                new ReferenceDataCategory()
                {
                    ID = Guid.NewGuid(),
                    CategoryName = "Postal Address Type",
                    ReferenceDatas = new List<ReferenceData>()
                   {
                        new ReferenceData()
                        {
                            ReferenceDataName = "PAF",
                            DataDescription = "PAF",
                            ReferenceDataCategory_GUID = new Guid("4A6F8F72-AE47-4EC4-8FCB-EFCFEB900ADD"),
                            ID = new Guid("4A6F8F72-AE47-4EC4-8FCB-EFCFEB900ADD")
                        }
                    }
                                    }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<ReferenceDataCategory>(referenceDataCategory);
            var mockReferenceDataCategoryDBSet = MockDbSet(referenceDataCategory);

            mockReferenceDataCategoryDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockReferenceDataCategoryDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockReferenceDataCategoryDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockReferenceDataCategoryDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<ReferenceDataCategory>)mockAsynEnumerable).GetAsyncEnumerator());

            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<ReferenceDataCategory>()).Returns(mockReferenceDataCategoryDBSet.Object);
            mockRMDBContext.Setup(x => x.ReferenceDataCategories).Returns(mockReferenceDataCategoryDBSet.Object);
            mockRMDBContext.Setup(c => c.ReferenceDataCategories.AsNoTracking()).Returns(mockReferenceDataCategoryDBSet.Object);
            mockReferenceDataCategoryDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockReferenceDataCategoryDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new ReferenceDataCategoryDataService(mockDatabaseFactory.Object);
        }
    }
}