using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    /// <summary>
    /// This interface contains declarations of methods for basic and advance search of street network
    /// </summary>
    public interface IStreetNetworkRepository
    {
        /// <summary>
        /// Fetch street name for Basic Search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <returns>StreetName DTO</returns>
        Task<List<StreetNameDTO>> FetchStreetNamesForBasicSearch(string searchText);

        /// <summary>
        /// Fetch street names for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <returns>StreetName DTO</returns>
        Task<List<StreetNameDTO>> FetchStreetNamesForAdvanceSearch(string searchText);

        /// <summary>
        /// Get the count of street name
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <returns>The total count of street name</returns>
        Task<int> GetStreetNameCount(string searchText);
    }
}