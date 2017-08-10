
using Ninject;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.HttpHandler;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RM.DataManagement.Batch.Delete
{
    public class DataBatchDelete 
    {
        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private const string DELETEPAFHOUSEKEEPING = "DeletePAFHousekeeping";


        public DataBatchDelete(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.configurationHelper = configurationHelper;
            this.loggingHelper = loggingHelper;
        }        

        public void BatchDelete()
        {
            using (loggingHelper.RMTraceManager.StartTrace("EXE.BatchDelete"))
            {
                try
                {
                    string methodName = typeof(DataBatchDelete) + "." + nameof(BatchDelete);
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodEntryEventId);
                    string pafDeleteHousekeepingUrl = configurationHelper.ReadAppSettingsConfigurationValues(DELETEPAFHOUSEKEEPING);
                    while (true)
                    {
                        Console.WriteLine("Press <ENTER> to start process");
                        Console.ReadLine();
                        httpHandler.DeleteAsync(pafDeleteHousekeepingUrl);
                    }
                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteControllerMethodExitEventId);
                }
                catch (Exception ex)
                {
                    loggingHelper.Log(ex, TraceEventType.Error);
                }
            }
        }

    }
}
