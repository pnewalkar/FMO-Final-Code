using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.Operational.SearchManager.WebAPI.BusinessService
{
    /// <summary>
    /// Interface for search business service
    /// </summary>
    public interface ISearchBusinessService
    {
        /// <summary>
        /// This method gets data for basic search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="userUnit">The user unit.</param>
        /// <param name="currentUserUnitType">The user unit type.</param>
        /// <returns>
        /// SearchResult DTO
        /// </returns>
        Task<SearchResultDTO> GetBasicSearchDetails(string searchText, Guid userUnit, string currentUserUnitType);

        /// <summary>
        /// This method gets data for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="userUnit">The user unit.</param>
        /// <param name="currentUserUnitType">The user unit type.</param>
        /// <returns>
        /// SearchResult DTO
        /// </returns>
        Task<SearchResultDTO> GetAdvanceSearchDetails(string searchText, Guid userUnit, string currentUserUnitType);
    }
}