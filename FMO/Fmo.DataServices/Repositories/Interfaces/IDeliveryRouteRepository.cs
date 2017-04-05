﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryRouteRepository
    {
        List<DeliveryRouteDTO> FetchDeliveryRoute(int operationStateID, int deliveryScenarioID);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRoute(string searchText);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteforBasicSearch(string searchText);

        Task<int> GetDeliveryRouteCount(string searchText);
    }
}