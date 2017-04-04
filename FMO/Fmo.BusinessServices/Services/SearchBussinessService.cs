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
        private IStreetNetworkRepository streetNetworkRepository = default(IStreetNetworkRepository);

        public SearchBussinessService(IDeliveryPointsRepository deliveryPointsRepository, IDeliveryRouteRepository deliveryRouteRepository, IPostCodeRepository postCodeRepository, IStreetNetworkRepository streetNetworkRepository)
        {
            this.deliveryRouteRepository = deliveryRouteRepository;
            this.postCodeRepository = postCodeRepository;
            this.streetNetworkRepository = streetNetworkRepository;
        }

        public async Task<SearchResultDTO> FetchBasicSearchDetails(string searchText)
        {
            int dataCount = 0;
            SearchResultDTO searchResultDto = new SearchResultDTO();
            searchResultDto.DeliveryRoute = await deliveryRouteRepository.FetchDeliveryRoute(searchText);
            dataCount = await deliveryRouteRepository.GetDeliveryRouteUnitCount(searchText);

            searchResultDto.PostCode = await postCodeRepository.FetchPostCodeUnitforBasicSearch(searchText);
            dataCount += await postCodeRepository.GetPostCodeUnitCount(searchText);

            await streetNetworkRepository.FetchStreetNetwork(searchText);
            return null;
        }

        public async Task<SearchResultDTO> FetchAdvanceSearchDetails(string searchText)
        {
            await postCodeRepository.FetchPostCodeUnit(searchText);
            await streetNetworkRepository.FetchStreetNetwork(searchText);
            await deliveryRouteRepository.FetchDeliveryRoute(searchText);
            return null;
        }
    }
}