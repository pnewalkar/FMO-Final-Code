using System;
using System.Collections.Generic;
using System.Linq;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    public class UnitLocationRepository : RepositoryBase<UnitLocation, FMODBContext>, IUnitLocationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitLocationRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public UnitLocationRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Fetch the delivery unit.
        /// </summary>
        /// <param name="unitId">The unit identifier.</param>
        /// <returns>
        /// The <see cref="UnitLocationDTO"/>.
        /// </returns>
        public UnitLocationDTO FetchDeliveryUnit(Guid unitId)
        {
            var result = DataContext.UnitLocations.SingleOrDefault(x => x.ID == unitId && x.UnitBoundryPolygon != null);
            return GenericMapper.Map<UnitLocation, UnitLocationDTO>(result);
        }

        /// <summary>
        /// Fetch the delivery units for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// List of <see cref="UnitLocationDTO"/>.
        /// </returns>
        public List<UnitLocationDTO> FetchDeliveryUnitsForUser(Guid userId)
        {
            var result = DataContext.UnitLocations.Where(x => x.UserRoleUnits.Any(uru => uru.User_GUID == userId && uru.Unit_GUID == x.ID) && x.UnitBoundryPolygon != null).ToList();
            return GenericMapper.MapList<UnitLocation, UnitLocationDTO>(result.ToList());
        }
    }
}