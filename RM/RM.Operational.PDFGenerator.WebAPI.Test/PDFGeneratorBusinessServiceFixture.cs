using System.IO;
using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.HelperMiddleware;
using RM.Operational.PDFGenerator.WebAPI.BusinessService;

namespace RM.Operational.PDFGenerator.WebAPI.Test
{
    [TestFixture]
    public class PDFGeneratorBusinessServiceFixture : TestFixtureBase
    {
        Mock<IFileProvider> mockFileProvider;
        Mock<IConfigurationHelper> mockConfigurationHelper;
        IPDFGeneratorBusinessService testCandidate;

        [Test]
        public void Test_GenerateRouteLogSummaryReport()
        {
            var result = testCandidate.CreateReport("<note><body>abc</body></note>", "file1");
        }
        protected override void OnSetup()
        {
            mockFileProvider = CreateMock<IFileProvider>();
            mockConfigurationHelper = CreateMock<IConfigurationHelper>();
            mockFileProvider.Setup(x => x.GetDirectoryContents(It.IsAny<string>())).Returns(It.IsAny<IDirectoryContents>());
            mockFileProvider.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(It.IsAny<IFileInfo>());

            mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(It.IsAny<string>())).Returns("C:\\");

            testCandidate = new PDFGeneratorBusinessService(mockFileProvider.Object, mockConfigurationHelper.Object);
        }
    }
}
