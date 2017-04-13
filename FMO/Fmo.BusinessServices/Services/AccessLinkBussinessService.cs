namespace Fmo.BusinessServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Helpers;
    using Fmo.Helpers.Interface;
    using Microsoft.SqlServer.Types;
    using Newtonsoft.Json;

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

                    List<List<double>> cords = new List<List<double>>();

                    for (int pt = 1; pt <= sqlGeo.STNumPoints().Value; pt++)
                    {
                        List<double> coordinates = new List<double> { sqlGeo.STPointN(pt).STX.Value, sqlGeo.STPointN(pt).STY.Value };
                        cords.Add(coordinates);

                        //cords.Add(coordinates);
                    }

                    geometry.coordinates = cords;
                }
                else
                {
                    sqlGeo = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();
                    geometry.coordinates = new double[] { sqlGeo.STX.Value, sqlGeo.STY.Value };
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
            return JsonConvert.SerializeObject(geoJson);
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