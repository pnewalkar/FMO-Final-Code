namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.DeliveryRoutePostcode")]
    public partial class DeliveryRoutePostcode
    {
        public Guid ID { get; set; }

        public Guid DeliveryRoute_GUID { get; set; }

        public Guid Postcode_GUID { get; set; }

        public bool IsPrimaryRoute { get; set; }

        public virtual DeliveryRoute DeliveryRoute { get; set; }

        public virtual Postcode Postcode { get; set; }
    }
}
