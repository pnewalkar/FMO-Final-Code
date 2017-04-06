using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace Fmo.BusinessServices.Tests.Services
{
    [TestFixture]
    public class SearchBusinessServiceFixture : TestFixtureBase
    {
        private ISearchBussinessService testCandidate;
        private Mock<IDeliveryRouteRepository> deliveryRouteRepositoryMock;
        private Mock<IPostCodeRepository> postCodeRepositoryMock;
        private Mock<IPostalAddressRepository> postalAddressRepositoryMock;
        private Mock<IStreetNetworkRepository> streetNetworkRepositoryMock;

        [Test]
        public async Task AdvanceSearch_NoResultFound([Values("", null)] string input)
        {
            postCodeRepositoryMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(input)).Returns(Task.FromResult(new List<DTO.PostCodeDTO>()));
            deliveryRouteRepositoryMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(input)).Returns(Task.FromResult(new List<DTO.DeliveryRouteDTO>()));
            postalAddressRepositoryMock.Setup(x => x.FetchPostalAddressforAdvanceSearch(input)).Returns(Task.FromResult(new List<DTO.PostalAddressDTO>()));
            streetNetworkRepositoryMock.Setup(x => x.FetchStreetNamesforAdvanceSearch(input)).Returns(Task.FromResult(new List<DTO.StreetNameDTO>()));

            var output = await testCandidate.FetchAdvanceSearchDetails(input);
            Assert.IsTrue(output.DeliveryRoute.Count == 0);
            Assert.IsTrue(output.PostalAddress.Count == 0);
            Assert.IsTrue(output.PostCode.Count == 0);
            Assert.IsTrue(output.StreetName.Count == 0);
            Assert.IsTrue(output.TotalCount == 0);
        }

        [Test]
        public async Task AdvanceSearch_ResultFound([Values("Test")] string input)
        {
            postCodeRepositoryMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(input)).Returns(Task.FromResult(new List<DTO.PostCodeDTO> { new DTO.PostCodeDTO { InwardCode = "dummyInwardCode" } }));
            deliveryRouteRepositoryMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(input)).Returns(Task.FromResult(new List<DTO.DeliveryRouteDTO> { new DTO.DeliveryRouteDTO { RouteName = "dumyRouteName" } }));
            postalAddressRepositoryMock.Setup(x => x.FetchPostalAddressforAdvanceSearch(input)).Returns(Task.FromResult(new List<DTO.PostalAddressDTO> { new DTO.PostalAddressDTO { UDPRN = 123456789 } }));
            streetNetworkRepositoryMock.Setup(x => x.FetchStreetNamesforAdvanceSearch(input)).Returns(Task.FromResult(new List<DTO.StreetNameDTO> { new DTO.StreetNameDTO { LocalName = "dummyLocalName" } }));

            var output = await testCandidate.FetchAdvanceSearchDetails(input);
            Assert.IsTrue(output.DeliveryRoute.Count == 1);
            Assert.IsTrue(output.PostalAddress.Count == 1);
            Assert.IsTrue(output.PostCode.Count == 1);
            Assert.IsTrue(output.StreetName.Count == 1);
            Assert.IsTrue(output.TotalCount == 4);
        }

        //[Test]
        //public async Task AdvanceSearch_OneResultThrowsError([Values("Test")] string input)
        //{
        //    postCodeRepositoryMock.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(input)).Returns(Task.FromResult(new List<DTO.PostCodeDTO> { new DTO.PostCodeDTO { InwardCode = "dummyInwardCode" } }));
        //    deliveryRouteRepositoryMock.Setup(x => x.FetchDeliveryRouteForAdvanceSearch(input)).Returns(Task.FromResult(new List<DTO.DeliveryRouteDTO> { new DTO.DeliveryRouteDTO { RouteName = "dumyRouteName" } }));
        //    postalAddressRepositoryMock.Setup(x => x.FetchPostalAddressforAdvanceSearch(input)).Returns(Task.FromResult(new List<DTO.PostalAddressDTO> { new DTO.PostalAddressDTO { UDPRN = 123456789 } }));
        //    streetNetworkRepositoryMock.Setup(x => x.FetchStreetNamesforAdvanceSearch(input)).Returns(Task.FromResult(new List<DTO.StreetNameDTO> { new DTO.StreetNameDTO { LocalName = "dummyLocalName" } }));

        //    var output = await testCandidate.FetchAdvanceSearchDetails(input);
        //    Assert.IsTrue(output.DeliveryRoute.Count == 1);
        //    Assert.IsTrue(output.PostalAddress.Count == 0);
        //    Assert.IsTrue(output.PostCode.Count == 1);
        //    Assert.IsTrue(output.StreetName.Count == 1);
        //    Assert.IsTrue(output.TotalCount == 3);
        //}

        protected override void OnSetup()
        {
            deliveryRouteRepositoryMock = CreateMock<IDeliveryRouteRepository>();
            postCodeRepositoryMock = CreateMock<IPostCodeRepository>();
            postalAddressRepositoryMock = CreateMock<IPostalAddressRepository>();
            streetNetworkRepositoryMock = CreateMock<IStreetNetworkRepository>();
            testCandidate = new SearchBussinessService(
                                         deliveryRouteRepositoryMock.Object,
                                         postCodeRepositoryMock.Object,
                                         postalAddressRepositoryMock.Object,
                                         streetNetworkRepositoryMock.Object);
        }
    }
}