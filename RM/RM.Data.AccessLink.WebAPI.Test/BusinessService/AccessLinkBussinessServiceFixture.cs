﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.AccessLink.WebAPI.DataDTOs;
using RM.DataManagement.AccessLink.WebAPI.BusinessService;
using RM.DataManagement.AccessLink.WebAPI.BusinessService.Interface;
using RM.DataManagement.AccessLink.WebAPI.DataService.Interfaces;
using RM.DataManagement.AccessLink.WebAPI.DTOs;
using RM.DataManagement.AccessLink.WebAPI.Integration;

namespace RM.Data.AccessLink.WebAPI.Test
{
    /// <summary>
    /// This class contains test methods for AccessLinkBusinessService.
    /// </summary>
    [TestFixture]
    public class AccessLinkBussinessServiceFixture : TestFixtureBase
    {
        private IAccessLinkBusinessService testCandidate;
        private Mock<IAccessLinkDataService> mockaccessLinkDataService;
        private Mock<IAccessLinkIntegrationService> mockAccessLinkIntegrationService;
        private Mock<ILoggingHelper> loggingHelperMock;
        private List<AccessLinkDTO> accessLinkDTO = null;
        private DeliveryPointDTO deliveryPointDTO = null;
        private AccessLinkManualCreateModelDTO accessLinkManualCreateModelDTO = null;
        private NetworkLinkDTO networkLink = null;
        private Guid operationalObjectId = System.Guid.NewGuid();
        private Guid operationObjectTypeId = new Guid("415c9129-0615-457e-98b7-3a60436320c5");

        /// <summary>
        /// Test for Create the Automatic AccessLink.
        /// </summary>
        [Test]
        public void Test_CreateAccessLink()
        {
            bool expectedResult = testCandidate.CreateAccessLink(operationalObjectId, operationObjectTypeId);
            Assert.True(expectedResult);
        }

        /// <summary>
        /// Test for create the Automatic AccessLink -NegativeScenario
        /// </summary>
        [Test]
        public void Test_GetAccessLinks_NegativeScenario()
        {
            string coordinates = null; // "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";
            Guid unitGuid = Guid.Empty;
            string strAccessLinkDTO = testCandidate.GetAccessLinks(coordinates, unitGuid);

            Assert.IsNull(strAccessLinkDTO);
        }

