using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.FileProcessing;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.ThirdPartyAddressLocation.WebAPI.BusinessService;
using System;
using System.Diagnostics;
using RM.CommonLibrary.LoggingMiddleware;
using System.Reflection;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.DataManagement.ThirdPartyAddressLocation.WebAPI.Controllers
{
    [Route("api/thirdpartyaddresslocationmanager")]
    public class ThirdPartyAddressLocationController : RMBaseController
    {
        private IThirdPartyAddressLocationBusinessService thirdPartyAddressLocationBusinessService = default(IThirdPartyAddressLocationBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public ThirdPartyAddressLocationController(IThirdPartyAddressLocationBusinessService thirdPartyAddressLocationBusinessService, ILoggingHelper logginghelper)
        {
            this.thirdPartyAddressLocationBusinessService = thirdPartyAddressLocationBusinessService;
            this.loggingHelper = logginghelper;
        }

        /// <summary>
        /// This method is used to fetch data for Access Links.
        /// </summary>
        /// <param name="uDPRN">UDPRN</param>
        /// <returns>
        /// Address Location DTO
        /// </returns>
        [Route("addresslocation/geojson/udprn:{udprn}")]
        [HttpGet]
        public async Task<IActionResult> GetAddressLocationByUDPRNJson(int udprn)
        {
            try
            {
                object addressLocationJson = await this.thirdPartyAddressLocationBusinessService.GetAddressLocationByUDPRNJson(udprn);
                return Ok(addressLocationJson);
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// Get AddressLocation by UDPRN
        /// </summary>
        /// <param name="udprn"> UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>
        [Route("addresslocation/udprn:{udprn}")]
        [HttpGet]
        public async Task<IActionResult> GetAddressLocationByUDPRN(int udprn)
        {
            try
            {
                AddressLocationDTO addressLocationDTO = await this.thirdPartyAddressLocationBusinessService.GetAddressLocationByUDPRN(udprn);
                return Ok(addressLocationDTO);
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// Method to save the list of USR data into the database.
        /// </summary>
        /// <param name="addressLocationUsrpostdtos"> List of Address Locations</param>
        /// <returns> Task </returns>
        [HttpPost("addresslocation/save")]
        public async Task<IActionResult> SaveUSRDetails([FromBody] List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.SaveUSRDetails"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    await this.thirdPartyAddressLocationBusinessService.SaveUSRDetails(addressLocationUsrpostdtos);
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationControllerMethodExitEventId, LoggerTraceConstants.Title);

                    return Ok("Saved successfully");
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }
    }
}