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
    public class UserRoleUnitBussinessService : IUserRoleUnitBussinessService
    {
        private IUserRoleUnitRepository userRoleUnitRepository = default(IUserRoleUnitRepository);

        public UserRoleUnitBussinessService(IUserRoleUnitRepository userRoleUnitRepository)
        {
            this.userRoleUnitRepository = userRoleUnitRepository;
        }

        public Task<Guid> GetUserUnitInfo(string userName)
        {
            try
            {
                return userRoleUnitRepository.GetUserUnitInfo(userName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
