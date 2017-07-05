using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.Operational.SearchManager.WebAPI.BusinessService
{
    public interface ISearchBusinessService
    {
        /// <summary>
        /// This method fetches data for basic search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>
        /// SearchResult DTO
        /// </returns>
        Task<SearchResultDTO> FetchBasicSearchDetails(string searchText, Guid userUnit);

        /// <summary>
        /// This method fetches data for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>
        /// SearchResult DTO
        /// </returns>
        Task<SearchResultDTO> FetchAdvanceSearchDetails(string searchText, Guid userUnit);
    }
}