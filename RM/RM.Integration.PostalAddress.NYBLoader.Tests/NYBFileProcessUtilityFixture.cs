using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Integration.PostalAddress.NYBLoader.Utils;
using RM.Integration.PostalAddress.NYBLoader.Utils.Interfaces;

namespace RM.Integration.PostalAddress.NYBLoader.Tests
{
    [TestFixture]
    public class NYBFileProcessUtilityFixture : TestFixtureBase
    {
        private Mock<IHttpHandler> httpHandlerMock;
        private Mock<IConfigurationHelper> configurationHelperMock;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private Mock<IExceptionHelper> mockExceptioHelper;
        private INYBFileProcessUtility testCandidate;

        protected override void OnSetup()
        {
            httpHandlerMock = CreateMock<IHttpHandler>();
            configurationHelperMock = CreateMock<IConfigurationHelper>();
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockExceptioHelper = CreateMock<IExceptionHelper>();
            mockExceptioHelper.Setup(n => n.HandleException(It.IsAny<Exception>(), It.IsAny<ExceptionHandlingPolicy>())).Returns(true);
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("ProcessedFilePath")).Returns("d:/processedfile/");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("ErrorFilePath")).Returns("d:/errorfile/");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("FMOWebAPIURL")).Returns("http://dunnyURl.com/");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("FMOWebAPIName")).Returns("api/SaveDetails");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues(Constants.NoOfCharactersForNYB)).Returns("15");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues(Constants.csvValuesForNYB)).Returns("16");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues(Constants.maxCharactersForNYB)).Returns("507");
            testCandidate = new NYBFileProcessUtility(httpHandlerMock.Object, configurationHelperMock.Object, mockLoggingHelper.Object, mockExceptioHelper.Object);
        }

        [Test]
        public async Task Test_FailureHttpClientCall()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { ID = Guid.NewGuid() } };

            httpHandlerMock.Setup(x => x.SetBaseAddress(It.IsAny<Uri>()));

            var result = await testCandidate.SaveNybDetails(lstPostalAddressDTO, "test.csv");
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