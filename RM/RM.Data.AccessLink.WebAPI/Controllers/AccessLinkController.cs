using System;
using System.Diagnostics;
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
        #endregion

        #region Constructor
        public AccessLinkController(IAccessLinkBusinessService accessLinkBusinessService, ILoggingHelper loggingHelper)
        {
            this.accessLinkBusinessService = accessLinkBusinessService;
            this.loggingHelper = loggingHelper;
        }
        #endregion

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
            //using (loggingHelper.RMTraceManager.StartTrace("Controller.CreaEteAccessLink"))
            //{
            bool success = false;
            try
            {
                loggingHelper.Log("Method CreateAccessLink entered", TraceEventType.Verbose, null, "General", 8, 8003, "Trace Log");

                success = accessLinkBusinessService.CreateAccessLink(operationalObjectId, operationalObjectTypeId);

                loggingHelper.Log("Method CreateAccessLink exited", TraceEventType.Verbose, null, "General", 8, 8004, "Trace Log");
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

            // }
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
            //using (loggingHelper.RMTraceManager.StartTrace("Controller.CreateManualAccessLink"))
            //{
            loggingHelper.Log("Method CreateManualAccessLink entered", TraceEventType.Verbose, null, "General", 8, 8103, "Trace Log");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isSaved = accessLinkBusinessService.CreateAccessLink(accessLinkDto);

            loggingHelper.Log("Method CreateManualAccessLink exited", TraceEventType.Verbose, null, "General", 8, 8104, "Trace Log");
            return Ok(isSaved);

            //}
        }

        /// <summary>
        /// This method is used to fetch Access Link.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <returns>GeoJson string of Access link data</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewAccessLinks)]
        [HttpGet("AccessLinks")]
        public IActionResult GetAccessLinks(string bbox)
        {
            string accessLink = accessLinkBusinessService.GetAccessLinks(bbox, CurrentUserUnit);
            return Ok(accessLink);
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
            decimal pathlength = accessLinkBusinessService.GetAdjPathLength(accessLinkManualCreateModelDTO);
            return Ok(pathlength);
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
            bool isValid = accessLinkBusinessService.CheckManualAccessLinkIsValid(accessLinkManualCreateModelDTO.BoundingBoxCoordinates, accessLinkManualCreateModelDTO.AccessLinkLine);
            return Ok(isValid);
        } 
        #endregion
    }
}