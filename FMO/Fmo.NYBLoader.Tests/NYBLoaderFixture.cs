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
using Fmo.Common.Interface;
using Fmo.Common.Enums;

namespace Fmo.NYBLoader.Tests
{
    [TestFixture]
    public class NYBLoaderFixture : TestFixtureBase
    {
        private Mock<IHttpHandler> httpHandlerMock;
        private Mock<IConfigurationHelper> configurationHelperMock;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private Mock<IExceptionHelper> mockExceptioHelper;
        private INYBLoader testCandidate;

        protected override void OnSetup()
        {
            httpHandlerMock = CreateMock<IHttpHandler>();
            configurationHelperMock = CreateMock<IConfigurationHelper>();
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockExceptioHelper = CreateMock<IExceptionHelper>();
            mockLoggingHelper.Setup(n => n.LogError(It.IsAny<Exception>()));
            mockExceptioHelper.Setup(n => n.HandleException(It.IsAny<Exception>(), It.IsAny<ExceptionHandlingPolicy>())).Returns(true);
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("ProcessedFilePath")).Returns("d:/processedfile/");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("ErrorFilePath")).Returns("d:/errorfile/");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("FMOWebAPIURL")).Returns("http://dunnyURl.com/");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("FMOWebAPIName")).Returns("api/SaveDetails");
            testCandidate = new NYB.NYBLoader(httpHandlerMock.Object, configurationHelperMock.Object, mockLoggingHelper.Object, mockExceptioHelper.Object);
        }

        [Test]
        public async Task Test_SuccessFullHttpClientCall()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { Address_Id = 10 } };

            httpHandlerMock.Setup(x => x.SetBaseAddress(It.IsAny<Uri>()));
            httpHandlerMock.Setup(x => x.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<List<PostalAddressDTO>>())).Returns(() => new Task<HttpResponseMessage>(() => new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            var result = await testCandidate.SaveNybDetails(lstPostalAddressDTO,"test.csv");
            httpHandlerMock.Verify(x => x.PostAsJsonAsync(It.IsAny<string>(), lstPostalAddressDTO), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_FailureHttpClientCall()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { Address_Id = 10 } };

            httpHandlerMock.Setup(x => x.SetBaseAddress(It.IsAny<Uri>()));
            httpHandlerMock.Setup(x => x.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<List<PostalAddressDTO>>())).Throws<Exception>();

            var result = await testCandidate.SaveNybDetails(lstPostalAddressDTO,"test.csv");
            httpHandlerMock.Verify(x => x.PostAsJsonAsync(It.IsAny<string>(), lstPostalAddressDTO), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_ValidRecords_Count()
        {
            string strLine = "AB10 1AB,London,,,Old Town Street, , ,2a,Flat 1,,,,53879041,S, ,1A\r\nAS10 1AS,Nahur,,,Old Town Street, , ,2a,Flat 2,,,,53879070,S, ,1B";
            List<PostalAddressDTO> methodOutput = testCandidate.LoadNybDetailsFromCsv(strLine);
            Assert.IsNotNull(methodOutput);
            Assert.IsTrue(methodOutput.Count == 2);
        }

        [Test]
        public void Test_ValidRecords_Data()
        {
            string strLine = "AB10 1AB,London,,,Old Town Street, , ,2a,Flat 1,,,,53879041,S, ,1A\r\nAS10 1AS,Nahur,,,Old Town Street, , ,2a,Flat 2,,,,53879070,S, ,1B";
            List<PostalAddressDTO> methodOutput = testCandidate.LoadNybDetailsFromCsv(strLine);
            Assert.IsNotNull(methodOutput);
            Assert.IsNotNull(methodOutput[0].UDPRN);
        }

        [Test]
        public void Test_InvalidRecords_Data()
        {
            string strLine = "AB10 1AB,London,,,Old Town Street, , ,2a,Flat 1,,,,53879041,S, ,1A,,,,,,\r\nAS10 1AS,Nahur,,,Old Town Street, , ,2a,Flat 2,,,,53879070,S, ,1B";
            List<PostalAddressDTO> methodOutput = testCandidate.LoadNybDetailsFromCsv(strLine);
            Assert.IsNull(methodOutput);
        }

        [Test]
        public void Test_InValidPostCodeType_Data()
        {
            string strLine = "AB10 1AB,London,,,Old Town Street, , ,2a,Flat 1,,,,53879041,G, ,1A\r\nAS10 1AS,Nahur,,,Old Town Street, , ,2a,Flat 2,,,,53879070,S, ,1B";
            List<PostalAddressDTO> methodOutput = testCandidate.LoadNybDetailsFromCsv(strLine);
            int InValidReccount = methodOutput.Where(n => n.IsValidData == false).Count();
            Assert.IsNotNull(methodOutput);
            Assert.IsTrue(InValidReccount == 1);
        }

        [Test]
        public void Test_InValidPostCode_Data()
        {
            string strLine = "1458 abc,London,,,Old Town Street, , ,2a,Flat 1,,,,53879041,l, ,1A\r\nAS10 1AS,Nahur,,,Old Town Street, , ,2a,Flat 2,,,,53879070,S, ,1B";
            List<PostalAddressDTO> methodOutput = testCandidate.LoadNybDetailsFromCsv(strLine);
            int InValidReccount = methodOutput.Where(n => n.IsValidData == false).Count();
            Assert.IsTrue(InValidReccount == 1);
        }
    }

}