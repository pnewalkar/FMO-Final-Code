using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Linq;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO;
using RM.Data.DeliveryPointGroupManager.WebAPI.DTO;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.DataService;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.Integration;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.Utils;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.BusinessService
{
    public class DeliveryPointGroupBusinessService : IDeliveryPointGroupBusinessService
    {
        #region Member Variables

        private const string Comma = ", ";
        private const string Polygon = "POLYGON(({0} {1}, {2} {3}, {4} {5}, {6} {7}, {8} {9}))";

        private IDeliveryPointGroupDataService deliveryPointGroupDataService = default(IDeliveryPointGroupDataService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IDeliveryPointGroupIntegrationService deliveryPointGroupIntegrationService = default(IDeliveryPointGroupIntegrationService);

        private int priority = LoggerTraceConstants.DeliveryPointGroupManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryPointGroupBusinessServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryPointGroupBusinessServiceMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryPointGroupBusinessService"/> class.
        /// </summary>
        public DeliveryPointGroupBusinessService(
            IDeliveryPointGroupDataService deliveryPointGroupDataService,
            ILoggingHelper loggingHelper,
            IDeliveryPointGroupIntegrationService deliveryPointGroupIntegrationService)
        {
            this.deliveryPointGroupDataService = deliveryPointGroupDataService;
            this.loggingHelper = loggingHelper;
            this.deliveryPointGroupIntegrationService = deliveryPointGroupIntegrationService;
        }

        #endregion Constructors

        public string GetDeliveryPointGroups(string boundaryBox, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"Business.{nameof(GetDeliveryPointGroups)}"))
            {
                string methodName = typeof(DeliveryPointGroupBusinessService) + "." + nameof(GetDeliveryPointGroups);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string deliveryPointGroupJsonData = null;

                if (!string.IsNullOrEmpty(boundaryBox))
                {
                    var deliveryGroupCoordinates = GetGroupCoordinatesDataByBoundingBox(boundaryBox.Split(Comma[0]));
                    var deliveryGroups = deliveryPointGroupDataService.GetDeliveryGroups(deliveryGroupCoordinates, unitGuid);

                    deliveryPointGroupJsonData = GetDeliveryGroupsJsonData(deliveryGroups);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return deliveryPointGroupJsonData;
            }
        }

        public DeliveryPointGroupDTO GetDeliveryGroup(Guid deliveryGroupId)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"Business.{nameof(GetDeliveryGroup)}"))
            {
                string methodName = typeof(DeliveryPointGroupBusinessService) + "." + nameof(GetDeliveryGroup);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var deliveryGroup = ConvertToDTO(deliveryPointGroupDataService.GetDeliveryGroup(deliveryGroupId));

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return deliveryGroup;
            }
        }

        public DeliveryPointGroupDTO UpdateDeliveryGroup(DeliveryPointGroupDTO deliveryPointGroupDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"Business.{nameof(UpdateDeliveryGroup)}"))
            {
                //    List<string> categoryNamesSimpleLists = new List<string>
                //{
                //        ReferenceDataCategoryNames.DataProvider,
                //    ReferenceDataCategoryNames.OperationalObjectType,
                //    ReferenceDataCategoryNames.AccessLinkDirection,
                //    ReferenceDataCategoryNames.AccessLinkStatus,
                //    ReferenceDataCategoryNames.AccessLinkType, // access link type (NLNodetype)
                //    ReferenceDataCategoryNames.NetworkLinkType,
                //    ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                //    ReferenceDataCategoryNames.NetworkNodeType
                //};

                //    var referenceDataCategoryList =
                //       deliveryPointGroupIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;
                deliveryPointGroupDataService.UpdateDeliveryGroup(ConvertToDataDTO(deliveryPointGroupDto));
            }

            return deliveryPointGroupDto;
        }

        private static string GetGroupCoordinatesDataByBoundingBox(params object[] deliveryGroupParameters)
        {
            string coordinates = string.Empty;

            if (deliveryGroupParameters != null && deliveryGroupParameters.Length == 4)
            {
                coordinates = string.Format(
                              Polygon,
                              Convert.ToString(deliveryGroupParameters[0]),
                              Convert.ToString(deliveryGroupParameters[1]),
                              Convert.ToString(deliveryGroupParameters[0]),
                              Convert.ToString(deliveryGroupParameters[3]),
                              Convert.ToString(deliveryGroupParameters[2]),
                              Convert.ToString(deliveryGroupParameters[3]),
                              Convert.ToString(deliveryGroupParameters[2]),
                              Convert.ToString(deliveryGroupParameters[1]),
                              Convert.ToString(deliveryGroupParameters[0]),
                              Convert.ToString(deliveryGroupParameters[1]));
            }

            return coordinates;
        }

        /// <summary>
        /// /// <summary>
        /// Create Delivery group
        /// </summary>
        /// <param name="deliveryPointGroupDto">Public Dto to create Delivery group</param>
        /// <returns></returns>
        public DeliveryPointGroupDTO CreateDeliveryPointGroup(DeliveryPointGroupDTO deliveryPointGroupDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.CreateDeliveryPointGroup"))
            {
                string methodName = typeof(DeliveryPointGroupBusinessService) + "." + nameof(CreateDeliveryPointGroup);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (deliveryPointGroupDto == null)
                {
                    throw new ArgumentNullException(nameof(deliveryPointGroupDto), string.Format(ErrorConstants.Err_ArgumentmentNullException, deliveryPointGroupDto));
                }

                // Get reference data related to Delivery group
                List<string> categoryNamesSimpleLists = new List<string> { ReferenceDataCategoryNames.NetworkNodeType };
                var referenceDataCategoryList = deliveryPointGroupIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;
                deliveryPointGroupDto.NetworkNodeType = GetReferenceData(referenceDataCategoryList, ReferenceDataCategoryNames.NetworkNodeType, DeliveryPointGroupConstants.DeliveryPointGroupDataProviderGUID, true);

                deliveryPointGroupDto.GroupBoundaryGUID = Guid.NewGuid();
                deliveryPointGroupDto.RelationshipTypeForCentroidToBoundaryGUID = Guid.NewGuid();
                deliveryPointGroupDto.GroupCentroid = deliveryPointGroupDto.GroupBoundary.Centroid;

                deliveryPointGroupDataService.CreateDeliveryGroup(ConvertToDataDTO(deliveryPointGroupDto));

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return null;
            }
        }

        /// This method fetches geojson data for groups.
        /// /// </summary>
        /// <param name="deliveryGroups"List of delivery groups.</param>
        /// <returns>Geojson string for groups.</returns>
        private static string GetDeliveryGroupsJsonData(List<DeliveryPointGroupDataDTO> deliveryGroups)
        {
            var geoJson = new GeoJson
            {
                features = new List<Feature>()
            };

            if (deliveryGroups != null && deliveryGroups.Count > 0)
            {
                foreach (var group in deliveryGroups)
                {
                    Geometry groupGeometry = new Geometry();

                    groupGeometry.type = group.GroupBoundary.Shape.SpatialTypeName;

                    SqlGeometry groupSqlGeometry = null;
                    if (groupGeometry.type == Convert.ToString(OpenGisGeometryType.Polygon.ToString()))
                    {
                        groupSqlGeometry = SqlGeometry.STPolyFromWKB(new SqlBytes(group.GroupBoundary.Shape.AsBinary()), 27700).MakeValid();
                        List<List<double[]>> listCords = new List<List<double[]>>();
                        List<double[]> cords = new List<double[]>();

                        for (int pt = 1; pt <= groupSqlGeometry.STNumPoints().Value; pt++)
                        {
                            double[] coordinates = new double[] { groupSqlGeometry.STPointN(pt).STX.Value, groupSqlGeometry.STPointN(pt).STY.Value };
                            cords.Add(coordinates);
                        }

                        listCords.Add(cords);
                        groupGeometry.coordinates = listCords;
                    }

                    Feature groupFeature = new Feature();
                    groupFeature.geometry = groupGeometry;
                    groupFeature.type = DeliveryPointGroupConstants.FeatureType;
                    groupFeature.id = group.DeliveryGroup.ID.ToString();
                    groupFeature.properties = new Dictionary<string, Newtonsoft.Json.Linq.JToken>
                    {
                        { DeliveryPointGroupConstants.LayerType, Convert.ToString(OtherLayersType.GroupLink.GetDescription()) },
                        { "type", "group" },
                        { "groupType", group.DeliveryGroup.GroupTypeGUID },
                        { "groupName", group.DeliveryGroup.GroupName }
                    };

                    geoJson.features.Add(groupFeature);

                    foreach (var addedDeliveryPoint in group.AddedDeliveryPoints)
                    {
                        Geometry groupDPGeometry = new Geometry();

                        groupDPGeometry.type = addedDeliveryPoint.Shape.SpatialTypeName;

                        SqlGeometry groupDPSqlGeometry = null;
                        if (groupDPGeometry.type == Convert.ToString(OpenGisGeometryType.Point.ToString()))
                        {
                            groupDPSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(addedDeliveryPoint.Shape.AsBinary()), DeliveryPointGroupConstants.BNGCOORDINATESYSTEM).MakeValid();
                            groupDPGeometry.coordinates = new double[] { groupDPSqlGeometry.STX.Value, groupDPSqlGeometry.STY.Value };
                        }

                        Feature groupDPFeature = new Feature();
                        groupDPFeature.geometry = groupDPGeometry;
                        groupDPFeature.type = DeliveryPointGroupConstants.FeatureType;
                        groupDPFeature.id = addedDeliveryPoint.ID.ToString();
                        groupDPFeature.properties = new Dictionary<string, Newtonsoft.Json.Linq.JToken>
                    {
                        { DeliveryPointGroupConstants.LayerType, Convert.ToString(OtherLayersType.GroupLink.GetDescription()) },
                        { "type", "deliverypoint" }
                    };

                        geoJson.features.Add(groupDPFeature);
                    }
                }
            }

            return JsonConvert.SerializeObject(geoJson);
        }

        /// <summary>
        /// Covert delivery point group public DTO to data DTO.
        /// </summary>
        /// <param name="deliveryPointGroupDTO">Delivery Point Group DTO.</param>
        /// <returns>Delivery point public DTO.</returns>
        private DeliveryPointGroupDataDTO ConvertToDataDTO(DeliveryPointGroupDTO deliveryPointGroupDTO)
        {
            DeliveryPointGroupDataDTO deliveryPointGroupDataDTO = new DeliveryPointGroupDataDTO();
            if (deliveryPointGroupDTO != null)
            {
                // Construct boundary Location
                deliveryPointGroupDataDTO.GroupBoundary.ID = deliveryPointGroupDTO.GroupBoundaryGUID;
                deliveryPointGroupDataDTO.GroupBoundary.Shape = deliveryPointGroupDTO.GroupBoundary;
                deliveryPointGroupDataDTO.GroupBoundary.RowCreateDateTime = DateTime.UtcNow;

                // Construct centroid Location
                deliveryPointGroupDataDTO.GroupCentroid.ID = deliveryPointGroupDTO.ID;
                deliveryPointGroupDataDTO.GroupCentroid.Shape = deliveryPointGroupDTO.GroupCentroid;
                deliveryPointGroupDataDTO.GroupCentroid.RowCreateDateTime = DateTime.UtcNow;

                // Construct centroid Network Node
                deliveryPointGroupDataDTO.GroupCentroidNetworkNode.ID = deliveryPointGroupDTO.ID;
                deliveryPointGroupDataDTO.GroupCentroidNetworkNode.NetworkNodeType_GUID = deliveryPointGroupDTO.NetworkNodeType;
                deliveryPointGroupDataDTO.GroupCentroidNetworkNode.RowCreateDateTime = DateTime.UtcNow;

                // Construct centroid Delivery Point
                deliveryPointGroupDataDTO.GroupCentroidDeliveryPoint.ID = deliveryPointGroupDTO.ID;
                deliveryPointGroupDataDTO.GroupCentroidDeliveryPoint.DeliveryPointUseIndicatorGUID = deliveryPointGroupDTO.DeliveryPointUseIndicatorGUID;
                deliveryPointGroupDataDTO.GroupCentroidDeliveryPoint.RowCreateDateTime = DateTime.UtcNow;

                // Construct centroid LocationRelationShip for relationship between boundary and centroid
                LocationRelationshipDataDTO groupShapeToCentroidRelationship = new LocationRelationshipDataDTO();
                groupShapeToCentroidRelationship.ID = deliveryPointGroupDTO.LocationRelationshipForCentroidToBoundaryGuid;
                groupShapeToCentroidRelationship.LocationID = deliveryPointGroupDTO.ID;
                groupShapeToCentroidRelationship.RelatedLocationID = deliveryPointGroupDTO.GroupBoundaryGUID;
                groupShapeToCentroidRelationship.RelationshipTypeGUID = deliveryPointGroupDTO.RelationshipTypeForCentroidToBoundaryGUID;
                groupShapeToCentroidRelationship.RowCreateDateTime = DateTime.UtcNow;
                deliveryPointGroupDataDTO.GroupCentroid.LocationRelationships.Add(groupShapeToCentroidRelationship);

                // Construct centroid LocationRelationShip for relationship between centroid and deliveryPoints
                if (deliveryPointGroupDTO.AddedDeliveryPoints != null && deliveryPointGroupDTO.AddedDeliveryPoints.Count > 1)
                {
                    foreach (var deliveryPoint in deliveryPointGroupDTO.AddedDeliveryPoints)
                    {
                        LocationRelationshipDataDTO deliveryPointToCentroidRelation = new LocationRelationshipDataDTO();
                        deliveryPointToCentroidRelation.ID = Guid.NewGuid();
                        deliveryPointToCentroidRelation.LocationID = deliveryPoint.ID;
                        deliveryPointToCentroidRelation.RelatedLocationID = deliveryPointGroupDTO.ID;
                        deliveryPointToCentroidRelation.RelationshipTypeGUID = deliveryPointGroupDTO.RelationshipTypeForCentroidToDeliveryPointGUID;
                        deliveryPointToCentroidRelation.RowCreateDateTime = DateTime.UtcNow;
                        deliveryPointGroupDataDTO.GroupCentroid.LocationRelationships1.Add(deliveryPointToCentroidRelation);
                    }
                }

                // Construct Delivery Point Group Details
                deliveryPointGroupDataDTO.DeliveryGroup.ID = deliveryPointGroupDTO.ID;
                deliveryPointGroupDataDTO.DeliveryGroup.GroupName = deliveryPointGroupDTO.GroupName;
                deliveryPointGroupDataDTO.DeliveryGroup.DeliverToReception = deliveryPointGroupDTO.DeliverToReception;
                deliveryPointGroupDataDTO.DeliveryGroup.GroupTypeGUID = deliveryPointGroupDTO.GroupTypeGUID;
                deliveryPointGroupDataDTO.DeliveryGroup.NumberOfFloors = deliveryPointGroupDTO.NumberOfFloors;
                deliveryPointGroupDataDTO.DeliveryGroup.InternalDistanceMeters = deliveryPointGroupDTO.InternalDistanceMeters;
                deliveryPointGroupDataDTO.DeliveryGroup.WorkloadTimeOverrideMinutes = deliveryPointGroupDTO.WorkloadTimeOverrideMinutes;
                deliveryPointGroupDataDTO.DeliveryGroup.RowCreateDateTime = DateTime.UtcNow;
            }

            return deliveryPointGroupDataDTO;
        }

        /// <summary>
        /// Covert list of delivery point group data DTO to public DTO.
        /// </summary>
        /// <param name="deliveryPointGroupDTOList">Collection of deliveryPoint group public DTO.</param>
        /// <returns>Collection of delivery point data DTO.</returns>
        private List<DeliveryPointGroupDataDTO> ConvertToDataDTO(List<DeliveryPointGroupDTO> deliveryPointGroupDTOList)
        {
            List<DeliveryPointGroupDataDTO> deliveryPointGroupDataDTO = new List<DeliveryPointGroupDataDTO>();

            foreach (var deliveryPointGroupDTO in deliveryPointGroupDTOList)
            {
                deliveryPointGroupDataDTO.Add(ConvertToDataDTO(deliveryPointGroupDTO));
            }

            return deliveryPointGroupDataDTO;
        }

        /// <summary>
        /// Covert data dto to public dto.
        /// </summary>
        /// <param name="deliveryPointGroupDataDTO">Delivery point Group data DTO.</param>
        /// <returns>Delivery point Group public DTO.</returns>
        private DeliveryPointGroupDTO ConvertToDTO(DeliveryPointGroupDataDTO deliveryPointGroupDataDTO)
        {
            DeliveryPointGroupDTO deliveryPointGroupDTO = null;

            if (deliveryPointGroupDataDTO != null)
            {
                deliveryPointGroupDTO = new DeliveryPointGroupDTO();
                // Not need these details as of now
                // Get boundary Location


                // Get centroid Location
                if (deliveryPointGroupDataDTO.AddedDeliveryPoints != null)
                {
                }
                // Get centroid Network Node


                // Get centroid Delivery Point




                // Get Delivery Point Group Details
                if (deliveryPointGroupDataDTO.DeliveryGroup != null)
                {
                    deliveryPointGroupDTO.ID = deliveryPointGroupDataDTO.DeliveryGroup.ID;
                    deliveryPointGroupDTO.GroupName = deliveryPointGroupDataDTO.DeliveryGroup.GroupName;
                    deliveryPointGroupDTO.DeliverToReception = deliveryPointGroupDataDTO.DeliveryGroup.DeliverToReception;
                    deliveryPointGroupDTO.GroupTypeGUID = deliveryPointGroupDataDTO.DeliveryGroup.GroupTypeGUID;
                    deliveryPointGroupDTO.NumberOfFloors = deliveryPointGroupDataDTO.DeliveryGroup.NumberOfFloors;
                    deliveryPointGroupDTO.InternalDistanceMeters = deliveryPointGroupDataDTO.DeliveryGroup.InternalDistanceMeters;
                    deliveryPointGroupDTO.WorkloadTimeOverrideMinutes = deliveryPointGroupDataDTO.DeliveryGroup.WorkloadTimeOverrideMinutes;
                }
            }

            return deliveryPointGroupDTO;
        }

        /// <summary>
        /// Covert collection of delivery point group data DTO to public DTO.
        /// </summary>
        /// <param name="deliveryPointDataDTOList">Collection of delivery point data DTO.</param>
        /// <returns>Collection of delivery point public dtDTOo.</returns>
        private List<DeliveryPointGroupDTO> ConvertToDTO(List<DeliveryPointGroupDataDTO> deliveryPointGroupDataDTOList)
        {
            List<DeliveryPointGroupDTO> deliveryPointGroupDTO = new List<DeliveryPointGroupDTO>();

            foreach (var deliveryPointGroupDataDTO in deliveryPointGroupDataDTOList)
            {
                deliveryPointGroupDTO.Add(ConvertToDTO(deliveryPointGroupDataDTO));
            }

            return deliveryPointGroupDTO;
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
                string methodName = typeof(DeliveryPointGroupBusinessService) + "." + nameof(GetReferenceData);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

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

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return referenceDataGuid;
            }
        }
    }
}