using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IStreetNetworkRepository
    {
        /// <summary>
        /// Fetches the street names for basic search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List<StreetNameDTO></returns>
        Task<List<StreetNameDTO>> FetchStreetNamesForBasicSearch(string searchText, Guid unitGuid);

        /// <summary>
        /// Fetches the street names for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List<StreetNameDTO></returns>
        Task<List<StreetNameDTO>> FetchStreetNamesForAdvanceSearch(string searchText, Guid unitGuid);

        /// <summary>
        /// Gets the street name count.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>int</returns>
        Task<int> GetStreetNameCount(string searchText, Guid unitGuid);
    }
}