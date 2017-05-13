using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Linq;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common;
using Fmo.Common.Constants;
using Fmo.Common.Enums;
using Fmo.Common.Interface;
using Fmo.Common.SqlGeometryExtension;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Helpers;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;

namespace Fmo.BusinessServices.Services
{
    /// <summary>
    /// This class contains methods for fetching data for AccessLinks
    /// </summary>
    public class AccessLinkBusinessService : IAccessLinkBusinessService
    {
        private IAccessLinkRepository accessLinkRepository = default(IAccessLinkRepository);
        private IReferenceDataCategoryRepository referenceDataCategoryRepository = default(IReferenceDataCategoryRepository);
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);
        private IStreetNetworkBusinessService streetNetworkBusinessService = default(IStreetNetworkBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public AccessLinkBusinessService(IAccessLinkRepository accessLinkRepository,
            IReferenceDataCategoryRepository referenceDataCategoryRepository,
            IDeliveryPointsRepository deliveryPointsRepository,
            IStreetNetworkBusinessService streetNetworkBusinessService,
            ILoggingHelper loggingHelper)
        {
            this.accessLinkRepository = accessLinkRepository;
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
            this.deliveryPointsRepository = deliveryPointsRepository;
            this.streetNetworkBusinessService = streetNetworkBusinessService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        ///  This method fetches data for AccsessLinks
        /// </summary>
        /// <param name="boundaryBox"> boundaryBox as string </param>
        /// <param name="unitGuid">Unit unique identifier.</param>
        /// <returns> AccsessLink object</returns>
        public string GetAccessLinks(string boundaryBox, Guid unitGuid)
        {
            try
            {
                string accessLinkJsonData = null;

                if (!string.IsNullOrEmpty(boundaryBox))
                {
                    var accessLinkCoordinates = GetAccessLinkCoordinatesDataByBoundingBox(boundaryBox.Split(Constants.Comma[0]));
                    accessLinkJsonData = GetAccessLinkJsonData(accessLinkRepository.GetAccessLinks(accessLinkCoordinates, unitGuid));
                }

                return accessLinkJsonData;
            }
            catch (Exception)
            {
                throw;
            }
        }

      /// <summary>
        /// Create automatic access link creation after delivery point creation.
        /// </summary>
        /// <param name="operationalObjectId">Operational Object unique identifier.</param>
        /// <param name="operationObjectTypeId">Operational Object type unique identifier.</param>
        /// <returns>bool</returns>
        public bool CreateAccessLink(Guid operationalObjectId, Guid operationObjectTypeId)
        {
            bool isAccessLinkCreated = false;
            try
            {
                // TODO: Move all the reference data service calls to respective service.
                object operationalObject = new object();

                List<string> categoryNames = new List<string>
                {
                    "Operational Object Type",
                    "Access Link Direction",
                    "Access Link Status",
                    "Access Link Type",
                    "Access Link Rules"
                };

                DbGeometry operationalObjectPoint = default(DbGeometry);
                string roadName = string.Empty;
                var referenceDataCategoryList =
                    referenceDataCategoryRepository.GetReferenceDataCategoriesByCategoryNames(categoryNames);

                // Get delivery point name for the OO
                if (referenceDataCategoryList
                        .Where(x => x.CategoryName == "Operational Object Type").SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == "DP").ID == operationObjectTypeId)
                {
                    var deliveryPointOperationalObject = deliveryPointsRepository.GetDeliveryPoint(operationalObjectId);
                    operationalObjectPoint = deliveryPointOperationalObject.LocationXY;
                    roadName = deliveryPointOperationalObject.PostalAddress.Thoroughfare;

                    operationalObject = deliveryPointOperationalObject;
                }

                double actualLength = 0;
                bool matchFound = false;
                string accessLinkStatus = string.Empty;
                string accessLinkType = string.Empty;

                // get actual length threshold.

                // Rule 1. Named road is within threshold limit.
                // getNearestNamedRoad must ensure that if it finds any named roads,
                // then there are no intersections with any other roads and will
                // return the road segment object and the access link intersection point
                Tuple<NetworkLinkDTO, SqlGeometry> nearestNamedStreetNetworkObjectWithIntersectionTuple =
                    streetNetworkBusinessService.GetNearestNamedRoadForOperationalObject(operationalObjectPoint, roadName);
                NetworkLinkDTO networkLink = nearestNamedStreetNetworkObjectWithIntersectionTuple.Item1;
                SqlGeometry networkIntersectionPoint = nearestNamedStreetNetworkObjectWithIntersectionTuple.Item2;

                if (networkLink != null && networkIntersectionPoint != SqlGeometry.Null)
                {
                    actualLength =
                        (double)operationalObjectPoint.ToSqlGeometry()
                            .ShortestLineTo(
                                nearestNamedStreetNetworkObjectWithIntersectionTuple.Item1.LinkGeometry.ToSqlGeometry())
                            .STLength();

                    matchFound = actualLength <= 40;
                    accessLinkStatus = "Live";
                    accessLinkType = "Both";
                }
                else
                {
                    // Rule 2. - look for any path or road
                    // if there is any other road other than the
                    // return the road segment object and the access link intersection point
                    Tuple<NetworkLinkDTO, SqlGeometry> nearestStreetNetworkObjectWithIntersectionTuple =
                        streetNetworkBusinessService.GetNearestRoadForOperationalObject(operationalObjectPoint);

                    networkLink = nearestNamedStreetNetworkObjectWithIntersectionTuple.Item1;
                    networkIntersectionPoint = nearestNamedStreetNetworkObjectWithIntersectionTuple.Item2;
                    if (networkLink != null && networkIntersectionPoint != SqlGeometry.Null)
                    {
                        actualLength =
                            (double)operationalObjectPoint.ToSqlGeometry()
                                .ShortestLineTo(
                                    nearestStreetNetworkObjectWithIntersectionTuple.Item1.LinkGeometry.ToSqlGeometry())
                                .STLength();
                        matchFound = actualLength <= 40;

                        accessLinkStatus = "Draft Pending Review";
                        accessLinkType = "Both";
                    }
                }

                if (matchFound)
                {
                    AccessLinkDTO accessLinkDto = new AccessLinkDTO();
                    accessLinkDto.ID = Guid.Empty;
                    accessLinkDto.AccessLinkLine =
                        operationalObjectPoint.ToSqlGeometry()
                            .ShortestLineTo(networkLink.LinkGeometry.ToSqlGeometry())
                            .ToDbGeometry();
                    accessLinkDto.ActualLengthMeter = Convert.ToDecimal(actualLength);
                    accessLinkDto.NetworkIntersectionPoint = networkIntersectionPoint.ToDbGeometry();
                    accessLinkDto.NetworkLink_GUID = networkLink.Id;
                    accessLinkDto.OperationalObjectPoint = operationalObjectPoint;
                    accessLinkDto.OperationalObject_GUID = operationalObjectId;

                    // TODO: calculate access link work length here
                    accessLinkDto.AccessLinkType_GUID = referenceDataCategoryList
                        .Where(x => x.CategoryName == "Access Link Type").SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == "Default").ID;

                    accessLinkDto.LinkDirection_GUID = referenceDataCategoryList
                        .Where(x => x.CategoryName == "Access Link Direction").SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == accessLinkType).ID;

                    accessLinkDto.LinkStatus_GUID = referenceDataCategoryList
                        .Where(x => x.CategoryName == "Access Link Status").SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == accessLinkStatus).ID;

                    // create access link
                    isAccessLinkCreated = accessLinkRepository.CreateAccessLink(accessLinkDto);

                    if (isAccessLinkCreated)
                    {
                        if (referenceDataCategoryList
                       .Where(x => x.CategoryName == "Operational Object Type").SelectMany(x => x.ReferenceDatas)
                       .Single(x => x.ReferenceDataValue == "DP").ID == operationObjectTypeId)
                        {
                            DeliveryPointDTO deliveryPointDto = (DeliveryPointDTO)operationalObject;
                            deliveryPointDto.AccessLinkPresent = true;
                            deliveryPointsRepository.UpdateDeliveryPointAccessLinkCreationStatus(deliveryPointDto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loggingHelper.LogError(ex);
            }

            return isAccessLinkCreated;
        }


        /// <summary>
        /// This method fetches geojson data for accesslink
        /// </summary>
        /// <param name="lstAccessLinkDTO"> accesslink as list of AccessLinkDTO</param>
        /// <returns> AccsessLink object</returns>
        private static string GetAccessLinkJsonData(List<AccessLinkDTO> lstAccessLinkDTO)
        {
            var geoJson = new GeoJson
            {
                features = new List<Feature>()
            };
            if (lstAccessLinkDTO != null && lstAccessLinkDTO.Count > 0)
            {
                foreach (var res in lstAccessLinkDTO)
                {
                    Geometry geometry = new Geometry();

                    geometry.type = res.AccessLinkLine.SpatialTypeName;

                    var resultCoordinates = res.AccessLinkLine;

                    SqlGeometry accessLinksqlGeometry = null;
                    if (geometry.type == Convert.ToString(GeometryType.LineString))
                    {
                        accessLinksqlGeometry = SqlGeometry.STLineFromWKB(new SqlBytes(resultCoordinates.AsBinary()), Constants.BNGCOORDINATESYSTEM).MakeValid();

                        List<List<double>> cords = new List<List<double>>();

                        for (int pt = 1; pt <= accessLinksqlGeometry.STNumPoints().Value; pt++)
                        {
                            List<double> accessLinkCoordinates = new List<double> { accessLinksqlGeometry.STPointN(pt).STX.Value, accessLinksqlGeometry.STPointN(pt).STY.Value };
                            cords.Add(accessLinkCoordinates);
                        }

                        geometry.coordinates = cords;
                    }
                    else
                    {
                        accessLinksqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), Constants.BNGCOORDINATESYSTEM).MakeValid();
                        geometry.coordinates = new double[] { accessLinksqlGeometry.STX.Value, accessLinksqlGeometry.STY.Value };
                    }

                    Feature feature = new Feature();
                    feature.geometry = geometry;

                    feature.type = Constants.FeatureType;
                    feature.id = res.ID.ToString();
                    feature.properties = new Dictionary<string, Newtonsoft.Json.Linq.JToken> { { Constants.LayerType, Convert.ToString(OtherLayersType.AccessLink.GetDescription()) } };

                    geoJson.features.Add(feature);
                }
            }

            return JsonConvert.SerializeObject(geoJson);
        }

        /// <summary>
        /// This method fetches co-ordinates of accesslink
        /// </summary>
        /// <param name="accessLinkParameters"> accessLinkParameters as object </param>
        /// <returns> accesslink coordinates</returns>
        private static string GetAccessLinkCoordinatesDataByBoundingBox(params object[] accessLinkParameters)
        {
            string coordinates = string.Empty;

            if (accessLinkParameters != null && accessLinkParameters.Length == 4)
            {
                coordinates = string.Format(
                              Constants.Polygon,
                              Convert.ToString(accessLinkParameters[0]),
                              Convert.ToString(accessLinkParameters[1]),
                              Convert.ToString(accessLinkParameters[0]),
                              Convert.ToString(accessLinkParameters[3]),
                              Convert.ToString(accessLinkParameters[2]),
                              Convert.ToString(accessLinkParameters[3]),
                              Convert.ToString(accessLinkParameters[2]),
                              Convert.ToString(accessLinkParameters[1]),
                              Convert.ToString(accessLinkParameters[0]),
                              Convert.ToString(accessLinkParameters[1]));
            }

            return coordinates;
        }


    }
}