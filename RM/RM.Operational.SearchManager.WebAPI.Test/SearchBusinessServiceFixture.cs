using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.SearchManager.WebAPI.DTO;
using RM.Operational.SearchManager.WebAPI.BusinessService;
using RM.Operational.SearchManager.WebAPI.Integration;

namespace RM.Operational.SearchManager.WebAPI.Test
{
    [TestFixture]
    public class SearchBusinessServiceFixture : TestFixtureBase
    {
        private SearchBusinessService testCandidate;
        private Mock<ISearchIntegrationService> searchIntegrationServiceMock;
        private Mock<ILoggingHelper> loggingHelperMock;
        private string input = "road";
        private Guid unitGuid = new Guid("97FE320A-AFEE-4E68-980D-3A70F418E46D");
        private Guid unitLocationTypeId = new Guid("97FE320A-AFEE-4E68-980D-3A70F418E46D");
        private string unitType = "Delivery Office";
        private PostalAddressDTO postalAddressDTO;

        [Test]
        public void Test_Fetch_Advance_Search_Details()
        {
            Exception mockException = It.IsAny<Exception>();
            var output = testCandidate.GetAdvanceSearchDetails(input, unitGuid, unitType);
            Assert.NotNull(output);
        }

        [Test]
        public void Test_Fetch_Basic_Search_Details()
        {
            Exception mockException = It.IsAny<Exception>();
            var output = testCandidate.GetBasicSearchDetails(input, unitGuid, unitType);
            Assert.NotNull(output);
        }

