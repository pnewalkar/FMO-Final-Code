using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Constants;
using Fmo.Common.Enums;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    /// <summary>
    /// This class contains methods for basic and advance search
    /// </summary>
    public class SearchBusinessService : ISearchBusinessService
    {
         private readonly IDeliveryRouteBusinessService deliveryRouteBusinessService = default(IDeliveryRouteBusinessService);
         private readonly IPostCodeBusinessService postcodeBusinessService = default(IPostCodeBusinessService);
        private readonly IStreetNetworkBusinessService streetNetworkBusinessService = default(IStreetNetworkBusinessService);
        private readonly IDeliveryPointBusinessService deliveryPointBusinessService = default(IDeliveryPointBusinessService);

        public SearchBusinessService(IDeliveryRouteBusinessService deliveryRouteBusinessService, IPostCodeBusinessService postcodeBusinessService, IStreetNetworkBusinessService streetNetworkBusinessService, IDeliveryPointBusinessService deliveryPointBusinessService)
        {
            this.deliveryRouteBusinessService = deliveryRouteBusinessService;
            this.postcodeBusinessService = postcodeBusinessService;
            this.streetNetworkBusinessService = streetNetworkBusinessService;
            this.deliveryPointBusinessService = deliveryPointBusinessService;
        }

        /// <summary>
        /// Fetch results from entities using basic search
        /// </summary>
        /// <param name="searchText">The text to be searched from the entities.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>
        /// The result set after filtering the values.
        /// </returns>
        public async Task<SearchResultDTO> FetchBasicSearchDetails(string searchText, Guid userUnit)
        {
            var deliveryRoutes =
                await deliveryRouteBusinessService.FetchDeliveryRouteForBasicSearch(searchText, userUnit);
            var deliveryRouteCount = await deliveryRouteBusinessService.GetDeliveryRouteCount(searchText, userUnit);
            var postcodes = await postcodeBusinessService.FetchPostCodeUnitForBasicSearch(searchText, userUnit);
            var postCodeCount = await postcodeBusinessService.GetPostCodeUnitCount(searchText, userUnit);
            var deliveryPoints =
                await deliveryPointBusinessService.FetchDeliveryPointsForBasicSearch(searchText, userUnit);
            var deliveryPointsCount = await deliveryPointBusinessService.GetDeliveryPointsCount(searchText, userUnit);
            var streetNames = await streetNetworkBusinessService.FetchStreetNamesForBasicSearch(searchText, userUnit);
            var streetNetworkCount = await streetNetworkBusinessService.GetStreetNameCount(searchText, userUnit);

            var searchResultDTO = MapSearchResults(deliveryRoutes, deliveryRouteCount, postcodes, postCodeCount,
                deliveryPoints, deliveryPointsCount, streetNames, streetNetworkCount);
            return searchResultDTO;
        }

        /// <summary>
        /// Fetch results from entities using advanced search
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>
        /// search Result Dto
        /// </returns>
        public async Task<SearchResultDTO> FetchAdvanceSearchDetails(string searchText, Guid userUnit)
        {
            var postcodesTask = postcodeBusinessService.FetchPostCodeUnitForAdvanceSearch(searchText, userUnit);
            var deliveryRoutesTask = deliveryRouteBusinessService.FetchDeliveryRouteForAdvanceSearch(searchText, userUnit);
            var streetNamesTask = streetNetworkBusinessService.FetchStreetNamesForAdvanceSearch(searchText, userUnit);
            var deliveryPointsTask = deliveryPointBusinessService.FetchDeliveryPointsForAdvanceSearch(searchText, userUnit);

            Task.WaitAll(deliveryRoutesTask, postcodesTask, streetNamesTask, deliveryPointsTask);

            var postcodes = await postcodesTask ?? new List<PostCodeDTO>();
            var deliveryRoutes = await deliveryRoutesTask ?? new List<DeliveryRouteDTO>();
            var streetNames = await streetNamesTask ?? new List<StreetNameDTO>();
            var deliveryPoints = await deliveryPointsTask ?? new List<DeliveryPointDTO>();

            var searchResultDTO = MapSearchResults(deliveryRoutes, deliveryRoutes.Count, postcodes, postcodes.Count, deliveryPoints, deliveryPoints.Count, streetNames, streetNames.Count);

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
        private static SearchResultDTO MapSearchResults(List<DeliveryRouteDTO> deliveryRoutes, int deliveryRouteCount, List<PostCodeDTO> postcodes, int postCodeCount, List<DeliveryPointDTO> deliveryPoints, int deliveryPointsCount, List<StreetNameDTO> streetNames, int streetNetworkCount)
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
                    Constants.StreetNameFormat,
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
                    Constants.DeliveryPointFormat,
                    deliveryPoint.PostalAddress.OrganisationName,
                    deliveryPoint.PostalAddress.BuildingName,
                    deliveryPoint.PostalAddress.SubBuildingName,
                    deliveryPoint.PostalAddress.BuildingNumber,
                    deliveryPoint.PostalAddress.Thoroughfare,
                    deliveryPoint.PostalAddress.DependentLocality),
                        ",+",
                        ", ").Trim(','),
                    UDPRN = deliveryPoint.PostalAddress.UDPRN,
                    Type = SearchBusinessEntityType.DeliveryPoint
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
    }
}