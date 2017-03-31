using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    public class SearchBussinessService : ISearchBussinessService
    {
        private ISearchRepository searchRepository = default(ISearchRepository);
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);
        private IDeliveryRouteRepository deliveryRouteRepository = default(IDeliveryRouteRepository);
        private IPostCodeRepository postCodeRepository = default(IPostCodeRepository);
        private IStreetNetworkRepository streetNetworkRepository = default(IStreetNetworkRepository);

        public SearchBussinessService(ISearchRepository searchRepository, IDeliveryPointsRepository deliveryPointsRepository, IDeliveryRouteRepository deliveryRouteRepository, IPostCodeRepository postCodeRepository, IStreetNetworkRepository streetNetworkRepository)
        {
            this.searchRepository = searchRepository;
            this.deliveryPointsRepository = deliveryPointsRepository;
            this.deliveryRouteRepository = deliveryRouteRepository;
            this.postCodeRepository = postCodeRepository;
            this.streetNetworkRepository = streetNetworkRepository;
        }

        public AdvanceSearchDTO FetchAdvanceSearchDetails(string searchText)
        {
            deliveryPointsRepository.SearchDeliveryPoints();
            postCodeRepository.FetchPostCodeUnit(searchText);
            streetNetworkRepository.FetchStreetNetwork(searchText);
            deliveryRouteRepository.FetchDeliveryRoute(searchText);
            return searchRepository.FetchAdvanceSearchDetails(searchText);
        }
    }
}