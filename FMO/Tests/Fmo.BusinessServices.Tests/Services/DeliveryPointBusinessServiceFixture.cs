namespace Fmo.BusinessServices.Tests.Services
{
    using System.Collections.Generic;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DeliveryPointBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IDeliveryPointsRepository> mockDeliveryPointsRepository;
        private IDeliveryPointBusinessService testCandidate;

        protected override void OnSetup()
        {

            List<DeliveryPointDTO> lstDeliveryPointDTO = new List<DeliveryPointDTO>();
            //            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>()
            //            {
            //                new PostalAddressDTO
            //                {
            //                    BuildingName= "";
            //        }
            //    };

            //    mockDeliveryPointsRepository = CreateMock<IDeliveryPointsRepository>();
            //            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsRepository.Object);
            //    mockDeliveryPointsRepository.Setup(n=>n.GetDeliveryPoints1(It.IsAny<string>())).Returns
            //}

        }
    }
}