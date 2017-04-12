using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Net;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    [Route("api/[controller]")]
    public class AccessLinkController : Controller
    {
       IAccessLinkBussinessService accessLinkBussinessService = default(IAccessLinkBussinessService);


        public AccessLinkController(IAccessLinkBussinessService businessService)
        {
            this.accessLinkBussinessService = businessService;
        }

        [Route("fetchAccessLink")]
        [HttpGet]
        public List<AccessLinkDTO> FetchAccessLink(List<AccessLinkDTO> AccessLinkDTO)
        {
            return accessLinkBussinessService.SearchAccessLink();
        }

        [Route("GetAccessLinks")]
        [HttpGet]
        public string GetAccessLinks(string bbox)
        {
           
                return accessLinkBussinessService.GetAccessLinks(bbox);

        }
        //// GET: api/values
        //[HttpGet]
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
