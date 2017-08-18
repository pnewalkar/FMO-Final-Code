using System;
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
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryPointGroups"))
            {
                string methodName = typeof(DeliveryPointGroupBusinessService) + "." + nameof(GetDeliveryPointGroups);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string deliveryPointGroupJsonData = null;

                if (!string.IsNullOrEmpty(boundaryBox))
                {
                    var deliveryGroupCoordinates = GetGroupCoordinatesDataByBoundingBox(boundaryBox.Split(Comma[0]));
                  //  var groupLinkDataDto = deliveryPointGroupDataService.GetDeliveryGroups(deliveryGroupCoordinates, unitGuid);
                  //  var accessLink = GenericMapper.MapList<DeliveryPointGroupDataDTO, DeliveryPointGroupDTO>(groupLinkDataDto);
                //    deliveryPointGroupJsonData = GetDeliveryGroupsJsonData(groupLinkDataDto);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return deliveryPointGroupJsonData;
            }
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
    }
}