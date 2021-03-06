﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.DataManagement.DeliveryRoute.WebAPI.DataDTO;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DataService
{
    /// <summary>
    /// This interface contains declarations of methods for basic and advance search of Delivery route
    /// </summary>
    public interface IDeliveryRouteDataService
    {
        /// <summary>
        /// Get route details specific to scenario.
        /// </summary>
        /// <param name="scenarioID">ID of the selected scenario</param>
        /// <returns>Returns list of route on the basis of selected scenario</returns>
        Task<List<RouteDataDTO>> GetScenarioRoutes(Guid scenarioID);

        /// <summary>
        /// Get filtred routes on basis of search text for Advance Search .
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>Returns list of routes that matches the search text</returns>
        Task<List<RouteDataDTO>> GetRoutesForAdvanceSearch(string searchText, Guid locationId);

        /// <summary>
        ///Get filtered routes on basis of search text for basic Search .
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>Returns list of routes that matches the search text</returns>
        Task<List<RouteDataDTO>> GetRoutesForBasicSearch(string searchText, Guid locationId);

        /// <summary>
        /// Get filtered route count
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>The total count of delivery route</returns>
        Task<int> GetRouteCount(string searchText, Guid locationId);

        /// <summary>
        /// Gets the delivery route detailsfor PDF generation.
        /// </summary>
        /// <param name="routeId">Selected route Id</param>
        /// <param name="referenceDataCategoryDtoList">The reference data category dto list.</param>
        /// <returns>Route details </returns>
        Task<RouteDataDTO> GetRouteSummary(Guid routeId, List<ReferenceDataCategoryDTO> referenceDataCategoryDtoList);

        /// <summary>
        /// retrieve Route Sequenced Point By passing RouteID specific to unit
        /// </summary>
        /// <param name="routeId">Selected Route</param>
        /// <returns>
        /// List of route log sequenced points
        /// </returns>
        Task<List<RouteLogSequencedPointsDataDTO>> GetSequencedRouteDetails(Guid routeId);

        /// <summary>
        /// Get route details specific to loaction
        /// </summary>
        /// <param name="locationId">Location ID</param>
        /// <returns> List of routes specific to location </returns>
        Task<List<RouteDataDTO>> GetRoutesByLocation(Guid locationId);

        /// <summary>
        /// Get route details mapped to delivery point
        /// </summary>
        /// <param name="deliveryPointId">Delivery Point Id</param>
        /// <returns>Route Details</returns>
        Task<RouteDataDTO> GetRouteByDeliverypoint(Guid deliveryPointId);

        /// <summary>
        /// Delete delivery point reference from route activity table.
        /// </summary>
        /// <param name="deliveryPointId">Delivery point Id</param>
        /// <returns>boolean value</returns>
        Task<bool> DeleteDeliveryPointRouteMapping(Guid deliveryPointId);
    }
}