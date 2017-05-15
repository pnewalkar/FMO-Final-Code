using System;

namespace Fmo.DTO.Model
{
    public class CreateDeliveryPointModelDTO
    {
        public string Message { get; set; }

        public Guid ID { get; set; }

        public byte[] RowVersion { get; set; }

        public bool IsAddressLocationAvailable { get; set; }
    }
}