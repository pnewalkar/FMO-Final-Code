using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Interface;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    [Route("api/deliveryPoints")]
    public class DeliveryPointsController : Controller
    {
        private IDeliveryPointBusinessService businessService = default(IDeliveryPointBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

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
        public JsonResult GetDeliveryPoints(string bbox)
        {
            return Json(businessService.GetDeliveryPoints(bbox));           
        }

        [Route("GetDeliveryPointByUDPRN")]
        [HttpGet]
        public object GetDeliveryPointByUDPRN(int udprn)
        {
            return businessService.GetDeliveryPointByUDPRN(udprn);
        }

    }   
}
