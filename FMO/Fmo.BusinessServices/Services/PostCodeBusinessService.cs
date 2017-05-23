using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    /// <summary>
    /// This class contains methods for fetching post code data.
    /// </summary>
    /// <seealso cref="Fmo.BusinessServices.Interfaces.IPostCodeBusinessService" />
    public class PostCodeBusinessService : IPostCodeBusinessService
    {
        private IPostCodeRepository postCodeRepository = default(IPostCodeRepository);

        /// <summary>
        /// Initializes a new instance of the <see cref="PostCodeBusinessService"/> class and other classes.
        /// </summary>
        /// <param name="postCodeRepository">IPostCodeRepository reference</param>
        public PostCodeBusinessService(IPostCodeRepository postCodeRepository)
        {
            this.postCodeRepository = postCodeRepository;
        }

        /* public async Task<List<PostCodeDTO>> FetchPostCodeUnit(string searchText)
         {
             return await postCodeRepository.FetchPostCodeUnitForAdvanceSearch(searchText);
         }
         */

        /// <summary>
        /// Fetch the post code for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText, Guid userUnit)
        {
            return await postCodeRepository.FetchPostCodeUnitForBasicSearch(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the count of post code
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of post code</returns>
        public async Task<int> GetPostCodeUnitCount(string searchText, Guid userUnit)
        {
            return await postCodeRepository.GetPostCodeUnitCount(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch the post code for advanced Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText, Guid userUnit)
        {
            return await postCodeRepository.FetchPostCodeUnitForAdvanceSearch(searchText, userUnit);
        }
    }
}