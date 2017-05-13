using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryRouteBusinessService
    {
        /// <summary>
        /// Fetch the Delivery Route status.
        /// </summary>
        /// <returns>List</returns>
        List<ReferenceDataDTO> FetchRouteLogStatus();

        /// <summary>
        /// Fetch the Delivery Route Selection Type.
        /// </summary>
        /// <returns>List</returns>
        List<ReferenceDataDTO> FetchRouteLogSelectionType();

        /// <summary>
        /// Fetch the Delivery Route by passing operationStateID and deliveryScenarioID.
        /// </summary>
        /// <param name="operationStateID">Guid operationStateID</param>
        /// <param name="deliveryScenarioID">Guid deliveryScenarioID</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>List</returns>
        List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID, Guid userUnit);

        /// <summary>
        /// Fetch the Delivery Scenario by passing the operationStateID and deliveryScenarioID.
        /// </summary>
        /// <param name="operationStateID">operationState ID</param>
        /// <param name="deliveryScenarioID">deliveryScenario ID</param>
        /// <returns>List</returns>
        List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryScenarioID);

        /// <summary>
        /// Fetch Delivery Route for Basic Search
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>Task</returns>
        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText, Guid userUnit);

        /// <summary>
        /// Fetch Delivery Route For Advance Search
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>Task</returns>
        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText, Guid userUnit);

        /// <summary>
        /// Get the count of delivery route
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of delivery route</returns>
        Task<int> GetDeliveryRouteCount(string searchText, Guid userUnit);

        /// <summary>
        /// Gets the delivery route details for Pdf Generation.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// DeliveryRouteDTO
        /// </returns>
        Task<DeliveryRouteDTO> GetDeliveryRouteDetailsforPdfGeneration(Guid deliveryRouteId, Guid unitGuid);

        /// <summary>
        /// Generates the route log.
        /// </summary>
        /// <param name="deliveryRouteDto">The delivery route dto.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>byte[]</returns>
        Task<byte[]> GenerateRouteLog(DeliveryRouteDTO deliveryRouteDto, Guid userUnit);
    }
}