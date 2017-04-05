using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    public class DeliveryRouteRepository : RepositoryBase<DeliveryRoute, FMODBContext>, IDeliveryRouteRepository
    {
        public DeliveryRouteRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<DeliveryRouteDTO> FetchDeliveryRoute(int operationStateID, int deliveryScenarioID)
        {
            IEnumerable<DeliveryRoute> result = DataContext.DeliveryRoutes.Where(x => x.DeliveryScenario_Id == deliveryScenarioID && x.OperationalStatus_Id == operationStateID).ToList();
            return GenericMapper.MapList<DeliveryRoute, DeliveryRouteDTO>(result.ToList());
        }

        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRoute(string searchText)
        {
            try
            {
                var result = await DataContext.DeliveryRoutes.ToListAsync();
                return GenericMapper.MapList<DeliveryRoute, DeliveryRouteDTO>(result.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteforBasicSearch(string searchText)
        {
            try
            {
                int takeCount = 5;
                var deliveryRoutesDto = await DataContext.DeliveryRoutes.Where(l => l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText))
                    .Take(takeCount)
                    .Select(l => new DeliveryRouteDTO
                    {
                        DeliveryRoute_Id = l.DeliveryRoute_Id,
                        RouteName = l.RouteName,
                        RouteNumber = l.RouteNumber
                    })
                    .ToListAsync();

                return deliveryRoutesDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetDeliveryRouteCount(string searchText)
        {
            try
            {
                return await DataContext.DeliveryRoutes.Where(l => l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)).CountAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}