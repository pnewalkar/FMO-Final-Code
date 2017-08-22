using System.Collections.Generic;
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
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.Utils;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.Integration
{
    public class DeliveryPointGroupIntegrationService : IDeliveryPointGroupIntegrationService
    {
        #region Property Declarations

        private string referenceDataWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.DeliveryPointAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryPointGroupIntegrationServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryPointGroupIntegrationServiceMethodExitEventId;

        #endregion Property Declarations

        #region Constructor

        public DeliveryPointGroupIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.loggingHelper = loggingHelper;
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointGroupConstants.ReferenceDataWebAPIName).ToString() : string.Empty;
        }

        #endregion Constructor

        #region Methods

        public async Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"IntegrationService.{nameof(GetReferenceDataSimpleLists)}"))
            {
                string methodName = typeof(DeliveryPointGroupIntegrationService) + "." + nameof(GetReferenceDataSimpleLists);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> listReferenceCategories = new List<ReferenceDataCategoryDTO>();

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

        #endregion Methods
    }
}