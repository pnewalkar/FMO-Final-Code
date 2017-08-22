using System.ComponentModel;

namespace RM.CommonLibrary.HelperMiddleware
{
    public enum OtherLayersType
    {
        [Description("accesslink")]
        AccessLink,

        [Description("deliverypoint")]
        DeliveryPoint,

        [Description("roadlink")]
        RoadLink,

        [Description("group")]
        Group
    }
}