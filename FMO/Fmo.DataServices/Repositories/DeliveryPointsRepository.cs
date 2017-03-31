namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;

    public class DeliveryPointsRepository : RepositoryBase<DeliveryPoint, FMODBContext>, IDeliveryPointsRepository
    {
        public DeliveryPointsRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<DeliveryPointDTO> SearchDelievryPoints()
        {
            try
            { 

             var result = DataContext.DeliveryPoints.ToList();
             return GenericMapper.MapList<DeliveryPoint, DeliveryPointDTO>(result.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}