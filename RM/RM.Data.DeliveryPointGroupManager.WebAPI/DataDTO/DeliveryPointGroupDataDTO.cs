using System.Collections.Generic;

namespace RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO
{
    public class DeliveryPointGroupDataDTO
    {
        public SupportingDeliveryPointDataDTO DeliveryGroup { get; set; }

        public List<LocationDataDTO> AddedDeliveryPoints { get; set; }

        public LocationDataDTO GroupBoundary { get; set; }
    }
}