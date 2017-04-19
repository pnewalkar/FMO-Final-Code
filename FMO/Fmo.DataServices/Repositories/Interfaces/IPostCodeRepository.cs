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
        Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText);

        Task<int> GetPostCodeUnitCount(string searchText);

        Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText);

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        Guid GetPostCodeID(string postCode);
    }
}