using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using System.IO;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using Newtonsoft.Json;
using Fmo.Helpers;

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
        /// 
        /// </summary>
        /// <param name="boundarybox"></param>
        /// <returns></returns>
        public string GetRoadRoutes(string boundarybox)
        {
            try
            {
                var coordinates = GetData(null, boundarybox.Split(','));
                return GetRoadLinkJsonData(roadNameRepository.GetRoadRoutes(coordinates));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string GetData(string query, params object[] parameters)
        {
            string coordinates = string.Empty;

            if (parameters != null && parameters.Length == 4)
            {
                coordinates = "POLYGON((" + Convert.ToString(parameters[0]) + " " + Convert.ToString(parameters[1]) + ", "
                                                             + Convert.ToString(parameters[0]) + " " + Convert.ToString(parameters[3]) + ", "
                                                             + Convert.ToString(parameters[2]) + " " + Convert.ToString(parameters[3]) + ", "
                                                             + Convert.ToString(parameters[2]) + " " + Convert.ToString(parameters[1]) + ", "
                                                             + Convert.ToString(parameters[0]) + " " + Convert.ToString(parameters[1]) + "))";
            }

            return coordinates;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="osRoadLinkDTO"></param>
        /// <returns></returns>
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

                    SqlGeometry sqlGeo = null;
                    if (geometry.type == "LineString")
                    {
                        sqlGeo = SqlGeometry.STLineFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();

                        List<double[]> cords = new List<double[]>();

                        for (int pt = 1; pt <= sqlGeo.STNumPoints().Value; pt++)
                        {
                            double[] coordinatesval = new double[] { sqlGeo.STPointN(pt).STX.Value, sqlGeo.STPointN(pt).STY.Value };
                            cords.Add(coordinatesval);
                        }

                        geometry.coordinates = new Coordinates(cords);
                    }
                    else
                    {
                        sqlGeo = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();
                        double[] coordinatesval = new double[] { sqlGeo.STX.Value, sqlGeo.STY.Value };
                        geometry.coordinates = new Coordinates(coordinatesval);
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
            return geoJson.getJson().ToString();
        }
    }
}
