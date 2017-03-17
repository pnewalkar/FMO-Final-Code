namespace Fmo.DataServices.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.DeliveryPoint")]
    public partial class DeliveryPoint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryPoint()
        {
            AccessLinks = new HashSet<AccessLink>();
            RMGDeliveryPoints = new HashSet<RMGDeliveryPoint>();
        }

        [Key]
        public int DeliveryPoint_Id { get; set; }

        public int Address_Id { get; set; }

        [StringLength(1)]
        public string LocationProvider { get; set; }

        [StringLength(1)]
        public string OperationalStatus { get; set; }

        public DbGeometry LocationXY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Longitude { get; set; }

        public bool Positioned { get; set; }

        public bool AccessLinkPresent { get; set; }

        public bool RMGDeliveryPointPresent { get; set; }

        public int? UDPRN { get; set; }

        public short? MultipleOccupancyCount { get; set; }

        public int? MailVolume { get; set; }

        [StringLength(1)]
        public string DeliveryPointUseIndicator { get; set; }

        public int? DeliveryGroup_Id { get; set; }

        public bool IsUnit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessLink> AccessLinks { get; set; }

        public virtual DeliveryGroup DeliveryGroup { get; set; }

        public virtual PostalAddress PostalAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RMGDeliveryPoint> RMGDeliveryPoints { get; set; }
    }
}
