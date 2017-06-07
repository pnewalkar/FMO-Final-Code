using System.ComponentModel;

namespace RM.CommonLibrary.HelperMiddleware
{
    /// <summary>
    /// Define type of ExceptionHandlingPolicy
    /// </summary>
    public enum ExceptionHandlingPolicy
    {
        None = 0,

        /// <summary>
        /// ExceptionShielding
        /// </summary>
        [Description("ExceptionShielding")]
        ExceptionShielding = 1,

        /// <summary>
        /// LoggingAndReplacingException
        /// </summary>
        [Description("LoggingAndReplacingException")]
        LoggingAndReplacingException = 2,

        /// <summary>
        /// LogAndWrap
        /// </summary>
        [Description("LogAndWrap")]
        LogAndWrap = 3
    }
}