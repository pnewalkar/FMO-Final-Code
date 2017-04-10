using System;
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

namespace Fmo.DataServices.Tests.Repositories
{
    public class RoadNameRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IRoadNameRepository testCandidate;
        private string coordinates;

        [Test]
        public void Test_Get()
        {
            coordinates = "1234.87";
            var actualResult = testCandidate.GetRoadRoutes(coordinates);
            Assert.IsNotNull(actualResult);
        }


        protected override void OnSetup()
        {
            List<OsRoadLinkDTO> lstRoadLinkDTO = new List<OsRoadLinkDTO>();
            var mocAccessLinkDtoDBSet = MockDbSet(lstRoadLinkDTO);

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new RoadNameRepository(mockDatabaseFactory.Object);
        }
    }
}
