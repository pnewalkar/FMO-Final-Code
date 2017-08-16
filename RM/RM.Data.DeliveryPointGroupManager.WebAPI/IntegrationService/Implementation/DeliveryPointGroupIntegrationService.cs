using RM.CommonLibrary.ConfigurationMiddleware;
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
    }
}