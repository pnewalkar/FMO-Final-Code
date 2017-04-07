namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.DeliveryRoute")]
    public partial class DeliveryRoute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryRoute()
        {
            DeliveryRouteBlocks = new HashSet<DeliveryRouteBlock>();
            DeliveryRouteActivities = new HashSet<DeliveryRouteActivity>();
        }

        public int DeliveryRoute_Id { get; set; }

        public int? ExternalId { get; set; }

        [StringLength(30)]
        public string RouteName { get; set; }

        [StringLength(10)]
        public string RouteNumber { get; set; }

        public int OperationalStatus_Id { get; set; }

        public int RouteMethodType_Id { get; set; }

        public int? TravelOutTransportType_Id { get; set; }

        public int? TravelInTransportType_Id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TravelOutTimeMin { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TravelInTimeMin { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SpanTimeMin { get; set; }

        public int? DeliveryScenario_Id { get; set; }

        [StringLength(20)]
        public string DeliveryRouteBarcode { get; set; }

        public Guid ID { get; set; }

        public Guid OperationalStatus_GUID { get; set; }

        public Guid RouteMethodType_GUID { get; set; }

        public Guid? TravelOutTransportType_GUID { get; set; }

        public Guid? TravelInTransportType_GUID { get; set; }

        public Guid? DeliveryScenario_GUID { get; set; }

        public virtual Scenario Scenario { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRouteBlock> DeliveryRouteBlocks { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }

        public virtual ReferenceData ReferenceData2 { get; set; }

        public virtual ReferenceData ReferenceData3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRouteActivity> DeliveryRouteActivities { get; set; }
    }
}