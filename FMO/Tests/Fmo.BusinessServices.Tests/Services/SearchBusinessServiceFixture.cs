using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void AdvanceSearch_NoResultFound([Values("Test")] string input)
        {
            var output=testCandidate.FetchAdvanceSearchDetails(input);
           // Assert.IsTrue(output.)
        }

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
