using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.DeliveryRoute.WebAPI.DataDTO;
using RM.DataManagement.DeliveryRoute.WebAPI.DataService;
using RM.DataManagement.DeliveryRoute.WebAPI.DTO;
using RM.DataManagement.DeliveryRoute.WebAPI.IntegrationService;
using RM.DataManagement.DeliveryRoute.WebAPI.Utils;

namespace RM.DataManagement.DeliveryRoute.WebAPI.BusinessService.Implementation
{
    public class DeliveryRouteBusinessService : IDeliveryRouteBusinessService
    {
        private IDeliveryRouteDataService deliveryRouteDataService;
        private IDeliveryRouteIntegrationService deliveryRouteIntegrationService;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IBlockSequenceDataService blockSequenceDataService = default(IBlockSequenceDataService);
        private IPostcodeDataService postCodeDataService = default(IPostcodeDataService);

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteBusinessService" /> class and other classes.
        /// </summary>
        /// <param name="deliveryRouteDataService">IDeliveryRouteRepository reference</param>
        /// <param name="scenarioDataService">IScenarioRepository reference</param>
        /// <param name="referenceDataBusinessService">The reference data business service.</param>
        public DeliveryRouteBusinessService(IDeliveryRouteDataService deliveryRouteDataService, IDeliveryRouteIntegrationService deliveryRouteIntegrationService, ILoggingHelper loggingHelper, IBlockSequenceDataService blockSequenceDataService, IPostcodeDataService postCodeDataService)
        {
            // Store  injected dependencies
            this.deliveryRouteDataService = deliveryRouteDataService;
            this.deliveryRouteIntegrationService = deliveryRouteIntegrationService;
            this.loggingHelper = loggingHelper;
            this.blockSequenceDataService = blockSequenceDataService;
            this.postCodeDataService = postCodeDataService;
        }

        /// <summary>
        /// Get route details specific to scenario.
        /// </summary>
        /// <param name="scenarioID">ID of the selected scenario</param>
        /// <returns>Returns list of route on the basis of selected scenario</returns>
        public async Task<List<RouteDTO>> GetScenarioRoutes(Guid scenarioID)
        {
            if (scenarioID == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(scenarioID));
            }

            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRoutes"))
            {
                string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetScenarioRoutes);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId);

                var routeDetails = await deliveryRouteDataService.GetScenarioRoutes(scenarioID);

