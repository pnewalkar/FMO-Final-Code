using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Constants;
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
        public string GetAccessLinks(string boundaryBox)
        {
            return "";
            return accessLinkBussinessService.GetAccessLinks(boundaryBox, CurrentUserUnit);
        }

        /// <summary>
        /// This method is used to create Access Link for auto.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <returns>string of Access link data</returns>
        [Route("CreateAccessLink")]
        [HttpPost]
        public bool CreateAccessLink([FromBody] System.Guid operationalObject_GUID)
        {
            return accessLinkBussinessService.CreateAccessLink(operationalObject_GUID);
        }
    }
}