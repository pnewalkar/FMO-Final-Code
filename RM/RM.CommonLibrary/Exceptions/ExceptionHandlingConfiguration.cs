using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.ExceptionMiddleware
{
    /// <summary>
    /// Exception Handling Configuration for programatically configuring exception policies
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
    public static class ExceptionHandlingConfiguration
    {
        #region Public Methods

        /// <summary>
        /// Builds the exception handling configuration.
        /// </summary>
        /// <param name="logWriter">The log writer.</param>
        /// <returns>ExceptionManager</returns>
        public static ExceptionManager BuildExceptionHandlingConfiguration(LogWriter logWriter)
        {
            //ResourceManager resxManager = new ResourceManager(ConfigurationManager.AppSettings["FmoMessages_ResourceFileName"], Assembly.GetExecutingAssembly());
            var policies = new List<ExceptionPolicyDefinition>();
            var exceptionShielding = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(
                    typeof(Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new WrapHandler(
                           ErrorConstants.GenralErrorMessage,
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
                           ErrorConstants.LogCategory,
                       Convert.ToInt32(ErrorConstants.EventID),
                       TraceEventType.Error,
                         ErrorConstants.EventLogTitle,
                         5,
                         typeof(TextExceptionFormatter),
                         logWriter),
                       new ReplaceHandler(
                           ErrorConstants.LogAndThrowErrorMessage,
                         typeof(Exception))
                     }),
                new ExceptionPolicyEntry(
                    typeof(BusinessLogicException),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new ReplaceHandler(
                           ErrorConstants.GenralErrorMessage,
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
                          ErrorConstants.LogCategory,
                       Convert.ToInt32(ErrorConstants.EventID),
                       TraceEventType.Error,
                         ErrorConstants.EventLogTitle,
                         5,
                         typeof(TextExceptionFormatter),
                         logWriter),
                       new WrapHandler(
                           ErrorConstants.GenralErrorMessage,
                         typeof(Exception))
                     }),
                new ExceptionPolicyEntry(
                    typeof(BusinessLogicException),
                    PostHandlingAction.NotifyRethrow,
                    new IExceptionHandler[]
                     {
                        new WrapHandler(
                            ErrorConstants.LogAndThrowErrorMessage,
                         typeof(BusinessLogicException)),
                       new LoggingExceptionHandler(
                           ErrorConstants.LogCategory,
                           Convert.ToInt32(ErrorConstants.EventID),
                           TraceEventType.Error,
                         ErrorConstants.EventLogTitle,
                         5,
                         typeof(TextExceptionFormatter),
                         logWriter)
                     })
            };

            policies.Add(new ExceptionPolicyDefinition(ErrorConstants.Policy_ExceptionShielding, exceptionShielding));
            policies.Add(new ExceptionPolicyDefinition(ErrorConstants.Policy_LoggingAndReplacingException, loggingAndReplacing));
            policies.Add(new ExceptionPolicyDefinition(ErrorConstants.Policy_LogAndWrap, logAndWrap));
            return new ExceptionManager(policies);
        }

        #endregion Public Methods
    }
}