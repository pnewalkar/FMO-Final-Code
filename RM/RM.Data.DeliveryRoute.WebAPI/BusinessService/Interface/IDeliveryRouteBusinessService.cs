using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.DataManagement.DeliveryRoute.WebAPI.DTO;

namespace RM.DataManagement.DeliveryRoute.WebAPI.BusinessService
{
    public interface IDeliveryRouteBusinessService
    {
        /// <summary>
        /// Get route details specific to scenario.
        /// </summary>
        /// <param name="scenarioID">ID of the selected scenario</param>
        /// <returns>Returns list of route on the basis of selected scenario</returns>
        List<RouteDTO> GetScenarioRoutes(Guid scenarioID);

        /// <summary>
        /// Get filtered routes on basis of search text for Advance Search .
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>Returns list of routes that matches the search text</returns>
        Task<List<RouteDTO>> GetRoutesForAdvanceSearch(string searchText, Guid locationId);

        /// <summary>
        /// Get filtered routes on basis of search text for basic Search .
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>Returns list of routes that matches the search text</returns>
        Task<List<RouteDTO>> GetRoutesForBasicSearch(string searchText, Guid locationId);

        /// <summary>
        /// Get filtered route count
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>The total count of delivery route</returns>
        Task<int> GetRouteCount(string searchText, Guid locationId);

        /// <summary>
        /// Gets the delivery route details for Pdf Generation.
        /// </summary>
        /// <param name="routeId">The delivery route identifier.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>DeliveryRouteDTO</returns>
        Task<RouteDTO> GetRouteSummary(Guid routeId);

        /// <summary>
        /// Generates the route log.
        /// </summary>
        /// <param name="routeDetails">Route Details</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>Route log summary</returns>
        Task<RouteLogSummaryDTO> GenerateRouteLog(RouteDTO routeDetails);

        /// <summary>
        /// Get route details specific to postcode
        /// </summary>
        /// <param name="postCodeUnit">Post code</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>List of routes</returns>
        Task<List<RouteDTO>> GetPostCodeSpecificRoutes(string postCodeUnit, Guid locationId);
    }
}