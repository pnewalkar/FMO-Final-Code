using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.AccessLink.WebAPI.DataDTOs;
using RM.Data.AccessLink.WebAPI.Utils;
using RM.DataManagement.AccessLink.WebAPI.DataService.Interfaces;
using RM.DataManagement.AccessLink.WebAPI.DTOs;
using RM.DataManagement.AccessLink.WebAPI.Integration;

namespace RM.DataManagement.AccessLink.WebAPI.BusinessService
{
    /// <summary>
    /// This class contains methods related to Access Links.
    /// </summary>
    public class AccessLinkBusinessService : Interface.IAccessLinkBusinessService
    {
        #region Member Variables

        private IAccessLinkDataService accessLinkDataService = default(IAccessLinkDataService);
        private IAccessLinkIntegrationService accessLinkIntegrationService = default(IAccessLinkIntegrationService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        private int priority = LoggerTraceConstants.AccessLinkAPIPriority;
        private int entryEventId = LoggerTraceConstants.AccessLinkBusinessMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.AccessLinkBusinessMethodExitEventId;

        #endregion Member Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessLinkBusinessService"/> class.
        /// </summary>
        /// <param name="accessLinkDataService">The access link DataService.</param>
        /// <param name="deliveryPointsDataService">The delivery points DataService.</param>
        /// <param name="loggingHelper">The logging helper.</param>
        /// <param name="accessLinkIntegrationService">The accessLink Integration Service.</param>
        public AccessLinkBusinessService(IAccessLinkDataService accessLinkDataService, ILoggingHelper loggingHelper, IAccessLinkIntegrationService accessLinkIntegrationService)
        {
            this.accessLinkDataService = accessLinkDataService;
            this.loggingHelper = loggingHelper;
            this.accessLinkIntegrationService = accessLinkIntegrationService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Calculate CalculateWorkloadLength
        /// </summary>
        /// <param name="pointDto">Delivery Point DTO object</param>
        /// <param name="actualLength">
        /// Actual Distance between two objects calculated by geometry function
        /// </param>
        /// <param name="networkObject">NetworkLink which is linked with access link</param>
        /// <returns>double</returns>
        public double CalculateWorkloadLength(DeliveryPointDTO pointDto, double actualLength, NetworkLinkDTO networkObject, List<ReferenceDataCategoryDTO> referenceDataCategoryList)
        {
            // TODO
            using (loggingHelper.RMTraceManager.StartTrace("Business.CalculateWorkloadLength"))
            {
                string methodName = typeof(AccessLinkBusinessService) + "." + nameof(CalculateWorkloadLength);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                double workloadLengthMeter = 0;
                double roadWidth = 0;

                // network link type whether it is road, path or connecting link
                string networkLinkType = referenceDataCategoryList
                                                .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.NetworkLinkType).SelectMany(x => x.ReferenceDatas)
                                                .Where(x => x.ID == networkObject.NetworkLinkType_GUID)
                                                .Select(x => x.ReferenceDataValue).SingleOrDefault();

                if (networkLinkType == ReferenceDataValues.NetworkLinkRoadLink)
                {
                    // get road type such as A road, B Road
                    string roadType = accessLinkIntegrationService.GetOSRoadLink(networkObject.TOID).Result;

                    roadWidth = Convert.ToDouble(referenceDataCategoryList
                                            .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkParameters).SelectMany(x => x.ReferenceDatas)
                                            .Where(x => x.ReferenceDataName.TrimEnd(AccessLinkConstants.LF, AccessLinkConstants.NL).Equals(roadType, StringComparison.OrdinalIgnoreCase))
                                            .Select(x => x.ReferenceDataValue).SingleOrDefault());
                }
                else if (networkLinkType == ReferenceDataValues.NetworkLinkPathLink)
                {
                    roadWidth = Convert.ToDouble(referenceDataCategoryList
                                            .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkParameters).SelectMany(x => x.ReferenceDatas)
                                            .Where(x => x.ReferenceDataName.TrimEnd(AccessLinkConstants.LF, AccessLinkConstants.NL).Equals(ReferenceDataValues.PathLink, StringComparison.OrdinalIgnoreCase))
                                            .Select(x => x.ReferenceDataValue).SingleOrDefault());
                }
                else
                {
                    throw new Exception(AccessLinkConstants.NWLinkTypeException);
                }

                // get pavement depth from reference data.
                double pavementDepth = Convert.ToDouble(referenceDataCategoryList
                                                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkParameters).SelectMany(x => x.ReferenceDatas)
                                                        .Where(x => x.ReferenceDataName.TrimEnd(AccessLinkConstants.LF, AccessLinkConstants.NL).Equals(ReferenceDataValues.PavementWidth, StringComparison.OrdinalIgnoreCase))
                                                        .Select(x => x.ReferenceDataValue).SingleOrDefault());

