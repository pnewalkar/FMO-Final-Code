using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.Common.Interface;
using Fmo.Common.TestSupport;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Moq;
using NUnit.Framework;

namespace Fmo.DataServices.Tests.Repositories
{
    public class AddressRepositoryPAFFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFMODBContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private Mock<ILoggingHelper> mockloggingHelper;
        private Mock<IFileProcessingLogRepository> mockfileProcessingLogRepository;
        private Mock<IPostCodeRepository> mockpostCodeRepository;
        private IAddressRepository testCandidate;
        private PostalAddressDTO testObject;

        #region Test Methods For PAF

        [Test]
        public void Test_InsertAddress()
        {
            testObject = new PostalAddressDTO()
            {
                Address_Id = 10,
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                UDPRN = 14856
            };
            var result = testCandidate.InsertAddress(testObject, "PAFTestFile.csv");
            mockFMODBContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_GetPostalAddressByUDPRN()
        {
            testObject = new PostalAddressDTO()
            {
                Address_Id = 10,
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                UDPRN = 14856
            };
            var result = testCandidate.GetPostalAddress(testObject.UDPRN);
            Assert.IsNull(result);
        }

        [Test]
        public void Test_GetPostalAddressByAddress()
        {
        }

        [Test]
        public void Test_LogFileException()
        {
        }

        #endregion

        protected override void OnSetup()
        {
            List<PostalAddress> lstPostalAddress = new List<PostalAddress>()
                        {
                            new PostalAddress
                            {
                                BuildingName = "Bldg 1",
                                BuildingNumber = 23,
                                Postcode = "123"
                        }
                    };
            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);

            mockFMODBContext = CreateMock<FMODBContext>();
            mockFMODBContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockFMODBContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFMODBContext.Object);

            mockloggingHelper = CreateMock<ILoggingHelper>();

            mockfileProcessingLogRepository = CreateMock<IFileProcessingLogRepository>();

            mockpostCodeRepository = CreateMock<IPostCodeRepository>();
            mockpostCodeRepository.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Guid.NewGuid);

            testCandidate = new AddressRepository(mockDatabaseFactory.Object, mockloggingHelper.Object, mockfileProcessingLogRepository.Object, mockpostCodeRepository.Object);
        }
    }
}
