﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.IntegrationService;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.NetworkManager.WebAPI.DTO;
using RM.DataManagement.NetworkManager.WebAPI.DataDTO;
using System.Linq;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

namespace RM.DataManagement.NetworkManager.WebAPI.BusinessService
{
    public class NetworkManagerBusinessService : INetworkManagerBusinessService
    {
        #region Member Variables

        private const string FeatureType = "Feature";
        private const string LayerType = "type";
        private const string Polygon = "POLYGON(({0} {1}, {2} {3}, {4} {5}, {6} {7}, {8} {9}))";
        private const int BNGCOORDINATESYSTEM = 27700;
        private const string Comma = ", ";

        private IStreetNetworkDataService streetNetworkDataService = default(IStreetNetworkDataService);
        private INetworkManagerIntegrationService networkManagerIntegrationService = default(INetworkManagerIntegrationService);
        private IOSRoadLinkDataService osRoadLinkDataService = default(IOSRoadLinkDataService);
        private IRoadNameDataService roadNameDataService = default(IRoadNameDataService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        private int priority = LoggerTraceConstants.NetworkManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.NetworkManagerBusinessServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.NetworkManagerBusinessServiceMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Network Manager Business Service class.
        /// </summary>
        /// <param name="streetNetworkDataService">The street network data service</param>
        /// <param name="networkManagerIntegrationService">The street network data service</param>
        /// <param name="osRoadLinkDataService">The Ordnance Survey road link data service</param>
        /// <param name="roadNameDataService">The road name data service</param>
        /// <param name="loggingHelper">The helper class object for logging</param>
        public NetworkManagerBusinessService(IStreetNetworkDataService streetNetworkDataService, INetworkManagerIntegrationService networkManagerIntegrationService, IOSRoadLinkDataService osRoadLinkDataService, IRoadNameDataService roadNameDataService, ILoggingHelper loggingHelper)
        {
            this.streetNetworkDataService = streetNetworkDataService;
            this.networkManagerIntegrationService = networkManagerIntegrationService;
            this.osRoadLinkDataService = osRoadLinkDataService;
            this.roadNameDataService = roadNameDataService;
            this.loggingHelper = loggingHelper;
        }
        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">The operational object unique identifier.</param>
        /// <param name="streetName">The street name.</param>
        /// <returns>The nearest street and the intersection point.</returns>
        public Tuple<NetworkLinkDTO, SqlGeometry> GetNearestNamedRoad(DbGeometry operationalObjectPoint, string streetName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetNearestNamedRoad"))
            {
                string methodName = typeof(NetworkManagerBusinessService) + "." + nameof(GetNearestNamedRoad);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<string> categoryNamesSimpleLists = new List<string>
                    {
                        ReferenceDataCategoryNames.NetworkLinkType,
                    };

                var referenceDataCategoryList = networkManagerIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;

                Tuple<NetworkLinkDataDTO, SqlGeometry> getNearestNamedRoad = streetNetworkDataService.GetNearestNamedRoad(operationalObjectPoint, streetName, referenceDataCategoryList);
                NetworkLinkDTO networkLink = new NetworkLinkDTO()
                    {
                        Id = getNearestNamedRoad.Item1.ID,
                        LinkGeometry = getNearestNamedRoad.Item1.LinkGeometry,
                        NetworkLinkType_GUID = getNearestNamedRoad.Item1.NetworkLinkTypeGUID,
                        TOID = getNearestNamedRoad.Item1.TOID
                    };
                
                Tuple<NetworkLinkDTO, SqlGeometry> nearestRoad = new Tuple<NetworkLinkDTO, SqlGeometry>(networkLink, getNearestNamedRoad.Item2);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return nearestRoad;
            }
        }

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public Tuple<NetworkLinkDTO, List<SqlGeometry>> GetNearestSegment(DbGeometry operationalObjectPoint)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetNearestSegment"))
            {
                string methodName = typeof(NetworkManagerBusinessService) + "." + nameof(GetNearestSegment);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<string> categoryNamesNameValuePairs = new List<string>
                    {
                        ReferenceDataCategoryNames.AccessLinkParameters,
                    };

                List<string> categoryNamesSimpleLists = new List<string>
                    {
                        ReferenceDataCategoryNames.NetworkLinkType
                    };

                var referenceDataCategoryList =
                    networkManagerIntegrationService.GetReferenceDataNameValuePairs(categoryNamesNameValuePairs).Result;

                referenceDataCategoryList.AddRange(
                   networkManagerIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result);

                Tuple<NetworkLinkDataDTO, List<SqlGeometry>> getNearestSegment = streetNetworkDataService.GetNearestSegment(operationalObjectPoint, referenceDataCategoryList);
                NetworkLinkDTO networkLink = new NetworkLinkDTO()
                {
                    Id = getNearestSegment.Item1.ID,
                    LinkGeometry = getNearestSegment.Item1.LinkGeometry,
                    NetworkLinkType_GUID = getNearestSegment.Item1.NetworkLinkTypeGUID,
                    TOID = getNearestSegment.Item1.TOID
                };

                Tuple<NetworkLinkDTO, List<SqlGeometry>> nearestSegment = new Tuple<NetworkLinkDTO, List<SqlGeometry>>(networkLink, getNearestSegment.Item2);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return nearestSegment;
            }
        }

