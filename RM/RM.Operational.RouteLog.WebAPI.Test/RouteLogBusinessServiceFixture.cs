﻿using System;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Operational.RouteLog.WebAPI.BusinessService;
using RM.Operational.RouteLog.WebAPI.DTO;
using RM.Operational.RouteLog.WebAPI.IntegrationService;

namespace RM.Operational.RouteLog.WebAPI.Test
{
    [TestFixture]
    public class RouteLogBusinessServiceFixture : TestFixtureBase
    {
        private IRouteLogBusinessService testCandidate;
        private Mock<IRouteLogIntegrationService> mockRouteLogIntegrationService;
        private Mock<IConfigurationHelper> mockConfigurationHelper;
        private RouteDTO deliveryRouteDto;
        private Mock<ILoggingHelper> loggingHelperMock;

        [Test]
        public void Test_ValidPostalAddressData()
        {
            var result = testCandidate.GenerateRouteLog(deliveryRouteDto);
            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {
            mockRouteLogIntegrationService = CreateMock<IRouteLogIntegrationService>();
            mockConfigurationHelper = CreateMock<IConfigurationHelper>();
            deliveryRouteDto = new RouteDTO() { };
            loggingHelperMock = CreateMock<ILoggingHelper>();
            RouteLogSummaryDTO routeLogSummaryModelDTO = new RouteLogSummaryDTO() { };

            mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(It.IsAny<string>())).Returns("xsltFilepath");

            mockRouteLogIntegrationService.Setup(x => x.GetRouteLog(It.IsAny<RouteDTO>())).ReturnsAsync(routeLogSummaryModelDTO);
            mockRouteLogIntegrationService.Setup(x => x.GeneratePdfDocument(It.IsAny<string>())).ReturnsAsync("<note><body>abc</body></note>");

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new RouteLogBusinessService(mockRouteLogIntegrationService.Object, loggingHelperMock.Object);
        }
    }
}