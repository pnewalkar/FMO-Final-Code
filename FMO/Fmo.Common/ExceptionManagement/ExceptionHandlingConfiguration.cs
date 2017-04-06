using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Fmo.Common.ExceptionManagement
{
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
    public class ExceptionHandlingConfiguration
    {
        private ExceptionHandlingConfiguration()
        {
        }

        public static ExceptionManager BuildExceptionHandlingConfiguration(LogWriter logWriter)
        {
            var policies = new List<ExceptionPolicyDefinition>();
            var exceptionShielding = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(
                    typeof(Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new WrapHandler(
                           "Application Error. Please contact your administrator.",
                         typeof(Exception))
                     })
            };

            var loggingAndReplacing = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(
                    typeof(Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new LoggingExceptionHandler(
                           "General",
                       9000,
                       TraceEventType.Error,
                         "FMO Exception",
                         5,
                         typeof(TextExceptionFormatter),
                         logWriter),
                       new ReplaceHandler(
                           "An application error occurred and has been logged. Please contact your administrator.",
                         typeof(Exception))
                     }),
                new ExceptionPolicyEntry(
                    typeof(BusinessLogicException),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new ReplaceHandler(
                           "An application error has occurred.Please contact your administrator.",
                         typeof(BusinessLogicException))
                     })
            };

            var logAndWrap = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(
                    typeof(Exception),
                    PostHandlingAction.NotifyRethrow,
                    new IExceptionHandler[]
                     {
                       new LoggingExceptionHandler(
                           "General",
                       9002,
                       TraceEventType.Error,
                         "FMO Exception",
                         5,
                         typeof(TextExceptionFormatter),
                         logWriter),
                       new WrapHandler(
                           "Application Error. Please contact your administrator.",
                         typeof(Exception))
                     }),
                new ExceptionPolicyEntry(
                    typeof(BusinessLogicException),
                    PostHandlingAction.NotifyRethrow,
                    new IExceptionHandler[]
                     {
                        new WrapHandler(
                            "An application error has occurred.Please contact your administrator.",
                         typeof(BusinessLogicException)),
                       new LoggingExceptionHandler(
                           "General",
                           9002,
                           TraceEventType.Error,
                         "FMO Exception",
                         5,
                         typeof(TextExceptionFormatter),
                         logWriter)
                     })
            };

            policies.Add(new ExceptionPolicyDefinition("ExceptionShielding", exceptionShielding));
            policies.Add(new ExceptionPolicyDefinition("LoggingAndReplacingException", loggingAndReplacing));
            policies.Add(new ExceptionPolicyDefinition("LogAndWrap", logAndWrap));
            return new ExceptionManager(policies);
        }
    }
}