using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// This controller contains methods for fetching Access Actions.
    /// </summary>
    [Route("api/[controller]")]
    public class AccessActionController : Controller
    {
        private IAccessActionBusinessService accessActionBussinessService = default(IAccessActionBusinessService);

        public AccessActionController(IAccessActionBusinessService accessActionBussinessService)
        {
            this.accessActionBussinessService = accessActionBussinessService;
        }

        /////// <summary>
        /////// This method is used to Fetch Access Action items.
        /////// </summary>
        /////// <param name="AccessActionDTO">AccessActionDTO as input</param>
        /////// <returns>List of AccessActionDto</returns>
        ////[Route("fetchAccessLink")]
        ////[HttpGet]
        ////public List<AccessActionDTO> FetchAccessActions(List<AccessActionDTO> AccessActionDTO)
        ////{
        ////    return accessActionBussinessService.FetchAccessActions();
        ////}
    }
}