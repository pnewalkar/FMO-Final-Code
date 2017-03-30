using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;

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
    }
}