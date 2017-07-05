﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.Operational.SearchManager.WebAPI.BusinessService;
using RM.Operational.SearchManager.WebAPI.Controllers.BaseController;

namespace RM.Operational.SearchManager.WebAPI.Controllers
{
    /// <summary>
    /// This class contains methods used for fetching Basic search and advance search results.
    /// </summary>
    [Route("api/SearchManager")]
    public class SearchController : RMBaseController
    {
        private readonly ISearchBusinessService searchBussinessService = default(ISearchBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public SearchController(ISearchBusinessService searchBussinessService, ILoggingHelper loggingHelper)
        {
            this.searchBussinessService = searchBussinessService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Fetch results from entities using basic search
        /// </summary>
        /// <param name="searchText">The text to be searched from the entities.</param>
        /// <returns>The result set after filtering the values.</returns>
        [HttpGet("basic/{searchText}")]
        public async Task<SearchResultDTO> BasicSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.BasicSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                try
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    var unitGuid = this.CurrentUserUnit;
                    return await searchBussinessService.FetchBasicSearchDetails(searchText, this.CurrentUserUnit);
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
                finally
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                }
            }
        }

        /// <summary>
        /// Fetch results from Advanced search entities.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <returns> Search Result Dto</returns>
        [HttpGet("advance/{searchText}")]
        public async Task<SearchResultDTO> AdvanceSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.AdvanceSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                try
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    var unitGuid = this.CurrentUserUnit;
                    return await searchBussinessService.FetchAdvanceSearchDetails(searchText, this.CurrentUserUnit);
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
                finally
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                }
            }
        }
    }
}