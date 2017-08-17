using System;

namespace RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO
{
    public class LocationOfferingDataDTO
    {
        public Guid ID { get; set; }

        public Guid LocationID { get; set; }

        public Guid OfferingID { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public LocationDataDTO Location { get; set; }
    }
}