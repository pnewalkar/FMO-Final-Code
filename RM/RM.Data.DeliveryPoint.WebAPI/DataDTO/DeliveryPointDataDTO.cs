namespace RM.Data.DeliveryPoint.WebAPI.DataDTO
{
    using System;
    using System.Collections.Generic;

    public class DeliveryPointDataDTO
    {
        public DeliveryPointDataDTO()
        {
            this.DeliveryPointStatus = new List<DeliveryPointStatusDataDTO>();
            this.PostalAddress = new PostalAddressDataDTO();
            this.NetworkNode = new NetworkNodeDataDTO();
            this.SupportingDeliveryPoint = new SupportingDeliveryPointDataDTO();
        }

        public Guid ID { get; set; }

        public Guid PostalAddressID { get; set; }

        public short? MultipleOccupancyCount { get; set; }

        public int? MailVolume { get; set; }

        public Guid DeliveryPointUseIndicatorGUID { get; set; }

        public byte[] RowVersion { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual NetworkNodeDataDTO NetworkNode { get; set; }

        public virtual PostalAddressDataDTO PostalAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<DeliveryPointStatusDataDTO> DeliveryPointStatus { get; set; }

        public virtual SupportingDeliveryPointDataDTO SupportingDeliveryPoint { get; set; }
    }
}