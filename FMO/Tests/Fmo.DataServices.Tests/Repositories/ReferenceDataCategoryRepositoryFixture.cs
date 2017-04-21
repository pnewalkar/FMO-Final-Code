namespace Fmo.DataServices.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ReferenceDataCategoryRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IReferenceDataCategoryRepository testCandidate;

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
                    ReferenceDataCategory_Id = 1,
                    CategoryName = "Postal Address Type",
                    ReferenceDatas = new List<ReferenceData>()
                   {
                        new ReferenceData()
                        {
                            ReferenceDataName = "PAF",
                            DataDescription = "PAF",
                            ReferenceDataCategory_Id = 1,
                            ReferenceData_Id = 3,
                            ID = new Guid("4A6F8F72-AE47-4EC4-8FCB-EFCFEB900ADD")
                        }
                    }
                                    }
            };

            var mockReferenceDataCategoryDBSet = MockDbSet(referenceDataCategory);
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<ReferenceDataCategory>()).Returns(mockReferenceDataCategoryDBSet.Object);
            mockFmoDbContext.Setup(x => x.ReferenceDataCategories).Returns(mockReferenceDataCategoryDBSet.Object);
            mockFmoDbContext.Setup(c => c.ReferenceDataCategories.AsNoTracking()).Returns(mockReferenceDataCategoryDBSet.Object);
            mockReferenceDataCategoryDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockReferenceDataCategoryDBSet.Object);
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new ReferenceDataCategoryRepository(mockDatabaseFactory.Object);
        }
    }
}
