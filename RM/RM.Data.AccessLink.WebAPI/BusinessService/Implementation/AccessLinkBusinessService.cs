using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.AccessLink.WebAPI.Utils;
using RM.DataManagement.AccessLink.WebAPI.Integration;
using RM.DataManagement.AccessLink.WebAPI.DTO;

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
            using (loggingHelper.RMTraceManager.StartTrace("Business.CalculateWorkloadLength"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodEntryEventId, LoggerTraceConstants.Title);

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

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodExitEventId, LoggerTraceConstants.Title);
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
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                bool isAccessLinkCreated = false;

                object operationalObject = new object();

                List<string> categoryNamesNameValuePairs = new List<string>
            {
                ReferenceDataCategoryNames.AccessLinkParameters,
            };

                List<string> categoryNamesSimpleLists = new List<string>
            {
                ReferenceDataCategoryNames.OperationalObjectType,
                ReferenceDataCategoryNames.AccessLinkDirection,
                ReferenceDataCategoryNames.AccessLinkStatus,
                ReferenceDataCategoryNames.AccessLinkType,
                ReferenceDataCategoryNames.NetworkLinkType,
                ReferenceDataCategoryNames.DeliveryPointUseIndicator
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

                    // if the delivery point is not positioned then return failure
                    if (!deliveryPointOperationalObject.Positioned)
                    {
                        return false;
                    }

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
                NetworkLinkDTO networkLink = nearestNamedStreetNetworkObjectWithIntersectionTuple?.Item1;
                SqlGeometry networkIntersectionPoint = nearestNamedStreetNetworkObjectWithIntersectionTuple?.Item2;

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
                    Tuple<NetworkLinkDTO, SqlGeometry> nearestStreetNetworkObjectWithIntersectionTuple =
                        accessLinkIntegrationService.GetNearestSegment(operationalObjectPoint).Result;

                    networkLink = nearestStreetNetworkObjectWithIntersectionTuple?.Item1;
                    networkIntersectionPoint = nearestStreetNetworkObjectWithIntersectionTuple?.Item2;
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
                    accessLinkDto.OperationalObjectType_GUID = operationObjectTypeId;
                    accessLinkDto.Approved = accessLinkApproved;
                    if (referenceDataCategoryList
                            .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.OperationalObjectType)
                            .SelectMany(x => x.ReferenceDatas)
                            .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID == operationObjectTypeId)
                    {
                        DeliveryPointDTO deliveryPointDto = (DeliveryPointDTO)operationalObject;
                        accessLinkDto.WorkloadLengthMeter = Convert.ToDecimal(CalculateWorkloadLength(deliveryPointDto, actualLength, networkLink, referenceDataCategoryList));
                    }

                    accessLinkDto.AccessLinkType_GUID = referenceDataCategoryList
                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkType)
                        .SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == accessLinkType).ID;

                    accessLinkDto.LinkDirection_GUID = referenceDataCategoryList
                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkDirection)
                        .SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == accessLinkDirection).ID;

                    accessLinkDto.LinkStatus_GUID = referenceDataCategoryList
                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkStatus)
                        .SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == accessLinkStatus).ID;

                    // create access link
                    isAccessLinkCreated = accessLinkDataService.CreateAccessLink(accessLinkDto);

                    if (isAccessLinkCreated)
                    {
                        if (referenceDataCategoryList
                                .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.OperationalObjectType)
                                .SelectMany(x => x.ReferenceDatas)
                                .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID ==
                            operationObjectTypeId)
                        {
                            DeliveryPointDTO deliveryPointDto = (DeliveryPointDTO)operationalObject;
                            deliveryPointDto.AccessLinkPresent = true;
                            accessLinkIntegrationService.UpdateDeliveryPointAccessLinkCreationStatus(deliveryPointDto);
                        }
                    }
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodExitEventId, LoggerTraceConstants.Title);
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
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                bool isAccessLinkCreated = false;

                string accessLinkLineManual = ObjectParser.GetGeometry(accessLinkManualDto.AccessLinkLine, AccessLinkConstants.LinestringObject);
                string operationalObjectPointManual = ObjectParser.GetGeometry(accessLinkManualDto.OperationalObjectPoint, AccessLinkConstants.PointObject);
                string networkIntersectionPointManual = ObjectParser.GetGeometry(accessLinkManualDto.NetworkIntersectionPoint, AccessLinkConstants.PointObject);
                Guid operationalObjectGuidManual = Guid.Parse(accessLinkManualDto.OperationalObjectGUID);
                Guid networkLinkGuidManual = Guid.Parse(accessLinkManualDto.NetworkLinkGUID);

                AccessLinkDTO accessLinkDto = new AccessLinkDTO
                {
                    ID = Guid.Empty,
                    AccessLinkLine = DbGeometry.LineFromText(accessLinkLineManual, AccessLinkConstants.BNGCOORDINATESYSTEM),
                    NetworkIntersectionPoint = DbGeometry.PointFromText(networkIntersectionPointManual, AccessLinkConstants.BNGCOORDINATESYSTEM),
                    NetworkLink_GUID = networkLinkGuidManual,
                    OperationalObjectPoint = DbGeometry.PointFromText(operationalObjectPointManual, AccessLinkConstants.BNGCOORDINATESYSTEM), // need to write logic
                    OperationalObject_GUID = operationalObjectGuidManual,
                    OperationalObjectType_GUID = Guid.Empty,
                    Approved = true,
                    WorkloadLengthMeter = accessLinkManualDto.Workloadlength,
                    AccessLinkType_GUID = Guid.Empty,
                    LinkDirection_GUID = Guid.Empty,
                    LinkStatus_GUID = Guid.Empty,
                };

                object operationalObject = new object();

                List<string> categoryNamesNameValuePairs = new List<string>
            {
                ReferenceDataCategoryNames.AccessLinkParameters,
            };

                List<string> categoryNamesSimpleLists = new List<string>
            {
                ReferenceDataCategoryNames.OperationalObjectType,
                ReferenceDataCategoryNames.AccessLinkDirection,
                ReferenceDataCategoryNames.AccessLinkStatus,
                ReferenceDataCategoryNames.AccessLinkType,
                ReferenceDataCategoryNames.NetworkLinkType,
                ReferenceDataCategoryNames.DeliveryPointUseIndicator
            };

                var referenceDataCategoryList =
                    accessLinkIntegrationService.GetReferenceDataNameValuePairs(categoryNamesNameValuePairs).Result;

                referenceDataCategoryList.AddRange(
                   accessLinkIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result);

                string roadName = string.Empty;

                accessLinkDto.OperationalObjectType_GUID = referenceDataCategoryList
                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.OperationalObjectType).SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID;

                var deliveryPointOperationalObject = accessLinkIntegrationService.GetDeliveryPoint(operationalObjectGuidManual).Result;
                accessLinkDto.OperationalObjectPoint = deliveryPointOperationalObject.LocationXY;
                operationalObject = deliveryPointOperationalObject;

                accessLinkDto.AccessLinkType_GUID = referenceDataCategoryList
                            .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkType).SelectMany(x => x.ReferenceDatas)
                            .Single(x => x.ReferenceDataValue == ReferenceDataValues.UserDefined).ID;

                accessLinkDto.LinkDirection_GUID = referenceDataCategoryList
                    .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkDirection).SelectMany(x => x.ReferenceDatas)
                    .Single(x => x.ReferenceDataValue == ReferenceDataValues.AccessLinkDirectionBoth).ID;

                accessLinkDto.LinkStatus_GUID = referenceDataCategoryList
                    .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.AccessLinkStatus).SelectMany(x => x.ReferenceDatas)
                    .Single(x => x.ReferenceDataValue == ReferenceDataValues.AccessLinkStatusDraftPendingReview).ID;

                NetworkLinkDTO networkObject = accessLinkIntegrationService.GetNetworkLink(accessLinkDto.NetworkLink_GUID).Result;

                accessLinkDto.ActualLengthMeter = Convert.ToDecimal((double)accessLinkDto.AccessLinkLine.ToSqlGeometry().STLength());

                // create access link
                isAccessLinkCreated = accessLinkDataService.CreateAccessLink(accessLinkDto);

                if (isAccessLinkCreated)
                {
                    if (referenceDataCategoryList
                   .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.OperationalObjectType).SelectMany(x => x.ReferenceDatas)
                   .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID == accessLinkDto.OperationalObjectType_GUID)
                    {
                        DeliveryPointDTO deliveryPointDto = (DeliveryPointDTO)operationalObject;
                        deliveryPointDto.AccessLinkPresent = true;
                        accessLinkIntegrationService.UpdateDeliveryPointAccessLinkCreationStatus(deliveryPointDto);
                    }
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodExitEventId, LoggerTraceConstants.Title);
                return isAccessLinkCreated;
            }
        }

        /// <summary>
        /// This method fetches data for AccsessLinks
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <param name="unitGuid">Unit unique identifier.</param>
        /// <returns>AccsessLink object</returns>
        public string GetAccessLinks(string boundaryBox, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetAccessLinks"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                string accessLinkJsonData = null;

                if (!string.IsNullOrEmpty(boundaryBox))
                {
                    var accessLinkCoordinates =
                        GetAccessLinkCoordinatesDataByBoundingBox(boundaryBox.Split(AccessLinkConstants.Comma[0]));
                    accessLinkJsonData =
                        GetAccessLinkJsonData(accessLinkDataService.GetAccessLinks(accessLinkCoordinates, unitGuid));
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodExitEventId, LoggerTraceConstants.Title);
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
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                string accessLinkLineManual = ObjectParser.GetGeometry(accessLinkManualDto.AccessLinkLine, AccessLinkConstants.LinestringObject);
                string operationalObjectPointManual = ObjectParser.GetGeometry(accessLinkManualDto.OperationalObjectPoint, AccessLinkConstants.PointObject);
                string networkIntersectionPointManual = ObjectParser.GetGeometry(accessLinkManualDto.NetworkIntersectionPoint, AccessLinkConstants.PointObject);
                Guid operationalObjectGuidManual = Guid.Parse(accessLinkManualDto.OperationalObjectGUID);
                Guid networkLinkGuidManual = Guid.Parse(accessLinkManualDto.NetworkLinkGUID);
                string roadName = string.Empty;

                AccessLinkDTO accessLinkDto = new AccessLinkDTO
                {
                    ID = Guid.Empty,
                    AccessLinkLine = DbGeometry.LineFromText(accessLinkLineManual, AccessLinkConstants.BNGCOORDINATESYSTEM),
                    ActualLengthMeter = Convert.ToDecimal(1.00),
                    NetworkIntersectionPoint = DbGeometry.PointFromText(networkIntersectionPointManual, AccessLinkConstants.BNGCOORDINATESYSTEM),
                    NetworkLink_GUID = networkLinkGuidManual,
                    OperationalObjectPoint = DbGeometry.PointFromText(operationalObjectPointManual, AccessLinkConstants.BNGCOORDINATESYSTEM),
                    OperationalObject_GUID = operationalObjectGuidManual,
                    OperationalObjectType_GUID = Guid.Empty,
                    Approved = true,
                    WorkloadLengthMeter = default(decimal),
                    AccessLinkType_GUID = Guid.Empty,
                    LinkDirection_GUID = Guid.Empty,
                    LinkStatus_GUID = Guid.Empty,
                };

                object operationalObject = new object();

                List<string> categoryNamesNameValuePairs = new List<string>
            {
                ReferenceDataCategoryNames.AccessLinkParameters,
            };

                List<string> categoryNamesSimpleLists = new List<string>
            {
                ReferenceDataCategoryNames.OperationalObjectType,
                ReferenceDataCategoryNames.AccessLinkDirection,
                ReferenceDataCategoryNames.AccessLinkStatus,
                ReferenceDataCategoryNames.AccessLinkType,
                ReferenceDataCategoryNames.NetworkLinkType,
                ReferenceDataCategoryNames.DeliveryPointUseIndicator
            };

                var referenceDataCategoryList =
                    accessLinkIntegrationService.GetReferenceDataNameValuePairs(categoryNamesNameValuePairs).Result;

                referenceDataCategoryList.AddRange(
                   accessLinkIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result);

                accessLinkDto.OperationalObjectType_GUID = referenceDataCategoryList
                        .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.OperationalObjectType).SelectMany(x => x.ReferenceDatas)
                        .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID;

                // get the delivery point data based on the guid.
                var deliveryPointOperationalObject = accessLinkIntegrationService.GetDeliveryPoint(operationalObjectGuidManual).Result;
                accessLinkDto.OperationalObjectPoint = deliveryPointOperationalObject.LocationXY;

                operationalObject = deliveryPointOperationalObject;

                // Get the network link object where the access link terminates
                NetworkLinkDTO networkObject = accessLinkIntegrationService.GetNetworkLink(accessLinkDto.NetworkLink_GUID).Result;

                // calculate the actual length in meters for the access link
                accessLinkDto.ActualLengthMeter = Convert.ToDecimal((double)accessLinkDto.AccessLinkLine.ToSqlGeometry().STLength());

                if (referenceDataCategoryList
                  .Where(x => x.CategoryName.Replace(AccessLinkConstants.Space, string.Empty) == ReferenceDataCategoryNames.OperationalObjectType).SelectMany(x => x.ReferenceDatas)
                  .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID == accessLinkDto.OperationalObjectType_GUID)
                {
                    DeliveryPointDTO deliveryPointDto = (DeliveryPointDTO)operationalObject;

                    // calculate the work load length in meters from the operational object to the n/w link
                    accessLinkDto.WorkloadLengthMeter = Convert.ToDecimal(CalculateWorkloadLength(deliveryPointDto, (double)accessLinkDto.ActualLengthMeter, networkObject, referenceDataCategoryList));
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodExitEventId, LoggerTraceConstants.Title);
                return accessLinkDto.WorkloadLengthMeter;
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
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                string parsedAccessLink = ObjectParser.GetGeometry(accessLinkCoordinates, AccessLinkConstants.LinestringObject);

                DbGeometry accessLink = DbGeometry.LineFromText(parsedAccessLink, AccessLinkConstants.BNGCOORDINATESYSTEM);
                string formattedBoundaryCoordinates = GetAccessLinkCoordinatesDataByBoundingBox(boundingBoxCoordinates.Replace(AccessLinkConstants.OpenSquareBracket, string.Empty).Replace(AccessLinkConstants.CloseSquareBracket, string.Empty).Split(AccessLinkConstants.Comma[0]));
                List<AccessLinkDTO> accessLinkDTOs = accessLinkDataService.GetAccessLinksCrossingOperationalObject(formattedBoundaryCoordinates, accessLink);
                List<NetworkLinkDTO> networkLinkDTOs = accessLinkIntegrationService.GetCrossingNetworkLinks(formattedBoundaryCoordinates, accessLink).Result;
                List<DeliveryPointDTO> deliveryPointDTOs = accessLinkIntegrationService.GetDeliveryPointsCrossingOperationalObject(formattedBoundaryCoordinates, accessLink).Result;

                if (accessLinkDTOs.Count > 0 || networkLinkDTOs.Count > 0 || deliveryPointDTOs.Count > 0)
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodExitEventId, LoggerTraceConstants.Title);
                    return false;
                }
                else
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkBusinessMethodExitEventId, LoggerTraceConstants.Title);
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

        #endregion Methods
    }
}