using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Fmo.DTO
{
    public class UnitLocationDTO
    {
        public int DeliveryUnit_Id { get; set; }

        public string ExternalId { get; set; }

        public string UnitName { get; set; }

        public int UnitAddressUDPRN { get; set; }

        public Guid ID { get; set; }

        public DbGeometry UnitBoundryPolygon { get; set; }

        [NotMapped]
        public string UnitBoundaryGeoJSONData { get; set; }

        [NotMapped]
        public List<double> BoundingBox { get; set; }

        [NotMapped]
        public List<double> BoundingBoxCenter { get; set; }
    }
}