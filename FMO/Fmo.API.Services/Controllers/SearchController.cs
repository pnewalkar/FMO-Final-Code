using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private ISearchBussinessService searchBussinessService = default(ISearchBussinessService);

        public SearchController(ISearchBussinessService _searchBussinessService)
        {
            searchBussinessService = _searchBussinessService;
        }

        /// <summary>
        /// Fetch results from entities using basic search
        /// </summary>
        /// <param name="searchText">The text to be searched from the entities.</param>
        /// <returns>The result set after filtering the values.</returns>
        [HttpGet("BasicSearch")]
        public async Task<SearchResultDTO> BasicSearch(string searchText)
        {
            return await searchBussinessService.FetchBasicSearchDetails(searchText);
        }

        [HttpGet("AdvanceSearch")]
        public async Task<SearchResultDTO> AdvanceSearch(string searchText)
        {
            return await searchBussinessService.FetchAdvanceSearchDetails(searchText);
        }
    }
}