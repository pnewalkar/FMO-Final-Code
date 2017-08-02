using Newtonsoft.Json;
using RM.CommonLibrary.HelperMiddleware;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using RM.CommonLibrary.Utilities.Geometry;

namespace RM.CommonLibrary.EntityFramework.DTO
{
    public class UnitLocationDTO
    {
        public string ExternalId { get; set; }

        public string UnitName { get; set; }

        public int UnitAddressUDPRN { get; set; }

        public string Area { get; set; }

        public Guid ID { get; set; }

        [JsonConverter(typeof(DbGeometryGeoJsonConverter))]
        public DbGeometry UnitBoundryPolygon { get; set; }

        [NotMapped]
        public List<double> BoundingBox { get; set; }

        [NotMapped]
        public List<double> BoundingBoxCenter { get; set; }
    }
}