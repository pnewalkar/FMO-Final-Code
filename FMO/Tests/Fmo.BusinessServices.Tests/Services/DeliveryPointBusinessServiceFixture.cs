using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Fmo.BusinessServices.Tests.Services
{
    [TestFixture]
    public class DeliveryPointBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IDeliveryPointsRepository> mockDeliveryPointsRepository;
        private IDeliveryPointBusinessService testCandidate;

        protected override void OnSetup()
        {

            List<DeliveryPointDTO> lstDeliveryPointDTO = new List<DeliveryPointDTO>();
           
            mockDeliveryPointsRepository = CreateMock<IDeliveryPointsRepository>();
            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsRepository.Object);
            mockDeliveryPointsRepository.Setup(n=>n.GetDeliveryPoints1(It.IsAny<string>())).Returns
        }


    }
}