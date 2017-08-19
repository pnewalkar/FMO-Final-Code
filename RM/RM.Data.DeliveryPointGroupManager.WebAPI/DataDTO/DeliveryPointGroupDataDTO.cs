namespace RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO
{
    public class DeliveryPointGroupDataDTO
    {
        public LocationDataDTO groupBoundaryLocation { get; set; }

        public LocationDataDTO groupCentroidLocation { get; set; }

        public NetworkNodeDataDTO groupCentroidNetworkNode { get; set; }

        public DeliveryPointDataDTO groupCentroidDeliveryPoint { get; set; }

        public SupportingDeliveryPointDataDTO groupDetails { get; set; }

    }
}