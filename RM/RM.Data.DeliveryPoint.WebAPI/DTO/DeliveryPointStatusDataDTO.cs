namespace RM.Data.DeliveryPoint.WebAPI.DTO
{
    using System;
    public class DeliveryPointStatusDataDTO
    {
        public Guid ID { get; set; }

        public Guid LocationGUID { get; set; }

        public Guid OperationalStatusGUID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }
    }
}
