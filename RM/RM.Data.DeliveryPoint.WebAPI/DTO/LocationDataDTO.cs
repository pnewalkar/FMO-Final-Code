namespace RM.Data.DeliveryPoint.WebAPI.DTO
{
    using System;
    using System.Data.Entity.Spatial;

    public class LocationDataDTO
    {
        public Guid ID { get; set; }

        public int AlternateID { get; set; }

        public DbGeometry Shape { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public NetworkNodeDataDTO NetworkNode { get; set; }
    }
}