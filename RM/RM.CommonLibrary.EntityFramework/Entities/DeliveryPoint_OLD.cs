namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.DeliveryPoint_OLD")]
    public partial class DeliveryPoint_OLD
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryPoint_OLD()
        {
            AccessLinks = new HashSet<AccessLink>();
            DeliveryPointAlias = new HashSet<DeliveryPointAlias>();
            RMGDeliveryPoints = new HashSet<RMGDeliveryPoint>();
        }

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

        public bool IsUnit { get; set; }

        public int? Temp_DeliveryGroup_Id { get; set; }

        public Guid ID { get; set; }

        public Guid Address_GUID { get; set; }

        public Guid? LocationProvider_GUID { get; set; }

        public Guid? OperationalStatus_GUID { get; set; }

        public Guid? DeliveryGroup_GUID { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Guid DeliveryPointUseIndicator_GUID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessLink> AccessLinks { get; set; }

        public virtual DeliveryGroup DeliveryGroup { get; set; }

        public virtual PostalAddress_OLD PostalAddress_OLD { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }

        public virtual ReferenceData ReferenceData2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryPointAlias> DeliveryPointAlias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RMGDeliveryPoint> RMGDeliveryPoints { get; set; }
    }
}
