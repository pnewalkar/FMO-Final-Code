using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;

using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    public class ActionManagerDataService : DataServiceBase<AccessFunction, RMDBContext>, IActionManagerDataService
    {
        public ActionManagerDataService(IDatabaseFactory<RMDBContext> databaseFactory)
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
