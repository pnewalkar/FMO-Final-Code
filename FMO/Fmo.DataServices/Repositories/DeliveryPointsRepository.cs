namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
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

        public List<Dto.DeliveryPointDTO> SearchDeliveryPoints()
        {
            try
            {
                // var result = DataContext.DeliveryPoints.ToList();
                ////IAutoMapper<Entity.DeliveryPoint, Dto.DeliveryPoint> deliveryMapper
                // return GenericMapper.MapList<Entity.DeliveryPoint, Dto.DeliveryPoint>(result);
                return new List<Dto.DeliveryPointDTO>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DeliveryPoint GetDeliveryPointByUDPRN(int uDPRN)
        {
            try
            {
                var objDeliveryPoint = DataContext.DeliveryPoints.Where(n => n.UDPRN == uDPRN).SingleOrDefault();

                // return context.Students.Find(id);
                return objDeliveryPoint;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool InsertDeliveryPoint(DeliveryPoint objDeliveryPoint)
        {
            bool saveFlag = false;
            try
            {
                if (objDeliveryPoint != null)
                {
                    DataContext.DeliveryPoints.Add(objDeliveryPoint);
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