namespace Fmo.BusinessServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using Common.Constants;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Helpers;
    using Microsoft.SqlServer.Types;
    using Newtonsoft.Json.Linq;

    public class AddressLocationBusinessService : IAddressLocationBusinessService
    {
        private IAddressLocationRepository addressLocationRepository = default(IAddressLocationRepository);

        public AddressLocationBusinessService(IAddressLocationRepository addressLocationRepository)
        {
            this.addressLocationRepository = addressLocationRepository;
        }

        public object GetAddressLocationByUDPRN(int uDPRN)
        {
            try
            {
                AddressLocationDTO addressLocationDTO = this.addressLocationRepository.GetAddressLocationByUDPRN(uDPRN);
                return GetAddressLocationJsonData(addressLocationDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to fetch GeoJson data for Address Location.
        /// </summary>
        /// <param name="addressLocationDTO">Address Location DTO</param>
        /// <returns>lstDeliveryPointDTO</returns>
        private static object GetAddressLocationJsonData(AddressLocationDTO addressLocationDTO)
        {
            var addressLocationGeoJson = new GeoJson
            {
                features = new List<Feature>()
            };

            if (addressLocationDTO != null)
            {
                if (addressLocationDTO.LocationXY != null)
                {
                    SqlGeometry addressLocationSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(addressLocationDTO.LocationXY.AsBinary()), Constants.BNGCOORDINATESYSTEM);

                    var feature = new Feature
                    {
                        id = addressLocationDTO.ID.ToString(),
                        properties = new Dictionary<string, JToken>
                        {
                            { Constants.UDPRN, addressLocationDTO.UDPRN },
                            { Constants.Latitude, addressLocationDTO.Lattitude },
                            { Constants.Longitude, addressLocationDTO.Longitude },
                        },
                        geometry = new Geometry
                        {
                            coordinates = new double[] { addressLocationSqlGeometry.STX.Value, addressLocationSqlGeometry.STY.Value }
                        }
                    };

                    addressLocationGeoJson.features.Add(feature);
                }
            }

            return addressLocationGeoJson;
        }
    }
}
