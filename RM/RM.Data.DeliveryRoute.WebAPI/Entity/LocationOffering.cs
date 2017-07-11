using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.LocationOffering")]
    public partial class LocationOffering
    {
        public Guid ID { get; private set; }

        public Guid LocationID { get; private set; }

        public Guid OfferingID { get; private set; }

        public DateTime? StartDateTime { get; private set; }

        public DateTime? EndDateTime { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }

        public virtual Location Location { get; private set; }

        public virtual Offering Offering { get; private set; }
    }
}