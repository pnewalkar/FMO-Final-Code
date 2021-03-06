﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RM.DataManagement.RoadName.WebAPI.BusinessService.Interface;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.DataManagement.RoadName.WebAPI.BusinessService
{
    public class RoadNameBusinessService : IRoadNameBusinessService
    {
        /// <summary>
        /// This class contains methods for fetching data for RoadLinks.
        /// </summary>
        private IRoadNameRepository roadNameRepository = default(IRoadNameRepository);

        /// <summary>
        /// Initializes a new instance of the <see cref="RoadNameBusinessService"/> class.
        /// </summary>
        /// <param name="roadNameRepository">The road name repository.</param>
        public RoadNameBusinessService(IRoadNameRepository roadNameRepository)
        {
            this.roadNameRepository = roadNameRepository;
        }

        /// <summary>
        /// This method fetches data for RoadLinks
        /// </summary>
        /// <param name="boundarybox">boundaryBox as string.</param>
        /// <param name="uniGuid">Unit unique identifier.</param>
        /// <returns>RoadLink object</returns>
        public string GetRoadRoutes(string boundarybox, Guid uniGuid)
        {
            try
            {
                string roadLinkJsonData = null;

                if (!string.IsNullOrEmpty(boundarybox))
                {
                    var boundingBoxCoordinates = GetRoadNameCoordinatesDatabyBoundarybox(boundarybox.Split(Constants.Comma[0]));
                    roadLinkJsonData = GetRoadLinkJsonData(roadNameRepository.GetRoadRoutes(boundingBoxCoordinates, uniGuid));
                }

                return roadLinkJsonData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method fetches co-ordinates of roadlink
        /// </summary>
        /// <param name="roadLinkparameters"> roadLinkparameters as object </param>
        /// <returns> roadlink coordinates </returns>
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
        /// <returns> roadlink object</returns>
        /// <param name="networkLinkDTO"> networkLinkDTO as list of NetworkLinkDTO </param>
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
    }
}