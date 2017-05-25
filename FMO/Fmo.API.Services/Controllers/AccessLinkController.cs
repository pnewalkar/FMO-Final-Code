using System;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Constants;
using Fmo.Common.Interface;
using Fmo.DTO.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// This class contains methods used to fetch Access Links data.
    /// </summary>
    [Route("api/[controller]")]
    public class AccessLinkController : FmoBaseController
    {
        private IAccessLinkBusinessService accessLinkBussinessService = default(IAccessLinkBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public AccessLinkController(IAccessLinkBusinessService businessService, ILoggingHelper loggingHelper)
        {
            this.accessLinkBussinessService = businessService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// This method is used to fetch Access Link.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <returns>string of Access link data</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewAccessLinks)]
        [Route("GetAccessLinks")]
        [HttpGet]
        public string GetAccessLinks(string boundaryBox)
        {
            return accessLinkBussinessService.GetAccessLinks(boundaryBox, CurrentUserUnit);
        }

        /// <summary>
        /// This method is used to create automatic Access Link .
        /// </summary>
        /// <param name="operationalObjectId">Operational Object unique identifier.</param>
        /// <param name="operationalObjectTypeId">Operational Object type unique identifier.</param>
        /// <returns>If <true> then access link creation succeeded,else failure.</true></returns>
        [HttpPost("Create")]
        public bool CreateAccessLink(Guid operationalObjectId, Guid operationalObjectTypeId)
        {
            using (loggingHelper.FmoTraceManager.StartTrace("Controller.CreateAccessLink"))
            {
                bool success = false;

                loggingHelper.LogInfo("Method CreateAccessLink entered", "General", 8, 8003, "Trace Log");

                success = accessLinkBussinessService.CreateAccessLink(operationalObjectId, operationalObjectTypeId);

                loggingHelper.LogInfo("Method CreateAccessLink exited", "General", 8, 8004, "Trace Log");

                return success;
            }
        }

        /// <summary>
        /// This method is used to create manual Access Link .
        /// </summary>
        ///<param name="accessLinkDto">access link object to be stored</param>
        /// <returns>If <true> then access link creation succeeded,else failure.</true></returns>
        [HttpPost("Create")]
        public bool CreateManualAccessLink([FromBody] AccessLinkManualCreateModelDTO accessLinkDto)
        {
            using (loggingHelper.FmoTraceManager.StartTrace("Controller.CreateManualAccessLink"))
            {
                loggingHelper.LogInfo("Method CreateManualAccessLink entered", "General", 8, 8103, "Trace Log");

                bool success = false;

                accessLinkDto = new AccessLinkManualCreateModelDTO
                {
                    AccessLinkLine = "LINESTRING (512722.70000000019 104752.6799999997, 512722.70000000019 104738)",
                    NetworkIntersectionPoint = "POINT (512722.70000000019 104738)",
                    NetworkLink_GUID = Guid.Parse("BC3E8414-DA95-4924-9C0D-B8D343C97E0A"),
                    OperationalObjectPoint = "POINT (512722.70000000019 104752.6799999997)",
                    OperationalObject_GUID = Guid.NewGuid(),
                };

                success = accessLinkBussinessService.CreateAccessLink(accessLinkDto);

                loggingHelper.LogInfo("Method CreateManualAccessLink exited", "General", 8, 8104, "Trace Log");

                return success;
            }
        }
    }
}