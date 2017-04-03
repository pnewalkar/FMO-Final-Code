using System.ComponentModel;

namespace Fmo.Common.Enums
{
    /// <summary>
    /// Define type of ExceptionHandlingPolicy
    /// </summary>
    public enum ExceptionHandlingPolicy
    {
        None = 0,

        /// <summary>
        /// JustThrowException
        /// </summary>
        [Description("JustThrowException")]
        JustThrowException = 1,

        /// <summary>
        /// LoggAndReplaceException
        /// </summary>
        [Description("LoggAndReplaceException")]
        LoggAndReplaceException = 2,

        /// <summary>
        /// LogAndWrapException
        /// </summary>
        [Description("LogAndWrapException")]
        LogAndWrapException = 3
    }
}
