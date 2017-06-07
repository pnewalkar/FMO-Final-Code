﻿using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DataService.Interfaces
{
    /// <summary>
    /// This interface contains declarations of methods for basic and advance search of Delivery route
    /// </summary>
    public interface IDeliveryRouteDataService
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

        /// <summary>
        /// Gets the delivery route details for Pdf Generation.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="referenceDataCategoryDtoList">The reference data category dto list.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>
        /// DeliveryRouteDTO
        /// </returns>
        Task<DeliveryRouteDTO> GetDeliveryRouteDetailsforPdfGeneration(Guid deliveryRouteId, List<ReferenceDataCategoryDTO> referenceDataCategoryDtoList, Guid userUnit);

        /// <summary>
        /// Generates the route log.
        /// </summary>
        /// <param name="deliveryRouteDto">The delivery route dto.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>RouteLogSummaryModelDTO</returns>
        Task<RouteLogSummaryModelDTO> GenerateRouteLog(DeliveryRouteDTO deliveryRouteDto, Guid userUnit, Guid operationalObjectTypeForDp);
    }
}