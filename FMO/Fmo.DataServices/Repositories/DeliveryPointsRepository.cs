namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Entity = Fmo.Entities;
    using Fmo.MappingConfiguration;
    using Dto = Fmo.DTO;

    public class SearchDeliveryPointsRepository : RepositoryBase<Entity.DeliveryPoint, FMODBContext>, IDeliveryPointsRepository
    {
        public SearchDeliveryPointsRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<Dto.DeliveryPoint> SearchDelievryPoints()
        {
            try
            {
                var result = DataContext.DeliveryPoints.ToList();
                //IAutoMapper<Entity.DeliveryPoint, Dto.DeliveryPoint> deliveryMapper 
                return GenericMapper.MapList<Entity.DeliveryPoint, Dto.DeliveryPoint>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
