using System;
using System.Collections.Generic;
using System.Linq;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.DataMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    public class UnitLocationDataService : DataServiceBase<UnitLocation, RMDBContext>, IUnitLocationDataService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitLocationDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public UnitLocationDataService(IDatabaseFactory<RMDBContext> databaseFactory)
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
            var result = (from a in DataContext.UnitLocations.AsNoTracking()
                          join b in DataContext.PostalAddresses.AsNoTracking() on a.UnitAddressUDPRN equals b.UDPRN
                          join c in DataContext.Postcodes.AsNoTracking() on b.PostCodeGUID equals c.ID
                          join d in DataContext.PostcodeSectors.AsNoTracking() on c.SectorGUID equals d.ID
                          join e in DataContext.PostcodeDistricts.AsNoTracking() on d.DistrictGUID equals e.ID
                          join f in DataContext.PostcodeAreas.AsNoTracking() on e.AreaGUID equals f.ID
                          join g in DataContext.UserRoleUnits.AsNoTracking() on a.ID equals g.Unit_GUID
                          where g.User_GUID == userId && a.UnitBoundryPolygon != null
                          select new UnitLocationDTO { ID = a.ID, UnitName = a.UnitName, Area = f.Area, UnitAddressUDPRN = a.UnitAddressUDPRN, UnitBoundryPolygon = a.UnitBoundryPolygon, ExternalId = a.ExternalId }).ToList();
            return result;
        }
    }
}