using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;
using System;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryRouteBusinessService
    {
        /// <summary>
        /// Fetch the Delivery unit.
        /// </summary>
        /// <returns>List</returns>
        List<DeliveryUnitLocationDTO> FetchDeliveryUnit();

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
        /// <param name="operationStateID">Guid</param>
        /// <param name="deliveryScenarioID">Guid</param>
        /// <returns>List</returns>
        List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID);

        /// <summary>
        /// Fetch the Delivery Scenario by passing the operationStateID and deliveryScenarioID.
        /// </summary>
        /// <param name="operationStateID"></param>
        /// <param name="deliveryScenarioID"></param>
        /// <returns>List</returns>
        List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryScenarioID);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteforBasicSearch(string searchText);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText);
    }
}