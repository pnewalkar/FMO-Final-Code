using System;
using Fmo.Common.TestSupport;
using Fmo.Entities;
using System.Collections.Generic;
using Fmo.DataServices.DBContext;
using Moq;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DataServices.Repositories;

namespace Fmo.DataServices.Tests.Repositories
{
    public class ReferenceDataCategoryRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IReferenceDataCategoryRepository testCandidate;

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
