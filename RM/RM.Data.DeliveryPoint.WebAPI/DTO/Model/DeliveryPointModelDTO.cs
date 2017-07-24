using System;

namespace RM.Data.DeliveryPoint.WebAPI.DTO.Model
{
    public class DeliveryPointModelDTO
    {
        public int? UDPRN { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? XCoordinate { get; set; }

        public decimal? YCoordinate { get; set; }

        public byte[] RowVersion { get; set; }

        public Guid ID { get; set; }
    }
}