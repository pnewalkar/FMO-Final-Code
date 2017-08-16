using System;

namespace RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO
{
    public class LocationRelationshipDataDTO
    {
        public Guid ID { get; set; }

        public Guid LocationID { get; set; }

        public Guid RelatedLocationID { get; set; }

        public Guid RelationshipTypeGUID { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual LocationDataDTO Location { get; set; }

        public virtual LocationDataDTO Location1 { get; set; }
    }
}