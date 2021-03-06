﻿namespace RM.Common.ReferenceData.WebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Reflection;
    using CommonLibrary.LoggingMiddleware;
    using Microsoft.AspNetCore.Mvc;
    using RM.Common.ReferenceData.WebAPI.BusinessService.Interface;
    using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
    using RM.CommonLibrary.ExceptionMiddleware;
    using RM.CommonLibrary.HelperMiddleware;

    [Route("api/ReferenceDataManager")]
    public class ReferenceDataController : RMBaseController
    {
        private const string NameValuePairs = "nameValuePairs";
        private const string SimpleList = "simpleList";
        private const string NameValuePair = "nameValuePair";

        #region Member Variables

        private IReferenceDataBusinessService referenceDataBusinessService = default(IReferenceDataBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.ReferenceDataAPIPriority;
        private int entryEventId = LoggerTraceConstants.ReferenceDataControllerMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.ReferenceDataControllerMethodExitEventId;

        #endregion Member Variables

        #region Constructor

        public ReferenceDataController(IReferenceDataBusinessService referenceDataBusinessService, ILoggingHelper loggingHelper)
        {
            this.referenceDataBusinessService = referenceDataBusinessService;
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// retrieval of name-value pairs as a discrete type of reference data addressed via the URI
        /// path /nameValuePairs
        /// </summary>
        /// <param name="appGroupName">appGroupName is recorded as the category name</param>
        /// <param name="appItemName">appItemName is recorded as the item name</param>
        /// <returns></returns>
        [HttpGet("nameValuePair")]
        public IActionResult GetNameValueReferenceData(string appGroupName, string appItemName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetNameValueReferenceData"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataControllerMethodEntryEventId, LoggerTraceConstants.Title);

                if (string.IsNullOrEmpty(appGroupName) || string.IsNullOrEmpty(appItemName))
                {
                    return BadRequest();
                }

                var nameValuePairsObject = referenceDataBusinessService.GetReferenceDataByNameValuePairs(appGroupName, appItemName);

                Tuple<string, NameValuePair> nameValueSingleResource = new Tuple<string, NameValuePair>(NameValuePair, nameValuePairsObject);

                if (nameValuePairsObject == null)
                {
                    throw new BusinessLogicException(ErrorConstants.Err_MisMatchConfigFile, HttpStatusCode.ExpectationFailed);
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataControllerMethodExitEventId, LoggerTraceConstants.Title);

                return Ok(nameValueSingleResource);
            }
        }

        /// <summary>
        /// retrieval of name-value pairs using GUID id as a discrete type of reference data addressed via the URI
        /// </summary>
        /// <param name="id">Guid Id</param>
        /// <returns></returns>
        [HttpGet("nameValuePair/{id}")]
        public IActionResult GetNameValueReferenceData(Guid id)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetNameValueReferenceDataByGuid"))
            {
                string methodName = typeof(ReferenceDataController) + "." + nameof(GetNameValueReferenceData);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var nameValuePairsObject = referenceDataBusinessService.GetReferenceDataByNameValuePairs(id);

                Tuple<string, NameValuePair> nameValueSingleResource = new Tuple<string, NameValuePair>(NameValuePair, nameValuePairsObject);

                if (nameValuePairsObject == null)
                {
                    throw new BusinessLogicException(ErrorConstants.Err_MisMatchConfigFile, HttpStatusCode.ExpectationFailed);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return Ok(nameValueSingleResource);
            }
        }

        /// <summary>
        /// retrieval of name-value pairs as a discrete type of reference data addressed via the URI
        /// path /nameValuePairs
        /// </summary>
        /// <param name="appGroupName">appGroupName is recorded as the category name</param>
        /// <param name="appItemName">appItemName is recorded as the item name</param>
        /// <returns></returns>
        [HttpGet("nameValuePairs")]
        public IActionResult GetNameValueReferenceData(string appGroupName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetNameValueReferenceData"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataControllerMethodEntryEventId, LoggerTraceConstants.Title);

                if (string.IsNullOrEmpty(appGroupName))
                {
                    return BadRequest();
                }

                var nameValuePairsObject = referenceDataBusinessService.GetReferenceDataByNameValuePairs(appGroupName);
                Tuple<string, List<NameValuePair>> nameValueList = new Tuple<string, List<NameValuePair>>(NameValuePairs, nameValuePairsObject);

                if (nameValuePairsObject == null)
                {
                    throw new BusinessLogicException(ErrorConstants.Err_MisMatchConfigFile, HttpStatusCode.ExpectationFailed);
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataControllerMethodExitEventId, LoggerTraceConstants.Title);
                return Ok(nameValueList);
            }
        }

        /// <summary>
        /// retrieval of simple lists as a discrete type of reference data addressed via the URI path /simpleLists
        /// </summary>
        /// <param name="listName">listname maps to ReferenceDataCategory.CategoryName</param>
        /// <returns></returns>
        [HttpGet("simpleLists")]
        public IActionResult GetSimpleListsReferenceData(string listName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetSimpleListsReferenceData"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataControllerMethodEntryEventId, LoggerTraceConstants.Title);

                if (string.IsNullOrEmpty(listName))
                {
                    return BadRequest();
                }

                var simpleListObject = referenceDataBusinessService.GetSimpleListsReferenceData(listName);

                Tuple<string, SimpleListDTO> nameValueSingleResource = new Tuple<string, SimpleListDTO>(SimpleList, simpleListObject);

                if (simpleListObject == null)
                {
                    throw new BusinessLogicException(ErrorConstants.Err_MisMatchConfigFile, HttpStatusCode.ExpectationFailed);
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataControllerMethodExitEventId, LoggerTraceConstants.Title);
                return Ok(nameValueSingleResource);
            }
        }

        /// <summary>
        /// retrieval of simple lists using Guid Id as a discrete type of reference data addressed via the URI path /simpleLists/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("simpleLists/{id}")]
        public IActionResult GetSimpleListsReferenceData(Guid id)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetSimpleListsReferenceData"))
            {
                string methodName = typeof(ReferenceDataController) + "." + nameof(GetSimpleListsReferenceData);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var simpleListObject = referenceDataBusinessService.GetSimpleListsReferenceData(id);

                Tuple<string, SimpleListDTO> nameValueSingleResource = new Tuple<string, SimpleListDTO>(SimpleList, simpleListObject);

                if (simpleListObject == null)
                {
                    throw new BusinessLogicException(ErrorConstants.Err_MisMatchConfigFile, HttpStatusCode.ExpectationFailed);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return Ok(nameValueSingleResource);
            }
        }

        /// <summary>
        /// retrieval of simple lists as a discrete type of reference data addressed via the URI path /simpleLists
        /// </summary>
        /// <param name="listName">listname maps to ReferenceDataCategory.CategoryName</param>
        /// <returns></returns>
        [HttpPost("simpleLists")]
        public IActionResult GetSimpleListsReferenceData([FromBody]List<string> listNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetSimpleListsReferenceData"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataControllerMethodEntryEventId, LoggerTraceConstants.Title);

                if (listNames == null)
                {
                    return BadRequest();
                }

                var simpleDtoList = referenceDataBusinessService.GetSimpleListsReferenceData(listNames);

                if (simpleDtoList.Count == 0)
                {
                    throw new BusinessLogicException(ErrorConstants.Err_MisMatchConfigFile, HttpStatusCode.ExpectationFailed);
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataControllerMethodExitEventId, LoggerTraceConstants.Title);
                return Ok(simpleDtoList);
            }
        }

        #endregion Methods
    }
}