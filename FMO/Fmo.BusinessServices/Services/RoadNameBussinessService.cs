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

        public MemoryStream GetRoadRoutes(string bbox)
        {
            OsRoadLinkDTO routeLinkFeatureCollections = new OsRoadLinkDTO();
            string[] bboxArr = bbox.Split(',');
            var coordinates = GetData(null, bboxArr);

            List<OsRoadLinkDTO> osRoadLinkDTO = roadNameRepository.GetRoadRoutes(coordinates);

            List<Feature> lstFeatures = new List<Feature>();

            string json = string.Empty;

            foreach (var res in osRoadLinkDTO)
            {
                Geometry geometry = new Geometry();

                geometry.type = res.CentreLineGeometry.SpatialTypeName;

                var resultCoordinates = res.CentreLineGeometry;

                geometry.coordinates = new object();

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

                    geometry.coordinates = cords;
                }
                else
                {
                    sqlGeo = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();
                    double[] coordinatesval = new double[] { sqlGeo.STX.Value, sqlGeo.STY.Value };
                    geometry.coordinates = coordinatesval;
                }

                Feature features = new Feature();
                features.geometry = geometry;

                features.type = "Feature";
                //  features.properties = new Properties { type = "roadlink" };

                lstFeatures.Add(features);
            }

            //routeLinkFeatureCollections.features = lstFeatures;
            //routeLinkFeatureCollections.type = "FeatureCollection";
            json = JsonConvert.SerializeObject(routeLinkFeatureCollections,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            var resultBytes = Encoding.UTF8.GetBytes(json);
            return new MemoryStream(resultBytes);
        }

        public string GetData(string query, params object[] parameters)
        {
            string x1 = Convert.ToString(parameters[0]);
            string y1 = Convert.ToString(parameters[1]);
            string x2 = Convert.ToString(parameters[2]);
            string y2 = Convert.ToString(parameters[3]);


            string coordinates = "POLYGON((" + x1 + " " + y1 + ", " + x1 + " " + y2 + ", " + x2 + " " + y2 + ", " + x2 + " " + y1 + ", " + x1 + " " + y1 + "))";

            return coordinates;
        }
    }
}
