namespace RM.Data.DeliveryPoint.WebAPI.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.DeliveryPointStatus")]
    public partial class DeliveryPointStatus
    {
        public Guid ID { get; set; }

        public Guid LocationID { get; set; }

        public Guid DeliveryPointStatusGUID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual DeliveryPoint DeliveryPoint { get; set; }
    }
}