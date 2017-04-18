using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Helpers;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;

namespace Fmo.BusinessServices.Services
{
    public class RoadNameBussinessService : IRoadNameBussinessService
    {
        private IRoadNameRepository roadNameRepository = default(IRoadNameRepository);

        public RoadNameBussinessService(IRoadNameRepository roadNameRepository)
        {
            this.roadNameRepository = roadNameRepository;
        }

        public async Task<List<RoadNameDTO>> FetchRoadName()
        {
            return await roadNameRepository.FetchRoadName();
        }

        /// <summary>
        /// This method fetches data for RoadLinks
        /// </summary>
        /// <param name="boundaryBox"> boundaryBox as string </param>
        /// <returns>RoadLink object</returns>
        public string GetRoadRoutes(string boundaryBox)
        {
            try
            {
                var coordinates = GetData(null, boundaryBox.Split(','));
                return GetRoadLinkJsonData(roadNameRepository.GetRoadRoutes(coordinates));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method fetches co-ordinates of roadlink
        /// </summary>
        /// <param name="query"> query as string  </param>
        /// <param name="roadLinkparameters"> roadLinkparameters as object </param>
        /// <returns> roadlink coordinates </returns>
        private string GetData(string query, params object[] roadLinkparameters)
        {
            string coordinates = string.Empty;

            if (roadLinkparameters != null && roadLinkparameters.Length == 4)
            {
                coordinates = "POLYGON((" + Convert.ToString(roadLinkparameters[0]) + " " + Convert.ToString(roadLinkparameters[1]) + ", "
                                                             + Convert.ToString(roadLinkparameters[0]) + " " + Convert.ToString(roadLinkparameters[3]) + ", "
                                                             + Convert.ToString(roadLinkparameters[2]) + " " + Convert.ToString(roadLinkparameters[3]) + ", "
                                                             + Convert.ToString(roadLinkparameters[2]) + " " + Convert.ToString(roadLinkparameters[1]) + ", "
                                                             + Convert.ToString(roadLinkparameters[0]) + " " + Convert.ToString(roadLinkparameters[1]) + "))";
            }

            return coordinates;
        }

        /// <summary>
        /// This method fetches geojson data for roadlink
        /// </summary>
        /// <returns> roadlink object</returns>
        /// <param name="osRoadLinkDTO"> osRoadLinkDTO as list of RoadLinkDTO </param>
        private string GetRoadLinkJsonData(List<OsRoadLinkDTO> osRoadLinkDTO)
        {
            OsRoadLinkDTO routeLinkFeatureCollections = new OsRoadLinkDTO();
            string json = string.Empty;

            var geoJson = new GeoJson
            {
                features = new List<Feature>()
            };

            if (osRoadLinkDTO != null && osRoadLinkDTO.Count > 0)
            {
                int i = 1;
                foreach (var res in osRoadLinkDTO)
                {
                    Geometry geometry = new Geometry();

                    geometry.type = res.CentreLineGeometry.SpatialTypeName;

                    var resultCoordinates = res.CentreLineGeometry;

                    SqlGeometry roadLinkSqlGeometry = null;
                    if (geometry.type == "LineString")
                    {
                        roadLinkSqlGeometry = SqlGeometry.STLineFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();

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
                        roadLinkSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();
                        geometry.coordinates = new double[] { roadLinkSqlGeometry.STX.Value, roadLinkSqlGeometry.STY.Value };
                    }

                    Feature feature = new Feature();
                    feature.geometry = geometry;
                    feature.id = i;
                    feature.type = "Feature";
                    feature.properties = new Dictionary<string, Newtonsoft.Json.Linq.JToken> { { "type", "roadlink" } };
                    geoJson.features.Add(feature);
                    i++;
                }
            }

            return JsonConvert.SerializeObject(geoJson);
        }
    }
}