        /// <summary>
        /// Get the street DTO for operational object.
        /// </summary>
        /// <param name="networkLinkID">networkLink unique identifier Guid.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public NetworkLinkDTO GetNetworkLink(Guid networkLinkID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetNetworkLink"))
            {
                string methodName = typeof(NetworkManagerBusinessService) + "." + nameof(GetNetworkLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                NetworkLinkDataDTO getNetworkLink = streetNetworkDataService.GetNetworkLink(networkLinkID);
                NetworkLinkDTO networkLink = new NetworkLinkDTO()
                {
                    Id = getNetworkLink.ID,
                    LinkGeometry = getNetworkLink.LinkGeometry,
                    NetworkLinkType_GUID = getNetworkLink.NetworkLinkTypeGUID,
                    TOID = getNetworkLink.TOID,
                    DataProvider_GUID = getNetworkLink.DataProviderGUID,
                    StartNode_GUID = getNetworkLink.StartNodeID,
                    EndNode_GUID = getNetworkLink.EndNodeID,
                    LinkGradientType = getNetworkLink.LinkGradientType,
                    LinkLength = getNetworkLink.LinkLength,
                    LinkName = getNetworkLink.LinkName,
                    RoadName_GUID = getNetworkLink.RoadNameGUID,
                    StreetName_GUID = getNetworkLink.StreetNameGUID
                };

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return networkLink;
            }
        }

        /// <summary> This method is used to get the road links crossing the access link </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLinkCoordinates">access link coordinate array</param> <returns>List<NetworkLinkDTO></returns>
        public List<NetworkLinkDTO> GetCrossingNetworkLinks(string boundingBoxCoordinates, DbGeometry accessLinkCoordinates)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetCrossingNetworkLinks"))
            {
                string methodName = typeof(NetworkManagerBusinessService) + "." + nameof(GetCrossingNetworkLinks);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<NetworkLinkDataDTO> getCrossingNetworkLinks = streetNetworkDataService.GetCrossingNetworkLink(boundingBoxCoordinates, accessLinkCoordinates);
                List<NetworkLinkDTO> networkLinkList = getCrossingNetworkLinks.Select(x => new NetworkLinkDTO()
                {
                    Id = x.ID,
                    DataProvider_GUID = x.DataProviderGUID,
                    StartNode_GUID = x.StartNodeID,
                    EndNode_GUID = x.EndNodeID,
                    LinkGeometry = x.LinkGeometry,
                    LinkGradientType = x.LinkGradientType,
                    LinkLength = x.LinkLength,
                    LinkName = x.LinkName,
                    NetworkLinkType_GUID = x.NetworkLinkTypeGUID,
                    RoadName_GUID = x.RoadNameGUID,
                    StreetName_GUID = x.StreetNameGUID,
                    TOID = x.TOID
                }).ToList();

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return networkLinkList;
            }
        }

