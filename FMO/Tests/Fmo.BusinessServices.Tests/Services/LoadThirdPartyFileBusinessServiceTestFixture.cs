namespace Fmo.BusinessServices.Tests.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using DTO;
    using Fmo.Common.TestSupport;
    using Fmo.NYBLoader;
    using Fmo.NYBLoader.Interfaces;
    using MessageBrokerCore.Messaging;
    using Moq;

    using NUnit.Framework;
    using Common.Constants;
    using System.Messaging;

    public class LoadThirdPartyFileBusinessServiceTestFixture : TestFixtureBase
    {
        private ITPFLoader testCandidate;
        private IMessageBroker<AddressLocationUSRDTO> msgBroker;

        [Test]
        public void TestValidRecords_Count()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidTestFile.xml");
            List<AddressLocationUSRDTO> testLstAddressLoc = new List<AddressLocationUSRDTO>(new AddressLocationUSRDTO[]
            {
                new AddressLocationUSRDTO
                {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                },
                new AddressLocationUSRDTO
                {
                    udprn = 13645,
                    changeType = "I",
                    xCoordinate = 393450,
                    yCoordinate = 805825,
                    latitude = (decimal)57.14326,
                    longitude = (decimal)-2.10988
                }
            });

            List<AddressLocationUSRDTO> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc.Count, methodOutput.Count);
        }

        [Test]
        public void TestValidRecords_Data_1()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidTestFile.xml");
            List<AddressLocationUSRDTO> testLstAddressLoc = new List<AddressLocationUSRDTO>(new AddressLocationUSRDTO[]
            {
                new AddressLocationUSRDTO
                {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                },
                new AddressLocationUSRDTO
                {
                    udprn = 13645,
                    changeType = "I",
                    xCoordinate = 393450,
                    yCoordinate = 805825,
                    latitude = (decimal)57.14326,
                    longitude = (decimal)-2.10988
                }
            });

            List<AddressLocationUSRDTO> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc[0].udprn, methodOutput[0].udprn);
        }

        [Test]
        public void TestValidRecords_Data_2()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidTestFile.xml");
            List<AddressLocationUSRDTO> testLstAddressLoc = new List<AddressLocationUSRDTO>(new AddressLocationUSRDTO[] 
            {
                new AddressLocationUSRDTO
                {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                },
                new AddressLocationUSRDTO
                {
                    udprn = 13645,
                    changeType = "I",
                    xCoordinate = 393450,
                    yCoordinate = 805825,
                    latitude = (decimal)57.14326,
                    longitude = (decimal)-2.10988
                }
            });

            List<AddressLocationUSRDTO> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc[0].changeType, methodOutput[0].changeType);
        }

        [Test]
        public void TestValidRecords_Data_3()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidTestFile.xml");
            List<AddressLocationUSRDTO> testLstAddressLoc = new List<AddressLocationUSRDTO>(new AddressLocationUSRDTO[]
            {
                new AddressLocationUSRDTO
                {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                },
                new AddressLocationUSRDTO
                {
                    udprn = 13645,
                    changeType = "I",
                    xCoordinate = 393450,
                    yCoordinate = 805825,
                    latitude = (decimal)57.14326,
                    longitude = (decimal)-2.10988
                }
            });

            List<AddressLocationUSRDTO> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc[1].xCoordinate, methodOutput[1].xCoordinate);
        }

        [Test]
        public void TestInValidRecordsCount()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InvalidFile\InvalidTestFile.xml");
            List<AddressLocationUSRDTO> testLstAddressLoc = new List<AddressLocationUSRDTO>(new AddressLocationUSRDTO[]
            {
                new AddressLocationUSRDTO
                {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                }
            });

            List<AddressLocationUSRDTO> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc.Count, methodOutput.Count);
        }

        [Test]
        public void TestInValidRecordsData1()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InvalidFile\InvalidTestFile.xml");
            List<AddressLocationUSRDTO> testLstAddressLoc = new List<AddressLocationUSRDTO>(new AddressLocationUSRDTO[]
            {
                new AddressLocationUSRDTO
                {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                }
            });

            List<AddressLocationUSRDTO> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc[0].udprn, methodOutput[0].udprn);
        }

        [Test]
        public void TestInValidRecordsData2()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InvalidFile\InvalidTestFile.xml");
            List<AddressLocationUSRDTO> testLstAddressLoc = new List<AddressLocationUSRDTO>(new AddressLocationUSRDTO[]
            {
                new AddressLocationUSRDTO
                {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                }
            });

            List<AddressLocationUSRDTO> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc[0].changeType, methodOutput[0].changeType);
        }

        [Test]
        public void TestInValidRecordsData3()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InvalidFile\InvalidTestFile.xml");
            List<AddressLocationUSRDTO> testLstAddressLoc = new List<AddressLocationUSRDTO>(new AddressLocationUSRDTO[]
            {
                new AddressLocationUSRDTO
                {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                }
            });

            List<AddressLocationUSRDTO> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc[0].xCoordinate, methodOutput[0].xCoordinate);
        }

        [Test]
        public void TestPushMessageToQueue()
        {
            //Mock<IMessageBroker<AddressLocationUSRDTO>> mockUSRMq = new Mock<IMessageBroker<AddressLocationUSRDTO>>();
            //IMessage data = null;
            //MessageQueue messageQueue = new MessageQueue(Constants.QUEUE_PATH + Constants.QUEUE_THIRD_PARTY);
            //AddressLocationUSRDTO testAddressLoc = new AddressLocationUSRDTO {
            //    udprn = 12345,
            //    changeType = "I",
            //    latitude = (decimal)23.57,
            //    longitude = (decimal)24.32,
            //    xCoordinate = 4567,
            //    yCoordinate = 2347
            //};

            //mockUSRMq.Setup(x => x.CreateMessage(testAddressLoc, Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH)).Returns(() => {

            //    return data;
            //});
        }

        protected override void OnSetup()
        {
            msgBroker = new MessageBroker<AddressLocationUSRDTO>();
            testCandidate = new TPFLoader(msgBroker);
        }
    }
}
