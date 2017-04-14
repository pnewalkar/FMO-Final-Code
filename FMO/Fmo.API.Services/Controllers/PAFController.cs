﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Fmo.Common.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
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

        [HttpPost("SavePAFDetails")]
        public bool SavePAFDetails([FromBody]PostalAddressDTO postalAddress)
        {
            if (postalAddress != null)
                return postalAddressBusinessService.SavePAFDetails(postalAddress, "abc.zip");
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
