namespace RM.Data.DeliveryPoint.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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

        public virtual Offering Offering { get; set; }
    }
}
