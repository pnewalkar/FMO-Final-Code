using System;
using System.Collections.Generic;

namespace RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO
{
    public class DeliveryPointGroupDataDTO
    {
        public DeliveryPointGroupDataDTO()
        {
            DeliveryGroup = new SupportingDeliveryPointDataDTO();
            AddedDeliveryPoints = new List<LocationDataDTO>();
            GroupBoundary = new DataDTO.LocationDataDTO();
        }

        public SupportingDeliveryPointDataDTO DeliveryGroup { get; set; }

        public List<LocationDataDTO> AddedDeliveryPoints { get; set; }

        public LocationDataDTO GroupBoundary { get; set; }

        public Guid RelationshipTypeForCentroidToBoundaryGUID { get; set; }

        public Guid RelationshipTypeForCentroidToDeliveryPointGUID { get; set; }

        public Guid DeliveryPointUseIndicatorGUID { get; set; }

        public Guid DeliveryGroupStatusGUID { get; set; }

        public Guid NetworkNodeType { get; set; }
    }
}