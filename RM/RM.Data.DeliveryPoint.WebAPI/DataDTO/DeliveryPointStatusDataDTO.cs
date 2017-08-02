namespace RM.Data.DeliveryPoint.WebAPI.DataDTO
{
    using System;

    public class DeliveryPointStatusDataDTO
    {
        public Guid ID { get; set; }

        public Guid LocationID { get; set; }

        public Guid DeliveryPointStatusGUID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }
    }
}