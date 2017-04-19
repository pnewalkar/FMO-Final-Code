﻿using System;
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
using System.Configuration;

namespace Fmo.DataServices.Repositories
{
    public class DeliveryRouteRepository : RepositoryBase<DeliveryRoute, FMODBContext>, IDeliveryRouteRepository
    {
        public DeliveryRouteRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Fetch the Delivery Route.
        /// </summary>
        /// <param name="operationStateID">Guid operationStateID</param>
        /// <param name="deliveryScenarioID">Guid deliveryScenarioID</param>
        /// <returns>List</returns>
        public List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID)
        {
            try
            {
                IEnumerable<DeliveryRoute> result = DataContext.DeliveryRoutes.Where(x => x.DeliveryScenario_GUID == deliveryScenarioID && x.Scenario.OperationalState_GUID == operationStateID).ToList();
                return GenericMapper.MapList<DeliveryRoute, DeliveryRouteDTO>(result.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText)
        {
            try
            {
                var deliveryRoutes = await DataContext.DeliveryRoutes.Where(l => l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)).Take(10).ToListAsync();
             //   var result = await DataContext.DeliveryRoutes.Take(10).ToListAsync();
                return GenericMapper.MapList<DeliveryRoute, DeliveryRouteDTO>(deliveryRoutes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Fetch Delivery route for Basic Search
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <returns>The result set of delivery route.</returns>
        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText)
        {
            try
            {
                int takeCount = Convert.ToInt32(ConfigurationManager.AppSettings["SearchResultCount"]);
                searchText = searchText ?? string.Empty;
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
                throw ex;
            }
        }

        /// <summary>
        /// Get the count of delivery route
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <returns>The total count of delivery route</returns>
        public async Task<int> GetDeliveryRouteCount(string searchText)
        {
            try
            {
                searchText = searchText ?? string.Empty;
                return await DataContext.DeliveryRoutes.Where(l => l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)).CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}