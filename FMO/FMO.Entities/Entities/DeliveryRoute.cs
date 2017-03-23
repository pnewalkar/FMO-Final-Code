namespace FMO.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.DeliveryRoute")]
    public partial class DeliveryRoute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryRoute()
        {
            DeliveryRouteActivities = new HashSet<DeliveryRouteActivity>();
            Blocks = new HashSet<Block>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

        public virtual Scenario Scenario { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }

        public virtual ReferenceData ReferenceData2 { get; set; }

        public virtual ReferenceData ReferenceData3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRouteActivity> DeliveryRouteActivities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Block> Blocks { get; set; }
    }
}
