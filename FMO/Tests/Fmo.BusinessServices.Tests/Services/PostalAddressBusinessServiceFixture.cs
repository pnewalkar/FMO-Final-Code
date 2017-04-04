using Fmo.BusinessServices.Interfaces;
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
    public class PostalAddressBusinessServiceFixture
    {
        private Mock<IAddressRepository> mockAddressRepository;
        private Mock<IReferenceDataCategoryRepository> mockReferenceDataCategoryRepository;
        private Mock<IDeliveryPointsRepository> mockDeliveryPointsRepository;
        private Mock<IAddressLocationRepository> mockAddressLocationRepository;
        private IPostalAddressBusinessService testCandidate;

        [Test]
        public void SavePAFDetailsTestFixture()
        {

        }

        public void SetUp()
        {

        }
    }
}
