using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// This class contains methods for fetching Delivery route data for basic and advance search
    /// </summary>
    public class DeliveryRouteDataService : DataServiceBase<DeliveryRoute, RMDBContext>, IDeliveryRouteDataService
    {
        private const string SearchResultCount = "SearchResultCount";

        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public DeliveryRouteDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Fetch the  Route.
        /// </summary>
        /// <param name="operationStateId">Guid operationStateID</param>
        /// <param name="deliveryScenarioId">Guid deliveryScenarioID</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>List</returns>
        public List<RouteDTO> FetchRoutes(Guid operationStateId, Guid deliveryScenarioId, Guid userUnit, string UnitName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.FetchRoutes"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodEntryEventId, LoggerTraceConstants.Title);
                List<RouteDTO> routedetails = null;

                if (string.Equals(UserUnit.CollectionUnit.GetDescription(), UnitName.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    IEnumerable<CollectionRoute> result = DataContext.CollectionRoutes.AsNoTracking()
                                                          .Where(x => x.Scenario.Unit_GUID == userUnit && x.CollectionScenario_GUID == deliveryScenarioId &&
                                                           x.Scenario.OperationalState_GUID == operationStateId).ToList();
                    routedetails = GenericMapper.MapList<CollectionRoute, RouteDTO>(result.ToList());
                }
                else
                {
                    IEnumerable<DeliveryRoute> result = DataContext.DeliveryRoutes.AsNoTracking()
                                                        .Where(x => x.Scenario.Unit_GUID == userUnit && x.DeliveryScenario_GUID == deliveryScenarioId &&
                                                        x.Scenario.OperationalState_GUID == operationStateId).ToList();
                    routedetails = GenericMapper.MapList<DeliveryRoute, RouteDTO>(result.ToList());
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodExitEventId, LoggerTraceConstants.Title);

                return routedetails;
            }
        }

        /// <summary>
        /// Fetch Delivery Route for Advance Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <param name="UnitName">UnitName </param>
        /// <returns>Task</returns>
        public async Task<List<RouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText, Guid userUnit, string UnitName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.FetchDeliveryRouteForAdvanceSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodEntryEventId, LoggerTraceConstants.Title);
                List<RouteDTO> routedetails = null;

                if (string.Equals(UserUnit.CollectionUnit.GetDescription(), UnitName.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    routedetails = await DataContext.CollectionRoutes.AsNoTracking()
                                  .Where(l => (l.Scenario.Unit_GUID == userUnit) &&
                                              (l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)))
                                  .Select(l => new RouteDTO
                                  {
                                      ID = l.ID,
                                      RouteName = l.RouteName,
                                      RouteNumber = l.RouteNumber
                                  }).ToListAsync();
                }
                else
                {
                    routedetails = await DataContext.DeliveryRoutes.AsNoTracking()
                                   .Where(l => (l.Scenario.Unit_GUID == userUnit) &&
                                               (l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)))
                                   .Select(l => new RouteDTO
                                   {
                                       ID = l.ID,
                                       RouteName = l.RouteName,
                                       RouteNumber = l.RouteNumber
                                   }).ToListAsync();
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return routedetails;
            }
        }

        /// <summary>
        /// Fetch Delivery route for Basic Search
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        ///  <param name="UnitName">UnitName </param>
        /// <returns>The result set of delivery route.</returns>
        public async Task<List<RouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText, Guid userUnit, string UnitName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.FetchDeliveryRouteForBasicSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                int takeCount = Convert.ToInt32(ConfigurationManager.AppSettings[SearchResultCount]);
                searchText = searchText ?? string.Empty;
                List<RouteDTO> routedetails = null;

                if (string.Equals(UserUnit.CollectionUnit.GetDescription(), UnitName.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    routedetails = await DataContext.CollectionRoutes.AsNoTracking()
                    .Where(l => (l.Scenario.Unit_GUID == userUnit) &&
                                (l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)))
                    .Take(takeCount)
                    .Select(l => new RouteDTO
                    {
                        ID = l.ID,
                        RouteName = l.RouteName,
                        RouteNumber = l.RouteNumber
                    })
                    .ToListAsync();
                }
                else
                {
                    routedetails = await DataContext.DeliveryRoutes.AsNoTracking()
                    .Where(l => (l.Scenario.Unit_GUID == userUnit) &&
                                (l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)))
                    .Take(takeCount)
                    .Select(l => new RouteDTO
                    {
                        ID = l.ID,
                        RouteName = l.RouteName,
                        RouteNumber = l.RouteNumber
                    })
                    .ToListAsync();
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return routedetails;
            }
        }

        /// <summary>
        /// Get the count of delivery route
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <param name="UnitName">UnitName </param>
        /// <returns>The total count of delivery route</returns>
        public async Task<int> GetDeliveryRouteCount(string searchText, Guid userUnit, string UnitName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetDeliveryRouteCount"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    searchText = searchText ?? string.Empty;
                    int getDeliveryRouteCount = 0; ;

                    if (string.Equals(UserUnit.CollectionUnit.GetDescription(), UnitName.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        getDeliveryRouteCount = await DataContext.CollectionRoutes.AsNoTracking()
                        .Where(l => (l.Scenario.Unit_GUID == userUnit) &&
                                    (l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)))
                        .CountAsync();
                    }
                    else
                    {
                        getDeliveryRouteCount = await DataContext.DeliveryRoutes.AsNoTracking()
                        .Where(l => (l.Scenario.Unit_GUID == userUnit) &&
                                    (l.RouteName.StartsWith(searchText) || l.RouteNumber.StartsWith(searchText)))
                        .CountAsync();
                    }

                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodExitEventId, LoggerTraceConstants.Title);
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
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="referenceDataCategoryDtoList">The reference data category dto list.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>DeliveryRouteDTO</returns>
        public async Task<RouteDTO> GetDeliveryRouteDetailsforPdfGeneration(Guid deliveryRouteId, List<ReferenceDataCategoryDTO> referenceDataCategoryDtoList, Guid userUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetDeliveryRouteDetailsforPdfGeneration"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

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

                Guid sharedDeliveryRouteMethodTypeGuid = referenceDataDeliveryMethodTypes
                    .Where(x => x.ReferenceDataValue == ReferenceDataValues.DeliveryRouteMethodTypeRmVanShared)
                    .Select(x => x.ID).SingleOrDefault();

                var deliveryRoutesDto = await (from dr in DataContext.DeliveryRoutes.AsNoTracking()
                                               join sr in DataContext.Scenarios.AsNoTracking() on dr.DeliveryScenario_GUID equals sr.ID
                                               where dr.ID == deliveryRouteId && sr.Unit_GUID == userUnit
                                               select new RouteDTO
                                               {
                                                   ID = dr.ID,
                                                   RouteName = dr.RouteName,
                                                   RouteNumber = dr.RouteNumber,
                                                   ScenarioName = sr.ScenarioName,
                                                   MethodReferenceGuid = dr.RouteMethodType_GUID,
                                                   Totaltime = Math.Abs(dr.TravelOutTimeMin.Value - dr.TravelInTimeMin.Value).ToString(),
                                                   TravelOutTransportType_GUID = dr.TravelOutTransportType_GUID,
                                                   TravelInTransportType_GUID = dr.TravelInTransportType_GUID
                                               }).SingleOrDefaultAsync();

                if (deliveryRoutesDto != null)
                {
                    deliveryRoutesDto.Totaltime = ConvertToMinutes(Convert.ToDouble(deliveryRoutesDto.Totaltime));
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
                }

                if (deliveryRoutesDto == null)
                {
                    deliveryRoutesDto = new RouteDTO();
                }

                deliveryRoutesDto.Aliases = await GetAliases(deliveryRouteId, operationalObjectTypeForDp, userUnit);
                deliveryRoutesDto.Blocks = await GetNumberOfBlocks(deliveryRouteId, userUnit);
                deliveryRoutesDto.DPs = await GetNumberOfDPs(deliveryRouteId, operationalObjectTypeForDp, userUnit);
                deliveryRoutesDto.BusinessDPs = await GetNumberOfCommercialResidentialDPs(deliveryRouteId, operationalObjectTypeForDpOrganisation, userUnit);
                deliveryRoutesDto.ResidentialDPs = await GetNumberOfCommercialResidentialDPs(deliveryRouteId, operationalObjectTypeForDpResidential, userUnit);
                deliveryRoutesDto.PairedRoute = await GetPairedRoutesByRouteID(deliveryRouteId, sharedDeliveryRouteMethodTypeGuid, operationalObjectTypeForDp, userUnit);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return deliveryRoutesDto;
            }
        }

        public async Task<RouteLogSummaryModelDTO> GenerateRouteLog(RouteDTO deliveryRouteDto, Guid userUnit, Guid operationalObjectTypeForDp)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GenerateRouteLog"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                RouteLogSummaryModelDTO routeLogSummary = new RouteLogSummaryModelDTO();
                routeLogSummary.RouteLogSequencedPoints = await GetDeliveryRouteSequencedPointsByRouteId(deliveryRouteDto.ID, userUnit, operationalObjectTypeForDp);
                routeLogSummary.DeliveryRoute = deliveryRouteDto;

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return routeLogSummary;
            }
        }

        #region Private Methods

        /// <summary>
        /// Gets the number of blocks.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>int</returns>
        private async Task<int> GetNumberOfBlocks(Guid deliveryRouteId, Guid userUnit)
        {
            try
            {
                int numberOfBlocks = await (from drb in DataContext.DeliveryRouteBlocks.AsNoTracking()
                                            join dr in DataContext.DeliveryRoutes.AsNoTracking() on drb.DeliveryRoute_GUID equals dr.ID
                                            join sr in DataContext.Scenarios.AsNoTracking() on dr.DeliveryScenario_GUID equals sr.ID
                                            where drb.DeliveryRoute_GUID == deliveryRouteId && sr.Unit_GUID == userUnit
                                            select drb.Block_GUID).CountAsync();
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

        /// <summary>
        /// Gets the number of d ps.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="operationalObjectTypeForDp">The operational object type for dp.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>int</returns>
        private async Task<int> GetNumberOfDPs(Guid deliveryRouteId, Guid operationalObjectTypeForDp, Guid userUnit)
        {
            try
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

        /// <summary>
        /// Gets the number of commercial residential d ps.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="operationalObjectTypeForDp">The operational object type for dp.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>int</returns>
        private async Task<int> GetNumberOfCommercialResidentialDPs(Guid deliveryRouteId, Guid operationalObjectTypeForDp, Guid userUnit)
        {
            try
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

        /// <summary>
        /// Gets the aliases.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <param name="operationalObjectTypeForDp">The operational object type for dp.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>int</returns>
        private async Task<int> GetAliases(Guid deliveryRouteId, Guid operationalObjectTypeForDp, Guid userUnit)
        {
            try
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

            var postCodeIds = (from dr in DataContext.DeliveryRoutes.AsNoTracking()
                               join drb in DataContext.DeliveryRouteBlocks.AsNoTracking() on dr.ID equals drb.DeliveryRoute_GUID
                               join b in DataContext.Blocks.AsNoTracking() on drb.Block_GUID equals b.ID
                               join bs in DataContext.BlockSequences.AsNoTracking() on b.ID equals bs.Block_GUID
                               join dp in DataContext.DeliveryPoints.AsNoTracking() on bs.OperationalObject_GUID equals dp.ID
                               join pa in DataContext.PostalAddresses.AsNoTracking() on dp.Address_GUID equals pa.ID
                               join sc in DataContext.Scenarios.AsNoTracking() on dr.DeliveryScenario_GUID equals sc.ID
                               join pc in DataContext.Postcodes.AsNoTracking() on pa.PostCodeGUID equals pc.ID
                               where bs.OperationalObjectType_GUID == operationalObjectId &&
                                     dr.RouteMethodType_GUID == sharedVanId && sc.Unit_GUID == userUnitId && dr.ID == deliveryRouteId
                               select pc.ID).ToList();

            var routeResults = await (from dr in DataContext.DeliveryRoutes.AsNoTracking()
                                      join drb in DataContext.DeliveryRouteBlocks.AsNoTracking() on dr.ID equals drb.DeliveryRoute_GUID
                                      join b in DataContext.Blocks.AsNoTracking() on drb.Block_GUID equals b.ID
                                      join bs in DataContext.BlockSequences.AsNoTracking() on b.ID equals bs.Block_GUID
                                      join dp in DataContext.DeliveryPoints.AsNoTracking() on bs.OperationalObject_GUID equals dp.ID
                                      join pa in DataContext.PostalAddresses.AsNoTracking() on dp.Address_GUID equals pa.ID
                                      join sc in DataContext.Scenarios.AsNoTracking() on dr.DeliveryScenario_GUID equals sc.ID
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
                foreach (var item in routeResults)
                {
                    pairedRoute = pairedRoute + "," + item.RouteNumber;
                }
                pairedRoute = Regex.Replace(pairedRoute, ",+", ",").Trim(',');
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
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <param name="userUnitIdGuid">userUnitIdGuid</param>
        /// <param name="operationalObjectTypeForDp">The operational object type for dp.</param>
        /// <returns>
        /// List of route log sequenced points
        /// </returns>
        private async Task<List<RouteLogSequencedPointsDTO>> GetDeliveryRouteSequencedPointsByRouteId(Guid deliveryRouteId, Guid userUnitIdGuid, Guid operationalObjectTypeForDp)
        {
            var deliveryRoutes = await (from dr in DataContext.DeliveryRoutes.AsNoTracking()
                                        join drb in DataContext.DeliveryRouteBlocks.AsNoTracking() on dr.ID equals drb.DeliveryRoute_GUID
                                        join b in DataContext.Blocks.AsNoTracking() on drb.Block_GUID equals b.ID
                                        join bs in DataContext.BlockSequences.AsNoTracking() on b.ID equals bs.Block_GUID
                                        join dp in DataContext.DeliveryPoints.AsNoTracking() on bs.OperationalObject_GUID equals dp.ID
                                        join pa in DataContext.PostalAddresses.AsNoTracking() on dp.Address_GUID equals pa.ID
                                        join sc in DataContext.Scenarios.AsNoTracking() on dr.DeliveryScenario_GUID equals sc.ID
                                        where bs.OperationalObjectType_GUID == operationalObjectTypeForDp && sc.Unit_GUID == userUnitIdGuid && dr.ID == deliveryRouteId && b.BlockType.Equals("L", StringComparison.OrdinalIgnoreCase)
                                        orderby bs.OrderIndex
                                        select new RouteLogSequencedPointsDTO
                                        {
                                            StreetName = pa.Thoroughfare,
                                            BuildingNumber = pa.BuildingNumber,
                                            DeliveryPointCount = 0,
                                            MultipleOccupancy = dp.MultipleOccupancyCount,
                                            SubBuildingName = pa.SubBuildingName,
                                            BuildingName = pa.BuildingName
                                        }).ToListAsync();

            return deliveryRoutes;
        }

        #endregion Private Methods

        #endregion GeneratePDF
    }
}