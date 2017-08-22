namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.LocationOffering")]
    public partial class LocationOffering
    {
        public Guid ID { get; set; }

        public Guid LocationID { get; set; }

        public Guid OfferingID { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual Location Location { get; set; }
    }
}