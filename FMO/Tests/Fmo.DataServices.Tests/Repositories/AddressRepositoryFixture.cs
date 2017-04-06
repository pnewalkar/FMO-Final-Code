using System.Collections.Generic;
using Fmo.Common.TestSupport;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;
using Moq;
using NUnit.Framework;
using System.Data.Entity;
using Fmo.DTO;
using Fmo.Common.Interface;

namespace Fmo.DataServices.Tests.Repositories
{
    [TestFixture]
    public class AddressRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IAddressRepository testCandidate;

        [Test]
        public void Test_UpdateAddressValidTestCase()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDTO objstPostalAddressDTO = new PostalAddressDTO() { Address_Id = 28, UDPRN = 14856 };
            var result = testCandidate.SaveAddress(objstPostalAddressDTO);
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_CreateAddressValidTestCase()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDTO objstPostalAddressDTO = new PostalAddressDTO() { Address_Id = 326, UDPRN = 15862 };
            var result = testCandidate.SaveAddress(objstPostalAddressDTO);
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_InValidAddress()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDTO objstPostalAddressDTO = null;
            var result = testCandidate.SaveAddress(objstPostalAddressDTO);
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Never);
            Assert.NotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_DeleteAddressWithDeliveryPoint()
        {
            SetUpdataWithDeliverypoints();
            List<int> lstUDPRNS = new List<int>() { 158623, 85963 };
            var result = testCandidate.DeleteNYBPostalAddress(lstUDPRNS, 2);
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Never);
            Assert.NotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_DeleteAddressWithoutDeliveryPoint()
        {
            SetUpdataWithOutDeliverypoints();
            List<int> lstUDPRNS = new List<int>() { 158623, 85963 };
            var result = testCandidate.DeleteNYBPostalAddress(lstUDPRNS, 2);
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_DeleteAddressWithoutMatchingUDPRN()
        {
            SetUpdataWithOutDeliverypoints();
            List<int> lstUDPRNS = new List<int>() { 14856 };
            var result = testCandidate.DeleteNYBPostalAddress(lstUDPRNS, 2);
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
                    Address_Id = 10,
                    AddressType_Id=2,
                    UDPRN = 14856,
                    DeliveryPoints = new List<DeliveryPoint>()
               {
                new DeliveryPoint() { DeliveryPoint_Id = 1, Address_Id = 10, UDPRN = 14856 },
                new DeliveryPoint() { DeliveryPoint_Id = 2, Address_Id = 10, UDPRN = 14856 }
            }
                }
            };

            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>()));
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            testCandidate = new AddressRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }

        private void SetUpdataWithOutDeliverypoints()
        {
            List<PostalAddress> lstPostalAddress = new List<PostalAddress>()
            {
                new PostalAddress()
                {
                    Address_Id = 10,
                    AddressType_Id=2,
                    UDPRN = 14856,
                }
            };

            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>()));
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            testCandidate = new AddressRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}