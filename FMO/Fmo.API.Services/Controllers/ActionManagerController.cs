using System;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.Common.Interface;
using Fmo.Common.ResourceFile;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// This class contains methods for fetching Action Roles.
    /// </summary>
    [Route("api/ActionManager")]
    public class ActionManagerController : FmoBaseController
    {
        private IActionManagerBussinessService actionManagerBussinessService = default(IActionManagerBussinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public ActionManagerController(IActionManagerBussinessService actionManagerBussinessService)
        {
            this.actionManagerBussinessService = actionManagerBussinessService;
        }

        /// <summary>
        /// This method is used to fetch Action Roles.
        /// </summary>
        /// <param name="userUnitInfoDto">userUnitInfoDto</param>
        /// <returns>List of Role Access Dto</returns>
        // POST api/values
        [HttpPost("RoleActions")]
        public async Task<List<RoleAccessDTO>> RoleActions([FromBody]UserUnitInfoDTO userUnitInfoDto)
        {
            try
            {
                var roleAccessDto = await actionManagerBussinessService.GetRoleBasedAccessFunctions(userUnitInfoDto);
                return roleAccessDto;
            }
            catch (AggregateException ae)
            {
                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }
    }
}