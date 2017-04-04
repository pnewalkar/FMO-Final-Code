using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private IDeliveryUnitLocationRepository deliveryUnitLocationRespository;

        public DeliveryRouteBusinessService(IDeliveryRouteRepository routeSimulationRespository, IReferenceDataCategoryRepository referenceDataCategoryRepository, IScenarioRepository scenarioRepository, IDeliveryUnitLocationRepository deliveryUnitLocationRespository)
        {
            this.routeSimulationRespository = routeSimulationRespository;
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
            this.scenarioRepository = scenarioRepository;
            this.deliveryUnitLocationRespository = deliveryUnitLocationRespository;
        }

        public List<DeliveryRouteDTO> FetchDeliveryRoute(int operationStateID, int deliveryScenarioID)
        {
            return routeSimulationRespository.FetchDeliveryRoute(operationStateID, deliveryScenarioID);
        }

        public List<ReferenceDataDTO> FetchRouteLogStatus()
        {
            return referenceDataCategoryRepository.RouteLogStatus();
        }

        public List<ScenarioDTO> FetchDeliveryScenario(int operationStateID, int deliveryUnitID)
        {
            return scenarioRepository.FetchScenario(operationStateID, deliveryUnitID);
        }

        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRoute(string searchText)
        {
            return await routeSimulationRespository.FetchDeliveryRoute(searchText);
        }

        public List<DeliveryUnitLocationDTO> FetchDeliveryUnit()
        {
            return deliveryUnitLocationRespository.FetchDeliveryUnit();
        }
    }
}