                List<RouteDTO> routes = GenericMapper.MapList<RouteDataDTO, RouteDTO>(routeDetails);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId);

                return routes.OrderBy(n => n.DisplayText).ToList();
            }
        }

        /// <summary>
        /// Get filtered routes on basis of search text for Advance Search .
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>Returns list of routes that matches the search text</returns>
        public async Task<List<RouteDTO>> GetRoutesForAdvanceSearch(string searchText, Guid locationId)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                throw new ArgumentNullException(nameof(searchText));
            }

            if (locationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(locationId));
            }

            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRouteForAdvanceSearch"))
            {
                string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetRoutesForAdvanceSearch);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId);

                var routeDetails = await deliveryRouteDataService.GetRoutesForAdvanceSearch(searchText, locationId);

                List<RouteDTO> routes = GenericMapper.MapList<RouteDataDTO, RouteDTO>(routeDetails);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId);

                return routes;
            }
        }

        /// <summary>
        /// Get filtered routes on basis of search text for basic Search .
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>Returns list of routes that matches the search text</returns>
        public async Task<List<RouteDTO>> GetRoutesForBasicSearch(string searchText, Guid locationId)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                throw new ArgumentNullException(nameof(searchText));
            }

            if (locationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(locationId));
            }

            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRouteForBasicSearch"))
            {
                string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetRoutesForBasicSearch);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId);

                var routeDetails = await deliveryRouteDataService.GetRoutesForBasicSearch(searchText, locationId);

                List<RouteDTO> routes = GenericMapper.MapList<RouteDataDTO, RouteDTO>(routeDetails);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId);

                return routes;
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
            if (string.IsNullOrEmpty(searchText))
            {
                throw new ArgumentNullException(nameof(searchText));
            }

            if (locationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(locationId));
            }

            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRouteCount"))
            {
                string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetRouteCount);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId);

                int routeCount = await deliveryRouteDataService.GetRouteCount(searchText, locationId);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId);

                return routeCount;
            }
        }

        /// <summary>
        /// Gets the delivery route details for Pdf Generation.
        /// </summary>
        /// <param name="routeId">The delivery route identifier.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>DeliveryRouteDTO</returns>
        public async Task<RouteDTO> GetRouteSummary(Guid routeId)
        {
            if (routeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(routeId));
            }

            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryRouteDetailsforPdfGeneration"))
            {
                string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetRouteSummary);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId);

                List<string> categoryNames = new List<string>
            {
                ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                ReferenceDataCategoryNames.OperationalObjectType,
                ReferenceDataCategoryNames.DeliveryRouteMethodType,
                ReferenceDataCategoryNames.RouteActivityType
            };

                var referenceDataCategoryList = deliveryRouteIntegrationService.GetReferenceDataSimpleLists(categoryNames).Result;

                var routeDetails = await deliveryRouteDataService.GetRouteSummary(routeId, referenceDataCategoryList);

                RouteDTO route = GenericMapper.Map<RouteDataDTO, RouteDTO>(routeDetails);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId);

                return route;
            }
        }

        /// <summary>
        /// Generates the route log.
        /// </summary>
        /// <param name="routeDetails">Route Details</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns>byte[]</returns>
        public async Task<RouteLogSummaryDTO> GenerateRouteLog(RouteDTO routeDetails)
        {
            if (routeDetails == null)
            {
                throw new ArgumentNullException(nameof(routeDetails));
            }

            using (loggingHelper.RMTraceManager.StartTrace("Business.GenerateRouteLog"))
            {
                string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GenerateRouteLog);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId);
                RouteLogSummaryDTO routeLogSummary = new RouteLogSummaryDTO();
                routeLogSummary.Route = routeDetails;
                var routeLogSequencedPoints = await deliveryRouteDataService.GetSequencedRouteDetails(routeDetails.ID);
                routeLogSummary.RouteLogSequencedPoints = GenericMapper.MapList<RouteLogSequencedPointsDataDTO, RouteLogSequencedPointsDTO>(routeLogSequencedPoints);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId);

                return routeLogSummary;
            }
        }

        /// <summary>
        /// Get route details specific to postcode
        /// </summary>
        /// <param name="postcodeUnit">Post code</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>List of routes</returns>
        public async Task<List<RouteDTO>> GetPostcodeSpecificRoutes(string postcodeUnit, Guid locationId)
        {
            if (string.IsNullOrEmpty(postcodeUnit))
            {
                throw new ArgumentNullException(nameof(postcodeUnit));
            }

            if (locationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(locationId));
            }

            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostcodeSpecificRoutes"))
            {
                string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetPostcodeSpecificRoutes);
                List<RouteDTO> routes = new List<RouteDTO>();
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId);

                var postcode = await postCodeDataService.GetPostcode(postcodeUnit);
                var routeDetails = await deliveryRouteDataService.GetRoutesByLocation(locationId);

                if (routeDetails != null && routeDetails.Count > 0)
                {
                    if (postcode != null && (postcode.PrimaryRouteGUID != null || postcode.SecondaryRouteGUID != null))
                    {
                        foreach (var route in routeDetails)
                        {
                            if (route.ID == postcode.PrimaryRouteGUID)
                            {
                                routes.Add(new RouteDTO { ID = route.ID, RouteName = DeliveryRouteConstants.PrimaryRoute + route.RouteName });
                            }
                            else if (route.ID == postcode.SecondaryRouteGUID)
                            {
                                routes.Add(new RouteDTO { ID = route.ID, RouteName = DeliveryRouteConstants.SecondaryRoute + route.RouteName });
                            }
                        }
                    }
                    else
                    {
                        routes = GenericMapper.MapList<RouteDataDTO, RouteDTO>(routeDetails);
                    }
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId);

                return routes.OrderBy(n => n.RouteName).ToList();
            }
        }

        /// <summary>
        /// method to save delivery point and selected route mapping in block sequence table
        /// </summary>
        /// <param name="routeId">selected route id</param>
        /// <param name="deliveryPointId">Delivery point unique id</param>
        public void SaveDeliveryPointRouteMapping(Guid routeId, Guid deliveryPointId)
        {
            if (routeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(routeId));
            }

            if (routeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(routeId));
            }

            using (loggingHelper.RMTraceManager.StartTrace("Business.SaveDeliveryPointRouteMapping"))
            {
                string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(SaveDeliveryPointRouteMapping);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId);

                blockSequenceDataService.SaveDeliveryPointRouteMapping(routeId, deliveryPointId);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId);
            }
        }

        /// <summary>
        /// Get route details mapped to delivery point
        /// </summary>
        /// <param name="deliveryPointId">Delivery Point Id</param>
        /// <returns>Route Details</returns>
        public async Task<RouteDTO> GetRouteByDeliveryPoint(Guid deliveryPointId)
        {
            if (deliveryPointId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(deliveryPointId));
            }

            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRouteByDeliverypoint"))
            {
                string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetRouteByDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId);

                var route = GenericMapper.Map<RouteDataDTO, RouteDTO>(await deliveryRouteDataService.GetRouteByDeliverypoint(deliveryPointId));

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId);

                return route;
            }
        }

        /// <summary>
        /// Delete delivery point reference from route activity table.
        /// </summary>
        /// <param name="deliveryPointId">Delivery point Id</param>
        /// <returns>boolean value</returns>
        public async Task<bool> DeleteDeliveryPointRouteMapping(Guid deliveryPointId)
        {
            if (deliveryPointId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(deliveryPointId));
            }

            using (loggingHelper.RMTraceManager.StartTrace("Business.DeleteDeliveryPointRouteMapping"))
            {
                string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(DeleteDeliveryPointRouteMapping);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId);

                bool routeActivityDeleted = await deliveryRouteDataService.DeleteDeliveryPointRouteMapping(deliveryPointId);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId);

                return routeActivityDeleted;
            }
        }
    }
}