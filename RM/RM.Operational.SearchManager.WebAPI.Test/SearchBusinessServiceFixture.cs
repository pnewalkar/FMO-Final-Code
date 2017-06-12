using Moq;
using NUnit.Framework;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Operational.SearchManager.WebAPI.BusinessService;
using RM.Operational.SearchManager.WebAPI.Integration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RM.Operational.SearchManager.WebAPI.Test
{
    [TestFixture]
    public class SearchBusinessServiceFixture : TestFixtureBase
    {
        private SearchBusinessService testCandidate;
        private Mock<ISearchIntegrationService> searchIntegrationServiceMock;
        private Mock<ILoggingHelper> loggingHelperMock;


        [Test]
        public async Task Fetch_Advance_Search_Details([Values("Test")] string input)
        {
            input = "road";
            Exception mockException = It.IsAny<Exception>();
            var output = testCandidate.FetchAdvanceSearchDetails(input);
            Assert.NotNull(output);
        }

        [Test]
        public async Task Fetch_Basic_Search_Details([Values("Test")] string input)
        {
            input = "road";
            Exception mockException = It.IsAny<Exception>();
            var output = testCandidate.FetchBasicSearchDetails(input);

            Assert.NotNull(output);
        }
        protected override void OnSetup()
        {
            searchIntegrationServiceMock = new Mock<ISearchIntegrationService>();
            loggingHelperMock = new Mock<ILoggingHelper>();
            

            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryRouteDTO>() { new DeliveryRouteDTO {RouteName ="Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<PostCodeDTO>() { new PostCodeDTO {PostcodeUnit  = "001" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchStreetNamesForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<StreetNameDTO>() { new StreetNameDTO { LocalName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryPointsForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryPointDTO>() { new DeliveryPointDTO {  LocationProvider= "Route 1" } }));

            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryRouteForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryRouteDTO>() { new DeliveryRouteDTO { RouteName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchPostCodeUnitForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<PostCodeDTO>() { new PostCodeDTO { PostcodeUnit = "001" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchStreetNamesForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<StreetNameDTO>() { new StreetNameDTO { LocalName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryPointsForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryPointDTO>() { new DeliveryPointDTO { LocationProvider = "Route 1" } }));

            searchIntegrationServiceMock.Setup(x => x.GetDeliveryRouteCount(It.IsAny<string>())).Returns(Task.FromResult(1));
            searchIntegrationServiceMock.Setup(x => x.GetPostCodeUnitCount(It.IsAny<string>())).Returns(Task.FromResult(1));
            searchIntegrationServiceMock.Setup(x => x.GetStreetNameCount(It.IsAny<string>())).Returns(Task.FromResult(1));
            searchIntegrationServiceMock.Setup(x => x.GetDeliveryPointsCount(It.IsAny<string>())).Returns(Task.FromResult(1));

            testCandidate = new SearchBusinessService(searchIntegrationServiceMock.Object, loggingHelperMock.Object);
        }
    }
}
