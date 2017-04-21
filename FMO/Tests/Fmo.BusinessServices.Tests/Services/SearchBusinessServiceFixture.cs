using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.Enums;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace Fmo.BusinessServices.Tests.Services
{
    [TestFixture]
    public class SearchBusinessServiceFixture : TestFixtureBase
    {
        private ISearchBusinessService testCandidate;
        private Mock<IDeliveryRouteRepository> deliveryRouteRepositoryMock;
        private Mock<IPostCodeRepository> postCodeRepositoryMock;
        private Mock<IDeliveryPointsRepository> deliveryPointsRepositoryMock;
        private Mock<IStreetNetworkRepository> streetNetworkRepositoryMock;
        private Guid unitGuid = System.Guid.NewGuid();

        [Test]
        public async Task AdvanceSearch_NoResultFound([Values("", null)] string input)
        {
            postCodeRepositoryMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.PostCodeDTO>()));
            deliveryRouteRepositoryMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.DeliveryRouteDTO>()));
            deliveryPointsRepositoryMock.Setup(x => x.FetchDeliveryPointsForAdvanceSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.DeliveryPointDTO>()));
            streetNetworkRepositoryMock.Setup(x => x.FetchStreetNamesForAdvanceSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.StreetNameDTO>()));

            var output = await testCandidate.FetchAdvanceSearchDetails(input, unitGuid);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Postcode));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.Postcode).Count == 0);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.DeliveryPoint));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.DeliveryPoint).Count == 0);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Route));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.Route).Count == 0);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.StreetNetwork));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.StreetNetwork).Count == 0);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.All));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.All).Count == 0);

            Assert.IsTrue(output.SearchResultItems.Count == 0);
        }

        [Test]
        public async Task AdvanceSearch_ExactOneResultFound([Values("Test")] string input)
        {
            postCodeRepositoryMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.PostCodeDTO> { new DTO.PostCodeDTO { InwardCode = "dummyInwardCode" } }));
            deliveryRouteRepositoryMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.DeliveryRouteDTO> { new DTO.DeliveryRouteDTO { RouteName = "dumyRouteName" } }));
            deliveryPointsRepositoryMock.Setup(x => x.FetchDeliveryPointsForAdvanceSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.DeliveryPointDTO> { new DTO.DeliveryPointDTO { UDPRN = 123456789 } }));
            streetNetworkRepositoryMock.Setup(x => x.FetchStreetNamesForAdvanceSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.StreetNameDTO> { new DTO.StreetNameDTO { LocalName = "dummyLocalName" } }));

            var output = await testCandidate.FetchAdvanceSearchDetails(input, unitGuid);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Postcode));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.Postcode).Count == 1);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.DeliveryPoint));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.DeliveryPoint).Count == 1);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Route));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.Route).Count == 1);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.StreetNetwork));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.StreetNetwork).Count == 1);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.All));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.All).Count == 4);

            Assert.IsTrue(output.SearchResultItems.Count == 4);
        }

        [Test]
        public async Task AdvanceSearch_MoreThanOneResultFound([Values("Test")] string input)
        {
            postCodeRepositoryMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.PostCodeDTO>
                {
                    new DTO.PostCodeDTO { InwardCode = "dummyInwardCode1" },
                    new DTO.PostCodeDTO { InwardCode = "dummyInwardCode2" }
                }));
            deliveryRouteRepositoryMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.DeliveryRouteDTO> { new DTO.DeliveryRouteDTO { RouteName = "dumyRouteName1" } }));
            deliveryPointsRepositoryMock.Setup(x => x.FetchDeliveryPointsForAdvanceSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.DeliveryPointDTO>
                {
                    new DTO.DeliveryPointDTO { UDPRN = 123456789 },
                    new DTO.DeliveryPointDTO { UDPRN = 23456789 }
                }));
            streetNetworkRepositoryMock.Setup(x => x.FetchStreetNamesForAdvanceSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.StreetNameDTO>
                {
                    new DTO.StreetNameDTO { LocalName = "dummyLocalName1" },
                    new DTO.StreetNameDTO { LocalName = "dummyLocalName2" }
                }));

            var output = await testCandidate.FetchAdvanceSearchDetails(input, unitGuid);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Postcode));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.Postcode).Count == 2);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.DeliveryPoint));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.DeliveryPoint).Count == 2);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Route));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.Route).Count == 1);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.StreetNetwork));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.StreetNetwork).Count == 2);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.All));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.All).Count == 7);

            Assert.IsTrue(output.SearchResultItems.Count == 7);
        }

        [Test]
        public async Task AdvanceSearch_OneResultThrowsError([Values("Test")] string input)
        {
            var output = new DTO.SearchResultDTO();
            var expectedException = new Exception("Expected exception");
            try
            {
                postCodeRepositoryMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(input, unitGuid)).Throws(expectedException);
                deliveryRouteRepositoryMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.DeliveryRouteDTO> { new DTO.DeliveryRouteDTO { RouteName = "dumyRouteName" } }));
                deliveryPointsRepositoryMock.Setup(x => x.FetchDeliveryPointsForAdvanceSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.DeliveryPointDTO> { new DTO.DeliveryPointDTO { UDPRN = 123456789 } }));
                streetNetworkRepositoryMock.Setup(x => x.FetchStreetNamesForAdvanceSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.StreetNameDTO> { new DTO.StreetNameDTO { LocalName = "dummyLocalName" } }));

                output = await testCandidate.FetchAdvanceSearchDetails(input, unitGuid);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex, expectedException);

                Assert.IsTrue(!output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Postcode));

                Assert.IsTrue(!output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.DeliveryPoint));

                Assert.IsTrue(!output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Route));

                Assert.IsTrue(!output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.StreetNetwork));

                Assert.IsTrue(!output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.All));

                Assert.IsTrue(output.SearchResultItems.Count == 0);
            }
        }

        [Test]
        public async Task BasicSearch_NoResultFound([Values("", null)] string input)
        {
            postCodeRepositoryMock.Setup(x => x.FetchPostCodeUnitForBasicSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.PostCodeDTO>()));
            deliveryRouteRepositoryMock.Setup(x => x.FetchDeliveryRouteForBasicSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.DeliveryRouteDTO>()));
            deliveryPointsRepositoryMock.Setup(x => x.FetchDeliveryPointsForBasicSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.DeliveryPointDTO>()));
            streetNetworkRepositoryMock.Setup(x => x.FetchStreetNamesForBasicSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.StreetNameDTO>()));

            var output = await testCandidate.FetchBasicSearchDetails(input, unitGuid);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Postcode));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.Postcode).Count == 0);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.DeliveryPoint));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.DeliveryPoint).Count == 0);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Route));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.Route).Count == 0);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.StreetNetwork));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.StreetNetwork).Count == 0);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.All));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.All).Count == 0);

            Assert.IsTrue(output.SearchResultItems.Count == 0);
        }

        [Test]
        public async Task BasicSearch_ExactOneResultFound([Values("Test")] string input)
        {
            postCodeRepositoryMock.Setup(x => x.FetchPostCodeUnitForBasicSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.PostCodeDTO> { new DTO.PostCodeDTO { InwardCode = "dummyInwardCode" } }));
            deliveryRouteRepositoryMock.Setup(x => x.FetchDeliveryRouteForBasicSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.DeliveryRouteDTO> { new DTO.DeliveryRouteDTO { RouteName = "dumyRouteName" } }));
            deliveryPointsRepositoryMock.Setup(x => x.FetchDeliveryPointsForBasicSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.DeliveryPointDTO> { new DTO.DeliveryPointDTO { UDPRN = 123456789 } }));
            streetNetworkRepositoryMock.Setup(x => x.FetchStreetNamesForBasicSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.StreetNameDTO> { new DTO.StreetNameDTO { LocalName = "dummyLocalName" } }));

            postCodeRepositoryMock.Setup(x => x.GetPostCodeUnitCount(input, unitGuid)).Returns(Task.FromResult(1));
            deliveryRouteRepositoryMock.Setup(x => x.GetDeliveryRouteCount(input, unitGuid)).Returns(Task.FromResult(1));
            deliveryPointsRepositoryMock.Setup(x => x.GetDeliveryPointsCount(input, unitGuid)).Returns(Task.FromResult(1));
            streetNetworkRepositoryMock.Setup(x => x.GetStreetNameCount(input, unitGuid)).Returns(Task.FromResult(1));

            var output = await testCandidate.FetchBasicSearchDetails(input, unitGuid);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Postcode));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.Postcode).Count == 1);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.DeliveryPoint));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.DeliveryPoint).Count == 1);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Route));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.Route).Count == 1);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.StreetNetwork));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.StreetNetwork).Count == 1);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.All));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.All).Count == 4);

            Assert.IsTrue(output.SearchResultItems.Count == 4);
        }

        [Test]
        public async Task BasicSearch_MoreThanOneResultFound([Values("Test")] string input)
        {
            postCodeRepositoryMock.Setup(x => x.FetchPostCodeUnitForBasicSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.PostCodeDTO>
                {
                    new DTO.PostCodeDTO { InwardCode = "dummyInwardCode1" },
                    new DTO.PostCodeDTO { InwardCode = "dummyInwardCode2" }
                }));
            deliveryRouteRepositoryMock.Setup(x => x.FetchDeliveryRouteForBasicSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.DeliveryRouteDTO> { new DTO.DeliveryRouteDTO { RouteName = "dumyRouteName1" } }));
            deliveryPointsRepositoryMock.Setup(x => x.FetchDeliveryPointsForBasicSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.DeliveryPointDTO>
                {
                    new DTO.DeliveryPointDTO { UDPRN = 123456789 },
                    new DTO.DeliveryPointDTO { UDPRN = 23456789 }
                }));
            streetNetworkRepositoryMock.Setup(x => x.FetchStreetNamesForBasicSearch(input, unitGuid))
                .Returns(Task.FromResult(new List<DTO.StreetNameDTO>
                {
                    new DTO.StreetNameDTO { LocalName = "dummyLocalName1" },
                    new DTO.StreetNameDTO { LocalName = "dummyLocalName2" }
                }));

            postCodeRepositoryMock.Setup(x => x.GetPostCodeUnitCount(input, unitGuid)).Returns(Task.FromResult(2));
            deliveryRouteRepositoryMock.Setup(x => x.GetDeliveryRouteCount(input, unitGuid)).Returns(Task.FromResult(1));
            deliveryPointsRepositoryMock.Setup(x => x.GetDeliveryPointsCount(input, unitGuid)).Returns(Task.FromResult(2));
            streetNetworkRepositoryMock.Setup(x => x.GetStreetNameCount(input, unitGuid)).Returns(Task.FromResult(2));

            var output = await testCandidate.FetchBasicSearchDetails(input, unitGuid);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Postcode));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.Postcode).Count == 2);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.DeliveryPoint));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.DeliveryPoint).Count == 2);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Route));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.Route).Count == 1);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.StreetNetwork));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.StreetNetwork).Count == 2);

            Assert.IsTrue(output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.All));
            Assert.IsTrue(output.SearchCounts.SingleOrDefault(x => x.Type == SearchBusinessEntityType.All).Count == 7);

            Assert.IsTrue(output.SearchResultItems.Count == 7);
        }

        [Test]
        public async Task BasicSearch_OneResultThrowsError([Values("Test")] string input)
        {
            var output = new DTO.SearchResultDTO();
            var expectedException = new Exception("Expected exception");
            try
            {
                postCodeRepositoryMock.Setup(x => x.FetchPostCodeUnitForBasicSearch(input, unitGuid)).Throws(expectedException);
                deliveryRouteRepositoryMock.Setup(x => x.FetchDeliveryRouteForBasicSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.DeliveryRouteDTO> { new DTO.DeliveryRouteDTO { RouteName = "dumyRouteName" } }));
                deliveryPointsRepositoryMock.Setup(x => x.FetchDeliveryPointsForBasicSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.DeliveryPointDTO> { new DTO.DeliveryPointDTO { UDPRN = 123456789 } }));
                streetNetworkRepositoryMock.Setup(x => x.FetchStreetNamesForBasicSearch(input, unitGuid)).Returns(Task.FromResult(new List<DTO.StreetNameDTO> { new DTO.StreetNameDTO { LocalName = "dummyLocalName" } }));

                output = await testCandidate.FetchBasicSearchDetails(input, unitGuid);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex, expectedException);

                Assert.IsTrue(!output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Postcode));

                Assert.IsTrue(!output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.DeliveryPoint));

                Assert.IsTrue(!output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.Route));

                Assert.IsTrue(!output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.StreetNetwork));

                Assert.IsTrue(!output.SearchCounts.Any(x => x.Type == SearchBusinessEntityType.All));

                Assert.IsTrue(output.SearchResultItems.Count == 0);
            }
        }

        protected override void OnSetup()
        {
            deliveryRouteRepositoryMock = CreateMock<IDeliveryRouteRepository>();
            postCodeRepositoryMock = CreateMock<IPostCodeRepository>();
            deliveryPointsRepositoryMock = CreateMock<IDeliveryPointsRepository>();
            streetNetworkRepositoryMock = CreateMock<IStreetNetworkRepository>();
            testCandidate = new SearchBussinessService(
                                         deliveryRouteRepositoryMock.Object,
                                         postCodeRepositoryMock.Object,
                                         streetNetworkRepositoryMock.Object,
                                         deliveryPointsRepositoryMock.Object);
        }
    }
}