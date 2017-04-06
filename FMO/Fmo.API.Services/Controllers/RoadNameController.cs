﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Net;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    [Route("api/[controller]")]
    public class RoadNameController : Controller
    {

        IRoadNameBussinessService roadNameBussinessService = default(IRoadNameBussinessService);

        public async Task<List<RoadNameDTO>> FetchRoadName(List<RoadNameDTO> RoadNameDTO)
        {
            return await roadNameBussinessService.FetchRoadName();
        }

        [Route("GetRouteLinks")]
        [HttpGet]
        public HttpResponseMessage GetRoouteData(string bbox)
        {
            //try
            //{
                string[] bboxArr = bbox.Split(',');
                MemoryStream memoryStream = roadNameBussinessService.GetRoadRoutes(bbox);

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(memoryStream)
                };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                result.Content.Headers.ContentLength = memoryStream.Length;
                return result;
            //}
            //catch (Exception ex)
            //{
            //    Logger.Logger.LogError(ex, ex.Message);
            //    var result = new HttpResponseMessage(HttpStatusCode.BadRequest)
            //    {
            //        ReasonPhrase = ex.Message
            //    };
            //    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    return result;
            //}
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
