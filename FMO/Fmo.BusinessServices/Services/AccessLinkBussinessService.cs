namespace Fmo.BusinessServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.Common.Constants;
    using Fmo.Common.Enums;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Helpers;
    using Microsoft.SqlServer.Types;
    using Newtonsoft.Json;

    /// <summary>
    /// This class contains methods for fetching data for AccessLinks
    /// </summary>
    public class AccessLinkBussinessService : IAccessLinkBussinessService
    {
        private IAccessLinkRepository accessLinkRepository = default(IAccessLinkRepository);

        public AccessLinkBussinessService(IAccessLinkRepository searchAccessLinkRepository)
        {
            this.accessLinkRepository = searchAccessLinkRepository;
        }

        public List<AccessLinkDTO> SearchAccessLink()
        {
            return accessLinkRepository.SearchAccessLink();
        }

        /// <summary>
        ///  This method fetches data for AccsessLinks
        /// </summary>
        /// <param name="boundaryBox"> boundaryBox as string </param>
        /// <returns> AccsessLink object</returns>
        public string GetAccessLinks(string boundaryBox)
        {
            try
            {
                if (!string.IsNullOrEmpty(boundaryBox))
                {
                    var accessLinkCoordinates = GetData(boundaryBox.Split(','));
                    return GetAccessLinkJsonData(accessLinkRepository.GetAccessLinks(accessLinkCoordinates));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method fetches geojson data for accesslink
        /// </summary>
        /// <param name="lstAccessLinkDTO"> accesslink as list of AccessLinkDTO</param>
        /// <returns> AccsessLink object</returns>
        private static string GetAccessLinkJsonData(List<AccessLinkDTO> lstAccessLinkDTO)
        {
            var geoJson = new GeoJson
            {
                features = new List<Feature>()
            };
            if (lstAccessLinkDTO != null && lstAccessLinkDTO.Count > 0)
            {
                foreach (var res in lstAccessLinkDTO)
                {
                    Geometry geometry = new Geometry();

                    geometry.type = res.AccessLinkLine.SpatialTypeName;

                    var resultCoordinates = res.AccessLinkLine;

                    SqlGeometry accessLinksqlGeometry = null;
                    if (geometry.type == Convert.ToString(GeometryType.LineString))
                    {
                        accessLinksqlGeometry = SqlGeometry.STLineFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();

                        List<List<double>> cords = new List<List<double>>();

                        for (int pt = 1; pt <= accessLinksqlGeometry.STNumPoints().Value; pt++)
                        {
                            List<double> accessLinkCoordinates = new List<double> { accessLinksqlGeometry.STPointN(pt).STX.Value, accessLinksqlGeometry.STPointN(pt).STY.Value };
                            cords.Add(accessLinkCoordinates);
                        }

                        geometry.coordinates = cords;
                    }
                    else
                    {
                        accessLinksqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();
                        geometry.coordinates = new double[] { accessLinksqlGeometry.STX.Value, accessLinksqlGeometry.STY.Value };
                    }

                    Feature feature = new Feature();
                    feature.geometry = geometry;

                    feature.type = Constants.FeatureType;
                    feature.id = res.AccessLink_Id;
                    feature.properties = new Dictionary<string, Newtonsoft.Json.Linq.JToken> { { Constants.FeaturePropertyType, Constants.FeaturePropertyAccessLink } };

                    geoJson.features.Add(feature);
                }
            }

           return JsonConvert.SerializeObject(geoJson);
        }

        /// <summary>
        /// This method fetches co-ordinates of accesslink
        /// </summary>
        /// <param name="accessLinkParameters"> accessLinkParameters as object </param>
        /// <returns> accesslink coordinates</returns>
        private static string GetData(params object[] accessLinkParameters)
        {
            string coordinates = string.Empty;

            if (accessLinkParameters != null && accessLinkParameters.Length == 4)
            {
                coordinates = "POLYGON((" + Convert.ToString(accessLinkParameters[0]) + " " + Convert.ToString(accessLinkParameters[1]) + ", "
                                          + Convert.ToString(accessLinkParameters[0]) + " " + Convert.ToString(accessLinkParameters[3]) + ", "
                                          + Convert.ToString(accessLinkParameters[2]) + " " + Convert.ToString(accessLinkParameters[3]) + ", "
                                          + Convert.ToString(accessLinkParameters[2]) + " " + Convert.ToString(accessLinkParameters[1]) + ", "
                                          + Convert.ToString(accessLinkParameters[0]) + " " + Convert.ToString(accessLinkParameters[1]) + "))";
            }

            return coordinates;
        }
    }
}