        /// <summary>
        /// Test for Load AccessLink.
        /// </summary>
        [Test]
        public void Test_GetAccessLinks()
        {
            string coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";
            Guid unitGuid = Guid.NewGuid();
            var result = testCandidate.GetAccessLinks(coordinates, unitGuid);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test for CheckManualAccess is valid.
        /// </summary>
        [Test]
        public void Test_CheckManualAccessLinkIsValid()
        {
            string coordinates = "[399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211]";
            string accessLinkLine = "[488938,197021],[488929.9088937093,197036.37310195228]";
            bool expectedResult = testCandidate.CheckManualAccessLinkIsValid(coordinates, accessLinkLine);
            Assert.IsFalse(expectedResult);
        }

        /// <summary>
        /// Test for Create Manual AccessLink.
        /// </summary>
        [Test]
        public void Test_CreateAccessLinkForMannual()
        {
            OnSetup();
            var expectedResult = testCandidate.CreateAccessLink(accessLinkManualCreateModelDTO);
            Assert.True(expectedResult);
        }

        [Test]
        public void Test_GetAdjPathLength()
        {
            OnSetup();
            decimal expectedResult = testCandidate.GetAdjPathLength(accessLinkManualCreateModelDTO);
            Assert.Greater(expectedResult, 0);
        }

        /// <summary>
        /// Test Method to delete access link once Delivery point deleted.
        /// </summary>
        /// <param name="operationalObjectId"></param>
        /// <returns></returns>
        [Test]
        public async Task Test_DeleteAccessLink_PositiveScenario()
        {
            var expectedResult = await testCandidate.DeleteAccessLink(operationalObjectId);
            Assert.IsNotNull(expectedResult);
        }

        /// <summary>
        /// Setup for Nunit Tests.
        /// </summary>
        protected override void OnSetup()
        {
            accessLinkManualCreateModelDTO = new AccessLinkManualCreateModelDTO
            {
                AccessLinkLine = "[[512455.33999999985,107127.7899999991],[512454.8208646417,107129.43228437999],[512474,107139]]",
                BoundingBoxCoordinates = "511684.44432227453,106504.23168359262,513419.32779204147,107103.43288199503",
                NetworkLinkGUID = "c55712fe-ce92-4386-a0a5-e9a158b15441",
                OperationalObjectGUID = "ffc86397-fbb5-4caf-a070-aca8d723de57",
                Workloadlength = 40,
                NetworkIntersectionPoint = "[512455.33999999985,107127.7899999991]",
                OperationalObjectPoint = "[512455.33999999985,107127.7899999991]"
            };

            deliveryPointDTO = new DeliveryPointDTO
            {
                LocationXY = DbGeometry.PointFromText("POINT (488938 197021)", 27700),
                Latitude = (decimal)50.83133590,
                Longitude = (decimal)-0.40071150,
                Positioned = true,
                UDPRN = 2364534,
                PostalAddress = new RM.CommonLibrary.EntityFramework.DTO.PostalAddressDTO
                {
                    Thoroughfare = "Salvington Gardens"
                },
                DeliveryPointUseIndicator_GUID = Guid.Parse("178EDCAD-9431-E711-83EC-28D244AEF9ED")
            };

            accessLinkDTO = new List<AccessLinkDTO>()
                {
                    new AccessLinkDTO()
                    {
                        ID = Guid.Parse("3981AD3B-D253-4BD9-AB82-000094D8EE67"),
                        OperationalObjectPoint = DbGeometry.PointFromText("POINT (488938 197021)", 27700),
                        NetworkIntersectionPoint = DbGeometry.PointFromText("POINT (488929.9088937093 197036.37310195228)", 27700),
                        AccessLinkLine = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700),
                        NetworkLink_GUID = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F")
                    }
                };

            networkLink = new NetworkLinkDTO
            {
                Id = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F"),
                TOID = "osgb4000000023358315",
                NetworkLinkType_GUID = Guid.Parse("09CE57B1-AF13-4F8E-B4AF-1DE35B4A68A8"),
                LinkGeometry = DbGeometry.LineFromText("LINESTRING (488952 197048, 488895 197018, 488888 197014, 488880 197008)", 27700)
            };

            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            SqlGeometry networkIntersectionPoint = SqlGeometry.Null;
            List<SqlGeometry> lstnetworkIntersectionPoint = new List<SqlGeometry>();

            try
            {
                networkIntersectionPoint = accessLinkDTO[0].OperationalObjectPoint.ToSqlGeometry().ShortestLineTo(networkLink.LinkGeometry.ToSqlGeometry()).STEndPoint();
                lstnetworkIntersectionPoint.Add(networkIntersectionPoint);
            }
            catch (Exception)
            {
            }

            Tuple<NetworkLinkDTO, SqlGeometry> tuple = new Tuple<NetworkLinkDTO, SqlGeometry>(networkLink, networkIntersectionPoint);
            List<Tuple<NetworkLinkDTO, SqlGeometry>> tuple1 = new List<Tuple<NetworkLinkDTO, SqlGeometry>> { new Tuple<NetworkLinkDTO, SqlGeometry>(networkLink, networkIntersectionPoint) };

            List<ReferenceDataCategoryDTO> refDataCategotyDTO = new List<ReferenceDataCategoryDTO>()
            {
                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.AccessLinkType,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.AccessLinkTypeDefault,
                            ID = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F")
                        },

                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.UserDefined,
                            ID = Guid.Parse("DA8F1A91-2E2B-4EEF-9A81-9B18A917CBF1")
                        }
                    }
                },
                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.AccessLinkDirection,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.AccessLinkDirectionBoth,
                            ID = Guid.Parse("5DBA7B39-D23E-493A-9B8F-B94D181A082F")
                        }
                    }
                },
                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.AccessLinkStatus,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.AccessLinkStatusLive,
                            ID = Guid.Parse("6DBA7B39-D23E-493A-9B8F-B94D181A082F")
                        },
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.AccessLinkStatusDraftPendingApproval,
                            ID = Guid.Parse("7DBA7B39-D23E-493A-9B8F-B94D181A082F")
                        },
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.AccessLinkStatusDraftPendingReview,
                            ID = Guid.Parse("7B90B2F9-F62F-E711-8735-28D244AEF9ED")
                        }
                    }
                },
                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.OperationalObjectType,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = ReferenceDataValues.OperationalObjectTypeDP,
                            ReferenceDataValue = ReferenceDataValues.OperationalObjectTypeDP,
                            ID = Guid.Parse("415c9129-0615-457e-98b7-3a60436320c5")
                        }
                    }
                },
                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.AccessLinkParameters,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = ReferenceDataValues.AccessLinkSameRoadMaxDistance,
                            ReferenceDataValue = "25",
                            ID = Guid.Parse("9DBA7B39-D23E-493A-9B8F-B94D181A082F")
                        },
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = ReferenceDataValues.AccessLinkDiffRoadMaxDistance,
                            ReferenceDataValue = "1",
                            ID = Guid.Parse("3DBA7B39-D23E-493A-9B8F-B94D181A081F")
                        },
                    }
                },
                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.NetworkLinkType,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = ReferenceDataValues.AccessLinkDiffRoadMaxDistance,
                            ReferenceDataValue = "Road Link",
                            ID = Guid.Parse("09ce57b1-af13-4f8e-b4af-1de35b4a68a8")
                        }
                    }
                },
                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.AccessLinkParameters,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = "Local Road",
                            ReferenceDataValue = "1",
                            ID = Guid.Parse("4DBA7B35-D23E-493A-9B8F-B94D181A082F")
                        }
                    }
                },
                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = "DeliveryPointUseIndicator",
                            ReferenceDataValue = "Residential",
                            ID = Guid.Parse("178edcad-9431-e711-83ec-28d244aef9ed")
                        }
                    }
                },
            };

            loggingHelperMock = CreateMock<ILoggingHelper>();

            // Setup Methods.
            mockaccessLinkDataService = new Mock<IAccessLinkDataService>();
            mockAccessLinkIntegrationService = CreateMock<IAccessLinkIntegrationService>();
            mockaccessLinkDataService.Setup(x => x.GetAccessLinks(It.IsAny<string>(), It.IsAny<Guid>())).Returns(It.IsAny<List<AccessLinkDataDTO>>);
            mockaccessLinkDataService.Setup(x => x.GetAccessLinksCrossingOperationalObject(It.IsAny<string>(), It.IsAny<DbGeometry>())).Returns(default(bool));
            mockaccessLinkDataService.Setup(x => x.DeleteAccessLink(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            mockAccessLinkIntegrationService.Setup(x => x.GetReferenceDataNameValuePairs(It.IsAny<List<string>>())).ReturnsAsync(new List<ReferenceDataCategoryDTO>() { });
            mockAccessLinkIntegrationService.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).ReturnsAsync(refDataCategotyDTO);
            mockAccessLinkIntegrationService.Setup(x => x.GetDeliveryPoint(It.IsAny<Guid>())).ReturnsAsync(deliveryPointDTO);
            mockAccessLinkIntegrationService.Setup(x => x.GetNearestNamedRoad(It.IsAny<DbGeometry>(), It.IsAny<string>())).ReturnsAsync(tuple);
            mockAccessLinkIntegrationService.Setup(x => x.GetNearestSegment(It.IsAny<DbGeometry>())).ReturnsAsync(tuple1);
            mockAccessLinkIntegrationService.Setup(x => x.GetOSRoadLink(It.IsAny<string>())).ReturnsAsync("Local Road");
            mockAccessLinkIntegrationService.Setup(x => x.GetCrossingNetworkLinks(It.IsAny<string>(), It.IsAny<DbGeometry>())).ReturnsAsync(new List<NetworkLinkDTO>() { });
            mockAccessLinkIntegrationService.Setup(x => x.GetDeliveryPointsCrossingOperationalObject(It.IsAny<string>(), It.IsAny<DbGeometry>())).ReturnsAsync(new List<DeliveryPointDTO>() { });
            mockAccessLinkIntegrationService.Setup(x => x.GetNetworkLink(It.IsAny<Guid>())).ReturnsAsync(networkLink);

            mockaccessLinkDataService.Setup(x => x.CreateAccessLink(It.IsAny<AccessLinkDataDTO>())).Returns(true);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new AccessLinkBusinessService(mockaccessLinkDataService.Object, loggingHelperMock.Object, mockAccessLinkIntegrationService.Object);
        }
    }
}