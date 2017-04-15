namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.DeliveryRouteNetworkLink")]
    public partial class DeliveryRouteNetworkLink
    {
        public int? DeliveryRouteActivity_Id { get; set; }

        public int? NetworkLink_Id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LinkOrderIndex { get; set; }

        public Guid ID { get; set; }

        public Guid? DeliveryRouteActivity_GUID { get; set; }

        public Guid? NetworkLink_GUID { get; set; }

        public virtual DeliveryRouteActivity DeliveryRouteActivity { get; set; }

        public virtual NetworkLink NetworkLink { get; set; }
    }
}
