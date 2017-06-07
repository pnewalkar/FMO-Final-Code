using System;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DTO
{
    /// <summary>
    /// This class represents data transfer object for AccessLink entity
    /// </summary>
    public class AccessLinkDTO
    {
        [JsonConverter(typeof(DbGeometryConverter))]
        public DbGeometry OperationalObjectPoint { get; set; }

        [JsonConverter(typeof(DbGeometryConverter))]
        public DbGeometry NetworkIntersectionPoint { get; set; }

        [JsonConverter(typeof(DbGeometryConverter))]
        public DbGeometry AccessLinkLine { get; set; }

        public bool? Approved { get; set; }

        public decimal ActualLengthMeter { get; set; }

        public decimal WorkloadLengthMeter { get; set; }

        public Guid ID { get; set; }

        public Guid? LinkStatus_GUID { get; set; }

        public Guid? AccessLinkType_GUID { get; set; }

        public Guid? LinkDirection_GUID { get; set; }

        public Guid? OperationalObject_GUID { get; set; }

        public Guid? OperationalObjectType_GUID { get; set; }

        public Guid NetworkLink_GUID { get; set; }
    }
}