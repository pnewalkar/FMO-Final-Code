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
        IAccessLinkBusinessService accessLinkBussinessService = default(IAccessLinkBusinessService);


        public AccessLinkController(IAccessLinkBusinessService businessService)
        {
            this.accessLinkBussinessService = businessService;
        }

        /// <summary>
        /// This method is used to fetch access link data.
        /// </summary>
        /// <param name="AccessLinkDTO">List of AccessLinkDTO</param>
        /// <returns>List of AccessLinkDTO</returns>
        [Route("fetchAccessLink")]
        [HttpGet]
        public List<AccessLinkDTO> FetchAccessLink(List<AccessLinkDTO> AccessLinkDTO)
        {
            return accessLinkBussinessService.SearchAccessLink();
        }

        /// <summary>
        /// This method is used to fetch Access Link.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <returns>string of Access link data</returns>
        [Route("GetAccessLinks")]
        [HttpGet]
        public string GetAccessLinks(string boundaryBox)
        {

            return accessLinkBussinessService.GetAccessLinks(boundaryBox);

        }

    }
}
