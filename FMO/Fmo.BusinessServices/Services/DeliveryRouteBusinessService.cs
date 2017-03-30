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
        private IReferenceDataCategoryRepository referenceDataCategoryRepository;
        private IScenarioRepository scenarioRepository;

        public DeliveryRouteBusinessService(IDeliveryRouteRepository routeSimulationRespository, IReferenceDataCategoryRepository referenceDataCategoryRepository, IScenarioRepository scenarioRepository)
        {
            this.routeSimulationRespository = routeSimulationRespository;
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
            this.scenarioRepository = scenarioRepository;
        }

        public List<DTO.DeliveryRouteDTO> ListOfRoute(int operationStateID, int deliveryScenarioID)
        {
            return routeSimulationRespository.ListOfRoute(operationStateID, deliveryScenarioID);
        }

        public List<DTO.ReferenceDataDTO> ListOfRouteLogStatus()
        {
            return referenceDataCategoryRepository.ListOfRouteLogStatus();
        }

        public List<DTO.ScenarioDTO> ListOfScenario(int operationStateID, int deliveryUnitID)
        {
            return scenarioRepository.ListOfScenario(operationStateID, deliveryUnitID);
        }
    }
}
