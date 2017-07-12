using System;

namespace RM.DataManagement.PostalAddress.WebAPI.DTO
{
    public class DeliveryPointDBDTO
    {
        public Guid ID { get; set; }

        public Guid PostalAddressID { get; set; }

        public short? MultipleOccupancyCount { get; set; }

        public int? MailVolume { get; set; }

        public Guid DeliveryPointUseIndicatorGUID { get; set; }

        public byte[] RowVersion { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual PostalAddressDBDTO PostalAddress { get; set; }
    }
}
