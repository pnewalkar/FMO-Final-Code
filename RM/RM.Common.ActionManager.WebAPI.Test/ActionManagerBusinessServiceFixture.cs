using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RM.Common.ActionManager.WebAPI.BusinessService;
using RM.Common.ActionManager.WebAPI.BusinessService.Interface;
using RM.Common.ActionManager.WebAPI.DataDTO;
using RM.Common.ActionManager.WebAPI.Interfaces;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.Common.ActionManager.WebAPI.Test
{
    /// <summary>
    /// This class contains test methods for ActionManagerBusinessService
    /// </summary>
    [TestFixture]
    public class ActionManagerBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IActionManagerDataService> mockActionManagerDataService;
        private IActionManagerBusinessService testCandidate;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private List<UserUnitInfoDataDTO> userUnitInfoDataDTOList;

        /// <summary>
        /// test scenario to get all available actions for current user
        /// </summary>
        [Test]
        public void Test_GetRoleBasedAccessFunctions()
        {
            var result = testCandidate.GetRoleBasedAccessFunctions(userUnitInfoDataDTOList[0]);
            Assert.IsNotNull(result.Result);
        }

        /// <summary>
        /// test scenario for first time application page load for any user
        /// </summary>
        [Test]
        public void Test_GetUserUnitInfo_FirstLogin_With_EmptyLocationId()
        {
            var result = testCandidate.GetUserUnitInfo("user1", Guid.Empty);
            Assert.IsNotNull(result.Result);
        }

        /// <summary>
        /// test scenario for subsequent requests for application page load for any user
        /// </summary>
        [Test]
        public void Test_GetUserUnitInfo_Not_FirstLogin_With_LocationId()
        {
            var result = testCandidate.GetUserUnitInfo("user1", Guid.NewGuid());
            Assert.IsNotNull(result.Result);
        }

        /// <summary>
        /// test scenario for first time application page load for user having access above mail center
        /// </summary>
        [Test]
        public void Test_GetUserUnitInfoFromReferenceData_FirstLogin_With_EmptyLocationId()
        {
            var result = testCandidate.GetUserUnitInfoFromReferenceData("user1", Guid.Empty);
            Assert.IsNotNull(result.Result);
        }

        /// <summary>
        /// test scenario for subsequent requests for application page load for  user having access above mail center
        /// </summary>
        [Test]
        public void Test_GetUserUnitInfoFromReferenceData_Not_FirstLogin_With_LocationId()
        {
            var result = testCandidate.GetUserUnitInfoFromReferenceData("user1", Guid.NewGuid());
            Assert.IsNotNull(result.Result);
        }

        protected override void OnSetup()
        {
            // Data Setup
            userUnitInfoDataDTOList = new List<UserUnitInfoDataDTO>()
            {
                new UserUnitInfoDataDTO()
                {
                    UserName = "user1",
                    LocationId = new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A")
                },

                new UserUnitInfoDataDTO()
                {
                    UserName = "user2",
                    LocationId = new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A")
                }
            };

            //Creating mock objects
            mockActionManagerDataService = CreateMock<IActionManagerDataService>();
            mockLoggingHelper = CreateMock<ILoggingHelper>();

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            //Methods Setup
            mockActionManagerDataService.Setup(x => x.GetRoleBasedAccessFunctions(It.IsAny<UserUnitInfoDataDTO>())).ReturnsAsync(new List<RoleAccessDataDTO>() { });
            mockActionManagerDataService.Setup(x => x.GetUserUnitInfo(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new UserUnitInfoDataDTO() { });
            mockActionManagerDataService.Setup(x => x.GetUserUnitInfoFromReferenceData(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new UserUnitInfoDataDTO() { });

            testCandidate = new ActionManagerBusinessService(mockActionManagerDataService.Object, mockLoggingHelper.Object);
        }
    }
}