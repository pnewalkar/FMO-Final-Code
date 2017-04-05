using Fmo.Common.TestSupport;
using Fmo.DTO;
using Fmo.MessageBrokerCore.Messaging;
using Fmo.NYBLoader.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.NYBLoader.Tests
{
    [TestFixture]
    class PAFLoaderFixture: TestFixtureBase
    {
        private Mock<IMessageBroker<PostalAddressDTO>> msgBrokerMock;
        private Mock<IConfigurationHelper> configurationHelperMock;
        private IPAFLoader testCandidate;

        protected override void OnSetup()
        {
            msgBrokerMock = CreateMock<IMessageBroker<PostalAddressDTO>>();
            configurationHelperMock = CreateMock<IConfigurationHelper>();
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("PAFProcessedFilePath")).Returns("d:/processedfile/");
            configurationHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("PAFErrorFilePath")).Returns("d:/errorfile/");
            testCandidate = new PAFLoader(msgBrokerMock.Object,configurationHelperMock.Object);
        }

        [Test]
        public void Test_LoadPAF_Valid()
        {
            string strLine = "7 / 19 / 2016,8:37:00,B,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162428,S, ,1A\r\n7 / 19 / 2016,8:37:00,C,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A\r\n";
            string strFileName = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.csv");
            List<PostalAddressDTO> methodOutput = testCandidate.ProcessPAF(strLine, strFileName);
            Assert.IsNotNull(methodOutput);
            Assert.IsTrue(methodOutput.Count == 2);
        }

        [Test]
        public void Test_LoadPAF_DuplicateFiles()
        {
            string strLine = "7 / 19 / 2016,8:37:00,B,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A\r\n7 / 19 / 2016,8:37:00,C,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A\r\n";
            string strFileName = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.csv");
            List<PostalAddressDTO> methodOutput = testCandidate.ProcessPAF(strLine, strFileName);
            Assert.IsNotNull(methodOutput);
            Assert.IsTrue(methodOutput.Count == 0);
        }

        [Test]
        public void Test_ValidRecords_Data()
        {
            string strLine = "7 / 19 / 2016,8:37:00,B,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162428,S, ,1A\r\n7 / 19 / 2016,8:37:00,C,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A\r\n";
            string strFileName = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.csv");
            List<PostalAddressDTO> methodOutput = testCandidate.ProcessPAF(strLine, strFileName);
            Assert.IsNotNull(methodOutput);
            Assert.IsNotNull(methodOutput[0].UDPRN);
        }

        [Test]
        public void Test_InvalidRecords_Data()
        {
            string strLine = "7 / 19 / 2016,8:37:00,B,2,YO23 1DQ, The Residence,,,,,54162428,S, ,1A\r\n7 / 19 / 2016,8:37:00,C,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A\r\n";
            string strFileName = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.csv");
            List<PostalAddressDTO> methodOutput = testCandidate.ProcessPAF(strLine, strFileName);
            Assert.IsNull(methodOutput);
        }

        [Test]
        public void Test_InvalidRecords_UDPRNNull_Data()
        {
            string strLine = "7 / 19 / 2016,8:37:00,B,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,,S, ,1A\r\n7 / 19 / 2016,8:37:00,C,2,YO23 1DQ,York,,,Bishopthorpe Road,, , The Residence,,,,,54162429,S, ,1A\r\n";
            string strFileName = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.csv");
            List<PostalAddressDTO> methodOutput = testCandidate.ProcessPAF(strLine, strFileName);
            int InValidReccount = methodOutput.Where(n => n.IsValidData == false).Count();
            Assert.IsNotNull(methodOutput);
            Assert.IsTrue(InValidReccount == 1);
        }
    }
}
