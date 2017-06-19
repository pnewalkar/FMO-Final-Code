using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Http;
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
        private IFileProvider fileProvider;
        Mock<IConfigurationHelper> mockConfigurationHelper;
        IPDFGeneratorBusinessService testCandidate;
        string filepath;

        [Test]
        public void Test_GenerateRouteLogSummaryReport()
        {
            mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(filepath));
            var result = testCandidate.GeneratePdfReport("ef6db2b6-d8b8-40eb-9d10-33c523ecbd50.pdf");
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GenerateRouteLogSummaryPdf()
        {
            var result = testCandidate.GenerateRouteLogSummaryReport("<note><body>hi</body></note>","abc");
            if (File.Exists(Path.GetTempPath()+ result))
            {
                File.Delete(Path.GetTempPath() + result);
            }
        }
        protected override void OnSetup()
        {
            mockFileProvider = CreateMock<IFileProvider>();
            mockConfigurationHelper = CreateMock<IConfigurationHelper>();
            filepath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug\net452", string.Empty), @"TestData\");
            fileProvider = new PhysicalFileProvider(filepath);
            IFileInfo fileInfo = fileProvider.GetFileInfo(@".\FMO_PDFReport_DeliveryRouteLogSummary.xslt");

            mockFileProvider.Setup(x => x.GetDirectoryContents(It.IsAny<string>())).Returns(It.IsAny<IDirectoryContents>());
            mockFileProvider.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(fileInfo);

            mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(It.IsAny<string>())).Returns(Path.GetTempPath());

            testCandidate = new PDFGeneratorBusinessService(mockFileProvider.Object, mockConfigurationHelper.Object);
        }
    }
}
