using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;
using RM.Common.DataService.Interface;
using RM.Common.ReferenceData.WebAPI.BusinessService;
using RM.Common.ReferenceData.WebAPI.BusinessService.Interface;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using System.IO;

namespace RM.Common.ReferenceData.WebAPI.Test
{
    [TestFixture]
    public class ReferenceDataBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IFileProvider> mockFileProvider;
        private IFileProvider fileProvider;
        private Mock<IReferenceDataDataService> mockReferenceDataDataService;
        private Mock<IConfigurationHelper> mockConfigurationHelper;
        private IReferenceDataBusinessService testCandidate;
        string filepath;

        //[Test]
        //public void Test_GetReferenceDataByNameValuePairs()
        //{
        //  //  mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(filepath));
        //  //  var result = testCandidate.GetReferenceDataByNameValuePairs("", "");
        //   // Assert.IsNull(result);
        //}

        protected override void OnSetup()
        {
            mockFileProvider = CreateMock<IFileProvider>();
            mockReferenceDataDataService = CreateMock<IReferenceDataDataService>();
            mockConfigurationHelper = CreateMock<IConfigurationHelper>();
            filepath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug\net452", string.Empty), @"TestData\");
            fileProvider = new PhysicalFileProvider(filepath);
            IFileInfo fileInfo = fileProvider.GetFileInfo(@".\FMO_PDFReport_DeliveryRouteLogSummary.xslt");

            mockFileProvider.Setup(x => x.GetDirectoryContents(It.IsAny<string>())).Returns(It.IsAny<IDirectoryContents>());
            mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(It.IsAny<string>())).Returns(filepath);
            mockFileProvider.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(fileInfo);

            testCandidate = new ReferenceDataBusinessService(mockFileProvider.Object, mockReferenceDataDataService.Object, mockConfigurationHelper.Object);
        }
    }
}