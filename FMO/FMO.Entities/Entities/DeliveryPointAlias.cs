namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.DeliveryPointAlias")]
    public partial class DeliveryPointAlias
    {
        public Guid ID { get; set; }

        public Guid DeliveryPoint_GUID { get; set; }

        [Required]
        [StringLength(60)]
        public string DPAlias { get; set; }

        public bool Preferred { get; set; }

        public virtual DeliveryPoint DeliveryPoint { get; set; }
    }
}