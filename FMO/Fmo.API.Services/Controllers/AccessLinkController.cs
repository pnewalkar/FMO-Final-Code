using System;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Constants;
using Fmo.DTO;
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

        public AccessLinkController(IAccessLinkBusinessService businessService)
        {
            this.accessLinkBussinessService = businessService;
        }

        /// <summary>
        /// This method is used to fetch Access Link.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <returns>string of Access link data</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewAccessLinks)]
        [Route("GetAccessLinks")]
        [HttpGet]
        public IActionResult GetAccessLinks(string boundaryBox)
        {
            string accessLink = accessLinkBussinessService.GetAccessLinks(boundaryBox, CurrentUserUnit);
            return Ok(accessLink);
        }

        /// <summary>
        /// This method is used to create automatic Access Link .
        /// </summary>
        /// <param name="operationalObjectId">Operational Object unique identifier.</param>
        /// <param name="operationalObjectTypeId">Operational Object type unique identifier.</param>
        /// <returns>If <true> then access link creation succeeded,else failure.</true></returns>
        [HttpPost("Create")]
        public IActionResult CreateAccessLink(Guid operationalObjectId, Guid operationalObjectTypeId)
        {
            bool isSaved = accessLinkBussinessService.CreateAccessLink(operationalObjectId, operationalObjectTypeId);
            return Ok(isSaved);
        }

        /// <summary>
        /// This method is used to create manual Access Link .
        /// </summary>
        ///<param name="accessLinkDto">access link object to be stored</param>
        /// <returns>If <true> then access link creation succeeded,else failure.</true></returns>
        [HttpPost("Create")]
        public IActionResult CreateManualAccessLink([FromBody] AccessLinkManualCreateModelDTO accessLinkDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            accessLinkDto = new AccessLinkManualCreateModelDTO
            {
                AccessLinkLine = "LINESTRING (512722.70000000019 104752.6799999997, 512722.70000000019 104738)",
                NetworkIntersectionPoint = "POINT (512722.70000000019 104738)",
                NetworkLink_GUID = Guid.Parse("BC3E8414-DA95-4924-9C0D-B8D343C97E0A"),
                OperationalObjectPoint = "POINT (512722.70000000019 104752.6799999997)",
                OperationalObject_GUID = Guid.NewGuid(),
            };

            bool isSaved = accessLinkBussinessService.CreateAccessLink(accessLinkDto);
            return Ok(isSaved);
        }
    }
}