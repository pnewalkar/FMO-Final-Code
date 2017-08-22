using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware.GeoJsonData;
using RM.DataManagement.NetworkManager.WebAPI.BusinessService;
using RM.DataManagement.NetworkManager.WebAPI.DTO;

namespace RM.DataManagement.NetworkManager.WebAPI.Controllers
{
    [Route("/api/NetworkManager")]
    public class NetworkManagerController : RMBaseController
    {
        #region Member Variables

        private INetworkManagerBusinessService networkManagerBusinessService = default(INetworkManagerBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        private int priority = LoggerTraceConstants.NetworkManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.NetworkManagerControllerMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.NetworkManagerControllerMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        public NetworkManagerController(INetworkManagerBusinessService networkManagerBusinessService, ILoggingHelper loggingHelper)
        {
            this.networkManagerBusinessService = networkManagerBusinessService;
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="streetName">Street name.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        [HttpPost("nearestnamedroad/{streetName}")]
        public IActionResult GetNearestNamedRoad([FromBody]string operationalObjectPointJson, string streetName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Controller.GetNearestNamedRoad"))
            {
                string methodName = typeof(NetworkManagerController) + "." + nameof(GetNearestNamedRoad);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                Tuple<NetworkLinkDTO, SqlGeometry> result;

                result = networkManagerBusinessService.GetNearestNamedRoad(JsonConvert.DeserializeObject<DbGeometry>(operationalObjectPointJson, new DbGeometryConverter()), streetName);

                var convertedResult = new Tuple<NetworkLinkDTO, DBGeometryDTO>(result.Item1, new DBGeometryDTO { Geometry = result.Item2.IsNull ? null : result.Item2.ToDbGeometry() });

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
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
            using (loggingHelper.RMTraceManager.StartTrace("Controller.GetNearestSegment"))
            {
                string methodName = typeof(NetworkManagerController) + "." + nameof(GetNearestSegment);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<Tuple<NetworkLinkDTO, SqlGeometry>> result;

                result = networkManagerBusinessService.GetNearestSegment(JsonConvert.DeserializeObject<DbGeometry>(operationalObjectPointJson, new DbGeometryConverter()));

                List<Tuple<NetworkLinkDTO, DbGeometry>> convertedResult = new List<Tuple<DTO.NetworkLinkDTO, DbGeometry>>();
                foreach (var item in result)
                {
                    convertedResult.Add(new Tuple<NetworkLinkDTO, DbGeometry>(item.Item1, item.Item2.ToDbGeometry()));
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
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
            using (loggingHelper.RMTraceManager.StartTrace("Controller.GetNetworkLink"))
            {
                string methodName = typeof(NetworkManagerController) + "." + nameof(GetNetworkLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                NetworkLinkDTO result;

                result = networkManagerBusinessService.GetNetworkLink(networkLinkID);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return Ok(result);
            }
        }

        /// <summary> This method is used to get the road links crossing the access link </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLinkCoordinates">access link coordinate array</param> <returns>List<NetworkLinkDTO></returns>
        [HttpPost("networklink/{boundingBoxCoordinates}")]
        public IActionResult GetNetworkLink(string boundingBoxCoordinates, [FromBody]string accessLinkCoordinatesJson)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Controller.GetNetworkLink"))
            {
                string methodName = typeof(NetworkManagerController) + "." + nameof(GetNetworkLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<NetworkLinkDTO> result;

                result = networkManagerBusinessService.GetCrossingNetworkLinks(boundingBoxCoordinates, JsonConvert.DeserializeObject<DbGeometry>(accessLinkCoordinatesJson, new DbGeometryConverter()));

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
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
            using (loggingHelper.RMTraceManager.StartTrace("Controller.GetOSRoadLink"))
            {
                string methodName = typeof(NetworkManagerController) + "." + nameof(GetOSRoadLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    string result;

                    result = await networkManagerBusinessService.GetOSRoadLink(toid);

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
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
            using (loggingHelper.RMTraceManager.StartTrace("Controller.GetRouteData"))
            {
                string methodName = typeof(NetworkManagerController) + "." + nameof(GetRouteData);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string route = networkManagerBusinessService.GetRoadRoutes(bbox, this.CurrentUserUnit, this.CurrentUserUnitType);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
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
        public async Task<IActionResult> GetStreetNamesForBasicSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Controller.GetStreetNamesForBasicSearch"))
            {
                string methodName = typeof(NetworkManagerController) + "." + nameof(GetStreetNamesForBasicSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    List<StreetNameDTO> streetNames = await networkManagerBusinessService.GetStreetNamesForBasicSearch(searchText, this.CurrentUserUnit, this.CurrentUserUnitType).ConfigureAwait(false);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
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
            using (loggingHelper.RMTraceManager.StartTrace("Controller.GetStreetNameCount"))
            {
                string methodName = typeof(NetworkManagerController) + "." + nameof(GetStreetNameCount);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    int streetCount = await networkManagerBusinessService.GetStreetNameCount(searchText, this.CurrentUserUnit, this.CurrentUserUnitType).ConfigureAwait(false);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
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
        public async Task<IActionResult> GetStreetNamesForAdvanceSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Controller.GetStreetNamesForAdvanceSearch"))
            {
                string methodName = typeof(NetworkManagerController) + "." + nameof(GetStreetNamesForAdvanceSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    List<StreetNameDTO> streetNames = await networkManagerBusinessService.GetStreetNamesForAdvanceSearch(searchText, this.CurrentUserUnit, this.CurrentUserUnitType);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
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

        #endregion Public Methods
    }
}