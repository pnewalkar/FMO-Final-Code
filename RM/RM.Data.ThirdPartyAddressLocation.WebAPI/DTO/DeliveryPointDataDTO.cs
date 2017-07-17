namespace RM.Data.ThirdPartyAddressLocation.WebAPI.DTO
{
    using System;

    public class DeliveryPointDataDTO
    {
        public Guid ID { get; private set; }

        public Guid PostalAddressID { get; private set; }

        public short? MultipleOccupancyCount { get; private set; }

        public int? MailVolume { get; private set; }

        public Guid DeliveryPointUseIndicatorGUID { get; private set; }

        public byte[] RowVersion { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }

        public PostalAddressDataDTO PostalAddress { get; private set; }
    }
}
