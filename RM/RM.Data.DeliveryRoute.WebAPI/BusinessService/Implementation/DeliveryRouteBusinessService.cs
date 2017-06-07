using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.HelperMiddleware;
using RM.DataManagement.DeliveryRoute.WebAPI.IntegrationService;

namespace RM.DataManagement.DeliveryRoute.WebAPI.BusinessService.Implementation
{
    public class DeliveryRouteBusinessService : IDeliveryRouteBusinessService
    {
        private IDeliveryRouteDataService deliveryRouteDataService;
        private IScenarioDataService scenarioDataService;
        private IDeliveryRouteIntegrationService deliveryRouteIntegrationService;
        private IBlockSequenceDataService blockSequenceDataService;

        // private IHttpHandler httpHandler;
        // private string ReferenceDataWebapiUri = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteBusinessService" /> class and other classes.
        /// </summary>
        /// <param name="deliveryRouteDataService">IDeliveryRouteRepository reference</param>
        /// <param name="scenarioDataService">IScenarioRepository reference</param>
        /// <param name="referenceDataBusinessService">The reference data business service.</param>
        public DeliveryRouteBusinessService(IDeliveryRouteDataService deliveryRouteDataService, IScenarioDataService scenarioDataService, IDeliveryRouteIntegrationService deliveryRouteIntegrationService, IBlockSequenceDataService blockSequenceDataService)
        {
            // this.httpHandler = httpHandler;
            this.deliveryRouteDataService = deliveryRouteDataService;
            this.scenarioDataService = scenarioDataService;
            this.deliveryRouteIntegrationService = deliveryRouteIntegrationService;
            this.blockSequenceDataService = blockSequenceDataService;
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
            var deliveryRoutes = deliveryRouteDataService.FetchDeliveryRoute(operationStateID, deliveryScenarioID, userUnit);
            return deliveryRoutes;
        }

        /// <summary>
        /// Fetch the Delivery Scenario.
        /// </summary>
        /// <param name="operationStateID">The operationstate id.</param>
        /// <param name="deliveryScenarioID">The delivery scenario id.</param>
        /// <returns>List</returns>
        public List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryScenarioID)
        {
            return scenarioDataService.FetchScenario(operationStateID, deliveryScenarioID);
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
            return await deliveryRouteDataService.FetchDeliveryRouteForBasicSearch(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the count of delivery route
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of delivery route</returns>
        public async Task<int> GetDeliveryRouteCount(string searchText, Guid userUnit)
        {
            return await deliveryRouteDataService.GetDeliveryRouteCount(searchText, userUnit).ConfigureAwait(false);
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
            return await deliveryRouteDataService.FetchDeliveryRouteForAdvanceSearch(searchText, unitGuid).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the delivery route details for Pdf Generation.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>DeliveryRouteDTO</returns>
        public async Task<DeliveryRouteDTO> GetDeliveryRouteDetailsforPdfGeneration(Guid deliveryRouteId, Guid unitGuid)
        {
            List<string> categoryNames = new List<string>
            {
                ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                ReferenceDataCategoryNames.OperationalObjectType,
                ReferenceDataCategoryNames.DeliveryRouteMethodType
            };

            var referenceDataCategoryList = deliveryRouteIntegrationService.GetReferenceDataSimpleLists(categoryNames).Result;

            var deliveryRouteDto =
                await deliveryRouteDataService.GetDeliveryRouteDetailsforPdfGeneration(deliveryRouteId, referenceDataCategoryList, unitGuid);
            return deliveryRouteDto;
        }

        /// <summary>
        /// Generates the route log.
        /// </summary>
        /// <param name="deliveryRouteDto">The delivery route dto.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>byte[]</returns>
        public async Task<RouteLogSummaryModelDTO> GenerateRouteLog(DeliveryRouteDTO deliveryRouteDto, Guid userUnit)
        {
            Guid operationalObjectTypeForDp = deliveryRouteIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.OperationalObjectType, ReferenceDataValues.OperationalObjectTypeDP).Result;
            return await deliveryRouteDataService.GenerateRouteLog(deliveryRouteDto, userUnit, operationalObjectTypeForDp);
        }

        /// <summary>
        /// Method to create block sequence for delivery point
        /// </summary>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <param name="deliveryPointId">deliveryPointId</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateBlockSequenceForDeliveryPoint(Guid deliveryRouteId, Guid deliveryPointId)
        {
            bool isBlockSequencInserted = false;
            List<string> categoryNames = new List<string> { ReferenceDataCategoryNames.OperationalObjectType };
            var referenceDataCategoryList = deliveryRouteIntegrationService.GetReferenceDataSimpleLists(categoryNames).Result;
            Guid operationalObjectTypeForDp = referenceDataCategoryList
              .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.OperationalObjectType)
              .SelectMany(x => x.ReferenceDatas)
              .Where(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).Select(x => x.ID)
              .SingleOrDefault();
            BlockSequenceDTO blockSequenceDTO = new BlockSequenceDTO { ID = Guid.NewGuid(), OperationalObjectType_GUID = operationalObjectTypeForDp, OperationalObject_GUID = deliveryPointId };
            isBlockSequencInserted = await blockSequenceDataService.AddBlockSequence(blockSequenceDTO, deliveryRouteId);
            return isBlockSequencInserted;
        }
    }
}