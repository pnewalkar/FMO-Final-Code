using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.SearchManager.WebAPI.DTO;
using RM.Operational.SearchManager.WebAPI.Integration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RM.Operational.SearchManager.WebAPI.BusinessService
{
    /// <summary>
    /// This class contains business methods for basic and advance search
    /// </summary>
    public class SearchBusinessService : ISearchBusinessService
    {
        private const string DeliveryPointFormat = "{0},{1},{2},{3},{4},{5}";
        private const string StreetNameFormat = "{0},{1}";

        private ISearchIntegrationService searchIntegrationService = default(ISearchIntegrationService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public SearchBusinessService(ISearchIntegrationService searchIntegrationService, ILoggingHelper loggingHelper)
        {
            this.searchIntegrationService = searchIntegrationService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Get results from entities using basic search
        /// </summary>
        /// <param name="searchText">The text to be searched from the entities.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <param name="currentUserUnitType">The user unit type.</param>
        /// <returns>The result set after filtering the values.</returns>
        public async Task<SearchResultDTO> GetBasicSearchDetails(string searchText, Guid userUnit, string currentUserUnitType)
        {
            string methodName = typeof(SearchBusinessService) + "." + nameof(GetBasicSearchDetails);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetBasicSearchDetails"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerBusinessServiceMethodEntryEventId);
                SearchResultDTO searchResultDTO = null;
                string unitName = currentUserUnitType;
                var deliveryRoutes = await searchIntegrationService.FetchDeliveryRouteForBasicSearch(searchText);
                var deliveryRouteCount = await searchIntegrationService.GetDeliveryRouteCount(searchText);

                var postcodes = await searchIntegrationService.FetchPostCodeUnitForBasicSearch(searchText);
                var postCodeCount = await searchIntegrationService.GetPostCodeUnitCount(searchText);

                var deliveryPoints = await searchIntegrationService.FetchDeliveryPointsForBasicSearch(searchText);
                var deliveryPointsCount = await searchIntegrationService.GetDeliveryPointsCount(searchText);

                var streetNames = await searchIntegrationService.FetchStreetNamesForBasicSearch(searchText);
                var streetNetworkCount = await searchIntegrationService.GetStreetNameCount(searchText);

                if (string.Equals(UserUnit.CollectionUnit.GetDescription(), unitName.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    searchResultDTO = MapSearchResultsForCollectionUnit(deliveryRoutes, deliveryRouteCount, postcodes, postCodeCount, deliveryPoints, deliveryPointsCount, streetNames, streetNetworkCount);
                }
                else
                {
                    searchResultDTO = MapSearchResultsForDeliveryUnit(deliveryRoutes, deliveryRouteCount, postcodes, postCodeCount, deliveryPoints, deliveryPointsCount, streetNames, streetNetworkCount);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerBusinessServiceMethodExitEventId);
                return searchResultDTO;
            }
        }

        /// <summary>
        /// Get results from entities using advanced search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="userUnit">The user unit.</param>
        /// <param name="currentUserUnitType">The user unit type.</param>
        /// <returns>search Result Dto</returns>
        public async Task<SearchResultDTO> GetAdvanceSearchDetails(string searchText, Guid userUnit, string currentUserUnitType)
        {
            string methodName = typeof(SearchBusinessService) + "." + nameof(GetAdvanceSearchDetails);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetAdvanceSearchDetails"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerBusinessServiceMethodEntryEventId);
                SearchResultDTO searchResultDTO = null;
                string unitName = currentUserUnitType;
                var deliveryRoutes = await searchIntegrationService.FetchDeliveryRouteForAdvanceSearch(searchText);
                var postcodes = await searchIntegrationService.FetchPostCodeUnitForAdvanceSearch(searchText);
                var streetNames = await searchIntegrationService.FetchStreetNamesForAdvanceSearch(searchText);
                var deliveryPoints = await searchIntegrationService.FetchDeliveryPointsForAdvanceSearch(searchText);

                if (string.Equals(UserUnit.CollectionUnit.GetDescription(), unitName.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    searchResultDTO = MapSearchResultsForCollectionUnit(deliveryRoutes, deliveryRoutes.Count, postcodes, postcodes.Count, deliveryPoints, deliveryPoints.Count, streetNames, streetNames.Count);
                }
                else
                {
                    searchResultDTO = MapSearchResultsForDeliveryUnit(deliveryRoutes, deliveryRoutes.Count, postcodes, postcodes.Count, deliveryPoints, deliveryPoints.Count, streetNames, streetNames.Count);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerBusinessServiceMethodExitEventId);
                return searchResultDTO;
            }
        }

        /// <summary>
        /// Get the result populated in the DTO
        /// </summary>
        /// <param name="deliveryRoutes">The list of delivery routes</param>
        /// <param name="deliveryRouteCount">The count of delivery routes</param>
        /// <param name="postcodes">The list of postcodes</param>
        /// <param name="postCodeCount">The count of postcodes</param>
        /// <param name="deliveryPoints">The list of delivery points</param>
        /// <param name="deliveryPointsCount">The count of delivery points</param>
        /// <param name="streetNames">The list of street names</param>
        /// <param name="streetNetworkCount">The count of street names</param>
        /// <returns>The result set</returns>
        private static SearchResultDTO MapSearchResultsForDeliveryUnit(List<RouteDTO> deliveryRoutes, int? deliveryRouteCount, List<PostCodeDTO> postcodes, int? postCodeCount, List<DeliveryPointDTO> deliveryPoints, int? deliveryPointsCount, List<StreetNameDTO> streetNames, int? streetNetworkCount)
        {
            var searchResultDTO = new SearchResultDTO();

            // postcodes
            foreach (var postcode in postcodes)
            {
                searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO
                {
                    DisplayText = postcode.PostcodeUnit,
                    Type = SearchBusinessEntityType.Postcode
                });
            }

            // streetNames
            foreach (var streetName in streetNames)
            {
                searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO
                {
                    DisplayText = Regex.Replace(
                        string.Format(
                    StreetNameFormat,
                    streetName.NationalRoadCode,
                    streetName.DesignatedName),
                        ",+",
                        ", ").Trim(','),
                    Type = SearchBusinessEntityType.StreetNetwork
                });
            }

            // deliveryPoints
            foreach (var deliveryPoint in deliveryPoints)
            {
                searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO
                {
                    DisplayText = Regex.Replace(
                        string.Format(
                    DeliveryPointFormat,
                    deliveryPoint.PostalAddress.OrganisationName,
                    deliveryPoint.PostalAddress.BuildingName,
                    deliveryPoint.PostalAddress.SubBuildingName,
                    deliveryPoint.PostalAddress.BuildingNumber,
                    deliveryPoint.PostalAddress.Thoroughfare,
                    deliveryPoint.PostalAddress.DependentLocality),
                        ",+",
                        ", ").Trim(','),
                    UDPRN = deliveryPoint.PostalAddress.UDPRN,
                    Type = SearchBusinessEntityType.DeliveryPoint,
                    DeliveryPointGuid = deliveryPoint.ID
                });
            }

            // deliveryRoutes
            foreach (var deliveryRoute in deliveryRoutes)
            {
                searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO
                {
                    DisplayText = deliveryRoute.RouteName,
                    Type = SearchBusinessEntityType.Route
                });
            }

            // Total Counts
            searchResultDTO.SearchCounts.Add(new SearchCountDTO
            {
                Count = deliveryRouteCount + postCodeCount + deliveryPointsCount + streetNetworkCount,
                Type = SearchBusinessEntityType.All
            });

            return searchResultDTO;
        }

        /// <summary>
        /// Get the result populated in the DTO
        /// </summary>
        /// <param name="deliveryRoutes">The list of delivery routes</param>
        /// <param name="deliveryRouteCount">The count of delivery routes</param>
        /// <param name="postcodes">The list of postcodes</param>
        /// <param name="postCodeCount">The count of postcodes</param>
        /// <param name="deliveryPoints">The list of delivery points</param>
        /// <param name="deliveryPointsCount">The count of delivery points</param>
        /// <param name="streetNames">The list of street names</param>
        /// <param name="streetNetworkCount">The count of street names</param>
        /// <returns>The result set</returns>
        private static SearchResultDTO MapSearchResultsForCollectionUnit(List<RouteDTO> deliveryRoutes, int? deliveryRouteCount, List<PostCodeDTO> postcodes, int? postCodeCount, List<DeliveryPointDTO> deliveryPoints, int? deliveryPointsCount, List<StreetNameDTO> streetNames, int? streetNetworkCount)
        {
            var searchResultDTO = new SearchResultDTO();

            // postcodes
            foreach (var postcode in postcodes)
            {
                searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO
                {
                    DisplayText = postcode.PostcodeUnit,
                    Type = SearchBusinessEntityType.Postcode
                });
            }

            // deliveryRoutes
            foreach (var deliveryRoute in deliveryRoutes)
            {
                searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO
                {
                    DisplayText = deliveryRoute.RouteName,
                    Type = SearchBusinessEntityType.Route
                });
            }

            // streetNames
            foreach (var streetName in streetNames)
            {
                searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO
                {
                    DisplayText = Regex.Replace(
                        string.Format(
                    StreetNameFormat,
                    streetName.NationalRoadCode,
                    streetName.DesignatedName),
                        ",+",
                        ", ").Trim(','),
                    Type = SearchBusinessEntityType.StreetNetwork
                });
            }

            // deliveryPoints
            foreach (var deliveryPoint in deliveryPoints)
            {
                searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO
                {
                    DisplayText = Regex.Replace(
                        string.Format(
                    DeliveryPointFormat,
                    deliveryPoint.PostalAddress.OrganisationName,
                    deliveryPoint.PostalAddress.BuildingName,
                    deliveryPoint.PostalAddress.SubBuildingName,
                    deliveryPoint.PostalAddress.BuildingNumber,
                    deliveryPoint.PostalAddress.Thoroughfare,
                    deliveryPoint.PostalAddress.DependentLocality),
                        ",+",
                        ", ").Trim(','),
                    UDPRN = deliveryPoint.PostalAddress.UDPRN,
                    Type = SearchBusinessEntityType.DeliveryPoint,
                    DeliveryPointGuid = deliveryPoint.ID
                });
            }

            // Total Counts
            searchResultDTO.SearchCounts.Add(new SearchCountDTO
            {
                Count = deliveryRouteCount + postCodeCount + deliveryPointsCount + streetNetworkCount,
                Type = SearchBusinessEntityType.All
            });

            return searchResultDTO;
        }

        /// <summary>
        /// Get current users Unit by passing unit ID
        /// </summary>
        /// <param name="userUnit"></param>
        /// <returns></returns>
        private string GetUnitNameByUnitId(Guid userUnit)
        {
            string unitName = string.Empty;
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetUnitNameByUnitId"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<string> categoryNames = new List<string> { ReferenceDataCategoryNames.UnitLocationType };

                var referenceDataCategoryList = searchIntegrationService.GetReferenceDataSimpleLists(categoryNames).Result;

                var locationtypeId = searchIntegrationService.GetUnitLocationTypeId(userUnit).Result;

                if (referenceDataCategoryList != null && referenceDataCategoryList.Count > 0)
                {
                    var referenceData = referenceDataCategoryList.SingleOrDefault().ReferenceDatas;
                    unitName = referenceData.Where(n => n.ID == locationtypeId).SingleOrDefault().ReferenceDataValue;
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return unitName;
            }
        }
    }
}