                // get house depth from reference data
                double houseDepth = Convert.ToDouble(referenceDataCategoryList
                                                    .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkParameters).SelectMany(x => x.ReferenceDatas)
                                                    .Where(x => x.ReferenceDataName.TrimEnd(AccessLinkConstants.LF, AccessLinkConstants.NL).Equals(ReferenceDataValues.PropertyDepth, StringComparison.OrdinalIgnoreCase))
                                                    .Select(x => x.ReferenceDataValue).SingleOrDefault());

                // selected dp is Residential or commercial
                string dpUseIndicatorType = referenceDataCategoryList
                                                    .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.DeliveryPointUseIndicator).SelectMany(x => x.ReferenceDatas)
                                                    .Where(x => x.ID == pointDto.DeliveryPointUseIndicator_GUID)
                                                    .Select(x => x.ReferenceDataValue).SingleOrDefault();

                if (dpUseIndicatorType == ReferenceDataValues.Residential)
                {
                    double residentialRoadWidthMultFactor = Convert.ToDouble(referenceDataCategoryList
                                                    .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkParameters).SelectMany(x => x.ReferenceDatas)
                                                    .Where(x => x.ReferenceDataName.TrimEnd(AccessLinkConstants.LF, AccessLinkConstants.NL).Equals(ReferenceDataValues.ResidentialRoadWidthMultiplicationFactor, StringComparison.OrdinalIgnoreCase))
                                                    .Select(x => x.ReferenceDataValue).SingleOrDefault());
                    double residentialPavementWidthMultFactor = Convert.ToDouble(referenceDataCategoryList
                                                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkParameters).SelectMany(x => x.ReferenceDatas)
                                                        .Where(x => x.ReferenceDataName.TrimEnd(AccessLinkConstants.LF, AccessLinkConstants.NL) == ReferenceDataValues.ResidentialPavementWidthMultiplicationFactor)
                                                        .Select(x => x.ReferenceDataValue).SingleOrDefault());
                    double residentialHouseDepthMultFactor = Convert.ToDouble(referenceDataCategoryList
                                                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkParameters).SelectMany(x => x.ReferenceDatas)
                                                        .Where(x => x.ReferenceDataName.TrimEnd(AccessLinkConstants.LF, AccessLinkConstants.NL) == ReferenceDataValues.ResidentialHouseDepthMultiplicationFactor)
                                                        .Select(x => x.ReferenceDataValue).SingleOrDefault());

                    workloadLengthMeter = actualLength -
                                                    (residentialRoadWidthMultFactor * roadWidth) -
                                                    (residentialPavementWidthMultFactor * pavementDepth) -
                                                    (residentialHouseDepthMultFactor * houseDepth);
                }
                else if (dpUseIndicatorType == ReferenceDataValues.Organisation)
                {
                    double businessRoadWidthMultFactor = Convert.ToDouble(referenceDataCategoryList
                                                    .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkParameters).SelectMany(x => x.ReferenceDatas)
                                                    .Where(x => x.ReferenceDataName.TrimEnd(AccessLinkConstants.LF, AccessLinkConstants.NL) == ReferenceDataValues.BusinessRoadWidthMultiplicationFactor)
                                                    .Select(x => x.ReferenceDataValue).SingleOrDefault());
                    double businessPavementWidthMultFactor = Convert.ToDouble(referenceDataCategoryList
                                                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkParameters).SelectMany(x => x.ReferenceDatas)
                                                        .Where(x => x.ReferenceDataName.TrimEnd(AccessLinkConstants.LF, AccessLinkConstants.NL) == ReferenceDataValues.BusinessPavementWidthMultiplicationFactor)
                                                        .Select(x => x.ReferenceDataValue).SingleOrDefault());

                    workloadLengthMeter = actualLength -
                                                    (businessRoadWidthMultFactor * roadWidth) -
                                                    (businessPavementWidthMultFactor * pavementDepth);
                }
                else
                {
                    throw new Exception(AccessLinkConstants.DPUseIndicatorTypeException);
                }

                if (workloadLengthMeter <= 0)
                {
                    workloadLengthMeter = 1;
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return workloadLengthMeter;
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
            using (loggingHelper.RMTraceManager.StartTrace("Business.CreateAutomaticAccessLink"))
            {
                string methodName = typeof(AccessLinkBusinessService) + "." + nameof(CreateAccessLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                bool isAccessLinkCreated = false;
                object operationalObject = new object();

                List<string> categoryNamesNameValuePairs = new List<string>
            {
                ReferenceDataCategoryNames.AccessLinkParameters,
            };

                List<string> categoryNamesSimpleLists = new List<string>
            {
                    ReferenceDataCategoryNames.DataProvider,
                ReferenceDataCategoryNames.OperationalObjectType,
                ReferenceDataCategoryNames.AccessLinkDirection,
                ReferenceDataCategoryNames.AccessLinkStatus,
                ReferenceDataCategoryNames.AccessLinkType, // access link type (NLNodetype)
                ReferenceDataCategoryNames.NetworkLinkType,
                ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                ReferenceDataCategoryNames.NetworkNodeType
            };

                DbGeometry operationalObjectPoint = default(DbGeometry);
                string roadName = string.Empty;
                var referenceDataCategoryList =
                    accessLinkIntegrationService.GetReferenceDataNameValuePairs(categoryNamesNameValuePairs).Result;

                referenceDataCategoryList.AddRange(
                   accessLinkIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result);

                // Get details for the OO
                if (referenceDataCategoryList
                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.OperationalObjectType)
                        .SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID ==
                    operationObjectTypeId)
                {
                    var deliveryPointOperationalObject = accessLinkIntegrationService.GetDeliveryPoint(operationalObjectId).Result;

                    operationalObjectPoint = deliveryPointOperationalObject.LocationXY;
                    roadName = deliveryPointOperationalObject.PostalAddress.Thoroughfare;
                    operationalObject = deliveryPointOperationalObject;
                }

                double actualLength = 0;
                bool matchFound = false;
                string accessLinkStatus = string.Empty;
                string accessLinkDirection = string.Empty;
                string accessLinkType = string.Empty;
                bool accessLinkApproved = false;

                // get actual length threshold.

                // Rule 1. Named road is within threshold limit and there are no intersections with any
                // other roads
                Tuple<NetworkLinkDTO, SqlGeometry> nearestNamedStreetNetworkObjectWithIntersectionTuple =
                    accessLinkIntegrationService.GetNearestNamedRoad(operationalObjectPoint, roadName).Result;

                SqlGeometry accesslink = nearestNamedStreetNetworkObjectWithIntersectionTuple?.Item2;
                NetworkLinkDTO networkLink = null;
                SqlGeometry networkIntersectionPoint = null;
                if (accesslink != null)
                {
                    // Condition to check any intersection in the delivery point
                    var intersectionCountForDeliveryPoint = accessLinkDataService.GetIntersectionCountForDeliveryPoint(operationalObjectPoint, accesslink.ToDbGeometry());

                    // Condition to check any Crosses or overlaps in Access link
                    var isAccessLinkCrossesorOverLaps = accessLinkDataService.CheckAccessLinkCrossesorOverLaps(operationalObjectPoint, accesslink.ToDbGeometry());

                    if (intersectionCountForDeliveryPoint == 0 && !isAccessLinkCrossesorOverLaps)
                    {
                        networkLink = nearestNamedStreetNetworkObjectWithIntersectionTuple?.Item1;
                        networkIntersectionPoint = nearestNamedStreetNetworkObjectWithIntersectionTuple?.Item2;
                    }
                }

                if (networkLink != null && networkIntersectionPoint != null && !networkIntersectionPoint.IsNull && !networkIntersectionPoint.STX.IsNull && !networkIntersectionPoint.STY.IsNull)
                {
                    actualLength =
                        (double)operationalObjectPoint.ToSqlGeometry()
                            .ShortestLineTo(
                                nearestNamedStreetNetworkObjectWithIntersectionTuple.Item1.LinkGeometry.ToSqlGeometry())
                            .STLength();

                    var accessLinkSameRoadMaxDistance = Convert.ToInt32(referenceDataCategoryList
                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkParameters)
                        .SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataName == ReferenceDataValues.AccessLinkSameRoadMaxDistance)
                        .ReferenceDataValue);

                    // check if the matched named road is withing the threshold defined.
                    matchFound = actualLength <= accessLinkSameRoadMaxDistance;

                    accessLinkStatus = ReferenceDataValues.AccessLinkStatusLive;
                    accessLinkDirection = ReferenceDataValues.AccessLinkDirectionBoth;
                    accessLinkType = ReferenceDataValues.AccessLinkTypeDefault;
                    accessLinkApproved = true;
                }
                else
                {
                    // Rule 2. - look for any path or road if there is any other road other than the
                    // return the road segment object and the access link intersection point
                    Tuple<NetworkLinkDTO, List<SqlGeometry>> nearestStreetNetworkObjectWithIntersectionTuple =
                        accessLinkIntegrationService.GetNearestSegment(operationalObjectPoint).Result;
                    var networkLikdDTO = nearestStreetNetworkObjectWithIntersectionTuple?.Item1;

                    // var accessLinks = operationalObjectPoint.ToSqlGeometry().ShortestLineTo(networkLikdDTO.LinkGeometry.ToSqlGeometry());
                    foreach (var interscetionPoint in nearestStreetNetworkObjectWithIntersectionTuple?.Item2)
                    {
                        if (interscetionPoint != null)
                        {
                            var intersectionCountForDeliveryPoint = accessLinkDataService.GetIntersectionCountForDeliveryPoint(operationalObjectPoint, interscetionPoint.ToDbGeometry());
                            var isAccessLinkCrossesorOverLaps = accessLinkDataService.CheckAccessLinkCrossesorOverLaps(operationalObjectPoint, interscetionPoint.ToDbGeometry());

                            if (intersectionCountForDeliveryPoint == 0 && !isAccessLinkCrossesorOverLaps)
                            {
                                networkLink = nearestStreetNetworkObjectWithIntersectionTuple?.Item1;
                                networkIntersectionPoint = interscetionPoint.STEndPoint();
                                break;
                            }
                        }
                    }

                    if (networkLink != null && !networkIntersectionPoint.IsNull && !networkIntersectionPoint.STX.IsNull && !networkIntersectionPoint.STY.IsNull)
                    {
                        actualLength = (double)operationalObjectPoint.ToSqlGeometry()
                            .ShortestLineTo(networkLink.LinkGeometry.ToSqlGeometry()).STLength();

                        var accessLinkDiffRoadMaxDistance = Convert.ToInt32(referenceDataCategoryList
                            .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkParameters)
                            .SelectMany(x => x.ReferenceDatas)
                            .Single(x => x.ReferenceDataName == ReferenceDataValues.AccessLinkDiffRoadMaxDistance)
                            .ReferenceDataValue);

                        // check if the matched segment is within the threshold defined.
                        matchFound = actualLength <= accessLinkDiffRoadMaxDistance;

                        accessLinkStatus = ReferenceDataValues.AccessLinkStatusDraftPendingReview;
                        accessLinkDirection = ReferenceDataValues.AccessLinkDirectionBoth;
                        accessLinkType = ReferenceDataValues.AccessLinkTypeDefault;
                    }
                }

                if (matchFound)
                {
                    Guid locationGuid = Guid.NewGuid();
                    Guid accessLinkGuid = Guid.NewGuid();
                    Guid networkNodeTypeGuid = Guid.NewGuid();

                    AccessLinkDataDTO accessLinkDataDto = new AccessLinkDataDTO();

                    // Create Location
                    accessLinkDataDto.NetworkLink.NetworkNode.Location.ID = locationGuid;
                    accessLinkDataDto.NetworkLink.NetworkNode.Location.Shape = networkIntersectionPoint.ToDbGeometry();
                    accessLinkDataDto.NetworkLink.NetworkNode.Location.RowCreateDateTime = DateTime.UtcNow;

                    // Create NetworkNode
                    accessLinkDataDto.NetworkLink.NetworkNode.ID = locationGuid;
                    accessLinkDataDto.NetworkLink.NetworkNode.DataProviderGUID = GetReferenceData(referenceDataCategoryList, ReferenceDataCategoryNames.DataProvider, AccessLinkConstants.Internal, true);

                    accessLinkDataDto.NetworkLink.NetworkNode.NetworkNodeType_GUID = GetReferenceData(referenceDataCategoryList, ReferenceDataCategoryNames.NetworkNodeType, AccessLinkConstants.AccessLinkDataProviderGUID, true);
                    accessLinkDataDto.NetworkLink.NetworkNode.RowCreateDateTime = DateTime.UtcNow;

                    // Create AccessLink
                    accessLinkDataDto.ID = accessLinkGuid;
                    accessLinkDataDto.Approved = accessLinkApproved;
                    if (referenceDataCategoryList
                            .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.OperationalObjectType)
                            .SelectMany(x => x.ReferenceDatas)
                            .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID == operationObjectTypeId)
                    {
                        DeliveryPointDTO deliveryPointDto = (DeliveryPointDTO)operationalObject;
                        accessLinkDataDto.WorkloadLengthMeter = Convert.ToDecimal(CalculateWorkloadLength(deliveryPointDto, actualLength, networkLink, referenceDataCategoryList));
                    }

                    accessLinkDataDto.AccessLinkTypeGUID = referenceDataCategoryList
                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkType)
                        .SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == accessLinkType).ID;

                    accessLinkDataDto.LinkDirectionGUID = referenceDataCategoryList
                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkDirection)
                        .SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == accessLinkDirection).ID;
                    accessLinkDataDto.ConnectedNetworkLinkID = networkLink.Id;
                    accessLinkDataDto.RowCreateDateTime = DateTime.UtcNow;

                    // Create NetworkLink
                    accessLinkDataDto.NetworkLink.ID = accessLinkGuid;
                    accessLinkDataDto.NetworkLink.DataProviderGUID = GetReferenceData(referenceDataCategoryList, ReferenceDataCategoryNames.DataProvider, AccessLinkConstants.Internal, true);
                    accessLinkDataDto.NetworkLink.NetworkLinkTypeGUID = GetReferenceData(referenceDataCategoryList, ReferenceDataCategoryNames.NetworkLinkType, AccessLinkConstants.AccessLink, true);
                    accessLinkDataDto.NetworkLink.LinkGeometry =
                        operationalObjectPoint.ToSqlGeometry()
                            .ShortestLineTo(networkLink.LinkGeometry.ToSqlGeometry())
                            .ToDbGeometry();
                    accessLinkDataDto.NetworkLink.LinkLength = Convert.ToDecimal(actualLength);
                    accessLinkDataDto.NetworkLink.StartNodeID = operationalObjectId;
                    accessLinkDataDto.NetworkLink.EndNodeID = locationGuid;
                    accessLinkDataDto.NetworkLink.RowCreateDateTime = DateTime.UtcNow;

                    // create AccessLinkStatus
                    AccessLinkStatusDataDTO accessLinkStatusDataDTO = new AccessLinkStatusDataDTO
                    {
                        ID = Guid.NewGuid(),
                        AccessLinkStatusGUID = referenceDataCategoryList
                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkStatus)
                        .SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == accessLinkStatus).ID,
                        RowCreateDateTime = DateTime.UtcNow,
                        NetworkLinkID = accessLinkGuid,
                        StartDateTime = DateTime.UtcNow
                    };

                    accessLinkDataDto.AccessLinkStatus.Add(accessLinkStatusDataDTO);

                    // calling dataservice to save AccessLink.
                    isAccessLinkCreated = accessLinkDataService.CreateAccessLink(accessLinkDataDto);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return isAccessLinkCreated;
            }
        }

        /// <summary>
        /// Create manual access link creation after delivery point creation.
        /// </summary>
        /// <param name="accessLinkManualDto">
        /// create modal for manual access link object to be stored
        /// </param>
        /// <returns>bool</returns>
        public bool CreateAccessLink(AccessLinkManualCreateModelDTO accessLinkManualDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.CreateManualAccessLink"))
            {
                string methodName = typeof(AccessLinkBusinessService) + "." + nameof(CreateAccessLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                bool isAccessLinkCreated = false;

                string accessLinkLineManual = ObjectParser.GetGeometry(accessLinkManualDto.AccessLinkLine, AccessLinkConstants.LinestringObject);
                string operationalObjectPointManual = ObjectParser.GetGeometry(accessLinkManualDto.OperationalObjectPoint, AccessLinkConstants.PointObject);
                string networkIntersectionPointManual = ObjectParser.GetGeometry(accessLinkManualDto.NetworkIntersectionPoint, AccessLinkConstants.PointObject);
                Guid operationalObjectGuidManual = Guid.Parse(accessLinkManualDto.OperationalObjectGUID);
                Guid networkLinkGuidManual = Guid.Parse(accessLinkManualDto.NetworkLinkGUID);

                object operationalObject = new object();
                List<string> categoryNamesNameValuePairs = new List<string>
            {
                ReferenceDataCategoryNames.AccessLinkParameters,
            };

                List<string> categoryNamesSimpleLists = new List<string>
            {
                    ReferenceDataCategoryNames.DataProvider,
                ReferenceDataCategoryNames.OperationalObjectType,
                ReferenceDataCategoryNames.AccessLinkDirection,
                ReferenceDataCategoryNames.AccessLinkStatus,
                ReferenceDataCategoryNames.AccessLinkType,
                ReferenceDataCategoryNames.NetworkLinkType,
                ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                ReferenceDataCategoryNames.NetworkNodeType
            };

                var referenceDataCategoryList =
                    accessLinkIntegrationService.GetReferenceDataNameValuePairs(categoryNamesNameValuePairs).Result;

                referenceDataCategoryList.AddRange(
                   accessLinkIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result);

                string roadName = string.Empty;

                Guid operationalObjectTypeGUID = referenceDataCategoryList
                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.OperationalObjectType).SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID;

                var deliveryPointOperationalObject = accessLinkIntegrationService.GetDeliveryPoint(operationalObjectGuidManual).Result;
                DbGeometry operationalObjectPoint = deliveryPointOperationalObject.LocationXY;
                operationalObject = deliveryPointOperationalObject;

                Guid accessLinkTypeGUID = referenceDataCategoryList
                            .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkType).SelectMany(x => x.ReferenceDatas)
                            .Single(x => x.ReferenceDataValue == ReferenceDataValues.UserDefined).ID;

                Guid linkDirectionGUID = referenceDataCategoryList
                   .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkDirection).SelectMany(x => x.ReferenceDatas)
                   .Single(x => x.ReferenceDataValue == ReferenceDataValues.AccessLinkDirectionBoth).ID;

                Guid linkStatusGUID = referenceDataCategoryList
                    .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkStatus).SelectMany(x => x.ReferenceDatas)
                    .Single(x => x.ReferenceDataValue == ReferenceDataValues.AccessLinkStatusDraftPendingReview).ID;

                NetworkLinkDTO networkObject = accessLinkIntegrationService.GetNetworkLink(networkLinkGuidManual).Result;

                decimal actualLengthMeter = Convert.ToDecimal((double)DbGeometry.LineFromText(accessLinkLineManual, AccessLinkConstants.BNGCOORDINATESYSTEM).ToSqlGeometry().STLength());

                // Rahul New manual access link creation
                Guid locationGuid = Guid.NewGuid();
                Guid accessLinkGuid = Guid.NewGuid();
                Guid networkNodeTypeGuid = Guid.NewGuid();

                AccessLinkDataDTO accessLinkDataDto = new AccessLinkDataDTO();

                // Create Location
                accessLinkDataDto.NetworkLink.NetworkNode.Location.ID = locationGuid;
                accessLinkDataDto.NetworkLink.NetworkNode.Location.Shape = DbGeometry.PointFromText(networkIntersectionPointManual, AccessLinkConstants.BNGCOORDINATESYSTEM);
                accessLinkDataDto.NetworkLink.NetworkNode.Location.RowCreateDateTime = DateTime.UtcNow;

                // Create NetworkNode
                accessLinkDataDto.NetworkLink.NetworkNode.ID = locationGuid;
                accessLinkDataDto.NetworkLink.NetworkNode.DataProviderGUID = GetReferenceData(referenceDataCategoryList, ReferenceDataCategoryNames.DataProvider, AccessLinkConstants.Internal, true);

                accessLinkDataDto.NetworkLink.NetworkNode.NetworkNodeType_GUID = GetReferenceData(referenceDataCategoryList, ReferenceDataCategoryNames.NetworkNodeType, AccessLinkConstants.AccessLinkDataProviderGUID, true);
                accessLinkDataDto.NetworkLink.NetworkNode.RowCreateDateTime = DateTime.UtcNow;

                // Create AccessLink
                accessLinkDataDto.ID = accessLinkGuid;
                accessLinkDataDto.Approved = true;
                accessLinkDataDto.WorkloadLengthMeter = accessLinkManualDto.Workloadlength;

                accessLinkDataDto.AccessLinkTypeGUID = accessLinkTypeGUID;

                accessLinkDataDto.LinkDirectionGUID = linkDirectionGUID;
                accessLinkDataDto.ConnectedNetworkLinkID = networkLinkGuidManual;
                accessLinkDataDto.RowCreateDateTime = DateTime.UtcNow;

                // Create NetworkLink
                accessLinkDataDto.NetworkLink.ID = accessLinkGuid;
                accessLinkDataDto.NetworkLink.DataProviderGUID = GetReferenceData(referenceDataCategoryList, ReferenceDataCategoryNames.DataProvider, AccessLinkConstants.Internal, true); // ReferenceDataValue(Internal) CategoryName(Data Provider)
                accessLinkDataDto.NetworkLink.NetworkLinkTypeGUID = GetReferenceData(referenceDataCategoryList, ReferenceDataCategoryNames.NetworkLinkType, AccessLinkConstants.AccessLink, true); // ReferenceDataValue(Access Link) CategoryName(Network Link Type)
                accessLinkDataDto.NetworkLink.LinkGeometry = DbGeometry.LineFromText(accessLinkLineManual, AccessLinkConstants.BNGCOORDINATESYSTEM);
                accessLinkDataDto.NetworkLink.LinkLength = Convert.ToDecimal(actualLengthMeter);
                accessLinkDataDto.NetworkLink.StartNodeID = operationalObjectGuidManual;
                accessLinkDataDto.NetworkLink.EndNodeID = locationGuid;
                accessLinkDataDto.NetworkLink.RowCreateDateTime = DateTime.UtcNow;

                // create AccessLinkStatus
                AccessLinkStatusDataDTO accessLinkStatusDataDTO = new AccessLinkStatusDataDTO
                {
                    ID = Guid.NewGuid(),
                    AccessLinkStatusGUID = linkStatusGUID,
                    RowCreateDateTime = DateTime.UtcNow,
                    NetworkLinkID = accessLinkGuid,
                    StartDateTime = DateTime.UtcNow
                };

                accessLinkDataDto.AccessLinkStatus.Add(accessLinkStatusDataDTO);

                // calling dataservice for creating accesslink  using networklink parameter.
                isAccessLinkCreated = accessLinkDataService.CreateAccessLink(accessLinkDataDto);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return isAccessLinkCreated;
            }
        }

        /// <summary>
        /// This method fetches data for AccsessLinks  ------Refactored this method 13-07-2017.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <param name="unitGuid">Unit unique identifier.</param>
        /// <returns>AccsessLink object</returns>
        public string GetAccessLinks(string boundaryBox, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetAccessLinks"))
            {
                string methodName = typeof(AccessLinkBusinessService) + "." + nameof(GetAccessLinks);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string accessLinkJsonData = null;

                if (!string.IsNullOrEmpty(boundaryBox))
                {
                    var accessLinkCoordinates = GetAccessLinkCoordinatesDataByBoundingBox(boundaryBox.Split(AccessLinkConstants.Comma[0]));
                    var accessLinkDataDto = accessLinkDataService.GetAccessLinks(accessLinkCoordinates, unitGuid);
                    var accessLink = GenericMapper.MapList<AccessLinkDataDTO, AccessLinkDTO>(accessLinkDataDto);
                    accessLinkJsonData = GetAccessLinkJsonData(accessLinkDataDto);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return accessLinkJsonData;
            }
        }

        /// <summary> This method is used to calculate path length. </summary> <param
        /// name="accessLinkManualDto">access link input required to calculate path length</param>
        /// <returns>returns calculated path length as <double>.</true></returns>
        public decimal GetAdjPathLength(AccessLinkManualCreateModelDTO accessLinkManualDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetAdjPathLength"))
            {
                string methodName = typeof(AccessLinkBusinessService) + "." + nameof(GetAdjPathLength);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string accessLinkLineManual = ObjectParser.GetGeometry(accessLinkManualDto.AccessLinkLine, AccessLinkConstants.LinestringObject);
                string operationalObjectPointManual = ObjectParser.GetGeometry(accessLinkManualDto.OperationalObjectPoint, AccessLinkConstants.PointObject);
                string networkIntersectionPointManual = ObjectParser.GetGeometry(accessLinkManualDto.NetworkIntersectionPoint, AccessLinkConstants.PointObject);
                Guid operationalObjectGuidManual = Guid.Parse(accessLinkManualDto.OperationalObjectGUID);
                Guid networkLinkGuidManual = Guid.Parse(accessLinkManualDto.NetworkLinkGUID);
                string roadName = string.Empty;
                decimal workloadLengthMeter = default(decimal);

                object operationalObject = new object();

                List<string> categoryNamesNameValuePairs = new List<string>
                {
                    ReferenceDataCategoryNames.AccessLinkParameters,
                };

                List<string> categoryNamesSimpleLists = new List<string>
                {
                    ReferenceDataCategoryNames.DataProvider,
                    ReferenceDataCategoryNames.OperationalObjectType,
                    ReferenceDataCategoryNames.AccessLinkDirection,
                    ReferenceDataCategoryNames.AccessLinkStatus,
                    ReferenceDataCategoryNames.AccessLinkType,
                    ReferenceDataCategoryNames.NetworkLinkType,
                    ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                    ReferenceDataCategoryNames.NetworkNodeType
                };

                var referenceDataCategoryList =
                    accessLinkIntegrationService.GetReferenceDataNameValuePairs(categoryNamesNameValuePairs).Result;

                referenceDataCategoryList.AddRange(
                   accessLinkIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result);

                Guid operationalObjectTypeGUID = referenceDataCategoryList
                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.OperationalObjectType).SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID;

                // get the delivery point data based on the guid.
                var deliveryPointOperationalObject = accessLinkIntegrationService.GetDeliveryPoint(operationalObjectGuidManual).Result;
                DbGeometry operationalObjectPoint = deliveryPointOperationalObject.LocationXY;

                operationalObject = deliveryPointOperationalObject;

                // Get the network link object where the access link terminates
                NetworkLinkDTO networkObject = accessLinkIntegrationService.GetNetworkLink(networkLinkGuidManual).Result;

                // calculate the actual length in meters for the access link
                decimal actualLengthMeter = Convert.ToDecimal((double)DbGeometry.LineFromText(accessLinkLineManual, AccessLinkConstants.BNGCOORDINATESYSTEM).ToSqlGeometry().STLength());

                if (referenceDataCategoryList
                  .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.OperationalObjectType).SelectMany(x => x.ReferenceDatas)
                  .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID == operationalObjectTypeGUID)
                {
                    DeliveryPointDTO deliveryPointDto = (DeliveryPointDTO)operationalObject;

                    // calculate the work load length in meters from the operational object to the n/w link
                    workloadLengthMeter = Convert.ToDecimal(CalculateWorkloadLength(deliveryPointDto, (double)actualLengthMeter, networkObject, referenceDataCategoryList));
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return workloadLengthMeter;
            }
        }

        /// <summary>
        /// This method is used to check whether an access link is valid
        /// </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param>
        /// <param name="accessLinkCoordinates">access link coordinate array</param>
        /// <returns>bool</returns>
        public bool CheckManualAccessLinkIsValid(string boundingBoxCoordinates, string accessLinkCoordinates)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.CheckManualAccessLinkIsValid"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string parsedAccessLink = ObjectParser.GetGeometry(accessLinkCoordinates, AccessLinkConstants.LinestringObject);

                DbGeometry accessLink = DbGeometry.LineFromText(parsedAccessLink, AccessLinkConstants.BNGCOORDINATESYSTEM);
                string formattedBoundaryCoordinates = GetAccessLinkCoordinatesDataByBoundingBox(boundingBoxCoordinates.Replace(AccessLinkConstants.OpenSquareBracket, string.Empty).Replace(AccessLinkConstants.CloseSquareBracket, string.Empty).Split(AccessLinkConstants.Comma[0]));
                bool isAccessLinkCrossing = accessLinkDataService.GetAccessLinksCrossingOperationalObject(formattedBoundaryCoordinates, accessLink);
                bool isNetworkLinkCrossing = accessLinkDataService.GetCrossingNetworkLink(formattedBoundaryCoordinates, accessLink);
                bool isDeliveryPointCrossing = accessLinkDataService.GetDeliveryPointsCrossingOperationalObject(formattedBoundaryCoordinates, accessLink);

                if ((!isAccessLinkCrossing) || (!isNetworkLinkCrossing) || (!isDeliveryPointCrossing))
                {
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return false;
                }
                else
                {
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return true;
                }
            }
        }

        /// <summary>
        /// This is a helper method which fetches the co-ordinates of accesslink
        /// </summary>
        /// <param name="accessLinkParameters">accessLinkParameters as object</param>
        /// <returns>accesslink coordinates</returns>
        private static string GetAccessLinkCoordinatesDataByBoundingBox(params object[] accessLinkParameters)
        {
            string coordinates = string.Empty;

            if (accessLinkParameters != null && accessLinkParameters.Length == 4)
            {
                coordinates = string.Format(
                              AccessLinkConstants.Polygon,
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

        /// <summary>
        /// This method fetches geojson data for access link
        /// </summary>
        /// <param name="lstAccessLinkDTO">accesslink as list of AccessLinkDTO</param>
        /// <returns>AccsessLink object</returns>
        private static string GetAccessLinkJsonData(List<AccessLinkDataDTO> lstAccessLinkDTO)
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

                    geometry.type = res.NetworkLink.LinkGeometry.SpatialTypeName;

                    var resultCoordinates = res.NetworkLink.LinkGeometry;

                    SqlGeometry accessLinksqlGeometry = null;
                    if (geometry.type == Convert.ToString(GeometryType.LineString))
                    {
                        accessLinksqlGeometry = SqlGeometry.STLineFromWKB(new SqlBytes(resultCoordinates.AsBinary()), AccessLinkConstants.BNGCOORDINATESYSTEM).MakeValid();

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
                        accessLinksqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), AccessLinkConstants.BNGCOORDINATESYSTEM).MakeValid();
                        geometry.coordinates = new double[] { accessLinksqlGeometry.STX.Value, accessLinksqlGeometry.STY.Value };
                    }

                    Feature feature = new Feature();
                    feature.geometry = geometry;

                    feature.type = AccessLinkConstants.FeatureType;
                    feature.id = res.ID.ToString();
                    feature.properties = new Dictionary<string, Newtonsoft.Json.Linq.JToken> { { AccessLinkConstants.LayerType, Convert.ToString(OtherLayersType.AccessLink.GetDescription()) } };

                    geoJson.features.Add(feature);
                }
            }

            return JsonConvert.SerializeObject(geoJson);
        }

        /// <summary>
        /// Get the Reference data for the categorynames based on the referencedata value
        /// </summary>
        /// <param name="categoryNamesSimpleLists">list of category names</param>
        /// <param name="categoryName">category name</param>
        /// <param name="referenceDataValue">reference data value</param>
        /// <param name="isWithSpace"> whether </param>
        /// <returns></returns>
        private Guid GetReferenceData(List<ReferenceDataCategoryDTO> referenceDataCategoryList, string categoryName, string referenceDataValue, bool isWithSpace = false)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetReferenceData"))
            {
                string methodName = typeof(AccessLinkBusinessService) + "." + nameof(GetReferenceData);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                Guid referenceDataGuid = Guid.Empty;
                if (isWithSpace)
                {
                    referenceDataGuid = referenceDataCategoryList
                                       .Where(list => list.CategoryName.Replace(" ", string.Empty) == categoryName)
                                       .SelectMany(list => list.ReferenceDatas)
                                       .Where(item => item.ReferenceDataValue.Equals(referenceDataValue, StringComparison.OrdinalIgnoreCase))
                                       .Select(s => s.ID).SingleOrDefault();
                }
                else
                {
                    referenceDataGuid = referenceDataCategoryList
                                    .Where(list => list.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
                                    .SelectMany(list => list.ReferenceDatas)
                                    .Where(item => item.ReferenceDataValue.Equals(referenceDataValue, StringComparison.OrdinalIgnoreCase))
                                    .Select(s => s.ID).SingleOrDefault();
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return referenceDataGuid;
            }
        }


        /// <summary>
        /// Method to delete access link once Delivery point deleted.
        /// </summary>
        /// <param name="operationalObjectId"></param>
        /// <returns></returns>
        public Task<bool> DeleteAccessLink(Guid operationalObjectId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.DeleteAccessLink"))
            {
                string methodName = typeof(AccessLinkBusinessService) + "." + nameof(DeleteAccessLink);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodEntryEventId);

                List<string> categoryNamesNameValuePairs = new List<string>
            {
                ReferenceDataCategoryNames.AccessLinkParameters,
            };

                List<string> categoryNamesSimpleLists = new List<string>
            {

                ReferenceDataCategoryNames.AccessLinkType, // access link type (NLNodetype)
                ReferenceDataCategoryNames.NetworkLinkType//NetworkLinkType to check whether accesslink,road link,path link          
            };

                var referenceDataCategoryList =
                    accessLinkIntegrationService.GetReferenceDataNameValuePairs(categoryNamesNameValuePairs).Result;

                referenceDataCategoryList.AddRange(
                   accessLinkIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result);

                Guid networkLinkTypeGUID = GetReferenceData(referenceDataCategoryList, ReferenceDataCategoryNames.NetworkLinkType, AccessLinkConstants.AccessLink, true);
                Guid accessLinkTypeGUID = GetReferenceData(referenceDataCategoryList, ReferenceDataCategoryNames.AccessLinkType, ReferenceDataValues.AccessLinkTypeDefault, true);
                var returnValue = accessLinkDataService.DeleteAccessLink(operationalObjectId, networkLinkTypeGUID, accessLinkTypeGUID);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodEntryEventId);

                return returnValue;
            }

        }

        #endregion Methods
    }
}