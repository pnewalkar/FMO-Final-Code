using System;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO;
using RM.Data.DeliveryPointGroupManager.WebAPI.DTO;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.DataService;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.Integration;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
                    //var accessLinkDataDto = deliveryPointGroupDataService.GetDeliveryPointGroups(deliveryGroupCoordinates, unitGuid);
                    //var accessLink = GenericMapper.MapList<DeliveryPointGroupDataDTO, DeliveryPointGroupDTO>(accessLinkDataDto);
                    // deliveryPointGroupJsonData = GetAccessLinkJsonData(accessLinkDataDto);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return deliveryPointGroupJsonData;
            }
        }

        public DeliveryPointGroupDTO UpdateDeliveryGroup(DeliveryPointGroupDTO deliveryPointGroupDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"Business.{nameof(UpdateDeliveryGroup)}"))
            {
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
        /// 
        /// </summary>
        /// <param name="deliveryPointGroupDto">UI Dto to create Delivery group</param>
        /// <returns></returns>
        public Task<DeliveryPointGroupDTO> CreateDeliveryPointGroup(DeliveryPointGroupDTO deliveryPointGroupDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.CreateDeliveryPointGroup"))
            {
                string methodName = typeof(DeliveryPointGroupBusinessService) + "." + nameof(CreateDeliveryPointGroup);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (deliveryPointGroupDto == null)
                {
                    throw new ArgumentNullException(nameof(deliveryPointGroupDto), string.Format(ErrorConstants.Err_ArgumentmentNullException, deliveryPointGroupDto));
                }

                deliveryPointGroupDataService.CreateDeliveryGroup(ConvertToDataDTO(deliveryPointGroupDto));
                throw new NotImplementedException();

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return null;
            }
        }

        /// <summary>
        /// This method fetches geojson data for groups link
        /// </summary>
        /// <param name="lstAccessLinkDTO">accesslink as list of AccessLinkDTO</param>
        /// <returns>AccsessLink object</returns>
        //private static string GetDeliveryGroupsJsonData(List<DeliveryPointGroupDataDTO> lstGroupLinkDTO)
        //{
        //    var geoJson = new GeoJson
        //    {
        //        features = new List<Feature>()
        //    };
        //    if (lstGroupLinkDTO != null && lstGroupLinkDTO.Count > 0)
        //    {
        //        foreach (var res in lstGroupLinkDTO)
        //        {
        //            Geometry geometry = new Geometry();

        //            geometry.type = res.loc.LinkGeometry.SpatialTypeName;

        //            var resultCoordinates = res.NetworkLink.LinkGeometry;

        //            SqlGeometry groupLinksqlGeometry = null;
        //            if (geometry.type == Convert.ToString(GeometryType.LineString))
        //            {
        //                groupLinksqlGeometry = SqlGeometry.STLineFromWKB(new SqlBytes(resultCoordinates.AsBinary()), DeliveryPointGroupConstants.BNGCOORDINATESYSTEM).MakeValid();

        //                List<List<double>> cords = new List<List<double>>();

        //                for (int pt = 1; pt <= groupLinksqlGeometry.STNumPoints().Value; pt++)
        //                {
        //                    List<double> accessLinkCoordinates = new List<double> { groupLinksqlGeometry.STPointN(pt).STX.Value, groupLinksqlGeometry.STPointN(pt).STY.Value };
        //                    cords.Add(accessLinkCoordinates);
        //                }

        //                geometry.coordinates = cords;
        //            }
        //            else
        //            {
        //                groupLinksqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), DeliveryPointGroupConstants.BNGCOORDINATESYSTEM).MakeValid();
        //                geometry.coordinates = new double[] { groupLinksqlGeometry.STX.Value, groupLinksqlGeometry.STY.Value };
        //            }

        //            Feature feature = new Feature();
        //            feature.geometry = geometry;

        //            feature.type = DeliveryPointGroupConstants.FeatureType;
        //            feature.id = res.ID.ToString();
        //            feature.properties = new Dictionary<string, Newtonsoft.Json.Linq.JToken> { { DeliveryPointGroupConstants.LayerType, Convert.ToString(OtherLayersType.GroupLink.GetDescription()) } };

        //            geoJson.features.Add(feature);
        //        }
        //    }

        //    return JsonConvert.SerializeObject(geoJson);
        //}


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
                deliveryPointGroupDataDTO.groupBoundaryLocation.ID = deliveryPointGroupDTO.GroupBoundaryGUID; // TODO : change to New Guid
                deliveryPointGroupDataDTO.groupBoundaryLocation.Shape = deliveryPointGroupDTO.GroupBoundary;
                deliveryPointGroupDataDTO.groupBoundaryLocation.RowCreateDateTime = DateTime.UtcNow;

                // Construct centroid Location
                deliveryPointGroupDataDTO.groupCentroidLocation.ID = deliveryPointGroupDTO.ID;
                deliveryPointGroupDataDTO.groupCentroidLocation.Shape = deliveryPointGroupDTO.GroupCentroid;
                deliveryPointGroupDataDTO.groupCentroidLocation.RowCreateDateTime = DateTime.UtcNow;

                // Construct centroid Network Node
                deliveryPointGroupDataDTO.groupCentroidNetworkNode.ID = deliveryPointGroupDTO.ID;
                deliveryPointGroupDataDTO.groupCentroidNetworkNode.NetworkNodeType_GUID = deliveryPointGroupDTO.NetworkNodeType; // TODO :
                deliveryPointGroupDataDTO.groupCentroidNetworkNode.RowCreateDateTime = DateTime.UtcNow;

                // Construct centroid Delivery Point
                deliveryPointGroupDataDTO.groupCentroidDeliveryPoint.ID = deliveryPointGroupDTO.ID;
                deliveryPointGroupDataDTO.groupCentroidDeliveryPoint.DeliveryPointUseIndicatorGUID = deliveryPointGroupDTO.DeliveryPointUseIndicatorGUID;
                deliveryPointGroupDataDTO.groupCentroidDeliveryPoint.RowCreateDateTime = DateTime.UtcNow;

                // Construct centroid LocationRelationShip for relationship between boundary and centroid
                LocationRelationshipDataDTO groupShapeToCentroidRelationship = new LocationRelationshipDataDTO();
                groupShapeToCentroidRelationship.ID = deliveryPointGroupDTO.LocationRelationshipForCentroidToBoundaryGuid; // TODO : 
                groupShapeToCentroidRelationship.LocationID = deliveryPointGroupDTO.ID;
                groupShapeToCentroidRelationship.RelatedLocationID = deliveryPointGroupDTO.GroupBoundaryGUID;
                groupShapeToCentroidRelationship.RelationshipTypeGUID = deliveryPointGroupDTO.RelationshipTypeForCentroidToBoundaryGUID;
                groupShapeToCentroidRelationship.RowCreateDateTime = DateTime.UtcNow;
                deliveryPointGroupDataDTO.groupCentroidLocation.LocationRelationships.Add(groupShapeToCentroidRelationship);

                // Construct centroid LocationRelationShip for relationship between centroid and deliveryPoints
                if (deliveryPointGroupDTO.AddedDeliveryPoints != null && deliveryPointGroupDTO.AddedDeliveryPoints.Count > 1)
                {
                    foreach (var deliveryPoint in deliveryPointGroupDTO.AddedDeliveryPoints)
                    {
                        LocationRelationshipDataDTO deliveryPointToCentroidRelation = new LocationRelationshipDataDTO();
                        deliveryPointToCentroidRelation.ID = Guid.NewGuid(); // TODOD : 
                        deliveryPointToCentroidRelation.LocationID = deliveryPoint.ID;
                        deliveryPointToCentroidRelation.RelatedLocationID = deliveryPointGroupDTO.ID;
                        deliveryPointToCentroidRelation.RelationshipTypeGUID = deliveryPointGroupDTO.RelationshipTypeForCentroidToDeliveryPointGUID;
                        deliveryPointToCentroidRelation.RowCreateDateTime = DateTime.UtcNow;
                        deliveryPointGroupDataDTO.groupCentroidLocation.LocationRelationships1.Add(deliveryPointToCentroidRelation);
                    }
                }

                // Construct Delivery Point Group Details
                deliveryPointGroupDataDTO.groupDetails.ID = deliveryPointGroupDTO.ID;
                deliveryPointGroupDataDTO.groupDetails.GroupName = deliveryPointGroupDTO.GroupName;
                deliveryPointGroupDataDTO.groupDetails.DeliverToReception = deliveryPointGroupDTO.DeliverToReception;
                deliveryPointGroupDataDTO.groupDetails.GroupTypeGUID = deliveryPointGroupDTO.GroupTypeGUID;
                deliveryPointGroupDataDTO.groupDetails.NumberOfFloors = deliveryPointGroupDTO.NumberOfFloors;
                deliveryPointGroupDataDTO.groupDetails.InternalDistanceMeters = deliveryPointGroupDTO.InternalDistanceMeters;
                deliveryPointGroupDataDTO.groupDetails.WorkloadTimeOverrideMinutes = deliveryPointGroupDTO.WorkloadTimeOverrideMinutes;
                deliveryPointGroupDataDTO.groupDetails.RowCreateDateTime = DateTime.UtcNow;
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
                if (deliveryPointGroupDataDTO.groupCentroidLocation != null)
                {
                    deliveryPointGroupDataDTO.groupCentroidLocation.ID = deliveryPointGroupDTO.ID;
                    deliveryPointGroupDataDTO.groupCentroidLocation.Shape = deliveryPointGroupDTO.GroupCentroid;

                    // Get centroid LocationRelationShip for relationship between boundary and centroid
                    if (deliveryPointGroupDataDTO.groupCentroidLocation.LocationRelationships != null && deliveryPointGroupDataDTO.groupCentroidLocation.LocationRelationships.Count > 0)
                    {

                    }

                    // Get centroid LocationRelationShip for relationship between centroid and deliveryPoints
                    if (deliveryPointGroupDataDTO.groupCentroidLocation.LocationRelationships1 != null && deliveryPointGroupDataDTO.groupCentroidLocation.LocationRelationships1.Count > 0)
                    {

                    }
                }
                // Get centroid Network Node


                // Get centroid Delivery Point




                // Get Delivery Point Group Details
                if (deliveryPointGroupDataDTO.groupDetails != null)
                {
                    deliveryPointGroupDTO.ID = deliveryPointGroupDataDTO.groupDetails.ID;
                    deliveryPointGroupDTO.GroupName = deliveryPointGroupDataDTO.groupDetails.GroupName;
                    deliveryPointGroupDTO.DeliverToReception = deliveryPointGroupDataDTO.groupDetails.DeliverToReception;
                    deliveryPointGroupDTO.GroupTypeGUID = deliveryPointGroupDataDTO.groupDetails.GroupTypeGUID;
                    deliveryPointGroupDTO.NumberOfFloors = deliveryPointGroupDataDTO.groupDetails.NumberOfFloors;
                    deliveryPointGroupDTO.InternalDistanceMeters = deliveryPointGroupDataDTO.groupDetails.InternalDistanceMeters;
                    deliveryPointGroupDTO.WorkloadTimeOverrideMinutes = deliveryPointGroupDataDTO.groupDetails.WorkloadTimeOverrideMinutes;
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
    }
}