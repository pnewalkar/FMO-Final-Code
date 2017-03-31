using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;

namespace Fmo.BusinessServices.Services
{
    public class DeliveryRouteBusinessService : IDeliveryRouteBusinessService
    {
        private IDeliveryRouteRepository routeSimulationRespository;
        private IReferenceDataCategoryRepository referenceDataCategoryRepository;
        private IScenarioRepository scenarioRepository;

        public DeliveryRouteBusinessService(IDeliveryRouteRepository routeSimulationRespository, IReferenceDataCategoryRepository referenceDataCategoryRepository, IScenarioRepository scenarioRepository)
        {
            this.routeSimulationRespository = routeSimulationRespository;
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
            this.scenarioRepository = scenarioRepository;
        }

        public List<DTO.DeliveryRouteDTO> SearchDeliveryRoute(int operationStateID, int deliveryScenarioID)
        {
            return routeSimulationRespository.SearchDeliveryRoute(operationStateID, deliveryScenarioID);
        }

        public List<DTO.ReferenceDataDTO> RouteLogStatus()
        {
            return referenceDataCategoryRepository.RouteLogStatus();
        }

        public List<DTO.ScenarioDTO> SearchDeliveryScenario(int operationStateID, int deliveryUnitID)
        {
            return scenarioRepository.Scenario(operationStateID, deliveryUnitID);
        }
    }
}