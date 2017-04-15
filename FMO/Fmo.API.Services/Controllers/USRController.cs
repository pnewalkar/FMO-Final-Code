using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Fmo.Common.Interface;
using Fmo.DTO.FileProcessing;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    [Route("api/[controller]")]
    public class USRController : Controller
    {
        private IUSRBusinessService usrBusinessService = default(IUSRBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public USRController(IUSRBusinessService usrBusinessService, ILoggingHelper loggingHelper)
        {
            this.usrBusinessService = usrBusinessService;
            this.loggingHelper = loggingHelper;
        }

        [HttpPost("SaveUSRDetails")]
        public async Task SaveUSRDetails([FromBody]List<AddressLocationUSRPOSTDTO> lstAddressLocationUSRPOSTDTO)
        {
            try
            {
                await usrBusinessService.SaveUSRDetails(lstAddressLocationUSRPOSTDTO);
            }
            catch (Exception ex)
            {
                loggingHelper.LogError(ex);
            }
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
