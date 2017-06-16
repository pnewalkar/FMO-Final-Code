using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;
using RM.Common.DataService.Interface;
using RM.Common.ReferenceData.WebAPI.BusinessService;
using RM.Common.ReferenceData.WebAPI.BusinessService.Interface;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.Common.ReferenceData.WebAPI.Test
{
    [TestFixture]
    public class ReferenceDataBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IFileProvider> mockFileProvider;
        private Mock<IReferenceDataDataService> mockReferenceDataDataService;
        private Mock<IConfigurationHelper> mockConfigurationHelper;
        private IReferenceDataBusinessService testCandidate;

        [Test]
        public void Test_GetReferenceDataByNameValuePairs()
        {
            var result = testCandidate.GetReferenceDataByNameValuePairs("", ""); 
            Assert.IsNull(result);
        }

        protected override void OnSetup()
        {
            mockFileProvider = CreateMock<IFileProvider>();
            mockReferenceDataDataService = CreateMock<IReferenceDataDataService>();
            mockConfigurationHelper = CreateMock<IConfigurationHelper>();

            mockFileProvider.Setup(x => x.GetDirectoryContents(It.IsAny<string>())).Returns(It.IsAny<IDirectoryContents>());
            mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(It.IsAny<string>())).Returns("c:\\");
            mockFileProvider.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(It.IsAny<IFileInfo>());

            testCandidate = new ReferenceDataBusinessService(mockFileProvider.Object, mockReferenceDataDataService.Object, mockConfigurationHelper.Object);
        }
    }
}