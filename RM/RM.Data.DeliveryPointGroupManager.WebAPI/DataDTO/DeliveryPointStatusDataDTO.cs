using System;

namespace RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO
{
    public class DeliveryPointStatusDataDTO
    {
        public Guid ID { get; set; }

        public Guid LocationID { get; set; }

        public Guid DeliveryPointStatusGUID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public DeliveryPointDataDTO DeliveryPoint { get; set; }
    }
}