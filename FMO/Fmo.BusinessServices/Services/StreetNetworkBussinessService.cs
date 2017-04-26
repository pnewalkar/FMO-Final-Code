using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    public class StreetNetworkBussinessService : IStreetNetworkBusinessService
    {
        private IStreetNetworkRepository streetNetworkRepository = default(IStreetNetworkRepository);

        public StreetNetworkBussinessService(IStreetNetworkRepository streetNetworkRepository)
        {
            this.streetNetworkRepository = streetNetworkRepository;
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
    }
}
