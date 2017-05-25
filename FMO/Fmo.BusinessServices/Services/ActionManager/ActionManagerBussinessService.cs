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
    /// <summary>
    /// This class contains methods for fetching the role based access functions.
    /// </summary>
    /// <seealso cref="Fmo.BusinessServices.Interfaces.IActionManagerBussinessService" />
    public class ActionManagerBussinessService : IActionManagerBussinessService
    {
        private IActionManagerRepository actionManagerRepository = default(IActionManagerRepository);

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionManagerBussinessService"/> class.
        /// </summary>
        /// <param name="actionManagerRepository">The action manager repository.</param>
        public ActionManagerBussinessService(IActionManagerRepository actionManagerRepository)
        {
            this.actionManagerRepository = actionManagerRepository;
        }

        /// <summary>
        /// Gets the role based access functions.
        /// </summary>
        /// <param name="userUnitInfo">The user unit information.</param>
        /// <returns>RoleAccessDTO</returns>
        public async Task<List<RoleAccessDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDTO userUnitInfo)
        {
            var roleAccessDto = await actionManagerRepository.GetRoleBasedAccessFunctions(userUnitInfo);
            return roleAccessDto;
        }
    }
}
