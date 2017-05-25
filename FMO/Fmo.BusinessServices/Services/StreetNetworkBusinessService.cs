﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Constants;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Microsoft.SqlServer.Types;
using Fmo.Common.ObjectParser;

namespace Fmo.BusinessServices.Services
{
    /// <summary>
    /// This class contains methods for fetching street data.
    /// </summary>
    /// <seealso cref="Fmo.BusinessServices.Interfaces.IStreetNetworkBusinessService" />
    public class StreetNetworkBusinessService : IStreetNetworkBusinessService
    {
        private IStreetNetworkRepository streetNetworkRepository = default(IStreetNetworkRepository);
        private IReferenceDataBusinessService referenceDataBusinessService = default(IReferenceDataBusinessService);

        /// <summary>
        /// Initializes a new instance of the <see cref="StreetNetworkBusinessService"/> class.
        /// </summary>
        /// <param name="streetNetworkRepository">The street network repository.</param>
        /// <param name="referenceDataBusinessService">The reference data business service.</param>
        public StreetNetworkBusinessService(IStreetNetworkRepository streetNetworkRepository, IReferenceDataBusinessService referenceDataBusinessService)
        {
            this.streetNetworkRepository = streetNetworkRepository;
            this.referenceDataBusinessService = referenceDataBusinessService;
        }

        /// <summary>
        /// Fetch the street name for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<List<StreetNameDTO>> FetchStreetNamesForBasicSearch(string searchText, Guid userUnit)
        {
            return await streetNetworkRepository.FetchStreetNamesForBasicSearch(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the count of street name
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of street name</returns>
        public async Task<int> GetStreetNameCount(string searchText, Guid userUnit)
        {
            return await streetNetworkRepository.GetStreetNameCount(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch street names for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>StreetNames</returns>
        public async Task<List<StreetNameDTO>> FetchStreetNamesForAdvanceSearch(string searchText, Guid unitGuid)
        {
            return await streetNetworkRepository.FetchStreetNamesForAdvanceSearch(searchText, unitGuid);
        }

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="streetName">Street name.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public Tuple<NetworkLinkDTO, SqlGeometry> GetNearestNamedRoad(DbGeometry operationalObjectPoint, string streetName)
        {
            List<string> categoryNames = new List<string>
            {
                ReferenceDataCategoryNames.NetworkLinkType
            };
            var referenceDataCategoryList = referenceDataBusinessService.GetReferenceDataCategoriesByCategoryNames(categoryNames);

            return streetNetworkRepository.GetNearestNamedRoad(operationalObjectPoint, streetName, referenceDataCategoryList);
        }

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public Tuple<NetworkLinkDTO, SqlGeometry> GetNearestSegment(DbGeometry operationalObjectPoint)
        {
            List<string> categoryNames = new List<string>
            {
               ReferenceDataCategoryNames.NetworkLinkType,
               ReferenceDataCategoryNames.AccessLinkParameters
            };
            var referenceDataCategoryList = referenceDataBusinessService.GetReferenceDataCategoriesByCategoryNames(categoryNames);

            return streetNetworkRepository.GetNearestSegment(operationalObjectPoint, referenceDataCategoryList);
        }

        /// <summary>
        /// Get the street DTO for operational object.
        /// </summary>
        /// <param name="networkLinkID">networkLink unique identifier Guid.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public NetworkLinkDTO GetNetworkLink(Guid networkLinkID)
        {
            return streetNetworkRepository.GetNetworkLink(networkLinkID);
        }

        /// <summary>
        /// This method is used to get the road links crossing the access link
        /// </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param>
        /// <param name="accessLinkCoordinates">access link coordinate array</param>
        /// <returns>List<NetworkLinkDTO></returns>
        public List<NetworkLinkDTO> GetCrossingNetworkLinks(string boundingBoxCoordinates, DbGeometry accessLinkCoordinates)
        {
            return streetNetworkRepository.GetCrossingNetworkLink(boundingBoxCoordinates, accessLinkCoordinates);
        }
    }
}
