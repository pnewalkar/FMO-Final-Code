using System.ComponentModel;

namespace RM.CommonLibrary.HelperMiddleware
{
    public enum LoggingCategory
    {
        None = 0,

        /// <summary>
        /// The General
        /// </summary>
        [Description("General")]
        General = 1,

        /// <summary>
        /// The Exception
        /// </summary>
        [Description("Exception")]
        Exception = 2
    }
}