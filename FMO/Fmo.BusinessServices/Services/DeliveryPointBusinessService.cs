using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common;
using Fmo.Common.Constants;
using Fmo.Common.Enums;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Helpers;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json.Linq;

namespace Fmo.BusinessServices.Services
{
    /// <summary>
    /// This class contains methods for fetching Delivery Points data.
    /// </summary>
    public class DeliveryPointBusinessService : IDeliveryPointBusinessService
    {
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);
        private IAddressLocationRepository addressLocationRepository = default(IAddressLocationRepository);

        public DeliveryPointBusinessService(IDeliveryPointsRepository deliveryPointsRepository, IAddressLocationRepository addressLocationRepository)
        {
            this.deliveryPointsRepository = deliveryPointsRepository;
            this.addressLocationRepository = addressLocationRepository;
        }

        /// <summary>
        /// This Method is used to fetch Delivery Points Co-ordinates.
        /// </summary>
        /// <param name="boundaryBox">Boundarybox as string</param>
        /// <param name="unitGuid">Unit unique identifier.</param>
        /// <returns>Object</returns>
        public object GetDeliveryPoints(string boundaryBox, Guid unitGuid)
        {
            try
            {
                if (!string.IsNullOrEmpty(boundaryBox))
                {
                    var coordinates = GetDeliveryPointsCoordinatesDatabyBoundingBox(boundaryBox.Split(Constants.Comma[0]));
                    return GetDeliveryPointsJsonData(deliveryPointsRepository.GetDeliveryPoints(coordinates, unitGuid));
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
        /// This method is used to fetch address location data for positioning of Delivery Points on the basis of udprn.
        /// </summary>
        /// <param name="udprn">The UDPRN number</param>
        /// <returns>The coordinates of the delivery point</returns>
        public AddressLocationDTO GetAddressLocationByUDPRN(int udprn)
        {
            try
            {
                return addressLocationRepository.GetAddressLocationByUDPRN(udprn);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch the Delivery point for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText, Guid userUnit)
        {
            return await deliveryPointsRepository.FetchDeliveryPointsForBasicSearch(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the count of delivery point
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of delivery point</returns>
        public async Task<int> GetDeliveryPointsCount(string searchText, Guid userUnit)
        {
            return await deliveryPointsRepository.GetDeliveryPointsCount(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// This method is used to fetch delivery points for advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// Task List of Delivery Point Dto
        /// </returns>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid)
        {
            return await deliveryPointsRepository.FetchDeliveryPointsForAdvanceSearch(searchText, unitGuid);
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
                        { Constants.BuildingName, point.PostalAddress.BuildingName },
                        { Constants.BuildingNumber, point.PostalAddress.BuildingNumber },
                        { Constants.Postcode, point.PostalAddress.Postcode },
                        { Constants.StreetName, point.PostalAddress.BuildingName },
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
        /// <param name="parameters">parameters as object</param>
        /// <returns>coordinates</returns>
        private static string GetDeliveryPointsCoordinatesDatabyBoundingBox(params object[] parameters)
        {
            string coordinates = string.Empty;

            if (parameters != null && parameters.Length == 4)
            {
                coordinates = string.Format(
                                     Constants.Polygon,
                                     Convert.ToString(parameters[0]),
                                     Convert.ToString(parameters[1]),
                                     Convert.ToString(parameters[0]),
                                     Convert.ToString(parameters[3]),
                                     Convert.ToString(parameters[2]),
                                     Convert.ToString(parameters[3]),
                                     Convert.ToString(parameters[2]),
                                     Convert.ToString(parameters[1]),
                                     Convert.ToString(parameters[0]),
                                     Convert.ToString(parameters[1]));
            }

            return coordinates;
        }
    }
}