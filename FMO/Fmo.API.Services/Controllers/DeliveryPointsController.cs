using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Fmo.Common.Interface;
using Fmo.Common.Enums;
using Fmo.Common.ExceptionManagement;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using IO=System.IO;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    
    [Route("api/deliveryPoints")]
    public class DeliveryPointsController : Controller
    {
        IDeliveryPointBusinessService businessService = default(IDeliveryPointBusinessService);
        ILoggingHelper loggingHelper = default(ILoggingHelper);

        public DeliveryPointsController(IDeliveryPointBusinessService businessService, ILoggingHelper loggingHelper)
        {
            this.businessService = businessService;
            this.loggingHelper = loggingHelper;
        }

        public JsonResult Get()
        {
           return Json("");   
        }

        [Route("fetchDeliveryPoint")]
        [HttpGet]
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPoints()
        {
            throw new NotImplementedException();
        }

        [Route("GetDeliveryPoints")]
        [HttpGet]
        public object GetDeliveryPoints(string bbox)
        {
            //try
            //{

            return Json("");
            //return businessService.GetDeliveryPoints();
            //}

            //catch (Exception ex)
            //{

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
