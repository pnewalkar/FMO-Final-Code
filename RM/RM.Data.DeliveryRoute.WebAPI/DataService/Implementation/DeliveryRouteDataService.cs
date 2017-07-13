using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryRoute.WebAPI.Entities;
using RM.DataManagement.DeliveryRoute.WebAPI.DataDTO;
using RM.DataManagement.DeliveryRoute.WebAPI.Utils;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DataService
{
    /// <summary>
    /// This class contains methods for fetching Delivery route data for basic and advance search
    /// </summary>
    public class DeliveryRouteDataService : DataServiceBase<Route, RouteDBContext>, IDeliveryRouteDataService
    {
        private int priority = LoggerTraceConstants.DeliveryRouteAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryRouteDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryRouteDataServiceMethodExitEventId;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public DeliveryRouteDataService(IDatabaseFactory<RouteDBContext> databaseFactory, ILoggingHelper loggingHelper, IConfigurationHelper configurationHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Get route details specific to scenario.
        /// </summary>
        /// <param name="scenarioID">ID of the selected scenario</param>
        /// <returns>Returns list of route on the basis of selected scenario</returns>
        public List<RouteDataDTO> GetRoutes(Guid scenarioID)
        {
            string methodName = typeof(DeliveryRouteDataService) + "." + nameof(GetRoutes);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetRoutes"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var routes = from r in DataContext.Routes.AsNoTracking()
                             join s in DataContext.ScenarioRoutes.AsNoTracking() on r.ID equals s.RouteID
                             where s.ScenarioID == scenarioID
                             select r;
                List<RouteDataDTO> routedetails = GenericMapper.MapList<Route, RouteDataDTO>(routes.ToList());

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return routedetails;
            }
        }

        /// <summary>
        /// Get filtred routes on basis of search text for Advance Search .
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>Returns list of routes that matches the search text</returns>
        public async Task<List<RouteDataDTO>> GetRoutesForAdvanceSearch(string searchText, Guid locationId)
        {
            string methodName = typeof(DeliveryRouteDataService) + "." + nameof(GetRoutesForAdvanceSearch);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetRouteForAdvanceSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var routes = await (from r in DataContext.Routes.AsNoTracking()
                                    join sr in DataContext.ScenarioRoutes.AsNoTracking() on r.ID equals sr.RouteID
                                    join s in DataContext.Scenarios.AsNoTracking() on sr.ScenarioID equals s.ID
                                    where s.LocationID == locationId && (r.RouteName.StartsWith(searchText) || r.RouteNumber.StartsWith(searchText))
                                    select r).ToListAsync();

                List<RouteDataDTO> routedetails = GenericMapper.MapList<Route, RouteDataDTO>(routes);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return routedetails;
            }
        }

        /// <summary>
        ///Get filtered routes on basis of search text for basic Search .
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>Returns list of routes that matches the search text</returns>
        public async Task<List<RouteDataDTO>> GetRoutesForBasicSearch(string searchText, Guid locationId)
        {
            string methodName = typeof(DeliveryRouteDataService) + "." + nameof(GetRoutesForBasicSearch);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetRouteForBasicSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                int takeCount = Convert.ToInt32(configurationHelper.ReadAppSettingsConfigurationValues(DeliveryRouteConstants.SearchResultCount));
                searchText = searchText ?? string.Empty;

                var routes = await (from r in DataContext.Routes.AsNoTracking()
                                    join sr in DataContext.ScenarioRoutes.AsNoTracking() on r.ID equals sr.RouteID
                                    join s in DataContext.Scenarios.AsNoTracking() on sr.ScenarioID equals s.ID
                                    where s.LocationID == locationId && (r.RouteName.StartsWith(searchText) || r.RouteNumber.StartsWith(searchText))
                                    select r).Take(takeCount).ToListAsync();

                List<RouteDataDTO> routedetails = GenericMapper.MapList<Route, RouteDataDTO>(routes);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return routedetails;
            }
        }

        /// <summary>
        /// Get filtered route count
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>The total count of delivery route</returns>
        public async Task<int> GetRouteCount(string searchText, Guid locationId)
        {
            string methodName = typeof(DeliveryRouteDataService) + "." + nameof(GetRouteCount);
            searchText = searchText ?? string.Empty;
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetRouteCount"))
            {
                try
                {
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    int getDeliveryRouteCount = await (from r in DataContext.Routes.AsNoTracking()
                                                       join sr in DataContext.ScenarioRoutes.AsNoTracking() on r.ID equals sr.RouteID
                                                       join s in DataContext.Scenarios.AsNoTracking() on sr.ScenarioID equals s.ID
                                                       where s.LocationID == locationId && (r.RouteName.StartsWith(searchText) || r.RouteNumber.StartsWith(searchText))
                                                       select r).CountAsync();

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return getDeliveryRouteCount;
                }
                catch (InvalidOperationException ex)
                {
                    ex.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForSingleorDefault, ex);
                }
                catch (OverflowException overflow)
                {
                    overflow.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_OverflowException, overflow);
                }
            }
        }

        #region GeneratePDF

        /// <summary>
        /// Gets the delivery route detailsfor PDF generation.
        /// </summary>
        /// <param name="routeId">Selected route Id</param>
        /// <param name="referenceDataCategoryDtoList">The reference data category dto list.</param>
        /// <returns>Route details </returns>
        public async Task<RouteDataDTO> GetDeliveryRouteDetailsforPdfGeneration(Guid routeId, List<ReferenceDataCategoryDTO> referenceDataCategoryDtoList)
        {
            string methodName = typeof(DeliveryRouteDataService) + "." + nameof(GetDeliveryRouteDetailsforPdfGeneration);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetDeliveryRouteDetailsforPdfGeneration"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                // No of DPs
                Guid operationalObjectTypeForDp = referenceDataCategoryDtoList
                .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.OperationalObjectType)
                .SelectMany(x => x.ReferenceDatas)
                .Where(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).Select(x => x.ID)
                .SingleOrDefault();

                // No. Organisation DP
                Guid operationalObjectTypeForDpOrganisation = referenceDataCategoryDtoList
                    .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DeliveryPointUseIndicator)
                    .SelectMany(x => x.ReferenceDatas)
                    .Where(x => x.ReferenceDataValue == ReferenceDataValues.Organisation).Select(x => x.ID)
                    .SingleOrDefault();

                // No. Residential DP
                Guid operationalObjectTypeForDpResidential = referenceDataCategoryDtoList
                    .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DeliveryPointUseIndicator)
                    .SelectMany(x => x.ReferenceDatas)
                    .Where(x => x.ReferenceDataValue == ReferenceDataValues.Residential).Select(x => x.ID)
                    .SingleOrDefault();

                // Delivery Route Method Type
                var referenceDataDeliveryMethodTypes = referenceDataCategoryDtoList
                    .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DeliveryRouteMethodType)
                    .SelectMany(x => x.ReferenceDatas).ToList();

                // Route Activity Type
                var referenceDataRouteActivityTypes = referenceDataCategoryDtoList
                    .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.RouteActivityType)
                    .SelectMany(x => x.ReferenceDatas).ToList();

                Guid sharedDeliveryRouteMethodTypeGuid = referenceDataDeliveryMethodTypes
                    .Where(x => x.ReferenceDataValue == ReferenceDataValues.DeliveryRouteMethodTypeRmVanShared)
                    .Select(x => x.ID).SingleOrDefault();

                Guid travelOutRouteActivityTypeGuid = referenceDataRouteActivityTypes
                    .Where(x => x.ReferenceDataValue == ReferenceDataValues.TravelOut)
                    .Select(x => x.ID).SingleOrDefault();

                Guid travelInRouteActivityTypeGuid = referenceDataRouteActivityTypes
                    .Where(x => x.ReferenceDataValue == ReferenceDataValues.TravelIn)
                    .Select(x => x.ID).SingleOrDefault();

                List<Guid> refDataActivityTypes = new List<Guid>() { travelOutRouteActivityTypeGuid, travelInRouteActivityTypeGuid };

                var deliveryRoutesDto = await (from dr in DataContext.Routes.AsNoTracking()
                                               join sr in DataContext.ScenarioRoutes.AsNoTracking() on dr.ID equals sr.RouteID
                                               join sc in DataContext.Scenarios.AsNoTracking() on sr.ScenarioID equals sc.ID
                                               where dr.ID == routeId
                                               select new RouteDataDTO
                                               {
                                                   ID = dr.ID,
                                                   RouteName = dr.RouteName,
                                                   RouteNumber = dr.RouteNumber,
                                                   ScenarioName = sc.ScenarioName,
                                                   MethodReferenceGuid = dr.RouteMethodTypeGUID,
                                                   Totaltime = Math.Abs(dr.SpanTimeMinute.Value).ToString()
                                               }).SingleOrDefaultAsync();

                var routeActivityTypes = (from dr in DataContext.RouteActivities.AsNoTracking()
                                          where dr.RouteID == routeId && refDataActivityTypes.Contains(dr.ActivityTypeGUID.Value)
                                          select new { dr.ActivityTypeGUID, dr.ResourceGUID }).Distinct().ToList();

                if (deliveryRoutesDto != null && routeActivityTypes != null && routeActivityTypes.Count > 0)
                {
                    deliveryRoutesDto.Totaltime = ConvertToMinutes(Convert.ToDouble(deliveryRoutesDto.Totaltime));

                    var methodType = referenceDataDeliveryMethodTypes.SingleOrDefault(x => x.ID == deliveryRoutesDto.MethodReferenceGuid);

                    if (methodType != null)
                    {
                        deliveryRoutesDto.Method = methodType.ReferenceDataValue;
                    }

                    foreach (var refdataActivity in routeActivityTypes)
                    {
                        if (refdataActivity.ActivityTypeGUID == travelInRouteActivityTypeGuid)
                        {
                            var travelInType = referenceDataDeliveryMethodTypes.SingleOrDefault(x => x.ID == refdataActivity.ResourceGUID);
                            if (travelInType != null)
                            {
                                deliveryRoutesDto.AccelarationIn = travelInType.ReferenceDataValue;
                            }
                        }
                        else
                        {
                            var travelOutType = referenceDataDeliveryMethodTypes.SingleOrDefault(x => x.ID == refdataActivity.ResourceGUID);
                            if (travelOutType != null)
                            {
                                deliveryRoutesDto.AccelarationOut = travelOutType.ReferenceDataValue;
                            }
                        }
                    }
                }
                else
                {
                    deliveryRoutesDto = new RouteDataDTO();
                }

                deliveryRoutesDto.Aliases = 0;
                deliveryRoutesDto.Blocks = await GetNumberOfBlocks(routeId);
                deliveryRoutesDto.DPs = await GetNumberOfDPs(routeId);
                deliveryRoutesDto.BusinessDPs = await GetNumberOfCommercialResidentialDPs(routeId, operationalObjectTypeForDpOrganisation);
                deliveryRoutesDto.ResidentialDPs = await GetNumberOfCommercialResidentialDPs(routeId, operationalObjectTypeForDpResidential);
                deliveryRoutesDto.PairedRoute = await GetPairedRoutesByRouteID(routeId, sharedDeliveryRouteMethodTypeGuid);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return deliveryRoutesDto;
            }
        }

        #region Private Methods

        /// <summary>
        /// Gets the number of blocks specific to route.
        /// </summary>
        /// <param name="routeId">The delivery route identifier.</param>
        /// <returns>block count</returns>
        private async Task<int> GetNumberOfBlocks(Guid routeId)
        {
            string methodName = typeof(DeliveryRouteDataService) + "." + nameof(GetNumberOfBlocks);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetNumberOfBlocks"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    int numberOfBlocks = await (from route in DataContext.Routes.AsNoTracking()
                                                join routeActivity in DataContext.RouteActivities.AsNoTracking() on route.ID equals routeActivity.RouteID
                                                where route.ID == routeId
                                                select routeActivity.BlockID).Distinct().CountAsync();

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return numberOfBlocks;
                }
                catch (InvalidOperationException ex)
                {
                    ex.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForCountAsync, ex);
                }
                catch (OverflowException overflow)
                {
                    overflow.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_OverflowException, overflow);
                }
            }
        }

        /// <summary>
        /// Get count of delivery points specific to route
        /// </summary>
        /// <param name="routeId">Selected route id.</param>
        /// <returns>int</returns>
        private async Task<int> GetNumberOfDPs(Guid routeId)
        {
            string methodName = typeof(DeliveryRouteDataService) + "." + nameof(GetNumberOfDPs);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetNumberOfDPs"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    int numberOfDPs = await (from route in DataContext.Routes.AsNoTracking()
                                             join routeActivity in DataContext.RouteActivities.AsNoTracking() on route.ID equals routeActivity.RouteID
                                             join deliveryPoint in DataContext.DeliveryPoints.AsNoTracking() on routeActivity.LocationID equals deliveryPoint.ID
                                             where route.ID == routeId
                                             select deliveryPoint.ID).CountAsync();

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return numberOfDPs;
                }
                catch (InvalidOperationException ex)
                {
                    ex.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForCountAsync, ex);
                }
                catch (OverflowException overflow)
                {
                    overflow.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_OverflowException, overflow);
                }
            }
        }

        /// <summary>
        /// Gets the number of commercial residential d ps.
        /// </summary>
        /// <param name="routeId">The delivery route identifier.</param>
        /// <param name="operationalObjectTypeForDp">The operational object type for dp.</param>
        /// <returns>int</returns>
        private async Task<int> GetNumberOfCommercialResidentialDPs(Guid routeId, Guid operationalObjectTypeForDp)
        {
            string methodName = typeof(DeliveryRouteDataService) + "." + nameof(GetNumberOfCommercialResidentialDPs);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetNumberOfCommercialResidentialDPs"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    int numberOfCommercialResidentialDPs = await (from route in DataContext.Routes.AsNoTracking()
                                                                  join routeActivity in DataContext.RouteActivities.AsNoTracking() on route.ID equals routeActivity.RouteID
                                                                  join deliveryPoint in DataContext.DeliveryPoints.AsNoTracking() on routeActivity.LocationID equals deliveryPoint.ID
                                                                  where deliveryPoint.DeliveryPointUseIndicatorGUID == operationalObjectTypeForDp && route.ID == routeId
                                                                  select deliveryPoint.DeliveryPointUseIndicatorGUID).CountAsync();

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return numberOfCommercialResidentialDPs;
                }
                catch (InvalidOperationException ex)
                {
                    ex.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForCountAsync, ex);
                }
                catch (OverflowException overflow)
                {
                    overflow.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_OverflowException, overflow);
                }
            }
        }

        /// <summary>
        /// Get paried routes specific to route
        /// </summary>
        /// <param name="routeId">Select route id</param>
        /// <param name="sharedVanId">Shared van ID from reference table</param>
        /// <returns>Comma separated value of paried routes</returns>
        private async Task<string> GetPairedRoutesByRouteID(Guid routeId, Guid sharedVanId)
        {
            string methodName = typeof(DeliveryRouteDataService) + "." + nameof(GetPairedRoutesByRouteID);
            string pairedRoute = string.Empty;

            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPairedRoutesByRouteID"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var routeResults = await (from r in DataContext.Routes.AsNoTracking()
                                          join pr in DataContext.Routes.AsNoTracking() on r.PairedRouteID equals pr.ID
                                          where r.ID == routeId && r.RouteMethodTypeGUID == sharedVanId
                                          select new
                                          {
                                              RouteNumber = pr.RouteNumber
                                          }).ToListAsync();

                if (routeResults != null && routeResults.Count > 0)
                {
                    foreach (var item in routeResults)
                    {
                        pairedRoute = pairedRoute + "," + item.RouteNumber;
                    }
                    pairedRoute = Regex.Replace(pairedRoute, ",+", ",").Trim(',');
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
            }
            return pairedRoute;
        }

        /// <summary>
        /// Convert hours in minutes
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>string</returns>
        private string ConvertToMinutes(double value)
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(value);
            return timeSpan.ToString(@"h\:mm") + " mins";
        }

        /// <summary>
        /// retrieve Route Sequenced Point By passing RouteID specific to unit
        /// </summary>
        /// <param name="routeId">Selected Route</param>
        /// <returns>
        /// List of route log sequenced points
        /// </returns>
        public async Task<List<RouteLogSequencedPointsDataDTO>> GetDeliveryRouteSequencedPointsByRouteId(Guid routeId)
        {
            var deliveryRoutes = await (from route in DataContext.Routes.AsNoTracking()
                                        join routeActivity in DataContext.RouteActivities.AsNoTracking() on route.ID equals routeActivity.RouteID
                                        join deliveryPoint in DataContext.DeliveryPoints.AsNoTracking() on routeActivity.LocationID equals deliveryPoint.ID
                                        join postalAddress in DataContext.PostalAddresses.AsNoTracking() on deliveryPoint.PostalAddressID equals postalAddress.ID
                                        where route.ID == routeId
                                        orderby routeActivity.RouteActivityOrderIndex
                                        select new RouteLogSequencedPointsDataDTO
                                        {
                                            StreetName = postalAddress.Thoroughfare,
                                            BuildingNumber = postalAddress.BuildingNumber,
                                            MultipleOccupancy = deliveryPoint.MultipleOccupancyCount,
                                            SubBuildingName = postalAddress.SubBuildingName,
                                            BuildingName = postalAddress.BuildingName
                                        }).ToListAsync();

            return deliveryRoutes;
        }

        #endregion Private Methods

        #endregion GeneratePDF
    }
}