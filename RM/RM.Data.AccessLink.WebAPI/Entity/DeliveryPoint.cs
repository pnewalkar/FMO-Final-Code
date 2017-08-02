namespace RM.DataManagement.AccessLink.WebAPI.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.DeliveryPoint")]
    public partial class DeliveryPoint
    {
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
    }
}