namespace FMO.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.DeliveryRouteActivity")]
    public partial class DeliveryRouteActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeliveryRouteActivity_Id { get; set; }

        public int? DeliveryRoute_Id { get; set; }

        public int? ActivityType_Id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RouteActivityOrderIndex { get; set; }

        public int? Block_Id { get; set; }

        public int? OperationalObjectType_Id { get; set; }

        public int? OperationalObject_id { get; set; }

        public int? DeliveryGroup_Id { get; set; }

        public virtual Block Block { get; set; }

        public virtual DeliveryGroup DeliveryGroup { get; set; }

        public virtual DeliveryRoute DeliveryRoute { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }
    }
}
