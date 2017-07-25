using System;

namespace RM.Data.DeliveryPoint.WebAPI.DTO.Model
{
    public class UpdateDeliveryPointModelDTO
    {
        public decimal? XCoordinate { get; set; }

        public decimal? YCoordinate { get; set; }

        public Guid ID { get; set; }
    }
}