using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using RM.CommonLibrary.EntityFramework.DataService;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.IntegrationService;

namespace RM.DataManagement.NetworkManager.WebAPI.BusinessService
{
    public class NetworkManagerBusinessService : INetworkManagerBusinessService
    {
        private IStreetNetworkDataService streetNetworkDataService = default(IStreetNetworkDataService);
        private INetworkManagerIntegrationService networkManagerIntegrationService = default(INetworkManagerIntegrationService);
        private IOSRoadLinkDataService osRoadLinkDataService = default(IOSRoadLinkDataService);
        private IRoadNameDataService roadNameDataService = default(RoadNameDataService);

        /// <summary>
        /// Initializes a new instance of the <see cref="StreetNetworkBusinessService"/> class.
        /// </summary>
        /// <param name="streetNetworkRepository">The street network repository.</param>
        public NetworkManagerBusinessService(IStreetNetworkDataService streetNetworkDataService, INetworkManagerIntegrationService networkManagerIntegrationService, IOSRoadLinkDataService osRoadLinkDataService, IRoadNameDataService roadNameDataService)
        {
            this.streetNetworkDataService = streetNetworkDataService;
            this.networkManagerIntegrationService = networkManagerIntegrationService;
            this.osRoadLinkDataService = osRoadLinkDataService;
            this.roadNameDataService = roadNameDataService;
        }

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="streetName">Street name.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public Tuple<NetworkLinkDTO, SqlGeometry> GetNearestNamedRoad(DbGeometry operationalObjectPoint, string streetName)
        {
            List<string> categoryNamesSimpleLists = new List<string>
            {
                ReferenceDataCategoryNames.NetworkLinkType,
            };

            var referenceDataCategoryList = networkManagerIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;

            return streetNetworkDataService.GetNearestNamedRoad(operationalObjectPoint, streetName, referenceDataCategoryList);
        }

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public Tuple<NetworkLinkDTO, SqlGeometry> GetNearestSegment(DbGeometry operationalObjectPoint)
        {
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

            return streetNetworkDataService.GetNearestSegment(operationalObjectPoint, referenceDataCategoryList);
        }

        /// <summary>
        /// Get the street DTO for operational object.
        /// </summary>
        /// <param name="networkLinkID">networkLink unique identifier Guid.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public NetworkLinkDTO GetNetworkLink(Guid networkLinkID)
        {
            return streetNetworkDataService.GetNetworkLink(networkLinkID);
        }

        /// <summary> This method is used to get the road links crossing the access link </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLinkCoordinates">access link coordinate array</param> <returns>List<NetworkLinkDTO></returns>
        public List<NetworkLinkDTO> GetCrossingNetworkLinks(string boundingBoxCoordinates, DbGeometry accessLinkCoordinates)
        {
            return streetNetworkDataService.GetCrossingNetworkLink(boundingBoxCoordinates, accessLinkCoordinates);
        }

        /// <summary>
        /// This method is used to fetch data for OSRoadLink.
        /// </summary>
        /// <param name="toid">toid unique identifier for OSRoadLink</param>
        /// <returns>returns Route Hierarchy as string</returns>
        public async Task<string> GetOSRoadLink(string toid)
        {
            return await osRoadLinkDataService.GetOSRoadLink(toid);
        }

        /// <summary>
        /// This method fetches data for RoadLinks
        /// </summary>
        /// <param name="boundarybox">boundaryBox as string.</param>
        /// <param name="uniGuid">Unit unique identifier.</param>
        /// <returns>RoadLink object</returns>
        public string GetRoadRoutes(string boundarybox, Guid uniGuid)
        {
            string roadLinkJsonData = null;

            if (!string.IsNullOrEmpty(boundarybox))
            {
                var boundingBoxCoordinates =
                    GetRoadNameCoordinatesDatabyBoundarybox(boundarybox.Split(Constants.Comma[0]));
                roadLinkJsonData =
                    GetRoadLinkJsonData(roadNameDataService.GetRoadRoutes(boundingBoxCoordinates, uniGuid));
            }

            return roadLinkJsonData;
        }

        #region basic_advanced search

        /// <summary>
        /// Fetch the street name for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>Task</returns>
        public async Task<List<StreetNameDTO>> FetchStreetNamesForBasicSearch(string searchText, Guid userUnit)
        {
            return await streetNetworkDataService.FetchStreetNamesForBasicSearch(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the count of street name
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of street name</returns>
        public async Task<int> GetStreetNameCount(string searchText, Guid userUnit)
        {
            return await streetNetworkDataService.GetStreetNameCount(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch street names for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>StreetNames</returns>
        public async Task<List<StreetNameDTO>> FetchStreetNamesForAdvanceSearch(string searchText, Guid unitGuid)
        {
            return await streetNetworkDataService.FetchStreetNamesForAdvanceSearch(searchText, unitGuid).ConfigureAwait(false);
        }

        #endregion basic_advanced search

        #region private methods

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
                                     Constants.Polygon,
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
                        roadLinkSqlGeometry = SqlGeometry.STLineFromWKB(new SqlBytes(resultCoordinates.AsBinary()), Constants.BNGCOORDINATESYSTEM).MakeValid();

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
                        roadLinkSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), Constants.BNGCOORDINATESYSTEM).MakeValid();
                        geometry.coordinates = new double[] { roadLinkSqlGeometry.STX.Value, roadLinkSqlGeometry.STY.Value };
                    }

                    Feature feature = new Feature();
                    feature.geometry = geometry;
                    feature.id = res.Id.ToString();
                    feature.type = Constants.FeatureType;
                    feature.properties = new Dictionary<string, Newtonsoft.Json.Linq.JToken> { { Constants.LayerType, Convert.ToString(OtherLayersType.RoadLink.GetDescription()) } };
                    geoJson.features.Add(feature);
                    i++;
                }
            }

            return JsonConvert.SerializeObject(geoJson);
        }

        #endregion private methods
    }
}