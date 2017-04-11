﻿using System;
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

        public List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID)
        {
            return deliveryRouteRepository.FetchDeliveryRoute(operationStateID, deliveryScenarioID);
        }

        public List<ReferenceDataDTO> FetchRouteLogStatus()
        {
            return referenceDataCategoryRepository.RouteLogStatus();
        }

        public List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryScenarioID)
        {
            return scenarioRepository.FetchScenario(operationStateID, deliveryScenarioID);
        }

        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteforBasicSearch(string searchText)
        {
            return await deliveryRouteRepository.FetchDeliveryRouteForBasicSearch(searchText);
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