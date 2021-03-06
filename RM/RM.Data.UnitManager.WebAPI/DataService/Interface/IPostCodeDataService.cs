﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
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
        /// <param name="searchInputs">SearchInputDataDto</param>
        /// <returns>collection of PostcodeDataDTO</returns>
        Task<IEnumerable<PostcodeDataDTO>> GetPostcodeUnitForBasicSearch(SearchInputDataDto searchInputs);

        /// <summary>
        /// Gets count of postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchInputs">SearchInputDataDto</param>
        /// <returns>count of PostcodeUnit</returns>
        Task<int> GetPostcodeUnitCount(SearchInputDataDto searchInputs);

        /// <summary>
        /// Gets all postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchInputs">SearchInputDataDto</param>
        /// <returns>collection of PostcodeDataDTO</returns>
        Task<IEnumerable<PostcodeDataDTO>> GetPostcodeUnitForAdvanceSearch(SearchInputDataDto searchInputs);

        /// <summary>
        /// Get post code ID by passing postcode.
        /// </summary>
        /// <param name="PostcodeDataDTO"> PostcodeDataDTO</param>
        /// <returns>Post code ID</returns>
        Task<PostcodeDataDTO> GetPostcodeID(string postCode);

        /// <summary>
        /// Gets approx location based on the postal code.
        /// </summary>
        /// <param name="postcode">Postal code</param>
        /// <param name="unitId">Unique identifier for unit.</param>
        /// <returns>The approx location for the given postal code.</returns>
        Task<DbGeometry> GetApproxLocation(string postcode, Guid unitId);
    }
}