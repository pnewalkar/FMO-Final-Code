namespace RM.CommonLibrary.EntityFramework.Entities
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
            DeliveryPointStatus = new HashSet<DeliveryPointStatus>();
        }

        public Guid ID { get; set; }

        public bool AccessLinkPresent { get; set; }

        public short? MultipleOccupancyCount { get; set; }

        public int? MailVolume { get; set; }

        public bool IsUnit { get; set; }

        public Guid Address_GUID { get; set; }

        public Guid DeliveryPointUseIndicator_GUID { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime RowCreateDateTime { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }

        public virtual PostalAddress PostalAddress { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryPointStatus> DeliveryPointStatus { get; set; }
    }
}
