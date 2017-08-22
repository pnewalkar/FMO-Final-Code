using System.Collections.Generic;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.Entities
{
    public class DeliveryPointGroupData
    {
        public DeliveryPoint GroupCentroidDeliveryPoint { get; set; }

        public SupportingDeliveryPoint DeliveryGroup { get; set; }

        public List<Location> AddedDeliveryPoints { get; set; }

        public Location GroupBoundary { get; set; }

        public Location GroupCentroid { get; set; }
    }
}