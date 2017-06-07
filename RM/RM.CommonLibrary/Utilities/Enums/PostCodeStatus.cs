using System.ComponentModel;

namespace RM.CommonLibrary.HelperMiddleware
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