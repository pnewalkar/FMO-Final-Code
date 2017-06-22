using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Operational.PDFGenerator.WebAPI.BusinessService;

namespace RM.Operational.PDFGenerator.WebAPI.Test
{
    [TestFixture]
    public class PDFGeneratorBusinessServiceFixture : TestFixtureBase
    {
        private const string PDFFileLoaction = "PDFFileLoaction";
        private const string XSLTFilePath = "XSLTFilePath";

        private Mock<IFileProvider> mockFileProvider;
        private IFileProvider fileProvider;
        private Mock<IConfigurationHelper> mockConfigurationHelper;
        private IPDFGeneratorBusinessService testCandidate;
        private Mock<ILoggingHelper> loggingHelperMock;
        private string filepath;

        [Test]
        public void Test_GenerateRouteLogSummaryReport()
        {
            mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(PDFFileLoaction)).Returns(filepath);
            testCandidate = new PDFGeneratorBusinessService(mockFileProvider.Object, mockConfigurationHelper.Object, loggingHelperMock.Object);
            var result = testCandidate.GeneratePdfReport("ef6db2b6-d8b8-40eb-9d10-33c523ecbd50.pdf");
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GenerateRouteLogSummaryPdf()
        {
            mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(PDFFileLoaction)).Returns(Path.GetTempPath());
            testCandidate = new PDFGeneratorBusinessService(mockFileProvider.Object, mockConfigurationHelper.Object, loggingHelperMock.Object);
            var result = testCandidate.CreateReport("<note><body>hi</body></note>", "abc");
            if (File.Exists(Path.GetTempPath() + result))
            {
                File.Delete(Path.GetTempPath() + result);
            }
        }

        protected override void OnSetup()
        {
            mockFileProvider = CreateMock<IFileProvider>();
            mockConfigurationHelper = CreateMock<IConfigurationHelper>();
            loggingHelperMock = CreateMock<ILoggingHelper>();
            #if DEBUG
                filepath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug\net452", string.Empty), @"TestData\");
            #else
                filepath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Release\net452", string.Empty), @"TestData\");
            #endif
            fileProvider = new PhysicalFileProvider(filepath);
            IFileInfo fileInfo = fileProvider.GetFileInfo(@".\FMO_PDFReport_DeliveryRouteLogSummary.xslt");

            mockFileProvider.Setup(x => x.GetDirectoryContents(It.IsAny<string>())).Returns(It.IsAny<IDirectoryContents>());
            mockFileProvider.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(fileInfo);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(XSLTFilePath)).Returns(Path.GetTempPath());
        }
    }
}