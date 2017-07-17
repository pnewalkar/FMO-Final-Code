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
        private int priority = LoggerTraceConstants.DeliveryRouteAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryRouteBusinessServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryRouteBusinessServiceMethodExitEventId;
        private IDeliveryRouteDataService deliveryRouteDataService;
        private IDeliveryRouteIntegrationService deliveryRouteIntegrationService;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IBlockSequenceDataService blockSequenceDataService = default(IBlockSequenceDataService);
        private IPostCodeDataService postCodeDataService = default(IPostCodeDataService);

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteBusinessService" /> class and other classes.
        /// </summary>
        /// <param name="deliveryRouteDataService">IDeliveryRouteRepository reference</param>
        /// <param name="scenarioDataService">IScenarioRepository reference</param>
        /// <param name="referenceDataBusinessService">The reference data business service.</param>
        public DeliveryRouteBusinessService(IDeliveryRouteDataService deliveryRouteDataService, IDeliveryRouteIntegrationService deliveryRouteIntegrationService, ILoggingHelper loggingHelper, IBlockSequenceDataService blockSequenceDataService, IPostCodeDataService postCodeDataService)
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
        public List<RouteDTO> GetScenarioRoutes(Guid scenarioID)
        {
            if (scenarioID == Guid.Empty)
            {
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(scenarioID)));
            }

            string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetScenarioRoutes);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRoutes"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var routeDetails = deliveryRouteDataService.GetScenarioRoutes(scenarioID);

                List<RouteDTO> routes = GenericMapper.MapList<RouteDataDTO, RouteDTO>(routeDetails);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

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
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(searchText)));
            }
            if (locationId == Guid.Empty)
            {
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(locationId)));
            }

            string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetRoutesForAdvanceSearch);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRouteForAdvanceSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var routeDetails = await deliveryRouteDataService.GetRoutesForAdvanceSearch(searchText, locationId);

                List<RouteDTO> routes = GenericMapper.MapList<RouteDataDTO, RouteDTO>(routeDetails);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

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
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(searchText)));
            }
            if (locationId == Guid.Empty)
            {
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(locationId)));
            }

            string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetRoutesForBasicSearch);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRouteForBasicSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var routeDetails = await deliveryRouteDataService.GetRoutesForBasicSearch(searchText, locationId);

                List<RouteDTO> routes = GenericMapper.MapList<RouteDataDTO, RouteDTO>(routeDetails);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

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
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(searchText)));
            }
            if (locationId == Guid.Empty)
            {
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(locationId)));
            }

            string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetRouteCount);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRouteCount"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                int routeCount = await deliveryRouteDataService.GetRouteCount(searchText, locationId);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

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
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(routeId)));
            }

            string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetRouteSummary);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryRouteDetailsforPdfGeneration"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

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

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

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
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(routeDetails)));
            }

            string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GenerateRouteLog);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GenerateRouteLog"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                RouteLogSummaryDTO routeLogSummary = new RouteLogSummaryDTO();
                routeLogSummary.Route = routeDetails;
                var routeLogSequencedPoints = await deliveryRouteDataService.GetSequencedRouteDetails(routeDetails.ID);
                routeLogSummary.RouteLogSequencedPoints = GenericMapper.MapList<RouteLogSequencedPointsDataDTO, RouteLogSequencedPointsDTO>(routeLogSequencedPoints);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return routeLogSummary;
            }
        }

        /// <summary>
        /// Get route details specific to postcode
        /// </summary>
        /// <param name="postCodeUnit">Post code</param>
        /// <param name="locationId">selected unit's location ID</param>
        /// <returns>List of routes</returns>
        public async Task<List<RouteDTO>> GetPostCodeSpecificRoutes(string postCodeUnit, Guid locationId)
        {
            if (string.IsNullOrEmpty(postCodeUnit))
            {
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(postCodeUnit)));
            }
            if (locationId == Guid.Empty)
            {
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(locationId)));
            }

            string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetPostCodeSpecificRoutes);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostCodeSpecificRoutes"))
            {
                List<RouteDTO> routes = new List<RouteDTO>();
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var postCode = await postCodeDataService.GetPostCode(postCodeUnit);
                var routeDetails = await deliveryRouteDataService.GetRoutesByLocation(locationId);

                if (routeDetails != null && routeDetails.Count > 0)
                {
                    if (postCode != null)
                    {
                        foreach (var route in routeDetails)
                        {
                            if (route.ID == postCode.PrimaryRouteGUID)
                            {
                                routes.Add(new RouteDTO { ID = route.ID, RouteName = DeliveryRouteConstants.PrimaryRoute + route.RouteName });
                            }
                            else if (route.ID == postCode.SecondaryRouteGUID)
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

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

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
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(routeId)));
            }
            if (routeId == Guid.Empty)
            {
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(routeId)));
            }

            string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(SaveDeliveryPointRouteMapping);
            using (loggingHelper.RMTraceManager.StartTrace("Business.SaveDeliveryPointRouteMapping"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                blockSequenceDataService.SaveDeliveryPointRouteMapping(routeId, deliveryPointId);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
            }
        }

        /// <summary>
        /// Get route details mapped to delivery point
        /// </summary>
        /// <param name="deliveryPointId">Delivery Point Id</param>
        /// <returns>Route Details</returns>
        public async Task<RouteDTO> GetRouteByDeliverypoint(Guid deliveryPointId)
        {
            if (deliveryPointId == Guid.Empty)
            {
                throw new ArgumentNullException(string.Format(ErrorConstants.Err_ArgumentmentNullException, nameof(deliveryPointId)));
            }

            string methodName = typeof(DeliveryRouteBusinessService) + "." + nameof(GetRouteByDeliverypoint);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRouteByDeliverypoint"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var route = GenericMapper.Map<RouteDataDTO, RouteDTO>(await deliveryRouteDataService.GetRouteByDeliverypoint(deliveryPointId));

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return route;
            }
        }
    }
}