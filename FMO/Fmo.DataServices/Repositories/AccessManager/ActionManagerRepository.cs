using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories
{
    public class ActionManagerRepository : RepositoryBase<AccessFunction, FMODBContext>, IActionManagerRepository
    {
        public ActionManagerRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public async Task<List<RoleAccessDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDTO userUnitInfo)
        {
            var roleAccessDto = await DataContext.AccessFunctions.AsNoTracking()
                .Where(x => x.UserName.Equals(userUnitInfo.UserName) && x.Unit_GUID.Equals(userUnitInfo.UnitGuid))
                .Select(x => new RoleAccessDTO
                {
                    RoleName = x.RoleName,
                    Unit_GUID = x.Unit_GUID,
                    UserName = x.UserName,
                    FunctionName = x.FunctionName,
                    ActionName = x.ActionName,
                    UserId = x.UserId
                }).ToListAsync();

            return roleAccessDto;
        }
    }
}
