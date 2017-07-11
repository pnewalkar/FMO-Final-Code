using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.DeliveryPoint")]
    public partial class DeliveryPoint
    {
        public Guid ID { get; private set; }

        public Guid PostalAddressID { get; private set; }

        public short? MultipleOccupancyCount { get; private set; }

        public int? MailVolume { get; private set; }

        public Guid DeliveryPointUseIndicatorGUID { get; private set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }

        public virtual NetworkNode NetworkNode { get; private set; }
    }
}