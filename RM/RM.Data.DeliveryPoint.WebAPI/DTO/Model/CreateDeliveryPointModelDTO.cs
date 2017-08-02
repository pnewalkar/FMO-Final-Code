using System;
using System.Collections.Generic;

namespace RM.Data.DeliveryPoint.WebAPI.DTO.Model
{
    public class CreateDeliveryPointModelDTO
    {
        public string Message { get; set; }

        public Guid ID { get; set; }

        public byte[] RowVersion { get; set; }

        public bool IsAddressLocationAvailable { get; set; }

        public double? XCoordinate { get; set; }

        public double? YCoordinate { get; set; }

        public List<PostalAddressDTO> PostalAddressDTOs { get; set; }
    }
}