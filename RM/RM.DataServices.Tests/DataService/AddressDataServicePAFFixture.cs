using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.DataServices.Tests.DataService
{
    public class AddressDataServicePAFFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private Mock<ILoggingHelper> mockloggingHelper;
        private Mock<IFileProcessingLogDataService> mockfileProcessingLogDataService;
        private Mock<IPostCodeDataService> mockpostCodeDataService;
        private Mock<IReferenceDataCategoryDataService> mockReferenceDataCategoryDataService;
        private IPostalAddressDataService testCandidate;
        private PostalAddressDTO testObject;

        #region Test Methods For PAF

        [Test]
        public void Test_InsertAddress()
        {
            testObject = new PostalAddressDTO()
            {
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                UDPRN = 14856
            };
            var result = testCandidate.InsertAddress(testObject, "PAFTestFile.csv");
            mockRMDBContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.IsNotNull(result);
            // Assert.IsTrue(result);
        }

        [Test]
        public void Test_GetPostalAddressByUDPRN()
        {
            testObject = new PostalAddressDTO()
            {
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                UDPRN = 14856
            };
            var result = testCandidate.GetPostalAddress(testObject.UDPRN);
            Assert.IsNull(result);
        }

        #endregion Test Methods For PAF

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

            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockRMDBContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockReferenceDataCategoryDataService = CreateMock<IReferenceDataCategoryDataService>();
            mockloggingHelper = CreateMock<ILoggingHelper>();
            //  mockloggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));

            mockfileProcessingLogDataService = CreateMock<IFileProcessingLogDataService>();

            mockpostCodeDataService = CreateMock<IPostCodeDataService>();
            mockpostCodeDataService.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockloggingHelper.Object, mockfileProcessingLogDataService.Object);
        }
    }
}