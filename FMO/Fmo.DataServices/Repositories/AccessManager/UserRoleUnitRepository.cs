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
    public class UserRoleUnitRepository : RepositoryBase<UserRoleUnit, FMODBContext>, IUserRoleUnitRepository
    {
        public UserRoleUnitRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public async Task<Guid> GetUserUnitInfo(string userName)
        {
            return await DataContext.UserRoleUnits.AsNoTracking()
                .Where(x => x.User.UserName == userName).Select(x => x.Unit_GUID).FirstOrDefaultAsync();
        }
    }
}
