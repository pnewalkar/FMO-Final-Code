using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Fmo.Common.Constants;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    /// <summary>
    /// This class contains methods for fetching Delivery route data for basic and advance search
    /// </summary>
    public class DeliveryRouteRepository : RepositoryBase<DeliveryRoute, FMODBContext>, IDeliveryRouteRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public DeliveryRouteRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Fetch the Delivery Route.
        /// </summary>
        /// <param name="operationStateID">Guid operationStateID</param>
        /// <param name="deliveryScenarioID">Guid deliveryScenarioID</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>
        /// List
        /// </returns>
        public List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID, Guid userUnit)
        {
            try
            {
                IEnumerable<DeliveryRoute> result = DataContext.DeliveryRoutes.AsNoTracking().Where(x => x.Scenario.Unit_GUID == userUnit && x.DeliveryScenario_GUID == deliveryScenarioID && x.Scenario.OperationalState_GUID == operationStateID).ToList();
                return GenericMapper.MapList<DeliveryRoute, DeliveryRouteDTO>(result.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch Delivery Route for Advance Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>Task</returns>
        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText, Guid userUnit)
        {
            try
            {
                var deliveryRoutes = await DataContext.DeliveryRoutes.AsNoTracking()
                                 .Where(l => (l.Scenario.Unit_GUID == userUnit) && (l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)))
                                 .Select(l => new DeliveryRouteDTO
                                 {
                                     ID = l.ID,
                                     RouteName = l.RouteName,
                                     RouteNumber = l.RouteNumber
                                 }).ToListAsync();

                return deliveryRoutes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch Delivery route for Basic Search
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The result set of delivery route.</returns>
        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText, Guid userUnit)
        {
            try
            {
                int takeCount = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SearchResultCount]);
                searchText = searchText ?? string.Empty;
                var deliveryRoutesDto = await DataContext.DeliveryRoutes.AsNoTracking()
                    .Where(l => (l.Scenario.Unit_GUID == userUnit) && (l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)))
                    .Take(takeCount)
                    .Select(l => new DeliveryRouteDTO
                    {
                        ID = l.ID,
                        RouteName = l.RouteName,
                        RouteNumber = l.RouteNumber
                    })
                    .ToListAsync();

                return deliveryRoutesDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the count of delivery route
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of delivery route</returns>
        public async Task<int> GetDeliveryRouteCount(string searchText, Guid userUnit)
        {
            try
            {
                searchText = searchText ?? string.Empty;
                return await DataContext.DeliveryRoutes.AsNoTracking()
                    .Where(l => (l.Scenario.Unit_GUID == userUnit) && (l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)))
                    .CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region GeneratePDF

        #region Public Methods

        public List<DeliveryRouteDTO> FetchDeliveryRouteDetailsforPDF(Guid deliveryRouteId, Guid operationalObjectTypeForDP, Guid operationalObjectTypeForDPCommercial, Guid operationalObjectTypeForDPResidential)
        {
            // TODO: get the operationalObjectTypeForDP from CategoryName name as 'Operational Object Type'(table : ReferenceDataCategory) and DisplayText as 'DP' (table; ReferenceData)
            // TODO: get the operationalObjectTypeForDP Commercial/Residential from CategoryName as 'DeliveryPoint Use Indicator' and DisplayText as 'Commercial'/'Residential'
            try
            {
                var deliveryRoutesDto = (from dr in DataContext.DeliveryRoutes.AsNoTracking()
                                         join rd in DataContext.ReferenceDatas.AsNoTracking() on dr.RouteMethodType_GUID equals rd.ID
                                         join sr in DataContext.Scenarios.AsNoTracking() on dr.DeliveryScenario_GUID equals sr.ID
                                         where dr.ID == deliveryRouteId
                                         select new DeliveryRouteDTO
                                         {
                                             RouteName = dr.RouteName,
                                             RouteNumber = dr.RouteNumber,
                                             ScenarioName = sr.ScenarioName,
                                             Method = rd.DisplayText,
                                             Aliases = GetAliases(deliveryRouteId),
                                             Blocks = GetNumberOfBlocks(deliveryRouteId),
                                             PairedRoute = GetPairedRoute(deliveryRouteId),
                                             DPs = GetNumberOfDPs(deliveryRouteId, operationalObjectTypeForDP),
                                             BusinessDPs = GetNumberOfCommercialResidentialDPs(deliveryRouteId, operationalObjectTypeForDPCommercial),
                                             ResidentialDPs = GetNumberOfCommercialResidentialDPs(deliveryRouteId, operationalObjectTypeForDPResidential),
                                             AccelarationIn = "AccelarationIn",
                                             AccelarationOut = "AccelarationOut",
                                             Totaltime = "h:mm"
                                         })
                                         .ToList();
                return deliveryRoutesDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private int GetNumberOfBlocks(Guid deliveryRouteId)
        {
            try
            {
                int numberOfBlocks = (from drb in DataContext.DeliveryRouteBlocks.AsNoTracking()
                                      join dr in DataContext.DeliveryRoutes.AsNoTracking() on drb.DeliveryRoute_GUID equals dr.ID
                                      where dr.ID == deliveryRouteId
                                      select drb.Block_GUID).Distinct().Count();
                return numberOfBlocks;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int GetNumberOfDPs(Guid deliveryRouteId, Guid operationalObjectTypeForDP)
        {
            try
            {
                int numberOfDPs = (from blkseq in DataContext.BlockSequences.AsNoTracking()
                                   join drb in DataContext.DeliveryRouteBlocks.AsNoTracking() on blkseq.Block_GUID equals drb.Block_GUID
                                   join dr in DataContext.DeliveryRoutes.AsNoTracking() on drb.DeliveryRoute_GUID equals dr.ID
                                   where blkseq.OperationalObjectType_GUID == operationalObjectTypeForDP && dr.ID == deliveryRouteId
                                   select blkseq.OperationalObject_GUID).Count();
                return numberOfDPs;
            }
            catch
            {
                return 0;
            }
        }

        private int GetNumberOfCommercialResidentialDPs(Guid deliveryRouteId, Guid operationalObjectTypeForDP)
        {
            try
            {
                int numberOfCommercialResidentialDPs = (from del in DataContext.DeliveryPoints.AsNoTracking()
                                                        join blkseq in DataContext.BlockSequences.AsNoTracking() on del.ID equals blkseq.OperationalObject_GUID
                                                        join dr in DataContext.DeliveryRoutes.AsNoTracking() on deliveryRouteId equals dr.ID
                                                        join drb in DataContext.DeliveryRouteBlocks.AsNoTracking() on dr.ID equals drb.DeliveryRoute_GUID
                                                        where del.DeliveryPointUseIndicator_GUID == operationalObjectTypeForDP && drb.DeliveryRoute_GUID == deliveryRouteId
                                                        select del.DeliveryPointUseIndicator_GUID).Count();
                return numberOfCommercialResidentialDPs;
            }
            catch
            {
                return 0;
            }
        }

        private string GetPairedRoute(Guid deliveryRouteId)
        {
            string pairedRoute = string.Empty;
            return pairedRoute;
        }

        private int GetAliases(Guid deliveryRouteId)
        {
            int numberOfAliases = 0;
            return numberOfAliases;
        }

        #endregion Private Methods

        #endregion GeneratePDF
    }
}