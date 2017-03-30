namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DTO;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using MappingConfiguration;

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
    }
}
