using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace RM.CommonLibrary.HelperMiddleware
{
    /// <summary>
    /// Define type of User Unit
    /// </summary>
    public enum UserUnit
    {
        /// <summary>
        /// Delivery Unit
        /// </summary>
        [Description("Delivery Unit")]
        DeliveryUnit = 1,

        /// <summary>
        /// Collection Unit
        /// </summary>
        [Description("Collection Unit")]
        CollectionUnit = 2,

        /// <summary>
        /// National
        /// </summary>
        [Description("National")]
        National = 3
    }
}
