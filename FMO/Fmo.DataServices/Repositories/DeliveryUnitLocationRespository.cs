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
    public class DeliveryUnitLocationRespository : RepositoryBase<UnitLocation, FMODBContext>, IDeliveryUnitLocationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryUnitLocationRespository"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public DeliveryUnitLocationRespository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Fetch the delivery unit.
        /// </summary>
        /// <param name="unitId">The unit identifier.</param>
        /// <returns>
        /// List
        /// </returns>
        public List<DeliveryUnitLocationDTO> FetchDeliveryUnit(Guid unitId)
        {
            try
            {
                var result = DataContext.UnitLocations.Where(x => x.ID == unitId && x.UnitBoundryPolygon != null).ToList();
                return GenericMapper.MapList<UnitLocation, DeliveryUnitLocationDTO>(result.ToList());
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
