using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.DataManagement.UnitManager.WebAPI.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface;

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
            string methodName = typeof(UnitManagerController) + "." + nameof(GetUnitLocations);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetUnitLocations"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);

                var getUnitLocations = unitLocationBusinessService.GetDeliveryUnitsForUser(UserId);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodExitEventId);
                return getUnitLocations;
            }
        }

        /// <summary>
        /// Gets Postcode sector by UDPRN
        /// </summary>
        /// <returns>List</returns>
        [Authorize]
        [HttpGet("postcodesector/udprn: {udprn}")]
        public async Task<PostCodeSectorDTO> GetPostCodeSectorByUdprn(int udprn)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostCodeSectorByUdprn"))
            {
                if (udprn.Equals(0))
                {
                    throw new ArgumentNullException(ErrorConstants.Error_NonZero, nameof(udprn));
                }

                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    var getPostCodeSectorByUDPRN = await unitLocationBusinessService.GetPostCodeSectorByUdprn(udprn);
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
        public async Task<List<PostCodeDTO>> GetPostCodeUnitForBasicSearch(string searchText)
        {
            string methodName = typeof(UnitManagerController) + "." + nameof(GetPostCodeUnitForBasicSearch);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostCodeUnitForBasicSearch"))
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    throw new ArgumentNullException(nameof(searchText));
                }

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);

                try
                {
                    var postcodeUnits = await unitLocationBusinessService.GetPostCodeUnitForBasicSearch(searchText, CurrentUserUnit);

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
                    var postCodeUnitCount = await unitLocationBusinessService.GetPostCodeUnitCount(searchText, CurrentUserUnit);

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
        public async Task<List<PostCodeDTO>> GetPostCodeUnitForAdvanceSearch(string searchText)
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
                    var postCodeUnits = await unitLocationBusinessService.GetPostCodeUnitForAdvanceSearch(searchText, CurrentUserUnit);
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
        public async Task<Guid> GetPostCodeID(string searchText)
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
                    var getPostCodeID = await unitLocationBusinessService.GetPostCodeID(searchText);
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
        public IActionResult GetRouteScenarios(Guid operationStateID, Guid locationID, string fields)
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
                List<ScenarioDTO> Scenerio = unitLocationBusinessService.GetRouteScenarios(operationStateID, locationID);
                CreateSummaryObject<ScenarioDTO> createSummary = new CreateSummaryObject<ScenarioDTO>();

                if (!string.IsNullOrEmpty(fields))
                {
                    deliveryScenerioList = createSummary.SummarisePropertiesForList(Scenerio, fields);
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
            if (postcodeGuids == null || postcodeGuids.Count.Equals(0) )
            {
                throw new ArgumentNullException(nameof(postcodeGuids));
            }

            string methodName = typeof(UnitManagerController) + "." + nameof(GetPostCodes);
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetPostCodes"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);

                List<PostCodeDTO> postCodes = await unitLocationBusinessService.GetPostCodeDetails(postcodeGuids);

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerControllerMethodEntryEventId);
                return Ok(postCodes);
            }
        }
    }
}