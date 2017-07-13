using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using RM.CommonLibrary.HelperMiddleware;
using RM.Common.ActionManager.WebAPI.DataDTO;
using RM.Common.ActionManager.WebAPI.Entity;
using Moq;
using RM.Common.ActionManager.WebAPI.Interfaces;
using RM.Common.ActionManager.WebAPI.DataService;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.Common.ActionManager.WebAPI.Test.DataService
{
    [TestFixture]
    public class ActionManagerDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<ActionDBContext> mockRMDBContext;
      //  private Mock<IDatabaseFactory<ActionDBContext>> mockDatabaseFactory;
        private IActionManagerDataService testCandidate;
        private Mock<ILoggingHelper> mockLoggingHelper;
        protected override void OnSetup()
        {

        }
    }
}
