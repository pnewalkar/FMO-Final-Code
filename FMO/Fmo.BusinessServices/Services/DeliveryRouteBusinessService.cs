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
        private IDeliveryRouteRepository deliveryRouteRepository;
        private IReferenceDataCategoryRepository referenceDataCategoryRepository;
        private IScenarioRepository scenarioRepository;
        private IDeliveryUnitLocationRepository deliveryUnitLocationRespository;

        public DeliveryRouteBusinessService(IDeliveryRouteRepository deliveryRouteRepository, IReferenceDataCategoryRepository referenceDataCategoryRepository, IScenarioRepository scenarioRepository, IDeliveryUnitLocationRepository deliveryUnitLocationRespository)
        {
            this.deliveryRouteRepository = deliveryRouteRepository;
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
            this.scenarioRepository = scenarioRepository;
            this.deliveryUnitLocationRespository = deliveryUnitLocationRespository;
        }

        public List<DeliveryRouteDTO> FetchDeliveryRoute(int operationStateID, int deliveryScenarioID)
        {
            return deliveryRouteRepository.FetchDeliveryRoute(operationStateID, deliveryScenarioID);
        }

        public List<ReferenceDataDTO> FetchRouteLogStatus()
        {
            return referenceDataCategoryRepository.RouteLogStatus();
        }

        public List<ScenarioDTO> FetchDeliveryScenario(int operationStateID, int deliveryUnitID)
        {
            return scenarioRepository.FetchScenario(operationStateID, deliveryUnitID);
        }

        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteforBasicSearch(string searchText)
        {
            return await deliveryRouteRepository.FetchDeliveryRouteforBasicSearch(searchText);
        }

        public List<DeliveryUnitLocationDTO> FetchDeliveryUnit()
        {
            return deliveryUnitLocationRespository.FetchDeliveryUnit();
        }

        public Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText)
        {
            throw new NotImplementedException();
        } 
    }
}