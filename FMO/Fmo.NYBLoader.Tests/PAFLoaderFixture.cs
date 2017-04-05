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
        private IPAFLoader testCandidate;

        protected override void OnSetup()
        {
            msgBrokerMock = CreateMock<IMessageBroker<PostalAddressDTO>>();
            testCandidate = new PAFLoader(msgBrokerMock.Object);
        }

        [Test]
        public void Test_LoadPAF_Count()
        {
            string strFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidPAF.zip");
            //List<PostalAddressDTO> methodOutput = testCandidate.LoadPAF(strFilePath);
        }
    }
}
