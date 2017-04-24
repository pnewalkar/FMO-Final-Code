﻿namespace Fmo.BusinessServices.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Messaging;
    using System.Reflection;
    using Common.Constants;
    using Common.Enums;
    using Common.Interface;
    using DataServices.Repositories.Interfaces;
    using DTO;
    using DTO.FileProcessing;
    using Fmo.Common.TestSupport;
    using Fmo.NYBLoader;
    using Fmo.NYBLoader.Interfaces;
    using MessageBrokerCore.Messaging;
    using Moq;
    using NUnit.Framework;

    public class LoadThirdPartyFileBusinessServiceTestFixture : TestFixtureBase
    {
        private IUSRLoader testCandidate;
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

            string filepath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidTestFile.xml");

            Exception mockException = It.IsAny<Exception>();
            fileMoverMock.Setup(x => x.MoveFile(It.IsAny<string[]>(), It.IsAny<string[]>()));
            msgBrokerMock.Setup(x => x.CreateMessage(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IMessage>());
            msgBrokerMock.Setup(x => x.SendMessage(It.IsAny<IMessage>()));
            exceptionHelperMock.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<ExceptionHandlingPolicy>(), out mockException));
            loggingHelperMock.Setup(x => x.LogError(It.IsAny<Exception>()));
            loggingHelperMock.Setup(x => x.LogWarn(It.IsAny<string>()));
            configHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("XSDLocation")).Returns(@"C:\Workspace\FMO\FMO\Fmo.NYBLoader\Schemas\USRFileSchema.xsd");
            configHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("TPFProcessedFilePath")).Returns(@"D:\Projects\SourceFiles\TPF\Processed");
            configHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("TPFErrorFilePath")).Returns(@"D:\Projects\SourceFiles\TPF\Error");            

            testCandidate = new USRLoader(
                                            msgBrokerMock.Object,
                                            fileMoverMock.Object,
                                            exceptionHelperMock.Object,
                                            loggingHelperMock.Object,
                                            configHelperMock.Object);

            testCandidate.LoadTPFDetailsFromXML(filepath);
            msgBrokerMock.Verify(x => x.CreateMessage(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            msgBrokerMock.Verify(x => x.SendMessage(It.IsAny<IMessage>()), Times.Exactly(2));
            exceptionHelperMock.Verify(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<ExceptionHandlingPolicy>(), out mockException), Times.Never);
            loggingHelperMock.Verify(x => x.LogError(It.IsAny<Exception>()), Times.Never);
            loggingHelperMock.Verify(x => x.LogWarn(It.IsAny<string>()), Times.Never);
            fileMoverMock.Verify(x => x.MoveFile(It.IsAny<string[]>(), It.IsAny<string[]>()), Times.Exactly(1));
        }

        /// <summary>
        /// Test the method with the invalid file data.
        /// </summary>
        [Test]
        public void Test_InValid_FileData()
        {
            string filepath = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InvalidFile\InvalidTestFile.xml");

            Exception mockException = It.IsAny<Exception>();
            fileMoverMock.Setup(x => x.MoveFile(It.IsAny<string[]>(), It.IsAny<string[]>()));
            msgBrokerMock.Setup(x => x.CreateMessage(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IMessage>());
            msgBrokerMock.Setup(x => x.SendMessage(It.IsAny<IMessage>()));
            exceptionHelperMock.Setup(x => x.HandleException(It.IsAny<Exception>(), It.IsAny<ExceptionHandlingPolicy>(), out mockException));
            loggingHelperMock.Setup(x => x.LogError(It.IsAny<Exception>()));
            loggingHelperMock.Setup(x => x.LogWarn(It.IsAny<string>()));
            configHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("XSDLocation")).Returns(@"C:\Workspace\FMO\FMO\Fmo.NYBLoader\Schemas\USRFileSchema.xsd");
            configHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("TPFProcessedFilePath")).Returns(@"D:\Projects\SourceFiles\TPF\Processed");
            configHelperMock.Setup(x => x.ReadAppSettingsConfigurationValues("TPFErrorFilePath")).Returns(@"D:\Projects\SourceFiles\TPF\Error");

            testCandidate = new USRLoader(
                                            msgBrokerMock.Object,
                                            fileMoverMock.Object,
                                            exceptionHelperMock.Object,
                                            loggingHelperMock.Object,
                                            configHelperMock.Object);

            testCandidate.LoadTPFDetailsFromXML(filepath);
            msgBrokerMock.Verify(x => x.CreateMessage(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            msgBrokerMock.Verify(x => x.SendMessage(It.IsAny<IMessage>()), Times.Never);
            loggingHelperMock.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            fileMoverMock.Verify(x => x.MoveFile(It.IsAny<string[]>(), It.IsAny<string[]>()), Times.Exactly(1));
        }

        /// <summary>
        /// Setup the mock objects to be available for testing.
        /// </summary>
        protected override void OnSetup()
        {
            msgBrokerMock = CreateMock<IMessageBroker<AddressLocationUSRDTO>>();
            fileMoverMock = CreateMock<IFileMover>();
            exceptionHelperMock = CreateMock<IExceptionHelper>();
            loggingHelperMock = CreateMock<ILoggingHelper>();
            configHelperMock = CreateMock<IConfigurationHelper>();
        }
    }
}
