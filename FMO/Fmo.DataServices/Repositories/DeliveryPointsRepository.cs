namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using MappingConfiguration;
    using Entity = Fmo.Entities;

    public class DeliveryPointsRepository : RepositoryBase<Entity.DeliveryPoint, FMODBContext>, IDeliveryPointsRepository
    {
        public DeliveryPointsRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public async Task<List<DeliveryPointDTO>> SearchDeliveryPoints()
        {
            try
            {
                var result = await DataContext.DeliveryPoints.ToListAsync();
                return GenericMapper.MapList<DeliveryPoint, DeliveryPointDTO>(result.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DeliveryPointDTO GetDeliveryPointByUDPRN(int uDPRN)
        {
            var objDeliveryPoint = new DeliveryPointDTO();
            try
            {
                var deliveryPoint = DataContext.DeliveryPoints.Where(n => n.UDPRN == uDPRN).SingleOrDefault();

                GenericMapper.Map(deliveryPoint, objDeliveryPoint);
            }
            catch (Exception)
            {
                throw;
            }

            return objDeliveryPoint;
        }

        public bool InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint)
        {
            bool saveFlag = false;
            try
            {
                if (objDeliveryPoint != null)
                {
                    var deliveryPoint = new DeliveryPoint();
                    GenericMapper.Map(objDeliveryPoint, deliveryPoint);

                    DataContext.DeliveryPoints.Add(deliveryPoint);
                    DataContext.SaveChanges();
                    saveFlag = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return saveFlag;
        }
    }
}