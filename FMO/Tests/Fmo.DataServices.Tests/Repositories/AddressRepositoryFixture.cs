using System;
using System.Collections.Generic;
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
    [TestFixture]
    public class AddressRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private Mock<IFileProcessingLogRepository> mockFileProcessingLog;
        private Mock<IPostCodeRepository> mockPostCodeRepository;
        private Mock<IReferenceDataCategoryRepository> mockReferenceDataCategoryRepository;

        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IAddressRepository testCandidate;

        [Test]
        public void Test_UpdateAddressValidTestCase()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDTO objstPostalAddressDTO = new PostalAddressDTO() { Address_Id = 28, UDPRN = 14856 };
            var result = testCandidate.SaveAddress(objstPostalAddressDTO, "NYB.CSV");
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_CreateAddressValidTestCase()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDTO objstPostalAddressDTO = new PostalAddressDTO() { Address_Id = 326, UDPRN = 15862 };
            var result = testCandidate.SaveAddress(objstPostalAddressDTO, "NYB.CSV");
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_InValidAddress()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDTO objstPostalAddressDTO = null;
            var result = testCandidate.SaveAddress(objstPostalAddressDTO, "NYB.CSV");
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Never);
            Assert.NotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_DeleteAddressWithDeliveryPoint()
        {
            SetUpdataWithDeliverypoints();
            List<int> lstUDPRNS = new List<int>() { 158623, 85963 };
            var result = testCandidate.DeleteNYBPostalAddress(lstUDPRNS, new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_DeleteAddressWithoutDeliveryPoint()
        {
            SetUpdataWithOutDeliverypoints();
            List<int> lstUDPRNS = new List<int>() { 158623, 85963 };
            var result = testCandidate.DeleteNYBPostalAddress(lstUDPRNS, new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_DeleteAddressWithoutMatchingUDPRN()
        {
            SetUpdataWithOutDeliverypoints();
            List<int> lstUDPRNS = new List<int>() { 14856 };
            var result = testCandidate.DeleteNYBPostalAddress(lstUDPRNS, new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Never);
            Assert.NotNull(result);
            Assert.IsFalse(result);
        }

        protected override void OnSetup()
        {
        }

        private void SetUpdataWithDeliverypoints()
        {
            List<PostalAddress> lstPostalAddress = new List<PostalAddress>()
            {
                new PostalAddress()
                {
                    ID= Guid.NewGuid(),
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    UDPRN = 14856,
                    DeliveryPoints = new List<DeliveryPoint>()
               {
                new DeliveryPoint() { ID = Guid.NewGuid(), Address_GUID = Guid.NewGuid(), UDPRN = 14856 },
                new DeliveryPoint() { ID = Guid.NewGuid(), Address_GUID = Guid.NewGuid(), UDPRN = 14856 }
            }
                }
            };

            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogRepository>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockPostCodeRepository = CreateMock<IPostCodeRepository>();
            mockReferenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>()));
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockPostCodeRepository.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Guid.NewGuid);
            testCandidate = new AddressRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object, mockPostCodeRepository.Object, mockReferenceDataCategoryRepository.Object);
        }

        private void SetUpdataWithOutDeliverypoints()
        {
            List<PostalAddress> lstPostalAddress = new List<PostalAddress>()
            {
                new PostalAddress()
                {
                    ID = Guid.NewGuid(),
                    UDPRN = 14856,
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15")
                }
            };

            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogRepository>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockPostCodeRepository = CreateMock<IPostCodeRepository>();
            mockReferenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>()));
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockPostCodeRepository.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Guid.NewGuid);
            testCandidate = new AddressRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object, mockPostCodeRepository.Object, mockReferenceDataCategoryRepository.Object);
        }
    }
}