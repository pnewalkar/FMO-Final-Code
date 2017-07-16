using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;

namespace RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces
{
    /// <summary>
    /// Interface to perfrom CRUD operations on postcode
    /// </summary>
    public interface IPostcodeDataService
    {
        /// <summary>
        /// Gets first five postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="unitlocationId"></param>
        /// <param name="postcodeTypeGUID"></param>
        /// <returns>list of PostCodeDataDTO</returns>
        Task<IEnumerable<PostCodeDataDTO>> GetPostcodeUnitForBasicSearch(string searchText, Guid unitlocationId, Guid postcodeTypeGUID);

        /// <summary>
        /// Gets count of postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="unitlocationId"></param>
        /// <param name="postcodeTypeGUID"></param>
        /// <returns>count of postcodeunits</returns>
        Task<int> GetPostcodeUnitCount(string searchText, Guid unitlocationId, Guid postcodeTypeGUID);

        /// <summary>
        /// Gets all postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="unitlocationId"></param>
        /// <param name="postcodeTypeGUID"></param>
        /// <returns>list of PostCodeDataDTO</returns>
        Task<IEnumerable<PostCodeDataDTO>> GetPostcodeUnitForAdvanceSearch(string searchText, Guid unitlocationId, Guid postcodeTypeGUID);

        /// <summary>
        /// Get post code ID by passing postcode.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        Task<Guid> GetPostcodeID(string postCode);
    }
}