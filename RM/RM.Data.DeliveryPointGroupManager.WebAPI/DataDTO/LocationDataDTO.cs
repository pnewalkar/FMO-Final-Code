using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO
{
    public class LocationDataDTO
    {
        public LocationDataDTO()
        {
            LocationOfferings = new HashSet<LocationOfferingDataDTO>();
            LocationRelationships = new HashSet<LocationRelationshipDataDTO>();
            LocationRelationships1 = new HashSet<LocationRelationshipDataDTO>();
        }

        public Guid ID { get; set; }

        public int AlternateID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public DbGeometry Shape { get; set; }

        public virtual ICollection<LocationOfferingDataDTO> LocationOfferings { get; set; }

        public virtual ICollection<LocationRelationshipDataDTO> LocationRelationships { get; set; }

        public virtual ICollection<LocationRelationshipDataDTO> LocationRelationships1 { get; set; }

        public virtual NetworkNodeDataDTO NetworkNode { get; set; }
    }
}