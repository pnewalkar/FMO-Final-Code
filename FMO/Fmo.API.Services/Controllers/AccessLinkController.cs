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
        [HttpPost("CreateManual")]
        public IActionResult CreateManualAccessLink([FromBody] AccessLinkManualCreateModelDTO accessLinkDto)
        {
            //accessLinkDto = new AccessLinkManualCreateModelDTO
            //{
            //    AccessLinkLine = "LINESTRING (512722.70000000019 104752.6799999997, 512722.70000000019 104738)",
            //    NetworkIntersectionPoint = "POINT (512722.70000000019 104738)",
            //    NetworkLink_GUID = Guid.Parse("BC3E8414-DA95-4924-9C0D-B8D343C97E0A"),
            //    OperationalObjectPoint = "POINT (512722.70000000019 104752.6799999997)",
            //    OperationalObject_GUID = Guid.NewGuid(),
            //};
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isSaved = accessLinkBussinessService.CreateAccessLink(accessLinkDto);
            return Ok(isSaved);
        }

        /// <summary>
        /// This method is used to check the manual access link adj length
        /// </summary>
        ///<param name="accessLinkManualCreateModelDTO">access link object of which adj length needs to be calculated</param>
        /// <returns>returns calculated path length</true></returns>
        //[Authorize(Roles = UserAccessFunctionsConstants.ViewAccessLinks)]
        //[Route("GetWorkloadLength")]
        [HttpPost("GetWorkloadLength")]
        public IActionResult GetAdjPathLength([FromBody] AccessLinkManualCreateModelDTO accessLinkManualCreateModelDTO)
        {
            //AccessLinkManualCreateModelDTO accessLinkDto = new AccessLinkManualCreateModelDTO
            //{
            //    //AccessLinkLine = "LINESTRING (512722.70000000019 104752.6799999997, 512722.70000000019 104738)",
            //    //NetworkIntersectionPoint = "POINT (512722.70000000019 104738)",
            //    ////NetworkLink_GUID = Guid.Parse("BC3E8414-DA95-4924-9C0D-B8D343C97E0A"),
            //    //OperationalObjectPoint = "POINT (512722.70000000019 104752.6799999997)",
            //    ////OperationalObject_GUID = Guid.NewGuid(),
            //};

            decimal pathlength = accessLinkBussinessService.GetAdjPathLength(accessLinkManualCreateModelDTO);
            return Ok(pathlength);
        }

        /// <summary>
        /// This method is used to check whether the access link is valid or not.
        /// </summary>
        ///<param name="accessLinkManualCreateModelDTO">access link object to be checked for valid access link</param>
        /// <returns>returns whether an access link is valid</true></returns>
        //[Authorize(Roles = UserAccessFunctionsConstants.ViewAccessLinks)]
        //[Route("GetWorkloadLength")]
        [HttpPost("CheckAccessLinkIsValid")]
        public IActionResult CheckAccessLinkIsValid([FromBody] AccessLinkManualCreateModelDTO accessLinkManualCreateModelDTO)
        {
            bool isValid = accessLinkBussinessService.CheckManualAccessLinkIsValid(accessLinkManualCreateModelDTO.BoundingBoxCoordinates, accessLinkManualCreateModelDTO.AccessLinkLine);
            return Ok(isValid);
        }
    }
}