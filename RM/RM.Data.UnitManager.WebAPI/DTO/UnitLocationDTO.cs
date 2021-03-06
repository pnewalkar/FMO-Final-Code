﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;
using RM.CommonLibrary.Utilities.Geometry;

namespace RM.DataManagement.UnitManager.WebAPI.DTO
{
    public class UnitLocationDTO
    {
        public string UnitName { get; set; }

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