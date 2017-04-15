namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.DeliveryRouteBlock")]
    public partial class DeliveryRouteBlock
    {
        public int Block_Id { get; set; }

        public int DeliveryRoute_Id { get; set; }

        public Guid ID { get; set; }

        public Guid DeliveryRoute_GUID { get; set; }

        public Guid Block_GUID { get; set; }

        public virtual Block Block { get; set; }

        public virtual DeliveryRoute DeliveryRoute { get; set; }
    }
}
