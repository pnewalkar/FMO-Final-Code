﻿namespace Fmo.BusinessServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Helpers;
    using Interfaces;
    using Microsoft.SqlServer.Types;
    using Newtonsoft.Json.Linq;
    using Common.Enums;
    using Common;
    using Common.Constants;

    /// <summary>
    /// This class contains methods for fetching Delivery Points data.
    /// </summary>
    public class DeliveryPointBusinessService : IDeliveryPointBusinessService
    {
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);

        public DeliveryPointBusinessService(IDeliveryPointsRepository deliveryPointsRepository)
        {
            this.deliveryPointsRepository = deliveryPointsRepository;
        }

        /// <summary>
        /// This Method is used to fetch Delivery Points Co-ordinates.
        /// </summary>
        /// <param name="boundaryBox">Boundarybox as string</param>
        /// <returns>Object</returns>
        public object GetDeliveryPoints(string boundaryBox)
        {
            try
            {
                var coordinates = GetData(null, boundaryBox.Split(','));
                return GetDeliveryPointsJsonData(deliveryPointsRepository.GetDeliveryPoints(coordinates));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get coordinates of the delivery point by UDPRN
        /// </summary>
        /// <param name="udprn">The UDPRN number</param>
        /// <returns>The coordinates of the delivery point</returns>
        public object GetDeliveryPointByUDPRN(int udprn)
        {
            try
            {
                return GetDeliveryPointsJsonData(deliveryPointsRepository.GetDeliveryPointListByUDPRN(udprn));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to fetch GeoJson data for Delivery Point.
        /// </summary>
        /// <param name="lstDeliveryPointDTO">List of Delivery Point Dto</param>
        /// <returns>lstDeliveryPointDTO</returns>
        private static object GetDeliveryPointsJsonData(List<DeliveryPointDTO> lstDeliveryPointDTO)
        {
            var deliveryPointGeoJson = new GeoJson
            {
                features = new List<Feature>()
            };

            if (lstDeliveryPointDTO != null && lstDeliveryPointDTO.Count > 0)
            {
                foreach (var point in lstDeliveryPointDTO)
                {
                    SqlGeometry deliveryPointSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(point.LocationXY.AsBinary()), 0);

                    var feature = new Feature
                    {
                        id = point.DeliveryPoint_Id,
                        properties = new Dictionary<string, JToken>
                    {
                        { "name", point.PostalAddress.BuildingName },
                        { "number", point.PostalAddress.BuildingNumber },
                        { "postcode", point.PostalAddress.Postcode },
                        { "street_name", point.PostalAddress.BuildingName },
                        { Constants.LayerType, Convert.ToString(OtherLayersType.DeliveryPoint.GetDescription()) }
                    },
                        geometry = new Geometry
                        {
                            coordinates = new double[] { deliveryPointSqlGeometry.STX.Value, deliveryPointSqlGeometry.STY.Value }
                        }
                    };
                    deliveryPointGeoJson.features.Add(feature);
                }
            }

            return deliveryPointGeoJson;
        }

        /// <summary>
        /// This Method is used to fetch Delivery Points Co-ordinates.
        /// </summary>
        /// <param name="query">query as string</param>
        /// <param name="parameters">parameters as object</param>
        /// <returns>coordinates</returns>
        private static string GetData(string query, params object[] parameters)
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