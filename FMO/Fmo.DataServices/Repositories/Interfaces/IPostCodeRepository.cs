using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    /// <summary>
    /// Interface to perfrom CRUD operations on postcode
    /// </summary>
    public interface IPostCodeRepository
    {
        /// <summary>
        ///  Fetch postcode for basic search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <returns>PostCode DTO</returns>
        Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText, Guid userUnit);

        /// <summary>
        /// Fetch Postcode Unit for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <returns>PostCode DTO</returns>
        Task<int> GetPostCodeUnitCount(string searchText, Guid userUnit);

        Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText, Guid unitGuid);

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        Guid GetPostCodeID(string postCode);
    }
}