namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Entity = Fmo.Entities;
    using MappingConfiguration;

    public class DeliveryPointsRepository : RepositoryBase<Entity.DeliveryPoint, FMODBContext>, IDeliveryPointsRepository
    {
        public DeliveryPointsRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<DeliveryPointDTO> SearchDeliveryPoints()
        {
            try
            {
                var result = DataContext.DeliveryPoints.ToList();
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