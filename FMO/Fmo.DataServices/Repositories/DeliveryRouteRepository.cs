using System.Collections.Generic;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;
using Fmo.DTO;
using Fmo.MappingConfiguration;
using System;
using System.Linq;

namespace Fmo.DataServices.Repositories
{
    public class DeliveryRouteRepository : RepositoryBase<DeliveryRoute, FMODBContext>, IDeliveryRouteRepository
    {
        public DeliveryRouteRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<DTO.DeliveryRouteDTO> ListOfRoute(int operationStateID, int deliveryScenarioID)
        {
            IEnumerable<DeliveryRoute> result = DataContext.DeliveryRoutes.ToList().Where(x => x.DeliveryScenario_Id == deliveryScenarioID && x.OperationalStatus_Id == operationStateID);
            return GenericMapper.MapList<DeliveryRoute, DeliveryRouteDTO>(result.ToList());
        }

        public List<DeliveryRouteDTO> FetchDeliveryRoute(string searchText)
        {
            try
            {
                var result = DataContext.DeliveryRoutes.ToList();
                return GenericMapper.MapList<DeliveryRoute, DeliveryRouteDTO>(result.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}