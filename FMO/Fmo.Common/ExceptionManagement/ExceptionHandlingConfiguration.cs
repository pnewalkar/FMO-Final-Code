using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Fmo.Common.ExceptionManagement
{
    /// <summary>
    /// Exception Handling Configuration for programatically configuring exception policies
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
    public static class ExceptionHandlingConfiguration
    {
        /// <summary>
        /// Builds the exception handling configuration.
        /// </summary>
        /// <param name="logWriter">The log writer.</param>
        /// <returns>ExceptionManager</returns>
        public static ExceptionManager BuildExceptionHandlingConfiguration(LogWriter logWriter)
        {
            ResourceManager resxManager = new ResourceManager(ConfigurationManager.AppSettings["FmoMessages_ResourceFileName"], Assembly.GetExecutingAssembly());
            var policies = new List<ExceptionPolicyDefinition>();
            var exceptionShielding = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(
                    typeof(Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new WrapHandler(
                           resxManager.GetString("GenralErrorMessage"),
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
                           resxManager.GetString("LogCategory"),
                       Convert.ToInt32(resxManager.GetString("EventID")),
                       TraceEventType.Error,
                         resxManager.GetString("EventLogTitle"),
                         5,
                         typeof(TextExceptionFormatter),
                         logWriter),
                       new ReplaceHandler(
                           resxManager.GetString("LogAndThrowErrorMessage"),
                         typeof(Exception))
                     }),
                new ExceptionPolicyEntry(
                    typeof(BusinessLogicException),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new ReplaceHandler(
                           resxManager.GetString("GenralErrorMessage"),
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
                          resxManager.GetString("LogCategory"),
                       Convert.ToInt32(resxManager.GetString("EventID")),
                       TraceEventType.Error,
                         resxManager.GetString("EventLogTitle"),
                         5,
                         typeof(TextExceptionFormatter),
                         logWriter),
                       new WrapHandler(
                           resxManager.GetString("GenralErrorMessage"),
                         typeof(Exception))
                     }),
                new ExceptionPolicyEntry(
                    typeof(BusinessLogicException),
                    PostHandlingAction.NotifyRethrow,
                    new IExceptionHandler[]
                     {
                        new WrapHandler(
                            resxManager.GetString("LogAndThrowErrorMessage"),
                         typeof(BusinessLogicException)),
                       new LoggingExceptionHandler(
                           resxManager.GetString("LogCategory"),
                           Convert.ToInt32(resxManager.GetString("EventID")),
                           TraceEventType.Error,
                         resxManager.GetString("EventLogTitle"),
                         5,
                         typeof(TextExceptionFormatter),
                         logWriter)
                     })
            };

            policies.Add(new ExceptionPolicyDefinition(resxManager.GetString("Policy_ExceptionShielding"), exceptionShielding));
            policies.Add(new ExceptionPolicyDefinition(resxManager.GetString("Policy_LoggingAndReplacingException"), loggingAndReplacing));
            policies.Add(new ExceptionPolicyDefinition(resxManager.GetString("Policy_LogAndWrap"), logAndWrap));
            return new ExceptionManager(policies);
        }
    }
}