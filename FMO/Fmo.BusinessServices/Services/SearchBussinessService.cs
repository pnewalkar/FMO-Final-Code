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
            var deliveryRoutes = deliveryRouteRepository.FetchDeliveryRouteForAdvanceSearch(searchText);
            var deliveryRouteCount = deliveryRouteRepository.GetDeliveryRouteUnitCount(searchText);

            var postcodes = postCodeRepository.FetchPostCodeUnitforBasicSearch(searchText);
            var postCodeCount = postCodeRepository.GetPostCodeUnitCount(searchText);

            var postalAddress = postalAddressRepository.FetchPostalAddressforBasicSearch(searchText);
            var postalAddressCount = postalAddressRepository.GetPostalAddressCount(searchText);

            var streetNames = streetNetworkRepository.FetchStreetNamesforBasicSearch(searchText);
            var streetNetworkCount = streetNetworkRepository.GetStreetNameCount(searchText);

            await Task.WhenAll(deliveryRoutes, deliveryRouteCount, postcodes, postCodeCount, postalAddress, postalAddressCount, streetNames, streetNetworkCount);
            return new SearchResultDTO
            {
                DeliveryRoute = await deliveryRoutes,
                PostCode = await postcodes,
                PostalAddress = await postalAddress,
                StreetName = await streetNames,
                TotalCount = await deliveryRouteCount + await postCodeCount + await postalAddressCount + await streetNetworkCount
            };
        }

        public async Task<SearchResultDTO> FetchAdvanceSearchDetails(string searchText)
        {
            //var postcodesTask =  postCodeRepository.FetchPostCodeUnitForAdvanceSearch(searchText);
            //var deliveryRoutesTask =  deliveryRouteRepository.FetchDeliveryRouteForAdvanceSearch(searchText);
            //var streetNamesTask =  streetNetworkRepository.FetchStreetNamesforAdvanceSearch(searchText);
            //var postalAddressTask =  postalAddressRepository.FetchPostalAddressforAdvanceSearch(searchText);

            //Task.WaitAll(deliveryRoutesTask, postcodesTask, streetNamesTask, postalAddressTask);
            //var postcodes = await postcodesTask ?? new List<PostCodeDTO>();
            //var deliveryRoutes = await deliveryRoutesTask ?? new List<DeliveryRouteDTO>();
            //var streetNames = await streetNamesTask ?? new List<StreetNameDTO>();
            //var postalAddresses = await postalAddressTask ?? new List<PostalAddressDTO>();

            var postcodes = await postCodeRepository.FetchPostCodeUnitForAdvanceSearch(searchText);
            var deliveryRoutes = await deliveryRouteRepository.FetchDeliveryRouteForAdvanceSearch(searchText);
            var streetNames = await streetNetworkRepository.FetchStreetNamesforAdvanceSearch(searchText);
            var postalAddresses = await postalAddressRepository.FetchPostalAddressforAdvanceSearch(searchText);

            return new SearchResultDTO
            {
                DeliveryRoute = deliveryRoutes,
                PostCode = postcodes,
                PostalAddress = postalAddresses,
                StreetName = streetNames,
                TotalCount = deliveryRoutes.Count + postcodes.Count + streetNames.Count + postalAddresses.Count
            };
        }
    }
}