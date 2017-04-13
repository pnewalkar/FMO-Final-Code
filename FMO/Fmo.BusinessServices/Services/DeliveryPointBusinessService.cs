namespace Fmo.BusinessServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Helpers;
    using Microsoft.SqlServer.Types;
    using Newtonsoft.Json.Linq;

    public class DeliveryPointBusinessService : IDeliveryPointBusinessService
    {
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);

        public DeliveryPointBusinessService(IDeliveryPointsRepository deliveryPointsRepository)
        {
            this.deliveryPointsRepository = deliveryPointsRepository;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="boundarybox"></param>
        /// <returns></returns>
        public object GetDeliveryPoints(string boundarybox)
        {
            try
            {
                var coordinates = GetData(null, boundarybox.Split(','));
                return GetDeliveryPointsJsonData(deliveryPointsRepository.GetDeliveryPoints(coordinates));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="udprn"></param>
        /// <returns></returns>
        public string GetDeliveryPointByUDPRN(int udprn)
        {
            try
            {
                return GetDeliveryPointsJsonData(deliveryPointsRepository.GetDeliveryPointListByUDPRN(udprn));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstDeliveryPointDTO"></param>
        /// <returns></returns>
        private object GetDeliveryPointsJsonData(List<DeliveryPointDTO> lstDeliveryPointDTO)
        {
            string jsonData = string.Empty;
            var geoJson = new GeoJson
            {
                features = new List<Feature>()
            };

            if (lstDeliveryPointDTO != null && lstDeliveryPointDTO.Count > 0)
            {
                foreach (var point in lstDeliveryPointDTO)
                {
                    SqlGeometry sqlGeo = SqlGeometry.STGeomFromWKB(new SqlBytes(point.LocationXY.AsBinary()), 0);

                    var feature = new Feature
                    {
                        id = point.DeliveryPoint_Id,
                        properties = new Dictionary<string, JToken>
                    {
                        { "name", point.PostalAddress.BuildingName },
                        { "number", point.PostalAddress.BuildingNumber },
                        { "postcode", point.PostalAddress.Postcode },
                        { "street_name", point.PostalAddress.BuildingName },
                        { "type", "deliverypoint" }
                    },
                        geometry = new Geometry
                        {
                            coordinates = new double[] { sqlGeo.STX.Value, sqlGeo.STY.Value }
                        }
                    };
                    geoJson.features.Add(feature);
                }
            }

            return geoJson;
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