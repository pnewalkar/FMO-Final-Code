using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;
using RM.CommonLibrary.Utilities.Geometry;

namespace RM.DataManagement.UnitManager.WebAPI.DataDTO
{
    public class UnitLocationDataDTO
    {
        public string Name { get; set; }

        public string Area { get; set; }

        public Guid LocationId { get; set; }

        [JsonConverter(typeof(DbGeometryGeoJsonConverter))]
        public DbGeometry Shape { get; set; }
    }
}