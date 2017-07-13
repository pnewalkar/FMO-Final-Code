using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface;

namespace RM.DataManagement.UnitManager.WebAPI.Controllers
{
    /// <summary>
    /// Unit location controller.
    /// </summary>
    [Route("api/UnitManager")]
    public class UnitManagerController : RMBaseController
    {
        private readonly IUnitLocationBusinessService unitLocationBusinessService = default(IUnitLocationBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public UnitManagerController(IUnitLocationBusinessService unitLocationBusinessService, ILoggingHelper loggingHelper)
        {
            this.unitLocationBusinessService = unitLocationBusinessService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Get all Delivery Units for logged in user
        /// </summary>
        /// <returns>List</returns>
        [Authorize]
        [HttpGet("Unit")]
        public List<UnitLocationDTO> GetUnitLocations()
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetUnitLocations"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                var getUnitLocations = unitLocationBusinessService.GetDeliveryUnitsForUser(UserId);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                return getUnitLocations;
            }
        }

        /// <summary>
        /// Fetches Postcode sector
        /// </summary>
        /// <returns>List</returns>
        [Authorize]
        [HttpGet("postcodesector/udprn: {udprn}")]
        public async Task<PostCodeSectorDTO> GetPostCodeSectorByUDPRN(int uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostCodeSectorByUDPRN"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    var getPostCodeSectorByUDPRN = await unitLocationBusinessService.GetPostCodeSectorByUDPRN(uDPRN);
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
        /// <param name="userUnit"></param>
        /// <returns></returns>
        [HttpGet("postcodes/basic/{searchText}")]
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.FetchPostCodeUnitForBasicSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    var fetchPostCodeUnitForBasicSearch = await unitLocationBusinessService.FetchPostCodeUnitForBasicSearch(searchText, CurrentUserUnit);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return fetchPostCodeUnitForBasicSearch;
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
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostCodeUnitCount"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    var getPostCodeUnitCount = await unitLocationBusinessService.GetPostCodeUnitCount(searchText, CurrentUserUnit);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return getPostCodeUnitCount;
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
        // [HttpGet("FetchPostCodeUnitForAdvanceSearch")]
        [HttpGet("postcodes/advance/{searchText}")]
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.FetchPostCodeUnitForAdvanceSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    var fetchPostCodeUnitForAdvanceSearch = await unitLocationBusinessService.FetchPostCodeUnitForAdvanceSearch(searchText, CurrentUserUnit);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return fetchPostCodeUnitForAdvanceSearch;
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
        public async Task<Guid> GetPostCodeID(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostCodeID"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    var getPostCodeID = await unitLocationBusinessService.GetPostCodeID(searchText);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
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
        /// Fetches Delivery Scenario
        /// </summary>
        /// <param name="operationStateID">operation State ID</param>
        /// <param name="deliveryUnitID">delivery Unit ID</param>
        /// <param name="fields">The fields to be returned</param>
        /// <returns></returns>
        [HttpGet("scenario/{operationStateID}/{deliveryUnitID}/{fields}")]
        public IActionResult FetchDeliveryScenario(Guid operationStateID, Guid deliveryUnitID, string fields)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.FetchDeliveryScenario"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                List<object> deliveryScenerioList = null;
                List<ScenarioDTO> deliveryScenerio = unitLocationBusinessService.FetchDeliveryScenario(operationStateID, deliveryUnitID);
                CreateSummaryObject<ScenarioDTO> createSummary = new CreateSummaryObject<ScenarioDTO>();

                if (!string.IsNullOrEmpty(fields))
                {
                    deliveryScenerioList = createSummary.SummarisePropertiesForList(deliveryScenerio, fields);
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
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
            List<PostCodeDTO> postCodes = await unitLocationBusinessService.GetPostCodeDetails(postcodeGuids);        
            return Ok(postCodes);
        }
    }
}