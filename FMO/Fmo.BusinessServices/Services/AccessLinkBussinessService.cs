namespace Fmo.BusinessServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Helpers;
    using Fmo.Helpers.Interface;
    using Newtonsoft.Json;
    using Microsoft.SqlServer.Types;
    using System.Data.SqlTypes;

    public class AccessLinkBussinessService : IAccessLinkBussinessService
    {
        private IAccessLinkRepository accessLinkRepository = default(IAccessLinkRepository);

        private ICreateOtherLayersObjects createOtherLayersObjects = default(ICreateOtherLayersObjects);

        public AccessLinkBussinessService(IAccessLinkRepository searchAccessLinkRepository, ICreateOtherLayersObjects createOtherLayerObjects)
        {
            this.accessLinkRepository = searchAccessLinkRepository;
            this.createOtherLayersObjects = createOtherLayerObjects;
        }

        public List<AccessLinkDTO> SearchAccessLink()
        {
            return accessLinkRepository.SearchAccessLink();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boundaryBox"></param>
        /// <returns></returns>
        public string GetAccessLinks(string boundaryBox)
        {
            try
            {
                var accessLinkCoordinates = GetData(null, boundaryBox.Split(','));
                return GetAccessLinkJsonData(accessLinkRepository.GetAccessLinks(accessLinkCoordinates));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstAccessLinkDTO"></param>
        /// <returns></returns>
        private string GetAccessLinkJsonData(List<AccessLinkDTO> lstAccessLinkDTO)
        {            
            AccessLinkDTO accessLinkDTOCollectionObj = new AccessLinkDTO();
            string json = string.Empty;

            var geoJson = new GeoJson
            {
                features = new List<Feature>()
            };

            foreach (var res in lstAccessLinkDTO)
            {
                Geometry geometry = new Geometry();

                geometry.type = res.AccessLinkLine.SpatialTypeName;

                var resultCoordinates = res.AccessLinkLine;

                SqlGeometry sqlGeo = null;
                if (geometry.type == "LineString")
                {
                    sqlGeo = SqlGeometry.STLineFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();

                    double[][] cords = new double[2][];

                    for (int pt = 1; pt <= sqlGeo.STNumPoints().Value; pt++)
                    {
                        double[] coordinates = new double[] { sqlGeo.STPointN(pt).STX.Value, sqlGeo.STPointN(pt).STY.Value };
                        cords[pt - 1] = coordinates;
                        //cords.Add(coordinates);
                    }

                    geometry.coordinates = new Coordinates(cords);
                }
                else
                {
                    sqlGeo = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();
                    double[] coordinates = new double[] { sqlGeo.STX.Value, sqlGeo.STY.Value };
                    geometry.coordinates = new Coordinates(coordinates);
                }

                Feature feature = new Feature();
                feature.geometry = geometry;

                feature.type = "Feature";
                feature.id = res.AccessLink_Id;
                feature.properties = new Dictionary<string, Newtonsoft.Json.Linq.JToken> { { "type", "accesslink" } };

                geoJson.features.Add(feature);
            }

            //accessLinkDTOCollectionObj.features = lstFeatures;
            //accessLinkDTOCollectionObj.type = "FeatureCollection";

            //json = JsonConvert.SerializeObject(accessLinkDTOCollectionObj, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return geoJson.getJson().ToString();
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
    }
}
