namespace RM.Data.DeliveryPoint.WebAPI.DTO
{
    using System;
    using System.Collections.Generic;
    
    public class DeliveryPointDatabaseDTO
    {

        public Guid ID { get; set; }

        public bool AccessLinkPresent { get; set; }

        public short? MultipleOccupancyCount { get; set; }

        public int? MailVolume { get; set; }

        public bool IsUnit { get; set; }

        public Guid Address_GUID { get; set; }

        public Guid DeliveryPointUseIndicator_GUID { get; set; }

        public byte[] RowVersion { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public NetworkNodeDatabaseDTO NetworkNode { get; set; }     
           
        public ICollection<DeliveryPointStatusDatabaseDTO> DeliveryPointStatus { get; set; }
    }
}
