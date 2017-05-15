﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fmo.Common.Constants;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.DTO.Model;
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
            IEnumerable<DeliveryRoute> result = DataContext.DeliveryRoutes.AsNoTracking()
                .Where(x => x.Scenario.Unit_GUID == userUnit && x.DeliveryScenario_GUID == deliveryScenarioID &&
                            x.Scenario.OperationalState_GUID == operationStateID).ToList();
            return GenericMapper.MapList<DeliveryRoute, DeliveryRouteDTO>(result.ToList());
        }

        /// <summary>
        /// Fetch Delivery Route for Advance Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>Task</returns>
        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText, Guid userUnit)
        {
            var deliveryRoutes = await DataContext.DeliveryRoutes.AsNoTracking()
                .Where(l => (l.Scenario.Unit_GUID == userUnit) &&
                            (l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)))
                .Select(l => new DeliveryRouteDTO
                {
                    ID = l.ID,
                    RouteName = l.RouteName,
                    RouteNumber = l.RouteNumber
                }).ToListAsync();

            return deliveryRoutes;
        }

        /// <summary>
        /// Fetch Delivery route for Basic Search
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The result set of delivery route.</returns>
        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText, Guid userUnit)
        {
            int takeCount = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SearchResultCount]);
            searchText = searchText ?? string.Empty;
            var deliveryRoutesDto = await DataContext.DeliveryRoutes.AsNoTracking()
                .Where(l => (l.Scenario.Unit_GUID == userUnit) &&
                            (l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)))
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

        /// <summary>
        /// Get the count of delivery route
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of delivery route</returns>
        public async Task<int> GetDeliveryRouteCount(string searchText, Guid userUnit)
        {
            searchText = searchText ?? string.Empty;
            return await DataContext.DeliveryRoutes.AsNoTracking()
                .Where(l => (l.Scenario.Unit_GUID == userUnit) &&
                            (l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)))
                .CountAsync();
        }

        #region GeneratePDF

        #region Public Methods

        /// <summary>
        /// Gets the delivery route detailsfor PDF generation.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="referenceDataCategoryDtoList">The reference data category dto list.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>DeliveryRouteDTO</returns>
        public async Task<DeliveryRouteDTO> GetDeliveryRouteDetailsforPdfGeneration(Guid deliveryRouteId, List<ReferenceDataCategoryDTO> referenceDataCategoryDtoList, Guid userUnit)
        {
            // No of DPs
            Guid operationalObjectTypeForDp = referenceDataCategoryDtoList
                .Where(x => x.CategoryName == ReferenceDataCategoryNames.OperationalObjectType)
                .SelectMany(x => x.ReferenceDatas)
                .Where(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).Select(x => x.ID)
                .SingleOrDefault();

            // No. Organisation DP
            Guid operationalObjectTypeForDpOrganisation = referenceDataCategoryDtoList
                .Where(x => x.CategoryName == ReferenceDataCategoryNames.DeliveryPointUseIndicator)
                .SelectMany(x => x.ReferenceDatas)
                .Where(x => x.ReferenceDataValue == ReferenceDataValues.Organisation).Select(x => x.ID)
                .SingleOrDefault();

            // No. Residential DP
            Guid operationalObjectTypeForDpResidential = referenceDataCategoryDtoList
                .Where(x => x.CategoryName == ReferenceDataCategoryNames.DeliveryPointUseIndicator)
                .SelectMany(x => x.ReferenceDatas)
                .Where(x => x.ReferenceDataValue == ReferenceDataValues.Residential).Select(x => x.ID)
                .SingleOrDefault();

            // Delivery Route Method Type
            var referenceDataDeliveryMethodTypes = referenceDataCategoryDtoList
                .Where(x => x.CategoryName == ReferenceDataCategoryNames.DeliveryRouteMethodType)
                .SelectMany(x => x.ReferenceDatas).ToList();

            Guid sharedDeliveryRouteMethodTypeGuid = referenceDataDeliveryMethodTypes
                .Where(x => x.ReferenceDataValue == ReferenceDataValues.DeliveryRouteMethodTypeRmVanShared)
                .Select(x => x.ID).SingleOrDefault();

            var deliveryRoutesDto = await (from dr in DataContext.DeliveryRoutes.AsNoTracking()
                                           join sr in DataContext.Scenarios.AsNoTracking() on dr.DeliveryScenario_GUID equals sr.ID
                                           where dr.ID == deliveryRouteId && sr.Unit_GUID == userUnit &&
                                                 dr.RouteMethodType_GUID == sharedDeliveryRouteMethodTypeGuid
                                           select new DeliveryRouteDTO
                                           {
                                               RouteName = dr.RouteName,
                                               RouteNumber = dr.RouteNumber,
                                               ScenarioName = sr.ScenarioName,
                                               MethodReferenceGuid = dr.RouteMethodType_GUID,
                                               Totaltime = dr.TravelOutTimeMin - dr.TravelOutTimeMin,
                                               TravelOutTransportType_GUID = dr.TravelOutTransportType_GUID,
                                               TravelInTransportType_GUID = dr.TravelInTransportType_GUID
                                           }).SingleOrDefaultAsync();

            var methodType = referenceDataDeliveryMethodTypes
                .SingleOrDefault(x => x.ID == deliveryRoutesDto.MethodReferenceGuid);
            if (methodType != null)
            {
                deliveryRoutesDto.Method = methodType.ReferenceDataValue;
            }

            var travelInType = referenceDataDeliveryMethodTypes
                .SingleOrDefault(x => x.ID == deliveryRoutesDto.TravelInTransportType_GUID);
            if (travelInType != null)
            {
                deliveryRoutesDto.AccelarationIn = travelInType.ReferenceDataValue;
            }

            var travelOutType = referenceDataDeliveryMethodTypes
                .SingleOrDefault(x => x.ID == deliveryRoutesDto.TravelOutTransportType_GUID);
            if (travelOutType != null)
            {
                deliveryRoutesDto.AccelarationOut = travelOutType.ReferenceDataValue;
            }

            Task<int> taskAliases = GetAliases(deliveryRouteId, operationalObjectTypeForDp, userUnit);
            Task<int> taskBlocks = GetNumberOfBlocks(deliveryRouteId, userUnit);
            Task<int> taskNumberOfDPs = GetNumberOfDPs(deliveryRouteId, operationalObjectTypeForDp, userUnit);
            Task<int> taskBusinessDPs =
                GetNumberOfCommercialResidentialDPs(deliveryRouteId, operationalObjectTypeForDpOrganisation, userUnit);
            Task<int> taskResidentialDPs =
                GetNumberOfCommercialResidentialDPs(deliveryRouteId, operationalObjectTypeForDpResidential, userUnit);
            Task<string> pairedRoutes = GetPairedRoutesByRouteID(deliveryRouteId, sharedDeliveryRouteMethodTypeGuid,
                operationalObjectTypeForDp, userUnit);

            Task.WaitAll(taskAliases, taskBlocks, taskNumberOfDPs, taskBusinessDPs, taskResidentialDPs, pairedRoutes);
            if (deliveryRoutesDto == null)
            {
                deliveryRoutesDto = new DeliveryRouteDTO();
            }

            deliveryRoutesDto.Aliases = taskAliases.Result;
            deliveryRoutesDto.Blocks = taskBlocks.Result;
            deliveryRoutesDto.DPs = taskNumberOfDPs.Result;
            deliveryRoutesDto.BusinessDPs = taskBusinessDPs.Result;
            deliveryRoutesDto.ResidentialDPs = taskResidentialDPs.Result;
            deliveryRoutesDto.PairedRoute = pairedRoutes.Result;

            return deliveryRoutesDto;
        }

        public async Task<RouteLogSummaryModelDTO> GenerateRouteLog(DeliveryRouteDTO deliveryRouteDto, Guid userUnit)
        {
            //TODO: add implementation for Route log story pdf generation.
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Gets the number of blocks.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>int</returns>
        private async Task<int> GetNumberOfBlocks(Guid deliveryRouteId, Guid userUnit)
        {
            int numberOfBlocks = await (from drb in DataContext.DeliveryRouteBlocks.AsNoTracking()
                                        join dr in DataContext.DeliveryRoutes.AsNoTracking() on drb.DeliveryRoute_GUID equals dr.ID
                                        join sr in DataContext.Scenarios.AsNoTracking() on dr.DeliveryScenario_GUID equals sr.ID
                                        where drb.DeliveryRoute_GUID == deliveryRouteId && sr.Unit_GUID == userUnit
                                        select drb.Block_GUID).CountAsync();
            return numberOfBlocks;
        }

        /// <summary>
        /// Gets the number of d ps.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="operationalObjectTypeForDp">The operational object type for dp.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>int</returns>
        private async Task<int> GetNumberOfDPs(Guid deliveryRouteId, Guid operationalObjectTypeForDp, Guid userUnit)
        {
            int numberOfDPs = await (from blkseq in DataContext.BlockSequences.AsNoTracking()
                                     join drb in DataContext.DeliveryRouteBlocks.AsNoTracking() on blkseq.Block_GUID equals drb.Block_GUID
                                     join dr in DataContext.DeliveryRoutes.AsNoTracking() on drb.DeliveryRoute_GUID equals dr.ID
                                     join sr in DataContext.Scenarios.AsNoTracking() on dr.DeliveryScenario_GUID equals sr.ID
                                     where blkseq.OperationalObjectType_GUID == operationalObjectTypeForDp && dr.ID == deliveryRouteId &&
                                           sr.Unit_GUID == userUnit
                                     select blkseq.OperationalObject_GUID).CountAsync();
            return numberOfDPs;
        }

        /// <summary>
        /// Gets the number of commercial residential d ps.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="operationalObjectTypeForDp">The operational object type for dp.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>int</returns>
        private async Task<int> GetNumberOfCommercialResidentialDPs(Guid deliveryRouteId, Guid operationalObjectTypeForDp, Guid userUnit)
        {
            int numberOfCommercialResidentialDPs = await (from del in DataContext.DeliveryPoints.AsNoTracking()
                                                          join blkseq in DataContext.BlockSequences.AsNoTracking() on del.ID equals blkseq.OperationalObject_GUID
                                                          join dr in DataContext.DeliveryRoutes.AsNoTracking() on deliveryRouteId equals dr.ID
                                                          join sr in DataContext.Scenarios.AsNoTracking() on dr.DeliveryScenario_GUID equals sr.ID
                                                          join drb in DataContext.DeliveryRouteBlocks.AsNoTracking() on dr.ID equals drb.DeliveryRoute_GUID
                                                          where del.DeliveryPointUseIndicator_GUID == operationalObjectTypeForDp &&
                                                                drb.DeliveryRoute_GUID == deliveryRouteId
                                                                && blkseq.Block_GUID == drb.Block_GUID && sr.Unit_GUID == userUnit
                                                          select del.DeliveryPointUseIndicator_GUID).CountAsync();
            return numberOfCommercialResidentialDPs;
        }

        /// <summary>
        /// Gets the aliases.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="operationalObjectTypeForDp">The operational object type for dp.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>int</returns>
        private async Task<int> GetAliases(Guid deliveryRouteId, Guid operationalObjectTypeForDp, Guid userUnit)
        {
            int noOfDpAliases = await (from dr in DataContext.DeliveryRoutes.AsNoTracking()
                                       join sr in DataContext.Scenarios.AsNoTracking() on dr.DeliveryScenario_GUID equals sr.ID
                                       join drb in DataContext.DeliveryRouteBlocks.AsNoTracking() on dr.ID equals drb.DeliveryRoute_GUID
                                       join b in DataContext.Blocks.AsNoTracking() on drb.Block_GUID equals b.ID
                                       join blkseq in DataContext.BlockSequences.AsNoTracking() on b.ID equals blkseq.Block_GUID
                                       join dp in DataContext.DeliveryPoints.AsNoTracking() on blkseq.OperationalObject_GUID equals dp.ID
                                       join dpa in DataContext.DeliveryPointAlias.AsNoTracking() on dp.ID equals dpa.DeliveryPoint_GUID
                                       where sr.Unit_GUID == userUnit && blkseq.OperationalObjectType_GUID == operationalObjectTypeForDp && dr.ID == deliveryRouteId
                                       select dpa.ID).CountAsync();

            return noOfDpAliases;
        }

        /// <summary>
        /// retrieve Route Sequenced Point By passing RouteID specific to unit
        /// </summary>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <param name="userUnitIdGuid">userUnitIdGuid</param>
        /// <param name="operationalObjectTypeForDp">The operational object type for dp.</param>
        /// <returns>
        /// List of route log sequenced points
        /// </returns>
        private List<RouteLogSequencedPointsDTO> GetDeliveryRouteSequencedPointsByRouteId(Guid deliveryRouteId, Guid userUnitIdGuid, Guid operationalObjectTypeForDp)
        {
            List<RouteLogSequencedPointsDTO> deliveryRouteResult = null;
            var deliveryRoutes = (from dr in DataContext.DeliveryRoutes.AsNoTracking()
                                  join drb in DataContext.DeliveryRouteBlocks.AsNoTracking() on dr.ID equals drb.DeliveryRoute_GUID
                                  join b in DataContext.Blocks.AsNoTracking() on drb.Block_GUID equals b.ID
                                  join bs in DataContext.BlockSequences.AsNoTracking() on b.ID equals bs.Block_GUID
                                  join dp in DataContext.DeliveryPoints.AsNoTracking() on bs.OperationalObject_GUID equals dp.ID
                                  join pa in DataContext.PostalAddresses.AsNoTracking() on dp.Address_GUID equals pa.ID
                                  join sc in DataContext.Scenarios.AsNoTracking() on dr.DeliveryScenario_GUID equals sc.ID
                                  where bs.OperationalObjectType_GUID == operationalObjectTypeForDp && sc.Unit_GUID == userUnitIdGuid && dr.ID == deliveryRouteId
                                  group new { pa, dp } by new { pa.Thoroughfare, pa.BuildingNumber } into grpAddress
                                  select new RouteLogSequencedPointsDTO
                                  {
                                      StreetName = grpAddress.Key.Thoroughfare,
                                      BuildingNumer = grpAddress.Key.BuildingNumber,
                                      DeliveryPointCount = grpAddress.Select(n => n.dp.ID).Count(),
                                      MultipleOccupancy = grpAddress.Sum(n => n.dp.MultipleOccupancyCount)
                                  }).ToList();

            if (deliveryRoutes != null && deliveryRoutes.Count > 0)
            {
                deliveryRouteResult = (from records in deliveryRoutes
                                       where records.BuildingNumer % 2 == 0
                                       group new { records } by new { records.StreetName } into g
                                       select new RouteLogSequencedPointsDTO
                                       {
                                           StreetName = g.Key.StreetName,
                                           FormattedBuildingNumber = g.Select(n => n.records.BuildingNumer).FirstOrDefault().ToString() + " to " + g.Select(n => n.records.BuildingNumer).LastOrDefault().ToString() + " evens",
                                           DeliveryPointCount = g.Select(n => n.records.DeliveryPointCount).Count(),
                                           MultipleOccupancy = g.Sum(n => n.records.MultipleOccupancy)
                                       }).Union(
                      from records in deliveryRoutes
                      where records.BuildingNumer % 2 != 0 && records.BuildingNumer != null
                      group new { records } by new { records.StreetName } into g
                      select new RouteLogSequencedPointsDTO
                      {
                          StreetName = g.Key.StreetName,
                          FormattedBuildingNumber = g.Select(n => n.records.BuildingNumer).FirstOrDefault().ToString() + " to " + g.Select(n => n.records.BuildingNumer).LastOrDefault().ToString() + " odds",
                          DeliveryPointCount = g.Select(n => n.records.DeliveryPointCount).Count(),
                          MultipleOccupancy = g.Sum(n => n.records.MultipleOccupancy)
                      }).Union(
                       from records in deliveryRoutes
                       where records.BuildingNumer == null
                       select new RouteLogSequencedPointsDTO
                       {
                           StreetName = records.StreetName,
                           FormattedBuildingNumber = Convert.ToString(records.BuildingNumer) ?? string.Empty,
                           DeliveryPointCount = records.DeliveryPointCount,
                           MultipleOccupancy = records.MultipleOccupancy
                       }).OrderBy(n => n.StreetName).ToList();
            }

            return deliveryRouteResult;
        }

        /// <summary>
        /// Get paried routes specific to route
        /// </summary>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <param name="sharedVanId">sharedVanId</param>
        /// <param name="operationalObjectId">operationalObjectId</param>
        /// <param name="userUnitId">userUnitId</param>
        /// <returns>Comma separated value of paried routes</returns>
        private async Task<string> GetPairedRoutesByRouteID(Guid deliveryRouteId, Guid sharedVanId, Guid operationalObjectId, Guid userUnitId)
        {
            string pairedRoute = string.Empty;

            var postCodeIds = (from dr in DataContext.DeliveryRoutes
                               join drb in DataContext.DeliveryRouteBlocks on dr.ID equals drb.DeliveryRoute_GUID
                               join b in DataContext.Blocks on drb.Block_GUID equals b.ID
                               join bs in DataContext.BlockSequences on b.ID equals bs.Block_GUID
                               join dp in DataContext.DeliveryPoints on bs.OperationalObject_GUID equals dp.ID
                               join pa in DataContext.PostalAddresses on dp.Address_GUID equals pa.ID
                               join sc in DataContext.Scenarios on dr.DeliveryScenario_GUID equals sc.ID
                               join pc in DataContext.Postcodes on pa.PostCodeGUID equals pc.ID
                               where bs.OperationalObjectType_GUID == operationalObjectId &&
                                     dr.RouteMethodType_GUID == sharedVanId && sc.Unit_GUID == userUnitId && dr.ID == deliveryRouteId
                               select pc.ID).ToList();

            var routeResults = await (from dr in DataContext.DeliveryRoutes
                                      join drb in DataContext.DeliveryRouteBlocks on dr.ID equals drb.DeliveryRoute_GUID
                                      join b in DataContext.Blocks on drb.Block_GUID equals b.ID
                                      join bs in DataContext.BlockSequences on b.ID equals bs.Block_GUID
                                      join dp in DataContext.DeliveryPoints on bs.OperationalObject_GUID equals dp.ID
                                      join pa in DataContext.PostalAddresses on dp.Address_GUID equals pa.ID
                                      join sc in DataContext.Scenarios on dr.DeliveryScenario_GUID equals sc.ID
                                      where bs.OperationalObjectType_GUID == operationalObjectId &&
                                            dr.RouteMethodType_GUID == sharedVanId && sc.Unit_GUID == userUnitId &&
                                            dr.ID != deliveryRouteId && postCodeIds.Contains(pa.PostCodeGUID)
                                      group new { dr } by new { dr.RouteNumber } into grpRoutes
                                      select new
                                      {
                                          RouteNumber = grpRoutes.Key.RouteNumber
                                      }).ToListAsync();

            if (routeResults != null && routeResults.Count > 0)
            {
                pairedRoute = string.Join(",", routeResults);
                pairedRoute = Regex.Replace(pairedRoute, ",+", ",").Trim(',');
            }

            return pairedRoute;
        }

        #endregion Private Methods

        #endregion GeneratePDF
    }
}