namespace RM.Data.DeliveryPoint.WebAPI.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class LocationDatabaseDTO
    {
        public Guid ID { get; set; }

        public int AlternateID { get; set; }

        public DbGeometry Shape { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public NetworkNodeDatabaseDTO NetworkNode { get; set; }
    }
}
