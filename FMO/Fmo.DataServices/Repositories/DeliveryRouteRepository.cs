﻿namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;

    public class DeliveryRouteRepository : RepositoryBase<DeliveryRoute, FMODBContext>, IDeliveryRouteRepository
    {
        public DeliveryRouteRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<DTO.DeliveryRoute> ListOfRoute()
        {
            IEnumerable<DeliveryRoute> result = DataContext.DeliveryRoutes.ToList().Where(x => x.DeliveryScenario_Id == 1);
            return GenericMapper.MapList<DeliveryRoute, DTO.DeliveryRoute>(result.ToList());
        }

        public List<DTO.ReferenceDataDTO> ListOfRouteLogStatus()
        {
            var result = DataContext.ReferenceDatas.ToList();
            return GenericMapper.MapList<ReferenceData, DTO.ReferenceDataDTO>(result);
        }

        public List<DTO.ScenarioDTO> ListOfScenario()
        {
            IEnumerable<Scenario> result = DataContext.Scenarios.ToList().Where(x => x.OperationalState_Id == 1 && x.DeliveryUnit_Id == 1);
            return GenericMapper.MapList<Scenario, DTO.ScenarioDTO>(result.ToList());
        }
    }
}
