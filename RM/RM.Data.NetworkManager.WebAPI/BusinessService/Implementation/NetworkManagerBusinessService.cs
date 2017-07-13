using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.IntegrationService;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces;

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

        // TODO Code to be refactored : Old DTO to be refactored to new DTO
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
                var getNearestNamedRoad = streetNetworkDataService.GetNearestNamedRoad(operationalObjectPoint, streetName, referenceDataCategoryList);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return getNearestNamedRoad;
            }
        }

        // TODO Code to be refactored : Old DTO to be refactored to new DTO
        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public Tuple<NetworkLinkDTO, SqlGeometry> GetNearestSegment(DbGeometry operationalObjectPoint)
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
                var getNearestSegment = streetNetworkDataService.GetNearestSegment(operationalObjectPoint, referenceDataCategoryList);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return getNearestSegment;
            }
        }

        // TODO Code to be refactored : Old DTO to be refactored to new DTO
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

                var getNetworkLink = streetNetworkDataService.GetNetworkLink(networkLinkID);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return getNetworkLink;
            }
        }

        // TODO Code to be refactored : Old DTO to be refactored to new DTO
        /// <summary> This method is used to get the road links crossing the access link </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLinkCoordinates">access link coordinate array</param> <returns>List<NetworkLinkDTO></returns>
        public List<NetworkLinkDTO> GetCrossingNetworkLinks(string boundingBoxCoordinates, DbGeometry accessLinkCoordinates)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetCrossingNetworkLinks"))
            {
                string methodName = typeof(NetworkManagerBusinessService) + "." + nameof(GetCrossingNetworkLinks);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var getCrossingNetworkLinks = streetNetworkDataService.GetCrossingNetworkLink(boundingBoxCoordinates, accessLinkCoordinates);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return getCrossingNetworkLinks;
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
                    roadLinkJsonData =
                        GetRoadLinkJsonData(roadNameDataService.GetRoadRoutes(boundingBoxCoordinates, locationID, referenceDataCategoryList));
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return roadLinkJsonData;
            }
        }

        #endregion Public Methods

        #region Basic And Advanced Search Public Methods

        // TODO Code to be refactored : Old DTO to be refactored to new DTO
        /// <summary>
        /// Fetch the street name for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>Task</returns>
        public async Task<List<StreetNameDTO>> FetchStreetNamesForBasicSearch(string searchText, Guid userUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchStreetNamesForBasicSearch"))
            {
                string methodName = typeof(NetworkManagerBusinessService) + "." + nameof(FetchStreetNamesForBasicSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var fetchStreetNamesForBasicSearch = await streetNetworkDataService.FetchStreetNamesForBasicSearch(searchText, userUnit).ConfigureAwait(false);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return fetchStreetNamesForBasicSearch;
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

                var fetchStreetNamesForBasicSearch = await streetNetworkDataService.FetchStreetNamesForBasicSearch(searchText, userUnit).ConfigureAwait(false);
                var getStreetNameCount = await streetNetworkDataService.GetStreetNameCount(searchText, userUnit).ConfigureAwait(false);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return getStreetNameCount;
            }
        }

        // TODO Code to be refactored : Old DTO to be refactored to new DTO
        /// <summary>
        /// Fetch street names for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>StreetNames</returns>
        public async Task<List<StreetNameDTO>> FetchStreetNamesForAdvanceSearch(string searchText, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchStreetNamesForAdvanceSearch"))
            {
                string methodName = typeof(NetworkManagerBusinessService) + "." + nameof(FetchStreetNamesForAdvanceSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var fetchStreetNamesForAdvanceSearch = await streetNetworkDataService.FetchStreetNamesForAdvanceSearch(searchText, unitGuid).ConfigureAwait(false);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return fetchStreetNamesForAdvanceSearch;
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
        // TODO Code to be refactored : Old DTO to be refactored to new DTO
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