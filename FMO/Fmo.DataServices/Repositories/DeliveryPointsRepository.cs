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

        public DeliveryPoint GetDeliveryPointByUDPRN(int uDPRN)
        {
            try
            {
                var objDeliveryPoint = DataContext.DeliveryPoints.Where(n => n.UDPRN == uDPRN).SingleOrDefault();
                return objDeliveryPoint;
            }
            catch (Exception)
            {
                throw;
            }
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