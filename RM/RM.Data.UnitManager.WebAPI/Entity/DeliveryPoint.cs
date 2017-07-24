namespace RM.DataManagement.UnitManager.WebAPI.Entity
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
    }
}