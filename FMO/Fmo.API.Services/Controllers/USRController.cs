using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Interface;
using Fmo.DTO.FileProcessing;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// USR controller to handle USR API request from windows service
    /// </summary>
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

        /// <summary>
        /// Api to save USR details in DB.
        /// </summary>
        /// <param name="lstAddressLocationUSRPOSTDTO">List of posatl address DTO</param>
        /// <returns></returns>           
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

    }
}
