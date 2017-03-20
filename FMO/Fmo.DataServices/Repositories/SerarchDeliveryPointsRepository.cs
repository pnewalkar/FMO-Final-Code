using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Fmo.DataServices.Repositories.Interface;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Entities;

namespace Fmo.DataServices.Repositories
{
    public class SearchDeliveryPointsRepository : RepositoryBase<DeliveryPoint, FMODBContext>, ISearchDeliveryPointsRepository
    {
        public SearchDeliveryPointsRepository(IDatabaseFactory<FMODBContext> databaseFactory) : base(databaseFactory)
        {
        }

        public List<DeliveryPoint> SearchDelievryPoints()
        {
            try
            {
                var result = DataContext.DeliveryPoints.ToList();
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
