using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;
using RM.Common.ReferenceData.WebAPI.BusinessService;
using RM.Common.ReferenceData.WebAPI.BusinessService.Interface;
using RM.Common.ReferenceData.WebAPI.DataService.Interface;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

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
        private Mock<ILoggingHelper> loggingHelperMock;

        private string filepath;

        [Test]
        public void Test_GetReferenceDataByNameValuePairs()
        {
            var result = testCandidate.GetReferenceDataByNameValuePairs("AccessLinkStatus", "AccessLinkParameters");
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, "key");
            Assert.AreEqual(result.Value, "value");
        }

        [Test]
        public void Test_GetSimpleListsReferenceData()
        {
            var result = testCandidate.GetSimpleListsReferenceData("AccessLinkStatus");
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetSimpleListsReferenceDataList()
        {
            var result = testCandidate.GetSimpleListsReferenceData(new List<string>() { "AccessLinkStatus" });
            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {
            mockFileProvider = CreateMock<IFileProvider>();
            mockReferenceDataDataService = CreateMock<IReferenceDataDataService>();
            mockConfigurationHelper = CreateMock<IConfigurationHelper>();
            loggingHelperMock = CreateMock<ILoggingHelper>();
#if DEBUG
            filepath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug\net452", string.Empty), @"TestData\");
#else
                filepath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Release\net452", string.Empty), @"TestData\");
#endif
            fileProvider = new PhysicalFileProvider(filepath);
            IFileInfo fileInfo = fileProvider.GetFileInfo(@".\ReferenceDataMapping.xml");
            NameValuePair collection = new NameValuePair();
            collection.Name = "key";
            collection.Value = "value";

            mockFileProvider.Setup(x => x.GetDirectoryContents(It.IsAny<string>())).Returns(It.IsAny<IDirectoryContents>());
            mockConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(It.IsAny<string>())).Returns(filepath);
            mockFileProvider.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns(fileInfo);

            mockReferenceDataDataService.Setup(x => x.GetNameValueReferenceData(It.IsAny<string>(), It.IsAny<string>())).Returns(collection);
            mockReferenceDataDataService.Setup(x => x.GetSimpleListReferenceData(It.IsAny<string>())).Returns(new SimpleListDTO() { });

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new ReferenceDataBusinessService(mockFileProvider.Object, mockReferenceDataDataService.Object, mockConfigurationHelper.Object, loggingHelperMock.Object);
        }
    }
}