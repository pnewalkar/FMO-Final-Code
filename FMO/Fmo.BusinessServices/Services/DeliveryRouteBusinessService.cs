using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

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

        public List<DeliveryRouteDTO> ListOfRoute(int operationStateID, int deliveryScenarioID)
        {
            return routeSimulationRespository.ListOfRoute(operationStateID, deliveryScenarioID);
        }

        public List<ReferenceDataDTO> ListOfRouteLogStatus()
        {
            return referenceDataCategoryRepository.ListOfRouteLogStatus();
        }

        public List<ScenarioDTO> ListOfScenario(int operationStateID, int deliveryUnitID)
        {
            return scenarioRepository.ListOfScenario(operationStateID, deliveryUnitID);
        }

        public List<DeliveryRouteDTO> FetchDeliveryRoute(string searchText)
        {
            return routeSimulationRespository.FetchDeliveryRoute(searchText);
        }
    }
}