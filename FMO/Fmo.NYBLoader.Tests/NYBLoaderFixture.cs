using Fmo.Common.TestSupport;
using Fmo.DTO;
using Fmo.NYBLoader.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NYB = Fmo.NYBLoader;
using System.Linq;

namespace Fmo.NYBLoader.Tests
{
    [TestFixture]
    public class NYBLoaderFixture : TestFixtureBase
    {
        private Mock<IHttpHandler> httpHandlerMock;
        private Mock<IConfigurationHelper> configurationHelperMock;
        private INYBLoader testCandidate;

        protected override void OnSetup()
        {
            httpHandlerMock = CreateMock<IHttpHandler>();
            configurationHelperMock = CreateMock<IConfigurationHelper>();
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("ProcessedFilePath")).Returns("d:/processedfile/");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("ErrorFilePath")).Returns("d:/errorfile/");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("FMOWebAPIURL")).Returns("http://dunnyURl.com/");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("FMOWebAPIName")).Returns("api/SaveDetails");
            testCandidate = new NYB.NYBLoader(httpHandlerMock.Object, configurationHelperMock.Object);
        }

        [Test]
        public void Test_SuccessFullHttpClientCall()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { Address_Id = 10 } };

            httpHandlerMock.Setup(x => x.SetBaseAddress(It.IsAny<Uri>()));
            httpHandlerMock.Setup(x => x.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<List<PostalAddressDTO>>())).Returns(() => new Task<HttpResponseMessage>(() => new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            var task = testCandidate.SaveNYBDetails(lstPostalAddressDTO);
            httpHandlerMock.Verify(x => x.SetBaseAddress(It.IsAny<Uri>()), Times.Once());
            httpHandlerMock.Verify(x => x.PostAsJsonAsync(It.IsAny<string>(), lstPostalAddressDTO), Times.Once());
            Assert.IsNotNull(task);
            Assert.IsTrue(task);
        }

        [Test]
        public void Test_FailureHttpClientCall()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { Address_Id = 10 } };

            httpHandlerMock.Setup(x => x.SetBaseAddress(It.IsAny<Uri>()));
            httpHandlerMock.Setup(x => x.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<List<PostalAddressDTO>>())).Throws<Exception>();

            var task = testCandidate.SaveNYBDetails(lstPostalAddressDTO);
            httpHandlerMock.Verify(x => x.SetBaseAddress(It.IsAny<Uri>()), Times.Once());
            httpHandlerMock.Verify(x => x.PostAsJsonAsync(It.IsAny<string>(), lstPostalAddressDTO), Times.Once());
            Assert.IsNotNull(task);
            Assert.IsFalse(task);
        }

        [Test]
        public void Test_ValidRecords_Count()
        {
            string strFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidNYB.zip");
            List<PostalAddressDTO> methodOutput = testCandidate.LoadNYBDetailsFromCSV(strFilePath);
            Assert.IsNotNull(methodOutput);
            Assert.IsTrue(methodOutput.Count == 2);
        }

        [Test]
        public void Test_ValidRecords_Data()
        {
            string strFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidNYB.zip");
            List<PostalAddressDTO> methodOutput = testCandidate.LoadNYBDetailsFromCSV(strFilePath);
            Assert.IsNotNull(methodOutput);
            Assert.IsNotNull(methodOutput[0].UDPRN);
        }

        [Test]
        public void Test_InvalidRecords_Data()
        {
            string strFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InValidFile\InValidNYB.zip");
            List<PostalAddressDTO> methodOutput = testCandidate.LoadNYBDetailsFromCSV(strFilePath);
            Assert.IsNull(methodOutput);
        }

        [Test]
        public void Test_InValidPostCodeType_Data()
        {
            string strFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InValidFile\InValidPostCodeType.zip");
            List<PostalAddressDTO> methodOutput = testCandidate.LoadNYBDetailsFromCSV(strFilePath);
            int InValidReccount = methodOutput.Where(n => n.IsValidData == false).Count();
            Assert.IsNotNull(methodOutput);
            Assert.IsTrue(InValidReccount == 1);
            Assert.IsFalse(InValidReccount == 2);
        }

        [Test]
        public void Test_InValidPostCode_Data()
        {
            string strFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InValidFile\InValidPostCode.zip");
            List<PostalAddressDTO> methodOutput = testCandidate.LoadNYBDetailsFromCSV(strFilePath);
            int InValidReccount = methodOutput.Where(n => n.IsValidData == false).Count();
            Assert.IsTrue(InValidReccount == 1);
            Assert.IsFalse(InValidReccount == 2);
        }
    }

}