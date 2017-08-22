using System;
using System.Collections.Generic;

namespace RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO
{
    public class DeliveryPointDataDTO
    {
        public DeliveryPointDataDTO()
        {
            DeliveryPointStatus = new List<DeliveryPointStatusDataDTO>();
        }

        public Guid ID { get; set; }

        public Guid? PostalAddressID { get; set; }

        public short? MultipleOccupancyCount { get; set; }

        public int? MailVolume { get; set; }

        public Guid DeliveryPointUseIndicatorGUID { get; set; }

        public byte[] RowVersion { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public NetworkNodeDataDTO NetworkNode { get; set; }

        public PostalAddressDataDTO PostalAddress { get; set; }

        public ICollection<DeliveryPointStatusDataDTO> DeliveryPointStatus { get; set; }

        public SupportingDeliveryPointDataDTO SupportingDeliveryPoint { get; set; }
    }
}