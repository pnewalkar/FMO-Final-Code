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
        Task<List<RouteDTO>> GetScenarioRoutes(Guid scenarioID);

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
        /// <param name="postcodeUnit">Post code</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>List of routes</returns>
        Task<List<RouteDTO>> GetPostcodeSpecificRoutes(string postcodeUnit, Guid locationId);

        /// <summary>
        /// method to save delivery point and selected route mapping in block sequence table
        /// </summary>
        /// <param name="routeId">selected route id</param>
        /// <param name="deliveryPointId">Delivery point unique id</param>
        void SaveDeliveryPointRouteMapping(Guid routeId, Guid deliveryPointId);

        /// <summary>
        /// Get route details mapped to delivery point
        /// </summary>
        /// <param name="deliveryPointId">Delivery Point Id</param>
        /// <returns>Route Details</returns>
        Task<RouteDTO> GetRouteByDeliveryPoint(Guid deliveryPointId);

        /// <summary>
        /// Delete delivery point reference from route activity table.
        /// </summary>
        /// <param name="deliveryPointId">Delivery point Id</param>
        /// <returns>boolean value</returns>
        Task<bool> DeleteDeliveryPointRouteMapping(Guid deliveryPointId);
    }
}