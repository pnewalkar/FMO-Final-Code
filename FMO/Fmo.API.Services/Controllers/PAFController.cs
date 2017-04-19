﻿using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Interface;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// PAF controller to handle PAF API request from windows service
    /// </summary>
    [Route("api/[controller]")]
    public class PAFController : Controller
    {
        private IPostalAddressBusinessService postalAddressBusinessService = default(IPostalAddressBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public PAFController(IPostalAddressBusinessService postalAddressBusinessService, ILoggingHelper loggingHelper)
        {
            this.postalAddressBusinessService = postalAddressBusinessService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Api to save PAF details in DB.
        /// </summary>
        /// <param name="lstAddressLocationUSRPOSTDTO">List of posatl address DTO</param>
        /// <returns></returns>
        [HttpPost("SavePAFDetails")]
        public bool SavePAFDetails([FromBody] List<PostalAddressDTO> postalAddress)
        {
            if (postalAddress != null && postalAddress.Count > 0)
                return postalAddressBusinessService.SavePAFDetails(postalAddress);
            else
                return false;
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