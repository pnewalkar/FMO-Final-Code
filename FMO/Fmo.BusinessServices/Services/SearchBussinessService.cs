using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Enums;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    public class SearchBussinessService : ISearchBusinessService
    {
        private IDeliveryRouteRepository deliveryRouteRepository = default(IDeliveryRouteRepository);
        private IPostCodeRepository postCodeRepository = default(IPostCodeRepository);
        private IStreetNetworkRepository streetNetworkRepository = default(IStreetNetworkRepository);
        private IDeliveryPointsRepository deliveryPointRepository = default(IDeliveryPointsRepository);

        public SearchBussinessService(IDeliveryRouteRepository deliveryRouteRepository, IPostCodeRepository postCodeRepository, IStreetNetworkRepository streetNetworkRepository, IDeliveryPointsRepository deliveryPointRepository)
        {
            this.deliveryRouteRepository = deliveryRouteRepository;
            this.postCodeRepository = postCodeRepository;
            this.streetNetworkRepository = streetNetworkRepository;
            this.deliveryPointRepository = deliveryPointRepository;
        }

        /// <summary>
        /// Fetch results from entities using basic search
        /// </summary>
        /// <param name="searchText">The text to be searched from the entities.</param>
        /// <returns>The result set after filtering the values.</returns>
        public async Task<SearchResultDTO> FetchBasicSearchDetails(string searchText)
        {
            try
            {
                var deliveryRoutes = await deliveryRouteRepository.FetchDeliveryRouteForBasicSearch(searchText).ConfigureAwait(false);
                var deliveryRouteCount = await deliveryRouteRepository.GetDeliveryRouteCount(searchText).ConfigureAwait(false);
                var postcodes = await postCodeRepository.FetchPostCodeUnitForBasicSearch(searchText).ConfigureAwait(false);
                var postCodeCount = await postCodeRepository.GetPostCodeUnitCount(searchText).ConfigureAwait(false);
                var deliveryPoints = await deliveryPointRepository.FetchDeliveryPointsForBasicSearch(searchText).ConfigureAwait(false);
                var deliveryPointsCount = await deliveryPointRepository.GetDeliveryPointsCount(searchText).ConfigureAwait(false);
                var streetNames = await streetNetworkRepository.FetchStreetNamesForBasicSearch(searchText).ConfigureAwait(false);
                var streetNetworkCount = await streetNetworkRepository.GetStreetNameCount(searchText).ConfigureAwait(false);

                var searchResultDTO = new SearchResultDTO();
                searchResultDTO = GetBasicSearchResults(deliveryRoutes, deliveryRouteCount, postcodes, postCodeCount, deliveryPoints, deliveryPointsCount, streetNames, streetNetworkCount);

                return searchResultDTO;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<SearchResultDTO> FetchAdvanceSearchDetails(string searchText)
        {
            var postcodesTask = postCodeRepository.FetchPostCodeUnitForAdvanceSearch(searchText);
            var deliveryRoutesTask = deliveryRouteRepository.FetchDeliveryRouteForAdvanceSearch(searchText);
            var streetNamesTask = streetNetworkRepository.FetchStreetNamesForAdvanceSearch(searchText);
            var deliveryPointsTask = deliveryPointRepository.FetchDeliveryPointsForAdvanceSearch(searchText);

            Task.WaitAll(deliveryRoutesTask, postcodesTask, streetNamesTask, deliveryPointsTask);

            var postcodes = await postcodesTask ?? new List<PostCodeDTO>();
            var deliveryRoutes = await deliveryRoutesTask ?? new List<DeliveryRouteDTO>();
            var streetNames = await streetNamesTask ?? new List<StreetNameDTO>();
            var deliveryPoints = await deliveryPointsTask ?? new List<DeliveryPointDTO>();

            var searchResultDTO = new SearchResultDTO();

            foreach (var postcode in postcodes)
            {
                searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO { DisplayText = postcode.PostcodeUnit, Type = SearchBusinessEntityType.Postcode });
            }

            searchResultDTO.SearchCounts.Add(new SearchCountDTO { Count = postcodes.Count, Type = SearchBusinessEntityType.Postcode });

            foreach (var streetName in streetNames)
            {
               searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO { DisplayText = streetName.LocalName, Type = SearchBusinessEntityType.StreetNetwork });
            }

            searchResultDTO.SearchCounts.Add(new SearchCountDTO { Count = streetNames.Count, Type = SearchBusinessEntityType.StreetNetwork });

            foreach (var deliveryPoint in deliveryPoints)
            {
                searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO
                {
                    DisplayText = string.Format(
                       "{0},{1},{3},{4},{5}",
                       deliveryPoint.PostalAddress.OrganisationName,
                       deliveryPoint.PostalAddress.BuildingName,
                       deliveryPoint.PostalAddress.SubBuildingName,
                       deliveryPoint.PostalAddress.BuildingNumber,
                       deliveryPoint.PostalAddress.Thoroughfare,
                       deliveryPoint.PostalAddress.DependentLocality),
                    UDPRN = deliveryPoint.PostalAddress.UDPRN,
                    Type = SearchBusinessEntityType.DeliveryPoint
                });

              //  searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO { DisplayText = deliveryPoint.UDPRN.ToString(), Type = SearchBusinessEntityType.DeliveryPoint });
            }

            searchResultDTO.SearchCounts.Add(new SearchCountDTO { Count = deliveryPoints.Count, Type = SearchBusinessEntityType.DeliveryPoint });

            foreach (var deliveryRoute in deliveryRoutes)
            {
                searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO { DisplayText = deliveryRoute.RouteName, Type = SearchBusinessEntityType.Route });
            }

            searchResultDTO.SearchCounts.Add(new SearchCountDTO { Count = deliveryRoutes.Count, Type = SearchBusinessEntityType.Route });

            int totalCount = searchResultDTO.SearchCounts.Sum(x => x.Count);

            searchResultDTO.SearchCounts.Add(new SearchCountDTO { Count = totalCount, Type = SearchBusinessEntityType.All });

            return searchResultDTO;
        }

        /// <summary>
        /// Get the result populated in the DTO
        /// </summary>
        /// <param name="deliveryRoutes">The list of delivery routes</param>
        /// <param name="deliveryRouteCount">The count of delivery reoutes</param>
        /// <param name="postcodes">The list of postcodes</param>
        /// <param name="postCodeCount">The count of postcodes</param>
        /// <param name="deliveryPoints">The list of delivery points</param>
        /// <param name="deliveryPointsCount">The count of delivery points</param>
        /// <param name="streetNames">The list of street names</param>
        /// <param name="streetNetworkCount">The count of street names</param>
        /// <returns>The result set</returns>
        private SearchResultDTO GetBasicSearchResults(List<DeliveryRouteDTO> deliveryRoutes, int deliveryRouteCount, List<PostCodeDTO> postcodes, int postCodeCount, List<DeliveryPointDTO> deliveryPoints, int deliveryPointsCount, List<StreetNameDTO> streetNames, int streetNetworkCount)
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
                    DisplayText = string.Format(
                    "{0},{1}",
                    streetName.NationalRoadCode,
                    streetName.DesignatedName),
                    Type = SearchBusinessEntityType.StreetNetwork
                });
            }

            // deliveryPoints
            foreach (var deliveryPoint in deliveryPoints)
            {
                searchResultDTO.SearchResultItems.Add(new SearchResultItemDTO
                {
                    DisplayText = string.Format(
                    "{0},{1},{3},{4},{5}",
                    deliveryPoint.PostalAddress.OrganisationName,
                    deliveryPoint.PostalAddress.BuildingName,
                    deliveryPoint.PostalAddress.SubBuildingName,
                    deliveryPoint.PostalAddress.BuildingNumber,
                    deliveryPoint.PostalAddress.Thoroughfare,
                    deliveryPoint.PostalAddress.DependentLocality),
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