        /// <summary>
        /// This method is used to fetch data for Ordinance survey Road Link.
        /// </summary>
        /// <param name="toid">toid unique identifier for OSRoadLink</param>
        /// <returns>The Route Hierarchy as string</returns>
        public async Task<string> GetOSRoadLink(string toid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetOSRoadLink"))
            {
                string methodName = typeof(NetworkManagerBusinessService) + "." + nameof(GetOSRoadLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var getOSRoadLink = await osRoadLinkDataService.GetOSRoadLink(toid);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return getOSRoadLink;
            }
        }

        /// <summary>
        /// This method fetches data for RoadLinks
        /// </summary>
        /// <param name="boundarybox">boundaryBox as string.</param>
        /// <param name="locationID">location unique identifier.</param>
        /// <returns>RoadLink object</returns>
        public string GetRoadRoutes(string boundarybox, Guid locationID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRoadRoutes"))
            {
                string methodName = typeof(NetworkManagerBusinessService) + "." + nameof(GetRoadRoutes);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string roadLinkJsonData = null;

                if (!string.IsNullOrEmpty(boundarybox))
                {
                    List<string> categoryNamesNameValuePairs = new List<string>
                    {
                        ReferenceDataCategoryNames.NetworkLinkType,
                    };
                    var referenceDataCategoryList = networkManagerIntegrationService.GetReferenceDataSimpleLists(categoryNamesNameValuePairs).Result;

                    var boundingBoxCoordinates =
                        GetRoadNameCoordinatesDatabyBoundarybox(boundarybox.Split(Comma[0]));

                    List<NetworkLinkDataDTO> getRoadRoutes = roadNameDataService.GetRoadRoutes(boundingBoxCoordinates, locationID, referenceDataCategoryList);
                    List<NetworkLinkDTO> networkLinkList = getRoadRoutes.Select(x => new NetworkLinkDTO()
                    {
                        Id = x.ID,
                        DataProvider_GUID = x.DataProviderGUID,
                        StartNode_GUID = x.StartNodeID,
                        EndNode_GUID = x.EndNodeID,
                        LinkGeometry = x.LinkGeometry,
                        LinkGradientType = x.LinkGradientType,
                        LinkLength = x.LinkLength,
                        LinkName = x.LinkName,
                        NetworkLinkType_GUID = x.NetworkLinkTypeGUID,
                        RoadName_GUID = x.RoadNameGUID,
                        StreetName_GUID = x.StreetNameGUID,
                        TOID = x.TOID
                    }).ToList();

                    roadLinkJsonData = GetRoadLinkJsonData(networkLinkList);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return roadLinkJsonData;
            }
        }

        #endregion Public Methods

        #region Basic And Advanced Search Public Methods

        /// <summary>
        /// Fetch the street name for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>Task</returns>
        public async Task<List<StreetNameDTO>> GetStreetNamesForBasicSearch(string searchText, Guid userUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetStreetNamesForBasicSearch"))
            {
                string methodName = typeof(NetworkManagerBusinessService) + "." + nameof(GetStreetNamesForBasicSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var fetchStreetNamesForBasicSearch = await streetNetworkDataService.GetStreetNamesForBasicSearch(searchText, userUnit).ConfigureAwait(false);
                var streetNameDTO = GenericMapper.MapList<StreetNameDataDTO, StreetNameDTO>(fetchStreetNamesForBasicSearch);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return streetNameDTO;
            }
        }

        /// <summary>
        /// Get the count of street name
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of street name</returns>
        public async Task<int> GetStreetNameCount(string searchText, Guid userUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetStreetNameCount"))
            {
                string methodName = typeof(NetworkManagerBusinessService) + "." + nameof(GetStreetNameCount);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var fetchStreetNamesForBasicSearch = await streetNetworkDataService.GetStreetNamesForBasicSearch(searchText, userUnit).ConfigureAwait(false);
                var getStreetNameCount = await streetNetworkDataService.GetStreetNameCount(searchText, userUnit).ConfigureAwait(false);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return getStreetNameCount;
            }
        }

        /// <summary>
        /// Fetch street names for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>StreetNames</returns>
        public async Task<List<StreetNameDTO>> GetStreetNamesForAdvanceSearch(string searchText, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetStreetNamesForAdvanceSearch"))
            {
                string methodName = typeof(NetworkManagerBusinessService) + "." + nameof(GetStreetNamesForAdvanceSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var fetchStreetNamesForAdvanceSearch = await streetNetworkDataService.GetStreetNamesForAdvanceSearch(searchText, unitGuid).ConfigureAwait(false);
              
                var streetNameDTO = GenericMapper.MapList<StreetNameDataDTO, StreetNameDTO>(fetchStreetNamesForAdvanceSearch);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return streetNameDTO;
            }
        }

        #endregion Basic And Advanced Search Public methods

        #region Private Methods

        /// <summary>
        /// This method fetches co-ordinates of roadlink
        /// </summary>
        /// <param name="roadLinkparameters">roadLinkparameters as object</param>
        /// <returns>roadlink coordinates</returns>
        private static string GetRoadNameCoordinatesDatabyBoundarybox(params object[] roadLinkparameters)
        {
            string coordinates = string.Empty;

            if (roadLinkparameters != null && roadLinkparameters.Length == 4)
            {
                coordinates = string.Format(
                                     Polygon,
                                     Convert.ToString(roadLinkparameters[0]),
                                     Convert.ToString(roadLinkparameters[1]),
                                     Convert.ToString(roadLinkparameters[0]),
                                     Convert.ToString(roadLinkparameters[3]),
                                     Convert.ToString(roadLinkparameters[2]),
                                     Convert.ToString(roadLinkparameters[3]),
                                     Convert.ToString(roadLinkparameters[2]),
                                     Convert.ToString(roadLinkparameters[1]),
                                     Convert.ToString(roadLinkparameters[0]),
                                     Convert.ToString(roadLinkparameters[1]));
            }

            return coordinates;
        }

        /// <summary>
        /// This method fetches geojson data for roadlink
        /// </summary>
        /// <returns>roadlink object</returns>
        /// <param name="networkLinkDTO">networkLinkDTO as list of NetworkLinkDTO</param>
        private static string GetRoadLinkJsonData(List<NetworkLinkDTO> networkLinkDTO)
        {
            var geoJson = new GeoJson
            {
                features = new List<Feature>()
            };

            if (networkLinkDTO != null && networkLinkDTO.Count > 0)
            {
                int i = 1;
                foreach (var res in networkLinkDTO)
                {
                    Geometry geometry = new Geometry();

                    geometry.type = res.LinkGeometry.SpatialTypeName;

                    var resultCoordinates = res.LinkGeometry;

                    SqlGeometry roadLinkSqlGeometry = null;
                    if (geometry.type == Convert.ToString(GeometryType.LineString))
                    {
                        roadLinkSqlGeometry = SqlGeometry.STLineFromWKB(new SqlBytes(resultCoordinates.AsBinary()), BNGCOORDINATESYSTEM).MakeValid();

                        List<List<double>> roadLinkCoordinates = new List<List<double>>();

                        for (int pt = 1; pt <= roadLinkSqlGeometry.STNumPoints().Value; pt++)
                        {
                            List<double> coordinatesval = new List<double> { roadLinkSqlGeometry.STPointN(pt).STX.Value, roadLinkSqlGeometry.STPointN(pt).STY.Value };
                            roadLinkCoordinates.Add(coordinatesval);
                        }

                        geometry.coordinates = roadLinkCoordinates;
                    }
                    else
                    {
                        roadLinkSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), BNGCOORDINATESYSTEM).MakeValid();
                        geometry.coordinates = new double[] { roadLinkSqlGeometry.STX.Value, roadLinkSqlGeometry.STY.Value };
                    }

                    Feature feature = new Feature();
                    feature.geometry = geometry;
                    feature.id = res.Id.ToString();
                    feature.type = FeatureType;
                    feature.properties = new Dictionary<string, Newtonsoft.Json.Linq.JToken> { { LayerType, Convert.ToString(OtherLayersType.RoadLink.GetDescription()) } };
                    geoJson.features.Add(feature);
                    i++;
                }
            }

            return JsonConvert.SerializeObject(geoJson);
        }

        #endregion private methods
    }
}