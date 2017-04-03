using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    public class DeliveryRouteBusinessService : IDeliveryRouteBusinessService
    {
        private IDeliveryRouteRepository deliveryRouteRespository;
        private IReferenceDataCategoryRepository referenceDataCategoryRepository;
        private IScenarioRepository scenarioRepository;

        public DeliveryRouteBusinessService(IDeliveryRouteRepository deliveryRouteRespository, IReferenceDataCategoryRepository referenceDataCategoryRepository, IScenarioRepository scenarioRepository)
        {
            this.deliveryRouteRespository = deliveryRouteRespository;
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
            this.scenarioRepository = scenarioRepository;
        }

        public List<DeliveryRouteDTO> FetchDeliveryRoute(int operationStateID, int deliveryScenarioID)
        {
            return deliveryRouteRespository.FetchDeliveryRoute(operationStateID, deliveryScenarioID);
        }

        public List<ReferenceDataDTO> FetchRouteLogStatus()
        {
            return referenceDataCategoryRepository.RouteLogStatus();
        }

        public List<ScenarioDTO> FetchDeliveryScenario(int operationStateID, int deliveryUnitID)
        {
            return scenarioRepository.FetchScenario(operationStateID, deliveryUnitID);
        }

        public List<DeliveryRouteDTO> FetchDeliveryRoute(string searchText)
        {
            return deliveryRouteRespository.FetchDeliveryRoute(searchText);
        }
    }
}