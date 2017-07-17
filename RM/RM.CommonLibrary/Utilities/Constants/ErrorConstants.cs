namespace RM.CommonLibrary.HelperMiddleware
{
    public class ErrorConstants
    {
        public const string EventID = "9002";
        public const string EventLogTitle = "RM Exception";
        public const string UserFriendlyErrorMessage = "An application error occurred and has been logged. Please contact your administrator.";
        public const string GenralErrorMessage = "Application Error. Please contact your administrator.";
        public const string LogAndThrowErrorMessage = "An application error occurred and has been logged. Please contact your administrator.";
        public const string LogCategory = "General";
        public const string Logging_LogName = "Application";
        public const string Logging_LogSource = "RM Logging";
        public const string Logging_TextFormat = "Timestamp: {timestamp(local)}{newline}Message: {message}{newline}Category: {category}{newline}Priority: {priority}{newline}EventId: {eventid}{newline}Severity: {severity}{newline}Title:{title}{newline}Machine: {localMachine}{newline}App Domain: {localAppDomain}{newline}ProcessId: {localProcessId}{newline}Process Name: {localProcessName}{newline}Thread Name: {threadName}{newline}Win32 ThreadId:{win32ThreadId}{newline}Extended Properties: {dictionary({key} - {value}{newline})}";
        public const string LogSource_LogSourceName = "General";
        public const string Policy_ExceptionShielding = "ExceptionShielding";
        public const string Policy_LogAndWrap = "LogAndWrap";
        public const string Policy_LoggingAndReplacingException = "LoggingAndReplacingException";

        public const string Err_ArgumentmentNullException = "Following arguments for the method are null {0}";
        public const string Err_Concurrency = "Data you are trying to modify has been already modified by another user.";
        public const string Err_Default = "Application error occured. Please contact your administrator.";
        public const string Err_EntityNotFoundException = "The entity trying to search cannot be found.";
        public const string Err_ExecutingErrorHandler = "An exception was thrown attempting to execute the error handler.";
        public const string Err_InvalidOperationExceptionForCountAsync = "The underlying provider does not support this operation.";
        public const string Err_InvalidOperationExceptionForFirstorDefault = "Source does not implement IDbAsyncQueryProvider";
        public const string Err_InvalidOperationExceptionForSingleorDefault = "There are more than one element in sequence for SingleOrDefault() extention";
        public const string Err_NotSupportedException = "An attempt was made to use unsupported behavior.";
        public const string Err_ObjectDisposedException = "The context or connection have been disposed.";
        public const string Err_OverflowException = "The number of elements in source is larger than MaxValue.";
        public const string Err_SqlAddException = "Error occured on adding new {0}.";
        public const string Err_SqlDeleteException = "Error occured while deleting {0}.";
        public const string Err_SqlUpdateException = "Error occured on modifying {0}.";
        public const string Err_Token = "Error occured while generating token";
        public const string Err_UnHandled = "An unhandled application error occurred and has been logged. Please contact your administrator.";
        public const string Err_MisMatchConfigFile = "Mismatch between reference configuration file and reference data tables.";
        public const string Error_NonZero = "Must be non-zero";
    }
}