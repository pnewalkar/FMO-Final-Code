using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.Operational.RouteLog.WebAPI.BusinessService;
using RM.Operational.RouteLog.WebAPI.IntegrationService;

namespace RM.Operational.RouteLog.WebAPI.Test
{
    [TestFixture]
    public class RouteLogBusinessServiceFixture : TestFixtureBase
    {
        IRouteLogBusinessService testCandidate;
        Mock<IRouteLogIntegrationService> mockRouteLogIntegrationService;
        Mock<IConfigurationHelper> mockConfigurationHelper;
        DeliveryRouteDTO deliveryRouteDto;
        RouteLogSequencedPointsDTO address;
        RouteSummaryGroupDTO grp;

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
            mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(It.IsAny<string>())).Returns("xsltFilepath");
            testCandidate = new RouteLogBusinessService(mockRouteLogIntegrationService.Object, mockConfigurationHelper.Object);

        }
    }
}
