using System.Collections.Generic;

namespace RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO
{
    public class DeliveryPointGroupDataDTO
    {

        public NetworkNodeDataDTO groupCentroidNetworkNode { get; set; }

        public DeliveryPointDataDTO groupCentroidDeliveryPoint { get; set; }

        public SupportingDeliveryPointDataDTO groupDetails { get; set; }

        public List<LocationDataDTO> AddedDeliveryPoints { get; set; }

        public LocationDataDTO GroupBoundary { get; set; }
    }
}