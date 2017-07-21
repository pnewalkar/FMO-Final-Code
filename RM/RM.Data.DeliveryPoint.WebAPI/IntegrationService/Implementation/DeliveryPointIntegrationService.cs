using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using RM.CommonLibrary.EntityFramework.Utilities.ReferenceData;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.Data.DeliveryPoint.WebAPI.DTO;
using RM.Data.DeliveryPoint.WebAPI.DTO.Model;
using RM.DataManagement.DeliveryPoint.WebAPI.Utils;

namespace RM.DataManagement.DeliveryPoint.WebAPI.Integration
{
    public class DeliveryPointIntegrationService : IDeliveryPointIntegrationService
    {
        #region Property Declarations

        private string referenceDataWebAPIName = string.Empty;
        private string postalAddressManagerWebAPIName = string.Empty;
        private string accessLinkWebAPIName = string.Empty;
        private string deliveryRouteManagerWebAPIName = string.Empty;
        private string unitManagerDataWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.DeliveryPointAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryPointIntegrationServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryPointIntegrationServiceMethodExitEventId;

        #endregion Property Declarations

        #region Constructor

        public DeliveryPointIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.loggingHelper = loggingHelper;
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointConstants.ReferenceDataWebAPIName).ToString() : string.Empty;
            this.accessLinkWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointConstants.AccessLinkWebAPIName).ToString() : string.Empty;
            this.postalAddressManagerWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointConstants.PostalAddressManagerWebAPIName).ToString() : string.Empty;
            this.deliveryRouteManagerWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointConstants.DeliveryRouteManagerWebAPIName).ToString() : string.Empty;
            this.unitManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointConstants.UnitManagerDataWebAPIName).ToString() : string.Empty;
        }

        #endregion Constructor

        #region public methods

        /// <summary>
        /// Retreive reference data details from
        /// </summary>
        /// <param name="categoryName">categoryname</param>
        /// <param name="itemName">Reference data Name</param>
        /// <returns>GUID</returns>
        public Guid GetReferenceDataGuId(string categoryName, string itemName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.GetReferenceDataGuId"))
            {
                string methodName = typeof(DeliveryPointIntegrationService) + "." + nameof(GetReferenceDataGuId);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string actionUrl = "/simplelists?listName=" + categoryName;
                string requestUrl = referenceDataWebAPIName + actionUrl;
                HttpResponseMessage result = httpHandler.GetAsync(requestUrl).Result;
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                Tuple<string, SimpleListDTO> simpleListDTO = JsonConvert.DeserializeObject<Tuple<string, SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);
                var getReferenceDataGuId = simpleListDTO.Item2.ListItems.Where(li => li.Value.Trim().Equals(itemName.Trim(), StringComparison.OrdinalIgnoreCase)).SingleOrDefault().Id;
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return getReferenceDataGuId;
            }
        }

        public bool CreateAccessLink(Guid operationalObjectId, Guid operationObjectTypeId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.CreateAccessLink"))
            {
                string methodName = typeof(DeliveryPointIntegrationService) + "." + nameof(CreateAccessLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                accessLinkWebAPIName = accessLinkWebAPIName + "AccessLink/" + operationalObjectId + "/" + operationObjectTypeId;
                HttpResponseMessage result = httpHandler.GetAsync(accessLinkWebAPIName).Result;
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                string returnvalue = result.Content.ReadAsStringAsync().Result;
                var createAccessLink = Convert.ToBoolean(returnvalue);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return createAccessLink;
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        public async Task<CreateDeliveryPointModelDTO> CreateAddressAndDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.CreateAddressAndDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointIntegrationService) + "." + nameof(CreateAddressAndDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(postalAddressManagerWebAPIName + "postaladdress/savedeliverypointaddress/", addDeliveryPointDTO);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                var createAddressAndDeliveryPoint = JsonConvert.DeserializeObject<CreateDeliveryPointModelDTO>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return createAddressAndDeliveryPoint;
            }
        }

        /// <summary>
        /// This method is used to check Duplicate NYB records
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress as input</param>
        /// <returns>string</returns>
        public async Task<string> CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.CheckForDuplicateNybRecords"))
            {
                string methodName = typeof(DeliveryPointIntegrationService) + "." + nameof(CheckForDuplicateNybRecords);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(postalAddressManagerWebAPIName + "postaladdress/nybduplicate/", objPostalAddress);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                var checkForDuplicateNybRecords = result.Content.ReadAsStringAsync().Result;
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return checkForDuplicateNybRecords;
            }
        }

        /// <summary>
        /// This method is used to check for Duplicate Address with Delivery Points.
        /// </summary>
        /// <param name="objPostalAddress">Postal Addess Dto as input</param>
        /// <returns>bool</returns>
        public async Task<bool> CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.CheckForDuplicateAddressWithDeliveryPoints"))
            {
                string methodName = typeof(DeliveryPointIntegrationService) + "." + nameof(CheckForDuplicateAddressWithDeliveryPoints);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(postalAddressManagerWebAPIName + "postaladdress/duplicatedeliverypoint/", objPostalAddress);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                var checkForDuplicateAddressWithDeliveryPoints = Convert.ToBoolean(result.Content.ReadAsStringAsync().Result);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return checkForDuplicateAddressWithDeliveryPoints;
            }
        }

        /// <summary>
        /// Method to map a route  for delivery point
        /// </summary>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <param name="deliveryPointId">deliveryPointId</param>
        /// <returns>bool</returns>
        public async Task<bool> MapRouteForDeliveryPoint(Guid deliveryRouteId, Guid deliveryPointId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.MapForDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointIntegrationService) + "." + nameof(MapRouteForDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(deliveryRouteManagerWebAPIName + "deliveryroute/deliverypoint/" + deliveryRouteId + "/" + deliveryPointId, string.Empty);
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                string returnvalue = result.Content.ReadAsStringAsync().Result;
                var mapRouteForDeliveryPointSuccess = Convert.ToBoolean(returnvalue);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return mapRouteForDeliveryPointSuccess;
            }
        }

        public async Task<List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.GetReferenceDataSimpleLists"))
            {
                string methodName = typeof(DeliveryPointIntegrationService) + "." + nameof(GetReferenceDataSimpleLists);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> listReferenceCategories = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>();

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(referenceDataWebAPIName + "/simpleLists", listNames);
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<SimpleListDTO> apiResult = JsonConvert.DeserializeObject<List<SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);

                listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(apiResult));

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return listReferenceCategories;
            }
        }

        /// <summary>
        /// This method is used to get route for delivery point.
        /// </summary>
        /// <param name="deliveryPointId">deliveryPointId as input</param>
        /// <returns>The Route details for the provided delivery point.</returns>
        public async Task<RouteDTO> GetRouteForDeliveryPoint(Guid deliveryPointId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.GetRouteForDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointIntegrationService) + "." + nameof(GetRouteForDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.GetAsync(deliveryRouteManagerWebAPIName + "deliveryroute/deliverypoint/" + deliveryPointId);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                RouteDTO route = JsonConvert.DeserializeObject<RouteDTO>(result.Content.ReadAsStringAsync().Result);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return route;
            }
        }

        // <summary>
        /// Gets approx location based on the postal code.
        /// </summary>
        /// <param name="postcode"></param>
        /// <returns>The approx location/</returns>
        public async Task<DbGeometry> GetApproxLocation(string postcode)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.GetApproxLocation"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(GetApproxLocation);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "postcode/approxlocation/" + postcode);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                // DbGeometry approxLocation = JsonConvert.DeserializeObject<DbGeometry>(result.Content.ReadAsStringAsync().Result);
                DBGeometryObj locationObject = JsonConvert.DeserializeObject<DBGeometryObj>(result.Content.ReadAsStringAsync().Result);
                DbGeometry approxLocation = locationObject.dbGeometry;
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return approxLocation;
            }
        }

        #endregion public methods
    }
}