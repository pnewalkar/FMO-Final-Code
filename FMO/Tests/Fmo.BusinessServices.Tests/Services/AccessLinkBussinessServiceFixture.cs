namespace Fmo.BusinessServices.Tests.Services
{
    using BusinessServices.Services;
    using DataServices.Repositories.Interfaces;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.Common.TestSupport;
    using Fmo.DTO;
    using Helpers.Interface;
    using Moq;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;

    public class AccessLinkBussinessServiceFixture : TestFixtureBase
    {
        private IAccessLinkBussinessService testCandidate;
        private Mock<IAccessLinkRepository> mockaccessLinkRepository;
        private Mock<ICreateOtherLayersObjects> mockCreateOtherLayers;
        private List<AccessLinkDTO> accessLinkDTO = null;
        


        [Test]
        public void Test_GetAccessLinks()
        {
            //var obj = new List<object>();
            //obj.Add(obj[0] = "123.33");
            //obj.Add(obj[1] = "133.33");
            //obj.Add(obj[2] = "143.33");
            //obj.Add(obj[3] = "153.33");
            string Coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";
            var result = testCandidate.GetAccessLinks(Coordinates);
            mockaccessLinkRepository.Verify(x => x.GetAccessLinks(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Test_GetData()
        {
            string Coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";
            string[] bboxArr = Coordinates.Split(',');
            var result = testCandidate.GetData(null, bboxArr);

            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {

            mockaccessLinkRepository = new Mock<IAccessLinkRepository>();
            mockCreateOtherLayers = new Mock<ICreateOtherLayersObjects>();
            accessLinkDTO = new List<AccessLinkDTO>() { new AccessLinkDTO() { AccessLink_Id = 1, features = "DI0001", AccessLinkType_Id = 1, type = "UnitOne", AccessLinkLine = It.IsAny<DbGeometry>() } };
            mockaccessLinkRepository.Setup(x => x.GetAccessLinks(It.IsAny<string>())).Returns(It.IsAny<List<AccessLinkDTO>>);

            testCandidate = new AccessLinkBussinessService(mockaccessLinkRepository.Object, mockCreateOtherLayers.Object);
        }
    }
}
