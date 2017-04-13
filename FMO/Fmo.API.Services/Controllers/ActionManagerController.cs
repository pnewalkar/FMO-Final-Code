using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    [Route("api/ActionManager")]
    public class ActionManagerController : Controller
    {
        IActionManagerBussinessService actionManagerBussinessService = default(IActionManagerBussinessService);

        public ActionManagerController(IActionManagerBussinessService actionManagerBussinessService)
        {
            this.actionManagerBussinessService = actionManagerBussinessService;
        }

        // POST api/values
        [HttpPost("RoleActions")]
        public async Task<List<RoleAccessDTO>> RoleActions([FromBody]UserUnitInfoDTO userUnitInfoDto)
        {
           var roleAccessDto = await actionManagerBussinessService.GetRoleBasedAccessFunctions(userUnitInfoDto);
           return roleAccessDto;
        }
    }
}
