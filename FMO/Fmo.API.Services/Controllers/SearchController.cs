using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// This class contains methods used for fetching Basic search and advance search results.
    /// </summary>

    [Route("api/[controller]")]
    public class SearchController : FmoBaseController
    {
        private ISearchBusinessService searchBussinessService = default(ISearchBusinessService);

        public SearchController(ISearchBusinessService searchBussinessService)
        {
            this.searchBussinessService = searchBussinessService;
        }

        /// <summary>
        /// Fetch results from entities using basic search
        /// </summary>
        /// <param name="searchText">The text to be searched from the entities.</param>
        /// <returns>The result set after filtering the values.</returns>
        [Authorize]
        [HttpGet("BasicSearch")]
        public async Task<SearchResultDTO> BasicSearch(string searchText)
        {
            var unitGuid = this.CurrentUserUnit;
            return await searchBussinessService.FetchBasicSearchDetails(searchText, unitGuid);
        }

        /// <summary>
        /// Fetch results from Advanced search entities.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <returns> Search Result Dto</returns>
        [Authorize]
        [HttpGet("AdvanceSearch")]
        public async Task<SearchResultDTO> AdvanceSearch(string searchText)
        {
            var unitGuid = this.CurrentUserUnit;
            return await searchBussinessService.FetchAdvanceSearchDetails(searchText, unitGuid);
        }
    }
}