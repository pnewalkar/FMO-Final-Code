using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface;
using RM.DataManagement.UnitManager.WebAPI.DTO;

namespace RM.DataManagement.UnitManager.WebAPI.Controllers
{
    /// <summary>
    /// Unit location controller. REST API Service for exposing Unit Manager resources.
    /// </summary>
    [Route("api/UnitManager")]
    public class UnitManagerController : RMBaseController
    {
        private readonly IUnitLocationBusinessService unitLocationBusinessService = default(IUnitLocationBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public UnitManagerController(IUnitLocationBusinessService unitLocationBusinessService, ILoggingHelper loggingHelper)
        {
            // Store injected dependencies
            this.unitLocationBusinessService = unitLocationBusinessService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Get all Delivery Units for logged in user
        /// </summary>
        /// <returns>List</returns>
        [Authorize]
        [HttpGet("Unit")]
        public async Task<IEnumerable<UnitLocationDTO>> GetUnitLocations()
        {
            string methodName = typeof(UnitManagerController) + "." + nameof(GetUnitLocations);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetUnitLocations"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);

                var unitLocations = await unitLocationBusinessService.GetUnitsByUser(UserId, CurrentUserUnitType);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodExitEventId);
                return unitLocations;
            }
        }

        /// <summary>
        /// Gets Postcode sector by UDPRN
        /// </summary>
        /// <returns>List</returns>
        [Authorize]
        [HttpGet("postcodesector/udprn: {udprn}")]
        public async Task<PostcodeSectorDTO> GetPostcodeSectorByUdprn(int udprn)
        {
            string methodName = typeof(UnitManagerController) + "." + nameof(GetPostcodeSectorByUdprn);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostcodeSectorByUdprn"))
            {
                if (udprn.Equals(0))
                {
                    throw new ArgumentNullException(ErrorConstants.Error_NonZero, nameof(udprn));
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    var getPostCodeSectorByUDPRN = await unitLocationBusinessService.GetPostcodeSectorByUdprn(udprn);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return getPostCodeSectorByUDPRN;
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
        /// Fetch the post code unit for basic search.
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpGet("postcodes/basic/{searchText}")]
        public async Task<IEnumerable<PostcodeDTO>> GetPostcodeUnitForBasicSearch(string searchText)
        {
            string methodName = typeof(UnitManagerController) + "." + nameof(GetPostcodeUnitForBasicSearch);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostcodeUnitForBasicSearch"))
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    throw new ArgumentNullException(nameof(searchText));
                }

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);

                try
                {
                    var postcodeUnits = await unitLocationBusinessService.GetPostcodeUnitForBasicSearch(searchText, CurrentUserUnit);

                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);
                    return postcodeUnits;
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
        /// Get the post code unit count
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="userUnit"></param>
        /// <returns></returns>
        [HttpGet("postcodes/count/{searchText}")]
        public async Task<int> GetPostCodeUnitCount(string searchText)
        {
            string methodName = typeof(UnitManagerController) + "." + nameof(GetPostCodeUnitCount);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostCodeUnitCount"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);

                if (string.IsNullOrEmpty(searchText))
                {
                    throw new ArgumentNullException(nameof(searchText));
                }

                try
                {
                    var postCodeUnitCount = await unitLocationBusinessService.GetPostcodeUnitCount(searchText, CurrentUserUnit);

                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodExitEventId);
                    return postCodeUnitCount;
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
        /// Get the post code unit for advance search.
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpGet("postcodes/advance/{searchText}")]
        public async Task<IEnumerable<PostcodeDTO>> GetPostCodeUnitForAdvanceSearch(string searchText)
        {
            string methodName = typeof(UnitManagerController) + "." + nameof(GetPostCodeUnitForAdvanceSearch);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostCodeUnitForAdvanceSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);

                if (string.IsNullOrEmpty(searchText))
                {
                    throw new ArgumentNullException(nameof(searchText));
                }

                try
                {
                    var postCodeUnits = await unitLocationBusinessService.GetPostcodeUnitForAdvanceSearch(searchText, CurrentUserUnit);
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodExitEventId);
                    return postCodeUnits;
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
        /// Fetch the post code unit for advance search.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="userUnit"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("postcode/guid/{searchText}")]
        public async Task<Guid> GetPostcodeID(string searchText)
        {
            string methodName = typeof(UnitManagerController) + "." + nameof(GetPostcodeID);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostcodeID"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);

                if (string.IsNullOrEmpty(searchText))
                {
                    throw new ArgumentNullException(nameof(searchText));
                }

                try
                {
                    var getPostCodeID = await unitLocationBusinessService.GetPostcodeID(searchText);
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodExitEventId);
                    return getPostCodeID;
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
        /// Get the list of route scenarios by the operationstateID and locationID.
        /// </summary>
        /// <param name="operationStateID">operation State ID</param>
        /// <param name="locationID">location ID</param>
        /// <param name="fields">The fields to be returned</param>
        /// <returns></returns>
        [HttpGet("scenario/{operationStateID}/{locationID}/{fields}")]
        public async Task<IActionResult> GetRouteScenarios(Guid operationStateID, Guid locationID, string fields)
        {
            if (operationStateID.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(operationStateID));
            }
            if (locationID.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(locationID));
            }
            if (fields.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(fields));
            }

            string methodName = typeof(UnitManagerController) + "." + nameof(GetPostCodes);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetRouteScenarios"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);

                List<object> deliveryScenerioList = null;
                IEnumerable<ScenarioDTO> Scenerio = await unitLocationBusinessService.GetRouteScenarios(operationStateID, locationID);
                CreateSummaryObject<ScenarioDTO> createSummary = new CreateSummaryObject<ScenarioDTO>();

                if (!string.IsNullOrEmpty(fields) && Scenerio != null)
                {
                    deliveryScenerioList = createSummary.SummarisePropertiesForList(Scenerio.ToList(), fields);
                }

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);
                return Ok(deliveryScenerioList);
            }
        }

        /// <summary>
        /// Gets postcode details by postcode guids
        /// </summary>
        /// <param name="postcodeGuids"></param>
        /// <returns></returns>
        [HttpPost("postcode/search/{unitGuid}")]
        public async Task<IActionResult> GetPostCodes([FromBody] List<Guid> postcodeGuids)
        {
            if (postcodeGuids == null || postcodeGuids.Count.Equals(0))
            {
                throw new ArgumentNullException(nameof(postcodeGuids));
            }

            string methodName = typeof(UnitManagerController) + "." + nameof(GetPostCodes);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostCodes"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);

                IEnumerable<PostcodeDTO> postCodes = await unitLocationBusinessService.GetPostcodes(postcodeGuids);

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);
                return Ok(postCodes);
            }
        }

        /// <summary>
        /// Gets approx location based on the postal code.
        /// </summary>
        /// <param name="postcode">Postal code</param>
        /// <returns>The approx location for the given postal code.</returns>
        [HttpPost("postcode/approxlocation/{postcode}")]
        public async Task<IActionResult> GetApproxLocation(string postcode)
        {
            if (postcode == null)
            {
                throw new ArgumentNullException(nameof(postcode));
            }

            string methodName = typeof(UnitManagerController) + "." + nameof(GetApproxLocation);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetApproxLocation"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);

                var location = await unitLocationBusinessService.GetApproxLocation(postcode, CurrentUserUnit);

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);
                return Ok(location);
            }
        }
    }
}