using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    public class SearchBussinessService : ISearchBussinessService
    {
        private IDeliveryRouteRepository deliveryRouteRepository = default(IDeliveryRouteRepository);
        private IPostCodeRepository postCodeRepository = default(IPostCodeRepository);
        private IPostalAddressRepository postalAddressRepository = default(IPostalAddressRepository);
        private IStreetNetworkRepository streetNetworkRepository = default(IStreetNetworkRepository);

        public SearchBussinessService(IDeliveryRouteRepository deliveryRouteRepository, IPostCodeRepository postCodeRepository, IPostalAddressRepository postalAddressRepository, IStreetNetworkRepository streetNetworkRepository)
        {
            this.deliveryRouteRepository = deliveryRouteRepository;
            this.postCodeRepository = postCodeRepository;
            this.streetNetworkRepository = streetNetworkRepository;
            this.postalAddressRepository = postalAddressRepository;
        }

        public async Task<SearchResultDTO> FetchBasicSearchDetails(string searchText)
        {
            try
            {
                var deliveryRoutes = await deliveryRouteRepository.FetchDeliveryRouteforBasicSearch(searchText).ConfigureAwait(false);
                var deliveryRouteCount = await deliveryRouteRepository.GetDeliveryRouteCount(searchText).ConfigureAwait(false);
                var postcodes = await postCodeRepository.FetchPostCodeUnitforBasicSearch(searchText).ConfigureAwait(false);
                var postCodeCount = await postCodeRepository.GetPostCodeUnitCount(searchText).ConfigureAwait(false);
                var postalAddress = await postalAddressRepository.FetchPostalAddressforBasicSearch(searchText).ConfigureAwait(false);
                var postalAddressCount = await postalAddressRepository.GetPostalAddressCount(searchText).ConfigureAwait(false);
                var streetNames = await streetNetworkRepository.FetchStreetNamesforBasicSearch(searchText).ConfigureAwait(false);
                var streetNetworkCount = await streetNetworkRepository.GetStreetNameCount(searchText).ConfigureAwait(false);

                return new SearchResultDTO
                {
                    PostCode = postcodes,
                    PostalAddress = postalAddress,
                    StreetName = streetNames,
                    TotalCount = deliveryRouteCount + postCodeCount + postalAddressCount + streetNetworkCount
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<SearchResultDTO> FetchAdvanceSearchDetails(string searchText)
        {
            await postCodeRepository.FetchPostCodeUnit(searchText);
            await deliveryRouteRepository.FetchDeliveryRoute(searchText);
            return null;
        }
    }
}