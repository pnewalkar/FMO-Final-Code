using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    /// <summary>
    /// This interface contains declarations of methods for basic and advance search of Delivery route
    /// </summary>
    public interface IDeliveryRouteRepository
    {
        /// <summary>
        /// Fetch the Delivery Route.
        /// </summary>
        /// <param name="operationStateID">operationStateID as Guid</param>
        /// <param name="deliveryScenarioID">deliveryScenarioID as Guid</param>
        /// <returns>DeliveryRoute DTO</returns>
        List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID, Guid userUnit);

        /// <summary>
        /// Fetch Delivery Route for Advance Search
        /// </summary>
        /// <param name="searchText">searchText as string </param>
        /// <returns>DeliveryRoute DTO</returns>
        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText, Guid userUnit);

        /// <summary>
        /// Fetch Delivery route for Basic Search
        /// </summary>
        /// <param name="searchText">searchText as string </param>
        /// <returns>DeliveryRoute DTO</returns>
        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText, Guid userUnit);

        /// <summary>
        /// Get the count of delivery route
        /// </summary>
        /// <param name="searchText">searchText as string </param>
        /// <returns>The total count of delivery route</returns>
        Task<int> GetDeliveryRouteCount(string searchText, Guid userUnit);
    }
}