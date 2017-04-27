﻿using System;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    /// <summary>
    /// This interface contains declarations for basic and advance search methods
    /// </summary>
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