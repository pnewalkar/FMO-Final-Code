using System;
using System.Data.Entity.Spatial;

namespace Fmo.DTO
{
    /// <summary>
    /// This class represents data transfer object for AccessLink entity
    /// </summary>
    public class AccessLinkDTO
    {
        public DbGeometry OperationalObjectPoint { get; set; }

        public DbGeometry NetworkIntersectionPoint { get; set; }

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