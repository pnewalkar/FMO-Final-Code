using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.BusinessService;

namespace RM.DataManagement.NetworkManager.WebAPI.Controllers
{
    [Route("/api/NetworkManager")]
    public class NetworkManagerController : RMBaseController
    {
        private INetworkManagerBusinessService networkManagerBusinessService = default(INetworkManagerBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public NetworkManagerController(INetworkManagerBusinessService networkManagerBusinessService, ILoggingHelper loggingHelper)
        {
            this.networkManagerBusinessService = networkManagerBusinessService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="streetName">Street name.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        [HttpPost("nearestnamedroad/{streetName}")]
        public IActionResult GetNearestNamedRoad([FromBody]string operationalObjectPointJson, string streetName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetNearestNamedRoad"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                Tuple<NetworkLinkDTO, SqlGeometry> result;

                result = networkManagerBusinessService.GetNearestNamedRoad(JsonConvert.DeserializeObject<DbGeometry>(operationalObjectPointJson, new DbGeometryConverter()), streetName);

                var convertedResult = new Tuple<NetworkLinkDTO, DbGeometry>(result.Item1, result.Item2.IsNull ? null : result.Item2.ToDbGeometry());

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                return Ok(convertedResult);
            }
        }

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        [HttpPost("nearestsegment")]
        public IActionResult GetNearestSegment([FromBody]string operationalObjectPointJson)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetNearestSegment"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                Tuple<NetworkLinkDTO, SqlGeometry> result;

                result = networkManagerBusinessService.GetNearestSegment(JsonConvert.DeserializeObject<DbGeometry>(operationalObjectPointJson, new DbGeometryConverter()));

                var convertedResult = new Tuple<NetworkLinkDTO, DbGeometry>(result.Item1, result.Item2.IsNull ? null : result.Item2.ToDbGeometry());
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                return Ok(convertedResult);
            }
        }

        /// <summary>
        /// Get the street DTO for operational object.
        /// </summary>
        /// <param name="networkLinkID">networkLink unique identifier Guid.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        [HttpGet("networklink/{networkLinkID}")]
        public IActionResult GetNetworkLink(Guid networkLinkID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetNetworkLink"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                NetworkLinkDTO result;

                result = networkManagerBusinessService.GetNetworkLink(networkLinkID);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                return Ok(result);
            }
        }

        /// <summary> This method is used to get the road links crossing the access link </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLinkCoordinates">access link coordinate array</param> <returns>List<NetworkLinkDTO></returns>
        [HttpPost("networklink/{boundingBoxCoordinates}")]
        public IActionResult GetNetworkLink(string boundingBoxCoordinates, [FromBody]string accessLinkCoordinatesJson)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetNetworkLink"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                List<NetworkLinkDTO> result;

                result = networkManagerBusinessService.GetCrossingNetworkLinks(boundingBoxCoordinates, JsonConvert.DeserializeObject<DbGeometry>(accessLinkCoordinatesJson, new DbGeometryConverter()));

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                return Ok(result);
            }
        }

        /// <summary>
        /// Gets the os road link.
        /// </summary>
        /// <param name="toid">The toid.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("osroadlink/{toid}")]
        public async Task<IActionResult> GetOSRoadLink(string toid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetOSRoadLink"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    string result;

                    result = await networkManagerBusinessService.GetOSRoadLink(toid);

                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(result);
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
        /// This method is used to get Route Link data.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as strintg</param>
        /// <returns></returns>
        [Route("routelinks")]
        [HttpGet]
        public IActionResult GetRouteData(string bbox)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetRouteData"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                string route = networkManagerBusinessService.GetRoadRoutes(bbox, CurrentUserUnit);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                return Ok(route);
            }
        }

        /// <summary>
        /// Fetch the street name for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>Task</returns>
        [Route("streetnames/basic/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> FetchStreetNamesForBasicSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.FetchStreetNamesForBasicSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    List<StreetNameDTO> streetNames = await networkManagerBusinessService.FetchStreetNamesForBasicSearch(searchText, CurrentUserUnit).ConfigureAwait(false);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(streetNames);
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
        /// Get the count of street name
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of street name</returns>
        [Route("streetnames/count/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> GetStreetNameCount(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetStreetNameCount"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    int streetCount = await networkManagerBusinessService.GetStreetNameCount(searchText, CurrentUserUnit).ConfigureAwait(false);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(streetCount);
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
        /// Fetch street names for advance search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>StreetNames</returns>
        [Route("streetnames/advance/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> FetchStreetNamesForAdvanceSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.FetchStreetNamesForAdvanceSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    List<StreetNameDTO> streetNames = await networkManagerBusinessService.FetchStreetNamesForAdvanceSearch(searchText, CurrentUserUnit);
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NetworkManagerAPIPriority, LoggerTraceConstants.NetworkManagerControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(streetNames);
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
}