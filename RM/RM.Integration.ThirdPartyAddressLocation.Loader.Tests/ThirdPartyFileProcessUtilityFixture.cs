using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.FileProcessing;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.MessageBrokerMiddleware;
using RM.Integration.ThirdPartyAddressLocation.Loader.Utils;
using RM.Integration.ThirdPartyAddressLocation.Loader.Utils.Interfaces;

namespace RM.Integration.ThirdPartyAddressLocation.Loader.Tests
{
    [TestFixture]
    public class ThirdPartyFileProcessUtilityFixture : TestFixtureBase
    {
        private IThirdPartyFileProcessUtility testCandidate;
        private Mock<IFileMover> fileMoverMock;
        private Mock<IMessageBroker<AddressLocationUSRDTO>> msgBrokerMock;
        private Mock<IExceptionHelper> exceptionHelperMock;
        private Mock<ILoggingHelper> loggingHelperMock;
        private Mock<IConfigurationHelper> configHelperMock;

        /// <summary>
        /// Test the method with the valid file data.
        /// </summary>
        [Test]
        public void Test_Valid_FileData()
        {
            msgBrokerMock = CreateMock<IMessageBroker<AddressLocationUSRDTO>>();
            fileMoverMock = CreateMock<IFileMover>();
            exceptionHelperMock = CreateMock<IExceptionHelper>();
            loggingHelperMock = CreateMock<ILoggingHelper>();
            configHelperMock = CreateMock<IConfigurationHelper>();
            string filepath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\OSABP_E703.xml");

            Exception mockException = It.IsAny<Exception>();
            fileMoverMock.Setup(x => x.MoveFile(It.IsAny<string[]>(), It.IsAny<string[]>()));
            msgBrokerMock.Setup(x => x.CreateMessage(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IMessage>());
            msgBrokerMock.Setup(x => x.SendMessage(It.IsAny<IMessage>()));
            exceptionHelperMock.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<ExceptionHandlingPolicy>(), out mockException));
            configHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("XSDLocation"))
                   .Returns(Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\Schemas\USRFileSchema.xsd"));
            configHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("TPFProcessedFilePath")).Returns(@"D:\Projects\SourceFiles\TPF\Processed");
            configHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("TPFErrorFilePath")).Returns(@"D:\Projects\SourceFiles\TPF\Error");

            testCandidate = new ThirdPartyFileProcessUtility(
                                            msgBrokerMock.Object,
                                            fileMoverMock.Object,
                                            exceptionHelperMock.Object,
                                            loggingHelperMock.Object,
                                            configHelperMock.Object);

            testCandidate.LoadUSRDetailsFromXML(filepath);
            msgBrokerMock.Verify(x => x.CreateMessage(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            msgBrokerMock.Verify(x => x.SendMessage(It.IsAny<IMessage>()), Times.Exactly(2));
            exceptionHelperMock.Verify(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<ExceptionHandlingPolicy>(), out mockException), Times.Never);
            fileMoverMock.Verify(x => x.MoveFile(It.IsAny<string[]>(), It.IsAny<string[]>()), Times.Exactly(1));
        }

        /// <summary>
        /// Test the method with the invalid file data.
        /// </summary>
        [Test]
        public void Test_InValid_FileData()
        {
            msgBrokerMock = CreateMock<IMessageBroker<AddressLocationUSRDTO>>();
            fileMoverMock = CreateMock<IFileMover>();
            exceptionHelperMock = CreateMock<IExceptionHelper>();
            loggingHelperMock = CreateMock<ILoggingHelper>();
            configHelperMock = CreateMock<IConfigurationHelper>();

            string filepath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InvalidFile\OSABP_E702.xml");

            Exception mockException = It.IsAny<Exception>();
            fileMoverMock.Setup(x => x.MoveFile(It.IsAny<string[]>(), It.IsAny<string[]>()));
            msgBrokerMock.Setup(x => x.CreateMessage(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IMessage>());
            msgBrokerMock.Setup(x => x.SendMessage(It.IsAny<IMessage>()));
            exceptionHelperMock.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<ExceptionHandlingPolicy>(), out mockException));
            configHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("XSDLocation"))
                .Returns(Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\Schemas\USRFileSchema.xsd"));
            configHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("TPFProcessedFilePath")).Returns(@"D:\Projects\SourceFiles\TPF\Processed");
            configHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("TPFErrorFilePath")).Returns(@"D:\Projects\SourceFiles\TPF\Error");

            testCandidate = new ThirdPartyFileProcessUtility(
                                            msgBrokerMock.Object,
                                            fileMoverMock.Object,
                                            exceptionHelperMock.Object,
                                            loggingHelperMock.Object,
                                            configHelperMock.Object);

            testCandidate.LoadUSRDetailsFromXML(filepath);
            msgBrokerMock.Verify(x => x.CreateMessage(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            msgBrokerMock.Verify(x => x.SendMessage(It.IsAny<IMessage>()), Times.Never);
            fileMoverMock.Verify(x => x.MoveFile(It.IsAny<string[]>(), It.IsAny<string[]>()), Times.Exactly(1));
        }

        /// <summary>
        /// Setup the mock objects to be available for testing.
        /// </summary>
        protected override void OnSetup()
        {
        }

    }
}
