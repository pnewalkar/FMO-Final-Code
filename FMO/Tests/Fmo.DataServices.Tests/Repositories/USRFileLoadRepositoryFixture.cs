using System;
using Fmo.Common.TestSupport;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Moq;
using NUnit.Framework;

namespace Fmo.DataServices.Tests.Repositories
{
    [TestFixture]
    public class USRFileLoadRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;

        protected override void OnSetup()
        {
            throw new NotImplementedException();
        }
    }
}