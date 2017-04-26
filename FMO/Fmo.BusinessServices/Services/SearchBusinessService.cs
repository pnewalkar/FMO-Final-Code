﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Constants;
using Fmo.Common.Enums;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    /// <summary>
    /// This class contains methods for basic and advance search
    /// </summary>
    public class SearchBusinessService : ISearchBusinessService
    {
        private readonly IDeliveryRouteRepository deliveryRouteRepository = default(IDeliveryRouteRepository);
        private readonly IPostCodeRepository postcodeRepository = default(IPostCodeRepository);
        private readonly IStreetNetworkRepository streetNetworkRepository = default(IStreetNetworkRepository);
        private readonly IDeliveryPointsRepository deliveryPointRepository = default(IDeliveryPointsRepository);

        public SearchBusinessService(IDeliveryRouteRepository deliveryRouteRepository, IPostCodeRepository postcodeRepository, IStreetNetworkRepository streetNetworkRepository, IDeliveryPointsRepository deliveryPointRepository)
        {
            this.deliveryRouteRepository = deliveryRouteRepository;
            this.postcodeRepository = postcodeRepository;
            this.streetNetworkRepository = streetNetworkRepository;
            this.deliveryPointRepository = deliveryPointRepository;
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
            try
            {
                var deliveryRoutes = await deliveryRouteRepository.FetchDeliveryRouteForBasicSearch(searchText, userUnit).ConfigureAwait(false);
                var deliveryRouteCount = await deliveryRouteRepository.GetDeliveryRouteCount(searchText, userUnit).ConfigureAwait(false);
                var postcodes = await postcodeRepository.FetchPostCodeUnitForBasicSearch(searchText, userUnit).ConfigureAwait(false);
                var postCodeCount = await postcodeRepository.GetPostCodeUnitCount(searchText, userUnit).ConfigureAwait(false);
                var deliveryPoints = await deliveryPointRepository.FetchDeliveryPointsForBasicSearch(searchText, userUnit).ConfigureAwait(false);
                var deliveryPointsCount = await deliveryPointRepository.GetDeliveryPointsCount(searchText, userUnit).ConfigureAwait(false);
                var streetNames = await streetNetworkRepository.FetchStreetNamesForBasicSearch(searchText, userUnit).ConfigureAwait(false);
                var streetNetworkCount = await streetNetworkRepository.GetStreetNameCount(searchText, userUnit).ConfigureAwait(false);

                var searchResultDTO = MapSearchResults(deliveryRoutes, deliveryRouteCount, postcodes, postCodeCount, deliveryPoints, deliveryPointsCount, streetNames, streetNetworkCount);

                return searchResultDTO;
            }
            catch
            {
                throw;
            }
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
            var postcodesTask = postcodeRepository.FetchPostCodeUnitForAdvanceSearch(searchText, userUnit);
            var deliveryRoutesTask = deliveryRouteRepository.FetchDeliveryRouteForAdvanceSearch(searchText, userUnit);
            var streetNamesTask = streetNetworkRepository.FetchStreetNamesForAdvanceSearch(searchText, userUnit);
            var deliveryPointsTask = deliveryPointRepository.FetchDeliveryPointsForAdvanceSearch(searchText, userUnit);

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