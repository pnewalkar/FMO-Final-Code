using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    public class ActionManagerBussinessService : IActionManagerBussinessService
    {
        private IActionManagerRepository actionManagerRepository = default(IActionManagerRepository);

        public ActionManagerBussinessService(IActionManagerRepository actionManagerRepository)
        {
            this.actionManagerRepository = actionManagerRepository;
        }

        public async Task<List<RoleAccessDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDTO userUnitInfo)
        {
            try
            {
                var roleAccessDto = await actionManagerRepository.GetRoleBasedAccessFunctions(userUnitInfo);
                return roleAccessDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
