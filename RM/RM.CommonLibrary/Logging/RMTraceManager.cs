using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace RM.CommonLibrary.LoggingMiddleware
{
    public class RMTraceManager : TraceManager, IRMTraceManager
    {
        public RMTraceManager(LogWriter logWriter) : base(logWriter)
        {
        }
    }
}