using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;


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
