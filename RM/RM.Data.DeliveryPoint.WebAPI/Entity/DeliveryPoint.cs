namespace RM.Data.DeliveryPoint.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.DeliveryPoint")]
    public partial class DeliveryPoint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Auto Generated")]
        public DeliveryPoint()
        {
            DeliveryPointStatus = new HashSet<DeliveryPointStatus>();
        }

        public Guid ID { get; set; }

        public Guid PostalAddressID { get; set; }

        public short? MultipleOccupancyCount { get; set; }

        public int? MailVolume { get; set; }

        public Guid DeliveryPointUseIndicatorGUID { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }

        public virtual PostalAddress PostalAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<DeliveryPointStatus> DeliveryPointStatus { get; set; }

        public virtual SupportingDeliveryPoint SupportingDeliveryPoint { get; set; }
    }
}