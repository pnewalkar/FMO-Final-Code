namespace RM.Data.DeliveryPoint.WebAPI.DTO
{
    using System;
    public class DeliveryPointStatusDatabaseDTO
    {
        public Guid ID { get; set; }

        public Guid LocationGUID { get; set; }

        public Guid OperationalStatusGUID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public DeliveryPointDatabaseDTO DeliveryPoint { get; set; }
    }
}
