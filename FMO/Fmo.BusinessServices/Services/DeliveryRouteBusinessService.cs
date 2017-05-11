using System;
using System.Collections.Generic;
using System.Linq;
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
        private IReferenceDataBusinessService referenceDataBusinessService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteBusinessService"/> class and other classes.
        /// </summary>
        /// <param name="deliveryRouteRepository">IDeliveryRouteRepository reference</param>
        /// <param name="referenceDataCategoryRepository">IReferenceDataCategoryRepository reference</param>
        /// <param name="scenarioRepository">IScenarioRepository reference</param>
        public DeliveryRouteBusinessService(IDeliveryRouteRepository deliveryRouteRepository, IReferenceDataCategoryRepository referenceDataCategoryRepository, IScenarioRepository scenarioRepository, IReferenceDataBusinessService referenceDataBusinessService)
        {
            this.deliveryRouteRepository = deliveryRouteRepository;
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
            this.scenarioRepository = scenarioRepository;
            this.referenceDataBusinessService = referenceDataBusinessService;
        }

        /// <summary>
        /// Fetch the Delivery Route by passing operationStateID and deliveryScenarioID.
        /// </summary>
        /// <param name="operationStateID">The operationstate id.</param>
        /// <param name="deliveryScenarioID">The delivery scenario id.</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// List
        /// </returns>
        public List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID, Guid userUnit)
        {
            var deliveryRoutes = deliveryRouteRepository.FetchDeliveryRoute(operationStateID, deliveryScenarioID, userUnit);
            return deliveryRoutes;
        }

        /// <summary>
        /// Fetch the Route Log Status.
        /// </summary>
        /// <returns>List</returns>
        public List<ReferenceDataDTO> FetchRouteLogStatus()
        {
            return referenceDataBusinessService.FetchRouteLogStatus();
        }

        /// <summary>
        /// Fetch the Route Log Selection Type.
        /// </summary>
        /// <returns>List</returns>
        public List<ReferenceDataDTO> FetchRouteLogSelectionType()
        {
            return referenceDataBusinessService.FetchRouteLogSelectionType();
        }

        /// <summary>
        /// Fetch the Delivery Scenario.
        /// </summary>
        /// <param name="operationStateID">The operationstate id.</param>
        /// <param name="deliveryScenarioID">The delivery scenario id.</param>
        /// <returns>List</returns>
        public List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryScenarioID)
        {
            return scenarioRepository.FetchScenario(operationStateID, deliveryScenarioID);
        }

        /// <summary>
        /// Fetch the Delivery Route for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText, Guid userUnit)
        {
            return await deliveryRouteRepository.FetchDeliveryRouteForBasicSearch(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the count of delivery route
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of delivery route</returns>
        public async Task<int> GetDeliveryRouteCount(string searchText, Guid userUnit)
        {
            return await deliveryRouteRepository.GetDeliveryRouteCount(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch Delivery Route for Advance Search
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// List of <see cref="DeliveryRouteDTO"/>.
        /// </returns>
        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText, Guid unitGuid)
        {
            return await deliveryRouteRepository.FetchDeliveryRouteForAdvanceSearch(searchText, unitGuid);
        }

        public DeliveryRouteDTO GetDeliveryRouteDetails(Guid deliveryRouteId)
        {
            //TODO: Move this to resource file
            List<string> categoryNames = new List<string>
            {
                "DeliveryPoint Use Indicator",
                "Operational Object Type",
                "Delivery Route Method Type",
                "Delivery Route Transport Type"
            };

            var referenceDataCategoryList = referenceDataCategoryRepository.GetReferenceDataCategoriesByCategoryNames(categoryNames);

            var deliveryRouteDto = deliveryRouteRepository.GetDeliveryRouteDetails(deliveryRouteId, referenceDataCategoryList);
            return deliveryRouteDto.Result;
        }
    }
}