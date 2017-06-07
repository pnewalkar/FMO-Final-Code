namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.RMGDeliveryPoint")]
    public partial class RMGDeliveryPoint
    {
        public DbGeometry LocationXY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Longitude { get; set; }

        public Guid ID { get; set; }

        public Guid DeliveryPoint_GUID { get; set; }

        public virtual AccessLink AccessLink { get; set; }

        public virtual DeliveryPoint DeliveryPoint { get; set; }
    }
}
