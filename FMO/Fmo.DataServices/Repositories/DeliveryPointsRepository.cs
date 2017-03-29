namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Dto = Fmo.DTO;
    using Entity = Fmo.Entities;

    public class DeliveryPointsRepository : RepositoryBase<Entity.DeliveryPoint, FMODBContext>, IDeliveryPointsRepository
    {
        public DeliveryPointsRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<Dto.DeliveryPointDTO> SearchDelievryPoints()
        {
            try
            {
                // var result = DataContext.DeliveryPoints.ToList();
                ////IAutoMapper<Entity.DeliveryPoint, Dto.DeliveryPoint> deliveryMapper
                // return GenericMapper.MapList<Entity.DeliveryPoint, Dto.DeliveryPoint>(result);
                return new List<Dto.DeliveryPointDTO>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}