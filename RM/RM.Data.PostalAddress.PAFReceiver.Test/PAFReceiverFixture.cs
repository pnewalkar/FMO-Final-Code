using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.MessageBrokerMiddleware;

namespace RM.Data.PostalAddress.PAFReceiver.Test
{
    [TestFixture]
    public class PAFReceiverFixture : TestFixtureBase
    {
        Mock<IMessageBroker<PostalAddressDTO>> mockMessageBroker;
        Mock<IHttpHandler> mockHttpHandler;
        Mock<IConfigurationHelper> mockIConfigurationHelper;
        Mock<ILoggingHelper> mockLoggingHelper;
        PAFReceiver testCandidate;

        [Test]
        public void Test_PAFMessageReceived()
        {
            testCandidate.PAFMessageReceived();
        }
        protected override void OnSetup()
        {
            mockMessageBroker = CreateMock< IMessageBroker <PostalAddressDTO>>();
            mockHttpHandler = CreateMock<IHttpHandler>();
            mockIConfigurationHelper = CreateMock<IConfigurationHelper>();
            mockLoggingHelper = CreateMock<ILoggingHelper>();

            mockIConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(Constants.PAFWEBAPINAME)).Returns("http://localhost:43585/api/postaladdressmanager/pafaddresses");
            mockIConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(Constants.ServiceName)).Returns("RM.Data.PostalAddress.PAFReceiver");
            mockMessageBroker.Setup(x => x.HasMessage(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mockMessageBroker.Setup(x => x.ReceiveMessage(It.IsAny<string>(), It.IsAny<string>())).Returns(new PostalAddressDTO() { });

            testCandidate = new PAFReceiver(mockMessageBroker.Object, mockHttpHandler.Object, mockIConfigurationHelper.Object, mockLoggingHelper.Object);
        }
    }
}