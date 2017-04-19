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
        private ISearchBusinessService searchBussinessService = default(ISearchBusinessService);

        public SearchController(ISearchBusinessService _searchBussinessService)
        {
            searchBussinessService = _searchBussinessService;
        }

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

        // GET: api/values
        // [HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}