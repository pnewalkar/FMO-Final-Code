using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.MessageBrokerMiddleware;
using RM.Integration.PostalAddress.PAFLoader.Utils;
using RM.Integration.PostalAddress.PAFLoader.Utils.Interfaces;

namespace RM.Integration.PostalAddress.PAFLoader.Tests
{
    [TestFixture]
    public class PAFFileProcessUtilityFixture : TestFixtureBase
    {
        private Mock<IMessageBroker<PostalAddressDTO>> msgBrokerMock;
        private Mock<IConfigurationHelper> configurationHelperMock;
        private Mock<ILoggingHelper> loggingHelperMock;
        private IPAFFileProcessUtility testCandidate;

        protected override void OnSetup()
        {
            msgBrokerMock = CreateMock<IMessageBroker<PostalAddressDTO>>();
            loggingHelperMock = CreateMock<ILoggingHelper>();
            configurationHelperMock = CreateMock<IConfigurationHelper>();
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("PAFProcessedFilePath")).Returns("d:/processedfile/");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("PAFErrorFilePath")).Returns("d:/errorfile/");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues(Constants.NoOfCharactersForPAF)).Returns("19");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues(Constants.MaxCharactersForPAF)).Returns("534");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues(Constants.CsvPAFValues)).Returns("20");
            testCandidate = new PAFFileProcessUtility(msgBrokerMock.Object, configurationHelperMock.Object, loggingHelperMock.Object);
        }

        [Test]
        public void Test_LoadPAF_Valid()
        {
            string strLine = "7 / 19 / 2016,8:37:00,B,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162428,S, ,1A\r\n7 / 19 / 2016,8:37:00,C,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A";
            string strFileName = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.csv");
            List<PostalAddressDTO> methodOutput = testCandidate.ProcessPAF(strLine, strFileName);
            Assert.IsNotNull(methodOutput);
            Assert.IsTrue(methodOutput.Count == 2);
        }

        [Test]
        public void Test_LoadPAF_DuplicateFiles()
        {
            string strLine = "7 / 19 / 2016,8:37:00,B,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A\r\n7 / 19 / 2016,8:37:00,C,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A";
            string strFileName = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.csv");
            List<PostalAddressDTO> methodOutput = testCandidate.ProcessPAF(strLine, strFileName);
            Assert.IsNotNull(methodOutput);
            Assert.IsTrue(methodOutput.Count == 0);
        }

        [Test]
        public void Test_ValidRecords_Data()
        {
            string strLine = "7 / 19 / 2016,8:37:00,B,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162428,S, ,1A\r\n7 / 19 / 2016,8:37:00,C,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A";
            string strFileName = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.csv");
            List<PostalAddressDTO> methodOutput = testCandidate.ProcessPAF(strLine, strFileName);
            Assert.IsNotNull(methodOutput);
            Assert.IsNotNull(methodOutput[0].UDPRN);
        }

        [Test]
        public void Test_InvalidRecords_Data()
        {
            string strLine = "7 / 19 / 2016,8:37:00,B,2,YO23 1DQ, The Residence,,,,,54162428,S, ,1A\r\n7 / 19 / 2016,8:37:00,C,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A";
            string strFileName = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.csv");
            List<PostalAddressDTO> methodOutput = testCandidate.ProcessPAF(strLine, strFileName);
            Assert.IsNull(methodOutput);
        }

        [Test]
        public void Test_InvalidRecords_UDPRNNull_Data()
        {
            string strLine = "7 / 19 / 2016,8:37:00,B,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,,S, ,1A\r\n7 / 19 / 2016,8:37:00,C,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A";
            string strFileName = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.csv");
            List<PostalAddressDTO> methodOutput = testCandidate.ProcessPAF(strLine, strFileName);
            int InValidReccount = methodOutput.Where(n => n.IsValidData == false).Count();
            Assert.IsNotNull(methodOutput);
            Assert.IsTrue(InValidReccount == 1);
        }

        [Test]
        public void Test_SavePAFDetails_Valid()
        {
            string strLine = "7 / 19 / 2016,8:37:00,I,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162428,S, ,1A\r\n7 / 19 / 2016,8:37:00,I,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A";
            string strFileName = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.csv");
            List<PostalAddressDTO> methodOutput = testCandidate.ProcessPAF(strLine, strFileName);
            msgBrokerMock.Setup(n => n.CreateMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IMessage>());
            msgBrokerMock.Setup(n => n.SendMessage(It.IsAny<IMessage>()));

            bool flag = testCandidate.SavePAFDetails(methodOutput);
            msgBrokerMock.Verify(m => m.CreateMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            msgBrokerMock.Verify(m => m.SendMessage(It.IsAny<IMessage>()), Times.AtLeastOnce);

            Assert.IsTrue(flag);
        }

        [Test]
        public void Test_SavePAFDetails_InValid()
        {
            string strLine = "7 / 19 / 2016,8:37:00,I,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162428,S, ,1A\r\n7 / 19 / 2016,8:37:00,I,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A";
            string strFileName = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.csv");
            List<PostalAddressDTO> methodOutput = testCandidate.ProcessPAF(strLine, strFileName);
            msgBrokerMock.Setup(n => n.CreateMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IMessage>());
            msgBrokerMock.Setup(n => n.SendMessage(It.IsAny<IMessage>())).Throws<Exception>();

            bool flag = testCandidate.SavePAFDetails(methodOutput);
            msgBrokerMock.Verify(m => m.CreateMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            msgBrokerMock.Verify(m => m.SendMessage(It.IsAny<IMessage>()), Times.AtLeastOnce);

            Assert.IsFalse(flag);
        }
    }
}