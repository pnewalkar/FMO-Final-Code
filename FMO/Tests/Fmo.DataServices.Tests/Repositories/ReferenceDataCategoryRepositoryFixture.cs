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

        protected override void OnSetup()
        {
            var referenceDataCategory = new List<ReferenceDataCategory>()
            {
                new ReferenceDataCategory() { ReferenceDataCategory_Id = 1 }
            };

            var mockReferenceDataCategoryDBSet = MockDbSet(referenceDataCategory);

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<ReferenceDataCategory>()).Returns(mockReferenceDataCategoryDBSet.Object);
            mockFmoDbContext.Setup(x => x.ReferenceDataCategories).Returns(mockReferenceDataCategoryDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);

            testCandidate = new ReferenceDataCategoryRepository(mockDatabaseFactory.Object);
        }
    }
}
