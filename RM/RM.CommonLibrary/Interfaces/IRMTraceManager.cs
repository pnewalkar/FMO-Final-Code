using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace RM.CommonLibrary.LoggingMiddleware
{
    public interface IRMTraceManager
    {
        // Summary: For testing purpose
        LogWriter LogWriter { get; }

        // Summary: Initializes a new instance of the
        // Microsoft.Practices.EnterpriseLibrary.Logging.Tracer class with the given logical
        // operation name.
        //
        // Parameters: operation: The operation for the Microsoft.Practices.EnterpriseLibrary.Logging.Tracer
        Tracer StartTrace(string operation);

        // Summary: Initializes a new instance of the
        // Microsoft.Practices.EnterpriseLibrary.Logging.Tracer class with the given logical
        // operation name and activity id.
        //
        // Parameters: operation: The operation for the Microsoft.Practices.EnterpriseLibrary.Logging.Tracer
        //
        // activityId: The activity id
        Tracer StartTrace(string operation, Guid activityId);
    }
}