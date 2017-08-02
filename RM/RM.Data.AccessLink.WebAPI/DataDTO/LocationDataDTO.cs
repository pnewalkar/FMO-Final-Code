using System;
using System.Data.Entity.Spatial;

namespace RM.Data.AccessLink.WebAPI.DataDTOs
{
    public class LocationDataDTO
    {
        public Guid ID { get; set; }

        public int AlternateID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public DbGeometry Shape { get; set; }
    }
}