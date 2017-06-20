using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;
using RM.Common.DataService.Interface;
using RM.Common.ReferenceData.WebAPI.BusinessService;
using RM.Common.ReferenceData.WebAPI.BusinessService.Interface;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using System.IO;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using System.Collections.Generic;

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
            var result = testCandidate.GetSimpleListsReferenceData(new List<string>() { "AccessLinkStatus"});
            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {
            mockFileProvider = CreateMock<IFileProvider>();
            mockReferenceDataDataService = CreateMock<IReferenceDataDataService>();
            mockConfigurationHelper = CreateMock<IConfigurationHelper>();
            filepath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug\net452", string.Empty), @"TestData\");
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

            testCandidate = new ReferenceDataBusinessService(mockFileProvider.Object, mockReferenceDataDataService.Object, mockConfigurationHelper.Object);
        }
    }
}