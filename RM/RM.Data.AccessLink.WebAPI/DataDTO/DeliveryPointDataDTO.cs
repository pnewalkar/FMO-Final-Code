using System;

namespace RM.Data.AccessLink.WebAPI.DataDTOs
{
    public class DeliveryPointDataDTO
    {
        /// <summary>
        /// This class represents data transfer object for DeliveryPoint entity
        /// </summary>
        public Guid ID { get; set; }

        public Guid PostalAddressID { get; set; }

        public short? MultipleOccupancyCount { get; set; }

        public int? MailVolume { get; set; }

        public Guid DeliveryPointUseIndicatorGUID { get; set; }

        public byte[] RowVersion { get; set; }

        public DateTime RowCreateDateTime { get; set; }
    }
}