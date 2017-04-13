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
    public class DeliveryUnitLocationRespository : RepositoryBase<DeliveryUnitLocation, FMODBContext>, IDeliveryUnitLocationRepository
    {
        public DeliveryUnitLocationRespository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Fetch the delivery unit.
        /// </summary>
        /// <returns>List</returns>
        public List<DeliveryUnitLocationDTO> FetchDeliveryUnit()
        {
            try
            {
                var result = DataContext.DeliveryUnitLocations.ToList();
                return GenericMapper.MapList<DeliveryUnitLocation, DeliveryUnitLocationDTO>(result.ToList());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
