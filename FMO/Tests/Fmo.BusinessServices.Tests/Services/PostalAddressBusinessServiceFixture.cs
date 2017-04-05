using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.MessageBrokerCore.Messaging;
using Fmo.NYBLoader;
using Fmo.NYBLoader.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.BusinessServices.Tests.Services
{
    [TestFixture]
    public class PostalAddressBusinessServiceFixture: TestFixtureBase
    {
        /*private Mock<IAddressRepository> mockAddressRepository;
        private Mock<IReferenceDataCategoryRepository> mockReferenceDataCategoryRepository;
        private Mock<IDeliveryPointsRepository> mockDeliveryPointsRepository;
        private Mock<IAddressLocationRepository> mockAddressLocationRepository;
        private IPostalAddressBusinessService testCandidate;*/

        private IPAFLoader testCandidate;
        private IMessageBroker<PostalAddressDTO> msgBroaker;

        [Test]
        public void Test_LoadPAFDetailsFromCSV_Positive()
        {
            string strPath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.zip");

            // testCandidate.LoadPAFDetailsFromCSV(strPath);
        }

        protected override void OnSetup()
        {
            msgBroaker = new MessageBroker<PostalAddressDTO>();
            testCandidate = new PAFLoader(msgBroaker);

            /*mockAddressRepository = CreateMock<IAddressRepository>();
            mockReferenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockDeliveryPointsRepository = CreateMock<IDeliveryPointsRepository>();
            mockAddressLocationRepository = CreateMock<IAddressLocationRepository>();

            testCandidate = new PostalAddressBusinessService(
                mockAddressRepository.Object,
                mockReferenceDataCategoryRepository.Object,
                mockDeliveryPointsRepository.Object,
                mockAddressLocationRepository.Object
                );*/
        } 
    }
}
