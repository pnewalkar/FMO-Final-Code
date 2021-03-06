﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.AccessLink.WebAPI.BusinessService.Interface;

namespace RM.DataManagement.AccessLink.WebAPI.Controllers
{
    [Route("api/AccessLinkManager")]
    public class AccessLinkController : RMBaseController
    {
        #region Member Variables

        private IAccessLinkBusinessService accessLinkBusinessService;

        private ILoggingHelper loggingHelper;
        private int priority = LoggerTraceConstants.AccessLinkAPIPriority;
        private int entryEventId = LoggerTraceConstants.AccessLinkControllerMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.AccessLinkControllerMethodExitEventId;

        #endregion Member Variables

        #region Constructor

        public AccessLinkController(IAccessLinkBusinessService accessLinkBusinessService, ILoggingHelper loggingHelper)
        {
            this.accessLinkBusinessService = accessLinkBusinessService;
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// This method is used to create automatic Access Link .
        /// </summary>
        /// <param name="operationalObjectId">Operational Object unique identifier.</param>
        /// <param name="operationalObjectTypeId">Operational Object type unique identifier.</param>
        /// <returns>If <true>then access link creation succeeded,else failure.</true></returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewAccessLinks)]
        [HttpGet]
        [Route("AccessLink/{operationalObjectId}/{operationalObjectTypeId}")]
        public IActionResult CreateAccessLink(Guid operationalObjectId, Guid operationalObjectTypeId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.CreateAccessLink"))
            {
                bool success = false;
                try
                {
                    string methodName = typeof(AccessLinkController) + "." + nameof(CreateAccessLink);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    success = accessLinkBusinessService.CreateAccessLink(operationalObjectId, operationalObjectTypeId);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                }
                catch (AggregateException ex)
                {
                    foreach (var exception in ex.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ex.Flatten().InnerException;

                    throw realExceptions;
                }

                return Ok(success);
            }
        }

        /// <summary>
        /// This method is used to create manual Access Link .
        /// </summary>
        ///<param name="accessLinkDto">access link object to be stored</param>
        /// <returns>If <true> then access link creation succeeded,else failure.</true></returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewAccessLinks)]
        [HttpPost("AccessLink/Manual")]
        public IActionResult CreateManualAccessLink([FromBody] AccessLinkManualCreateModelDTO accessLinkDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.CreateManualAccessLink"))
            {
                string methodName = typeof(AccessLinkController) + "." + nameof(CreateManualAccessLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool isSaved = accessLinkBusinessService.CreateAccessLink(accessLinkDto);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return Ok(isSaved);
            }
        }

        /// <summary>
        /// This method is used to fetch Access Link.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <returns>GeoJson string of Access link data</returns>
        [Authorize]
        [HttpGet("AccessLinks")]
        public IActionResult GetAccessLinks(string bbox)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetAccessLinks"))
            {
                string methodName = typeof(AccessLinkController) + "." + nameof(GetAccessLinks);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string accessLink = accessLinkBusinessService.GetAccessLinks(bbox, this.CurrentUserUnit);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return Ok(accessLink);
            }
        }

        /// <summary>
        /// This method is used to check the manual access link adj length
        /// </summary>
        ///<param name="accessLinkManualCreateModelDTO">access link object of which adj length needs to be calculated</param>
        /// <returns>returns calculated path length</true></returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewAccessLinks)]
        [HttpPost("AccessLink/PathLength")]
        public IActionResult GetAdjPathLength([FromBody] AccessLinkManualCreateModelDTO accessLinkManualCreateModelDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetAdjPathLength"))
            {
                string methodName = typeof(AccessLinkController) + "." + nameof(GetAdjPathLength);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                decimal pathlength = accessLinkBusinessService.GetAdjPathLength(accessLinkManualCreateModelDTO);
                pathlength = Math.Round(pathlength);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return Ok(pathlength);
            }
        }

        /// <summary>
        /// This method is used to check whether the access link is valid or not.
        /// </summary>
        ///<param name="accessLinkManualCreateModelDTO">access link object to be checked for valid access link</param>
        /// <returns>returns whether an access link is valid</true></returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewAccessLinks)]
        [HttpPost("AccessLink/Valid")]
        public IActionResult CheckAccessLinkIsValid([FromBody] AccessLinkManualCreateModelDTO accessLinkManualCreateModelDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.CheckAccessLinkIsValid"))
            {
                string methodName = typeof(AccessLinkController) + "." + nameof(CheckAccessLinkIsValid);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                bool isValid = accessLinkBusinessService.CheckManualAccessLinkIsValid(accessLinkManualCreateModelDTO.BoundingBoxCoordinates, accessLinkManualCreateModelDTO.AccessLinkLine);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return Ok(isValid);
            }
        }

        /// <summary>
        /// This method is used to Delete access link when Delivery Point is deleted.
        /// </summary>
        /// <param name="operationalObjectId">Operational Object unique identifier.</param>
        /// <returns>If <true>then access link deleted succeeded(HttpStatusCode:200),else not found(HttpStatusCode:204).</true></returns>
        [HttpDelete]
        [Route("AccessLink/delete/id:{operationalObjectId}")]
        public Task<bool> DeleteAccessLink(Guid operationalObjectId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.DeleteAccessLink"))
            {
                string methodName = typeof(AccessLinkController) + "." + nameof(DeleteAccessLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                var success = accessLinkBusinessService.DeleteAccessLink(operationalObjectId);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return success;
            }
        }

        /// <summary>
        /// This method gets called when a new OS 3rd Party Location file, with a new location comes.
        /// In this case the DP location will be moved.
        /// The scope of this method is to Delete Access link pointing to old location of the Delivery Point,
        /// And to create a New access link for Deliver Point based on the new location.
        /// </summary>
        /// <param name="deliveryPointGuid">Delivery point unique identifier</param>
        /// <returns>returns the status of update operation</returns>
        [HttpPut("AccessLink/UpdateAccessLinkForMovedDeliveryPoint/id:{deliveryPointId}")]
        public IActionResult UpdateAccessLinkForMovedDeliveryPoint(Guid deliveryPointId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.UpdateAccessLinkForMovedDeliveryPoint"))
            {
                bool isAccessLinkCreateSuccessful = false;
                try
                {
                    string methodName = typeof(AccessLinkController) + "." + nameof(UpdateAccessLinkForMovedDeliveryPoint);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    isAccessLinkCreateSuccessful = accessLinkBusinessService.UpdateAccessLinkForMovedDeliveryPoint(deliveryPointId);

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                }
                catch (AggregateException ex)
                {
                    foreach (var exception in ex.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ex.Flatten().InnerException;

                    throw realExceptions;
                }

                return Ok(isAccessLinkCreateSuccessful);
            }
        }

        #endregion Methods
    }
}