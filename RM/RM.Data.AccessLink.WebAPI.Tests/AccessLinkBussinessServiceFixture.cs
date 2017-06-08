﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Microsoft.SqlServer.Types;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.AccessLink.WebAPI.BusinessService;
using RM.DataManagement.AccessLink.WebAPI.BusinessService.Interface;
using RM.DataManagement.AccessLink.WebAPI.Integration;

namespace RM.Data.AccessLink.WebAPI.Test
{
    [TestFixture]
    public class AccessLinkBussinessServiceFixture : TestFixtureBase
    {
        private IAccessLinkBusinessService testCandidate;

        private Mock<IAccessLinkDataService> mockaccessLinkRepository;

        private Mock<ILoggingHelper> loggingHelperMock;
        private List<AccessLinkDTO> accessLinkDTO = null;
        private DeliveryPointDTO deliveryPointDTO = null;
        private Guid operationalObjectId = System.Guid.NewGuid();
        private Guid operationObjectTypeId = new Guid("415c9129-0615-457e-98b7-3a60436320c5");

        [Test]
        public void Test_GetAccessLinks()
        {
            string coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";
            Guid unitGuid = Guid.NewGuid();
            var result = testCandidate.GetAccessLinks(coordinates, unitGuid);
            mockaccessLinkRepository.Verify(x => x.GetAccessLinks(It.IsAny<string>(), It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void Test_CreateAccessLink()
        {
            bool expectedResult = testCandidate.CreateAccessLink(operationalObjectId, operationObjectTypeId);
            Assert.IsTrue(expectedResult);
        }

        protected override void OnSetup()
        {
            deliveryPointDTO = new DeliveryPointDTO
            {
                LocationXY = DbGeometry.PointFromText("POINT(512722.70000000019 104752.6799999997)", 27700),
                Latitude = (decimal)50.83133590,
                Longitude = (decimal)-0.40071150,
                Positioned = true,
                UDPRN = 2364534,
                PostalAddress = new PostalAddressDTO
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
                        OperationalObjectPoint = DbGeometry.PointFromText("POINT (512722.70000000019 104752.6799999997)", 27700),
                        NetworkIntersectionPoint = DbGeometry.PointFromText("POINT (512722.70000000019 104738)", 27700),
                        AccessLinkLine = DbGeometry.LineFromText("LINESTRING (512722.70000000019 104752.6799999997, 512722.70000000019 104738)", 27700),
                        NetworkLink_GUID = Guid.Parse("BC3E8414-DA95-4924-9C0D-B8D343C97E0A")
                    }
                };

            NetworkLinkDTO networkLink = new NetworkLinkDTO()
            {
                Id = Guid.Parse("BC3E8414-DA95-4924-9C0D-B8D343C97E0A"),
                TOID = "osgb4000000030107256",
                NetworkLinkType_GUID = Guid.Parse("09CE57B1-AF13-4F8E-B4AF-1DE35B4A68A8"),
                LinkGeometry = DbGeometry.LineFromText("LINESTRING (512796 104738, 512703 104738, 512642 104738)", 27700)
            };

            SqlGeometry networkIntersectionPoint = SqlGeometry.Null;
            try
            {
                networkIntersectionPoint = accessLinkDTO[0].OperationalObjectPoint.ToSqlGeometry().ShortestLineTo(networkLink.LinkGeometry.ToSqlGeometry()).STEndPoint();
            }
            catch (Exception ex)
            {
            }

            Tuple<NetworkLinkDTO, SqlGeometry> tuple = new Tuple<NetworkLinkDTO, SqlGeometry>(networkLink, networkIntersectionPoint);

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
                            ID = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F")
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
                            ID = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F")
                        },
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.AccessLinkStatusDraftPendingApproval,
                            ID = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F")
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
                            ReferenceDataValue = "1",
                            ID = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F")
                        },
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = ReferenceDataValues.AccessLinkDiffRoadMaxDistance,
                            ReferenceDataValue = "1",
                            ID = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F")
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
                            ID = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F")
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
                            ID = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F")
                        }
                    }
                },
            };

            mockaccessLinkRepository = new Mock<IAccessLinkDataService>();
            mockaccessLinkRepository.Setup(x => x.GetAccessLinks(It.IsAny<string>(), It.IsAny<Guid>())).Returns(It.IsAny<List<AccessLinkDTO>>);

            loggingHelperMock = new Mock<ILoggingHelper>();

            // loggingHelperMock.Setup(x => x.RMTraceManager).Returns(CreateMock<IRMTraceManager>().Object);

            testCandidate = new AccessLinkBusinessService(mockaccessLinkRepository.Object, loggingHelperMock.Object, new Mock<IAccessLinkIntegrationService>().Object);
        }
    }
}