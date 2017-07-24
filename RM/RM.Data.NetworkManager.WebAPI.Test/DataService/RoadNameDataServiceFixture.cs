using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.Entities;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Implementation;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.DataServices.Tests.DataService
{
    /// <summary>
    /// This class contains test methods for RoadNameDataService.
    /// </summary>
    public class RoadNameDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<NetworkDBContext> mockNetworkDBContext;
        private Mock<IDatabaseFactory<NetworkDBContext>> mockDatabaseFactory;
        private IRoadNameDataService testCandidate;
        private Mock<ILoggingHelper> mockILoggingHelper;
        private List<ReferenceDataCategoryDTO> refDataCategotyDTO = null;
        private string coordinates;
        private Guid unit1Guid = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid unit2Guid = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid unit3Guid = new Guid("0A852795-03C1-432D-8DE6-70BB4820BD1A");
        private Guid user1Id;
        private Guid user2Id;

        /// <summary>
        /// Test for fetch Road routes data.
        /// </summary>
        [Test]
        public void Test_GetRoadRoutes()
        {
            coordinates = "POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))";
            var actualResult = testCandidate.GetRoadRoutes(coordinates, unit1Guid, refDataCategotyDTO);
            Assert.IsNotNull(actualResult);
        }

        /// <summary>
        /// Setup for Nunit Tests.
        /// </summary>
        protected override void OnSetup()
        {
            unit1Guid = Guid.NewGuid();
            unit2Guid = Guid.NewGuid();
            unit3Guid = Guid.NewGuid();
            user1Id = System.Guid.NewGuid();
            user2Id = System.Guid.NewGuid();

            var unitBoundary = DbGeometry.PolygonFromText("POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))", 27700);
            var Location = new List<Location>() { new Location() { ID = unit1Guid, Shape = unitBoundary } };
            var roadName = new List<OSRoadLink>()
            {
               new OSRoadLink()
               {
                  CentreLineGeometry = unitBoundary,
                  RoadName = "Road 001"
                      }
            };

            refDataCategotyDTO = new List<ReferenceDataCategoryDTO>()
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

            var networkLinks = new List<NetworkLink>()
            {
                new NetworkLink()
                {
                    ID = Guid.Parse("09ce57b1-af13-4f8e-b4af-1de35b4a68a8"),
                    TOID = "osgb4000000023358315",
                    LinkGeometry =  DbGeometry.LineFromText("LINESTRING (511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933)", 27700),
                    NetworkLinkTypeGUID = new Guid("09ce57b1-af13-4f8e-b4af-1de35b4a68a8")

                }
            };

            // Setup for OSRoadLink.
            mockNetworkDBContext = CreateMock<NetworkDBContext>();
            var mockRoadNameDataService = MockDbSet(roadName);
            mockNetworkDBContext.Setup(x => x.Set<OSRoadLink>()).Returns(mockRoadNameDataService.Object);
            mockNetworkDBContext.Setup(x => x.OSRoadLinks).Returns(mockRoadNameDataService.Object);
            mockNetworkDBContext.Setup(c => c.OSRoadLinks.AsNoTracking()).Returns(mockRoadNameDataService.Object);
            mockRoadNameDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockRoadNameDataService.Object);

            //Setup for Location.
            var mockRoadNameDataService2 = MockDbSet(Location);
            mockNetworkDBContext.Setup(x => x.Set<Location>()).Returns(mockRoadNameDataService2.Object);
            mockNetworkDBContext.Setup(x => x.Locations).Returns(mockRoadNameDataService2.Object);
            mockNetworkDBContext.Setup(c => c.Locations.AsNoTracking()).Returns(mockRoadNameDataService2.Object);
            mockRoadNameDataService2.Setup(x => x.Include(It.IsAny<string>())).Returns(mockRoadNameDataService2.Object);

            //Setup for NetworkLink.
            var mockNetworkLink = MockDbSet(networkLinks);
            mockNetworkDBContext.Setup(x => x.Set<NetworkLink>()).Returns(mockNetworkLink.Object);
            mockNetworkDBContext.Setup(x => x.NetworkLinks).Returns(mockNetworkLink.Object);
            mockNetworkDBContext.Setup(c => c.NetworkLinks.AsNoTracking()).Returns(mockNetworkLink.Object);
            mockNetworkLink.Setup(x => x.Include(It.IsAny<string>())).Returns(mockNetworkLink.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<NetworkDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockNetworkDBContext.Object);

            //Setup for IRMTraceManager.
            mockILoggingHelper = CreateMock<ILoggingHelper>();
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockILoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new RoadNameDataService(mockDatabaseFactory.Object, mockILoggingHelper.Object);
        }
    }
}