using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
            mockRMDBContext.Verify(n => n.SaveChangesAsync(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
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
            Assert.IsNull(result.Result);
        }

        #endregion Test Methods For PAF

        protected override void OnSetup()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            List<PostalAddress> lstPostalAddress = new List<PostalAddress>()
                        {
                            new PostalAddress
                            {
                                BuildingName = "Bldg 1",
                                BuildingNumber = 23,
                                Postcode = "123"
                        }
                    };
            var mockPostalAddressEnumerable = new DbAsyncEnumerable<PostalAddress>(lstPostalAddress);
            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);

            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostalAddressEnumerable.AsQueryable().Provider);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostalAddressEnumerable.AsQueryable().Expression);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostalAddressEnumerable.AsQueryable().ElementType);
            mockPostalAddressDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostalAddress>)mockPostalAddressEnumerable).GetAsyncEnumerator());

            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockRMDBContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockRMDBContext.Setup(c => c.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressDBSet.Object);
            mockReferenceDataCategoryDataService = CreateMock<IReferenceDataCategoryDataService>();
            mockloggingHelper = CreateMock<ILoggingHelper>();

            mockfileProcessingLogDataService = CreateMock<IFileProcessingLogDataService>();

            mockpostCodeDataService = CreateMock<IPostCodeDataService>();
            mockpostCodeDataService.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockloggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockloggingHelper.Object, mockfileProcessingLogDataService.Object);
        }
    }
}