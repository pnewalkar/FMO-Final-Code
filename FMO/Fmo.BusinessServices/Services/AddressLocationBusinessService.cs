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

        /// <summary>
        /// This method is used to fetch data for Access Links.
        /// </summary>
        /// <param name="uDPRN">UDPRN</param>
        /// <returns>
        /// Address Location DTO
        /// </returns>
        public object GetAddressLocationByUDPRN(int uDPRN)
        {
            AddressLocationDTO addressLocationDto = this.addressLocationRepository.GetAddressLocationByUDPRN(uDPRN);
            return GetAddressLocationJsonData(addressLocationDto);
        }

        /// <summary>
        /// This method is used to fetch GeoJson data for Address Location.
        /// </summary>
        /// <param name="addressLocationDto">Address Location DTO</param>
        /// <returns>lstDeliveryPointDTO</returns>
        private static object GetAddressLocationJsonData(AddressLocationDTO addressLocationDto)
        {
            var addressLocationGeoJson = new GeoJson
            {
                features = new List<Feature>()
            };

            if (addressLocationDto?.LocationXY != null)
            {
                SqlGeometry addressLocationSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(addressLocationDto.LocationXY.AsBinary()), Constants.BNGCOORDINATESYSTEM);

                var feature = new Feature
                {
                    id = addressLocationDto.ID.ToString(),
                    properties = new Dictionary<string, JToken>
                    {
                        { Constants.UDPRN, addressLocationDto.UDPRN },
                        { Constants.Latitude, addressLocationDto.Lattitude },
                        { Constants.Longitude, addressLocationDto.Longitude },
                    },
                    geometry = new Geometry
                    {
                        coordinates = new double[] { addressLocationSqlGeometry.STX.Value, addressLocationSqlGeometry.STY.Value }
                    }
                };

                addressLocationGeoJson.features.Add(feature);
            }

            return addressLocationGeoJson;
        }
    }
}
