using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO.FileProcessing;
using RM.DataManagement.ThirdPartyAddressLocation.WebAPI.BusinessService;

namespace RM.DataManagement.ThirdPartyAddressLocation.WebAPI.Controllers
{
    [Route("api/thirdpartyaddresslocationmanager")]
    public class ThirdPartyAddressLocationController : RMBaseController
    {
        private IThirdPartyAddressLocationBusinessService thirdPartyAddressLocationBusinessService = default(IThirdPartyAddressLocationBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public ThirdPartyAddressLocationController(IThirdPartyAddressLocationBusinessService thirdPartyAddressLocationBusinessService, ILoggingHelper logginghelper)
        {
            // Store injected dependencies
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
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetAddressLocationByUDPRNJson"))
            {
                if (udprn == 0)
                {
                    throw new ArgumentException(nameof(udprn));
                }

                try
                {
                    string methodName = typeof(ThirdPartyAddressLocationController) + "." + nameof(GetAddressLocationByUDPRNJson);
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationControllerMethodEntryEventId);
                    object addressLocationJson = await this.thirdPartyAddressLocationBusinessService.GetAddressLocationByUDPRNJson(udprn);
                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationControllerMethodExitEventId);
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
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetAddressLocationByUDPRN"))
            {
                if (udprn == 0)
                {
                    throw new ArgumentException(nameof(udprn));
                }

                try
                {
                    string methodName = typeof(ThirdPartyAddressLocationController) + "." + nameof(GetAddressLocationByUDPRN);
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationControllerMethodEntryEventId);
                    AddressLocationDTO addressLocationDTO = await this.thirdPartyAddressLocationBusinessService.GetAddressLocationByUDPRN(udprn);
                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationControllerMethodExitEventId);
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
        }

        // TODO : Add method when ready
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
                    if (addressLocationUsrpostdtos == null || addressLocationUsrpostdtos.Count <= 0)
                    {
                        throw new ArgumentException(nameof(addressLocationUsrpostdtos));
                    }

                    string methodName = typeof(ThirdPartyAddressLocationController) + "." + nameof(SaveUSRDetails);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    await this.thirdPartyAddressLocationBusinessService.SaveUSRDetails(addressLocationUsrpostdtos);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationControllerMethodExitEventId, LoggerTraceConstants.Title);

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