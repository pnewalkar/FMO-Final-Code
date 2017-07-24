using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using RM.CommonLibrary.EntityFramework.Utilities.ReferenceData;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.DataManagement.NetworkManager.WebAPI.IntegrationService
{
    public class NetworkManagerIntegrationService : INetworkManagerIntegrationService
    {
        #region Property Declarations
        private const string ReferenceDataWebAPIName = "ReferenceDataWebAPIName";
        private string referenceDataWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        private int priority = LoggerTraceConstants.NetworkManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.NetworkManagerIntegrationServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.NetworkManagerIntegrationServiceMethodExitEventId;

        #endregion Property Declarations

        #region Constructors
        public NetworkManagerIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(ReferenceDataWebAPIName).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
        }
        #endregion Constructors

        #region Public Methods

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<ReferenceDataCategoryDTO>> GetReferenceDataNameValuePairs(List<string> categoryNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataNameValuePairs"))
            {
                string methodName = typeof(NetworkManagerIntegrationService) + "." + nameof(GetReferenceDataNameValuePairs);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<ReferenceDataCategoryDTO> listReferenceCategories = new List<ReferenceDataCategoryDTO>();
                List<NameValuePair> nameValuePairs = new List<NameValuePair>();
                foreach (var category in categoryNames)
                {
                    HttpResponseMessage result = await httpHandler.GetAsync(referenceDataWebAPIName + "nameValuePairs?appGroupName=" + category);
                    if (!result.IsSuccessStatusCode)
                    {
                        var responseContent = result.ReasonPhrase;
                        throw new ServiceException(responseContent);
                    }

                    Tuple<string, List<NameValuePair>> apiResult = JsonConvert.DeserializeObject<Tuple<string, List<NameValuePair>>>(result.Content.ReadAsStringAsync().Result);
                    nameValuePairs.AddRange(apiResult.Item2);
                }

                listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(nameValuePairs));
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return listReferenceCategories;
            }
        }

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataSimpleLists"))
            {
                string methodName = typeof(NetworkManagerIntegrationService) + "." + nameof(GetReferenceDataSimpleLists);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<ReferenceDataCategoryDTO> listReferenceCategories = new List<ReferenceDataCategoryDTO>();

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(referenceDataWebAPIName + "simpleLists", listNames);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<SimpleListDTO> apiResult = JsonConvert.DeserializeObject<List<SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);
                listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(apiResult));
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return listReferenceCategories;
            }
        }

        #endregion Public Methods

    }
}