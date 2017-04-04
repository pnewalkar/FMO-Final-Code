namespace Fmo.BusinessServices.Tests.Services
{
    using System.Collections.Generic;
    using System.Reflection;
    using DTO;
    using Fmo.Common.TestSupport;
    using Fmo.NYBLoader;
    using Fmo.NYBLoader.Interfaces;
    using MessageBrokerCore.Messaging;
    using Moq;
    using System.IO;
    using NUnit.Framework;

    public class LoadThirdPartyFileBusinessServiceTestFixture : TestFixtureBase
    {
        private ITPFLoader testCandidate;
        private IMessageBroker<addressLocation> msgBroker;

        protected override void OnSetup()
        {
            msgBroker = new MessageBroker<addressLocation>();
            testCandidate = new TPFLoader(msgBroker);
        }

        [Test]
        public void Test_ValidRecords_Count()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidTestFile.xml");
            List<addressLocation> testLstAddressLoc = new List<addressLocation>(new addressLocation[] {
                new addressLocation {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                },
                new addressLocation {
                    udprn = 13645,
                    changeType = "I",
                    xCoordinate = 393450,
                    yCoordinate = 805825,
                    latitude = (decimal)57.14326,
                    longitude = (decimal)-2.10988
                }
            });

            List<addressLocation> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc.Count, methodOutput.Count);
        }

        [Test]
        public void Test_ValidRecords_Data_1()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidTestFile.xml");
            List<addressLocation> testLstAddressLoc = new List<addressLocation>(new addressLocation[] {
                new addressLocation {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                },
                new addressLocation {
                    udprn = 13645,
                    changeType = "I",
                    xCoordinate = 393450,
                    yCoordinate = 805825,
                    latitude = (decimal)57.14326,
                    longitude = (decimal)-2.10988
                }
            });

            List<addressLocation> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc[0].udprn, methodOutput[0].udprn);
        }

        [Test]
        public void Test_ValidRecords_Data_2()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidTestFile.xml");
            List<addressLocation> testLstAddressLoc = new List<addressLocation>(new addressLocation[] {
                new addressLocation {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                },
                new addressLocation {
                    udprn = 13645,
                    changeType = "I",
                    xCoordinate = 393450,
                    yCoordinate = 805825,
                    latitude = (decimal)57.14326,
                    longitude = (decimal)-2.10988
                }
            });

            List<addressLocation> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc[0].changeType, methodOutput[0].changeType);
        }

        [Test]
        public void Test_ValidRecords_Data_3()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\ValidFile\ValidTestFile.xml");
            List<addressLocation> testLstAddressLoc = new List<addressLocation>(new addressLocation[] {
                new addressLocation {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                },
                new addressLocation {
                    udprn = 13645,
                    changeType = "I",
                    xCoordinate = 393450,
                    yCoordinate = 805825,
                    latitude = (decimal)57.14326,
                    longitude = (decimal)-2.10988
                }
            });

            List<addressLocation> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc[1].xCoordinate, methodOutput[1].xCoordinate);
        }

        [Test]
        public void Test_InValidRecords_Count()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InvalidFile\InvalidTestFile.xml");
            List<addressLocation> testLstAddressLoc = new List<addressLocation>(new addressLocation[] {
                new addressLocation {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                }
            });

            List<addressLocation> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc.Count, methodOutput.Count);
        }

        [Test]
        public void Test_InValidRecords_Data_1()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InvalidFile\InvalidTestFile.xml");
            List<addressLocation> testLstAddressLoc = new List<addressLocation>(new addressLocation[] {
                new addressLocation {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                }
            });

            List<addressLocation> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc[0].udprn, methodOutput[0].udprn);
        }

        [Test]
        public void Test_InValidRecords_Data_2()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InvalidFile\InvalidTestFile.xml");
            List<addressLocation> testLstAddressLoc = new List<addressLocation>(new addressLocation[] {
                new addressLocation {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                }
            });

            List<addressLocation> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc[0].changeType, methodOutput[0].changeType);
        }

        [Test]
        public void Test_InValidRecords_Data_3()
        {
            string str = Path.Combine(TestContext.CurrentContext.TestDirectory.Replace(@"bin\Debug", string.Empty), @"TestData\InvalidFile\InvalidTestFile.xml");
            List<addressLocation> testLstAddressLoc = new List<addressLocation>(new addressLocation[] {
                new addressLocation {
                    udprn = 12132,
                    changeType = "I",
                    xCoordinate = 1234,
                    yCoordinate = 806221,
                    latitude = (decimal)57.14683,
                    longitude = (decimal)-2.08973
                }
            });

            List<addressLocation> methodOutput = testCandidate.GetValidRecords(str);
            Assert.AreEqual(testLstAddressLoc[0].xCoordinate, methodOutput[0].xCoordinate);
        }
    }
}
