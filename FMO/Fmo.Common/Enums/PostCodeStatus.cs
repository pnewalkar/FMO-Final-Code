using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.Common.Enums
{
    public enum PostCodeStatus
    {
        None = 0,

        [Description("Pending Delete in PAF")]
        PendingDeleteInPAF = 1,

        [Description("Live")]
        Live = 2,

        [Description("Pending Delete in FMO")]
        PendingDeleteInFMO
    }
}
