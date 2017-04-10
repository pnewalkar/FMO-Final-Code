namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.DeliveryRouteActivity")]
    public partial class DeliveryRouteActivity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryRouteActivity()
        {
            DeliveryRouteNetworkLinks = new HashSet<DeliveryRouteNetworkLink>();
        }

        public int DeliveryRouteActivity_Id { get; set; }

        public int? DeliveryRoute_Id { get; set; }

        public int? ActivityType_Id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RouteActivityOrderIndex { get; set; }

        public int? Block_Id { get; set; }

        public int? OperationalObjectType_Id { get; set; }

        public int? OperationalObject_id { get; set; }

        public int? DeliveryGroup_Id { get; set; }

        public Guid ID { get; set; }

        public Guid? DeliveryRoute_GUID { get; set; }

        public Guid? ActivityType_GUID { get; set; }

        public Guid? Block_GUID { get; set; }

        public Guid? OperationalObjectType_GUID { get; set; }

        public Guid? DeliveryGroup_GUID { get; set; }

        public virtual Block Block { get; set; }

        public virtual DeliveryGroup DeliveryGroup { get; set; }

        public virtual DeliveryRoute DeliveryRoute { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRouteNetworkLink> DeliveryRouteNetworkLinks { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }
    }
}