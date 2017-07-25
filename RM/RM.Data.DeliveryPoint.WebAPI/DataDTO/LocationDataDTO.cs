namespace RM.Data.DeliveryPoint.WebAPI.DataDTO
{
    using System;
    using System.Data.Entity.Spatial;

    public class LocationDataDTO
    {
        public Guid ID { get; set; }

        public int AlternateID { get; set; }

        public DbGeometry Shape { get; set; }

        public DateTime RowCreateDateTime { get; set; }
    }
}