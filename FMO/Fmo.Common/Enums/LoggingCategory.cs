using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.Common.Enums
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
