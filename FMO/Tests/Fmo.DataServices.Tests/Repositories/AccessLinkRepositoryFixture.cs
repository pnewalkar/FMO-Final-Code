using System;

namespace Fmo.DataServices.Tests.Repositories
{
    using System.Collections.Generic;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Moq;
    using NUnit.Framework;

    public class AccessLinkRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IAccessLinkRepository testCandidate;
        private string coordinates;
        private Guid unitGuid;

        [Test]
        public void Test_Get()
        {
            coordinates = "1234.87";
            var actualResult = testCandidate.GetAccessLinks(coordinates, unitGuid);
            Assert.IsNotNull(actualResult);
        }

        protected override void OnSetup()
        {
            List<AccessLinkDTO> lstaccessLinkDTO = new List<AccessLinkDTO>();
            var mocAccessLinkDtoDBSet = MockDbSet(lstaccessLinkDTO);

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new AccessLinkRepository(mockDatabaseFactory.Object);
        }
    }
}