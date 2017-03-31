using Fmo.BusinessServices.Interfaces;
using System.Collections.Generic;
using Fmo.DTO;
using Fmo.DataServices.Repositories.Interfaces;
using System;

namespace Fmo.BusinessServices.Services
{
    public class DeliveryRouteBusinessService : IDeliveryRouteBusinessService
    {
        private IDeliveryRouteRepository routeSimulationRespository;

        public DeliveryRouteBusinessService(IDeliveryRouteRepository routeSimulationRespository)
        {
            this.routeSimulationRespository = routeSimulationRespository;
        }

        public List<DTO.DeliveryRouteDTO> ListOfRoute()
        {
            return routeSimulationRespository.ListOfRoute();
        }

        public List<DTO.ReferenceDataDTO> ListOfRouteLogStatus()
        {
            return routeSimulationRespository.ListOfRouteLogStatus();
        }

        public List<DTO.ScenarioDTO> ListOfScenario()
        {
            return routeSimulationRespository.ListOfScenario();
        }

        public List<DeliveryRouteDTO> FetchDeliveryRoute(string searchText)
        {
            return routeSimulationRespository.FetchDeliveryRoute(searchText);
        }
    }
}