        [Test]
        public async Task AdvanceSearch_NoResultFound()
        {
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<RouteDTO>() { new RouteDTO { RouteName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<PostCodeDTO>() { new PostCodeDTO { PostcodeUnit = "001" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchStreetNamesForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<StreetNameDTO>() { new StreetNameDTO { LocalName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryPointsForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryPointDTO>() { new DeliveryPointDTO { LocationProvider = "Route 1", PostalAddress = postalAddressDTO } }));

            var output = await testCandidate.GetAdvanceSearchDetails("xyz", unitGuid, unitType);
            Assert.NotNull(output);
        }

        [Test]
        public async Task AdvanceSearch_MoreThanOneResultFound()
        {
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<RouteDTO>() { new RouteDTO { RouteName = "dumyRouteName1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<PostCodeDTO>()
            {
                new PostCodeDTO { InwardCode = "dummyInwardCode1" },
                new PostCodeDTO { InwardCode = "dummyInwardCode2" }
            }));
            searchIntegrationServiceMock.Setup(x => x.FetchStreetNamesForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<StreetNameDTO>()
            {
                new StreetNameDTO { LocalName = "dummyLocalName1" },
                new StreetNameDTO { LocalName = "dummyLocalName2" }
            }));
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryPointsForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryPointDTO>()
            {
                new DeliveryPointDTO { UDPRN = 123456789, PostalAddress = new PostalAddressDTO() },
                new DeliveryPointDTO { UDPRN = 23456789, PostalAddress = new PostalAddressDTO() }
            }));

            var output = await testCandidate.GetAdvanceSearchDetails("test", unitGuid, unitType);
            Assert.IsTrue(output.SearchResultItems.Count == 7);
        }

        [Test]
        public async Task AdvanceSearch_OneResultThrowsError()
        {
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<RouteDTO>() { new RouteDTO { RouteName = "dumyRouteName" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<PostCodeDTO>() { new PostCodeDTO { InwardCode = "dummyInwardCode" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchStreetNamesForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<StreetNameDTO>() { new StreetNameDTO { LocalName = "dummyLocalName" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryPointsForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryPointDTO>() { new DeliveryPointDTO { UDPRN = 123456789, PostalAddress = new PostalAddressDTO() { BuildingName = "Bldg-001" } } }));

            var output = new SearchResultDTO();
            var expectedException = new Exception("Expected exception");
            try
            {
                output = await testCandidate.GetAdvanceSearchDetails("test", unitGuid, unitType);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex, expectedException);
                Assert.IsTrue(output.SearchResultItems.Count == 0);
            }
        }

        [Test]
        public async Task AdvanceSearch_ExactOneResultFound()
        {
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<RouteDTO>() { new RouteDTO { RouteName = "dumyRouteName" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<PostCodeDTO>() { new PostCodeDTO { InwardCode = "dummyInwardCode" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchStreetNamesForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<StreetNameDTO>() { new StreetNameDTO { LocalName = "dummyLocalName" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryPointsForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryPointDTO>() { new DeliveryPointDTO { UDPRN = 123456789, PostalAddress = new PostalAddressDTO() { BuildingName = "Bldg-001" } } }));

            var output = await testCandidate.GetAdvanceSearchDetails("test", unitGuid, unitType);

            Assert.IsTrue(output.SearchResultItems.Count == 4);
        }

        [Test]
        public async Task BasicSearch_NoResultFound()
        {
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryRouteForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<RouteDTO>() { new RouteDTO { RouteName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchPostCodeUnitForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<PostCodeDTO>() { new PostCodeDTO { PostcodeUnit = "001" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchStreetNamesForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<StreetNameDTO>() { new StreetNameDTO { LocalName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryPointsForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryPointDTO>() { new DeliveryPointDTO { LocationProvider = "Route 1", PostalAddress = postalAddressDTO } }));

            var output = await testCandidate.GetBasicSearchDetails(input, unitGuid, unitType);
            Assert.NotNull(output);
        }

        [Test]
        public async Task BasicSearch_ExactOneResultFound()
        {
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryRouteForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<RouteDTO>() { new RouteDTO { RouteName = "dumyRouteName" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchPostCodeUnitForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<PostCodeDTO>() { new PostCodeDTO { InwardCode = "dummyInwardCode" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchStreetNamesForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<StreetNameDTO>() { new StreetNameDTO { LocalName = "dummyLocalName" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryPointsForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryPointDTO>() { new DeliveryPointDTO { UDPRN = 123456789, PostalAddress = new PostalAddressDTO() { BuildingName = "Bldg-001" } } }));

            var output = await testCandidate.GetBasicSearchDetails("test", unitGuid, unitType);

            Assert.IsTrue(output.SearchResultItems.Count == 4);
        }

        [Test]
        public async Task BasicSearch_MoreThanOneResultFound()
        {
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryRouteForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<RouteDTO>() { new RouteDTO { RouteName = "dumyRouteName1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchPostCodeUnitForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<PostCodeDTO>()
            {
                new PostCodeDTO { InwardCode = "dummyInwardCode1" },
                new PostCodeDTO { InwardCode = "dummyInwardCode2" }
            }));
            searchIntegrationServiceMock.Setup(x => x.FetchStreetNamesForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<StreetNameDTO>()
            {
                new StreetNameDTO { LocalName = "dummyLocalName1" },
                new StreetNameDTO { LocalName = "dummyLocalName2" }
            }));
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryPointsForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryPointDTO>()
            {
                new DeliveryPointDTO { UDPRN = 123456789, PostalAddress = new PostalAddressDTO() },
                new DeliveryPointDTO { UDPRN = 23456789, PostalAddress = new PostalAddressDTO() }
            }));

            var output = await testCandidate.GetBasicSearchDetails("test", unitGuid, unitType);

            Assert.IsTrue(output.SearchResultItems.Count == 7);
        }

        [Test]
        public async Task BasicSearch_OneResultThrowsError()
        {
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryRouteForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<RouteDTO>() { new RouteDTO { RouteName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchPostCodeUnitForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<PostCodeDTO>() { new PostCodeDTO { PostcodeUnit = "001" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchStreetNamesForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<StreetNameDTO>() { new StreetNameDTO { LocalName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryPointsForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryPointDTO>() { new DeliveryPointDTO { LocationProvider = "Route 1", PostalAddress = postalAddressDTO } }));

            var output = new SearchResultDTO();
            var expectedException = new Exception("Expected exception");
            try
            {
                output = await testCandidate.GetBasicSearchDetails("test", unitGuid, unitType);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex, expectedException);

                Assert.IsTrue(output.SearchResultItems.Count == 0);
            }
        }

        protected override void OnSetup()
        {
            searchIntegrationServiceMock = new Mock<ISearchIntegrationService>();
            loggingHelperMock = new Mock<ILoggingHelper>();

            postalAddressDTO = new PostalAddressDTO()
            {
                BuildingName = "abc"
            };

            List<ReferenceDataCategoryDTO> referenceDataCategoryDTOList = new List<ReferenceDataCategoryDTO>()
            {
                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.OperationalObjectType,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.OperationalObjectTypeDP,
                            ID = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F")
                        },
                         new ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.DeliveryUnit,
                            ID = unitLocationTypeId
                        }
                    }
                }
            };

            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<RouteDTO>() { new RouteDTO { RouteName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<PostCodeDTO>() { new PostCodeDTO { PostcodeUnit = "001" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchStreetNamesForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<StreetNameDTO>() { new StreetNameDTO { LocalName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryPointsForAdvanceSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryPointDTO>() { new DeliveryPointDTO { LocationProvider = "Route 1", PostalAddress = postalAddressDTO } }));

            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryRouteForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<RouteDTO>() { new RouteDTO { RouteName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchPostCodeUnitForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<PostCodeDTO>() { new PostCodeDTO { PostcodeUnit = "001" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchStreetNamesForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<StreetNameDTO>() { new StreetNameDTO { LocalName = "Route 1" } }));
            searchIntegrationServiceMock.Setup(x => x.FetchDeliveryPointsForBasicSearch(It.IsAny<string>())).Returns(Task.FromResult(new List<DeliveryPointDTO>() { new DeliveryPointDTO { LocationProvider = "Route 1", PostalAddress = postalAddressDTO } }));

            searchIntegrationServiceMock.Setup(x => x.GetDeliveryRouteCount(It.IsAny<string>())).Returns(Task.FromResult(1));
            searchIntegrationServiceMock.Setup(x => x.GetPostCodeUnitCount(It.IsAny<string>())).Returns(Task.FromResult(1));
            searchIntegrationServiceMock.Setup(x => x.GetStreetNameCount(It.IsAny<string>())).Returns(Task.FromResult(1));
            searchIntegrationServiceMock.Setup(x => x.GetDeliveryPointsCount(It.IsAny<string>())).Returns(Task.FromResult(1));

            searchIntegrationServiceMock.Setup(n => n.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).Returns(Task.FromResult(referenceDataCategoryDTOList));
            searchIntegrationServiceMock.Setup(n => n.GetUnitLocationTypeId(It.IsAny<Guid>())).Returns(Task.FromResult(unitLocationTypeId));

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new SearchBusinessService(searchIntegrationServiceMock.Object, loggingHelperMock.Object);
        }
    }
}